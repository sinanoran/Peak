
using Peak.Auth;
using Peak.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Peak.Test.Console.Auth {
	public class PxAuthorizationTester {

		public static void InitTest() {
			PxAuthorizationTester a = new PxAuthorizationTester();
			//a.TestAuthorization();

			a.GetAuthentication();

		}


		public void GetAuthentication() {
			PxAuthorization a = new PxAuthorization();
			a.Authorize(2);
		}

		public PxAuthorizationInfo GetAuthenticationMock() {
			 PxAuthorizationInfo auth = new PxAuthorizationInfo();


			PxPermissionInfo perm1 = new PxPermissionInfo();
			perm1.ID = 1;
			perm1.Code = "READ";
			perm1.Name = "Okuma";
			perm1.IsGiven = true;

			PxPermissionInfo perm2 = new PxPermissionInfo();
			perm2.ID = 2;
			perm2.Code = "WRITE";
			perm2.Name = "Yazma";
			perm2.IsGiven = true;

			PxPermissionInfo p3 = new PxPermissionInfo();
			p3.ID = 3;
			p3.Code = "ACCESS";
			p3.Name = "ERişim yetkisi";
			p3.IsGiven = true;


			PxResourceInfo r1 = new PxResourceInfo();
			r1.ID = 1;
			r1.Code = "Home_Index";
			r1.Name = "Karşılama sayfası";
			r1.Type = Peak.Auth.Enums.PxResourceType.CodeBlock;
			r1.Permissions.Add(perm1.Code, perm1);
			r1.Permissions.Add(perm2.Code, perm2);

			PxResourceInfo r2 = new PxResourceInfo();
			r2.ID = 2;
			r2.Code = "Home";
			r2.Name = "Home Controller - container";
			r2.Type = Peak.Auth.Enums.PxResourceType.CodeBlockContainer;
			r2.Permissions.Add(p3.Code, p3);

			auth.AuthorizationDate = DateTime.Now;
			auth.Source = "MOCK CREATOR - Peak.Test.Console.Auth.PxAuthorizationTester";
			auth.Resources.Add(r1.Code, r1);
			auth.Resources.Add(r2.Code, r2);

			return auth;
		}

		
		public void TestAuthorization() {
			PxAuthorizationInfo au = this.GetAuthenticationMock();

			PxResourceInfo r1 = au.GetResource("Home");
			PxResourceInfo r2 = au.GetResource("Home", "READ");

			bool a = au.IsPermissionGiven("Home", "READ");
			bool b = au.IsPermissionGiven("Home", "ACCESS");
		}


	}
}
