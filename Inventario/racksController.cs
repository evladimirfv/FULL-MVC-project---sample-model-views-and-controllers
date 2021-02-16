using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;

namespace Inventario
{
    public class racksController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: racks
        public ActionResult Index()
        {
            var rack = db.rack.Include(r => r.ubicacion);
            return View(rack.ToList());
        }

        // GET: racks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rack rack = db.rack.Find(id);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // GET: racks/Create
        public ActionResult Create()
        {
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre");
            return View();
        }

        // POST: racks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idrack,codigo,nombre,ubicacion_fk,orden,altura_u,prof_inch")] rack rack)
        {
            if (ModelState.IsValid)
            {
                db.rack.Add(rack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);
            return View(rack);
        }

        // GET: racks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rack rack = db.rack.Find(id);
            if (rack == null)
            {
                return HttpNotFound();
            }
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);
            return View(rack);
        }

        // POST: racks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idrack,codigo,nombre,ubicacion_fk,orden,altura_u,prof_inch")] rack rack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);
            return View(rack);
        }

        // GET: racks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rack rack = db.rack.Find(id);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // POST: racks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rack rack = db.rack.Find(id);
            db.rack.Remove(rack);
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
