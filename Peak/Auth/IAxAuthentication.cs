using Peak.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth {

  /// <summary>
  /// Authentication işlemini yapacak sınıfın türemesi gereken sınıftır. Authentication için gerekli metodları içermektedir.
  /// </summary>
  public interface IPxAuthentication {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="credential"></param>
    /// <returns></returns>
    PxPrincipalInfo Login(PxCredentialInfo credential);

    /// <summary>
    /// 
    /// </summary>
    void Logout();

    /// <summary>
    /// Eğer message null ise parametre tablosundaki MFAMessage alanı okunur.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="msg"></param>
    void SendMFACode(PxPrincipalInfo info, string msg = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="verifyCode"></param>
    void CheckMFACode(PxPrincipalInfo info, string verifyCode);
    
  }
}
