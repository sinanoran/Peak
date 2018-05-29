using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using Peak.Common;

namespace Peak.Web {
  /// <summary>
  /// Json inputların html encode edilmesi için kullanılır. MvcOptions UseHtmlEncodeJsonInputFormatter extensionı ile eklenebilir.
  /// </summary>
  public class HtmlEncodeContractResolver : DefaultContractResolver {
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
      var properties = base.CreateProperties(type, memberSerialization);

      foreach (var property in properties.Where(p => p.PropertyType == typeof(string))) {
        var propertyInfo = type.GetProperty(property.UnderlyingName);
        if (propertyInfo != null) {
          property.ValueProvider = new HtmlEncodingValueProvider(propertyInfo);
        }
      }

      return properties;
    }

    protected class HtmlEncodingValueProvider : IValueProvider {
      private readonly PropertyInfo _targetProperty;

      public HtmlEncodingValueProvider(PropertyInfo targetProperty) {
        this._targetProperty = targetProperty;
      }

      public void SetValue(object target, object value) {
        var s = value as string;
        if (s != null) {
          var encodedString = Toolkit.Instance.HtmlEncode(s);
          _targetProperty.SetValue(target, encodedString);
        }
        else {
          _targetProperty.SetValue(target, value);
        }
      }

      public object GetValue(object target) {
        return _targetProperty.GetValue(target);
      }
    }
  }
}
