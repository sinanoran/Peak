using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Auth;
using Peak.Common;
using Peak.Dal;
using Peak.Dal.Entities;

namespace Peak.Test.Console.Auth {
  public class PxAuthenticationTester {

    public static void InitTest() {
      //AddUser();
      //UpdateUser();
      Login();
    }

    public static void Login() {
      PxAuthentication auth = new PxAuthentication();
      var principleInfo = auth.Login(new Peak.Auth.Models.PxCredentialInfo() { UserName = "p18928", Password = "123456" });
      auth.SendMFACode(principleInfo, "Peak için tek kullanimlik sifreniz: {0} sadece 3 dakika için geçerlidir. Güvenliginiz icin sifrenizi kimseyle paylasmayiniz");
      auth.CheckMFACode(principleInfo, "887035");
    }
    public static void AddUser() {
      User user = new User();
      user.Code = "p18928";
      user.CultureCode = "tr-TR";
      user.Email = "sinan.oran@windowslive.com";
      user.Name = "Sinan";
      user.PasswordSalt = Toolkit.Instance.Security.GetSecureSalt();
      user.Password = Toolkit.Instance.Security.GetSecureHash("123456", user.PasswordSalt, Encoding.UTF8, Peak.Common.Enums.HashFormat.Base64);
      user.PhoneNumber = "5555555555";
      user.Surname = "Oran";
      using (PeakDbContext dbContext = new PeakDbContext()) {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
      }
    }

    public static void UpdateUser() {
      using (PeakDbContext dbContext = new PeakDbContext()) {
        User user = dbContext.Users.FirstOrDefault(x => x.Id == 2);
        user.Code = "p12314";
        user.CultureCode = "en-US";
        user.Email = "sinan.oran@yahoo.com";
        user.Name = "Ali";
        user.PasswordSalt = Toolkit.Instance.Security.GetSecureSalt();
        user.Password = Toolkit.Instance.Security.GetSecureHash("1234", user.PasswordSalt, Encoding.UTF8, Peak.Common.Enums.HashFormat.Base64);
        user.PhoneNumber = "5555555555";
        user.Surname = "Veli";
        dbContext.SaveChanges();
      }
    }
  }
}
