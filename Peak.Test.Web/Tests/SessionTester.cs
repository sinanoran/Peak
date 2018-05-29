using Peak.Auth.Models;
using Peak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Session;

namespace Peak.Test.Web.Tests {
	public class SessionTester {
		public static void InitTest() {
			SessionTester cct = new SessionTester();
			cct.GetSessionTest();
		}

		public void GetSessionTest() {

			PxSession session = PxSession.Get();

			PersonTest pt = new PersonTest(){ Ad="Sinan", Soyad="Oran", Yas=35 };

			int a;
			string b;

			session.Set<PersonTest>("MY_TEST0", pt);


			if (session.Get<int>("MY_TEST1", out a)) {
				//session'dan başarılı şekilde alındı
			}

			if (session.Get<string>("MY_TEST2", out b)) {
				//session'dan başarılı şekilde alındı
			}

			session.Set<int>("MY_TEST1", 100);
			session.Set<string>("MY_TEST2", "sinan oran");

						

		}

	}
}
