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
  [Table("MFA_MESAJ")]
  public class MFAMessage {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("MFAMESAJNO")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("TARIH")]
    public DateTime Date { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KULLANICINO")]
    public int? UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("CEP_TEL")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KOD")]
    public string VerificationCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("REFERANSNO")]
    public string RereferenceCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KULLANILDI")]
    public bool IsUsed { get; set; }
  }
}
