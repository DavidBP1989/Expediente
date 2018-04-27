using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmeciExpediente.Controllers
{
    public class InitController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            System.Web.HttpContext.Current.Session["IdPatient"] = 3050;
            if (System.Web.HttpContext.Current.Session["IdPatient"] == null)
                filterContext.Result = new RedirectResult(Url.Action("FirstAccess", "LogIn"));

            ViewBag.Header_PatientName = "";
            ViewBag.Header_PatientSex = "";
            ViewBag.Header_PatientAge = "";
            dynamic Image = new ExpandoObject();
            Image.Href = Url.Content("~/images/foto-bebe.png");
            Image.Name = "foto-bebe.png";
            ViewBag.ImagePerfil = Image;
            ViewBag.agePatient = 1;
            ViewBag.sexPatient = "M";

            ViewBag.showObstetrica_Ginecologia = false;

            //antecedentes
            ViewBag.showAntecedent = false;
        }
    }
}