using Peak.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Peak.Session;

namespace Peak.Web {
	public class PxSessionStore : IPxSessionStore{

		private IHttpContextAccessor _iHttpContextAccessor;
		private IServiceProvider _iServiceProvider;

		public PxSessionStore(IHttpContextAccessor pIHttpContextAccessor, IServiceProvider pIServiceProvider) {
			_iHttpContextAccessor = pIHttpContextAccessor;
			_iServiceProvider = pIServiceProvider;

		}


		public IPxSessionStore Instance() {
			IPxSessionStore session = _iServiceProvider.GetService<IPxSessionStore>();
			return session;
		}

		public bool IsReady {
			get {
				if (this._iHttpContextAccessor == null ||
					this._iHttpContextAccessor.HttpContext == null ||
					this._iHttpContextAccessor.HttpContext.Session == null) { return false; }

				return true;
			}
		}

		public bool Get<T>(string pKey, out T pPayload) {

			pPayload = default(T);

			if (!this.IsReady) {
				return false;
			}

			string json = this._iHttpContextAccessor.HttpContext.Session.GetString(pKey);
			if (string.IsNullOrEmpty(json)) { return false; }

			try {
				pPayload = JsonConvert.DeserializeObject<T>(json);
			}
			/*Depolanan serialized string istenen nesneye cast edilemiyor olabilir*/
			catch (Newtonsoft.Json.JsonReaderException) {
				return false;
			}

			return true;
		}

		public void Set<T>(string pKey, T pPayload) {
			if (!this.IsReady) {
				return;
			}

			string json = JsonConvert.SerializeObject(pPayload);
			this._iHttpContextAccessor.HttpContext.Session.SetString(pKey, json);
		}

		public void Remove(string pKey) {
			if (!IsReady) {
				return;
			}

			this._iHttpContextAccessor.HttpContext.Session.Remove(pKey);
		}

		public T Remove<T>(string pKey) {
			if (!IsReady) {
				return default(T);
			}

			T result;
			if (this._iHttpContextAccessor.HttpContext.Session.Keys.Contains<string>(pKey)) {

				string json = this._iHttpContextAccessor.HttpContext.Session.GetString(pKey);
				if (string.IsNullOrEmpty(json)) {
					result = default(T);
				}

				this._iHttpContextAccessor.HttpContext.Session.Remove(pKey);
				result = JsonConvert.DeserializeObject<T>(json);
			}
			else {
				result = default(T);
			}

			return result;

		}

		public void RemoveAll() {

			if (!IsReady) { return; }

			_iHttpContextAccessor.HttpContext.Session.Clear();

		}

		public void Abandon() {
			if (!IsReady) { return; }

			_iHttpContextAccessor.HttpContext.Session.Clear();
		}

		public HttpContext HttpContext {
			get {
				return _iHttpContextAccessor.HttpContext;
			}
		}
		
	}
}
