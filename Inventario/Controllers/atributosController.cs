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
    public class atributosController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: atributos
        public ActionResult Index(string searchString, int? Page_No)
        {

            var atributo = db.atributo.Include(a => a.tipo_dato);

            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }
  
            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                atributo = atributo.Where(s => s.nombre.Contains(searchString)
                                     );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(atributo.OrderBy(p => p.idatributo).ToPagedList(noOfPage, pageSize));

        }

        // GET: atributos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            atributo atributo = db.atributo.Find(id);
            if (atributo == null)
            {
                return HttpNotFound();
            }
            return View(atributo);
        }

        // GET: atributos/Create
        public ActionResult Create()
        {
            ViewBag.tipo_dato_fk = new SelectList(db.tipo_dato, "idtipo_dato", "nombre");
            return View();
        }

        // POST: atributos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idatributo,nombre,tipo_dato_fk,es_requerido")] atributo atributo)
        {
            if (ModelState.IsValid)
            {
                db.atributo.Add(atributo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tipo_dato_fk = new SelectList(db.tipo_dato, "idtipo_dato", "nombre", atributo.tipo_dato_fk);
            return View(atributo);
        }

        // GET: atributos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            atributo atributo = db.atributo.Find(id);
            if (atributo == null)
            {
                return HttpNotFound();
            }
            ViewBag.tipo_dato_fk = new SelectList(db.tipo_dato, "idtipo_dato", "nombre", atributo.tipo_dato_fk);
            return View(atributo);
        }

        // POST: atributos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idatributo,nombre,tipo_dato_fk,es_requerido")] atributo atributo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atributo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tipo_dato_fk = new SelectList(db.tipo_dato, "idtipo_dato", "nombre", atributo.tipo_dato_fk);
            return View(atributo);
        }

        // GET: atributos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            atributo atributo = db.atributo.Find(id);
            if (atributo == null)
            {
                return HttpNotFound();
            }
            return View(atributo);
        }

        // POST: atributos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            atributo atributo = db.atributo.Find(id);
            db.atributo.Remove(atributo);
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
