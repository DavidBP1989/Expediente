using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class EmeciController : Controller
    {
        // GET: Emeci
        emeciEntities db = new emeciEntities();
        public ActionResult Index()
        {
            var model = db.viewMedicos.OrderByDescending(p => p.PediatraPlus);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByCiudad(int idc)
        {
            var model = db.viewMedicos.Where(p => p.idciudad == idc).OrderByDescending(p => p.PediatraPlus);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByEstado(string ide)
        {
            var model = db.viewMedicos.Where(p => p.idEstado == ide).OrderByDescending(p => p.PediatraPlus);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEstados()
        {
            var model = db.Estados.Where(p => p.IdPais == "MX").OrderBy(p=>p.codigo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCiudades(string idest)
        {
            var model = db.Ciudades.Where(p => p.idEstado == idest).OrderBy(p => p.Nombre);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEspecialista(int idespec)
        {
            var model = db.viewMedico.Where(p => p.idmedico == idespec);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: Emeci/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Emeci/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emeci/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emeci/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Emeci/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emeci/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Emeci/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
