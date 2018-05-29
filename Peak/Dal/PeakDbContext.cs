using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using Peak.Dal.Entities;

namespace Peak.Dal {

  /// <summary>
  /// PxDbContext'in Peak'in sahip olduğu tablolar için kullandığı veri tabanı kontexidir.
  /// </summary>
  public class PeakDbContext : PxDbContext {

    /// <summary>
    /// 
    /// </summary>
    public PeakDbContext() : base("AX", PxConfigurationManager.PxConfig.Dal.PeakDbSchema) {
    }


    /// <summary>
    /// 
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<ActiveSession> ActiveSessions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<UnsuccessfulSession> UnsuccessfulSessions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<TerminatedSession> TerminatedSessions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<MFAMessage> MFAMessages { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<LocalizedString> LocalizedStrings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<ScheduledTask> ScheduledTasks { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<ScheduledTaskLog> ScheduledTaskLogs { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<Parameter> Parameters { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<PasswordHistory> PasswordHistories { get; set; }
  }
}
