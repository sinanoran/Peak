using Peak.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;

namespace Peak.Web
{
	public class PxClientInfoHelper : IPxClientInfoHelper {

		private IHttpContextAccessor _iHttpContextAccessor;


		public PxClientInfoHelper(IHttpContextAccessor pIContextAccessor) {
			_iHttpContextAccessor = pIContextAccessor;
		}

		public string GetBrowserUserAgent() {
			if (_iHttpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent")) {
				return _iHttpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
			}

			return null;
		}
		
		public string GetIPAddress() {
			return _iHttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
		}
	}
}
