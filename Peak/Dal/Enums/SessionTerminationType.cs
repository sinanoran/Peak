using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Dal.Enums
{
  public enum SessionTerminationType {

    /// <summary>
    /// 
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// 
    /// </summary>
    Logout = 1,   

    /// <summary>
    /// 
    /// </summary>
    Expire = 2,

    /// <summary>
    /// 
    /// </summary>
    Kill = 3
  }
}
