using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Models {
	public class PxAuthorizationInfo {

		private string _source;
		private DateTime _authorizationDate;
		private Dictionary<string, PxResourceInfo> _resources;

		public PxAuthorizationInfo() {
			this._resources = new Dictionary<string, PxResourceInfo>();
		}

		/// <summary>
		/// Authorization işleminin yapıldığı kaynak sistemi belirtir bir bilgi
		/// </summary>
		public string Source {
			get {
				return _source;
			}
			set {
				_source = value;
			}
		}


		/// <summary>
		/// Authorization işleminin ne zaman yapıldığını tutar. Bu değerin normalde Login tarihi ile aynı olması beklenir. Ancak bir nedenle
		/// Authentication'dan bağımsız olarak Authorization tekrar yapılmış olabilir. Bu durumda bu tarihler fark eder. Herhangi bir durumda
		/// Authorization verisi 10 günden daha eski olamaz
		/// </summary>
		public DateTime AuthorizationDate {
			get {
				return _authorizationDate;
			}
			set {
				//if (value < DateTime.Now.AddDays(-10)) {
				//	throw new ArgumentException("Peak.Auth.PxAuthorization AuthorizationDate cannot be older more than 10 days");
				//}
				_authorizationDate = value;
			}
		}
		/// <summary>
		/// Bu authorization'a bağlı olan resource'ların tamamını içeren dictionary
		/// </summary>
		public Dictionary<string, PxResourceInfo> Resources {
			get {
				return _resources;
			}
			set {
				_resources = value;
			}
		}
		/// <summary>
		/// Authorization dictionary'si içerisinden istenen Resource nesnesini verir. 
		/// </summary>
		/// <param name="pResourceCode"></param>
		/// <returns></returns>
		public PxResourceInfo GetResource(string pResourceCode) {
			if (_resources == null || _resources.Count <= 0) { return null; }

			PxResourceInfo resource;
			_resources.TryGetValue(pResourceCode, out resource);
			return resource;
		}

		/// <summary>
		/// Authoriaztion dictionary'si içerisinden istenen resource'a istenen permission bağlanmışsa bu resource'u geri verir.
		/// Ancak resource verilen permission ALLOW da olabilir DENY da olabilir. Bunu developer'ın kontrol etmesi gerekir.
		/// </summary>
		/// <param name="pResourceCode"></param>
		/// <param name="pPermissionCode"></param>
		/// <returns></returns>
		public PxResourceInfo GetResource(string pResourceCode, string pPermissionCode) {
			if (_resources == null || _resources.Count <= 0) { return null; }

			PxResourceInfo resource;
			_resources.TryGetValue(pResourceCode, out resource);

			if (resource == null) { return null; }

			PxPermissionInfo permission = resource.GetPermission(pPermissionCode);

			if (permission == null) { return null; }

			return resource;
		}

		public void AddResource(PxResourceInfo pResource) {
			PxResourceInfo existingResource;
			if (_resources.TryGetValue(pResource.Code, out existingResource)) {
				throw new InvalidOperationException("Cannot add a resource second time"); 
			}
			else {
				_resources.Add(pResource.Code, pResource);
			}
		}


		/// <summary>
		/// Verilen resource'a verilen Permission için ALLOW verilmiş mi kontrol eder. Verilmişse TRUE döner.
		/// DENY verilmiş ya da tanım yapılmamışsa FALSE döner
		/// </summary>
		/// <param name="pResourceCode"></param>
		/// <param name="pPermissionCode"></param>
		/// <returns></returns>
		public bool IsPermissionGiven(string pResourceCode, string pPermissionCode) {
			if (_resources == null || _resources.Count <= 0) { return false; }

			PxResourceInfo resource;
			_resources.TryGetValue(pResourceCode, out resource);

			if (resource == null) { return false; }

			bool result = resource.IsPermissionGiven(pPermissionCode);

			return result;
		}


    public void Clear() {
      this._source = string.Empty;
      this._authorizationDate = DateTime.MinValue;
      this._resources.Clear();
    }

	}
}
