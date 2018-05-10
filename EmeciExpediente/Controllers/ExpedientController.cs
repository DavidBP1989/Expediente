using EmeciExpediente.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Web;
using System.Dynamic;
using static System.IO.Directory;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Globalization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;

namespace EmeciExpediente.Controllers
{
    public class ExpedientController : BaseController
    {
        emeciEntities DB = new emeciEntities();
        public int idPatient { get { return (int)Session["IdPatient"]; } }

        /*Max Imagen*/
        public int maxContent { get; set; } = 1024 * 2048;




        /// <summary>
        /// Pagina principal
        /// Si se inicio sesion como paciente
        /// el resumen sera solo lectura
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Summary()
        {
            ViewBag.record = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p.historialclinico).FirstOrDefault();

            ReadOnly();
            return View();
        }

        private void ReadOnly()
        {
            if (Session["idMedEmeci"] == null)
                ViewBag.readOnly = true;
            else ViewBag.readOnly = false;
        }
        
        [HttpPost]
        public ActionResult Summary(string record)
        {
            Paciente pat;
            pat = (from p in DB.Paciente
                   where p.idPaciente == idPatient
                   select p).First();
            pat.historialclinico = record;
            ReadOnly();

            DB.SaveChanges();
            return View();
        }









        



        /* Informacion general */
        [HttpGet]
        public ActionResult GeneralInfo()
        {
            var model = new GeneralInformation();
            
            var patient = (from p in DB.Paciente
                           join reg in DB.Registro on p.IdRegistro equals reg.idRegistro
                           where p.idPaciente == idPatient
                           select new { reg, p }).FirstOrDefault();

            model.name = patient.reg.Nombre;
            model.LastName = patient.reg.Apellido;
            model.sex = patient.p.Sexo;
            model.lifeStage = patient.p.Etapa.ToString();
            model.education = patient.p.TipoEscolaridad.ToString();
            model.typeEducation = patient.p.Escolaridad;
            model.civilStatus = patient.p.estadocivil.ToString();
            model.ocupation = patient.p.ocupacion;
            model.address = patient.reg.Domicilio;
            model.colony = patient.reg.Colonia;
            model.cp = patient.reg.CodigoPostal;
            model.country = patient.reg.IdPais;
            model.phone = patient.reg.Telefono;
            model.officePhone = patient.p.telefonooficina;
            model.cellPhone = patient.reg.TelefonoCel;
            model.email = patient.reg.Emails;
            model.Curp = patient.p.Curp;
            model.state = patient.reg.idEstado;
            model.city = patient.reg.idCiudad.ToString();

            if (patient.p.acepto != null)
                model.acepto = (bool)patient.p.acepto;
            else model.acepto = false;

            Load_States(model.country);
            Load_Cities(model.state, model.country);

            return View(model);
        }


