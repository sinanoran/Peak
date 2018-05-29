using Peak.Auth.Models;
using Peak.Common.Enums;
using Peak.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth {
  /// <summary>
  /// Authentication ve Authorization işlemlerini yardımcı olan sınıftır.
  /// Dışarıdan IPxAuthentication ve IPxAuthorization implementasyonlarını alır ve bu implementasyonları kullanır.
  /// </summary>
  public class PxAuthHelper {

    #region Private Member(s)

    private readonly IPxAuthentication _authentication;
    private readonly IPxAuthorization _authorization;

    #endregion

    #region Constructor(s)
    public PxAuthHelper(IPxAuthentication authentication, IPxAuthorization authorization) {
      this._authentication = authentication;
      this._authorization = authorization;
    }

    /// <summary>
    /// Default implementasyon olan PxAuthentication ve PxAuthorization sınıflarını kullanır.
    /// </summary>
    public PxAuthHelper() {
      this._authentication = new PxAuthentication();
      this._authorization = new PxAuthorization();
    }

    #endregion

    #region Public Property(s)

    public IPxAuthentication Authentication {
      get {
        return _authentication;
      }
    }

    public IPxAuthorization Authorization {
      get {
        return _authorization;
      }
    }

    #endregion

    #region Public Method(s)
    public void Login(string pUserName, string pPassword, bool pMFAEnabled) {
      /* MFA Enabled durumda Login işlemi sırasında sadece Authentication yapılır, authorization yapılmaz
       * MFA disabled ise Login işlemi sırasnda hem Authentication hem Authorization gerçekleştirilir */
      try {
        PxSession session = PxSession.Current;
        PxPrincipalInfo principalInfo;

        PxCredentialInfo credentials = new PxCredentialInfo();

        //TODO: pUserName ve pPassword geldiği haliyle kullanılmamalı! Validate edilmeli
        credentials.UserName = pUserName;
        credentials.Password = pPassword;

        principalInfo = this.Authentication.Login(credentials);

        if (!pMFAEnabled) {
          principalInfo.Authorization = this.Authorization.Authorize(principalInfo.UserId);
        }

        session.Principal = principalInfo;
        session.Save();
      }
      catch(Exception ex) {
        Logging.Loggers.PxErrorLogger.Log(ex);
        throw;
      }
    }
    public void Logout() {
      PxSession session = PxSession.Current;
      session.Abandon();
      session.Save();
    }    
    #endregion
  }
}
