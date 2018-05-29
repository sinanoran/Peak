using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Peak.Localization {
  public class PxMultilingualData : IPxMultilingualData {

    #region Private Member(s)

    /// <summary>
    /// İçerik değiştiğinde tetiklenir.
    /// </summary>
    public event EventHandler ContentChanged;
    /// <summary>
    /// ContentChanged event'inin tetiklenmesini önler.
    /// </summary>
    private bool suppressContentChanged;
    private const string CDATA_TEMPLATE = "<![CDATA[{0}]]>";
    private XmlDocument doc = null;
    #endregion

    #region Constructor(s)

    public PxMultilingualData(string content) {
      doc = new XmlDocument();
      if (string.IsNullOrEmpty(content)) {
        doc.LoadXml("<Data/>");
      }
      else {
        doc.LoadXml(content);
      }
      foreach (string lang in this.LanguageList()) {
        if (string.IsNullOrEmpty(this[lang])) {
          this[lang] = null;
        }
      }
    }

    #endregion

    #region Private Method(s)

    private void onContentChanged() {
      if (!suppressContentChanged && ContentChanged != null) {
        ContentChanged(this, EventArgs.Empty);
      }
    }

    private string normalizeLineEndings(string str) {
      if (string.IsNullOrEmpty(str)) {
        return str;
      }
      return str.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
    }

    #endregion

    #region Public Property(s)

    /// <summary>
    /// CultureInfo.Name
    /// </summary>
    /// <param name="cultureCode">CultureInfo.Name</param>
    /// <returns></returns>
    public string this[string cultureName] {
      get {
        XmlNodeList list = doc.GetElementsByTagName(cultureName);
        if (list.Count == 0) {
          return null;
        }
        string tmp = list[0].InnerText;
        tmp = normalizeLineEndings(tmp);
        return tmp;
      }
      set {
        bool changed = false;
        XmlNodeList list = doc.GetElementsByTagName(cultureName);
        if (list.Count == 1) {
          doc.DocumentElement.RemoveChild(list[0]);
          changed = true;
        }
        if (!string.IsNullOrEmpty(value)) {
          XmlElement element = doc.CreateElement(cultureName);
          XmlCDataSection xmlCData = doc.CreateCDataSection(value);
          element.AppendChild(xmlCData);
          doc.DocumentElement.AppendChild(element);
          changed = true;
        }
        if (changed) {
          onContentChanged();
        }
      }
    }   
    public string Content { get { return doc.InnerXml; } }

    #endregion

    #region Public Method(s)
    public KeyValuePair<string, string>[] SplitContentByLanguage() {
      string[] languages = LanguageList();
      List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(languages.Length);
      foreach (string language in languages) {
        list.Add(new KeyValuePair<string, string>(language, this[language]));
      }
      return list.ToArray();
    }
    public static string SetData(string wholeContent, string language, string data) {
      PxMultilingualData md = new PxMultilingualData(wholeContent);
      md[language] = data;
      return md.Content;
    }

    public static string GetData(string wholeContent, string language) {
      PxMultilingualData md = new PxMultilingualData(wholeContent);
      return md[language];
    }

    public string[] LanguageList() {
      List<string> list = new List<string>();
      foreach (XmlNode item in doc.DocumentElement.ChildNodes) {
        list.Add(item.Name);
      }
      return list.ToArray();
    }

    /// <summary>
    /// Dil kodu bazında verilen içeriği set eder.
    /// </summary>
    /// <param name="contentByLanguageCode">Key --> dil kodu (tr-TR gibi), Value--> içerik </param>
    public void SetContent(IEnumerable<KeyValuePair<string, string>> contentByLanguageCode) {
      suppressContentChanged = true;
      try {
        foreach (KeyValuePair<string, string> data in contentByLanguageCode) {
          this[data.Key] = data.Value;
        }
        suppressContentChanged = false;
        onContentChanged();
      }
      finally {
        suppressContentChanged = false;
      }
    }

    #endregion


    #region IMultilingualData Members


    public bool SupportsCulture(string cultureCode) {
      return LanguageList().Contains(cultureCode);
    }

    public string GetContentByAnyCulture() {
      string[] cultures = LanguageList();
      if (cultures.Length > 0) {
        return this[cultures[0]];
      }
      return string.Empty;
    }

    #endregion
  }
}
