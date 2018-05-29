using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace Peak.Scheduling {

  /// <summary>
  /// Schedulerin çalıştıracağı sınıfların türemesi gereken interfacedir. Schedule'ın zamanı geldiğin Execute metodu çağrılır.
  /// </summary>
  public interface IPxTaskAction {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduledTime"></param>
    void Excecute(DateTime scheduledTime);
  }
}
