using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// 
  /// </summary>
  public class PxMultiFAConfig {

    private const string PARAMETER_GROUP_CODE = "MFA";

    /// <summary>
    /// 
    /// </summary>
    public PxMultiFAConfig() {
      Enabled = false;
      CodeValidDuration = 3;
    }
    /// <summary>
    /// Çift faktör doğrulama işleminin aktifliğini gösterir. Çift Faktör doğrulama (SMS OTP, Time base OTP vs.)
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Çift faktör doğrulama kullanılan generic json parameter
    /// </summary>
    public string Parameter { get; set; }

    /// <summary>
    /// Gönderilen kodun geçerlilik süresi dakika cinsinden
    /// </summary>
    public int CodeValidDuration { get; set; }

    /// <summary>
    /// Gönderilecek mesaj. Parametre tablosundaki MFA Grup Kodlu, MFAMessage adlı alanı göstermektedir.
    /// Örnek: Tek kullanımlık şifreniz : {0}
    /// </summary>
    public string Message { get; set; }
  }
}
