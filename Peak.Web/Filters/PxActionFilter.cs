using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Common;
using Peak.Configuration;
using Peak.Logging.Loggers;
using Peak.Session;
using Peak.Web.Attributes;
using Peak.Web.Controllers;
using Peak.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Peak.Web.Filters {
  public class PxActionFilter : ActionFilterAttribute {

    #region Private Method(s)
    private void logRequest(ActionExecutingContext context) {
      try {
        if (PxConfigurationManager.PxConfig.Logging.ActionLogEnabled) {
          PxActionLogAttribute logAttribute = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(false).OfType<PxActionLogAttribute>().FirstOrDefault();
          if (logAttribute != null && logAttribute.LogSetting == ActionLogSetting.NoLog) {
            return;
          }
          PxLogRequestInfo info = new PxLogRequestInfo();
          info.RequestId = context.HttpContext.TraceIdentifier;
          if (context.HttpContext.Request.Headers != null && context.HttpContext.Request.Headers.Count > 0) {
            info.Header = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers);
          }
          if (logAttribute == null || logAttribute.LogSetting == ActionLogSetting.TraceWithAllData || logAttribute.LogSetting == ActionLogSetting.TraceWithRequestData) {
            if (context.ActionArguments != null && context.ActionArguments.Count > 0) {
              info.Message = JsonConvert.SerializeObject(context.ActionArguments);
            }
          }
          info.Ip = PxSession.Current.Client.IPAddress;
          info.MethodType = context.HttpContext.Request.Method;
          info.RequestType = context.HttpContext.Request.IsAjaxRequest() ? Common.Enums.RequestType.XHttp : Common.Enums.RequestType.Http;
          info.Url = context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : null;
          info.UserId = PxSession.Current.Principal.UserId;
          info.SessionKey = PxSession.Current.Principal.Authentication.Token;
          PxActionRequestLogger.Log(info);
        }
      }
      catch (Exception ex) {
        PxErrorLogger.Log(ex);
      }
    }

    private void logResponse(ActionExecutedContext context) {

      try {
        if (PxConfigurationManager.PxConfig.Logging.ActionLogEnabled) {
          PxActionLogAttribute logAttribute = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(false).OfType<PxActionLogAttribute>().FirstOrDefault();
          if (logAttribute != null && logAttribute.LogSetting == ActionLogSetting.NoLog) {
            return;
          }
          PxLogResponseInfo info = new PxLogResponseInfo();
          if (context.HttpContext.Response.Headers != null && context.HttpContext.Response.Headers.Count > 0) {
            info.Header = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Response.Headers);
          }
          info.RequestId = context.HttpContext.TraceIdentifier;
          info.ResponseCode = context.HttpContext.Response.StatusCode.ToString();
          if (logAttribute != null && (logAttribute.LogSetting == ActionLogSetting.TraceWithAllData || logAttribute.LogSetting == ActionLogSetting.TraceWithResponseData)) {
            if (context.Result is RedirectResult) {
              RedirectResult redirectResult = context.Result as RedirectResult;
              info.Message = JsonConvert.SerializeObject(new { RedirectUrl = redirectResult.Url });
            }
            else if (context.Result is RedirectToActionResult) {
              RedirectToActionResult redirectToActionResult = context.Result as RedirectToActionResult;
              info.Message = JsonConvert.SerializeObject(new { ControllerName = redirectToActionResult.ControllerName, ActionName = redirectToActionResult.ActionName, RouteValues = redirectToActionResult.RouteValues });
            }
            else if (context.Result is RedirectToRouteResult) {
              RedirectToRouteResult redirectToRouteResult = context.Result as RedirectToRouteResult;
              info.Message = JsonConvert.SerializeObject(redirectToRouteResult.RouteValues);
            }
            else if (!(context.Result is FileResult)) {
              info.Message = JsonConvert.SerializeObject(context.Result);
            }
          }
          info.UserId = PxSession.Current.Principal.UserId;
          info.SessionKey = PxSession.Current.Principal.Authentication.Token;
          PxActionResponseLogger.Log(info);
        }
      }
      catch (Exception ex) {
        PxErrorLogger.Log(ex);
      }

    }

    #endregion

    #region IAction Filter Member(s)
    public override void OnActionExecuted(ActionExecutedContext context) {
      logResponse(context);
    }
    public override void OnActionExecuting(ActionExecutingContext context) {
      logRequest(context);
      if (context.ActionDescriptor.FilterDescriptors.FirstOrDefault(x => x.Filter is Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter) == null) {
        string controller = context.RouteData.Values["controller"].ToString();
        string action = context.RouteData.Values["action"].ToString();
        PxSession session = PxSession.Get();
        if (!session.Principal.Authentication.IsAuthenticated) {
          if (context.HttpContext.Request.IsAjaxRequest()) {
            context.HttpContext.Response.Headers.Add("REQUIRE_AUTHENTICATION", "1");
            context.Result = new JsonResult(null);
          }
          else {
            context.Result = new RedirectToActionResult("Login", "Account", null);
          }
        }
        else if (PxConfigurationManager.PxConfig.Authentication.MultiFA.Enabled
                    && !session.Principal.Authentication.IsMFAAuthenticationCompleted
                        && controller != "Account" && action != "TwoFA") {
          if (context.HttpContext.Request.IsAjaxRequest()) {
            context.HttpContext.Response.Headers.Add("REQUIRE_TWOFA_AUTHENTICATION", "1");
            context.Result = new JsonResult(null);
          }
          else {
            context.Result = new RedirectToActionResult("TwoFA", "Account", null);
          }
        }
        else if(session.Principal.Authentication.IsPasswordMustChanged && controller != "Account" && action != "ChangePassword") {
          if (context.HttpContext.Request.IsAjaxRequest()) {
            context.HttpContext.Response.Headers.Add("REQUIRE_CHANGE_PASSWORD", "1");
            context.Result = new JsonResult(null);
          }
          else {
            context.Result = new RedirectToActionResult("ChangePassword", "Account", null);
          }
        }
      }      
    }

    #endregion
  }
}
