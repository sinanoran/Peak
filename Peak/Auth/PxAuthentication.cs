using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Auth.Error;
using Peak.Common;
using Peak.Common.Enums;
using Peak.Configuration;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Dal.Enums;
using Peak.Auth.Models;
using Microsoft.Extensions.Logging;
using System.Transactions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using MFAWebServicesProxy;
using System.Globalization;
using Peak.ErrorHandling;
using Peak.Session;
using Peak.Logging;
using Peak.Logging.Loggers;

namespace Peak.Auth {

  /// <summary>
  /// IPxAuthentication Peak default implementasyondur. MFA için FES (Bankanın mail ve sms yapısı) kullanılmaktadır.
  /// </summary>
  public class PxAuthentication : IPxAuthentication {

    #region Private Method(s)

    /// <summary>
    /// MFA için kullanılan referans numarası 
    /// </summary>
    /// <returns></returns>
    private string generateReferenceNo(int userId) {
      string reference_format = "{0}{1}{2}{3}{4}{5}{6}{7}{8}";
      DateTime dt = DateTime.Now;
      string refNo = string.Format(reference_format, "AX", dt.ToString("yy"), dt.ToString("MM"), dt.ToString("dd"), dt.ToString("HH"), dt.ToString("mm"), dt.ToString("ss"), dt.ToString("fff"), userId);
      return refNo;
    }

    /// <summary>
    /// FES için bankanın kullandığı F_ENCRYPTED_DATA fonksiyonunu çağıran encryption metodu.
    /// </summary>
    /// <param name="verificationCode"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private string encryptVerificationCode(string verificationCode, string phoneNumber) {
      string result = null;
      using (PeakDbContext dbContext = new PeakDbContext()) {
        string sql = "select F_ENCRYPTED_DATA(:verificationCode,:phoneNumber) from dual";
        Oracle.ManagedDataAccess.Client.OracleParameter pVerificationCode = new Oracle.ManagedDataAccess.Client.OracleParameter("verificationCode", verificationCode);
        Oracle.ManagedDataAccess.Client.OracleParameter pPhoneNumber = new Oracle.ManagedDataAccess.Client.OracleParameter("phoneNumber", phoneNumber);
        result = dbContext.Database.SqlQuery<string>(sql, pVerificationCode, pPhoneNumber).FirstOrDefault();
      }
      return result;
    }

