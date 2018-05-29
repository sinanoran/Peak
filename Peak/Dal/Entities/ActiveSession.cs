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
  [Table("OTURUM_ACIK")]
  public class ActiveSession {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("OTURUMACIKNO")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("OTURUM_BILET")]
    public string SessionKey { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("ACILIS_ZAMANI")]
    public DateTime OpenDate { get; set; }

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
  }
}
