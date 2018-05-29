using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Models {
	public class PxCredentialInfo {

		public string UserName { get; set; }

		/// <summary>
		/// Bu alanda kullanıcının hash fonksiyonundan geçirilmiş şifresi bulunur. Login için girilen şifre bilgisi açık olarak bulundurulmaz!
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string HostName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string OperatingSystemUser { get; set; }

    public void Clear() {
      this.UserName = string.Empty;
      this.Password = string.Empty;
      this.HostName = string.Empty;
      this.OperatingSystemUser = string.Empty;
    }

	}
}
