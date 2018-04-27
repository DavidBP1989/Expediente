using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmeciExpediente.Models;

namespace EmeciExpediente.Controllers
{
    public class AntecedentController : BaseController
    {
        emeciEntities DB = new emeciEntities();
        public int idPatient { get { return (int)Session["IdPatient"]; } }





        /// <summary>
        /// Antecedentes patologicos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Pathological()
        {
            var model = new ModelPathological();
            var patient = (from p in DB.Paciente
                           where p.idPaciente == idPatient
                           select p).First();

            model.antecendet = patient.AntecedentePerionatal;
            if (patient.rubeola != null)
            {
                model.rubeola = CheckElement(patient.rubeola.ToString().Substring(0, 1));
                model.ageRubeola = int.Parse(patient.rubeola.ToString().Substring(1));
            }
            if (patient.sarampion != null)
            {
                model.sarampion = CheckElement(patient.sarampion.ToString().Substring(0, 1));
                model.ageSarampion = int.Parse(patient.sarampion.ToString().Substring(1));
            }
            if (patient.tosferina != null)
            {
                model.tosFerina = CheckElement(patient.tosferina.ToString().Substring(0, 1));
                model.ageTosFerina = int.Parse(patient.tosferina.ToString().Substring(1));
            }
            if (patient.roseola != null)
            {
                model.roseola = CheckElement(patient.roseola.ToString().Substring(0, 1));
                model.ageRoseola = int.Parse(patient.roseola.ToString().Substring(1));
            }
            if (patient.varicela != null)
            {
                model.varicela = CheckElement(patient.varicela.ToString().Substring(0, 1));
                model.ageVaricela = int.Parse(patient.varicela.ToString().Substring(1));
            }
            if (patient.EnfContagiosas != null)
            {
                string enf = patient.EnfContagiosas.ToString();
                model.faringitis = CheckElement(enf.Substring(0, 1));
                model.amigdalitis = CheckElement(enf.Substring(1, 1));
                model.parasitosis = CheckElement(enf.Substring(2, 1));
                model.hepatitis = CheckElement(enf.Substring(3, 1));
                model.infeccion = CheckElement(enf.Substring(4, 1));
                model.gastro = CheckElement(enf.Substring(5, 1));
                model.otitis = CheckElement(enf.Substring(6, 1));
                model.conjutivitis = CheckElement(enf.Substring(7, 1));
                model.micosis = CheckElement(enf.Substring(8, 1));
                model.bronquitis = CheckElement(enf.Substring(9, 1));
                model.bronco = CheckElement(enf.Substring(10, 1));
                model.estomatitis = CheckElement(enf.Substring(11, 1));
            }

            model.otherDisease = patient.EnfInfectocontagiosas;
            model.otherContagious = patient.EnfContagiosasyEvol;
            SetAges();
            GetPathological();

            return View(model);
        }

        private void SetAges()
        {
            List<SelectListItem> ages = new List<SelectListItem>();
            for (int i = 0; i <= 18; i++)
            {
                if (i == 0)
                    ages.Add(new SelectListItem { Text = "Seleccione", Value = "-1" });
                else ages.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            } ViewBag.ages = ages;
        }
        private bool CheckElement(string input) => input == "1" ? true : false;
        private string NumBoolean(bool input) => input ? "1" : "0";

        private void GetPathological()
        {
            var patho = (from pth in DB.Patologias
                         where pth.idpaciente == idPatient
                         select pth).ToList();

            List<Patologias> xList;
            xList = patho.Where(x => x.Categoria == 1).ToList();
            ViewBag.surgeries = xList;
            xList = patho.Where(x => x.Categoria == 2).ToList();
            ViewBag.trauma = xList;
            xList = patho.Where(x => x.Categoria == 5).ToList();
            ViewBag.diseases = xList;
            xList = patho.Where(x => x.Categoria == 6).ToList();
            ViewBag.otherDiseases = xList;
        }

