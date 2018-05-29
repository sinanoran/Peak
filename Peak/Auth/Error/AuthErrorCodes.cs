using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Error {
  public struct AuthErrorCodes {

    /// <summary>
    /// Yalış şifre veya kullanıcı adı girildiğinde atılacak hata.
    /// </summary>
    public const string InvalidUserNameOrPassword = "101.0001";

    /// <summary>
    /// Kullanıcının şifresini belirli bir sayıda arka arkadaya girmesininden dolayı kitliyken giriş yapmaya çalışırsa atılacak hata.
    /// </summary>
    public const string UserHasBeenLocked = "101.0002";

    /// <summary>
    /// Kullanıma kapatılmış bir kullanıcı ile login olmak istendiğinde atılacak hata
    /// </summary>
    public const string UserHasBeenClosed = "101.0003";

    /// <summary>
    /// Aktivasyon tarihi henüz gelmemiş bir kullanıcı login olmaya çaılştığında atılacak hata.
    /// </summary>
    public const string UserActivationIsNotStartedYet = "101.0005";

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public const string UserActivationIsEnded = "101.0005";

    /// <summary>
    /// Multi Faktör Authenticatin da gerekli parametre konfig dosyasında bulunamadı.
    /// </summary>
    public const string MFAParameterCanNotFind = "101.0006";

    /// <summary>
    /// Multi Faktör Authenticatin da gerekli parametre konfig dosyasında bulunamadı.
    /// </summary>
    public const string PrincipleInfoNotFound = "101.0007";

    /// <summary>
    /// Sms  servisi tarafından geçersiz kod döndü.
    /// </summary>
    public const string InvalidMFAReferenceNo = "101.0008";

    /// <summary>
    /// Maksimum hatalı deneme adedine ulaşılması
    /// </summary>
    public const string MFAUserBlocked = "101.0009";

    /// <summary>
    /// MFA'da gönderilen şifrenin öngörülen sürede kullanılmamış olması
    /// </summary>
    public const string MFACodeExpired = "101.0010";

    /// <summary>
    /// Sms OTP 'de gönderilen şifrenin geçersiz olduğunu gösterir.
    /// </summary>
    public const string MFAAuthenticationFailed = "101.0011";

    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public const string TryToChangeOrhersPassword = "101.0012";

    /// <summary>
    /// Şifre regex patterne uymuyor.
    /// </summary>
    public const string PasswordRegexNotMatch = "101.0013";
    /// <summary>
    /// Şifre regex patterne uymuyor.
    /// </summary>
    public const string LastUsedPasswords = "101.0014";
  }
}
