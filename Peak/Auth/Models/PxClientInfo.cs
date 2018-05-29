using Peak.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Peak.Auth.Models {
	public class PxClientInfo {

		private IPxClientInfoHelper _iPxClientInfoHelper;

		public PxClientInfo(IPxClientInfoHelper pIPxClientInfoHelper) {
			_iPxClientInfoHelper = pIPxClientInfoHelper;
		}


		public string IPAddress {
			get {
				return _iPxClientInfoHelper.GetIPAddress();
			}
		}
		public string BrowserUserAgent {
			get {
				return _iPxClientInfoHelper.GetBrowserUserAgent();
			}
		}
		
	}
}
