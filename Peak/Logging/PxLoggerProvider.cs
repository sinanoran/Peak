using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peak.Logging
{
  /// <summary>
  /// 
  /// </summary>
  public class PxLoggerProvider : ILoggerProvider {

    private IDictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string name) {
      if (!_loggers.ContainsKey(name)) {
        lock (_loggers) {
          if (!_loggers.ContainsKey(name)) {
            _loggers[name] = new PxLogger(name);
          }
        }
      }
      return _loggers[name];
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose() {
      _loggers.Clear();
      _loggers = null;
    }
  }
}
