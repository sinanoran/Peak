using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Dal.Entities {

  [Table("PARAMETRE")]
  public class Parameter : EntityBase {

    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("PARAMETRENO")]
    public override int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("GRUP")]
    public string Group { get; set; }

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
    /// 
    /// </summary>
    [Column("DEGER")]
    public string Value { get; set; }

  }
}
