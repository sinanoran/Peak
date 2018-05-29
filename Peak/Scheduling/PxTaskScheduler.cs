using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Peak.Common;
using Peak.Configuration;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Dal.Enums;
using Quartz;
using Quartz.Impl;

namespace Peak.Scheduling {

  /// <summary>
  /// Quartz kullanarak ZAMANLANMIG_GOREV tablosunda bulunan görevleri zamanlayan sınıftır. 
  /// GlobalInitializationManager sınıfı run metodunu çağırmaktadır.
  /// </summary>
  public class PxTaskScheduler : SingletonBase<PxTaskScheduler> {

    #region Private Member(s)

    private IScheduler _scheduler;
    private Timer _taksListRefreshTimer;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public PxTaskScheduler() {
      _scheduler = new StdSchedulerFactory().GetScheduler();
      _taksListRefreshTimer = new Timer(PxConfigurationManager.PxConfig.Schedule.RefreshInterval * 60 * 1000);
      _taksListRefreshTimer.Elapsed += taksListRefreshTimer_Elapsed;
    }

    #endregion

    #region Private Method(s)

    private void scheduleAllTaks() {
      using (PeakDbContext dbContext = new PeakDbContext()) {
        var scheduledTasks = dbContext.ScheduledTasks.Where(x => x.CancelDate == null).ToList();
        if (scheduledTasks == null || scheduledTasks.Count == 0) {
          return;
        }
        foreach (var task in scheduledTasks) {
          scheduleTask(task);
        }
      }
    }

    private void scheduleTask(ScheduledTask task) {
      if (!string.IsNullOrEmpty(task.ServerName) && task.ServerName.ToLower(CultureInfo.GetCultureInfo("en-US")) != System.Environment.MachineName.ToLower(CultureInfo.GetCultureInfo("en-US"))) {
        return;
      }
      JobKey jobKey = new JobKey(task.Name);
      if (!task.Enabled) {
        if (_scheduler.CheckExists(jobKey)) {
          _scheduler.DeleteJob(jobKey);
        }
        return;
      }
      TriggerKey triggerkey = new TriggerKey(string.Format("{0}_trigger", task.Name));
      TriggerBuilder triggerBuilder = null;
      if (!_scheduler.CheckExists(triggerkey)) {
        triggerBuilder = TriggerBuilder.Create().WithIdentity(triggerkey);
      }
      else {
        ICronTrigger oldTrigger = (ICronTrigger)_scheduler.GetTrigger(triggerkey);
        if (oldTrigger.CronExpressionString == task.CronExpression) {
          return;
        }
        triggerBuilder = oldTrigger.GetTriggerBuilder();
      }

      TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(PxConfigurationManager.PxConfig.Schedule.TimeZoneId);
      switch (task.MissedScheduleAction) {
        case MissedScheduleActionType.RunLatest:
          triggerBuilder = triggerBuilder.WithCronSchedule(task.CronExpression, x => x.InTimeZone(timeZoneInfo).WithMisfireHandlingInstructionFireAndProceed());
          break;
        case MissedScheduleActionType.RunAll:
          triggerBuilder = triggerBuilder.WithCronSchedule(task.CronExpression, x => x.InTimeZone(timeZoneInfo).WithMisfireHandlingInstructionIgnoreMisfires());
          break;
        case MissedScheduleActionType.NoAction:
        default:
          triggerBuilder = triggerBuilder.WithCronSchedule(task.CronExpression, x => x.InTimeZone(timeZoneInfo).WithMisfireHandlingInstructionDoNothing());
          break;
      }

      if (task.StartDate != null && (DateTime)task.StartDate != DateTime.MinValue) {
        triggerBuilder = triggerBuilder.StartAt(new DateTimeOffset(task.StartDate.Value));
      }
      else {
        triggerBuilder = triggerBuilder.StartAt(new DateTimeOffset(DateTime.Now));
      }
      if (task.EndDate != null && (DateTime)task.EndDate != DateTime.MinValue) {
        triggerBuilder = triggerBuilder.EndAt(new DateTimeOffset((DateTime)task.EndDate));
      }

      IJobDetail job = JobBuilder.Create<PxScheduledTask>().WithIdentity(jobKey).RequestRecovery().Build();
      job.JobDataMap.Put("ScheduledTask", task);
      ICronTrigger trigger = (ICronTrigger)triggerBuilder.ForJob(job).Build();

      if (!_scheduler.CheckExists(triggerkey)) {
        _scheduler.ScheduleJob(job, trigger);
      }
      else {
        _scheduler.RescheduleJob(triggerkey, trigger);
      }

    }

    private void taksListRefreshTimer_Elapsed(object sender, ElapsedEventArgs e) {
      _taksListRefreshTimer.Stop();
      scheduleAllTaks();
      _taksListRefreshTimer.Start();
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// 
    /// </summary>
    public void Start() {
      scheduleAllTaks();
      _taksListRefreshTimer.Start();
      _scheduler.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Stop() {
      _taksListRefreshTimer.Stop();
      _scheduler.Shutdown();
    }

    #endregion

  }
}
