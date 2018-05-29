using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Peak.Dal;

namespace Peak.Test.Console.Dal {

  public class LeasDbContext : PxDbContext {
    public LeasDbContext() : base("LEAS", "LEAS") { }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<Crm> Crms { get; set; }

  }
}
