using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace Peak.Logging {

  /// <summary>
  /// 
  /// </summary>
  public static partial class PxLoggerExtensions {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="configure"></param>
    public static void AddPxLogger(this ILoggerFactory loggerFactory, bool configure) {
      loggerFactory.AddProvider(new PxLoggerProvider());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    public static void LogError(this ILogger logger, string message, Exception ex) {
      logger.LogError(new EventId(), ex, message ?? ex.Message);
    }

    public static void LogError(this ILogger logger, Exception ex) {
      logger.LogError(new EventId(), ex, ex.Message);
    }

  }
}
