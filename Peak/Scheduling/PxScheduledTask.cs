using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Peak.Common;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Logging;
using Peak.Logging.Loggers;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Peak.Scheduling {
  /// <summary>
  /// Scheduler için Quartz kullanılmaktadır. Quartzın çalıştırdığı IJob interfacesinin default implementasyonudur.
  /// </summary>
  public class PxScheduledTask : IJob {

    #region Private Member(s)

    private IPxTaskAction _action;
    private ScheduledTask _task;
    private System.Exception _lastError;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public PxScheduledTask() {
    }

    #endregion

    #region Private Method(s)

    private bool executeAction(DateTime planingTime, int retry) {
      if (retry > _task.FailedTryCount) {
        return false;
      }

      if (retry > 0 && _task.FailedRetryInterval > 0) {
        Thread.Sleep(_task.FailedRetryInterval * 60 * 1000);
      }

      try {
        this._action.Excecute(planingTime);
        this._lastError = null;
        return true;
      }
      catch (System.Exception ex) {
        this._lastError = ex;
        return this.executeAction(planingTime, ++retry);
      }
    }

    #endregion

    #region IJob Member(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Execute(IJobExecutionContext context) {
      try {
        _task = (ScheduledTask)context.MergedJobDataMap["ScheduledTask"];
        DateTime planingTime = context.ScheduledFireTimeUtc.HasValue ? context.ScheduledFireTimeUtc.Value.LocalDateTime : DateTime.Now;
        planingTime = new DateTime(planingTime.Ticks - (planingTime.Ticks % TimeSpan.TicksPerMinute), planingTime.Kind);
        if (_task.OnlyWorkingDays) {
          if (HolidayCalculator.Instance.IsHoliday(planingTime, _task.RunAtHalfPublicHoliday)) {
            return;
          }
        }
        _action = Toolkit.Instance.CreateInstance<IPxTaskAction>(_task.Action);
        using (PeakDbContext dbContext = new PeakDbContext()) {
          if (dbContext.ScheduledTaskLogs.FirstOrDefault(x => x.ScheduledTaskId == _task.Id && x.ScheduledTime == planingTime) != null) {
            return;
          }
          ScheduledTaskLog taskLog = new ScheduledTaskLog()
          {
            ScheduledTaskId = _task.Id,
            State = Dal.Enums.ScheduleState.Running,
            ScheduledTime = planingTime,
            StartTime = DateTime.Now
          };
          dbContext.ScheduledTaskLogs.Add(taskLog);
          dbContext.SaveChanges();
          bool succeed = executeAction(planingTime, 0);
          taskLog.EndTime = DateTime.Now;
          if (succeed) {
            taskLog.State = Dal.Enums.ScheduleState.Succeed;
          }
          else {
            taskLog.State = Dal.Enums.ScheduleState.Failed;
            if (_lastError != null) {
              taskLog.ErrorMessage = _lastError.Message;
              taskLog.ErrorDetail = _lastError.ToString();
              _lastError = null;
            }
          }
          dbContext.SaveChanges();
        }
      }
      catch (Exception ex) {
        PxErrorLogger.Log(ex);
      }
    }

    #endregion
  }
}
