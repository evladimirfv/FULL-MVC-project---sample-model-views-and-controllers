using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;

using PagedList;


namespace Inventario.Controllers
{
    public class tipo_de_equipoController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: tipo_de_equipo
        public ActionResult Index(string searchString, int? Page_No)
        {

            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var tipo_equipo = db.tipo_de_equipo.Where(s => s.idtipo_de_equipo > -1000);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                tipo_equipo = tipo_equipo.Where(s => s.categoria.Contains(searchString)
                                     
                                       );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(tipo_equipo.OrderBy(p => p.idtipo_de_equipo).ToPagedList(noOfPage, pageSize));

        }

        // GET: tipo_de_equipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_equipo tipo_de_equipo = db.tipo_de_equipo.Find(id);
            if (tipo_de_equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_equipo);
        }

        // GET: tipo_de_equipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipo_de_equipo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idtipo_de_equipo,categoria")] tipo_de_equipo tipo_de_equipo)
        {
            if (ModelState.IsValid)
            {
                db.tipo_de_equipo.Add(tipo_de_equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_de_equipo);
        }

        // GET: tipo_de_equipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_equipo tipo_de_equipo = db.tipo_de_equipo.Find(id);
            if (tipo_de_equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_equipo);
        }

        // POST: tipo_de_equipo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idtipo_de_equipo,categoria")] tipo_de_equipo tipo_de_equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_de_equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_de_equipo);
        }

        // GET: tipo_de_equipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_equipo tipo_de_equipo = db.tipo_de_equipo.Find(id);
            if (tipo_de_equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_equipo);
        }

        // POST: tipo_de_equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_de_equipo tipo_de_equipo = db.tipo_de_equipo.Find(id);
            db.tipo_de_equipo.Remove(tipo_de_equipo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
