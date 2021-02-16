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



using System.Reflection;



namespace Inventario.Controllers
{

    public class DSourceItem
    {
        public int id_equipo { get; set; }
        public string name_equipo { get; set; }
    }

    public partial class racksController : Controller
    {
        private cloudmanageEntities db = new cloudmanageEntities();

        // GET: racks
        public ActionResult Index(string searchString, int? Page_No)
        {

            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }

            var rack = db.rack.Include(r => r.proveedor).Include(r => r.ubicacion);


            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                rack = rack.Where(s => s.codigo.Contains(searchString)
                                        || s.ubicacion.nombre.Contains(searchString)
                                        || s.nombre.Contains(searchString)
                                        || s.altura_u.ToString().Contains(searchString)
                                        || s.prof_inch.ToString().Contains(searchString)
                                       );
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(rack.OrderBy(p => p.idrack).ToPagedList(noOfPage, pageSize));
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

            // Get contained equipos

            equipo tempOne = new equipo();
            List<equipo> equiposRack = new List<equipo>();
            foreach (equipo_por_rack exr in rack.equipo_por_rack)
            {
                tempOne = new equipo();
                tempOne.idequipo = exr.equipo.idequipo;
                tempOne.altura_u = exr.equipo.altura_u;
                tempOne.s_equipo_numero_serie = exr.equipo.s_equipo_numero_serie + " " + exr.equipo.marca_modelo.modelo + " (" + exr.equipo.altura_u + "U)";
                tempOne.comentarios = exr.inicia_u.ToString();   //OJO//
                
                equiposRack.Add(tempOne);
            }
            ViewBag.equiposRack = equiposRack;

         

            return View(rack);
        }

        // GET: racks/Create
        public ActionResult Create()
        {
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial");
            ViewBag.equipos = GetEquiposSelectList();            //  new SelectList(db.equipo, "idequipo", "s_equipo_numero_serie");   //*****//
       
  			TempData["epr"] = new List<equipo_por_rack>();
            TempData["equipoId"] = null;
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre");
            return View();
        }


        

        public SelectList GetEquiposSelectList()
        {
            //  return new SelectList(db.equipo, "idequipo", "s_equipo_numero_serie");


            IQueryable<DSourceItem> dsitemList = db.equipo.Select(s => new DSourceItem
            {
                id_equipo = s.idequipo,
                name_equipo = s.s_equipo_numero_serie + " - " + s.marca_modelo.modelo + " - (" + s.altura_u + "U)"
            }
               ); ;


            return new SelectList(dsitemList, "id_equipo", "name_equipo");
            
            
        }


        public string GetPersonalizedNameForEquipoNombre(int equipoId)
        {
            equipo eq = db.equipo.Where(s => s.idequipo == equipoId).First();
            string decoName = "";
            decoName = eq.s_equipo_numero_serie + " - " + eq.marca_modelo.modelo;
            return decoName;
        }




