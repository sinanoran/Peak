using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// Configurasyon dosyasında bulunan PxConfig tagının deserialize halidir.
  /// </summary>
  public class PxConfig {

    /// <summary>
    /// 
    /// </summary>
    public PxConfig() {
      this.Authentication = new PxAuthenticationConfig();
      this.Authorization = new PxAuthorizationConfig();
      this.Session = new PxSessionConfig();
      this.Localization = new PxLocalizationConfig();
      this.Schedule = new PxScheduleConfig();
      this.Logging = new PxLoggingConfig();
      this.Dal = new PxDalConfig();
      this.UseCompression = true;
    }
    public PxAuthenticationConfig Authentication { get; set; }
    public PxAuthorizationConfig Authorization { get; set; }
    public PxSessionConfig Session { get; set; }
    public PxLocalizationConfig Localization { get; set; }
    public PxScheduleConfig Schedule { get; set; }
    public PxLoggingConfig Logging { get; set; }
    public PxDalConfig Dal { get; set; }
    /// <summary>
    /// Tatil Günlerini Getiren sınıfın assembly qualified name'i IHolidaysProvider'dan türemelidir.
    /// </summary>
    public string HolidaysProviderAssemblyQualifiedName { get; set; }
    public bool UseCompression { get; set; }
  }
}
