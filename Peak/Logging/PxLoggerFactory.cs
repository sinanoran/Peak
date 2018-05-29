using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common;
using Microsoft.Extensions.Logging;

namespace Peak.Logging {

  /// <summary>
  /// PxLogger sınıfı oluşturmak için kullanılmaktadır.
  /// </summary>
  public class PxLoggerFactory : SingletonBase<PxLoggerFactory> {

    #region Private Member(s)

    private ILoggerFactory _loggerFactory;

    #endregion

    #region Constructor(s)

    public PxLoggerFactory() {
      _loggerFactory = new LoggerFactory();
      _loggerFactory.AddPxLogger(true);
    }

    #endregion

    #region Public Method(s)

    public ILogger CreateLogger(string categoryName) {
     return _loggerFactory.CreateLogger(categoryName);      
    }

    public ILogger<T> CreateLogger<T>() {
      return _loggerFactory.CreateLogger<T>();
    }

    #endregion
  }
}
