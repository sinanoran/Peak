using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Peak.Dal {

  /// <summary>
  /// 
  /// </summary>
  public abstract class EntityBase {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    public abstract int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("YRT_TAR")]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("YRT_IP")]
    public string CreateUserIp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("YRT_KULLANICINO")]
    public int CreateUserId { get; set; } 

    /// <summary>
    /// 
    /// </summary>
    [Column("IPT_TAR")]
    public DateTime? CancelDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IPT_KULLANICINO")]
    public int? CancelUserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IPT_IP")]
    public string CancelUserIp { get; set; }
    

    /// <summary>
    /// 
    /// </summary>
    [Column("KYT_KONTROL")]
    public DateTime ModifyTimeStamp { get; set; }
  }
}
