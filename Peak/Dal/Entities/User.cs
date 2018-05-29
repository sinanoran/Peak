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
  [Table("KULLANICI")]
  public class User : EntityBase {

    /// <summary>
    /// 
    /// </summary>
    public User() {
      this.PasswordChangeDate = DateTime.Now;
      this.IsPwdMustChange = true;
      this.StartDate = DateTime.Now;
      this.PasswordNeverExpires = false;
      this.PasswordState = PasswordState.Active;
      this.ModifyTimeStamp = DateTime.Now;
      this.Type = UserType.Standart;
    }

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("KULLANICINO")]
    public override int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("KOD")]
    public string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE")]
    public string Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("TIP")]
    public UserType Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("AD")]    
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IKINCI_AD")]
    public string MiddleName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SOYAD")]
    public string Surname { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("DIL_KOD")]
    public string CultureCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public string Fullname {
      get { return string.Format("{0} {1} {2}", Name, MiddleName, Surname); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Column("EPOSTA")]
    public string Email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("TELEFON")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("BAS_TAR")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("BIT_TAR")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_DEGISIM_TAR")]
    public DateTime PasswordChangeDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_DURUM")]
    public PasswordState PasswordState { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_DENEME_SAYISI")]
    public short PasswordTryCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_DEGISIM_GEREKLI")]
    public bool IsPwdMustChange { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("MFA_DENEME_SAYISI")]
    public short MFATryCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_HEP_GECERLI")]
    public bool PasswordNeverExpires { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("PROFIL_RESMI")]
    public byte[] Image { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("SIFRE_SALT")]
    public string PasswordSalt { get; set; }
  }
}
