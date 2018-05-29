using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Peak.Common;
using Peak.Web.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Peak.Web.ModelBinders {

  /// <summary>
  /// String type olan model veya model propertylerini html encode edilmesi için kullanılır.
  /// </summary>
  public class HtmlEncodeModelBinder : IModelBinder {

    private readonly IModelBinder _fallbackBinder;

    public HtmlEncodeModelBinder(IModelBinder fallbackBinder) {
      this._fallbackBinder = fallbackBinder;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext) {
      if(bindingContext == null) {
        throw new ArgumentNullException(nameof(bindingContext));
      }

      var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
      if(valueProviderResult != null && valueProviderResult.Length > 0) {
        if(string.IsNullOrEmpty(valueProviderResult.FirstValue)) {
          return _fallbackBinder.BindModelAsync(bindingContext);
        }
        var result = Toolkit.Instance.HtmlEncode(valueProviderResult.FirstValue);
        bindingContext.Result = ModelBindingResult.Success(result);
      }
      return Task.CompletedTask;
    }
  }

  /// <summary>
  /// HtmlEncodeModelBinder Provider'idır. MvcOptions'lara UseHtmlEncodeModelBinding extension metodu ile eklenir.
  /// </summary>
  public class HtmlEncodeModelBinderProvider : IModelBinderProvider {
    public IModelBinder GetBinder(ModelBinderProviderContext context) {
      if (context == null) {
        throw new ArgumentNullException(nameof(context));
      }
      if(!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string)) {
        if (context.Metadata.ContainerType != null) {
          var propertyInfo = context.Metadata.ContainerType.GetProperty(context.Metadata.PropertyName);
          var attribute = propertyInfo.GetCustomAttributes(typeof(PxNoHtmlEncodeAttribute), false).FirstOrDefault();
          if (attribute == null) {
            return new HtmlEncodeModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType));
          }
        }       
      }

      return null;
    }
  }
}
