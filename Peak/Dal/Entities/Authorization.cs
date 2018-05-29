using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Dal.Entities {

	[Table("RC_AUTH")]
	public class Authorization {

		[Column("VARLIKNO")]
		public long VARLIKNO { get; set; }

		[Column("VARLIKKOD")]
		public string VARLIKKOD { get; set; }

		[Column("AD")]
		public string VARLIKAD { get; set; }

		[Column("UST_VARLIKNO")]
		public long? UST_VARLIKNO { get; set; }

		public long? ROLNO { get; set; }
		public string ROLKOD { get; set; }
		public long? YETKINO { get; set; }
		public string YETKIKOD { get; set; }
		public int? IZIN { get; set; }
		public int? DAIRESEL { get; set; }
		public int? SIRANO { get; set; }

		public int VARLIKTURNO { get; set; }
		public string VARLIKTURKOD { get; set; }


	}
}
