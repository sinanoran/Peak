using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Peak.Test.Web.Tests;

namespace Peak.Test.Web.Controllers {
	public class HomeController : Controller {
		public IActionResult Index() {

			SessionTester.InitTest();

			return View();
		}

		public IActionResult About() {
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact() {
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error() {
			return View();
		}
	}
}
