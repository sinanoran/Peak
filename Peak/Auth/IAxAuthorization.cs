using Peak.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth {
  public interface IPxAuthorization {
    PxAuthorizationInfo Authorize(int pUserId);
  }
}
