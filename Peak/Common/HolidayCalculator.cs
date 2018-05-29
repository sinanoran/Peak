using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;

namespace Peak.Common {
  /// <summary>
  /// Tatil günü olup olmadığını bildiren sınıftır. Konfig dosyasında yer alan HolidaysProviderAssemblyQualifiedName parametresinde yer alan sınıfı 
  /// çağırarak tatil günlerini tutmaktadır.
  /// </summary>
  public class HolidayCalculator : SingletonBase<HolidayCalculator> {

    #region Private Member(s)

    private Dictionary<DateTime, Holiday> _holidays = null;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public HolidayCalculator() {
      if (!string.IsNullOrEmpty(PxConfigurationManager.PxConfig.HolidaysProviderAssemblyQualifiedName)) {
        Type type = Type.GetType(PxConfigurationManager.PxConfig.HolidaysProviderAssemblyQualifiedName);
        if (type != null && type.GetInterfaces().Contains(typeof(IHolidaysProvider))) {
          IHolidaysProvider provider = Activator.CreateInstance(type) as IHolidaysProvider;
          _holidays = provider.GetHolidays().ToDictionary(x => x.Date, x => x);
        }
      }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="halfPublicDay"></param>
    /// <returns></returns>
    public bool IsHoliday(DateTime time, bool halfPublicDay) {
      bool isHoliday = false;
      if (_holidays != null) {
        Holiday hol = _holidays[time];
        if (hol != null) {
          if (halfPublicDay) {
            if (!hol.IsHalfDay) {
              isHoliday = true;
            }
          }
          else {
            isHoliday = true;
          }
        }
      }
      return time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday || isHoliday;
    }

    #endregion
  }
}
