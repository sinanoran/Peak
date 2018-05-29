using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using Peak.Configuration;
using Peak.ErrorHandling;
using Peak.Session;


namespace Peak.Dal {

  /// <summary>
  /// Base DbContext sıfıdır. Diğer veri tabanı contextlerinin türemesi gereken sınıftır. 
  /// EntityFramewok DbContext'den türemektedir.
  /// </summary> 
  public class PxDbContext : DbContext {

    #region Private Member(s)

    private string _defaultSchema;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionStringName"></param>
    /// <param name="defaultSchema"></param>
    public PxDbContext(string connectionStringName, string defaultSchema) : base(connectionStringName) {
      this.Configuration.ProxyCreationEnabled = false;
      this.Configuration.LazyLoadingEnabled = false;
      _defaultSchema = defaultSchema;
    }

    #endregion

    #region Overriden Method(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(_defaultSchema);
      base.OnModelCreating(modelBuilder);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges() {
      try {
        var entities = ChangeTracker.Entries<EntityBase>();
        DateTime originalModifyTimeStamp;
        PxSession session = PxSession.Get();
        foreach (var entity in entities) {
          switch (entity.State) {
            case EntityState.Added:
              entity.Entity.CreateDate = DateTime.Now;
              entity.Entity.CreateUserIp = session.Client.IPAddress;
              entity.Entity.CreateUserId = session.Principal.UserId;
              entity.Entity.ModifyTimeStamp = DateTime.Now;
              break;
            case EntityState.Detached:
            case EntityState.Deleted:
              originalModifyTimeStamp = entity.GetDatabaseValues().GetValue<DateTime>("ModifyTimeStamp");
              if (originalModifyTimeStamp != entity.Entity.ModifyTimeStamp) {
                throw new DbUpdateConcurrencyException("Could not be saved successfully. Record may have been changed or canceled.");
              }
              entity.State = EntityState.Modified;
              entity.Entity.CancelDate = DateTime.Now;
              entity.Entity.CancelUserIp = session.Client.IPAddress;
              entity.Entity.CancelUserId = session.Principal.UserId;
              entity.Entity.ModifyTimeStamp = DateTime.Now;
              break;
            case EntityState.Modified:
              originalModifyTimeStamp = entity.GetDatabaseValues().GetValue<DateTime>("ModifyTimeStamp");
              if (originalModifyTimeStamp != entity.Entity.ModifyTimeStamp) {
                throw new DbUpdateConcurrencyException("Could not be saved successfully. Record may have been changed or canceled.");
              }
              entity.Entity.ModifyTimeStamp = DateTime.Now;
              break;
            case EntityState.Unchanged:
            default:
              break;
          }
        }
        return base.SaveChanges();
      }
      catch (DbEntityValidationException e) {
        StringBuilder errorBuilder = new StringBuilder();
        foreach (var eve in e.EntityValidationErrors) {
          errorBuilder.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
          foreach (var ve in eve.ValidationErrors) {
            errorBuilder.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
          }
          errorBuilder.AppendLine();
        }
        throw new PxException(DalErrorCodes.DbEntityValidationException, Common.Enums.ErrorPriority.High, errorBuilder.ToString());
      }
    }

    #endregion
  
  }
}
