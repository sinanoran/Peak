using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// Kullanıcıların parolaları ile ilgili kavramları içerir.
  /// </summary>
  public class PxPasswordPolicyConfig {

    /// <summary>
    /// 
    /// </summary>
    public PxPasswordPolicyConfig() {
      MaxFailedMFAAttemptCount = 6;
    }

    /// <summary>
    /// Kullanıcı yeni şifresini belirlerken en son kaç eski şifresi ile aynı olamayacak
    /// </summary>
    public int LastUsedPasswordsRestriction { get; set; }

    /// <summary>
    /// Eski bir şifrenin tekrar kullanılabilmesi için geçmesi gereken minumum gün sayısı.
    /// </summary>
    public int OldPasswordReusabilityPeriod { get; set; }

    /// <summary>
    /// Kullanıcı kaç tane hatalı şifre girişinden sonra kilitlenecek.
    /// </summary>
    public int IncorrectPasswordCount { get; set; }

    /// <summary>
    /// Şifre yaratıldıktan kaç gün sonra değiştirmek zorunlu olacak.
    /// </summary>
    public int PasswordChangeDuration { get; set; }

    /// <summary>
    /// Şifre yaratıldıktan sonra, kaç gün değiştirilemyeceğini gösterir
    /// </summary>
    public int PasswordChangeMinPeriod { get; set; }

    /// <summary>
    /// Maximum MFA Deneme Sayısı
    /// </summary>
    public int MaxFailedMFAAttemptCount { get; set; }

    /// <summary>
    /// Belirlenen şifrenin gerekli şartları sağlayığ sağlamadığını kontrol etmek için kullanılır.
    /// </summary>
    public string Regex { get; set; }
  }
}
