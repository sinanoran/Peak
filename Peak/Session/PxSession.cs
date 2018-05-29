using Peak.Auth.Models;

namespace Peak.Session {
  public class PxSession {

		private static string SESSION_STORE_KEY = "000__Peak__SESSION";
		private static IPxSessionStore _store;
		private static IPxClientInfoHelper _clientInfoHelper;
		//private static PxSession _singletonInstance;

		public static PxSession Current {
      get {
        return PxSession.Get();
      }
		}

		/// <summary>
		/// İlgili request için PxSession instance'ını getirir
		/// </summary>
		/// <returns></returns>
		public static PxSession Get() {
      PxSession instance = null;

      if (_store != null) {
				_store.Get<PxSession>(SESSION_STORE_KEY, out instance);
			}

      if (instance == null) {
        instance = new PxSession();
      }

      return instance;
    }
		/// <summary>
		/// PxSession instance'ini kaydeder (ASPNETCORE Session üzerine)
		/// </summary>
		/// <param name="pSession"></param>
		public static void Save(PxSession pSession) {

			if (_store != null) {
				_store.Set<PxSession>(SESSION_STORE_KEY, pSession);
				return;
			}

			//_singletonInstance = pSession;
		}
		/// <summary>
		/// PxSession'ı initilize etmek için kullanılır. Normalde altyapı tarafından çağrılan bir metoddur. 
		/// Kullanıcının buraya müdahalesi gereksizdir. Ancak session bilgisinin serialize edileceği ortamın
		/// değiştirilmesi isteniyor ise buraya yeni bir IPxSessionStore implemantasyonu verilebilir
		/// </summary>
		/// <param name="pIPxSessionStore"></param>
		internal static void Initilize(IPxSessionStore pIPxSessionStore, IPxClientInfoHelper pIPxClientInfoHelper) {
			_store = pIPxSessionStore;
			_clientInfoHelper = pIPxClientInfoHelper;
		}


		#region Instance seviyesindeki property'ler

		public PxPrincipalInfo Principal { get; set; }

		public PxClientInfo Client { get; }

		public PxAuthorizationInfo Menu { get; set; }


		/// <summary>
		/// Verilen tipteki nesneyi session store'dan alır. DIKKAT: Default Peak implemantasyonunda nesne JSON string'e serialize edilerek 
		/// session içerisine konur. Bu nedenle bu nesnenin session'dan geri alınması sırasında readonly property'lerin değeri kaybolur!
		/// </summary>
		/// <typeparam name="T">Saklanacak nesnenin tipi</typeparam>
		/// <param name="pKey">Session key değeri</param>
		/// <param name="pValue">Serialize edilip session'a konacak nesne</param>
		/// <returns></returns>
		public bool Get<T>(string pKey, out T pValue) {
			bool result = _store.Get<T>(pKey, out pValue);
			return result;
		}

		/// <summary>
		/// Verilen tipteki nesneyi session store üzerine yazar. Default Peak implemantasyonunda nesne JSON string'e serialize edilerek 
		/// session içerisine konur. Bu nedenle bu nesnenin session'dan geri alınması sırasında readonly property'lerin değeri kaybolur!
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pKey"></param>
		/// <param name="pValue"></param>
		public void Set<T>(string pKey, T pValue) {
			_store.Set<T>(pKey, pValue);
		}

		/// <summary>
		/// Verilen bir nesneyi session'dan alır. Session'daki saklı veriyi siler, aldığı nesneyi ilgili tipe deserialize ederek geri döndürür
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pKey"></param>
		/// <returns></returns>
		public T Remove<T>(string pKey) {
			T result = default(T);
			result = _store.Remove<T>(pKey);
			return result;
		}
		/// <summary>
		/// Verilen key'de bulunan değeri siler
		/// </summary>
		/// <param name="pKey"></param>
		public void Remove(string pKey) {
			_store.Remove(pKey);
		}
		/// <summary>
		/// Session üzerinde tüm key'lerdeki değerleri temizler
		/// </summary>
		public void RemoveAll() {
			_store.RemoveAll();
		}
		/// <summary>
		/// Session üzerindeki tüm key'lerdeki değerleri temizler. Ayrıca session nesnesi üzerinde Authentication ve authorization ile gelen
    /// bütün bilgileri de temizler (logout)
    /// ASPNETCORE'da SESSION_ON_END gibi bir event yoktur. Bu nedenle böyle bir event
		/// fire edilmez. .
		/// </summary>
		public void Abandon() {
			_store.Abandon();

      this.Principal.Clear();

		}


    public void Save() {
      if (PxSession._store != null) {
        _store.Set<PxSession>(SESSION_STORE_KEY, this);
        return;
      }

      //_singletonInstance = this;
    }

		//public HttpContext HttpContext {
		//	get {
		//		return _store.HttpContext;
		//	}
		//}

		#endregion



		private PxSession() {
			this.Principal = new PxPrincipalInfo();
			this.Client = new PxClientInfo(PxSession._clientInfoHelper);
		}


    

	}
}
