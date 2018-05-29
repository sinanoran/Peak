using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Hosting;

namespace Peak.Web.Extensions {

  /// <summary>
  /// 
  /// </summary>
  public  static partial class PxLoggerExtensions {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appEnv"></param>
    /// <param name="configFileRelativePath"></param>
    public static void ConfigurePxLog(this IHostingEnvironment appEnv, string configFileRelativePath) {
      GlobalContext.Properties["appRoot"] = appEnv.ContentRootPath;
      XmlConfigurator.Configure(new FileInfo(Path.Combine(appEnv.ContentRootPath, configFileRelativePath)));
    }
  }
}
