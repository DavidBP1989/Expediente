using System;
using EmeciExpediente.Models;
using System.Linq;
using System.Web.Mvc;
using System.Dynamic;
using static System.IO.Directory;
using System.IO;
using static System.Configuration.ConfigurationManager;

namespace EmeciExpediente.Controllers
{
    public class BaseController : Controller
    {
        public int? _SessionId => (int?)System.Web.HttpContext.Current.Session["IdPatient"];

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (_SessionId == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("FirstAccess", "LogIn"));
            }
            else
            {
                GetImage_Patient();

                using (var DB = new emeciEntities())
                {
                    var query = (from p in DB.Paciente
                                 join r in DB.Registro on p.IdRegistro equals r.idRegistro
                                 where p.idPaciente == _SessionId.Value
                                 select new { r.Nombre, r.Apellido, p.Sexo, p.FechaNacimiento }).First();
                    
                    if (query != null)
                    {
                        ViewBag.Header_PatientName = $"{query.Nombre} {query.Apellido}";
                        ViewBag.Header_PatientSex = query.Sexo;
                        ViewBag.Header_PatientAge = (DateTime.Now.Year - query.FechaNacimiento.Value.Year);
                        if (query.FechaNacimiento.HasValue)
                            ViewBag.agePatient = (DateTime.Now.Year - query.FechaNacimiento.Value.Year);
                        else ViewBag.agePatient = 0;

                        ViewBag.sexPatient = query.Sexo;

                        ViewBag.showObstetrica_Ginecologia = (ViewBag.agePatient >= 12 && query.Sexo == "F");

                        ViewBag.showAntecedent = (ViewBag.agePatient >= 12 && query.Sexo == "M");
                    }
                }
            }
        }


        public ActionResult SignOff()
        {
            System.Web.HttpContext.Current.Session["IdPatient"] = null;
            return RedirectToAction("FirstAccess", "LogIn");
        }


        public ActionResult Agree()
        {
            if (_SessionId != null)
            {
                using (var DB = new emeciEntities())
                {
                    var Data = (from p in DB.Paciente
                                where p.idPaciente == _SessionId.Value
                                select p).FirstOrDefault();

                    if (Data != null)
                    {
                        Data.acepto = true;
                        DB.SaveChanges();
                    }
                }
            }

            return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
        }


        void GetImage_Patient()
        {
            dynamic Image = new ExpandoObject();
            string Folder = $"{AppSettings["FolderImagesApp"]}{_SessionId}\\Perfil";
            if (Exists(Folder))
            {
                string[] Files = GetFiles(Folder);
                if (Files.Length > 0)
                {
                    string ImageName = Path.GetFileName(Files[0]);
                    string Url = $"{AppSettings["RouteImagesApp"]}{_SessionId}/Perfil/{ImageName}";
                    Image.Href = Url;
                    Image.Name = ImageName;
                }
                else
                {
                    Image.Href = Url.Content("~/images/foto-bebe.png");
                    Image.Name = "foto-bebe.png";
                }
            }
            else
            {
                Image.Href = Url.Content("~/images/foto-bebe.png");
                Image.Name = "foto-bebe.png";
            }

            ViewBag.ImagePerfil = Image;
        }
    }
}