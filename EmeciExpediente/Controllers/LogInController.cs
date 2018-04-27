using EmeciExpediente.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Net.Mail;
using System.Configuration;

namespace EmeciExpediente.Controllers
{
    public class LogInController : Controller
    {
        /* conexion */
        emeciEntities DB = new emeciEntities();
        public string coordinate { get { return Session["coordinate"].ToString(); } set { Session["coordinate"] = value; } }
        public string cardNumber { get { return Session["cardNumber"].ToString(); } set { Session["cardNumber"] = value; } }
        public int idPatient { get { return (int)Session["IdPatient"]; } }




        [HttpGet]
        public ActionResult FirstAccess()
        {
            if (Session["IdPatient"] != null)
                return RedirectToAction("GeneralInfo", "Expedient");

            var model = new Access();
            ModelState.Clear();

            return View(model);
        }

        [HttpPost]
        public ActionResult FirstAccess(Access model)
        {
            if (ModelState.IsValid)
            {
                cardNumber = model.numberCard1 
                    + "-" + model.numberCard2 
                    + "-" + model.numberCard3;

                var inf = (from p in DB.Paciente
                           join r in DB.Registro on p.IdRegistro equals r.idRegistro
                           where r.Emeci == cardNumber && r.Status == "V"
                           select new { p, r }).FirstOrDefault();

                if (inf != null && inf.r.FechaExpiracion.HasValue)
                {
                    if (inf.r.clave == model.password)
                    {
                        if (ItIsExpired(inf.r.FechaExpiracion))
                            model.expiredCard = true;
                        else return RedirectToAction("SecondAccess", "LogIn");
                    }
                    else model.accessDenied = true;
                } else model.accessDenied = true;  
            } else model.accessDenied = true;

            return View(model);
        }

        private bool ItIsExpired(DateTime? expirationDate) => (DateTime.Now.Date - expirationDate.Value.Date).TotalDays > 30 ? true : false;







        /*
            Se verifica las coordenada
        */
        [HttpGet]
        public ActionResult SecondAccess()
        {
            if (Session["IdPatient"] != null)
                return RedirectToAction("GeneralInfo", "Expedient");

            var model = new AccessByCoordinate();

            int num = new Random().Next(1, 10);
            int lyric = new Random().Next(65, 74);
            coordinate = (char)lyric + "," + num;
            ViewBag.coordinate = coordinate;
            
           return View(model);
        }

        [HttpPost]
        public ActionResult SecondAccess(AccessByCoordinate model)
        {
            ViewBag.coordinate = coordinate;
            if (ModelState.IsValid)
            {
                var inf = (from p in DB.Paciente
                           join r in DB.Registro on p.IdRegistro equals r.idRegistro
                           join d in DB.DatosTarjeta on r.Emeci equals d.noTarjeta
                           where r.Emeci == cardNumber && d.Dato == model.coordinate && d.Coordenada == coordinate.Replace(",", "")
                           select new { p, r }).FirstOrDefault();

                if (inf != null)
                {
                    Session.Add("IdPatient", inf.p.idPaciente);
                    Session.Add("IdRegister", inf.r.idRegistro);
                    return RedirectToAction("GeneralInfo", "Expedient");
                } else model.accessDenied = true;
            } else model.accessDenied = true;

            return View(model);
        }



        /*
            Recordar contraseña
        */
        [HttpGet]
        public ActionResult RememberPassword()
        {
            var model = new RememberPwd();
            ModelState.Clear();

            return View(model);
        }

        [HttpPost]
        public ActionResult RememberPassword(RememberPwd model)
        {
            if (ModelState.IsValid)
            {
                cardNumber = model.numberCard1
                    + "-" + model.numberCard2
                    + "-" + model.numberCard3;

                var inf = (from p in DB.Paciente
                           join r in DB.Registro on p.IdRegistro equals r.idRegistro
                           where r.Emeci == cardNumber && r.Status == "V"
                           select new { p, r }).FirstOrDefault();

                if (inf != null && inf.r.FechaExpiracion.HasValue)
                {
                    if (SendEmail(inf.r.Emails, inf.r.Nombre, inf.r.clave))
                        model.success = true;
                    else model.errorMail = true;
                } else model.accessDenied = true;
            } else model.accessDenied = true;

            return View(model);
        }


        private bool SendEmail(string email, string name, string psw)
        {
            bool send = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.Priority = MailPriority.Normal;
                mail.Subject = "Registro Paciente EMECI";
                mail.To.Add(email);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["email"]);
                //mail.Bcc.Add("bustamante24.1989@gmail.com");

                string body = "Bienvenido a Emeci.com, su petición ha sido procesada satisfactoriamente" +
                    Environment.NewLine + new string('*', 100) + Environment.NewLine +
                    "Nombre: " + name + Environment.NewLine +
                    "Correo: " + email + Environment.NewLine +
                    "Acceso a expediente: " + psw + Environment.NewLine +
                    new string('*', 100);

                mail.Body = body;
                SmtpClient cl = new SmtpClient();
                string xmail = ConfigurationManager.AppSettings["email"];
                string xpwd = ConfigurationManager.AppSettings["pwdEmail"];
                cl.Credentials = new System.Net.NetworkCredential(xmail, xpwd);
                cl.Port = 587;
                cl.EnableSsl = true;
                cl.Host = ConfigurationManager.AppSettings["SMTP"];  
                cl.Send(mail);
                send = true;
            }
            catch (SmtpException ex) { throw new Exception( ex.Message); }
            return send;
        }



        /*
            Cambiar contraseña
        */
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var model = new ChangePwd();
            return View(model);
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePwd model)
        {
            if (Session["IdPatient"] == null)
                return RedirectToAction("FirstAccess", "LogIn");

            if (ModelState.IsValid)
            {
                var reg = (from r in DB.Registro
                           join p in DB.Paciente on r.idRegistro equals p.IdRegistro
                           where p.idPaciente == idPatient
                           select r).FirstOrDefault();
                if (reg != null)
                {
                    if (reg.clave == model.oldPassword)
                    {
                        if (model.newPassword == model.newPassword2)
                        {
                            reg.clave = model.newPassword;
                            DB.SaveChanges();
                            model.success = true;
                        } else model.errorNewPassword = true;
                    } else model.errorOldPassword = true;
                } else return RedirectToAction("FirstAccess", "LogIn");
            } else model.errorFields = true;

            return View(model);
        }
    }
}