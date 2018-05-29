using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Test.Console.Dal {
  [Table("SYS_YAPI")]
  public class SysStructure {

    #region Private Member(s)

    private int itemId;
    private AuthorizableItemType itemType;
    private long hashId;
    private string xmlData;
    private object itemObject;

    #endregion

    [Key]
    [Column("SYSYAPINO")]
    public int Id { get; set; }

    [Column("UST_SYSYAPINO")]
    public int? ParentId { get; set; }

    [Column("OGE_TIP")]
    public AuthorizableItemType ItemType {
      get { return itemType; }
      set {
        itemType = value;
        hashId = AuthorizableItem.GetAuthorizableHashId(itemId, (int)itemType);
      }
    }

    [Column("OGE_NO")]
    /// <summary>
    /// 
    /// </summary>
    public int ItemId {
      get { return itemId; }
      set {
        itemId = value;
        hashId = AuthorizableItem.GetAuthorizableHashId(itemId, (int)itemType);
      }
    }

    [Column("OGE_VERI")]
    public string XmlData { get; set; }

    [Column("OGE_ORJ")]
    public bool IsOriginal { get; set; }

    [Column("IPT_TAR")]
    public DateTime? CancelDate { get; set; }
  }

  public enum AuthorizableItemType {
    Module = 1,
    Menu = 2,
    Control = 3,
    Service = 4,
    Method = 5,
    Form = 6,
    Report = 7,
    Gadget = 8,
    WebForm = 9
  }

  public class AuthorizableItem {
    public static long GetAuthorizableHashId(int authorizableId, int type) {
      return (long)authorizableId * 100 + type;
    }
  }

}
