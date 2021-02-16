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
    public class softwaresController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: softwares
        public ActionResult Index(string searchString, int? Page_No)
        {


            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var software = db.software.Include(s => s.proveedor).Include(s => s.tipo_de_software);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                software = software.Where(s => s.codigo.Contains(searchString)
                                        || s.version.Contains(searchString)
                                        || s.nombre.Contains(searchString)
                                       
                                       );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(software.OrderBy(p => p.idsoftware).ToPagedList(noOfPage, pageSize));


        }

        // GET: softwares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            software software = db.software.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }
            return View(software);
        }

        // GET: softwares/Create
        public ActionResult Create()
        {
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial");
            ViewBag.tipo_software_fk = new SelectList(db.tipo_de_software, "idtipo_de_software", "categoria");
            return View();
        }

        // POST: softwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idsoftware,nombre,version,codigo,proveedor_fk,tipo_software_fk,informacion_tecnica,configuraciones,es_activo,licencias_adquiridas,licencias_instaladas")] software software)
        {
            if (ModelState.IsValid)
            {
                db.software.Add(software);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", software.proveedor_fk);
            ViewBag.tipo_software_fk = new SelectList(db.tipo_de_software, "idtipo_de_software", "categoria", software.tipo_software_fk);
            return View(software);
        }

        // GET: softwares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            software software = db.software.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", software.proveedor_fk);
            ViewBag.tipo_software_fk = new SelectList(db.tipo_de_software, "idtipo_de_software", "categoria", software.tipo_software_fk);
            return View(software);
        }

        // POST: softwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idsoftware,nombre,version,codigo,proveedor_fk,tipo_software_fk,informacion_tecnica,configuraciones,es_activo,licencias_adquiridas,licencias_instaladas")] software software)
        {
            if (ModelState.IsValid)
            {
                db.Entry(software).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", software.proveedor_fk);
            ViewBag.tipo_software_fk = new SelectList(db.tipo_de_software, "idtipo_de_software", "categoria", software.tipo_software_fk);
            return View(software);
        }

        // GET: softwares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            software software = db.software.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }
            return View(software);
        }

        // POST: softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            software software = db.software.Find(id);
            db.software.Remove(software);
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
