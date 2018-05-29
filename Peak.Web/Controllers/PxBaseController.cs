using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Peak.Configuration;
using Peak.Logging;
using Microsoft.Extensions.Logging;

namespace Peak.Web.Controllers {
  public class PxBaseController : Controller {   

    #region Constructor(s)

    public PxBaseController() {
    
    }

    #endregion

    #region Public Method(s)    

    public string RenderViewToString(string name, object model) {
      ViewDataDictionary viewData = new ViewDataDictionary(metadataProvider: new EmptyModelMetadataProvider(), modelState: new ModelStateDictionary());
      if (model != null) {
        viewData.Model = model;
      }
      foreach (var key in ViewData.Keys) {
        viewData.Add(key, ViewData[key]);
      }
      return PxRazorViewToStringRenderer.Instance.RenderViewToStringAsync(ControllerContext, name, viewData).Result;
    }

    public string RenderViewToString<TModel>(string name) {
      return RenderViewToString(name, null);
    }

    #endregion  

  }
}
