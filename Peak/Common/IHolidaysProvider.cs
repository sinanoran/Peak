using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Common {

  /// <summary>
  /// Tatil günlerini getiren sınıfın türemesi gereken interfacedir. (Konfig dosyasında yer alan HolidaysProviderAssemblyQualifiedName sınıfı)
  /// </summary>
  public interface IHolidaysProvider {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Holiday> GetHolidays();
  }
}
