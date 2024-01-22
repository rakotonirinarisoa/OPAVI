using apptab;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SOFTOPAVI.Controllers
{
    public class AdminController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult AdminMaPUserList()
        {
            ViewBag.Controller = "Droit des Utilisateurs";

            return View();
        }

        [HttpPost]
        public JsonResult FillTable(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var droits = db.OPA_DROITS.Where(a => a.IDSOCIETE == suser.IDSOCIETE).Select(a => new
                {
                    USER = db.OPA_USERS.FirstOrDefault(x => x.ID == a.IDUSER).LOGIN,
                    INSTANCE = db.OPA_MAPPAGES.FirstOrDefault(x => x.ID == a.IDMAPPAGE).INSTANCE,
                    DBASE = db.OPA_MAPPAGES.FirstOrDefault(x => x.ID == a.IDMAPPAGE).DBASE,
                    ID = a.ID,
					SOCIETE = db.OPA_SOCIETES.Where(z => z.ID == suser.IDSOCIETE).FirstOrDefault().SOCIETE
				}).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = droits }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult DeleteDroit(OPA_USERS suser, string DroitId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int droitID = int.Parse(DroitId);
                var droit = db.OPA_DROITS.FirstOrDefault(a => a.ID == droitID);
                if (droit != null)
                {
                    db.OPA_DROITS.Remove(droit);
                    db.SaveChanges();
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Suppression avec succès. " }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "message" }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        public ActionResult AdminMaPUserCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminMaPUserCreate(OPA_USERS suser, OPA_DROITS droit)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var userExist = db.OPA_DROITS.FirstOrDefault(a => a.IDUSER == droit.IDUSER && a.IDSOCIETE == suser.IDSOCIETE && a.IDMAPPAGE == droit.IDMAPPAGE);
                if (userExist == null)
                {
                    var newDroit = new OPA_DROITS()
                    {
                        IDUSER = droit.IDUSER,
                        IDMAPPAGE = droit.IDMAPPAGE,
                        IDSOCIETE = suser.IDSOCIETE
                    };
                    db.OPA_DROITS.Add(newDroit);
                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = droit }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le droit de l'utilisateur existe déjà pour l'instance et la base de données sélectionées. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public ActionResult GetAllUser(OPA_USERS suser)
        {
            var userR = db.OPA_USERS;
            var user = db.OPA_USERS.Where(a => a.IDSOCIETE == suser.IDSOCIETE).DistinctBy(a => a.LOGIN).Select(a => new
            {
                LOGIN = a.LOGIN,
                ID = a.ID
            }).ToList();

            var instance = db.OPA_MAPPAGES.Where(a => a.IDSOCIETE == suser.IDSOCIETE).DistinctBy(a => a.INSTANCE).Select(a => new
            {
                INSTANCE = a.INSTANCE,
                ID = a.ID
            }).ToList();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { USER = user, INSTANCE = instance } }, settings));
        }

        [HttpPost]
        public ActionResult GetDB(OPA_USERS suser, string instanceID)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var instance = db.OPA_MAPPAGES.Where(a => a.IDSOCIETE == suser.IDSOCIETE && a.INSTANCE == instanceID).DistinctBy(a => a.DBASE).Select(a => new
            {
                DBASE = a.DBASE,
                ID = a.ID
            }).ToList();


            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = instance }, settings));
        }

        public ActionResult DetailsUser(string UserId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailsUser(OPA_USERS suser, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var user = db.OPA_DROITS.FirstOrDefault(a => a.ID == useID);

                if (user != null)
                {
                    var map = db.OPA_MAPPAGES.FirstOrDefault(a => a.ID == user.IDMAPPAGE);
                    var inst = map.INSTANCE;
                    var basee = map.DBASE;
                    var id = map.ID;

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { USER = user.IDUSER, INSTANCE = inst, BASED = basee, id = id } }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "message" }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateUser(OPA_USERS suser, OPA_DROITS droit, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int userId = int.Parse(UserId);
                var userExist = db.OPA_DROITS.FirstOrDefault(a => a.ID == userId);
                if (userExist != null)
                {
                    userExist.IDUSER = droit.IDUSER;
                    userExist.IDMAPPAGE = droit.IDMAPPAGE;
                    userExist.IDSOCIETE = suser.IDSOCIETE;

                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = droit }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le droit de l'utilisateur existe déjà pour l'instance et la base de données sélectionées. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}