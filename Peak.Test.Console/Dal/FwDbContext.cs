using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Peak.Dal;

namespace Peak.Test.Console.Dal {
  public class FwDbContext : PxDbContext {
    public FwDbContext() : base("FW", "FW") { }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<SysStructure> Structures { get; set; }

  }
}
