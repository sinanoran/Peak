using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common;
using Peak.ErrorHandling;
using Peak.Session;
using Microsoft.Extensions.Logging;

namespace Peak.Logging.Loggers {
  public class PxErrorLogger {

    #region Private Member(s)

    private static ILogger logger = PxLoggerFactory.Instance.CreateLogger("ErrorLogger");

    #endregion

    public static string Log(Exception ex) {
      return Log(null, ex, null);
    }

    public static string Log(Exception ex, string requestId) {
     return Log(null, ex, requestId);
    }

    public static string Log(string message, Exception ex, string requestId) {
      string errorCode = null;
      if (ex is PxException) {
        errorCode = ((PxException)ex).ErrorCode;
      }
      else {
        errorCode = Toolkit.Instance.CreateUniqueId();
      }
      log4net.ThreadContext.Properties["ErrorCode"] = errorCode;
      log4net.ThreadContext.Properties["RequestId"] = requestId;
      log4net.ThreadContext.Properties["Source"] = ex.Source;
      log4net.ThreadContext.Properties["Ip"] = PxSession.Current.Client.IPAddress;
      logger.LogError(message, ex);
      return errorCode;
    }
  }
}
