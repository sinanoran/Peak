using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Peak.Web.Extensions;
using Peak.ErrorHandling;
using Peak.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Peak.Logging.Loggers;
using Peak.Common;

namespace Peak.Web.Filters {
  public class PxExceptionFilter : ExceptionFilterAttribute {

    public override void OnException(ExceptionContext context) {
      if (context.Exception == null) {
        return;
      }
      string errorCode = PxErrorLogger.Log(context.Exception, context.HttpContext.TraceIdentifier);

      if (context.ExceptionHandled) {
        return;
      }
      PxException axException = context.Exception as PxException;
      if (context.HttpContext.Request.IsAjaxRequest()) {
        PxAjaxResult result = new PxAjaxResult() { IsError = true };
        if (axException != null) {
          result.IsFatalError = (axException.Priority == Common.Enums.ErrorPriority.High);
          result.ErrorCode = axException.ErrorCode;
          //result.ErrorMessage = axException.Message;
        }
        else {
          result.IsFatalError = true;
          result.ErrorCode = errorCode;
          //result.ErrorMessage = context.Exception.Message;
        }
        context.Result = new JsonResult(result);
      }
      else {
        context.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(
                                    new
                                    {
                                      controller = "Error",
                                      action = "Index",
                                      errorCode = (axException != null ? axException.ErrorCode : errorCode)
                                    })
                                );
      }
      context.ExceptionHandled = true;
    }
  }
}
