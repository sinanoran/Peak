using System;

namespace Peak.Auth.Models {
	/// <summary>
	/// Bir kaynağa (Resource) verilebilecek yetkileri (Permission) temsil eden sınıftır. 
	/// Permission ile Kaynak ilişkilendirildiğinde ilişki ALLOW ya da DENY şeklinde olur.
	/// Tanımsız (UNDEFINED) diye bir durum olmaz. Tanımsız olma durumunda Permission ile Kaynak
	/// ilişkilendirilmemiş demektir. 
	/// </summary>
	public class PxPermissionInfo {
		private long _id;
		private string _code;
		private string _name;
		private bool _isGiven;

		/// <summary>
		/// Permission'ın saklandığı kalıcı ortamdaki (örneğin veri tabanı) ayırıcı ID değeri
		/// </summary>
		public long ID {
			get {
				return _id;
			}
			set {
				_id = value;
			}
		}
		/// <summary>
		/// Permission'a verilmiş ID'den farklı bir unique değer. Developer'lar tarafından verilir. Ilgili permission'ı
		/// unique olarak tanımlar. Permission tanımı bir kalıcı ortamdan bir diğerine taşındığında (örneğin DEV veri tabanından TEST veri tabanına kopyalandığında)
		/// ID değeri değişse bile bu değer sabit kalır.
		/// </summary>
		public string Code {
			get {
				return _code;
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentException("Peak.Auth.PxPermission Code property cannot be null or empty");
				}
				_code = value;
			}
		}

		/// <summary>
		/// Permission tanımı için verilmiş açıklayıcı bilgidir.
		/// </summary>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}
		
		/// <summary>
		/// Bir entity authorization aşamasından geçtiğinde kendisine yetki verilmiş / yetki reddedilmiş kaynaklar bir liste halinde kalıcı ortamdan (veri tabanı) alınır.
		/// Her bir kaynakla ilgili verilmiş/reddedilmiş yetkiler de teker teker kaynakların altına bir dictionary olarak konur. Bu property'nin değeri
		/// TRUE ise bağlı olduğu kaynak için bu yetki verilmiş (ALLOW) demektir. FALSE ise bağlı olduğu kaynak için bu yetki reddedilmiş (DENY) demektir.
		/// </summary>
		public bool IsGiven {
			get {
				return _isGiven;
			}
			set {
				_isGiven = value;
			}
		}

	}
}
