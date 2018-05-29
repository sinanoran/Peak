using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Peak.Auth.Error;
using Peak.Auth.Models;
using Peak.Common;
using Peak.Common.Enums;
using Peak.Configuration;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Session;

namespace Peak.Auth {
  public class PxPasswordHelper : SingletonBase<PxPasswordHelper> {
    public void ChangeOwnPassword(PxPasswordChangeInfo info) {
      if (PxSession.Current.Principal.UserId != info.UserId) {
        throw AuthExceptions.TryToChangeOrhersPassword();
      }

      if (string.IsNullOrEmpty(info.New) || !Regex.IsMatch(info.New, PxConfigurationManager.PxConfig.Authentication.Policy.Regex)) {
        throw AuthExceptions.PasswordRegexNotMatch();
      }
      using (PeakDbContext dbContext = new PeakDbContext()) {
        User usr = dbContext.Users.FirstOrDefault(x => x.Id == info.UserId);
        if (usr == null) {
          throw AuthExceptions.InvalidUserNameOrPassword();
        }
        string encryptedOldPassword = Toolkit.Instance.Security.GetSecureHash(info.Old, usr.PasswordSalt, Encoding.UTF8, HashFormat.Base64);
        string encryptedNewPassword = Toolkit.Instance.Security.GetSecureHash(info.New, usr.PasswordSalt, Encoding.UTF8, HashFormat.Base64);
        if (encryptedOldPassword != usr.Password) {
          throw AuthExceptions.InvalidUserNameOrPassword();
        }
        int passwordHistoryCheckFlag = dbContext.PasswordHistories.Where(x => x.UserId == info.UserId).OrderByDescending(x => x.Date).Take(PxConfigurationManager.PxConfig.Authentication.Policy.LastUsedPasswordsRestriction)
                                                                .Union(dbContext.PasswordHistories.Where(y => y.UserId == info.UserId && y.Date > (DateTime.Now.AddDays(-PxConfigurationManager.PxConfig.Authentication.Policy.OldPasswordReusabilityPeriod))))
                                                                .Where(z => z.Password == encryptedNewPassword).Count();
        if (passwordHistoryCheckFlag > 0) {
          throw AuthExceptions.LastUsedPasswords(PxConfigurationManager.PxConfig.Authentication.Policy.LastUsedPasswordsRestriction, PxConfigurationManager.PxConfig.Authentication.Policy.OldPasswordReusabilityPeriod);
        }
        usr.IsPwdMustChange = false;
        usr.PasswordChangeDate = DateTime.Today;
        usr.Password = encryptedNewPassword;
        PxSession session = PxSession.Current;
        session.Principal.Authentication.IsPasswordMustChanged = false;
        session.Save();
        PasswordHistory history = new PasswordHistory();
        history.Date = DateTime.Now;
        history.Password = encryptedNewPassword;
        history.UserId = info.UserId;
        using (TransactionScope trn = new TransactionScope()) {
          dbContext.SaveChanges();
          trn.Complete();
        }
      }
    }
  }
}
