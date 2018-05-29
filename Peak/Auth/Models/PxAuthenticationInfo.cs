using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Peak.Auth.Models {

  /// <summary>
  /// 
  /// </summary>
  public class PxAuthenticationInfo: IIdentity {
    /// <summary>
    /// 
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime ExpireDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsPasswordMustChanged { get; set; }

    #region MFA

    /// <summary>
    /// 
    /// </summary>
    public bool IsMFAAuthenticationCompleted { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string MFAReferenceCode { get; set; }

    public string Name {
      get;
      set;
    }

    public string AuthenticationType {
      get { return "Custom"; }
    }

    public void Clear() {
      this.ExpireDate = DateTime.Now;
      this.IsAuthenticated = false;
      this.IsPasswordMustChanged = false;
      this.IsMFAAuthenticationCompleted = false;
      this.MFAReferenceCode = string.Empty;
      this.Name = null;
    }

    #endregion
  }
}
