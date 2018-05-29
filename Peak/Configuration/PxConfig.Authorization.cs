using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {
  public class PxAuthorizationConfig {

    public PxAuthorizationConfig() {
      Enabled = true;
    }

    /// <summary>
    /// Yetki kontrolünün yapılıp yapılmayacağını belirtir.
    /// </summary>
    public bool Enabled { get; set; }
  }
}
