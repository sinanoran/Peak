using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peak.Logging.Loggers {
  public class PxActionResponseLogger {

    #region Private Member(s)

    private static ILogger logger = PxLoggerFactory.Instance.CreateLogger("ActionResponseLogger");

    #endregion

    public static void Log(PxLogResponseInfo info) {
      Task.Run(() =>
      {
        log4net.ThreadContext.Properties["Header"] = info.Header;
        log4net.ThreadContext.Properties["RequestId"] = info.RequestId;
        log4net.ThreadContext.Properties["ResponseCode"] = info.ResponseCode;
        log4net.ThreadContext.Properties["UserId"] = info.UserId;
        log4net.ThreadContext.Properties["SessionKey"] = info.SessionKey;
        logger.LogInformation(info.Message);
      });
    }
  }

  public class PxLogResponseInfo {

    public PxLogResponseInfo() {
      Message = string.Empty;
    }
    public string RequestId { get; set; }
    public string Message { get; set; }
    public string ResponseCode { get; set; }
    public string Header { get; set; }
    public int UserId { get; set; }
    public string SessionKey { get; set; }
  }
}
