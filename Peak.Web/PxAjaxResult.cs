using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Web {
  public class PxAjaxResult {
    public PxAjaxResult() {
      IsError = false;
      IsFatalError = false;
    }

    public bool IsError { get; set; }
    public bool IsFatalError { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public object Data { get; set; }
  }
}
