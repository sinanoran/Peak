using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common.Enums;
using Peak.ErrorHandling;

namespace Peak.Auth.Error {

  /// <summary>
  /// 
  /// </summary>
  public sealed class AuthExceptions {

    /// <summary>
    /// Yalış şifre veya kullanıcı adı girildiğinde atılacak hata.
    /// </summary>
    public static PxException InvalidUserNameOrPassword() {
      return InvalidUserNameOrPassword(null);
    }

    /// <summary>
    /// Yalış şifre veya kullanıcı adı girildiğinde atılacak hata.
    /// </summary>
    public static PxException InvalidUserNameOrPassword(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.InvalidUserNameOrPassword, ErrorPriority.Low, new PxErrorMessageProvider("InvalidUserNameOrPassword"));
      return ex;
    }

    /// <summary>
    /// Kullanıcının şifresini belirli bir sayıda arka arkadaya girmesininden dolayı kitliyken giriş yapmaya çalışırsa atılacak hata.
    /// </summary>
    public static PxException UserHasBeenLocked() {
      return UserHasBeenLocked(null);
    }

    /// <summary>
    /// Kullanıcının şifresini belirli bir sayıda arka arkadaya girmesininden dolayı kitliyken giriş yapmaya çalışırsa atılacak hata.
    /// </summary>
    public static PxException UserHasBeenLocked(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.UserHasBeenLocked, ErrorPriority.Low, new PxErrorMessageProvider("UserHasBeenLocked"));
      return ex;
    }

    /// <summary>
    /// Kullanıma kapatılmış bir kullanıcı ile login olmak istendiğinde atılacak hata
    /// </summary>
    /// <param name = "username">Kullanıma kapatılan kullanıcı adı</param>
    public static PxException UserHasBeenClosed(string username) {
      return UserHasBeenClosed(null, username);
    }

    /// <summary>
    /// Kullanıma kapatılmış bir kullanıcı ile login olmak istendiğinde atılacak hata
    /// </summary>
    /// <param name = "innerEx">inner exception</param>
    /// <param name = "username">Kullanıma kapatılan kullanıcı adı</param>
    public static PxException UserHasBeenClosed(System.Exception innerEx, string username) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.UserHasBeenClosed, ErrorPriority.Low, new PxErrorMessageProvider("UserHasBeenClosed", username));
      return ex;
    }

    /// <summary>
    /// Aktivasyon tarihi henüz gelmemiş bir kullanıcı login olmaya çaılştığında atılacak hata.
    /// </summary>
    public static PxException UserActivationIsNotStartedYet() {
      return UserActivationIsNotStartedYet(null);
    }

    /// <summary>
    /// Aktivasyon tarihi henüz gelmemiş bir kullanıcı login olmaya çaılştığında atılacak hata.
    /// </summary>
    public static PxException UserActivationIsNotStartedYet(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.UserActivationIsNotStartedYet, ErrorPriority.Low, new PxErrorMessageProvider("UserActivationIsNotStartedYet"));
      return ex;
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException UserActivationIsEnded() {
      return UserActivationIsEnded(null);
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException UserActivationIsEnded(System.Exception innerEx) {    
      PxException ex = new PxException(innerEx, AuthErrorCodes.UserActivationIsEnded, ErrorPriority.Low, new PxErrorMessageProvider("UserActivationIsEnded"));
      return ex;
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException MFAParameterNotFound() {
      return MFAParameterNotFound(null);
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException MFAParameterNotFound(System.Exception innerEx) {     
      PxException ex = new PxException(innerEx, AuthErrorCodes.UserActivationIsEnded, ErrorPriority.Low, new PxErrorMessageProvider("MFAParameterNotFound"));
      return ex;
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException PrincipleInfoNotFound() {
      return PrincipleInfoNotFound(null);
    }

    /// <summary>
    /// Aktivasyon tarihi geçmiş bir kullanıcı login olmaya çalıştığında atılacak hata.
    /// </summary>
    public static PxException PrincipleInfoNotFound(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.PrincipleInfoNotFound, ErrorPriority.Low, new PxErrorMessageProvider("PrincipleInfoNotFound"));
      return ex;
    }

    /// <summary>
    /// Sms  servisi tarafından geçersiz kod döndü.
    /// </summary>
    public static PxException InvalidMFAReferenceNo() {
      return InvalidMFAReferenceNo(null);
    }
    /// <summary>
    /// Sms  servisi tarafından geçersiz kod döndü.
    /// </summary>
    public static PxException InvalidMFAReferenceNo(System.Exception innerEx) {     
      PxException ex = new PxException(innerEx, AuthErrorCodes.InvalidMFAReferenceNo, ErrorPriority.Low, new PxErrorMessageProvider("InvalidMFAReferenceNo"));
      return ex;
    }

    /// <summary>
    /// Maksimum hatalı deneme adedine ulaşılması
    /// </summary>
    public static PxException MFAUserBlocked() {
      return MFAUserBlocked(null);
    }
    /// <summary>
    /// Maksimum hatalı deneme adedine ulaşılması
    /// </summary>
    public static PxException MFAUserBlocked(System.Exception innerEx) {
      PxException ex = new PxException(innerEx, AuthErrorCodes.MFAUserBlocked, ErrorPriority.Low, new PxErrorMessageProvider("MFAUserBlocked"));
      return ex;
    }

    /// <summary>
    /// MFA'da gönderilen şifrenin öngörülen sürede kullanılmamış olması
    /// </summary>
    public static PxException MFACodeExpired() {
      return MFACodeExpired(null);
    }
    /// <summary>
    /// MFA'da gönderilen şifrenin öngörülen sürede kullanılmamış olması
    /// </summary>
    public static PxException MFACodeExpired(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.MFACodeExpired, ErrorPriority.Low, new PxErrorMessageProvider("MFACodeExpired"));
      return ex;
    }

    /// <summary>
    /// MFA'da gönderilen şifrenin geçersiz olduğunu gösterir.
    /// </summary>
    public static PxException MFAAuthenticationFailed() {
      return MFAAuthenticationFailed(null);
    }
    /// <summary>
    /// MFA'da gönderilen şifrenin geçersiz olduğunu gösterir.
    /// </summary>
    public static PxException MFAAuthenticationFailed(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.MFAAuthenticationFailed, ErrorPriority.Low, new PxErrorMessageProvider("MFAAuthenticationFailed"));
      return ex;
    }

    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException TryToChangeOrhersPassword() {
      return TryToChangeOrhersPassword(null);
    }
    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException TryToChangeOrhersPassword(System.Exception innerEx) {      
      PxException ex = new PxException(innerEx, AuthErrorCodes.TryToChangeOrhersPassword, ErrorPriority.Low, new PxErrorMessageProvider("TryToChangeOrhersPassword"));
      return ex;
    }

    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException PasswordRegexNotMatch() {
      return PasswordRegexNotMatch(null);
    }
    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException PasswordRegexNotMatch(System.Exception innerEx) {
      PxException ex = new PxException(innerEx, AuthErrorCodes.PasswordRegexNotMatch, ErrorPriority.Low, new PxErrorMessageProvider("PasswordRegexNotMatch"));
      return ex;
    }

    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException LastUsedPasswords(int lastUsedPasswordsCount, int pwdage) {
      return PasswordRegexNotMatch(null);
    }
    /// <summary>
    /// Şifre değiştirmek isteyen kullanıcı login olan kullanıcı değilse atılacak hata.
    /// </summary>
    public static PxException LastUsedPasswords(System.Exception innerEx, int lastUsedPasswordsCount, int pwdage) {
      PxException ex = new PxException(innerEx, AuthErrorCodes.LastUsedPasswords, ErrorPriority.Low, new PxErrorMessageProvider("LastUsedPasswords", lastUsedPasswordsCount, pwdage));
      return ex;
    }
  }
}
