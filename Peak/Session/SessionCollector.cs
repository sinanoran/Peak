using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.Scheduling;

namespace Peak.Session {
  public class SessionCollector : IPxTaskAction {

    public void Excecute(DateTime scheduledTime) {
      using (PeakDbContext dbContext = new PeakDbContext()) {
        DateTime expiredTime = DateTime.Now.AddMinutes(-PxConfigurationManager.PxConfig.Session.DefaultExpireDuration);
        var expiredSessions = dbContext.ActiveSessions.Where(x => x.OpenDate <= expiredTime).ToList();
        dbContext.ActiveSessions.RemoveRange(expiredSessions);
        foreach (var expiredSession in expiredSessions) {
          TerminatedSession terminatedSession = new TerminatedSession()
          {
            Ip = expiredSession.Ip,
            OpenDate = expiredSession.OpenDate,
            TerminationDate = DateTime.Now,
            SessionKey = expiredSession.SessionKey,
            TerminationType = Dal.Enums.SessionTerminationType.Expire,
            BrowserUserAgent = expiredSession.BrowserUserAgent,
            UserId = expiredSession.UserId
          };
          dbContext.TerminatedSessions.Add(terminatedSession);
        }
        dbContext.SaveChanges();
      }
    }
  }
}
