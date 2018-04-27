using EmeciExpediente.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmeciExpediente.Controllers
{
    public class VacunasController : InitController
    {
        emeciEntities DB = new emeciEntities();
        public int _SessionId => (int)Session["IdPatient"];

        // GET: Vacunas
        public ActionResult Index()
        {
            VaccinesModel Model = new VaccinesModel();
            GetVaccines(ref Model);

            return View(Model);
        }

        void GetVaccines(ref VaccinesModel Model)
        {
            List<dynamic> LVaccines = new List<dynamic>();
            dynamic Row;

            var Vaccines = (from v in DB.Vacunas
                            where v.idpaciente == _SessionId
                            select v).ToList();
            if (Vaccines != null && Vaccines.Count > 0)
            {

            }
            else
            {
                /*vacunas por defecto*/
                var ListVaccines = (from l in DB.ListaVacunas select l).ToList();
                Model.TotalVaccines = ListVaccines.Count;

                var Index = 0;
                foreach (var l in ListVaccines)
                {
                    bool First = true;
                    var Doses = (from dos in DB.DosisAplicadaVacuna
                                 where dos.IdVacuna == l.IdVacuna
                                 select dos).ToList();
                    foreach (var d in Doses)
                    {
                        Row = new ExpandoObject();
                        Row.Name = First ? l.Nombre : string.Empty;
                        Row.Sickness = First ? l.Enfermedad : string.Empty;
                        Row.Dose = d.Dosis;
                        Row.Age = d.Edad;
                        Row.Date = d.FechaVacunacion.HasValue ? d.FechaVacunacion.Value : (DateTime?) null;
                        Row.Index = Index;
                        Row.RowsPan = (Doses.Count + 1);
                        LVaccines.Add(Row);
                        First = false;
                    }

                    Index++;
                }
            }

            Model.LVaccines = LVaccines;
        }
    }
}