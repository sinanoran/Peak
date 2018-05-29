using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Configuration
{
  public class PxDalConfig {
    public PxDalConfig() {
      PeakDbSchema = "AX";
    }

    public string PeakDbSchema { get; set; }
  }

}
