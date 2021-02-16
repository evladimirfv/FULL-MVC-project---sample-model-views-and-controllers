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
    public class marca_modeloController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: marca_modelo
        public ActionResult Index(string searchString, int? Page_No)
        {
            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var marca_modelo = db.marca_modelo.Where(s => s.idmarca_modelo > -1000);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                marca_modelo = marca_modelo.Where(s => s.marca.Contains(searchString)
                || s.modelo.Contains(searchString)
                || s.especificaciones.Contains(searchString)
                                      );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(marca_modelo.OrderBy(p => p.idmarca_modelo).ToPagedList(noOfPage, pageSize));

        }

        // GET: marca_modelo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_modelo marca_modelo = db.marca_modelo.Find(id);
            if (marca_modelo == null)
            {
                return HttpNotFound();
            }
            return View(marca_modelo);
        }

        // GET: marca_modelo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: marca_modelo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idmarca_modelo,marca,modelo,especificaciones")] marca_modelo marca_modelo)
        {
            if (ModelState.IsValid)
            {
                db.marca_modelo.Add(marca_modelo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marca_modelo);
        }

        // GET: marca_modelo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_modelo marca_modelo = db.marca_modelo.Find(id);
            if (marca_modelo == null)
            {
                return HttpNotFound();
            }
            return View(marca_modelo);
        }

        // POST: marca_modelo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idmarca_modelo,marca,modelo,especificaciones")] marca_modelo marca_modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marca_modelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marca_modelo);
        }

        // GET: marca_modelo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_modelo marca_modelo = db.marca_modelo.Find(id);
            if (marca_modelo == null)
            {
                return HttpNotFound();
            }
            return View(marca_modelo);
        }

        // POST: marca_modelo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            marca_modelo marca_modelo = db.marca_modelo.Find(id);
            db.marca_modelo.Remove(marca_modelo);
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
