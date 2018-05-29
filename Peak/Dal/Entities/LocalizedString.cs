using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Dal.Entities {
  /// <summary>
  /// 
  /// </summary>
  [Table("LOKALIZASYON")]
  public class LocalizedString {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("LOKALIZASYONNO")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KOD")]
    public string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("DIL_KOD")]
    public string CultureCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("DEGER")]
    public string Value { get; set; }

  }
}
