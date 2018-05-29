using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// 
  /// </summary>
  public class PxAuthenticationConfig {

    /// <summary>
    /// 
    /// </summary>
    public PxAuthenticationConfig() {
      Policy = new PxPasswordPolicyConfig();
      MultiFA = new PxMultiFAConfig();
    }

    /// <summary>
    /// Kullanıcıların parolaları ile ilgili kavramları içerir.
    /// </summary>
    public PxPasswordPolicyConfig Policy { get; set; }

    /// <summary>
    /// Birden fazla doğrulama ile ilgili konfigurasyon bilgisi
    /// </summary>
    public PxMultiFAConfig MultiFA { get; set; }
  }
}
