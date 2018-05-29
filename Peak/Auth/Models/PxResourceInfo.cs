using Peak.Auth.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Models {
	/// <summary>
	/// Erişim yetkisi verilecek her bir kaynağı tanımlayacak, ifade edecek sınıftır. Kaynak yetkilendirilmek istenen herhangi bir şey olabilir.
	/// Örneğin bir ekran, ekran üzerindeki bir alan ya da erişilecek bir servis kaynak olarak tanımlanabilir.
	/// </summary>
	public class PxResourceInfo {
		private long _id;
		private string _code;
		private string _name;
		private PxResourceType _type;
		private long? _parentId;
		private Dictionary<string, PxPermissionInfo> _permissions;
		private string _groupCode1;
		private string _groupCode2;
		private string _groupCode3;
    private string _value; 

		public PxResourceInfo() {
			this._permissions = new Dictionary<string, PxPermissionInfo>();
		}

		/// <summary>
		/// Permission'ın saklandığı kalıcı ortamdaki (veri tabanı) ayrıcı ID değeri
		/// </summary>
		public long ID {
			get { return _id; }
			set { _id = value; }
		}
		public long? ParentID {
			get { return _parentId; }
			set { _parentId = value; }
		}
		/// <summary>
		/// Resource'a verilmiş ID'den farklı bir unique değer. Developer'lar tarafından verilir. Ilgili resource'u
		/// unique olarak tanımlar. Resource tanımı bir kalıcı ortamdan bir diğerine taşındığında (örneğin DEV veri tabanından TEST veri tabanına kopyalandığında)
		/// ID değeri değişse bile bu değer sabit kalır.
		/// </summary>
		public string Code {
			get { return _code; }
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentException("Peak.Auth.PxResource Code property cannot be null or empty");
				}
				_code = value;
			}
		}
		/// <summary>
		/// Resource tanımı için verilmiş açıklayıcı bilgidir.
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		/// <summary>
		/// Resource'un tipini tanımlar. Resource'lar pek çok tipte olabilir. Örneğin Servis metodu, ekran, ekrandaki buton gibi bir UI bileşeni...vb
		/// </summary>
		public PxResourceType Type {
			get { return _type; }
			set { _type = value; }
		}
		/// <summary>
		/// Resource'ları gruplamak gerektiğinde kullanılabilecek ayırıcı bir bilgi
		/// </summary>
		public string GroupCode1 {
			get { return _groupCode1; }
			set { _groupCode1 = value; }
		}
		/// <summary>
		/// Resource'ları gruplamak gerektiğinde kullanılabilecek ayırıcı bir bilgi
		/// </summary>
		public string GroupCode2 {
			get { return _groupCode2; }
			set { _groupCode2 = value; }
		}
		/// <summary>
		/// Resource'ları gruplamak gerektiğinde kullanılabilecek ayırıcı bir bilgi
		/// </summary>
		public string GroupCode3 {
			get { return _groupCode3; }
			set { _groupCode3 = value; }
		}
    /// <summary>
    /// Resource'un gösterdiği şeyle ilgili bilgi. Örneğin bir URL ise bu, bir file ise adı ve yolu...vb
    /// </summary>
    public string Value {
      get {
        return _value;
      }
      set {
        _value = value;
      }
    }

		/// <summary>
		/// Verilen bir permission kodunun bu resource'a bağlı olarak var olup olmadığını kontrol eder eğer bağlı ise opermission nesnesini geri döner
		/// </summary>
		/// <param name="pPermissionCode"></param>
		/// <returns></returns>
		public PxPermissionInfo GetPermission(string pPermissionCode) {
			if (_permissions == null || _permissions.Count <= 0) { return null; }

			PxPermissionInfo result;
			_permissions.TryGetValue(pPermissionCode, out result);
			return result;
		}

		public void AddPermission(PxPermissionInfo pPermission) {
			PxPermissionInfo existingPerm;
			if (_permissions.TryGetValue(pPermission.Code, out existingPerm)) {
				existingPerm.IsGiven = pPermission.IsGiven;
			}
			else {
				_permissions.Add(pPermission.Code, pPermission);
			}
		}


		/// <summary>
		/// Verilen bir permission kodunun bu resource'a bağlı olarak ALLOW ile verilip verilmediğini döner. Permission resource ile hiyerarşi içerisinde
		/// hiç ilişkilendirilmemiş ise sonuç DENY'dır (false döner)
		/// </summary>
		/// <param name="pPermissionCode"></param>
		/// <returns></returns>
		public bool IsPermissionGiven(string pPermissionCode) {
			if (this._permissions == null || this._permissions.Count <= 0) { return false; }

			PxPermissionInfo permission;
			_permissions.TryGetValue(pPermissionCode, out permission);
			
			if (permission == null) {
				return false;
			}

			return permission.IsGiven;

		}

		public Dictionary<string, PxPermissionInfo> Permissions {
			get { return _permissions; }
			set { _permissions = value; }
		}
	}
}
