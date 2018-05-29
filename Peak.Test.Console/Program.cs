using Peak.Test.Console.Auth;
using Peak.Test.Console.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Test.Console.Schedule;
using Peak.Test.Console.Dal;

namespace Peak.Test.Console {
  public class Program {
    public static void Main(string[] args) {
      try {
        //PxAuthorizationTester.InitTest();
        //PxToolkitTester.InitTest();
        //PxAuthenticationTester.InitTest();
        //PxLoggerTester.InitTest();
        //PxLocalizationTester.InitTest();
        //PxSchedulerTest.InitTest();
        //PxDalTester.InitTest();

        PxSessionTester.InitTest();
      }
      catch(Exception ex) {
        System.Console.WriteLine(ex.ToString());
      }
      System.Console.ReadLine();
    }
  }
}
