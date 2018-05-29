using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// 
  /// </summary>
  public class PxScheduleConfig {

    /// <summary>
    /// 
    /// </summary>
    public PxScheduleConfig() {
      RefreshInterval = 60;
      TimeZoneId = "Turkey Standard Time";
      Enabled = true;
    }

    /// <summary>
    /// Scheduleların çalıştırmasını set eder. Default true
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Schedule tanımlarının hangi aralıkla cache yenileceğinin belirtir. Dakika Cinsinden
    /// </summary>
    public int RefreshInterval { get; set; }

    /// <summary>
    /// Schedule'ın hangi time zone ile çalışacağını belirtir. Default "Turkey Standard Time"
    /// </summary>
    public string TimeZoneId { get; set; }
  }
}
