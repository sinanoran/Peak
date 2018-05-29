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
  [Table("LOG_ZAMANLANMIS_GOREV")]
  public class ScheduledTaskLog {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("LOGZAMANLANMISGOREVNO")]
    public int Id { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [Column("ZAMANLANMISGOREVNO")]
    public int ScheduledTaskId { get; set; }

    /// <summary>
    /// Planlanan zaman
    /// </summary>
    [Column("PLAN_ZAMAN")]
    public DateTime ScheduledTime { get; set; }

    /// <summary>
    /// Görevin fiilen başladığı zaman
    /// </summary>
    [Column("BASLANGIC_ZAMAN")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Görevin fiilen bittiği zaman
    /// </summary>
    [Column("BITIS_ZAMAN")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Görevin durumu
    /// </summary>
    [Column("DURUM")]
    public ScheduleState State { get; set; }

    /// <summary>
    /// Eğer görev çalıştırılırken hata oluşmuşsa, oluşan hatanın mesajı.
    /// </summary>
    [Column("HATA_MESAJ")]
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Eğer görev çalıştırılırken hata oluşmuşsa, oluşan hatanın Stack Trace'i
    /// </summary>
    [Column("HATA_DETAY")]
    public string ErrorDetail { get; set; }

  }
}
