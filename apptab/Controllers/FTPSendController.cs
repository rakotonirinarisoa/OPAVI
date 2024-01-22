using apptab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SOFTOPAVI.Controllers
{
    public class FTPSendController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult FTPSendCreate()
        {
            ViewBag.Controller = "Paramétrage FTP";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsFTP(OPA_USERS suser)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = suser.IDSOCIETE.Value;
                var crpto = db.OPA_FTP.FirstOrDefault(a => a.IDSOCIETE == crpt);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer un nouveau FTP. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //public ActionResult FTPSendList()
        //{
        //    return View();
        //}

        [HttpPost]
        public JsonResult UpdateFTP(OPA_USERS suser, OPA_FTP ftp/*, string MDPA*/)
        {
            var exist = db.OPA_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.IDSOCIETE == suser.IDSOCIETE);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            String uploadUrl = String.Format("{0}/{1}/", "ftp://" + ftp.HOTE, ftp.PATH);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ftp.IDENTIFIANT, ftp.FTPPWD);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;

            try
            {
                int IdS = suser.IDSOCIETE.Value;
                var SExist = db.OPA_FTP.FirstOrDefault(a => a.IDSOCIETE == IdS);

                try
                {
                    request.GetResponse();

                    if (SExist != null)
                    {
                        SExist.HOTE = ftp.HOTE;
                        SExist.IDENTIFIANT = ftp.IDENTIFIANT;
                        SExist.FTPPWD = ftp.FTPPWD;
                        SExist.PATH = ftp.PATH;
                        SExist.IDSOCIETE = IdS;

                        db.SaveChanges();

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = ftp }, settings));
                    }
                    else
                    {
                        var ftpp = new OPA_FTP()
                        {
                            HOTE = ftp.HOTE,
                            IDENTIFIANT = ftp.IDENTIFIANT,
                            FTPPWD = ftp.FTPPWD,
                            PATH = ftp.PATH,
                            IDSOCIETE = IdS
                        };

                        db.OPA_FTP.Add(ftpp);
                        db.SaveChanges();

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = ftp }, settings));
                    }
                }
                catch (Exception)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Informations sur le FTP incorrectes et/ou chemin ou dossier de destination non existant. " }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Informations sur le FTP incorrectes et/ou chemin ou dossier de destination non existant. " }, settings));
            }
        }
    }
}