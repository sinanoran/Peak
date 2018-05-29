using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Peak.Common {

  /// <summary>
  /// 
  /// </summary>
  public partial class Toolkit : SingletonBase<Toolkit> {

    #region Private Member(s)

    private readonly SecurityTool _security;
    private readonly XmlTool _xml;
    private readonly RandomNumberProvider _randomNumberProvider;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public Toolkit() {
      _security = new SecurityTool();
      _xml = new XmlTool();
      _randomNumberProvider = new RandomNumberProvider();
    }

    #endregion

    #region Public Property(s)

    /// <summary>
    /// 
    /// </summary>
    public SecurityTool Security {
      get {
        return _security;
      }
    }

    public XmlTool Xml {
      get {
        return _xml;
      }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public int GenerateRandomNumber(int size) {
      string minValue = "1";
      string maxValue = "9";
      for (int i = 0; i < size - 1; i++) {
        minValue = minValue + "0";
        maxValue = maxValue + "9";
      }
      int minVal = int.Parse(minValue);
      int maxVal = int.Parse(maxValue);
      return _randomNumberProvider.Next(minVal, maxVal);
    }

    /// <summary>
    /// Tip bilgisi verilen class'ın örneğinin yaratır.
    /// </summary>
    /// <param name="assemblyQualifiedName"></param>
    /// <returns></returns>
    public object CreateInstance(string assemblyQualifiedName) {
      Type type = Type.GetType(assemblyQualifiedName);
      return Activator.CreateInstance(type);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assemblyQualifiedName"></param>
    /// <returns></returns>
    public T CreateInstance<T>(string assemblyQualifiedName) {
      return (T)CreateInstance(assemblyQualifiedName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string CreateUniqueId() {
      return Guid.NewGuid().ToString("N");
    }

    public string HtmlEncode(string value) {
      StringBuilder sb = new StringBuilder(WebUtility.HtmlEncode(value));
      sb.Replace("&#304;", "İ");
      sb.Replace("&#305;", "ı");
      sb.Replace("&#214;", "Ö");
      sb.Replace("&#246;", "ö");
      sb.Replace("&#220;", "Ü");
      sb.Replace("&#252;", "ü");
      sb.Replace("&#199;", "Ç");
      sb.Replace("&#231;", "ç");
      sb.Replace("&#286;", "Ğ");
      sb.Replace("&#287;", "ğ");
      sb.Replace("&#350;", "Ş");
      sb.Replace("&#351;", "ş");
      sb.Replace("&#235;", "ë");
      sb.Replace("&#203;", "Ë");
      return sb.ToString();
    }

    public byte[] HexStringToByteArray(string hex) {
      return Enumerable.Range(0, hex.Length)
                       .Where(x => x % 2 == 0)
                       .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                       .ToArray();
    }

    #endregion
  }
}
