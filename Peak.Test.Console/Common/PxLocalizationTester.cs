using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Auth.Error;

namespace Peak.Test.Console.Common {
  public class PxLocalizationTester {
    public static void InitTest() {
      try {
        throw AuthExceptions.UserHasBeenClosed("p18928");
      }
      catch (Exception ex) {
        System.Console.WriteLine(ex.Message);
      }

      try {
        throw AuthExceptions.MFAAuthenticationFailed();
      }
      catch (Exception ex) {
        System.Console.WriteLine(ex.Message);
      }

      System.Console.ReadLine();
    }
  }
}
