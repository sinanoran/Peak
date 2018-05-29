using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Common {
  /// <summary>
  /// Tatil günleri için kullanılan sınıftır.
  /// </summary>
  public class Holiday {

    /// <summary>
    /// 
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsHalfDay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; }
  }
}
