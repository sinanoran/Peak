using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Web
{
  public static class PxHttpContext {
    private static Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;
    internal static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor) {
      PxHttpContext.httpContextAccessor = httpContextAccessor;
    }

    public static Microsoft.AspNetCore.Http.HttpContext Current {
      get {
        return httpContextAccessor.HttpContext;
      }
    }

  }  
}
