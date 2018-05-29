using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration {
  public class PxLoggingConfig {
    public PxLoggingConfig() {
      ActionLogEnabled = true;
    }

    public bool ActionLogEnabled { get; set; }
  }
}
