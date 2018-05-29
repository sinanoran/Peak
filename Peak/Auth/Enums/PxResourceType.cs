using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Auth.Enums {
	/// <summary>
	/// Sistemde tanımlanan resource'lara verilebilecek tipler
	/// </summary>
	public enum PxResourceType {


		/* 
		 
			Method <- CodeBlock
			Class <- CodeBlockcontainer
			
			-----



			 
			 */



		/// <summary>
		/// Execute edilebilecek kod bloklarını içeren yapılar. Örneğin bir servis (servis metodları içerir), örneğin bir controller (action metodları içerir...vb)
		/// </summary>
		CodeBlockContainer = 100,
		/// <summary>
		/// Execute edilebilecek herhangi bir kod bloğu. Örneğin bir MVC action ya da bir web servis metodu
		/// </summary>
		CodeBlock = 200,
		/// <summary>
		/// UI bileşenlerini içeren en üst yapılar. Örneğin bir desktop uygulamasında kullanıcıya gösterilen ekranlar, web uygulamalarında sayfalar...vb
		/// </summary>
		UITopLevelContainer = 300,
		/// <summary>
		/// UITopLevelContainer içerisinde olan diğer container yapılar. Örneğin bir tab, bir grid...vb
		/// </summary>
		UIContainer = 400,
		/// <summary>
		/// Herhangi bir UI elementi. Bir buton, scroll bar, web sahfalarında bir DIV...vb
		/// </summary>
		UIElement = 500,
		/// <summary>
		/// Bir menu tanımında en üstte yer alan menu bileşenleri. ÖRneğin bir windows formunda window üzerinde görünen menü başlıkları (File, Edit, View...vb)
		/// Örnek: MenuTopLevelContainer=File, MenuContainer=Open, MenuItem=Team Project...
		/// </summary>
		MenuTopLevelContainer = 600,
		/// <summary>
		/// Top level menu container altında yer alan, kendisi bir menu element olmayan, başka menu container ve menu element'leri içeren container
		/// Örnek: MenuTopLevelContainer=File, MenuContainer=Open, MenuItem=Team Project...
		/// </summary>
		MenuContainer = 700,
		/// <summary>
		/// Menulerde işlemlere bağlanmış menu elementleridir 
		/// Örnek: MenuTopLevelContainer=File, MenuContainer=Open, MenuItem=Team Project...
		/// </summary>
		MenuElement = 800
	}
}
