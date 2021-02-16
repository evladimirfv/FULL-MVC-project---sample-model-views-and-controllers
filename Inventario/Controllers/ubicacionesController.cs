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
    public class ubicacionesController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: ubicaciones
       
        public ActionResult Index(string searchString, int? Page_No)
        {
            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var ubicacion = db.ubicacion.Include(u => u.ciudad).Include(u => u.compania).Include(u => u.pais);


            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                ubicacion = ubicacion.Where(s => s.nombre.Contains(searchString)
                || s.nombre_de_contacto.Contains(searchString)
                || s.direccion.Contains(searchString)
                

                                      );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(ubicacion.OrderBy(p => p.idubicacion).ToPagedList(noOfPage, pageSize));

        }

        // GET: ubicaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // GET: ubicaciones/Create
        public ActionResult Create()
        {
            ViewBag.ciudad_fk = new SelectList(db.ciudad, "idciudad", "nombre");
            ViewBag.compania_fk = new SelectList(db.compania, "idcompania", "nombre");
            ViewBag.pais_fk = new SelectList(db.pais, "idpais", "nombre");
            return View();
        }

        // POST: ubicaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idubicacion,nombre,direccion,coordenadas,nombre_de_contacto,telefono_de_contacto,celular_de_contacto,es_datacenter,es_cloud,compania_fk,pais_fk,ciudad_fk,es_activo")] ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                db.ubicacion.Add(ubicacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ciudad_fk = new SelectList(db.ciudad, "idciudad", "nombre", ubicacion.ciudad_fk);
            ViewBag.compania_fk = new SelectList(db.compania, "idcompania", "nombre", ubicacion.compania_fk);
            ViewBag.pais_fk = new SelectList(db.pais, "idpais", "nombre", ubicacion.pais_fk);
            return View(ubicacion);
        }

        // GET: ubicaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ciudad_fk = new SelectList(db.ciudad, "idciudad", "nombre", ubicacion.ciudad_fk);
            ViewBag.compania_fk = new SelectList(db.compania, "idcompania", "nombre", ubicacion.compania_fk);
            ViewBag.pais_fk = new SelectList(db.pais, "idpais", "nombre", ubicacion.pais_fk);
            return View(ubicacion);
        }

        // POST: ubicaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idubicacion,nombre,direccion,coordenadas,nombre_de_contacto,telefono_de_contacto,celular_de_contacto,es_datacenter,es_cloud,compania_fk,pais_fk,ciudad_fk,es_activo")] ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ciudad_fk = new SelectList(db.ciudad, "idciudad", "nombre", ubicacion.ciudad_fk);
            ViewBag.compania_fk = new SelectList(db.compania, "idcompania", "nombre", ubicacion.compania_fk);
            ViewBag.pais_fk = new SelectList(db.pais, "idpais", "nombre", ubicacion.pais_fk);
            return View(ubicacion);
        }

        // GET: ubicaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // POST: ubicaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ubicacion ubicacion = db.ubicacion.Find(id);
            db.ubicacion.Remove(ubicacion);
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
