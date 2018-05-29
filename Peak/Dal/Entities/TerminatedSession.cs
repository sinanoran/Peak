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
  [Table("OTURUM_KAPALI")]
  public class TerminatedSession {

    /// <summary>
    /// 
    /// </summary>
    public TerminatedSession() {

    }

    /// <summary>
    /// 
    /// </summary>
    public TerminatedSession(ActiveSession activeSession) {
      this.Ip = activeSession.Ip;
      this.OpenDate = activeSession.OpenDate;
      this.SessionKey = activeSession.SessionKey;
      this.BrowserUserAgent = activeSession.BrowserUserAgent;
      this.UserId = activeSession.UserId;
    }

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("OTURUMKAPALINO")]
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
    [Column("KAPANIS_ZAMANI")]
    public DateTime TerminationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KAPANIS_TIPI")]
    public SessionTerminationType TerminationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KAPANIS_KULLANICINO")]
    public int CloserUserId { get; set; }

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
