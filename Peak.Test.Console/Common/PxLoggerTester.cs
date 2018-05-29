using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using Peak.Dal;
using Peak.Dal.Entities;
using Peak.ErrorHandling;
using Peak.Logging;
using Microsoft.Extensions.Logging;

namespace Peak.Test.Console.Common {
  public class PxLoggerTester {
    public static void InitTest() {
      var platform= PxConfigurationManager.AppSettings["Platform"];

      ILogger logger = PxLoggerFactory.Instance.CreateLogger("LogError");
      logger.LogError("Configuration Test", new PxUnexpectedErrorException(new Exception("Test")));

      //LoggerFactory loggerFactory = new LoggerFactory();
      //loggerFactory.AddPxLogger(true);

      //using (PeakDbContext dbContext = new PeakDbContext()) {
      //  //Insert İşlemi
      //  User insertedUser = new User();
      //  insertedUser.Name = "Ali";
      //  insertedUser.Surname = "Veli";
      //  dbContext.Users.Add(insertedUser);

      //  //Update İşlemi
      //  User updateUser = dbContext.Users.FirstOrDefault(x => x.Id == 5);
      //  if (updateUser != null) {
      //    updateUser.MiddleName = "Ankara";
      //  }

      //  //Delete İşlemi
      //  User deleteUser = dbContext.Users.FirstOrDefault(x => x.Id == 4);
      //  dbContext.Users.Remove(deleteUser);

      //  //Select İşlemi
      //  List<string> activeUsers = (from a in dbContext.ActiveSessions
      //                              join u in dbContext.Users on a.UserId equals u.Id
      //                              where u.CancelDate == null
      //                              select u.Fullname).ToList();

      //  //Değişikliklerin veri tabanına aktarıldığı metod
      //  dbContext.SaveChanges();
      //}
    }
  }
}
