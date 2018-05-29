using Peak.Auth.Models;
using Peak.Configuration;
using Peak.Dal;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Peak.Dal.Entities;

namespace Peak.Auth {
	public class PxAuthorization : IPxAuthorization {

		public PxAuthorizationInfo Authorize(int pUserId) {

			PxAuthorizationInfo auth = null;
			using (PeakDbContext db = new PeakDbContext()) {
				OracleParameter param1 = new OracleParameter("pKullaniciNo", OracleDbType.Int32, System.Data.ParameterDirection.Input);
				OracleParameter param2 = new OracleParameter("RC_AUTH", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
				param1.Value = pUserId;

				DbRawSqlQuery<Authorization> a = db.Database.SqlQuery<Authorization>("begin PKG_AUTH.P_GET_AUTHORIZATION(:pKullaniciNo, :RC_AUTH); end;", param1, param2);
				List<Authorization> result = a.ToList<Authorization>();

				auth = this.getPxAuthorizationInfo(result);
			}

			return auth;
		}



		public PxAuthorizationInfo GetMenu(int pUserId) {
			PxAuthorizationInfo menu = null;
			using (PeakDbContext db = new PeakDbContext()) {
				OracleParameter param1 = new OracleParameter("pKullaniciNo", OracleDbType.Int32, System.Data.ParameterDirection.Input);
				OracleParameter param2 = new OracleParameter("RC_MENU", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
				param1.Value = pUserId;


				DbRawSqlQuery<Authorization> a = db.Database.SqlQuery<Authorization>("begin PKG_AUTH.P_GET_MENU(:pKullaniciNo, :RC_MENU); end;", param1, param2);
				List<Authorization> result = a.ToList<Authorization>();
				menu = this.getPxAuthorizationInfo(result);
			}

			return menu;
		}



		private PxAuthorizationInfo getPxAuthorizationInfo(List<Authorization> pData) {
			PxAuthorizationInfo result = new PxAuthorizationInfo();
			long tailYetkiNo = 0;
			string tailYetkiKod = null;
			int tailIzin = 0;

			foreach (Authorization a in pData) {
				PxResourceInfo resource = result.GetResource(a.VARLIKKOD);
				if (resource == null) {
					resource = this.createNewResource(a, ref tailYetkiNo, ref tailYetkiKod, ref tailIzin);
					result.AddResource(resource);
				}
				else {
					PxPermissionInfo perm = resource.GetPermission(string.IsNullOrEmpty(a.YETKIKOD) ? tailYetkiKod : a.YETKIKOD);
					if (perm == null) {
						perm = this.createNewPermission(a, ref tailYetkiNo, ref tailYetkiKod, ref tailIzin);
					}
					resource.AddPermission(perm);
				}
			}

			return result;

		}
		private PxResourceInfo createNewResource(Authorization pAuth, ref long pTailYetkiNo, ref string pTailYetkiKod, ref int pTailIzin) {
			PxResourceInfo resource = new PxResourceInfo();

			resource = new PxResourceInfo();
			resource.Code = pAuth.VARLIKKOD;
			resource.ID = pAuth.VARLIKNO;
			resource.Name = pAuth.VARLIKAD;
			resource.ParentID = pAuth.UST_VARLIKNO;
			resource.Permissions = new Dictionary<string, PxPermissionInfo>();


			PxPermissionInfo perm = this.createNewPermission(pAuth, ref pTailYetkiNo, ref pTailYetkiKod, ref pTailIzin);

			resource.AddPermission(perm);

			return resource;
		}
		private PxPermissionInfo createNewPermission(Authorization pAuth, ref long pTailYetkiNo, ref string pTailYetkiKod, ref int pTailIzin) {
			/*Oracle'dan gelen yetkilerde ilk yetki mutlaka üst bir node'dur ve yetki tanımı vardır.
			 Çünkü ilgili select ifadesi yetkiye göre resource'ları alıp getirmektedir ve tepe node'ların yetkisi daima
			 olmak zorundadır. Aksi takdirde tail değişkenleri doludur. */
			PxPermissionInfo perm = new PxPermissionInfo();
			if (pAuth.YETKINO.HasValue) {
				perm.ID = pAuth.YETKINO.Value;
				pTailYetkiNo = pAuth.YETKINO.Value;
			}
			else {
				perm.ID = pTailYetkiNo;
			}


			if (!string.IsNullOrEmpty(pAuth.YETKIKOD)) {
				perm.Code = pAuth.YETKIKOD;
				pTailYetkiKod = pAuth.YETKIKOD;
			}
			else {
				perm.Code = pTailYetkiKod;
			}


			if (pAuth.IZIN > 0) {
				perm.IsGiven = true;
				pTailIzin = pAuth.IZIN.Value;
			}
			else if (pAuth.IZIN < 0) {
				perm.IsGiven = false;
				pTailIzin = pAuth.IZIN.Value;
			}
			else { //0 geliyorsa izin üstten alınır
				if (pTailIzin > 0) {
					perm.IsGiven = true;
				}
				else if (pTailIzin < 0) {
					perm.IsGiven = false;
				}
				else {
					//Hem üst node'dan gelen hem de resource tanımından gelen izin tanımının boş olması mümkün değil. Bir hata olmalı!
					throw new InvalidOperationException("Both resource permission and rail permission cannot be zero. Please check resource-role-permission tree");
				}
			}

			return perm;

		}

	}
}
