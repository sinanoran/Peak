using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Web.Attributes {

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class PxActionLogAttribute : Attribute {

    #region Private Member(s)

    private ActionLogSetting logSetting = ActionLogSetting.TraceWithRequestData;

    #endregion

    #region Contrustur(s)

    public PxActionLogAttribute() {
    }

    public PxActionLogAttribute(ActionLogSetting logSetting) {
      this.logSetting = logSetting;
    }


    #endregion

    #region Public Property(s)

    public ActionLogSetting LogSetting {
      get {
        return logSetting;
      }
    }

    #endregion
  }

  /// <summary>
  /// Action Loglarının nasıl tutulacağını belirtmek için kullanılır.
  /// </summary>
  public enum ActionLogSetting {
    /// <summary>
    /// Request ve Response ile ilgili hiçbirşey loglanmaz
    /// </summary>
    NoLog = 0,
    /// <summary>
    /// Gelen giden veri dışında herşey loglanır. Örnek : IP, Url, HttpMethod vs.
    /// </summary>
    OnlyTrace = 1,
    /// <summary>
    /// Sadece Gelen veri loglanır. Giden veri için trace loglaması yapılır. (Default davranıştır.)
    /// </summary>
    TraceWithRequestData = 2,
    /// <summary>
    /// Sadece Giden veri loglanır. Gelen veri için trace loglaması yapılır.
    /// </summary>
    TraceWithResponseData = 3,
    /// <summary>
    /// Gelen giden veri herşeyi ile loglanır.
    /// </summary>
    TraceWithAllData = 4
  }
}
