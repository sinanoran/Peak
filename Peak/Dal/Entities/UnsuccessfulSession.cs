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
  [Table("OTURUM_BASARISIZ")]
  public class UnsuccessfulSession {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("OTURUMBASARISIZNO")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KULLANICINO")]
    public int UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IP")]
    public string Ip { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("TARAYICI_BILGI")]
    public string BrowserUserAgent { get; set; }

    [Column("TARIH")]
    public DateTime? Date { get; set; }

  }
}
