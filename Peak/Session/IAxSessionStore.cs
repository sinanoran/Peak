using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Session {
	public interface IPxSessionStore {
		bool IsReady { get; }
		bool Get<T>(string pKey, out T pPayload);
		void Set<T>(string pKey, T pPayload);
		T Remove<T>(string pKey);
		void Remove(string pKey);
		void RemoveAll();
		void Abandon();

		
	}
}
