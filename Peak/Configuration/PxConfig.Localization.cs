using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {
  /// <summary>
  /// 
  /// </summary>
  public class PxLocalizationConfig {
    /// <summary>
    /// 
    /// </summary>
    public PxLocalizationConfig() {    
    }

    /// <summary>
    /// 
    /// </summary>
    public string[] AvailableCultures { get; set; }

    /// <summary>
    /// Lokalizationının aktif olup olmadığını belirtir.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Default culture code (tr-TR)
    /// </summary>
    public string DefaultCulture { get; set; }
  }
}
