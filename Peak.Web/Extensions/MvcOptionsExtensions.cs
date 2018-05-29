using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Web.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace Peak.Web.Extensions {
  public static class MvcOptionsExtensions {
    public static void UseHtmlEncodeModelBinding(this MvcOptions opts) {
      var binderToFind = opts.ModelBinderProviders.OfType<SimpleTypeModelBinderProvider>().FirstOrDefault();
      if(binderToFind == null) {
        return;
      }
      var index = opts.ModelBinderProviders.IndexOf(binderToFind);
      opts.ModelBinderProviders.Insert(index, new HtmlEncodeModelBinderProvider());
    }

    public static void UseHtmlEncodeJsonInputFormatter(this MvcOptions opts, ILogger<MvcOptions> logger, ObjectPoolProvider objectPoolProvider) {
      opts.InputFormatters.RemoveType<JsonInputFormatter>();
      var serializerSettings = new JsonSerializerSettings
      {
        ContractResolver = new HtmlEncodeContractResolver()
      };
      var jsonInputFormatter = new JsonInputFormatter(logger, serializerSettings, ArrayPool<char>.Shared, objectPoolProvider);
      opts.InputFormatters.Add(jsonInputFormatter);
    }
  }
}
