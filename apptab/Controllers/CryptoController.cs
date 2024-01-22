using apptab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOFTOPAVI.Controllers
{
    public class CryptoController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult CryptoList()
        {
            ViewBag.Controller = "Historique de changement du mot de passe du fichier. ";
            return View();
        }

        [HttpPost]
        public JsonResult FillTable(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var users = db.OPA_CRYPTOHIST.Where(a => a.IDSOCIETE == suser.IDSOCIETE).Select(a => new
                {
                    CRYPTODATE = a.CRYPTODATE.ToString(),
                    CRYPTO = a.CRYPTOPWD,
                    IDUSER = db.OPA_USERS.FirstOrDefault(x => x.ID == a.IDUSER).LOGIN,
                    ID = a.ID
                }).OrderBy(x => x.CRYPTODATE).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = users }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public ActionResult DetailsCrypto(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = suser.IDSOCIETE.Value;
                var crpto = db.OPA_CRYPTO.FirstOrDefault(a => a.IDSOCIETE == crpt);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer un nouveau mot de passe. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        public ActionResult CryptoCreate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateCRT(OPA_USERS suser, OPA_CRYPTO user/*, string MDPA*/)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IdS = suser.IDSOCIETE.Value;
                var SExist = db.OPA_CRYPTO.FirstOrDefault(a => a.IDSOCIETE == IdS);
                if (SExist != null)
                {
                    SExist.CRYPTPWD = user.CRYPTPWD;
                    db.SaveChanges();

                    //Insertion historique de changement MDP Crypto//
                    var crtH = new OPA_CRYPTOHIST()
                    {
                        CRYPTODATE = DateTime.Now,
                        IDSOCIETE = IdS,
                        IDUSER = exist.ID,
                        CRYPTOPWD = user.CRYPTPWD
                    };
                    db.OPA_CRYPTOHIST.Add(crtH);
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    var crt = new OPA_CRYPTO()
                    {
                        IDSOCIETE = IdS,
                        CRYPTPWD = user.CRYPTPWD
                    };
                    db.OPA_CRYPTO.Add(crt);
                    db.SaveChanges();

                    //Insertion historique de changement MDP Crypto//
                    var crtH = new OPA_CRYPTOHIST()
                    {
                        CRYPTODATE = DateTime.Now,
                        IDSOCIETE = IdS,
                        IDUSER = exist.ID,
                        CRYPTOPWD = user.CRYPTPWD
                    };
                    db.OPA_CRYPTOHIST.Add(crtH);
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}