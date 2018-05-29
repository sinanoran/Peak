using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common;
using Peak.Configuration;
using Peak.Dal;
using Peak.Session;

namespace Peak.Localization {
  /// <summary>
  /// 
  /// </summary>
  public static class PxLocalizationManager {

    #region Private Member(s)

    private static Dictionary<string, Dictionary<string, string>> _resources;

    #endregion

    #region Internal Method(s)

    internal static void Initialize() {
      _resources = new Dictionary<string, Dictionary<string, string>>();
      try {
        if (PxConfigurationManager.PxConfig.Localization.Enabled) {
          using (PeakDbContext dbContext = new PeakDbContext()) {
            foreach (var culture in PxConfigurationManager.PxConfig.Localization.AvailableCultures) {
              _resources[culture] = dbContext.LocalizedStrings.Where(x => x.CultureCode == culture).ToDictionary(x => x.Code, x => x.Value);
            }
          }
        }
      }
      catch (Exception ex) {
        _resources = new Dictionary<string, Dictionary<string, string>>();
      }
    }

    #endregion

    #region Public Method(s)

    public static string Get(string code) {
      return Get(code, PxSession.Get().Principal.CultureCode);
    }

    public static string Get(string code, CultureInfo culture) {
      return Get(code, culture.Name);
    }

    public static string Get(string code, string cultureCode) {
      string val = string.Empty;
      if (_resources != null && _resources.ContainsKey(cultureCode) && _resources[cultureCode] != null) {
        _resources[cultureCode].TryGetValue(code, out val);
      }
      return val;
    }

    public static Dictionary<string,string> GetResources(string cultureCode) {
      if (_resources != null && _resources.ContainsKey(cultureCode)) {
        return _resources[cultureCode];
      }
      return null;
    }

    #endregion
  }
}
