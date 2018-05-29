using Peak.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Test.Console.Auth {
  public class PxSessionTester {
    public static void InitTest() {
      PxSessionTester ast = new Auth.PxSessionTester();
      ast.PropertyChangeTest();
    }

    public void PropertyChangeTest() {
      PxSession x = PxSession.Get();
      x.Principal.CultureCode = "en-GB";

    }
  }
}
