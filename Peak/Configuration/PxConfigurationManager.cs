using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Logging;
using Microsoft.Extensions.Configuration;
using Peak.Common;
using System.Collections.Specialized;

namespace Peak.Configuration {

  /// <summary>
  /// Konfigurasyon dosyasını okuyup, konfigurasyon dosyasında bulunan ifadeleri ilgili sınıflara bind eder.
  /// ConfigurationManager gibi çalışmaktadır.
  /// </summary>
  public static class PxConfigurationManager {

    #region Private Member(s)
    
    private static readonly PxConfig _axConfig = new PxConfig();
    private static readonly IConfiguration _configuration;

    #endregion

    #region Constructor(s)
    static PxConfigurationManager() {
      _configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json")
         .Build();
      var axConfigSection = _configuration.GetSection("PxConfig");
      if (axConfigSection != null) {
        axConfigSection.Bind(_axConfig);
      }    
    }

    #endregion   

    #region Public Property(s)

    /// <summary>
    /// 
    /// </summary>
    public static NameValueCollection AppSettings {
      get {
        return System.Configuration.ConfigurationManager.AppSettings;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static PxConfig PxConfig {
      get {
        return _axConfig;
      }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IConfigurationSection GetSection(string key) {
      return _configuration.GetSection(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetFileValue<T>(string key) {
      return _configuration.GetValue<T>(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetDbValue<T>(string group, string name) {
      return PxDbConfigurationManager.GetValue<T>(group, name);
    }

    #endregion

  }
}
