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
    public class responsablesController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: responsables
        public ActionResult Index(string searchString, int? Page_No)
        {
            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var resp = db.responsable.Where(s => s.idresponsable > -1000);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                resp = resp.Where(s => s.cedula_ruc.Contains(searchString)
                 || s.nombre.Contains(searchString)
               

                                      );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(resp.OrderBy(p => p.idresponsable).ToPagedList(noOfPage, pageSize));

        }

        // GET: responsables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            responsable responsable = db.responsable.Find(id);
            if (responsable == null)
            {
                return HttpNotFound();
            }
            return View(responsable);
        }

        // GET: responsables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: responsables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idresponsable,cedula_ruc,nombre")] responsable responsable)
        {
            if (ModelState.IsValid)
            {
                db.responsable.Add(responsable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(responsable);
        }

        // GET: responsables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            responsable responsable = db.responsable.Find(id);
            if (responsable == null)
            {
                return HttpNotFound();
            }
            return View(responsable);
        }

        // POST: responsables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idresponsable,cedula_ruc,nombre")] responsable responsable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responsable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsable);
        }

        // GET: responsables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            responsable responsable = db.responsable.Find(id);
            if (responsable == null)
            {
                return HttpNotFound();
            }
            return View(responsable);
        }

        // POST: responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            responsable responsable = db.responsable.Find(id);
            db.responsable.Remove(responsable);
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
