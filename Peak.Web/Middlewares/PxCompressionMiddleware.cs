using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Peak.Web.Middlewares {

  public class PxCompressionMiddleware {

    #region Private Member(s)

    private const string AcceptEncodingName = "Accept-Encoding";
    private const string ContentEncodingName = "Content-Encoding";
    private const string ContentLengthName = "Content-Length";
    private const string GzipEncodingType = "gzip";
    private const string DeflateEncodingType = "deflate";

    private readonly RequestDelegate _next;

    #endregion

    #region Constructor(s)

    public PxCompressionMiddleware(RequestDelegate next) {
      _next = next;
    }

    #endregion

    #region Public Method(s)

    public async Task Invoke(HttpContext context) {
      var acceptEncoding = context.Request.Headers[AcceptEncodingName];
      IEnumerable<string> acceptTypes;

      if (!IsCompressionAllowed(acceptEncoding, out acceptTypes)) {
        await _next(context);
        return;
      }

      using (var buffer = new MemoryStream()) {
        var body = context.Response.Body;
        context.Response.Body = buffer;
        try {
          await _next(context);
          using (var compressed = new MemoryStream()) {
            if (acceptTypes.Contains(GzipEncodingType)) {
              using (var gzip = new GZipStream(compressed, CompressionLevel.Optimal, leaveOpen: true)) {
                context.Response.Headers.Add(ContentEncodingName, new[] { GzipEncodingType });
                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(gzip);
              }
            }
            else {
              using (var gzip = new DeflateStream(compressed, CompressionLevel.Optimal, leaveOpen: true)) {
                context.Response.Headers.Add(ContentEncodingName, new[] { DeflateEncodingType });
                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(gzip);
              }
            }

            if (context.Response.Headers[ContentLengthName].Count > 0) {
              context.Response.Headers[ContentLengthName] = compressed.Length.ToString();
            }

            compressed.Seek(0, SeekOrigin.Begin);
            await compressed.CopyToAsync(body);
          }
        }
        finally {
          context.Response.Body = body;
        }
      }
    }

    #endregion

    #region Private Method(s)

    private static bool IsCompressionAllowed(string acceptEncodingHeader, out IEnumerable<string> acceptTypes) {
      if (String.IsNullOrWhiteSpace(acceptEncodingHeader)) {
        acceptTypes = null;
        return false;
      }
      acceptTypes = acceptEncodingHeader.Split(',').Select(x => x.ToLower());
      if (acceptTypes.Contains(GzipEncodingType) || acceptTypes.Contains(DeflateEncodingType)) {
        return true;
      }
      return false;
    }

    #endregion
  }
}
