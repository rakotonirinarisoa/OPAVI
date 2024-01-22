using apptab;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOFTOPAVI.Controllers
{
	public class DBaseTOMController : Controller
	{
		private readonly OPAVIWEB db = new OPAVIWEB();

		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		public ActionResult DBaseTOMCreate()
		{
			return View();
		}

		[HttpPost]
		public ActionResult GetInstance(OPA_USERS suser)
		{
			var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
			if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));


			var instance = db.OPA_DATABASE.Where(u => u.IDUSER == exist.ID).Join(db.OPA_MAPPAGES, x => x.IDMAPPAGE, y => y.ID, (x, y) => new
			{
				ID = y.ID,
				INSTANCE = y.INSTANCE,
				TYPE = x.TYPE
			}).FirstOrDefault();

			var instances = db.OPA_DROITS.Where(a => a.IDUSER == exist.ID && a.IDSOCIETE == exist.IDSOCIETE).Join(db.OPA_MAPPAGES, x => x.IDMAPPAGE, y => y.ID, (x, y) => y).DistinctBy(y => y.INSTANCE).Select(x => new
			{
				ID = x.ID,
				INSTANCE = x.INSTANCE,
			}).ToList();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { actual = instance, list = instances } }, settings));
		}

		[HttpPost]
		public ActionResult GetBase(OPA_USERS suser, string instance)
		{
			var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
			if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

			//var instance = db.OPA_MAPPAGES.Where(x=>x.ID == instanceID).Select(x=>x.INSTANCE).FirstOrDefault();


			var bddTom = db.OPA_DROITS.Where(a => a.IDUSER == exist.ID && a.IDSOCIETE == exist.IDSOCIETE).Join(db.OPA_MAPPAGES, x => x.IDMAPPAGE, y => y.ID, (x, y) => y).Where(y => y.INSTANCE == instance).DistinctBy(y => y.DBASE).Select(x => new
			{
				ID = x.ID,
				DBASE = x.DBASE,
			}).ToList();

			var mybdd = db.OPA_DATABASE.Where(a => a.IDUSER == exist.ID && a.IDSOCIETE == exist.IDSOCIETE).Join(db.OPA_MAPPAGES, x => x.IDMAPPAGE, y => y.ID, (x, y) => y).Select(y => y.ID).FirstOrDefault();
			/*
             * var bddTom = db.OPA_MAPPAGES.Where(a => a.IDSOCIETE == suser.IDSOCIETE && a.INSTANCE == instance).DistinctBy(a => a.DBASE).Select(a => new
            {
                DBASE = a.DBASE,
                ID = a.ID
            }).ToList();
            */
			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { db = bddTom, mydb = mybdd } }, settings));
		}

		[HttpPost]
		public ActionResult Save(OPA_USERS suser, int bdd, int type)
		{
			var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
			if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

			//var instance = db.OPA_MAPPAGES.Where(x=>x.ID == instanceID).Select(x=>x.INSTANCE).FirstOrDefault();

			var bddTom = db.OPA_DATABASE.Where(a => a.IDUSER == exist.ID && a.IDSOCIETE == exist.IDSOCIETE).FirstOrDefault();
			if (bddTom == null)
			{
				var elem = new OPA_DATABASE()
				{
					IDMAPPAGE = bdd,
					TYPE = type,
					IDSOCIETE = exist.IDSOCIETE,
					IDUSER = exist.ID
				};

				db.OPA_DATABASE.Add(elem);
				db.SaveChanges();
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message" }, settings));
			}

			bddTom.IDMAPPAGE = bdd;
			bddTom.TYPE = type;
			db.SaveChanges();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message" }, settings));
		}


	}
}