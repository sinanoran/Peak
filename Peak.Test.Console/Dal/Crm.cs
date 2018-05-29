using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Test.Console.Dal {
  
  public class Crm {

    [Key]
    [Column("CRMNO")]
    public int Id { get; set; }

    [Column("TC_KIMLIK_NO")]
    public string TcKimlikNo { get; set; }

    [Column("VRG_NO")]
    public string VergiNo { get; set; }

    [Column("AD_SOYAD_UNVAN")]
    public string AdSoyadUnvan { get; set; }
  }
}