    /// <summary>
    /// FES'in kabul etmiş olduğu Xml'i hazırlar.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="projectId"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private string prepareInputXmlForFes(string message, string projectId, string phoneNumber) {
      StringBuilder sb = new StringBuilder();
      sb.Append("<FESXML>");
      sb.Append("<SETTINGS>");
      sb.Append("</SETTINGS>");
      sb.Append("<FESINFO>");
      sb.Append("<PFESTANIMNO>");
      sb.Append(projectId);
      sb.Append("</PFESTANIMNO>");
      sb.Append("<PPRM>TEXT=");
      sb.Append(message);
      sb.Append("</PPRM>");
      sb.Append("<PBAS_TAR>");
      sb.Append(DateTime.Today.ToString("ddMMyy", CultureInfo.InvariantCulture));
      sb.Append("</PBAS_TAR>");
      sb.Append("<PBIT_TAR>");
      sb.Append(DateTime.Today.ToString("ddMMyy", CultureInfo.InvariantCulture));
      sb.Append("</PBIT_TAR>");
      sb.Append("<PBAS_SAAT/>");
      sb.Append("<PBIT_SAAT/>");
      sb.Append("<PADR_TO>");
      sb.Append(phoneNumber);
      sb.Append("</PADR_TO>");
      sb.Append("<PDETAYISLE_EH>");
      sb.Append("H");
      sb.Append("</PDETAYISLE_EH>");
      sb.Append("<PILETITIPNO>");
      sb.Append("</PILETITIPNO>");
      sb.Append("<PDRM>");
      sb.Append("G");
      sb.Append("</PDRM>");
      sb.Append("</FESINFO>");
      sb.Append("</FESXML>");
      return sb.ToString();
    }


    #endregion

    #region IPxAuthentication Member(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="credential"></param>
    public PxPrincipalInfo Login(PxCredentialInfo credential) {
      User user = null;
      PxPrincipalInfo info = null;
      PxSession axSession = PxSession.Get();
      try {
        using (TransactionScope scope = new TransactionScope()) {
          using (PeakDbContext dbContext = new PeakDbContext()) {
            user = dbContext.Users.FirstOrDefault(x => x.Code == credential.UserName);
            if (user == null) {
              throw AuthExceptions.InvalidUserNameOrPassword();
            }
            if (user.PasswordState == PasswordState.Blocked) {
              throw AuthExceptions.UserHasBeenLocked();
            }
            string pwd = Toolkit.Instance.Security.GetSecureHash(credential.Password, user.PasswordSalt, Encoding.UTF8, HashFormat.Base64);
            if (user.Password != pwd) {
              user.PasswordTryCount++;

              if (user.PasswordTryCount > PxConfigurationManager.PxConfig.Authentication.Policy.IncorrectPasswordCount) {
                user.PasswordState = PasswordState.Blocked;
              }
              dbContext.SaveChanges();
              if (user.PasswordState == PasswordState.Blocked) {
                throw AuthExceptions.UserHasBeenLocked();
              }
              throw AuthExceptions.InvalidUserNameOrPassword();
            }


            if (user.CancelDate.HasValue && user.CancelDate <= DateTime.Now) {
              throw AuthExceptions.InvalidUserNameOrPassword();
            }

            // kullanıcı henüz aktive edilmediyse hata at.
            if (user.StartDate > DateTime.Now) {
              throw AuthExceptions.UserActivationIsNotStartedYet();
            }

            // kullanıcı aktivasyonu sona ermişse hata at.
            if (user.EndDate.HasValue && user.EndDate < DateTime.Now) {
              throw AuthExceptions.UserActivationIsEnded();
            }
            string sessionKey = Toolkit.Instance.CreateUniqueId();

            ActiveSession activeSession = new ActiveSession()
            {
              Ip = axSession.Client.IPAddress,
              OpenDate = DateTime.Now,
              SessionKey = sessionKey,
              BrowserUserAgent = axSession.Client.BrowserUserAgent,
              UserId = user.Id
            };

            dbContext.ActiveSessions.Add(activeSession);
            user.PasswordTryCount = 0;

            dbContext.SaveChanges();

            info = new PxPrincipalInfo();
            info.Authentication.ExpireDate = DateTime.Now.AddMinutes(PxConfigurationManager.PxConfig.Session.DefaultExpireDuration);
            info.Authentication.Token = sessionKey;
            info.Authentication.Timeout = PxConfigurationManager.PxConfig.Session.DefaultTimeoutDuration;
            if (!PxConfigurationManager.PxConfig.Authentication.MultiFA.Enabled) {
              info.Authentication.IsAuthenticated = true;
            }
            info.Authentication.IsPasswordMustChanged = user.IsPwdMustChange;
            info.Authentication.Name = user.Name;

            info.CultureCode = user.CultureCode;
            info.UserName = user.Code;
            info.EmailAddress = user.Email;
            info.UserId = user.Id;
            info.ProfileImage = user.Image;
            info.MiddleName = user.MiddleName;
            info.Name = user.Name;
            info.PhoneNumber = user.PhoneNumber;
            info.Surname = user.Surname;
          }

          scope.Complete();
        }

        axSession.Principal = info;
        this.authorizeAndGetMenu(axSession); //Authorization verisi ve ana menu bilgileri alınıyor
        PxSession.Save(axSession);

        return info;
      }
      catch {
        //Başarısız oturum denemeleri tablosuna kayıt atılıyor.
        using (PeakDbContext dbContext = new PeakDbContext()) {
          UnsuccessfulSession unSuccesfulSession = new UnsuccessfulSession()
          {
            UserId = user != null ? user.Id : 0,
            Ip = axSession.Client.IPAddress,
            Date = DateTime.Now,
            BrowserUserAgent = axSession.Client.BrowserUserAgent
          };
          dbContext.UnsuccessfulSessions.Add(unSuccesfulSession);
          dbContext.SaveChanges();
        }
        throw;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Logout() {
      using (TransactionScope trn = new TransactionScope()) {
        using (PeakDbContext dbContext = new PeakDbContext()) {
          ActiveSession activeSession = dbContext.ActiveSessions.FirstOrDefault(x => x.SessionKey == PxSession.Current.Principal.Authentication.Token); 
          TerminatedSession sessionToTerminate = new TerminatedSession(activeSession);
          sessionToTerminate.TerminationType = SessionTerminationType.Logout;
          sessionToTerminate.TerminationDate = DateTime.Now;
          dbContext.ActiveSessions.Remove(activeSession);
          dbContext.TerminatedSessions.Add(sessionToTerminate);
          dbContext.SaveChanges();
        }
        trn.Complete();
      }
    }

    /// <summary>
    /// Çoklu doğrulama için gönderilecek mesajdır. Msg'da string format için {0} ifadesi yer almalıdır.
    /// </summary>
    /// <param name="principleInfo"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public void SendMFACode(PxPrincipalInfo principleInfo, string msg) {
      FesMultiFAParameter parameter = JsonConvert.DeserializeObject<FesMultiFAParameter>(PxConfigurationManager.PxConfig.Authentication.MultiFA.Parameter);
      if (parameter == null) {
        throw AuthExceptions.MFAParameterNotFound();
      }
      if (principleInfo == null) {
        throw AuthExceptions.PrincipleInfoNotFound();
      }
      if (string.IsNullOrEmpty(msg)) {
        msg = PxConfigurationManager.PxConfig.Authentication.MultiFA.Message;
      }
      string refNo = generateReferenceNo(principleInfo.UserId);
      string verificationCode = Toolkit.Instance.GenerateRandomNumber(6).ToString();
      string encryptedVerificationCode = encryptVerificationCode(verificationCode, principleInfo.PhoneNumber);
      string message = string.Format(msg, string.Format("#{0}#", encryptedVerificationCode));
      MFAWebServiceResult result = null;
      using (MFAWebServicesClient svcClient = new MFAWebServicesClient(MFAWebServicesClient.EndpointConfiguration.MFAWebServicesSoapHttpPort, new System.ServiceModel.EndpointAddress(parameter.FesServiceUrl))) {
        result = svcClient.MFAWebSrvAsync(parameter.FesUser, parameter.FesUserPassword, parameter.FesServiceId, parameter.FesEnvironment, prepareInputXmlForFes(message, parameter.FesProjectId, principleInfo.PhoneNumber)).Result;
      }
      using (PeakDbContext dbContext = new PeakDbContext()) {
        MFAMessage mfa = new MFAMessage()
        {
          Date = DateTime.Now,
          IsUsed = false,
          PhoneNumber = principleInfo.PhoneNumber,
          UserId = principleInfo.UserId,
          RereferenceCode = refNo,
          VerificationCode = encryptedVerificationCode
        };
        dbContext.MFAMessages.Add(mfa);
        dbContext.SaveChanges();
      }

      if (result.errorCode != "0") {
        throw new PxUnexpectedErrorException(new Exception(result.errorMsg));
      }
      principleInfo.Authentication.MFAReferenceCode = refNo;
      PxSession session = PxSession.Get();
      session.Principal = principleInfo;
      PxSession.Save(session);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="principleInfo"></param>
    /// <param name="verificationCode"></param>
    public void CheckMFACode(PxPrincipalInfo principleInfo, string verificationCode) {
      using (PeakDbContext dbContext = new PeakDbContext()) {
        MFAMessage mfa = dbContext.MFAMessages.FirstOrDefault(x => x.RereferenceCode == principleInfo.Authentication.MFAReferenceCode && x.UserId == principleInfo.UserId && !x.IsUsed);
        if (mfa == null) {
          throw AuthExceptions.InvalidMFAReferenceNo();
        }
        User usr = dbContext.Users.FirstOrDefault(x => x.Id == principleInfo.UserId);
        if (usr.PasswordState == PasswordState.Blocked) {
          throw AuthExceptions.MFAUserBlocked();
        }

        if (DateTime.Now > mfa.Date.AddMinutes(PxConfigurationManager.PxConfig.Authentication.MultiFA.CodeValidDuration)) {
          throw AuthExceptions.MFACodeExpired();
        }

        string encryptedVerificationCode = encryptVerificationCode(verificationCode, principleInfo.PhoneNumber);
        if (!string.Equals(encryptedVerificationCode, mfa.VerificationCode)) {
          usr.MFATryCount++;
          if (usr.MFATryCount >= PxConfigurationManager.PxConfig.Authentication.Policy.MaxFailedMFAAttemptCount) {
            usr.MFATryCount = 0;
            usr.PasswordState = PasswordState.Blocked;
            dbContext.SaveChanges();
            throw AuthExceptions.MFAUserBlocked();
          }
          dbContext.SaveChanges();
          throw AuthExceptions.MFAAuthenticationFailed();
        }
        usr.MFATryCount = 0;
        dbContext.SaveChanges();
      }
      principleInfo.Authentication.IsMFAAuthenticationCompleted = true;
      PxSession session = PxSession.Get();
      session.Principal = principleInfo;
      PxSession.Save(session);
    }

    private void authorizeAndGetMenu(PxSession pSession) {
      if (pSession == null || pSession.Principal == null) { return; }
      PxAuthorization authEngine = new PxAuthorization();
      pSession.Principal.Authorization = authEngine.Authorize(pSession.Principal.UserId);
      pSession.Menu = authEngine.GetMenu(pSession.Principal.UserId);
    }


    #endregion
  }
}
