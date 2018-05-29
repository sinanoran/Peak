using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Logging;
using Peak.Logging.Loggers;
using Microsoft.Extensions.Logging;

namespace Peak.Configuration {
  internal static class PxDbConfigurationManager {

    #region Private Members(s)

    private static readonly Dictionary<string, List<Parameter>> _parameters;

    #endregion

    #region Constructor(s)

    static PxDbConfigurationManager() {
      try {
        using (PeakDbContext dbContext = new PeakDbContext()) {
          _parameters = dbContext.Parameters.Where(x => !x.CancelDate.HasValue).GroupBy(x => x.Group).ToDictionary(g => g.Key, g => g.ToList());
        }
      }
      catch {
        _parameters = new Dictionary<string, List<Parameter>>();
      }
    }

    #endregion

    #region Internal Method(s)

    internal static T GetValue<T>(string group, string name) {
      if (_parameters.ContainsKey(group) && _parameters[group] != null) {
        var parameter = _parameters[group].FirstOrDefault(x => x.Name == name);
        if (parameter != null) {
          return (T)Convert.ChangeType(parameter.Value, typeof(T));
        }
      }
      return default(T);
    }

    #endregion

  }
}
