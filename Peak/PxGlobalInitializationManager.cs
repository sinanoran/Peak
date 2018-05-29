using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using Peak.Localization;
using Peak.Scheduling;

namespace Peak {
  public static class PxGlobalInitializationManager {

    #region Private Member(s)

    private static object lockObj = new object();
    private static volatile bool firstRun = true;

    #endregion

    #region Public Method(s)

    public static void Run() {
      lock (lockObj) {
        if (firstRun) {
          PxLocalizationManager.Initialize();
          if (PxConfigurationManager.PxConfig.Schedule.Enabled) {
            PxTaskScheduler.Instance.Start();
          }
        }
      }
    }

    #endregion
  }
}