        private void Load_States(string idCountry)
        {
            var states = (from s in DB.Estados
                          where s.IdPais == idCountry
                          select s).ToList();

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "-- Seleccione --", Value = "" });
            foreach (var i in states) {
                items.Add(new SelectListItem { Text = i.Nombre, Value = i.idEstado });
            }

            ViewBag.states = items;
        }

        public ActionResult ChangeCities(string idState, string idCountry)
        {
            Load_Cities(idState, idCountry);
            return Json(ViewBag.cities, JsonRequestBehavior.AllowGet);
        }

        private void Load_Cities(string idState, string idCountry)
        {
            var cities = (from c in DB.Ciudades
                          where c.IdPais == idCountry && c.idEstado == idState
                          select c).ToList();

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "-- Seleccione --", Value = "-1" });
            foreach(var i in cities) {
                items.Add(new SelectListItem { Text = i.Nombre, Value = i.idciudad.ToString() });
            }
            items.Add(new SelectListItem { Text = "Otra -->", Value = "-2" });

            ViewBag.cities = items;
        }

        public ActionResult ChangeStates(string idCountry)
        {
            Load_States(idCountry);
            return Json(ViewBag.states, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GeneralInfo(GeneralInformation info)
        {
            string IdCity = info.city;
            if (!string.IsNullOrEmpty(info.OtherCity))
            {
                try
                {
                    Ciudades Last = DB.Ciudades.ToArray().Last();

                    var Country = (from e in DB.Estados
                                   where e.idEstado == info.state
                                   select e.IdPais).First();

                    Ciudades Cities = new Ciudades();
                    Cities.IdPais = Country;
                    Cities.idciudad = (Last.idciudad + 1);
                    Cities.Nombre = info.OtherCity;
                    Cities.idEstado = info.state;
                    DB.Ciudades.Add(Cities);
                    DB.SaveChanges();

                    Last = DB.Ciudades.ToArray().Last();
                    IdCity = Last.idciudad.ToString();
                }
                catch (Exception ex) { }
            }

            var query = (from p in DB.Paciente
                         join reg in DB.Registro on p.IdRegistro equals reg.idRegistro
                         where p.idPaciente == idPatient
                         select new { reg, p }).FirstOrDefault();
            Paciente patient = query.p;
            Registro register = query.reg;

            patient.Sexo = info.sex;
            patient.Etapa = byte.Parse(info.lifeStage);
            patient.TipoEscolaridad = byte.Parse(info.education);
            patient.Escolaridad = info.typeEducation;
            patient.estadocivil = int.Parse(info.civilStatus);
            patient.ocupacion = info.ocupation;
            patient.Curp = info.Curp;
            patient.telefonooficina = info.officePhone;
            register.Nombre = info.name;
            register.Apellido = info.LastName;
            register.Domicilio = info.address;
            register.Colonia = info.colony;
            register.CodigoPostal = info.cp;
            register.IdPais = info.country;
            register.Telefono = info.phone;
            register.TelefonoCel = info.cellPhone;
            register.Emails = info.email;
            register.idEstado = info.state;
            register.idCiudad = int.Parse(IdCity);
            info.city = IdCity;
            
            if (patient.acepto != null)
                info.acepto = (bool)patient.acepto;
            else info.acepto = false;

            Load_States(info.country);
            Load_Cities(info.state, info.country);
            DB.SaveChanges();
            ModelState.Clear();

            return View(info);
        }





        









        /// <summary>
        /// Historial de consultas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Consultation()
        {
            ConsultationShow(0);
            TreatmentByDate();
            return View();
        }

        [HttpPost]
        public ActionResult Consultation(int date)
        {
            ConsultationShow(date);
            TreatmentByDate();
            return View();
        }

        public void ConsultationShow(int queryId)
        {
            var cnsult = (from c in DB.Consultas
                          join g in DB.ConsultaGinecologa
                          on c.idconsulta equals g.idconsulta into joined
                          from j in joined.DefaultIfEmpty()
                          where c.idpaciente == idPatient && j == null
                          orderby c.Fecha descending
                          select c).ToList();

            List<SelectListItem> iConsultation = new List<SelectListItem>();
            foreach (var i in cnsult) {
                iConsultation.Add(new SelectListItem { Text = i.Fecha.ToString(), Value = i.idconsulta.ToString() });
            }

            ViewBag.consultation = iConsultation;

            /**/
            int id;
            if (queryId == 0 && cnsult.Count>0)
                id = cnsult[0].idconsulta;
            else id = queryId;

            var data = (from c in DB.Consultas
                          where c.idconsulta == id
                          select c).ToList();

            //EDAD DEL PACIENTE
            var Patient = (from R in DB.Registro
                           join P in DB.Paciente on R.idRegistro equals P.IdRegistro
                           where P.idPaciente == idPatient
                           select new { P.FechaNacimiento }).FirstOrDefault();
            if (Patient != null && Patient.FechaNacimiento.HasValue)
            {
                if (cnsult.Count > 0)
                {
                    DateTime? QueryDate = cnsult.Single(x => x.idconsulta == id).Fecha.Value;
                    if (QueryDate.HasValue)
                    {
                        int TotalMonths = (QueryDate.Value.Year - Patient.FechaNacimiento.Value.Year) * 12;
                        TotalMonths += QueryDate.Value.Month - Patient.FechaNacimiento.Value.Month;
                        ViewBag.age = (int)(TotalMonths / 12);
                        int RemainingMonths = TotalMonths % 12;
                        if (RemainingMonths > 0)
                        {
                            ViewBag.age = $"{ViewBag.age} año(s) - {RemainingMonths} Mes(es)";
                        }
                    }
                    else ViewBag.age = string.Empty;
                }
                else ViewBag.age = string.Empty;
            }
            else ViewBag.age = string.Empty;



            if (data.Count>0)
            {
                ViewBag.temperature = data[0].Temperatura;
                ViewBag.reason = data[0].motivo;
                ViewBag.observations = data[0].observaciones;
                ViewBag.preventive = data[0].MedidasPreventivas;
                ViewBag.sistolica = data[0].TensionArterial;
                ViewBag.diastolica = data[0].TensionArterialB;
                ViewBag.cefalico = data[0].perimetroCefalico;
                ViewBag.meters = data[0].Altura;
                ViewBag.kilo = data[0].Peso;
                double masa = 0;
                if (data[0].Peso.HasValue && data[0].Altura.HasValue)
                {
                    masa = data[0].Peso.Value / (data[0].Altura.Value * data[0].Altura.Value);
                }

                ViewBag.mass = masa > 0 ? Math.Round(masa, 2).ToString() : "";

                ViewBag.symptom =
                    data[0].SignosSintomas1 + Environment.NewLine +
                    data[0].SignosSintomas2 + Environment.NewLine +
                    data[0].SignosSintomas3;
                ViewBag.date = id;

                int idpac = (int)data[0].idpaciente;
                DateTime Fecha = data[0].Fecha.Value.Date;
                int idcons = (int)data[0].idconsulta;

                // recetas
                var rec = from r in DB.Recetas
                          where r.idpaciente == idpac && r.idconsulta == idcons
                          orderby r.Fecha descending, r.idconsulta descending
                          select r;
                if (rec.Count() == 0)
                {
                    rec = from r in DB.Recetas
                          where r.idpaciente == idpac && EntityFunctions.TruncateTime(r.Fecha) == EntityFunctions.TruncateTime(Fecha)
                          orderby r.Fecha descending, r.idconsulta descending
                          select r;
                }
                string strRecetas = "";
                if (rec.Count() > 0)
                    strRecetas = rec.First().Lineas;
                else {                 // && EntityFunctions.TruncateTime(d.Fecha)== EntityFunctions.TruncateTime(Fecha)
                }

                // diagnosticos
                var diag = from d in DB.Diagnosticos
                           where d.idpaciente == idpac && d.idconsulta == idcons
                           orderby d.Fecha descending, d.idconsulta descending
                           select d;

                if (diag.Count() == 0)
                {
                    diag = from d in DB.Diagnosticos
                           where d.idpaciente == idpac && EntityFunctions.TruncateTime(d.Fecha) == EntityFunctions.TruncateTime(Fecha)
                           orderby d.Fecha descending, d.idconsulta descending
                           select d;
                }
                string strDiag = "";

                if (diag.Count() > 0) strDiag = diag.First().Lineas;
                else {
                    //EntityFunctions.TruncateTime(d.Fecha)== EntityFunctions.TruncateTime(Fecha)
                }



                ViewBag.treatment = formateaCadena(strRecetas);
                ViewBag.diagnosis = formateaCadena(strDiag);
            }
        }

        // formatea a texto la consulta

        string formateaCadena(string cadena)
        {
            string strCad = "";
            foreach (string c in cadena.Replace("\r\n", "\r").Split(new string[] { "\r" }, StringSplitOptions.None))
            {
                if (c.Length > 2)
                    strCad += c.Substring(2) + "\r\n";
            }
            return strCad;
        }

        private void TreatmentByDate()
        {
            string line = "";
            string[] lines;
            List<dynamic> myList = new List<dynamic>();
            dynamic row;

            var treatment = (from t in DB.Recetas
                             where t.idpaciente == idPatient
                             orderby t.Fecha descending
                             select t).ToList();
            if (treatment.Count > 0)
            {
                foreach(var i in treatment)
                {
                    line = i.Lineas.Replace(Environment.NewLine, "\r");
                    lines = line.Split('\r');
                    foreach(var l in lines)
                    {
                        if (l != "")
                        {
                            row = new ExpandoObject();
                            row.Fecha = i.Fecha;
                            row.report = l.Split('|')[0].Substring(2);
                            myList.Add(row);
                        }
                    }
                }
            }

            ViewBag.treatments = myList;
            
        }










        /// <summary>
        /// Obtiene las imagenes
        /// </summary>
        /// <param name="TypeScanning"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Scanning(int TypeScanning, string error = "")
        {
            var model = new ImagesModel();

            string type = TypeScanning == 0 ? "imgDiag" : "imgRec";
            string root = $"/imagenes/Expedientes/{idPatient}/{type}";

            List<dynamic> list = new List<dynamic>();
            if (Exists(Server.MapPath(root)))
            {
                try
                {
                    string path = ConfigurationManager.AppSettings["pathImages"] + root.Replace("/", "\\");
                    string[] allFiles = GetFiles(path);
                    foreach(string file in allFiles)
                    {
                        dynamic row = new ExpandoObject();

                        string nameImage = Path.GetFileName(file);
                        row.href = $"{root}/{nameImage}";

                        /*
                            hay que crear thumbnail si
                            no lo tiene
                        */

                        try
                        {
                            //Si existe se trae la imagen
                            var fileThumb = (from f in EnumerateFiles(Server.MapPath($"{root}/Thumbnail"))
                                             where f.Contains(nameImage)
                                             select $"{root}/Thumbnail/" + Path.GetFileName(f)).First();
                            row.thumb = fileThumb;
                        }
                        catch
                        {
                            //Error crear el thumbnail
                            CreateThumb(root, nameImage);
                            row.thumb = $"{root}/Thumbnail/{nameImage}";
                        }

                        row.name = nameImage;
                        row.showName = Path.GetFileNameWithoutExtension(nameImage);
                        list.Add(row);
                    }
                }
                catch { }
            }

            model.images = list;
            return View(model);
        }








        /// <summary>
        /// Crea thumbnail
        /// </summary>
        /// <param name="root">ruta</param>
        /// <param name="nameImage">nombre de la imagen</param>
        private void CreateThumb(string root, string nameImage)
        {
            try
            {
                int height = int.Parse(ConfigurationManager.AppSettings["heightThumb"]);
                int width = int.Parse(ConfigurationManager.AppSettings["widthThumb"]);

                Image img = Image.FromFile(Path.Combine(Server.MapPath(root), nameImage));

                Image thumb = img.GetThumbnailImage(width, height, null, IntPtr.Zero);
                img.Dispose();
                root += "/Thumbnail";

                string path = ConfigurationManager.AppSettings["pathImages"] + root.Replace("/", "\\");
                if (!Exists(Server.MapPath(root)))
                    CreateDirectory(path);

                thumb.Save(Path.Combine(Server.MapPath(root), nameImage));
            }
            catch { }
        }









        /// <summary>
        /// Guarda imagen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Scanning(ImagesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string type = model.typeScanning == 0 ? "imgDiag" : "imgRec";
                    string root = $"/imagenes/Expedientes/{idPatient}/{type}";

                    string path = ConfigurationManager.AppSettings["pathImages"] + root.Replace("/", "\\");
                    if (!Exists(Server.MapPath(root)))
                        CreateDirectory(path);

                    if (AllowMaxImage(model.file))
                    {
                        //Reenombrar imagen
                        string extension = Path.GetExtension(model.file.FileName);
                        string renameImage = null;
                        if (!ExistImage(path, model.nameImage + extension))
                        {
                            renameImage = Path.Combine(Server.MapPath(root), model.nameImage + extension);
                        }

                        if (renameImage != null)
                        {
                            model.file.SaveAs(renameImage);
                            CreateThumb(root, (model.nameImage + extension));
                        }
                        else return RedirectToAction("Scanning", "Expedient", new { TypeScanning = model.typeScanning, error = "1" });

                    }
                    else return RedirectToAction("Scanning", "Expedient", new { TypeScanning = model.typeScanning, error = "2" });
                }
            }
            catch { }

            return RedirectToAction("Scanning", "Expedient", new { TypeScanning = model.typeScanning });
        }




        /// <summary>
        /// Cambia la imagen del paciente 
        /// en el header
        /// </summary>
        /// <param name="fileInput">Imagen</param>
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult ChangeImgPatient(HttpPostedFileBase fileInput)
        {
            int status;
            try
            {
                string root = $"/imagenes/Expedientes/{idPatient}/Perfil";

                string path = ConfigurationManager.AppSettings["pathImages"] + root.Replace("/", "\\");
                if (Exists(Server.MapPath(root)))
                {
                    //Eliminar toda las posibles imagenes.
                    string[] allFiles = GetFiles(path);
                    foreach (string file in allFiles)
                    {
                        System.IO.File.Delete(file);
                    }
                }
                else CreateDirectory(path);

                if (AllowMaxImage(fileInput))
                {
                    var file = Path.Combine(Server.MapPath(root), fileInput.FileName);
                    fileInput.SaveAs(file);
                    status = 200;
                }
                else status = 415;

            }
            catch { status = 500; }

            return Json(new { status = status.ToString() }, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// Comprueba que no sea mayor 
        /// al maximo de tamaño
        /// </summary>
        /// <param name="file">Imagen</param>
        /// <returns></returns>
        private bool AllowMaxImage(HttpPostedFileBase file) => file.ContentLength > maxContent ? false : true;







        /// <summary>
        /// pregunta si la imagen existe
        /// </summary>
        /// <param name="path">ruta</param>
        /// <param name="nameImage">nombre imagen</param>
        /// <returns></returns>
        private bool ExistImage(string path, string nameImage)
        {
            bool exist = false;
            string[] allFiles = GetFiles(path);
            foreach (string file in allFiles)
            {
                if (Path.GetFileName(file).ToLower() == nameImage.ToLower())
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }






        /// <summary>
        /// Borra imagen
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public ActionResult DeleteImage(string img, int type)
        {
            bool status = false;
            try
            {
                img = HttpUtility.UrlDecode(img);
                if (!string.IsNullOrEmpty(img))
                {
                    string _type = type == 0 ? "imgDiag" : "imgRec";
                    string root = $"/imagenes/Expedientes/{idPatient}/{_type}";
                    if (Exists(Server.MapPath(root)))
                    {
                        string file = Path.Combine(Server.MapPath(root), img);
                        System.IO.File.Delete(file);

                        root += "/Thumbnail";

                        status = true;
                        if (Exists(Server.MapPath(root)))
                        {
                            file = Path.Combine(Server.MapPath(root), img);
                            System.IO.File.Delete(file);
                        }
                    }
                }
            }
            catch { }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }








        [HttpGet]
        public ActionResult Gynecological()
        {
            GynecologicalConsultation(0);
            return View();
        }

        [HttpPost]
        public ActionResult Gynecological(int date)
        {
            GynecologicalConsultation(date);
            return View();
        }

        public void GynecologicalConsultation(int queryId)
        {
            var cnsult = (from c in DB.Consultas
                          join g in DB.ConsultaGinecologa
                          on c.idconsulta equals g.idconsulta into joined
                          from j in joined.DefaultIfEmpty()
                          where j != null && c.idpaciente == idPatient
                          orderby c.Fecha descending
                          select new
                          {
                              gine = j,
                              c.Fecha,
                              c.Peso,
                              c.Altura,
                              c.TensionArterial,
                              c.TensionArterialB,
                              c.Temperatura,
                              c.motivo
                          }).ToList();

            List<SelectListItem> iConsultation = new List<SelectListItem>();
            foreach (var i in cnsult) {
                iConsultation.Add(new SelectListItem { Text = i.Fecha.ToString(), Value = i.gine.idconsulta.ToString() });
            }

            ViewBag.consultation = iConsultation;

            if (cnsult.Count > 0)
            {
                int id;
                if (queryId == 0)
                    id = cnsult[0].gine.idconsulta;
                else id = queryId;

                var data = (from c in cnsult
                            where c.gine.idconsulta == id
                            select c).ToList();

                var pac = (from p in DB.Paciente
                           where p.idPaciente == idPatient
                           select p).First();

                ViewBag.kilo = data[0].Peso;
                ViewBag.meter = data[0].Altura;
                double masa = 0;
                if (data[0].Peso.HasValue && data[0].Altura.HasValue)
                {
                    masa = data[0].Peso.Value / (data[0].Altura.Value * data[0].Altura.Value);
                }

                ViewBag.mass = masa > 0 ? Math.Round(masa, 2).ToString() : "";
                ViewBag.menarca = pac.EdadMenarca;
                ViewBag.temperature = data[0].Temperatura;
                ViewBag.sistolica = data[0].TensionArterial;
                ViewBag.diastolica = data[0].TensionArterialB;
                if (data[0].gine.FechaUltimaMestruacion.HasValue)
                    ViewBag.menstruation = data[0].gine.FechaUltimaMestruacion.Value;
                ViewBag.gesta = data[0].gine.Gestas;
                ViewBag.paragesta = data[0].gine.ParaGestas;
                ViewBag.cesarea = data[0].gine.Cesareas;
                ViewBag.abortion = data[0].gine.abortos;
                ViewBag.nacido = data[0].gine.RecienNacidosVivos;
                ViewBag.mortinato = data[0].gine.mortinatos;
                ViewBag.sexualAge = data[0].gine.EdadInicioVidaSexual;
                ViewBag.menacma = data[0].gine.menacma;
                ViewBag.rh = pac.grupoRH;

                ViewBag.oligomenorrea = data[0].gine.oligonorrea.HasValue ? data[0].gine.oligonorrea.Value : false;
                ViewBag.proimenorrea = data[0].gine.Proiomenorrea.HasValue ? data[0].gine.Proiomenorrea.Value : false;
                ViewBag.hipermenorrea = data[0].gine.Hipermenorrea.HasValue ? data[0].gine.Hipermenorrea.Value : false;
                ViewBag.dismenorrea = data[0].gine.Dismenorrea.HasValue ? data[0].gine.Dismenorrea.Value : false;
                ViewBag.dispareunia = data[0].gine.Dispareunia.HasValue ? data[0].gine.Dispareunia.Value : false;
                ViewBag.leucorrea = data[0].gine.Leucorrea.HasValue ? data[0].gine.Leucorrea.Value : false;
                ViewBag.lactorrea = data[0].gine.Lactorrea.HasValue ? data[0].gine.Lactorrea.Value : false;
                ViewBag.amenorrea = data[0].gine.Amenorrea.HasValue ? data[0].gine.Amenorrea.Value : false;
                ViewBag.metrorragia = data[0].gine.Metrorragia.HasValue ? data[0].gine.Metrorragia.Value : false;
                ViewBag.others = data[0].gine.Otros.HasValue ? data[0].gine.Otros.Value : false;
                ViewBag.explain = data[0].gine.OtrosEspecifique;

                ViewBag.hasPartner = data[0].gine.TienePareja.HasValue ? (data[0].gine.TienePareja.Value ? "Si" : "No") : "No";
                ViewBag.partnerName = data[0].gine.nombrePareja;
                ViewBag.partnerSex = data[0].gine.SexoPareja == null ? "Masculino" : (data[0].gine.SexoPareja == "M" ? "Masculino" : "Femenino");
                string civilStatus = "";
                switch (data[0].gine.EstadoCivilPareja ?? "")
                {
                    case "0":
                        civilStatus = "Casado";
                        break;
                    case "1":
                        civilStatus = "Separado";
                        break;
                    case "2":
                        civilStatus = "Soltero";
                        break;
                    case "3":
                        civilStatus = "Union libre";
                        break;
                    case "4":
                        civilStatus = "Viudo";
                        break;
                    case "5":
                        civilStatus = "Divorciado";
                        break;
                }
                ViewBag.partnerCivilStatus = civilStatus;
                ViewBag.partnerRh = data[0].gine.GrupoRHPareja;
                if (data[0].gine.FechaNacimientoPareja.HasValue)
                    ViewBag.partnerDate = data[0].gine.FechaNacimientoPareja.Value;
                ViewBag.partnerAge = data[0].gine.edadPareja;
                ViewBag.partnerOcupation = data[0].gine.OcupacionPareja;
                ViewBag.partnerPhone = data[0].gine.TelefonoPareja;
                ViewBag.reason = data[0].motivo;
                
            }
            else
            {
                ViewBag.oligomenorrea = false;
                ViewBag.proimenorrea = false;
                ViewBag.hipermenorrea = false;
                ViewBag.dismenorrea = false;
                ViewBag.dispareunia = false;
                ViewBag.leucorrea = false;
                ViewBag.lactorrea = false;
                ViewBag.amenorrea = false;
                ViewBag.metrorragia = false;
                ViewBag.others = false;
            }
            
        }


        
        [HttpGet]
        public ActionResult Obstetrica()
        {
            ObstetricaConsultation(0);
            return PartialView();
        }

        [HttpPost]
        public ActionResult Obstetrica(int date)
        {
            ObstetricaConsultation(date);
            return PartialView();
        }

        public void ObstetricaConsultation(int queryId)
        {
            var cnsult = (from c in DB.Consultas
                          join g in DB.ConsultaObstetrica
                          on c.idconsulta equals g.idconsulta into joined
                          from j in joined.DefaultIfEmpty()
                          where j != null && c.idpaciente == idPatient
                          orderby c.Fecha descending
                          select new
                          {
                              obst = j,
                              c.Fecha,
                              c.Peso,
                              c.Altura,
                              c.TensionArterial,
                              c.TensionArterialB,
                              c.Temperatura,
                              c.motivo
                          }).ToList();

            List<SelectListItem> iConsultation = new List<SelectListItem>();
            foreach (var i in cnsult)
            {
                iConsultation.Add(new SelectListItem { Text = i.Fecha.ToString(), Value = i.obst.idconsulta.ToString() });
            }

            ViewBag.consultation = iConsultation;

            if (cnsult.Count > 0)
            {
                int id;
                if (queryId == 0)
                    id = cnsult[0].obst.idconsulta.Value;
                else id = queryId;

                var data = (from c in cnsult
                            where c.obst.idconsulta == id
                            select c).ToList();

                ViewBag.noEmbarazo = data[0].obst.noembarazo.Value;
                ViewBag.peso = data[0].Peso;
                ViewBag.altura = data[0].Altura;
                double masa = 0;
                if (data[0].Peso.HasValue && data[0].Altura.HasValue)
                {
                    masa = data[0].Peso.Value / (data[0].Altura.Value * data[0].Altura.Value);
                }
                ViewBag.masa = masa > 0 ? Math.Round(masa, 2).ToString() : "";
                ViewBag.activaSexualmente = data[0].obst.activaSexualmente.HasValue ? Convert.ToInt32(data[0].obst.activaSexualmente.Value) : 0;
                ViewBag.temperature = data[0].Temperatura;
                ViewBag.sistolica = data[0].TensionArterial;
                ViewBag.diastolica = data[0].TensionArterialB;
                ViewBag.abortos = data[0].obst.abortos.HasValue ? Convert.ToInt32(data[0].obst.abortos.Value) : 0;
                if (data[0].obst.FechaUltmoParto.HasValue)
                    ViewBag.ultimaParto = data[0].obst.FechaUltmoParto.Value;
                else ViewBag.ultimaParto = "";
                if (data[0].obst.PrimerDiaUltimaMestruacuion.HasValue)
                    ViewBag.ultimaMenstruacion = data[0].obst.PrimerDiaUltimaMestruacuion.Value;
                else ViewBag.ultimaMenstruacion = "";
                ViewBag.toxemias = data[0].obst.ToxemiasPrevias.HasValue ? Convert.ToInt32(data[0].obst.ToxemiasPrevias.Value) : 0;
                ViewBag.expToxemias = data[0].obst.EspecifiqueToxemias;
                ViewBag.partos = data[0].obst.Partos.HasValue ? data[0].obst.Partos.Value : 0;
                ViewBag.tipoDistocia = data[0].obst.TipoDistocia.HasValue ? data[0].obst.TipoDistocia.Value : 0;
                ViewBag.espTipoDistocia = data[0].obst.EspecifiqueTipoDistocia;
                ViewBag.motivoDistocia = data[0].obst.MotivoDistocia.HasValue ? data[0].obst.MotivoDistocia.Value : 0;
                ViewBag.espMotivoDistocia = data[0].obst.EspecifiqueMotivoDistocia;
                ViewBag.cesarea = data[0].obst.CesareasPrevia.HasValue ? Convert.ToInt32(data[0].obst.CesareasPrevia.Value) : 0;
                ViewBag.forceps = data[0].obst.UsoDeForceps.HasValue ? Convert.ToInt32(data[0].obst.UsoDeForceps.Value) : 0;
                ViewBag.mortinatos = data[0].obst.Motinatos.HasValue ? Convert.ToInt32(data[0].obst.Motinatos.Value) : 0;
                ViewBag.rnvivos = data[0].obst.RMVivos.HasValue ? Convert.ToInt32(data[0].obst.RMVivos.Value) : 0;
                ViewBag.ectopico = data[0].obst.EmbarazoEtopicos.HasValue ? Convert.ToInt32(data[0].obst.EmbarazoEtopicos.Value) : 0;
                ViewBag.espEctopico = data[0].obst.EmbrazoEtopicoExplique;
                ViewBag.embarazosPrevios = data[0].obst.EmbrazosComplicadosPrevios.HasValue ? Convert.ToInt32(data[0].obst.EmbrazosComplicadosPrevios.Value) : 0;
                ViewBag.espEmbarazosPrevios = data[0].obst.EmbarazosComplicadosExplique;
                ViewBag.perinatales = data[0].obst.NoComplicacionesPertinales.HasValue ? Convert.ToInt32(data[0].obst.NoComplicacionesPertinales.Value) : 0;
                ViewBag.espPerinatales = data[0].obst.ComplicacionesPerinatalesExplique;
                ViewBag.anormales = data[0].obst.NoEmbrazosAnormales.HasValue ? Convert.ToInt32(data[0].obst.NoEmbrazosAnormales.Value) : 0;
                ViewBag.espAnormales = data[0].obst.EmbarazosAnormalesExplique;
                ViewBag.observaciones = data[0].obst.Observaciones;

                ViewBag.gestacion = Gestacion(data[0].obst.PrimerDiaUltimaMestruacuion);
                ViewBag.fu = data[0].obst.FU;
                ViewBag.fcf = data[0].obst.FCF;
                ViewBag.cc = data[0].obst.CC;
                ViewBag.ca = data[0].obst.CA;
                ViewBag.lf = data[0].obst.LF;
                ViewBag.dbp = data[0].obst.DSP;
                ViewBag.posicion = data[0].obst.Posicion;
                ViewBag.presentacion = data[0].obst.Presentacion;
                ViewBag.situacion = data[0].obst.siuacuion;
                ViewBag.actitud = data[0].obst.Actitud;
                ViewBag.fetales = data[0].obst.MovimientosFetales;
                ViewBag.pesoProducto = data[0].obst.PesoAproxProducto;
                double pesoMadre = 0;
                if (data[0].Peso.HasValue && data[0].obst.PesoAproxProducto.HasValue)
                {
                    pesoMadre = data[0].Peso.Value - data[0].obst.PesoAproxProducto.Value;
                }
                ViewBag.pesoMadre = pesoMadre > 0 ? Math.Round(pesoMadre, 2).ToString() : "";
                ViewBag.ta = data[0].obst.TA;
                ViewBag.fcm = data[0].obst.FCM;
                ViewBag.edema = data[0].obst.Edema;
                ViewBag.hizous = data[0].obst.SeHizoUf.HasValue ? Convert.ToInt32(cnsult[0].obst.SeHizoUf.Value) : 0;
                ViewBag.ultrasonido = data[0].obst.ultrasonido;
                ViewBag.motivoConsulta = data[0].motivo;
                ViewBag.exploracion = data[0].obst.exploracionFisica;
            }
        }



        private string Gestacion(DateTime? LastMenstruation)
        {
            string gest = "";
            if (LastMenstruation.HasValue)
            {
                double diffDays = (DateTime.Now.Date - LastMenstruation.Value).TotalDays;

                int weeks = 0;
                int days = 0;
                if (diffDays > 0)
                {
                    int rank = 6;
                    bool pending = false;
                    for (int i = 0; i < diffDays; i++)
                    {
                        if (i == rank)
                        {
                            weeks++;
                            rank = rank + 7;
                            pending = false;
                        }
                        else pending = true;
                    }

                    if (pending)
                    {
                        if (rank > 6)
                        {
                            for (int b = (weeks * 7); b <= diffDays; b++) days++;
                        }
                        else days = (int)diffDays;
                    }
                }

                gest = weeks.ToString() + " / " + days.ToString();
            }

            return gest;
        }













        /// <summary>
        /// Antecedentes del nacimiento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataBirth()
        {
            var model = new ModelDataBirth();
            var query = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p).FirstOrDefault();
            if (query.FechaNacimiento.HasValue)
                model.birthDate = query.FechaNacimiento.Value.ToString("dd/MM/yyyy");
            model.birthPlace = query.LugarNacer;
            model.birthHospital = query.HospitalNacer;
            model.pregnantNumber = query.EmbarazoNo;
            model.abortions = query.AbortosPrevios;
            if (query.TipoEmbarazo != null)
            {
                model.pregnantType = Convert.ToByte(query.TipoEmbarazo.Substring(0, 1));
                model.pregnantTypeInfo = Convert.ToByte(query.TipoEmbarazo.Substring(1, 1));
            }

            model.obtainedByBirth = Convert.ToByte(query.EmbarazoParto);
            model.mortinatos = query.mortinatos;
            ViewBag.gestationalAge = GestationalAge();
            model.gestationalAge = query.EdadGestal;
            model.gestationalAgeBy = query.tipoEdadGestal;
            model.typeDistocia = byte.Parse(query.tipodistocia ?? "0");
            model.typeDistociaInfo = query.tipodistociamencione;
            model.reasonDistocia = byte.Parse(query.motivodistocia ?? "0");
            model.reasonDistociaInfo = query.motivodistociamencione;
            model.birthWeight = query.PesoNacer.HasValue ? query.PesoNacer.Value : 0;
            model.birthSize = query.TallaNacer.HasValue ? query.TallaNacer.Value : 0;
            model.cefalico = query.PerCefalicoNacer.HasValue ? query.PerCefalicoNacer.Value : 0;
            model.scoreApgar1 = query.CalApgarMin;
            model.scoreApgar5 = query.CalApgar5Min;
            model.scoreSilverman1 = query.CalSilverMin;
            model.scoreSilverman5 = query.CalSilver5Min;
            model.tamiz = byte.Parse(query.Tamizneonatal ?? "0");
            model.folio = query.NoFolioTamiz;
            model.complicationIn = query.CompINM;
            model.controlIn = query.CompINMManejo;
            model.complicationMe = query.CompMed;
            model.controlMe = query.CompMedManejo;
            model.lactancia = byte.Parse(query.lactanciaMat ?? "1");
            model.typeLactancia = byte.Parse(query.tipolactancia ?? "0");
            model.formula = query.nombreformula;
            model.reduccion = query.motivoreduccion;
            model.suspension = query.motivoSuspension;
            model.sanguineo = query.grupoRH;
            if (query.fechaConsultaPed.HasValue)
                model.date = query.fechaConsultaPed.Value.ToString("dd/MM/yyyy");

            return View(model);
        }

        private List<SelectListItem> GestationalAge()
        {
            var list = new List<SelectListItem>();
            for (int i = 20; i < 42; i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            } list.Add(new SelectListItem { Text = "42 o más", Value = "42" });

            return list;
        }

        [HttpPost]
        public ActionResult DataBirth(ModelDataBirth model)
        {
            Paciente pat;
            pat = (from p in DB.Paciente
                   where p.idPaciente == idPatient
                   select p).First();

            //informacion del nacimiento
            pat.FechaNacimiento = model.birthDate == null ? (DateTime?)null : Convert.ToDateTime(model.birthDate);
            pat.LugarNacer = model.birthPlace;
            pat.HospitalNacer = model.birthHospital;
            //embarazo
            pat.EmbarazoNo = model.pregnantNumber;
            pat.AbortosPrevios = model.abortions;
            pat.TipoEmbarazo = model.pregnantType.ToString() + model.pregnantTypeInfo.ToString();
            pat.EmbarazoParto = model.obtainedByBirth.ToString();
            pat.mortinatos = model.mortinatos;
            pat.EdadGestal = model.gestationalAge;
            pat.tipoEdadGestal = model.gestationalAgeBy;
            pat.tipodistocia = model.typeDistocia.ToString();
            pat.tipodistociamencione = model.typeDistociaInfo;
            pat.motivodistocia = model.reasonDistocia.ToString();
            pat.motivodistociamencione = model.reasonDistociaInfo;
            pat.PesoNacer = model.birthWeight;
            pat.TallaNacer = model.birthSize;
            pat.PerCefalicoNacer = model.cefalico;
            pat.CalApgarMin = model.scoreApgar1;
            pat.CalApgar5Min = model.scoreApgar5;
            pat.CalSilverMin = model.scoreSilverman1;
            pat.CalSilver5Min = model.scoreSilverman5;
            pat.Tamizneonatal = model.tamiz.ToString();
            pat.NoFolioTamiz = model.folio;
            //complicaciones inmediatas
            pat.CompINM = model.complicationIn;
            pat.CompINMManejo = model.controlIn;
            //complicaciones mediatas
            pat.CompMed = model.complicationMe;
            pat.CompMedManejo = model.controlMe;
            //lactancia
            pat.lactanciaMat = model.lactancia.ToString();
            pat.tipolactancia = model.typeLactancia.ToString();
            pat.nombreformula = model.formula;
            pat.motivoreduccion = model.reduccion;
            pat.motivoSuspension = model.suspension;
            //mas informacion
            pat.grupoRH = model.sanguineo;
            pat.fechaConsultaPed = model.date == null ? (DateTime?)null : Convert.ToDateTime(model.date);

            ViewBag.gestationalAge = GestationalAge();
            DB.SaveChanges();
            ModelState.Clear();

            return View(model);
        }











        /// <summary>
        /// Estudios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Studies()
        {
            string line = "";
            string[] lines;
            List<dynamic> myList = new List<dynamic>();
            dynamic row;

            var lab = (from l in DB.EstudiosLab
                       where l.idpaciente == idPatient
                       select l).ToList();

            if (lab.Count > 0)
            {
                foreach (var i in lab) {
                    line = i.Lineas.Replace(Environment.NewLine, "\r");
                    lines = line.Split('\r');
                    foreach (var l in lines) {
                        if (l != "")
                        {
                            row = new ExpandoObject();
                            row.name = l.Split('|')[0].Substring(2);
                            row.date = i.Fecha;
                            row.report = l.Split('|').Length > 1 ? l.Split('|')[1] : "";
                            myList.Add(row);
                        }
                    }
                }
            } ViewBag.listLab = myList;

            List<dynamic> myList2 = new List<dynamic>();
            var gab = (from g in DB.EstudiosGab
                       where g.idpaciente == idPatient
                       select g).ToList();
            if (gab.Count > 0)
            {
                foreach (var i in gab)
                {
                    line = i.Lineas.Replace(Environment.NewLine, "\r");
                    lines = line.Split('\r');
                    foreach (var l in lines) {
                        if (l != "")
                        {
                            row = new ExpandoObject();
                            row.name = l.Split('|')[0].Substring(2);
                            row.date = i.Fecha;
                            row.report = l.Split('|').Length > 1 ? l.Split('|')[1] : "";
                            myList2.Add(row);
                        }
                    }
                }
            } ViewBag.listGab = myList2;

            return View();
        }












        /// <summary>
        /// Otras consultas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OtherQueries()
        {
            ViewBag.query = (from p in DB.Paciente
                             where p.idPaciente == idPatient
                             select p.OtrasConsultas).FirstOrDefault();

            return View();
        }

        [HttpPost]
        public ActionResult OtherQueries(string query)
        {
            Paciente pat;
            pat = (from p in DB.Paciente
                   where p.idPaciente == idPatient
                   select p).First();
            pat.OtrasConsultas = query;
            DB.SaveChanges();

            return View();
        }














        /// <summary>
        /// Vacunas
        /// </summary>
        /// <returns></returns>
        public ActionResult Vaccines()
        {
            LoadVaccines();
            return View();
        }

        private Dictionary<int, dynamic> ListOfVaccines()
        {
            Dictionary<int, dynamic> vacc = new Dictionary<int, dynamic>();

            vacc.Add(1, new { name = "BCG", sickness = "Tuberculosis" });
            vacc.Add(2, new { name = "HB", sickness = "Hepatitis B" });
            vacc.Add(3, new { name = "PENTAVALENTE DPT + IPV + Hib", sickness = "Difteria, Tosferina, Tétanos, Poliovirus Inactivado, Haemophilus tipo b" });
            vacc.Add(4, new { name = "RV", sickness = "Rotavirus" });
            vacc.Add(5, new { name = "Neumococo", sickness = "Neumococo" });
            vacc.Add(6, new { name = "HA", sickness = "Hepatitis A" });
            vacc.Add(7, new { name = "Varicela", sickness = "Varicela" });
            vacc.Add(8, new { name = "Triple Viral", sickness = "Sarampión, Parotiditis y Rubéola" });
            vacc.Add(9, new { name = "VPH", sickness = "Virus del Papiloma Humano" });
            vacc.Add(10, new { name = "Gripe", sickness = "Influenza" });

            return vacc;
        }

        private dynamic row;
        List<dynamic> vacc = new List<dynamic>();
        private void LoadVaccines()
        {
            var vac = (from v in DB.Vacunas
                       where v.idpaciente == idPatient
                       select v).ToList();
            if (vac != null && vac.Count > 0)
            {
                foreach(KeyValuePair<int, dynamic> v in ListOfVaccines())
                {
                    bool haveRow = false;
                    foreach (var vcc in vac)
                        if (int.Parse(vcc.codigo) == v.Key) haveRow = true;
                    if (!haveRow)
                        DefaultVaccines(v.Key, v.Value);
                    else
                    {
                        bool first = true;
                        foreach(var vcc in vac) {
                            if (int.Parse(vcc.codigo) == v.Key)
                            {
                                row = new ExpandoObject();
                                row.vaccine = first ? v.Value.name : "";
                                row.sickness = first ? v.Value.sickness : "";
                                row.dose = vcc.vacunadosis;
                                row.age = vcc.vacunaEdad;
                                if (vcc.Fecha.HasValue)
                                    row.date = vcc.Fecha.Value.ToString("dd/MM/yyyy");
                                else row.date = "";
                                row.index = v.Key;
                                row.id = vcc.idvacuna;
                                vacc.Add(row);
                                first = false;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach(KeyValuePair<int, dynamic> v in ListOfVaccines())
                    DefaultVaccines(v.Key, v.Value);
            }

            ViewBag.vaccines = vacc;
        }

        private int AddVaccine(dynamic val)
        {
            Vacunas _vac = new Vacunas();
            _vac.idpaciente = idPatient;
            _vac.vacunaEdad = val.age;
            _vac.vacunaenfprev = Convert.ToString(val.index) == "3" ? "" : val.sickness;
            _vac.codigo = Convert.ToString(val.index);
            _vac.Fecha = val.date == "" ? (DateTime?)null : Convert.ToDateTime(val.date, CultureInfo.GetCultureInfo("es-MX"));
            _vac.vacunadosis = val.dose;
            DB.Vacunas.Add(_vac);
            DB.SaveChanges();

            var id = (from v in DB.Vacunas
                      where v.idpaciente == idPatient
                      select v).ToList().Last();
            return id.idvacuna;
        }

        private void DefaultVaccines(int Index, dynamic TypeVacc)
        {
            
            switch (Index)
            {
                case 1:
                    row = new ExpandoObject();
                    row.vaccine = TypeVacc.name;
                    row.sickness = TypeVacc.sickness;
                    row.dose = "Única";
                    row.age = "Al nacer";
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 9:
                case 10:
                    row = new ExpandoObject();
                    row.vaccine = TypeVacc.name;
                    row.sickness = TypeVacc.sickness;
                    row.dose = "Primera";
                    string age = "";
                    switch (Index)
                    {
                        case 2: age = "Al nacer"; break;
                        case 3:
                        case 4:
                        case 5: age = "2 Meses"; break;
                        case 9: age = "9 Años"; break;
                        case 10: age = "6 Años"; break;
                    }
                    row.age = age;
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);
                    //--
                    row = new ExpandoObject();
                    row.vaccine = "";
                    row.sickness = "";
                    row.dose = "Segunda";
                    age = "";
                    switch (Index)
                    {
                        case 2: age = "2 Meses"; break;
                        case 3:
                        case 4:
                        case 5: age = "4 Meses"; break;
                        case 9: age = "2 Meses después"; break;
                        case 10: age = "7 Meses"; break;
                    }
                    row.age = age;
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);
                    //--
                    row = new ExpandoObject();
                    row.vaccine = "";
                    row.sickness = "";
                    row.dose = "Tercera";
                    age = "";
                    switch (Index)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5: age = "6 Meses"; break;
                        case 9: age = "6 Meses después"; break;
                        case 10: age = "Anual"; break;
                    }
                    row.age = age;
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);

                    //--
                    if (Index == 5)
                    {
                        row = new ExpandoObject();
                        row.vaccine = "";
                        row.sickness = "";
                        row.dose = "Refuerzo";
                        row.age = "15 Meses";
                        row.date = "";
                        row.index = Index;
                        row.id = AddVaccine(row);
                        vacc.Add(row);
                    }
                    break;
                case 6:
                case 7:
                case 8:
                    row = new ExpandoObject();
                    row.vaccine = TypeVacc.name;
                    row.sickness = TypeVacc.sickness;
                    row.dose = "Primera";
                    row.age = "1 Año";
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);
                    //--
                    row = new ExpandoObject();
                    row.vaccine = "";
                    row.sickness = "";
                    row.dose = Index == 8 ? "Refuerzo" : "Segunda";
                    row.age = Index == 6 ? "18 Meses" : Index == 7 ? "13 Años" : "6 Años";
                    row.date = "";
                    row.index = Index;
                    row.id = AddVaccine(row);
                    vacc.Add(row);
                    break;
            }
        }


        public ActionResult SaveVaccines(string data)
        {
            bool status = true;
            try
            {
                if (data != string.Empty)
                {
                    foreach(var vacc in data.Split('|'))
                    {
                        string[] info = vacc.Split(',');
                        if (info.Length > 0)
                        {
                            switch (int.Parse(info[0]))
                            {
                                case 1:
                                    //UPDATE DATE
                                    if (info.Length >= 2)
                                    {
                                        DateTime? _date = info[2] != string.Empty ? Convert.ToDateTime(info[2]) : (DateTime?) null;
                                        UpdateVaccine(int.Parse(info[1]), _date);
                                    }
                                    break;
                                case 2:
                                    //ADD VACCINE
                                    if (info.Length >= 4)
                                    {
                                        dynamic newV = new ExpandoObject();
                                        newV.index = info[1];
                                        newV.dose = info[2];
                                        newV.age = info[3];
                                        newV.date = info[4];
                                        newV.sickness = "";
                                        
                                        AddVaccine(newV);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { status = false; }

            return Json(new { status = status.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        private void UpdateVaccine(int idVaccine, DateTime? date)
        {
            Vacunas vac;
            vac = (from v in DB.Vacunas
                   where v.idpaciente == idPatient && v.idvacuna == idVaccine
                   select v).First();
            vac.Fecha = date;
            DB.SaveChanges();
        }
    }
}