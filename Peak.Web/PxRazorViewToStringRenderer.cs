using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Peak.Web {
  public class PxRazorViewToStringRenderer {

    #region Private Member(s)

    private static PxRazorViewToStringRenderer _instance;
    private IRazorViewEngine _viewEngine;
    private ITempDataProvider _tempDataProvider;
    private IServiceProvider _serviceProvider;

    #endregion

    #region Constructor(s)
    public PxRazorViewToStringRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider) {
      this._viewEngine = viewEngine;
      this._tempDataProvider = tempDataProvider;
      this._serviceProvider = serviceProvider;
    }

    #endregion

    #region Public Method(s)
 
    public async Task<string> RenderViewToStringAsync(ActionContext actionContext, string name, ViewDataDictionary viewData) {
      if (actionContext == null) {
        actionContext = getActionContext();
      }

      var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

      if (!viewEngineResult.Success) {
        throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
      }

      var view = viewEngineResult.View;

      using (var output = new StringWriter()) {
        var viewContext = new ViewContext(
            actionContext,
            view,
            viewData,
            new TempDataDictionary(
                actionContext.HttpContext,
                _tempDataProvider),
            output,
            new HtmlHelperOptions());
     
        await view.RenderAsync(viewContext);

        return output.ToString();
      }
    }

    #endregion

    #region Internal Method(s)
    internal static void Configure(PxRazorViewToStringRenderer renderer) {
      _instance = renderer;
    }

    #endregion

    #region Private Method(s)

    private ActionContext getActionContext() {
      var httpContext = new DefaultHttpContext();
      httpContext.RequestServices = _serviceProvider;
      return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

    #endregion

    #region Public Property(s)

    public static PxRazorViewToStringRenderer Instance {
      get {
        return _instance;
      }
    }

    #endregion
  }

}
