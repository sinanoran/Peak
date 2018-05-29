using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Peak.Web.Extensions {
  public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseCompression(this IApplicationBuilder builder) {
      return builder.UseMiddleware<PxCompressionMiddleware>();
    }
  }
}
