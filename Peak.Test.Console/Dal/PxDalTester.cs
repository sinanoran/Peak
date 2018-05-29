using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Test.Console.Dal {
  public class PxDalTester {
    public static void InitTest() {
      using (FwDbContext dbContext = new FwDbContext()) {
        List<SysStructure> structures = dbContext.Structures.Where(x => !x.CancelDate.HasValue).ToList();
      }

      using (LeasDbContext dbContext = new LeasDbContext()) {
        string sql = @" SELECT  CRMNO,
                                  TC_KIMLIK_NO,
                                  VRG_NO,
                                  DECODE(REF_CRM_TIP,
                                  3, AD || ' ' || SOYAD,
                                  2, UNVAN,
                                  1, DECODE(UNVAN,
                                                NULL,AD || ' ' || SOYAD,
                                                DECODE(AD || ' ' || SOYAD,
                                                        ' ',UNVAN,
                                                        AD || ' ' || SOYAD || '/' || UNVAN
                                                        )
                                            )
                                  ) AS AD_SOYAD_UNVAN
                          FROM CRM
                          WHERE IPT_TAR IS NULL 
                                AND (TC_KIMLIK_NO IS NOT NULL OR VRG_NO IS NOT NULL )";
        List<Crm> crms = dbContext.Database.SqlQuery<Crm>(sql).ToList();
      }
    }
  }
}
