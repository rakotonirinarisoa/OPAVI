using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using apptab;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.Runtime;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

namespace SOFTOPAVI.Controllers
{
    public class UserController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //// GET: User
        //[Route("UserList")]
        //[HttpGet]
        public ActionResult List()
        {
            ViewBag.Controller = "Liste des Utilisateurs";
            return View();
        }

        [HttpPost]
		public JsonResult FillTable(OPA_USERS suser)
		{
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD);
            ViewBag.Role = exist.ROLE;
			if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

			try
			{
				var test = db.OPA_USERS.Where(x => x.ROLE == suser.ROLE && x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
				if (test.ROLE == Role.SuperAdministrateur)
				{
					var users = db.OPA_USERS.Select(a => new
					{
						LOGIN = a.LOGIN,
						PWD = a.PWD,
						ROLE = a.ROLE.ToString(), //db.OPA_ROLES.Where(x => x.ID == a.ROLE).FirstOrDefault().INTITULES,
						ID = a.ID,
                        SOCIETE = db.OPA_SOCIETES.Where(z => z.ID == suser.IDSOCIETE).FirstOrDefault().SOCIETE
					}).ToList();
					return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = users }, settings));
				}
				else
				{
					var users = db.OPA_USERS.Where(x => x.ROLE != Role.SuperAdministrateur && x.IDSOCIETE == suser.IDSOCIETE).Select(a => new
					{
						LOGIN = a.LOGIN,
						PWD = a.PWD,
						ROLE = a.ROLE.ToString(), //db.OPA_ROLES.Where(x => x.ID == a.ROLE).FirstOrDefault().INTITULES,
						ID = a.ID,
                        SOCIETE = db.OPA_SOCIETES.Where(z => z.ID == suser.IDSOCIETE).FirstOrDefault().SOCIETE
					}).ToList();
					return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = users }, settings));
				}



			}
			catch (Exception e)
			{
				return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
			}
		}
		public ActionResult Create() {

            return View();
        }

        [HttpPost]
        public ActionResult GetAllRole()
        {
            var role = db.OPA_ROLES.Where(a => a.INTITULES != "SAdministrateur").ToList();
            var enumlist = Enum.GetValues(typeof(Role));

            var roles = new Dictionary<int,string>();

            foreach (var item in enumlist)
            {
                roles.Add((int)item, Enum.GetName(typeof(Role), item));
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = roles }, settings));
        }

        [HttpPost]
        public JsonResult AddUser(OPA_USERS suser, OPA_USERS user)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var userExist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == user.LOGIN && a.IDSOCIETE == suser.IDSOCIETE);
                if (userExist == null)
                {
                    var newUser = new OPA_USERS()
                    {
                        LOGIN = user.LOGIN,
                        PWD = user.PWD,
                        IDSOCIETE = suser.IDSOCIETE,
                        ROLE = user.ROLE,
                    };
                    db.OPA_USERS.Add(newUser);
                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "L'utilisateur existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateUser(OPA_USERS suser, OPA_USERS user, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int userId = int.Parse(UserId);
                var userExist = db.OPA_USERS.FirstOrDefault(a => a.ID == userId);
                if (userExist != null)
                {
                    userExist.LOGIN = user.LOGIN;
                    userExist.PWD = user.PWD;
                    userExist.IDSOCIETE = suser.IDSOCIETE;
                    userExist.ROLE = user.ROLE;

                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "L'utilisateur existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        public ActionResult Param()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Param(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.ID;
                var crpto = db.OPA_USERS.FirstOrDefault(a => a.ID == crpt);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
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
        public JsonResult UpdateMDP(OPA_USERS suser, OPA_USERS user/*, string MDPA*/)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.ID;
                var SExist = db.OPA_USERS.FirstOrDefault(a => a.ID == crpt);
                if (SExist != null)
                {
                    if(SExist.PWD != user.LOGIN)
                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Veuillez vérifier votre ancien mot de passe. ", data = user }, settings));

                    SExist.PWD = user.PWD;
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
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
                var user = db.OPA_USERS.FirstOrDefault(a => a.ID == useID);
				
				if (user != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
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
        public ActionResult Login()
        {
            db.OPA_USERS.Any();
            return View();
        }

        [HttpPost]
        public ActionResult Login(OPA_USERS Users)
        {
            try
            {
                var test = db.OPA_USERS.FirstOrDefault(x => x.LOGIN == Users.LOGIN && x.PWD == Users.PWD);
                if (test == null) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vérifiez vos identifiants. " }, settings));

                Session["userSession"] = test;


				if (String.IsNullOrEmpty(test.IDSOCIETE.ToString())) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vous n'êtes pas rattaché à une société. " }, settings));

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", Data = new { ROLE = test.ROLE, IDSOCIETE = test.IDSOCIETE } }, settings));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteUser(OPA_USERS suser, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var user = db.OPA_USERS.FirstOrDefault(a => a.ID == useID);
                if (user != null)
                {
                    db.OPA_USERS.Remove(user);
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

		[HttpPost]
		public JsonResult GetUR(OPA_USERS suser)
		{
			var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
			if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

			return Json(JsonConvert.SerializeObject(new { type = "login", msg ="" , data = exist.ROLE != Role.SuperAdministrateur }, settings));
		}
	}
}