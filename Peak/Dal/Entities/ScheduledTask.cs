using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Peak.Dal.Enums;

namespace Peak.Dal.Entities {
  /// <summary>
  /// 
  /// </summary>
  [Table("ZAMANLANMIS_GOREV")]
  public class ScheduledTask : EntityBase {
    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("ZAMANLANMISGOREVNO")]
    public override int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("AD")]
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("ACK")]
    public string Description { get; set; }

    /// <summary>
    /// Çalıştırılacak sınıfın assembly qualified name
    /// </summary>
    [Column("AKSIYON")]
    public string Action { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("CRON_FORMAT")]
    public string CronExpression{ get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("BAS_TAR")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("BIT_TAR")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("CALISMAMIS_IS_PLAN")]
    public MissedScheduleActionType MissedScheduleAction { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SADECE_IS_GUNLERI")]
    public bool OnlyWorkingDays { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("YARIM_GUN_CALISACAK")]
    public bool RunAtHalfPublicHoliday { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SUNUCU_AD")]
    public string ServerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("HATA_DENEME_SAYI")]
    public int FailedTryCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("HATA_DENEME_ZAMAN_ARALIK")]
    public int FailedRetryInterval { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("AKTIF")]
    public bool Enabled { get; set; }   
  }
}