        public ActionResult AddSurgery_Or_Trauma(int category, string name, string date, string observations)
        {
            Patologias p = new Patologias();
            p.Nombre = name;
            p.Categoria = category;
            p.idpaciente = idPatient;
            p.Fecha = date;
            p.Observaciones = observations;
            DB.Patologias.Add(p);
            DB.SaveChanges();

            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDiseases_Or_Others(int category, 
            string name, string alergeno, int age, string manejo, string evolution)
        {
            Patologias p = new Patologias();
            p.idpaciente = idPatient;
            p.Nombre = name;
            p.Manejo = manejo;
            p.Categoria = category;
            p.Evoluciones = evolution;
            p.EdadAdquirida = age;
            p.Alergeno = alergeno;
            DB.Patologias.Add(p);
            DB.SaveChanges();

            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int idPatologia)
        {
            Patologias p = (from patho in DB.Patologias
                            where patho.idpaciente == idPatient && patho.idpatologia == idPatologia
                            select patho).First();
            bool resp = false;
            try
            {
                DB.Patologias.Remove(p);
                DB.SaveChanges();
                resp = true;
            }
            catch {  }
            return Json(new { status = resp.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Pathological(ModelPathological model)
        {
            var pat = (from p in DB.Paciente
                       where p.idPaciente == idPatient
                       select p).First();

            pat.AntecedentePerionatal = model.antecendet;
            string enfContagiosas;
            enfContagiosas = NumBoolean(model.faringitis);
            enfContagiosas += NumBoolean(model.amigdalitis);
            enfContagiosas += NumBoolean(model.parasitosis);
            enfContagiosas += NumBoolean(model.hepatitis);
            enfContagiosas += NumBoolean(model.infeccion);
            enfContagiosas += NumBoolean(model.gastro);
            enfContagiosas += NumBoolean(model.otitis);
            enfContagiosas += NumBoolean(model.conjutivitis);
            enfContagiosas += NumBoolean(model.micosis);
            enfContagiosas += NumBoolean(model.bronquitis);
            enfContagiosas += NumBoolean(model.bronco);
            enfContagiosas += NumBoolean(model.estomatitis);
            pat.EnfContagiosas = enfContagiosas;
            pat.EnfContagiosasyEvol = model.otherContagious;
            pat.EnfInfectocontagiosas = model.otherDisease;

            pat.rubeola = NumBoolean(model.rubeola) + model.ageRubeola;
            pat.sarampion = NumBoolean(model.sarampion) + model.ageSarampion;
            pat.tosferina = NumBoolean(model.tosFerina) + model.ageTosFerina;
            pat.roseola = NumBoolean(model.roseola) + model.ageRoseola;
            pat.varicela = NumBoolean(model.varicela) + model.ageVaricela;

            ModelState.Clear();
            DB.SaveChanges();
            SetAges();
            GetPathological();
            return View(model);
        }
















        /*
            Antecende HeredoFamiliares
        */
        [HttpGet]
        public ActionResult HeredoFamiliares()
        {
            var model = new ModelHeredoF();

            var patient = (from p in DB.Paciente
                           where p.idPaciente == idPatient
                           select p).First();

            /* informacion padre  */
            model.f_name = patient.NombrePadre;
            model.f_1LastName = patient.ApellPaternoPadre;
            model.f_2LastName = patient.ApellMaternoPadre;
            if (patient.NacimientoPadre.HasValue)
                model.f_birthdate = patient.NacimientoPadre.Value.ToString("dd/MM/yyyy");
            model.f_civilStatus = patient.EstadoCivilPadre ?? 0;
            model.f_education = patient.EscolaridadPadre ?? 0;
            model.f_educationInfo = patient.DetalleEscolaridadPadre;
            model.f_health = patient.SaludPadre ?? 0;
            model.f_healthInfo = patient.DetalleSaludPadre;
            if (patient.toxicomaniaPadre.HasValue)
                model.f_toxicomania = patient.toxicomaniaPadre.Value ? 1 : 0;
            model.f_toxicomaniaInfo = patient.DetalletoxicomaniaPadre;

            model.f_others = patient.OtrosAntecedentesPadre;
            if (patient.AntecedentePadre != null)
            {
                model.f_alergia = CheckElement(patient.AntecedentePadre.Substring(0, 1));
                model.f_asma = CheckElement(patient.AntecedentePadre.Substring(1, 1));
                model.f_epilepsia = CheckElement(patient.AntecedentePadre.Substring(2, 1));
                model.f_diabetes = CheckElement(patient.AntecedentePadre.Substring(3, 1));
                model.f_hiper = CheckElement(patient.AntecedentePadre.Substring(4, 1));
                model.f_cancer = CheckElement(patient.AntecedentePadre.Substring(5, 1));
                model.f_malform = CheckElement(patient.AntecedentePadre.Substring(6, 1));
                model.f_cardio = CheckElement(patient.AntecedentePadre.Substring(7, 1));
            }

            /* informacion madre */
            model.m_name = patient.NombreMadre;
            model.m_1LastName = patient.ApellPaternoMadre;
            model.m_2LastName = patient.ApellMaternoMadre;
            if (patient.NacimientoMadre.HasValue)
                model.m_birthdate = patient.NacimientoMadre.Value.ToString("dd/MM/yyyy");
            model.m_civilStatus = patient.EstadoCivilMadre ?? 0;
            model.m_education = patient.EscolaridadMadre ?? 0;
            model.m_educationInfo = patient.DetalleEscolaridadMadre;
            model.m_health = patient.SaludMadre ?? 0;
            model.m_healthInfo = patient.DetalleSaludMadre;
            if (patient.toxicomaniaMadre.HasValue)
                model.m_toxicomania = patient.toxicomaniaMadre.Value ? 1 : 0;
            model.m_toxicomaniaInfo = patient.DetalletoxicomaniaMadre;

            model.m_others = patient.OtrosAntecedentesMadre;
            if (patient.AntecedenteMadre != null)
            {
                model.m_alergia = CheckElement(patient.AntecedenteMadre.Substring(0, 1));
                model.m_asma = CheckElement(patient.AntecedenteMadre.Substring(1, 1));
                model.m_epilepsia = CheckElement(patient.AntecedenteMadre.Substring(2, 1));
                model.m_diabetes = CheckElement(patient.AntecedenteMadre.Substring(3, 1));
                model.m_hiper = CheckElement(patient.AntecedenteMadre.Substring(4, 1));
                model.m_cancer = CheckElement(patient.AntecedenteMadre.Substring(5, 1));
                model.m_malform = CheckElement(patient.AntecedenteMadre.Substring(6, 1));
                model.m_cardio = CheckElement(patient.AntecedenteMadre.Substring(7, 1));
            }

            GetDataFamily();
            /* otra informacion */
            if (patient.SostenCefalico.HasValue) model.cefalico = patient.SostenCefalico.Value;
            if (patient.Ablactacion.HasValue) model.ablactacion = patient.Ablactacion.Value;
            if (patient.Posicionsedente.HasValue) model.sedente = patient.Posicionsedente.Value;
            if (patient.Destete.HasValue) model.destete = patient.Destete.Value;
            if (patient.caminarSinAyuda.HasValue) model.caminar = patient.caminarSinAyuda.Value;
            if (patient.primerasilaba.HasValue) model.silaba = patient.primerasilaba.Value;
            if (patient.primeraspalabras.HasValue) model.palabras = patient.primeraspalabras.Value;
            if (patient.Guarderia.HasValue) model.guarderia = patient.Guarderia.Value;
            if (patient.Jardin.HasValue) model.jardin = patient.Jardin.Value;
            if (patient.Primaria.HasValue) model.primaria = patient.Primaria.Value;
            if (patient.Secundaria.HasValue) model.secundaria = patient.Secundaria.Value;
            if (patient.Preparatoria.HasValue) model.preparatoria = patient.Preparatoria.Value;
            if (patient.Urbanización.HasValue) model.urbanizacion = patient.Urbanización.Value;
            if (patient.TipodeVivienda.HasValue) model.vivienda = patient.TipodeVivienda.Value;
            if (patient.Cohabitantes.HasValue) model.cohabitan = patient.Cohabitantes.Value;
 
            return View(model);
        }

        private void GetDataFamily()
        {
            var f = (from fam in DB.Familiares
                     where fam.idpaciente == idPatient
                     select fam).ToList();

            List<Familiares> xList;
            xList = f.Where(x => x.Relacion == -1).ToList();
            ViewBag.brother = xList;
            xList = f.Where(x => x.Relacion != -1).ToList();
            ViewBag.family = xList;
            SetMonths();
        }

        private void SetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i <= 24; i++)
            {
                months.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                if (i == 24)
                {
                    var m = i + " o más";
                    months.Add(new SelectListItem { Text = m, Value = i.ToString() });
                }
            }

            ViewBag.months = months;
        }

        public ActionResult AddBrother(string name, string sex, string date, string diseases)
        {
            Familiares f = new Familiares();
            f.idpaciente = idPatient;
            f.Relacion = -1;
            f.Nombre = name;
            f.Sexo = sex.ToUpper();
            f.Nacimiento = Convert.ToDateTime(date);
            f.Enfermedades = diseases;
            DB.Familiares.Add(f);
            DB.SaveChanges();

            return Json(f, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOtherFamily(string name, int relation, string other, string diseases)
        {
            Familiares f = new Familiares();
            f.idpaciente = idPatient;
            f.Relacion = relation;
            f.Nombre = name;
            f.DetalleRelacion = other;
            f.Enfermedades = diseases;
            DB.Familiares.Add(f);
            DB.SaveChanges();

            return Json(f, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveHeredo(int idFamiliar)
        {
            Familiares f = (from fam in DB.Familiares
                            where fam.idpaciente == idPatient && fam.IdFamiliar == idFamiliar
                            select fam).First();
            bool resp = false;
            try
            {
                DB.Familiares.Remove(f);
                DB.SaveChanges();
                resp = true;
            }
            catch { }
            return Json(new { status = resp.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HeredoFamiliares(ModelHeredoF model)
        {
            var patient = (from p in DB.Paciente
                           where p.idPaciente == idPatient
                           select p).First();

            /*informacion padre */
            patient.NombrePadre = model.f_name;
            patient.ApellPaternoPadre = model.f_1LastName;
            patient.ApellMaternoPadre = model.f_2LastName;
            patient.NacimientoPadre = model.f_birthdate == null ? (DateTime?)null : Convert.ToDateTime(model.f_birthdate);
            patient.EstadoCivilPadre = model.f_civilStatus;
            patient.EscolaridadPadre = model.f_education;
            patient.DetalleEscolaridadPadre = model.f_educationInfo;
            patient.SaludPadre = (byte)model.f_health;
            patient.DetalleSaludPadre = model.f_healthInfo;
            patient.toxicomaniaPadre = Convert.ToBoolean(model.f_toxicomania);
            patient.DetalletoxicomaniaPadre = model.f_toxicomaniaInfo;

            string antecedentePadre;
            antecedentePadre = NumBoolean(model.f_alergia);
            antecedentePadre += NumBoolean(model.f_asma);
            antecedentePadre += NumBoolean(model.f_epilepsia);
            antecedentePadre += NumBoolean(model.f_diabetes);
            antecedentePadre += NumBoolean(model.f_hiper);
            antecedentePadre += NumBoolean(model.f_cancer);
            antecedentePadre += NumBoolean(model.f_malform);
            antecedentePadre += NumBoolean(model.f_cardio);
            patient.AntecedentePadre = antecedentePadre;
            patient.OtrosAntecedentesPadre = model.f_others;

            /*informacion madre */
            patient.NombreMadre = model.m_name;
            patient.ApellPaternoMadre = model.m_1LastName;
            patient.ApellMaternoMadre = model.m_2LastName;
            patient.NacimientoMadre = model.m_birthdate == null ? (DateTime?)null : Convert.ToDateTime(model.m_birthdate);
            patient.EstadoCivilMadre = model.m_civilStatus;
            patient.EscolaridadMadre = model.m_education;
            patient.DetalleEscolaridadMadre = model.m_educationInfo;
            patient.SaludMadre = (byte)model.m_health;
            patient.DetalleSaludMadre = model.m_healthInfo;
            patient.toxicomaniaMadre = Convert.ToBoolean(model.m_toxicomania);
            patient.DetalletoxicomaniaMadre = model.m_toxicomaniaInfo;

            string antecedenteMadre;
            antecedenteMadre = NumBoolean(model.m_alergia);
            antecedenteMadre += NumBoolean(model.m_asma);
            antecedenteMadre += NumBoolean(model.m_epilepsia);
            antecedenteMadre += NumBoolean(model.m_diabetes);
            antecedenteMadre += NumBoolean(model.m_hiper);
            antecedenteMadre += NumBoolean(model.m_cancer);
            antecedenteMadre += NumBoolean(model.m_malform);
            antecedenteMadre += NumBoolean(model.m_cardio);
            patient.AntecedenteMadre = antecedenteMadre;
            patient.OtrosAntecedentesMadre = model.m_others;

            /* otra informacion */
            patient.SostenCefalico = model.cefalico;
            patient.Ablactacion = model.ablactacion;
            patient.Posicionsedente = model.sedente;
            patient.Destete = model.destete;
            patient.caminarSinAyuda = model.caminar;
            patient.primerasilaba = model.silaba;
            patient.primeraspalabras = model.palabras;
            patient.Guarderia = model.guarderia;
            patient.Jardin = model.jardin;
            patient.Primaria = model.primaria;
            patient.Secundaria = model.secundaria;
            patient.Preparatoria = model.preparatoria;
            patient.Urbanización = model.urbanizacion;
            patient.TipodeVivienda = model.vivienda;
            patient.Cohabitantes = model.cohabitan;

            GetDataFamily();
            ModelState.Clear();
            DB.SaveChanges();
            return View(model);
        }











        /// <summary>
        /// Antecendetes ginecologicos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Ginecologicos()
        {
            var model = new ModelGinecologicos();

            var query = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p).First();
            if (query != null)
            {
                model.menarca = query.EdadMenarca.HasValue ? query.EdadMenarca.Value : (int?)null;
                model.menacmia = query.menacmia.HasValue ? query.menacmia.Value : (int?)null;
                model.relacion = query.edadPrimerRelacionSexual.HasValue ? query.edadPrimerRelacionSexual.Value : (int?)null;
                model.lastPapanicolaou = query.ultimoPapanicolaou.HasValue ? query.ultimoPapanicolaou.Value.ToString("dd/MM/yyyy") : "";
                model.nextPapanicolaou = query.proximoPapanicolaou.HasValue ? query.proximoPapanicolaou.Value.ToString("dd/MM/yyyy") : "";
                model.studies = query.estudiosGinecologicos;

                model.dispareunia = query.dispareunia.HasValue ? query.dispareunia.Value : false;
                model.leucorrea = query.leucorrea.HasValue ? query.leucorrea.Value : false;
                model.leucorreaTxt = query.mencioneLeucorrea;
                model.lactorrea = query.lactorrea.HasValue ? query.lactorrea.Value : false;
                model.sickness = query.enfUteroOvarios.HasValue ? query.enfUteroOvarios.Value : false;
                model.sicknessTxt = query.mencioneEnfUteroOvarios;
                model.mamografia = query.mamografia.HasValue ? query.mamografia.Value : false;
                model.dateMamografia = query.fechaMamografia.HasValue ? query.fechaMamografia.Value.ToString("dd/MM/yyyy") : "";
                model.nextMamografia = query.fechaProximaMamografia.HasValue ? query.fechaProximaMamografia.Value.ToString("dd/MM/yyyy") : "";
                model.spQuirurgico = query.procQuirurgico.HasValue ? query.procQuirurgico.Value : false;
                model.spQuirurgicoTxt = query.mencioneProcQuirurgico;

                model.observations = query.observaciones;
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Ginecologicos(ModelGinecologicos model)
        {
            var query = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p).FirstOrDefault();

            query.EdadMenarca = model.menarca;
            query.menacmia = model.menacmia;
            query.edadPrimerRelacionSexual = model.relacion;
            query.ultimoPapanicolaou = model.lastPapanicolaou == null ? (DateTime?)null : Convert.ToDateTime(model.lastPapanicolaou);
            query.proximoPapanicolaou = model.nextPapanicolaou == null ? (DateTime?)null : Convert.ToDateTime(model.nextPapanicolaou);
            query.estudiosGinecologicos = model.studies;

            query.dispareunia = model.dispareunia;
            query.leucorrea = model.leucorrea;
            query.mencioneLeucorrea = model.leucorreaTxt;
            query.lactorrea = model.lactorrea;
            query.enfUteroOvarios = model.sickness;
            query.mencioneEnfUteroOvarios = model.sicknessTxt;
            query.mamografia = model.mamografia;
            query.fechaMamografia = model.dateMamografia == null ? (DateTime?)null : Convert.ToDateTime(model.dateMamografia);
            query.fechaProximaMamografia = model.nextMamografia == null ? (DateTime?)null : Convert.ToDateTime(model.nextMamografia);
            query.procQuirurgico = model.spQuirurgico;
            query.mencioneProcQuirurgico = model.spQuirurgicoTxt;

            query.observaciones = model.observations;

            ModelState.Clear();
            DB.SaveChanges();
            return View(model);
        }








        [HttpGet]
        public ActionResult Obstetricos()
        {
            var model = new ModelObstetrico();

            var query = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p).First();

            if (query != null)
            {
                model.fechaUltimoParto = query.fechaUltimoParto.HasValue ? query.fechaUltimoParto.Value.ToString("dd/MM/yyyy") : "";
                model.ultimoDiaRegla = query.ultimoDiaRegla.HasValue ? query.ultimoDiaRegla.Value.ToString("dd/MM/yyyy") : "";
                model.gestas = query.gestas;
                model.partosAnormales = query.partosAnormales;
                model.partosNormales = query.partosNormales;
                model.cesareas = query.cesareas;
                model.forceps = query.forceps;
                model.rn = query.rnPesoAlto;
                model.prematuro = query.prematuros;
                model.aborto = query.abortos;
                model.obito = query.obitos;
                model.malFormado = query.malformados;
                model.rnEnfermos = query.recienNacidosEnfermos;
                model.embarazoMult = query.embarazoMultiple;

                model.complicacionesEmbarazo = query.complicacionEmbarazo;
                model.complicacionesParto = query.complicacionParto;
                model.complicacionesRecien = query.complicacionRecienNacido;
                model.complicacionesLact = query.complicacionLact;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Obstetricos(ModelObstetrico model)
        {
            var query = (from p in DB.Paciente
                         where p.idPaciente == idPatient
                         select p).FirstOrDefault();

            query.fechaUltimoParto = string.IsNullOrEmpty(model.fechaUltimoParto) ? (DateTime?)null : Convert.ToDateTime(model.fechaUltimoParto);
            query.ultimoDiaRegla = string.IsNullOrEmpty(model.ultimoDiaRegla) ? (DateTime?)null : Convert.ToDateTime(model.ultimoDiaRegla);
            query.gestas = model.gestas;
            query.partosAnormales = model.partosAnormales;
            query.partosNormales = model.partosNormales;
            query.cesareas = model.cesareas;
            query.forceps = model.forceps;
            query.rnPesoAlto = model.rn;
            query.prematuros = model.prematuro;
            query.abortos = model.aborto;
            query.obitos = model.obito;
            query.malformados = model.malFormado;
            query.recienNacidosEnfermos = model.rnEnfermos;
            query.embarazoMultiple = model.embarazoMult;

            query.complicacionEmbarazo = model.complicacionesEmbarazo;
            query.complicacionParto = model.complicacionesParto;
            query.complicacionRecienNacido = model.complicacionesRecien;
            query.complicacionLact = model.complicacionesLact;

            DB.SaveChanges();
            return View(model);
        }
    }
}