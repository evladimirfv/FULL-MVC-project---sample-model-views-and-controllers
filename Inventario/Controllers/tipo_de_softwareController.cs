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
    public class tipo_de_softwareController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: tipo_de_software
        public ActionResult Index(string searchString, int? Page_No)
        {
            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var tiposoftware = db.tipo_de_software.Where(s => s.idtipo_de_software > -1000);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                tiposoftware = tiposoftware.Where(s => s.categoria.Contains(searchString)
               
                                      );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(tiposoftware.OrderBy(p => p.idtipo_de_software).ToPagedList(noOfPage, pageSize));

        }

        // GET: tipo_de_software/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_software tipo_de_software = db.tipo_de_software.Find(id);
            if (tipo_de_software == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_software);
        }

        // GET: tipo_de_software/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipo_de_software/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idtipo_de_software,categoria")] tipo_de_software tipo_de_software)
        {
            if (ModelState.IsValid)
            {
                db.tipo_de_software.Add(tipo_de_software);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_de_software);
        }

        // GET: tipo_de_software/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_software tipo_de_software = db.tipo_de_software.Find(id);
            if (tipo_de_software == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_software);
        }

        // POST: tipo_de_software/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idtipo_de_software,categoria")] tipo_de_software tipo_de_software)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_de_software).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_de_software);
        }

        // GET: tipo_de_software/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_software tipo_de_software = db.tipo_de_software.Find(id);
            if (tipo_de_software == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_software);
        }

        // POST: tipo_de_software/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_de_software tipo_de_software = db.tipo_de_software.Find(id);
            db.tipo_de_software.Remove(tipo_de_software);
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
