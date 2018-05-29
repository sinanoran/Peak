using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth {

  /// <summary>
  /// Çoklu doğrulama için FES Parametrelerini içerir. FES -> Bankanın sms ve mail atma uygulaması
  /// </summary>
  public class FesMultiFAParameter : PxMultiFAParameter {

    /// <summary>
    /// 
    /// </summary>
    public string FesUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FesUserPassword { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FesServiceId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FesEnvironment { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FesProjectId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FesServiceUrl { get; set; }
  }
}
