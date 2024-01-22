using apptab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace SOFTOPAVI.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult SuperAdmin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddSociete(OPA_USERS suser, OPA_SOCIETES societe, OPA_USERS user)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var societeExist = db.OPA_SOCIETES.FirstOrDefault(a => a.SOCIETE == societe.SOCIETE);
            if (societeExist == null)
            {
                var newSociete = new OPA_SOCIETES()
                {
                    SOCIETE = societe.SOCIETE
                };
                db.OPA_SOCIETES.Add(newSociete);
                //var eeee = db.GetValidationErrors();
                db.SaveChanges();

                //First ADMIN//
                int IDSOC = db.OPA_SOCIETES.FirstOrDefault(a => a.SOCIETE == societe.SOCIETE).ID;
                var newFirstAdmin = new OPA_USERS()
                {
                    LOGIN = user.LOGIN,
                    PWD = user.PWD,
                    ROLE = Role.Administrateur,// db.OPA_ROLES.Where(a => a.INTITULES == "Administrateur").FirstOrDefault().ID,
                    IDSOCIETE = IDSOC
                };
                db.OPA_USERS.Add(newFirstAdmin);
                //var eeee = db.GetValidationErrors();
                db.SaveChanges();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "ID Société déjà existant. " }, settings));
            }
        }

        public ActionResult SocieteList()
        {
            ViewBag.Controller = "Liste des Sociétés";
            return View();
        }

        public JsonResult FillTable(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var societe = db.OPA_SOCIETES.Select(a => new
                {
                    SOCIETE = a.SOCIETE,
                    ID = a.ID
                }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = societe }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        public ActionResult SuperAdminMaPList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FillTableMAPP(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var mapp = db.OPA_MAPPAGES.Select(a => new
                {
                    SOCIETE = db.OPA_SOCIETES.FirstOrDefault(x => x.ID == a.IDSOCIETE).SOCIETE,
                    INSTANCE = a.INSTANCE,
                    DBASE = a.DBASE,
                    ID = a.ID
                }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = mapp }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult DeleteMAPP(OPA_USERS suser, string MAPPId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(MAPPId);
                var user = db.OPA_MAPPAGES.FirstOrDefault(a => a.ID == useID);
                if (user != null)
                {
                    db.OPA_MAPPAGES.Remove(user);
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

        public ActionResult DetailsMAPP(string UserId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailsMAPP(OPA_USERS suser, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var map = db.OPA_MAPPAGES.FirstOrDefault(a => a.ID == useID);

                if (map != null)
                {
                    var mapp = new
                    {
                        soc = db.OPA_SOCIETES.FirstOrDefault(a => a.ID == map.IDSOCIETE).ID,
                        inst = map.INSTANCE,
                        auth = map.AUTH,
                        conn = map.CONNEXION,
                        mdp = map.CONNEXPWD,
                        baseD = map.DBASE,
                        id = map.ID
                    };

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { SOCIETE = mapp.soc, INSTANCE = mapp.inst, AUTH = mapp.auth, CONNEXION = mapp.conn, MDP = mapp.mdp, BASED = mapp.baseD, id = mapp.id } }, settings));
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

        public ActionResult SuperAdminMaPCreate()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SuperAdminMaPCreate(OPA_USERS suser, OPA_MAPPAGES user)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var userExist = db.OPA_MAPPAGES.FirstOrDefault(a => a.IDSOCIETE == user.IDSOCIETE && a.INSTANCE == user.INSTANCE && a.DBASE == user.DBASE);
                if (userExist == null)
                {
                    var newUser = new OPA_MAPPAGES()
                    {
                        INSTANCE = user.INSTANCE,
                        AUTH = user.AUTH,
                        CONNEXION = user.CONNEXION,
                        CONNEXPWD = user.CONNEXPWD,
                        DBASE = user.DBASE,
                        IDSOCIETE = user.IDSOCIETE
                    };
                    db.OPA_MAPPAGES.Add(newUser);
                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le mappage existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public ActionResult GetAllSociete(OPA_USERS suser)
        {
            var user = db.OPA_SOCIETES.Select(a => new
            {
                SOCIETE = a.SOCIETE,
                ID = a.ID
            }).ToList();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
        }

        [HttpPost]
        public ActionResult GetNewInstance(OPA_USERS suser, OPA_MAPPAGES map)
        {
            //Get all bases with the instance
            var BaseList = new List<string>();

            //System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            //builder["Data Source"] = map.INSTANCE;
            //builder["integrated Security"] = true;

            //using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            //{
            //    connection.Open();
            //    DataTable listbd = connection.GetSchema("Databases");
            //    foreach (DataRow ligne in listbd.Rows)
            //    {
            //        foreach (DataColumn col in listbd.Columns)
            //        {
            //            BaseList.Add(ligne[0].ToString());

            //            break;
            //        }
            //    }
            //}

            try
            {
                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = map.INSTANCE;
                if (map.CONNEXION != null) connection.UserID = map.CONNEXION;
                if (map.CONNEXPWD != null) connection.Password = map.CONNEXPWD;

                if (map.AUTH == 1) connection.IntegratedSecurity = false;
                else connection.IntegratedSecurity = true;

                String strConn = connection.ToString();
                SqlConnection sqlConn = new SqlConnection(strConn);
                try
                {
                    sqlConn.Open();
                    DataTable tblDatabases = sqlConn.GetSchema("Databases");
                    sqlConn.Close();
                    foreach (DataRow row in tblDatabases.Rows)
                    {
                        String strDatabaseName = row["database_name"].ToString();
                        BaseList.Add(strDatabaseName);
                    }
                    BaseList.Sort();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = BaseList }, settings));
                }
                catch (Exception)
                {
                    if (map.AUTH == 1) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Échec de l'ouverture de session de l'utilisateur 'sa'. Vérifiez vos identifiants pour la connexion à SQL Server. " }, settings));
                    else return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Une erreur liée au réseau ou spécifique à l'instance s'est produite lors de l'établissement d'une connexion à SQL Server. Le serveur est introuvable ou n'est pas accessible. Vérifiez que le nom de l'instance est correct et que SQL Server est configuré pour autoriser les connexions distantes. " }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Une erreur liée au réseau ou spécifique à l'instance s'est produite lors de l'établissement d'une connexion à SQL Server. Le serveur est introuvable ou n'est pas accessible. Vérifiez que le nom de l'instance est correct et que SQL Server est configuré pour autoriser les connexions distantes. " }, settings));
            }
        }

        [HttpPost]
        public JsonResult SuperAdminMaPUpdate(OPA_USERS suser, OPA_MAPPAGES user, string UserId)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int userId = int.Parse(UserId);
                var userExist = db.OPA_MAPPAGES.FirstOrDefault(a => a.IDSOCIETE == user.IDSOCIETE && a.INSTANCE == user.INSTANCE && a.DBASE == user.DBASE);
                var userupdate = db.OPA_MAPPAGES.FirstOrDefault(a => a.ID == userId);
                if (userExist == null)
                {
                    userupdate.INSTANCE = user.INSTANCE;
                    userupdate.AUTH = user.AUTH;
                    userupdate.CONNEXION = user.CONNEXION;
                    userupdate.CONNEXPWD = user.CONNEXPWD;
                    userupdate.DBASE = user.DBASE;
                    userupdate.IDSOCIETE = user.IDSOCIETE;

                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le mappage existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}