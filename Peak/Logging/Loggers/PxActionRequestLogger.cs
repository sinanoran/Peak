using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common.Enums;
using Peak.Session;
using Microsoft.Extensions.Logging;

namespace Peak.Logging.Loggers {
  public class PxActionRequestLogger {

    #region Private Member(s)

    private static ILogger logger = PxLoggerFactory.Instance.CreateLogger("ActionRequestLogger");

    #endregion

    public static void Log(PxLogRequestInfo info) {
      Task.Run(() =>
      {
        log4net.ThreadContext.Properties["Header"] = info.Header;
        log4net.ThreadContext.Properties["MethodType"] = info.MethodType;
        log4net.ThreadContext.Properties["RequestId"] = info.RequestId;
        log4net.ThreadContext.Properties["RequestType"] = (int)info.RequestType;
        log4net.ThreadContext.Properties["Url"] = info.Url;
        log4net.ThreadContext.Properties["Ip"] = info.Ip;
        log4net.ThreadContext.Properties["UserId"] = info.UserId;
        log4net.ThreadContext.Properties["SessionKey"] = info.SessionKey;
        logger.LogInformation(info.Message);
      });
    }
  }

  public class PxLogRequestInfo {

    public PxLogRequestInfo() {
      Message = string.Empty;
    }
    public string RequestId { get; set; }
    public string Message { get; set; }
    public string MethodType { get; set; }
    public string Url { get; set; }
    public RequestType RequestType { get; set; }
    public string Header { get; set; }
    public int UserId { get; set; }
    public string SessionKey { get; set; }
    public string Ip { get; set; }
  }
}
