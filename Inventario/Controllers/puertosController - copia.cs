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
    public class puertosController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: puertos

        //public ActionResult Index()
        //{
        //    return View(db.puerto. ToPagedList(1, 10));
        //}

        
        public ActionResult Index(string searchString,int? Page_No)
        {
         
           
            if (searchString != null)  
            {  
                Page_No = 1;  
            }  
        
            

            var vPuerto = from s in db.puerto
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                vPuerto = db.puerto.Where(s => s.nombre.Contains(searchString)
                                       || s.especificaciones.Contains(searchString));
            }
            else 
            {
                vPuerto = from s in db.puerto
                                  select s;
            }


            
            int pageSize = 10;  
            int noOfPage = (Page_No ?? 1);
            //    return View(students.ToPagedList(No_Of_Page, Size_Of_Page));  
             return View(vPuerto.OrderBy(p => p.idpuerto).ToPagedList(noOfPage,pageSize));
          

        }
        
        
        


        // GET: puertos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            puerto puerto = db.puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            return View(puerto);
        }

        // GET: puertos/Create
        public ActionResult Create()
        {
            ViewBag.idpuerto = new SelectList(db.puerto_por_equipo, "idpuerto_por_equipo", "numero_de_puertos");
            return View();
        }

        // POST: puertos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idpuerto,nombre,especificaciones")] puerto puerto)
        {
            if (ModelState.IsValid)
            {
                db.puerto.Add(puerto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idpuerto = new SelectList(db.puerto_por_equipo, "idpuerto_por_equipo", "numero_de_puertos", puerto.idpuerto);
            return View(puerto);
        }

        // GET: puertos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            puerto puerto = db.puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpuerto = new SelectList(db.puerto_por_equipo, "idpuerto_por_equipo", "numero_de_puertos", puerto.idpuerto);
            return View(puerto);
        }

        // POST: puertos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idpuerto,nombre,especificaciones")] puerto puerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idpuerto = new SelectList(db.puerto_por_equipo, "idpuerto_por_equipo", "numero_de_puertos", puerto.idpuerto);
            return View(puerto);
        }

        // GET: puertos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            puerto puerto = db.puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            return View(puerto);
        }

        // POST: puertos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            puerto puerto = db.puerto.Find(id);
            db.puerto.Remove(puerto);
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


        //public JsonResult Country_read(int? id)
        //{
        //    using (var Context = new cloudmanageEntities())
        //    {
        //        var puertosQuery = Context.puerto.Select(c => new HierarchicalViewModel
        //        {
        //            ID = c.PKCountryId,
        //            Name = c.CountryName,
        //            ParentID = null,
        //            HasChildren = c.States.Any()
        //        })
        //        .Union(Context.States.Select(c => new HierarchicalViewModel
        //        {
        //            ID = c.PKStateId,
        //            Name = c.StateName,
        //            ParentID = c.FKCountryId,
        //            HasChildren = c.Addresses.Any()
        //        }));
        //        var result = countryQuery.ToList()
        //         .Where(x => id.HasValue ? x.ParentID == id : x.ParentID == null)
        //         .Select(item => new
        //         {
        //             id = item.ID,
        //             Name = item.Name,
        //             expanded = item.Expanded,
        //             hasChildren = item.HasChildren

        //         });

        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}


// https://tutorialshelper.com/dynamically-populated-treeview-in-kendo-using-mvc-2/