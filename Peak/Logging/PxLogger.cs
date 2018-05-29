using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.ErrorHandling;
using Peak.Session;
using log4net;
using Microsoft.Extensions.Logging;

namespace Peak.Logging {

  /// <summary>
  /// 
  /// </summary>
  public class PxLogger : Microsoft.Extensions.Logging.ILogger {
    private readonly ILog _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerName"></param>
    public PxLogger(string loggerName) {
      _logger = LogManager.GetLogger(loggerName);
    }

    static PxLogger() {
      log4net.Config.XmlConfigurator.Configure();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public bool IsEnabled(LogLevel logLevel) {
      switch (logLevel) {
        case LogLevel.Trace:
        case LogLevel.Debug:
          return _logger.IsDebugEnabled;
        case LogLevel.Information:
          return _logger.IsInfoEnabled;
        case LogLevel.Warning:
          return _logger.IsWarnEnabled;
        case LogLevel.Error:
          return _logger.IsErrorEnabled;
        case LogLevel.Critical:
          return _logger.IsFatalEnabled;
        case LogLevel.None:
          return false;
        default:
          throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable BeginScope<TState>(TState state) {
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
      if (!IsEnabled(logLevel)) {
        return;
      }
      if (formatter == null) {
        throw new ArgumentNullException(nameof(formatter));
      }

      if (!log4net.ThreadContext.Properties.GetKeys().Contains("UserId")) {
        log4net.ThreadContext.Properties["UserId"] = PxSession.Get().Principal.UserId;
      }
      if (!log4net.ThreadContext.Properties.GetKeys().Contains("SessionKey")) {
        log4net.ThreadContext.Properties["SessionKey"] = PxSession.Get().Principal.Authentication.Token;
      }

      var message = formatter(state, exception);

      switch (logLevel) {
        case LogLevel.Trace:
        case LogLevel.Debug:
          _logger.Debug(message, exception);
          break;
        case LogLevel.Information:
          _logger.Info(message, exception);
          break;
        case LogLevel.Warning:
          _logger.Warn(message, exception);
          break;
        case LogLevel.Error:
          _logger.Error(message, exception);
          break;
        case LogLevel.Critical:
          _logger.Fatal(message, exception);
          break;
        case LogLevel.None:
          break;
        default:
          _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
          _logger.Info(message, exception);
          break;
      }
    }

    /// <summary>
    /// log4net.ThreadContext.Properties'ine custom property eklenip, config dosyasında almak için kullanılır.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddContextProperty(string key, object value) {
      log4net.ThreadContext.Properties[key] = value;
    }
  }
}