using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {

  /// <summary>
  /// 
  /// </summary>
  public class PxSessionConfig {

    #region Private Member(s)
    
    private int _defaultTimeoutDuration;
    private int _defaultExpireDuration;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public PxSessionConfig() {
      DefaultTimeoutDuration = 5;
      DefaultExpireDuration = 360;
    }


    #endregion

    #region Public Property(s)

    /// <summary>
    /// 
    /// </summary>
    public int DefaultTimeoutDuration{
      get {
        return _defaultTimeoutDuration;
      }

      set {
        _defaultTimeoutDuration = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public int DefaultExpireDuration {
      get {
        return _defaultExpireDuration;
      }

      set {
        _defaultExpireDuration = value;
      }
    }

    #endregion
  }
}
