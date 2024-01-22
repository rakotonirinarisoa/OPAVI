using apptab;
using apptab.Extension;
using apptab.Models;
using Aspose.Zip;
using Aspose.Zip.Saving;
using Microsoft.Ajax.Utilities;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SOFTOPAVI.Controllers
{
    public class HomeController : Controller
    {
        private readonly OPAVIWEB db = new OPAVIWEB();
        private readonly OPAVITOMATE __db = new OPAVITOMATE();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public ActionResult Index()
        {
			
            return View();
        }
		
		public ActionResult TdbAccueil()
        {
            return View();
        }

        public ActionResult HistoReg()
        {
            return View();
        }
		public ActionResult CancelHisto()
        {
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
                    ID = a.ID
                }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = droits }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
		[HttpPost]
		public JsonResult CreateZipFile_old(OPA_USERS suser, string list)
		{
			StreamWriter sw = null;
			var lists = list.Split(',');
			List<AnalY> analY = new List<AnalY>();
			//List<OPA_REGLEMENT> test = new List<OPA_REGLEMENT>();
			sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\OPAVI.txt");
			foreach (var item in lists)
			{
				var tt = Convert.ToDecimal(item);
				var xy = db.OPA_REGLEMENT.Where(x => x.NUM == tt).FirstOrDefault();
				sw.Write(xy.NUM + "\t" + xy.DATE + "\t" + xy.BENEFICIAIRE + "\t" + xy.BANQUE + "\t" + xy.GUICHET + "\t" + xy.RIB + "\t" + xy.MONTANT + "\t" + xy.LIBELLE + "\t" + xy.NUM_ETABLISSEMENT + "\t" + xy.CODE_J + "\t" + xy.DOM1 + "\t" + xy.DOM2 + "\t" + xy.CATEGORIE + "\t" + xy.APPLICATION + "\t" + xy.IDSOCIETE + Environment.NewLine);
			}
			sw.Close();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));
		}
		public string CreateAFBTXT(string pathchemin, string pathfiles) 
		{
			try
			{
				string pth = AppDomain.CurrentDomain.BaseDirectory + "\\FILERESULT\\";
				if (!Directory.Exists(pth))
				{
					Directory.CreateDirectory(pth);
				}
				StreamWriter sw = null;
				sw = new StreamWriter(pth + pathchemin + ".txt");
				sw.Write(pathfiles);
				sw.Close();
				string source = pth + pathchemin + ".txt";
				return source;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			
		}
		public string CreateAFBTXTArch(string pathchemin, string pathfiles,string psw) 
		{
			string pth = AppDomain.CurrentDomain.BaseDirectory + "\\FILERESULT\\";
			if (!Directory.Exists(pth))
			{
				Directory.CreateDirectory(pth);
			}
			StreamWriter sw = null;
			sw = new StreamWriter(pth + pathchemin + ".txt");
			//sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + pathchemin + ".txt");
			sw.Write(pathfiles);
			sw.Close();
			
			Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(psw)));

			// Add files to the archive
			archive.CreateEntry(pathchemin + ".txt", pth + pathchemin + ".txt");
			string source = pth + pathchemin + ".zip";
			archive.Save(source);
			return source;
		}
		[HttpPost] 
		public ActionResult CreateZipFile(OPA_USERS suser/*, List<AnalY> analY*/,int intbasetype, bool devise,string codeJ,string baseName)
        {
			StreamWriter sw = null;
			AFB160 aFB160 = new AFB160();
			var send = "";
			var ps = db.OPA_USERS.Where(x => x.LOGIN == suser.LOGIN && x.IDSOCIETE == suser.IDSOCIETE).Select(x => x.PWD).FirstOrDefault();
			if (baseName == "1")
			{
				var pathfile = aFB160.CreateTOMPAIEAFB160(devise, codeJ, suser);
				if (intbasetype == 0)
				{
					send = CreateAFBTXT(pathfile.Chemin,pathfile.Fichier);
				}else if (intbasetype == 1)
				{
					send = CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
				}else if (intbasetype == 2)
				{
					send = CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				else
				{
					send = CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					//GenererG(send);
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				#region comms
				//sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".txt");
				//sw.Write(pathfile.Fichier);
				//sw.Close();


				//Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(ps)));

				//// Add files to the archive
				//archive.CreateEntry(pathfile.Chemin +".txt", AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".txt");
				//string source = AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin +".zip";
				//archive.Save(source);
				#endregion
				//var telch = GenererG(send);

				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Archive Success ", data = send }, settings));
			}
			else if (baseName == "2")
            {
				var pathfile=aFB160.CreateTOMPROAFB160(devise, codeJ, suser);
				if (intbasetype == 0)
				{
					CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
				}
				else if (intbasetype == 1)
				{
					CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
				}
				else if (intbasetype == 2)
				{
					send = CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				else
				{
					send = CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				#region comms
				//sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".txt");
				//sw.Write(pathfile.Fichier);
				//sw.Close();
				////get password
				////var ps = db.OPA_USERS.Where(x=>x.LOGIN == suser.LOGIN && x.IDSOCIETE == suser.IDSOCIETE).Select(x=>x.PWD).FirstOrDefault();
				//Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(ps)));

				//// Add files to the archive
				//archive.CreateEntry(pathfile.Chemin + ".txt", AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".txt");

				//// save the archive
				//string source = AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".zip";
				//archive.Save(source);
# endregion

				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Archive Success ", data = send }, settings));
			}
			else 
			{
				var pathfile = aFB160.CreateBRAFB160(devise, codeJ, suser);
				//var send = "";
				if (intbasetype == 0)
				{
					send = CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
				}
				else if (intbasetype == 1)
				{
					send = CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
				}
				else if (intbasetype == 2)
				{
					send = CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				else
				{
					send = CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
					var ftp = db.OPA_FTP.Where(x => x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
					SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
				}
				#region comms
				//sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin +".txt");
				//sw.Write(pathfile.Fichier);
				//sw.Close();
				////var ps = db.OPA_USERS.Where(x => x.LOGIN == suser.LOGIN && x.IDSOCIETE == suser.IDSOCIETE).Select(x => x.PWD).FirstOrDefault();
				//Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(ps)));

				//// Add files to the archive
				//archive.CreateEntry(pathfile.Chemin + ".txt", AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".txt");
				//string source = AppDomain.CurrentDomain.BaseDirectory + pathfile.Chemin + ".zip";
				//archive.Save(source);
				#endregion
				//var telch = GenererG(send);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Archive Success ", data = send }, settings));
			}
			#region comm
			//         sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\OPAVI.txt");
			//if (analY != null)
			//{
			//	foreach (var x in analY.ToList())
			//	{
			//		try
			//		{
			//			if (x.Numero == null)
			//			{
			//				x.Numero = "";
			//			}
			//			if (x.Datedordre == null)
			//			{
			//				x.Datedordre = "";
			//			}
			//			if (x.NumPiece == null)
			//			{
			//				x.NumPiece = "";
			//			}
			//			if (x.Compte == null)
			//			{
			//				x.Compte = "";
			//			}
			//			if (x.Libelle == null)
			//			{
			//				x.Libelle = "";
			//			}
			//			if (x.debit == null)
			//			{
			//				x.debit = "";
			//			}
			//			if (x.credit == null)
			//			{
			//				x.credit = "";
			//			}
			//			if (x.Montadevise == null)
			//			{
			//				x.Montadevise = "";
			//			}
			//			if (x.Mon == null)
			//			{
			//				x.Mon = "";
			//			}
			//			if (x.Rang == null)
			//			{
			//				x.Rang = "";
			//			}
			//			if (x.FinCat == null)
			//			{
			//				x.FinCat = "";
			//			}
			//			if (x.Comm == null)
			//			{
			//				x.Comm = "";
			//			}
			//			if (x.Plan6 == null)
			//			{
			//				x.Plan6 = "";
			//			}
			//			if (x.Journal == null)
			//			{
			//				x.Journal = "";
			//			}
			//			if (x.Marche == null)
			//			{
			//				x.Marche = "";
			//			}
			//			sw.Write(x.Numero + "\t"+ x.Datedordre+ "\t"+ x.NumPiece+ "\t"+ x.Compte+ "\t"+ x.Libelle+ "\t"+ x.debit+ "\t"+ x.credit+ "\t"+ x.Montadevise+ "\t"+ x.Mon+ "\t"+ x.Rang+ "\t"+ x.FinCat+ "\t"+ x.Comm+ "\t"+ x.Plan6+ "\t"+ x.Journal+ "\t"+ x.Marche + Environment.NewLine);

			//		}
			//		catch
			//		{
			//			return Json(JsonConvert.SerializeObject(new { type = "Error", msg = "Saisie Incorrect. "}, settings));
			//		}
			//	}
			//	sw.Close();
			//}

			//Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings("12345")));

			//// Add files to the archive
			//archive.CreateEntry("OPAVI.txt", AppDomain.CurrentDomain.BaseDirectory + "\\OPAVI.txt");

			//// save the archive

			//string source = AppDomain.CurrentDomain.BaseDirectory + "EncryptOPAVI.zip"; ;
			//string path = "";
			//archive.Save(source);
			//string hote = "";
			//string USERFTP = "";
			//string PWDFTP = "";
			//var ftp = db.OPA_FTP.Where(x=>x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();
			//hote = ftp.HOTE;
			//USERFTP = ftp.IDENTIFIANT;
			//PWDFTP = ftp.FTPPWD;
			//path = ftp.PATH;
			//SENDFTP(hote, path,USERFTP,PWDFTP,source);
			#endregion
			
			//return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Archive Success ", data = archive }, settings));
        }
		[HttpGet]
		public ActionResult GetFile(string file)
		{
			try
			{
				if (file.Contains(".zip"))
				{
					string path = file;
					var fs = new FileStream(path, FileMode.Open);
					return File(fs, "application/x-7z-compressed", file.Split('\\').Last().Split('.').First() + ".zip");
				}
				else
				{
					string path = file;
					var fs = new FileStream(path, FileMode.Open);
					return File(fs, "application/txt", file.Split('\\').Last().Split('.').First() + ".txt");
				}
				
			}
			catch { return null; }
		}
		public class AnalY
		{
			public string Numero { get; set; }
			public string Datedordre { get; set; }
			public string NumPiece { get; set; }
			public string Compte { get; set; }
			public string Libelle { get; set; }
			public string debit { get; set; }
			public string credit { get; set; }
			public string Montadevise { get; set; }
			public string Mon { get; set; }
			public string Rang { get; set; }
			public string FinCat { get; set; }
			public string Comm { get; set; }
			public string Plan6 { get; set; }
			public string Journal { get; set; }
			public string Marche { get; set; }
		}

		[HttpPost]
		public JsonResult getelementjsPaie(int ChoixBase, string journal, int mois, int annee, string matr1, string matr2, DateTime datePaie, OPA_USERS suser)
		{
			AFB160 afb160 = new AFB160();
			List<DataListTompaie> resultP = new List<DataListTompaie>();
			var hst = db.OPA_HISTORIQUE.Select(x => x.NUMENREG.ToString()).ToArray();
			var list = afb160.getListEcriturePaie(journal, mois, annee, matr1, matr2, datePaie, suser).Where(x=>!hst.Contains(x.No.ToString())).ToList();
			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));

		}
		[HttpPost]	
		public JsonResult getelementjs(int ChoixBase, string journal, DateTime datein, DateTime dateout, string comptaG, string auxi, string auxi1, DateTime dateP,/* int mois, int annee, string matr1, string matr2, DateTime datePaie,*/ OPA_USERS suser)
		{
			AFB160 afb160 = new AFB160();
			var hst = db.OPA_HISTORIQUE.Select(x => x.NUMENREG.ToString()).ToArray();
			var list = afb160.getListEcritureCompta(journal, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => !hst.Contains(x.No.ToString())).ToList();
			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
			
		}
		[HttpPost]
		public JsonResult getelementjsBR(int ChoixBase, string journal, DateTime datein,bool devise, DateTime dateout, string comptaG, string auxi, string auxi1, string etat, DateTime dateP,/* int mois, int annee, string matr1, string matr2, DateTime datePaie,*/ OPA_USERS suser)
		{
			AFB160 afb160 = new AFB160();
			var hst = db.OPA_HISTORIQUEBR.Where(x=>x.IDSOCIETE == suser.IDSOCIETE).Select(x => x.NUMENREG.ToString()).ToArray();
			var list = afb160.getListEcritureBR(journal, datein, dateout,devise, comptaG, auxi, etat, dateP, suser).Where(x => !hst.Contains(x.No.ToString())).ToList();
			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));

		}
		[HttpPost]
		public JsonResult GetEtat()
		{

			List<string>listEtat = __db.OP_CHAINETRAITEMENT.Select(x => x.ETAT).ToList();
			return Json(JsonConvert.SerializeObject(new { type = "sucess", msg = "", data = listEtat }));
		}
		[HttpPost]
        public JsonResult GetCODEJournal(string baseName)
        {
			//if (baseName == "1")
			//{
			//	OPAVITOMATE.connex = "Data Source=FID-INF-PC;Initial Catalog=TOMPAIE;User ID=sa;Password=Soft123well!;";
			//}else 
			//{
			//	OPAVITOMATE.connex = "Data Source=FID-INF-PC;Initial Catalog=PIC3;User ID=sa;Password=Soft123well!;";
			//}
			//OPAVITOMATE.connex = "Data Source=FID-INF-PC;Initial Catalog=TOMPAIE;User ID=sa;Password=Soft123well!;";
			OPAVITOMATE.connex = "Data Source=FID-INF-PC;Initial Catalog=PIC;Integrated Security=True";
			//OPAVITOMATE.connex = "Data Source=NOM-IT-PC;Initial Catalog=PIC3;Integrated Security=True";
			OPAVITOMATE __db = new OPAVITOMATE();

			var JournalVM = __db.RJL1.Select(x=> new { 
                CODE = x.CODE,
                LIBELLE = x.LIBELLE
            }).ToList();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = JournalVM }, settings));
		}
		[HttpPost]
		public JsonResult GetCompteG()
        {
            
			var CompteG = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).GroupBy(x => x.COGE).Select(x => new
			{
				COGE = x.Key,
				AUXI = x.Select(y=>y.AUXI).Distinct().ToList()
			}).ToList();

            var CompteG1 = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).Join(__db.RTIERS, mcompta => mcompta.COGEAUXI, rtiers => rtiers.COGEAUXI, (mcompta, rtiers) => new
            {
                COGE = mcompta.COGE,
                AUXI = mcompta.AUXI,
				NOM = rtiers.NOM
            }).GroupBy(x => x.COGE).Select(x => new
			{
				COGE = x.Key,
				AUXI = x.Select(y => new { AUXI = y.AUXI, NOM = y.NOM }).Distinct().ToList()
			}).ToList();

			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.ToString().Substring(0,1) == "4").ToList();
			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).Select(x=>x.COGE && x.AUXI).Distinct().ToList();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteG }, settings));
		}

		[HttpPost]
		public JsonResult GetCheckedCompte(string baseName ,DateTime datein, DateTime dateout,string comptaG ,string auxi, DateTime dateP, string listCompte, string journal ,string etat,bool devise,OPA_USERS suser)
		{
            if (string.IsNullOrEmpty(listCompte))
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = "" }, settings));

            List<string> list = listCompte.Split(',').ToList();
			List<string> numBR = listCompte.Split(',').ToList();
			List<OPA_REGLEMENTBR> brResult = new List<OPA_REGLEMENTBR>();
			List<DataListTompro> listReg = new List<DataListTompro>();
			List<DataListTomOP> listRegBR = new List<DataListTomOP>();
			AFB160 aFB160 = new AFB160();
			if (baseName == "3")
			{
				
				aFB160.SaveValideSelectEcritureBR(numBR, journal, etat, devise, suser);
				foreach (var item in numBR)
				{
					brResult.Add(db.OPA_REGLEMENTBR.Where(x => x.NUM == item && x.CODE_J == journal && x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault());
					//foreach (var br in brResult)
					//{
					//	if (br.NUM == item)
					//	{
					//		//listRegBR = aFB160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser);
							
					//	}
						
					//}
					
				}
				listRegBR = aFB160.getREGLEMENTBR(suser);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = listRegBR }, settings));
			}
			else
			{
				aFB160.SaveValideSelectEcriture(list, true, suser);
				//foreach (var item in list)
				//{
				//	//listReg.Add(db.OPA_REGLEMENT.Where(x => x.NUM.ToString() == item && x.CODE_J == journal && x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault());
					
				//}
				listReg = aFB160.getREGLEMENT(suser);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = listReg }, settings));
			}
            

            //return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = "" }, settings)); 
			//var listRegl = db.OPA_REGLEMENT.Where(x=>x.)

			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.ToString().Substring(0,1) == "4").ToList();
			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).Select(x=>x.COGE && x.AUXI).Distinct().ToList();

			//return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteG }, settings));
		}
		public JsonResult GetCheckedComptePaie(string baseName,int mois,int annee, string listCompte,string matriculeD,string matriculeF,bool devise, DateTime dateP, string journal ,OPA_USERS suser)
		{
			if (string.IsNullOrEmpty(listCompte))
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = "" }, settings));

			List<OPA_REGLEMENTBR> brResult = new List<OPA_REGLEMENTBR>();
			List<string> listReg = new List<string>();
			listReg = listCompte.Split(',').ToList();
			AFB160 aFB160 = new AFB160();
			try
			{
				
				aFB160.SaveValideSelectEcriturePaie(listReg, journal, devise, suser);
				//var zz = aFB160.getListEcriturePaie(journal, mois, annee, matriculeD, matriculeF, dateP, suser);
				var listePaie = aFB160.getREGLEMENTPaie(suser);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = listePaie }, settings));
			}
			catch (Exception ex)
			{

				return Json(JsonConvert.SerializeObject(new { type = "Error", msg = ex.Message, data = ex.Message }, settings));
			}
			
			//var listRegl = db.OPA_REGLEMENT.Where(x=>x.)

			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.ToString().Substring(0,1) == "4").ToList();
			//var CompteG = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).Select(x=>x.COGE && x.AUXI).Distinct().ToList();

			//return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteG }, settings));
		}

        public JsonResult GetAnomalieBack(OPA_USERS suser,string baseName)
        {
            AFB160 Afb = new AFB160();
			if (baseName == "3")
			{
				//var anom = db.OPA_ANOMALIEBR.ToList();
				var resultAnomalies = Afb.getListAnomalieBR(suser);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = resultAnomalies }, settings));
			}
			else
			{
				//var anom = db.OPA_ANOMALIE.ToList();
				var resultAnomalies = Afb.getListAnomalie(suser);
				return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = resultAnomalies }, settings));
			}
			
		}
		public void SENDFTP(string HOTE, string PATH, string USERFTP, string PWDFTP, string SOURCE)
		{
			DateTime now = DateTime.Now;
			#region commentaire
			//try
			//{
			//    //GC.Collect(0);

			//    //string PureFileName = new FileInfo(NAMEFILE).Name;
			//    //String uploadUrl = String.Format("{0}/{1}/{2}", "ftp://" + HOTE, PATH, PureFileName);
			//    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
			//    //request.Method = WebRequestMethods.Ftp.UploadFile;
			//    //// This example assumes the FTP site uses anonymous logon.  
			//    //request.Credentials = new NetworkCredential(USERFTP, PWDFTP);
			//    //request.Proxy = null;
			//    //request.KeepAlive = true;
			//    //request.UseBinary = true;
			//    //request.Method = WebRequestMethods.Ftp.UploadFile;

			//    //// Copy the contents of the file to the request stream.  
			//    //StreamReader sourceStream = new StreamReader(@SOURCE);

			//    //Task T = Task.Run(() =>
			//    //{
			//    //    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
			//    //    sourceStream.Close();
			//    //    request.ContentLength = fileContents.Length;
			//    //    Stream requestStream = request.GetRequestStream();
			//    //    requestStream.Write(fileContents, 0, fileContents.Length);
			//    //    requestStream.Close();
			//    //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

			//    //    GC.Collect(0);
			//    //    throw new OutOfMemoryException();
			//    //});

			//    //if (T.IsCompleted)
			//    //    return;

			//    //T.Wait();
			//}
			//catch (Exception ex)
			//{
			//    Library.WriteErrorLog(ex.Message + ex.StackTrace);
			//}

			#endregion
			FileStream fs = null;
			Stream rs = null;

			try
			{
				GC.Collect(0);
				int Size = 2048;/*8092*/

				Task T = Task.Run(() =>
				{
					string file = SOURCE;
					string uploadFileName = new FileInfo(file).Name;
					string uploadUrl = String.Format("{0}/{1}/", "ftp://" + HOTE, PATH);
					fs = new FileStream(file, FileMode.Open, FileAccess.Read);

					string ftpUrl = string.Format("{0}/{1}", uploadUrl, uploadFileName);
					FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
					requestObj.Method = WebRequestMethods.Ftp.UploadFile;
					requestObj.Credentials = new NetworkCredential(USERFTP, PWDFTP);

					//ADD 17h Zoma
					requestObj.UseBinary = true;
					requestObj.UsePassive = true;
					requestObj.KeepAlive = true;
					//

					rs = requestObj.GetRequestStream();

					byte[] buffer = new byte[Size];
					int read = 0;
					while ((read = fs.Read(buffer, 0, buffer.Length)) != 0)
					{
						rs.Write(buffer, 0, read);
					}
					rs.Flush();

				});

				if (T.IsCompleted)
					return;

				T.Wait();

				GC.Collect(0);

				throw new OutOfMemoryException();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}

				if (rs != null)
				{
					rs.Close();
					rs.Dispose();
				}
			}
		}
		[HttpPost]
		public JsonResult GetHistoriques(OPA_USERS suser)
		{
			
			var usr = db.OPA_USERS.Where(x=>x.LOGIN == suser.LOGIN && x.IDSOCIETE == suser.IDSOCIETE).FirstOrDefault();

			var query = db.OPA_HISTORIQUE
				.Where(x => x.IDSOCIETE == suser.IDSOCIETE && x.IDUSER == usr.ID)
				.Join(db.OPA_REGLEMENT, histo => histo.NUMENREG, reglement => reglement.NUM, (histo, reglement) => new
				{
					NUMENREG = histo.NUMENREG,
					DATEAFB = histo.DATEAFB,
					IDUSER = histo.IDUSER,
					IDSOCIETE = histo.IDSOCIETE,
					CODE_J = reglement.CODE_J,
					RIB = reglement.RIB,
					MONTANT = reglement.MONTANT,
					DATE = reglement.DATE,
					LIBELLE = reglement.LIBELLE,
					BANQUE = reglement.BANQUE,
					GUICHET = reglement.GUICHET,
				}).Join(db.OPA_USERS, x => x.IDUSER, user => user.ID, (x, user) => new
				{
					NUMENREG = x.NUMENREG,
					DATEAFB = x.DATEAFB,
					IDUSER = x.IDUSER,
					IDSOCIETE = x.IDSOCIETE,
					CODE_J = x.CODE_J,
					RIB = x.RIB,
					MONTANT = x.MONTANT,
					DATE = x.DATE,
					LIBELLE = x.LIBELLE,
					BANQUE = x.BANQUE,
					GUICHET = x.GUICHET,
					LOGIN = user.LOGIN
				})
				.OrderBy(x=>x.DATE).ToList();
			var queryBr = db.OPA_HISTORIQUEBR
				.Where(x => x.IDSOCIETE == suser.IDSOCIETE && x.IDUSER == usr.ID)
				.Join(db.OPA_REGLEMENTBR, histo => histo.NUMENREG, reglement => reglement.NUM, (histo, reglement) => new
				{
					NUMENREG = histo.NUMENREG,
					DATEAFB = histo.DATEAFB,
					IDUSER = histo.IDUSER,
					IDSOCIETE = histo.IDSOCIETE,
					CODE_J = reglement.CODE_J,
					RIB = reglement.RIB,
					MONTANT = reglement.MONTANT,
					DATE = reglement.DATE,
					LIBELLE = reglement.LIBELLE,
					BANQUE = reglement.BANQUE,
					GUICHET = reglement.GUICHET,
				}).Join(db.OPA_USERS, x => x.IDUSER, user => user.ID, (x, user) => new
				{
					NUMENREG = x.NUMENREG,
					DATEAFB = x.DATEAFB,
					IDUSER = x.IDUSER,
					IDSOCIETE = x.IDSOCIETE,
					CODE_J = x.CODE_J,
					RIB = x.RIB,
					MONTANT = x.MONTANT,
					DATE = x.DATE,
					LIBELLE = x.LIBELLE,
					BANQUE = x.BANQUE,
					GUICHET = x.GUICHET,
					LOGIN = user.LOGIN
				})
				.OrderBy(x => x.DATE).ToList();

			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = query , databr = queryBr }, settings));
		}

		public JsonResult GetCancel(OPA_USERS suser,string listCompte)
		{
			var list = listCompte.Split(',');
			List<OPA_HISTORIQUE> result = new List<OPA_HISTORIQUE>();
			List<OPA_HISTORIQUEBR> resultBR = new List<OPA_HISTORIQUEBR>();
			var user = db.OPA_USERS.Where(x=>x.LOGIN == suser.LOGIN && x.PWD == suser.PWD).FirstOrDefault();
			foreach (var item in list)
			{
				if (item.Contains("BR"))
				{
					resultBR=db.OPA_HISTORIQUEBR.Where(y => y.NUMENREG == item && y.IDUSER == user.ID && y.IDSOCIETE == suser.IDSOCIETE).ToList();
				}
				else
				{
					decimal ii;
					ii = Convert.ToDecimal(item);
					result = db.OPA_HISTORIQUE.Where(x => x.NUMENREG == ii && x.IDUSER == user.ID && x.IDSOCIETE == suser.IDSOCIETE).ToList();

				}

			
				foreach (var pc in result)
				{
					db.OPA_HISTORIQUE.Remove(pc);
				}
				foreach (var br in resultBR)
				{
					db.OPA_HISTORIQUEBR.Remove(br);
				}
			}
			db.SaveChanges();
			return Json(JsonConvert.SerializeObject(new { msg = "Success", data = result, datebr = resultBR }));
		}

		public JsonResult SFTP(string HOTE, string PATH, string USERFTP, string PWDFTP, string SOURCE)
		{
			Console.WriteLine("Create client Object");
			using (SftpClient sftpClient = new SftpClient(getSftpConnection(HOTE, USERFTP, 22, SOURCE)))
			{
				Console.WriteLine("Connect to server");
				sftpClient.Connect();
				Console.WriteLine("Creating FileStream object to stream a file");
				using (FileStream fs = new FileStream("filePath", FileMode.Open))
				{
					sftpClient.BufferSize = 1024;
					sftpClient.UploadFile(fs, Path.GetFileName("filePath"));
				}
				sftpClient.Dispose();
			}
			return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = "" }, settings));
		}

		public static ConnectionInfo getSftpConnection(string host, string username, int port, string publicKeyPath)
		{
			return new ConnectionInfo(host, port, username, privateKeyObject(username, publicKeyPath));
		}

		private static AuthenticationMethod[] privateKeyObject(string username, string publicKeyPath)
		{
			PrivateKeyFile privateKeyFile = new PrivateKeyFile(publicKeyPath);
			PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod =
			   new PrivateKeyAuthenticationMethod(username, privateKeyFile);
			return new AuthenticationMethod[]
			 {
		privateKeyAuthenticationMethod
			 };
		}
	}
}