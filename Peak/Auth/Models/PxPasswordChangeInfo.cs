using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Models {
  public class PxPasswordChangeInfo {
    public string New { get; set; }

    public string Old { get; set; }

    public int UserId { get; set; }
  }
}