        // POST: racks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idrack,codigo,nombre,ubicacion_fk,orden,altura_u,prof_inch,proveedor_fk")] rack rack)
        {
            if (ModelState.IsValid)
            {
                db.rack.Add(rack);
                db.SaveChanges();
 				/*Software por equipo*/
                if (TempData["epr"] != null)    
                {
                   
                    foreach (equipo_por_rack _spe in (List<equipo_por_rack>)TempData["epr"])
                    {
                        equipo_por_rack newItem = new equipo_por_rack();

                        newItem.equipo = null;
                        newItem.equipo_fk = _spe.equipo_fk;
                        newItem.rack_fk = rack.idrack;
                        // newItem.observaciones = _spe.observaciones;
                        newItem.inicia_u = _spe.inicia_u;


                        db.equipo_por_rack.Add(newItem);

                    }
                }
 				db.SaveChanges();     
                return RedirectToAction("Index");
            }

            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", rack.proveedor_fk);
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);
            return View(rack);
        }

        // GET: racks/Edit/5
        public ActionResult Edit(int? id)
        {
  			TempData["epr"] = new List<equipo_por_rack>();
 			TempData["equipoId"] = id!=null?id:0;
     		ViewBag.equipos  = GetEquiposSelectList();      // new SelectList(db.equipo, "idequipo", "s_equipo_numero_serie");   //*****//

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rack rack = db.rack.Find(id);
            if (rack == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", rack.proveedor_fk);
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);



            // Get contained equipos

            equipo tempOne = new equipo();
            List<equipo> equiposRack = new List<equipo>();
            foreach (equipo_por_rack _exr in rack.equipo_por_rack)
            {
                tempOne = new equipo();
                tempOne.idequipo = _exr.equipo.idequipo;
                tempOne.altura_u = _exr.equipo.altura_u;
                tempOne.s_equipo_numero_serie = _exr.equipo.s_equipo_numero_serie + " " + _exr.equipo.marca_modelo.modelo + " (" + _exr.equipo.altura_u + "U)";
                tempOne.comentarios = _exr.inicia_u.ToString();   //OJO//

                equiposRack.Add(tempOne);
            }
            ViewBag.equiposRack = equiposRack;

            // Recover current existent values. 
            List<equipo_por_rack> exr = rack.equipo_por_rack.ToList();    //OJO//
            TempData["epr"] = exr;


            return View(rack);

  	


            


        }

        // POST: racks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idrack,codigo,nombre,ubicacion_fk,orden,altura_u,prof_inch,proveedor_fk")] rack rack)
        {
    		//*****//  ViewBag.equipos = new SelectList(db.equipo, "idequipo", "s_equipo_numero_serie");   //*****//

            if (ModelState.IsValid)
            {
            	
            	//*****// down
            	
           
                db.Entry(rack).State = EntityState.Modified;
                 db.SaveChanges();


                /*Equipo por rack*/
                if (TempData["epr"] != null)
                {

                    using (var dbDinamo = new cloudmanageEntities())
                    {
                        foreach (equipo_por_rack _epr in (List<equipo_por_rack>)TempData["epr"])
                        {
                           
         

                            if (dbDinamo.equipo_por_rack.Find(_epr.equipo_fk, _epr.rack_fk) != null)
                            {
                                _epr.equipo = null;
                                _epr.rack = null;


                                if (_epr.observaciones == "DELETED")
                                {
                                    db.Entry(_epr).State = EntityState.Deleted;
                                }
                                else
                                {
                                    db.Entry(_epr).State = EntityState.Modified;
                                }

                                
                            }
                            else
                            {
                                equipo_por_rack newItem = new equipo_por_rack();

                                newItem.equipo = null;
                                newItem.equipo_fk = _epr.equipo_fk;
                                newItem.rack_fk = rack.idrack;
                                // newItem.observaciones = _epr.observaciones;
                                newItem.inicia_u = _epr.inicia_u;

                                db.Entry(newItem).State = EntityState.Added;
                            }
                        }
                    }
                }
                
                //*****// up
                
                                db.SaveChanges();




                // Get contained equipos
                rack racky = db.rack.Find(rack.idrack);          // db.rack.Include(s => s.equipo_por_rack).FirstOrDefault(x => x.idrack == rack.idrack);
                if (racky == null)
                {
                    return HttpNotFound();
                }
                equipo tempOne = new equipo();
                List<equipo> equiposRack = new List<equipo>();
                foreach (equipo_por_rack _exr in racky.equipo_por_rack)
                {
                    _exr.equipo = db.equipo.Find(_exr.equipo_fk);
                    tempOne = new equipo();
                    tempOne.idequipo = _exr.equipo.idequipo;
                    tempOne.altura_u = _exr.equipo.altura_u;
                    tempOne.s_equipo_numero_serie = _exr.equipo.s_equipo_numero_serie + " " + _exr.equipo.marca_modelo.modelo + " (" + _exr.equipo.altura_u + "U)";
                    tempOne.comentarios = _exr.inicia_u.ToString();   //OJO//

                    equiposRack.Add(tempOne);
                }
                ViewBag.equiposRack = equiposRack;


                // Recover current existent values. 
                List<equipo_por_rack> exr = racky.equipo_por_rack.ToList();    //OJO//
                TempData["epr"] = exr;


                // GENERAL BEHAVIOR OK   return RedirectToAction("Index");
                // New Behavior
                ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", rack.proveedor_fk);
                ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", rack.ubicacion_fk);
                ViewBag.equipos = GetEquiposSelectList();
                return View(rack);
                // End new behavior
            }
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", rack.proveedor_fk);
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
