using Peak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak.Test.Console.Common {
	public class PxToolkitTester {

		public static void InitTest() {
			//PxToolkitTester tester = new PxToolkitTester();
			//tester.GetSecureSaltTest();
		}


		public void GetSecureSaltTest() {
			string salt = Toolkit.Instance.Security.GetSecureSalt();

			string hash = Toolkit.Instance.Security.GetSecureHash("p18928", salt, Encoding.UTF8, Peak.Common.Enums.HashFormat.Base64);
		}

	}
}
