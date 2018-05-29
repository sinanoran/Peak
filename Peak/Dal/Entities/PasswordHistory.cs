using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Dal.Entities {

  [Table("SIFRE_TARIHCE")]
  public class PasswordHistory {

    [Key]
    [Column("SIFRETARIHCENO")]
    public int Id { get; set; }

    [Column("KULLANICINO")]
    public int UserId { get; set; }

    [Column("SIFRE")]
    public string Password { get; set; }

    [Column("TAR")]
    public DateTime Date { get; set; }
  }
}
