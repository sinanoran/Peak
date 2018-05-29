using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Localization {
  public interface IPxMultilingualData {
    string this[string index] { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string[] LanguageList();
    /// <summary>
    /// 
    /// </summary>
    string Content { get; }
    /// <summary>
    /// Verilen kültür kodunda bir içerik barındırıp barındırmadığını belirtir.
    /// </summary>
    /// <param name="cultureCode"></param>
    /// <returns></returns>
    bool SupportsCulture(string cultureCode);
    /// <summary>
    /// İçeriği, desteklenen kültürlerden ilki ile getirir.
    /// </summary>
    /// <returns></returns>
    string GetContentByAnyCulture();
  }
}
