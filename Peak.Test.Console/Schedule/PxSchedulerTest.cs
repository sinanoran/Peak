using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Scheduling;

namespace Peak.Test.Console.Schedule {
  public class PxSchedulerTest {
    public static void InitTest() {
      PxTaskScheduler.Instance.Start();
      System.Console.ReadLine();
      PxTaskScheduler.Instance.Stop();
    }
  }
}
