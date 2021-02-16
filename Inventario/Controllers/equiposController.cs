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

  

    public partial class equiposController : Controller
    {

      private cloudmanageEntities db = new cloudmanageEntities();

       //  private cloudmanageEntities dbDinamo = new cloudmanageEntities();

        // GET: equipos
        public ActionResult Index(string searchString, int? Page_No)
        {
            //A*
            if (searchString != null)
            {
                Page_No = 1;
            }


            var equipo = db.equipo.Include(e => e.marca_modelo).
                Include(e => e.proveedor).
                Include(e => e.responsable).
                Include(e => e.tipo_de_equipo).
                Include(e => e.ubicacion).Include(e => e.puerto_por_equipo).
                Include(e => e.atributo_por_equipo);

            //A*
            if (!String.IsNullOrEmpty(searchString))
            {
                equipo = equipo.Where(s => s.marca_modelo.marca.Contains(searchString)
                                        || s.ubicacion.nombre.Contains(searchString)
                                        || s.s_equipo_numero_serie.Contains(searchString)
                                        || s.marca_modelo.modelo.Contains(searchString)
                                       || s.tipo_de_equipo.categoria.Contains(searchString)
                                       || s.descripcion.Contains(searchString));
            }

            //A*
            int pageSize = 10;
            int noOfPage = (Page_No ?? 1);
            return View(equipo.OrderBy(p => p.idequipo).ToPagedList(noOfPage, pageSize));
        }


        //private void PopulateSoftwareEC()
        //{



        //    var dataContext = new cloudmanageEntities();
        //    var softwares = dataContext.software
        //                .Select(c => new software
        //                {
        //                    idsoftware = c.idsoftware,
        //                    nombre = c.nombre
        //                })
        //                .OrderBy(e => e.nombre);

        //    ViewBag.sofwares = softwares;
        //    ViewBag.defaultSoftware = dataContext.software.First();
        //}



        // GET: equipos/Details/5
        public ActionResult Details(int? id)
        {

            ViewBag.listaAtributos = (List<Inventario.Models.atributo>)db.atributo.ToList();
            SetRegExDictionary();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipo equipo = db.equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }


        // GRID
        // GET: equipos/Create
        public ActionResult Create()
        {

            //  PopulateSoftwareEC();


            ViewBag.softwares = new SelectList(db.software, "idsoftware", "nombre");
            ViewBag.puertos = new SelectList(db.puerto, "idpuerto", "nombre");


            TempData["spe"] = new List<software_por_equipo>();
            TempData["ppe"] = new List<puerto_por_equipo>();
            TempData["equipoId"] = null;


            ViewBag.marca_modelo_fk = new SelectList(db.marca_modelo, "idmarca_modelo", "marca_modelo_string" );




            ViewBag.responsable_fk = new SelectList(db.responsable, "idresponsable", "nombre");
			ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial");
            ViewBag.tipo_de_equipo_fk = new SelectList(db.tipo_de_equipo, "idtipo_de_equipo", "categoria");
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre");

            ViewBag.listaAtributos = (List<Inventario.Models.atributo>) db.atributo.ToList();
            SetRegExDictionary();

     
            return View();
        }


       



        // POST: equipos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idequipo,ubicacion_fk,tipo_de_equipo_fk,marca_modelo_fk,s_equipo_numero_serie,descripcion,altura_u,prof_inch,es_contenedor,s_equipo_fecha_configuracion,responsable_fk,es_propio,comentarios,path_imagen,es_activo,es_virtual,proveedor_fk,atributo_por_equipo")] equipo equipo)
        {
            if (ModelState.IsValid)
            {

                db.equipo.Add(equipo);

                db.SaveChanges();


                /*Software por equipo*/
                if (TempData["spe"] != null)    
                {
                   
                    foreach (software_por_equipo _spe in (List<software_por_equipo>)TempData["spe"])
                    {
                        software_por_equipo newItem = new software_por_equipo();

                        newItem.software = null;
                        newItem.software_fk = _spe.software_fk;
                        newItem.equipo_fk = equipo.idequipo;
                        newItem.observaciones = _spe.observaciones;


                        db.software_por_equipo.Add(newItem);

                    }
                }

                /*Puerto por equipo*/
                if (TempData["ppe"] != null)
                {

                    foreach (puerto_por_equipo _ppe in (List<puerto_por_equipo>)TempData["ppe"])
                    {
                        puerto_por_equipo newItem = new puerto_por_equipo();

                        newItem.puerto = null;
                        newItem.puerto_fk = _ppe.puerto_fk;
                        newItem.equipo_fk = equipo.idequipo;
                        newItem.numero_de_puertos = _ppe.numero_de_puertos;


                        db.puerto_por_equipo.Add(newItem);

                    }
                }


                db.SaveChanges();           
                return RedirectToAction("Index");
            }

            ViewBag.marca_modelo_fk = new SelectList(db.marca_modelo, "idmarca_modelo", "marca_modelo_string", equipo.marca_modelo_fk);
 			ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", equipo.proveedor_fk);
            ViewBag.responsable_fk = new SelectList(db.responsable, "idresponsable", "cedula_ruc", equipo.responsable_fk);
            ViewBag.tipo_de_equipo_fk = new SelectList(db.tipo_de_equipo, "idtipo_de_equipo", "categoria", equipo.tipo_de_equipo_fk);
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", equipo.ubicacion_fk);
         
            ViewBag.listaAtributos = (List<Inventario.Models.atributo>)db.atributo.ToList();
            SetRegExDictionary();
            return View(equipo);
        }


        //public string getValueFromBind(equipo eq,int idatributo)
        //{
        //    var dictionary = GetAttribute<string, equipo>(a => a == "atributo_" + idatributo.ToString());

        //    return "";
        //}




        // GET: equipos/Edit/5
        public ActionResult Edit(int? id)
        {

            TempData["spe"] = new List<software_por_equipo>();
            TempData["ppe"] = new List<puerto_por_equipo>();

            TempData["equipoId"] = id!=null?id:0;

            ViewBag.softwares = new SelectList(db.software, "idsoftware", "nombre");
            ViewBag.puertos = new SelectList(db.puerto, "idpuerto", "nombre");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipo equipo = db.equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.marca_modelo_fk = new SelectList(db.marca_modelo, "idmarca_modelo", "marca_modelo_string", equipo.marca_modelo_fk);
            ViewBag.responsable_fk = new SelectList(db.responsable, "idresponsable", "nombre", equipo.responsable_fk);
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", equipo.proveedor_fk);
            ViewBag.tipo_de_equipo_fk = new SelectList(db.tipo_de_equipo, "idtipo_de_equipo", "categoria", equipo.tipo_de_equipo_fk);
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", equipo.ubicacion_fk);


            // Recover current existent values. 

            List<software_por_equipo> sxe = equipo.software_por_equipo.ToList();
            TempData["spe"] = sxe;

            List<puerto_por_equipo> pxe = equipo.puerto_por_equipo.ToList();
            TempData["ppe"] = pxe;


            ViewBag.listaAtributos = (List<Inventario.Models.atributo>)db.atributo.ToList();
            SetRegExDictionary();
            return View(equipo);
        }

        // POST: equipos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
   
        public ActionResult Edit([Bind(Include = "idequipo,ubicacion_fk,tipo_de_equipo_fk,marca_modelo_fk,s_equipo_numero_serie,descripcion,altura_u,prof_inch,es_contenedor,s_equipo_fecha_configuracion,responsable_fk,es_propio,comentarios,path_imagen,es_activo,es_virtual,proveedor_fk,atributo_por_equipo")] equipo equipo)
        {

            ViewBag.softwares = new SelectList(db.software, "idsoftware", "nombre");
            ViewBag.puertos = new SelectList(db.puerto, "idpuerto", "nombre");

            ViewBag.listaAtributos = (List<Inventario.Models.atributo>)db.atributo.ToList();
            SetRegExDictionary();


            if (ModelState.IsValid)
            {

                using (var dbDinamo = new cloudmanageEntities())
                {

                    foreach (atributo_por_equipo ar in equipo.atributo_por_equipo)
                    {
                                           
                        if (dbDinamo.atributo_por_equipo.Find(ar.atributo_fk, ar.equipo_fk) != null)
                        {
                            db.Entry(ar).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(ar).State = EntityState.Added;
                        }
                    }        
                }

           
                db.Entry(equipo).State = EntityState.Modified;
                 db.SaveChanges();


                /*Software por equipo*/
                if (TempData["spe"] != null)
                {

                    using (var dbDinamo = new cloudmanageEntities())
                    {
                        foreach (software_por_equipo _spe in (List<software_por_equipo>)TempData["spe"])
                        {
                           
         

                            if (dbDinamo.software_por_equipo.Find(_spe.equipo_fk, _spe.software_fk) != null)
                            {
                                _spe.equipo = null;
                                _spe.software = null;


                                if (_spe.observaciones == "DELETED")
                                {
                                    db.Entry(_spe).State = EntityState.Deleted;
                                }
                                else
                                {
                                    db.Entry(_spe).State = EntityState.Modified;
                                }

                                
                            }
                            else
                            {
                                software_por_equipo newItem = new software_por_equipo();

                                newItem.software = null;
                                newItem.software_fk = _spe.software_fk;
                                newItem.equipo_fk = equipo.idequipo;
                                newItem.observaciones = _spe.observaciones;

                                db.Entry(newItem).State = EntityState.Added;
                            }
                        }
                    }
                }


                /*Puerto por equipo*/
                if (TempData["ppe"] != null && ((List<puerto_por_equipo>)TempData["ppe"]).Count > 0)
                {

                    using (var dbDinamo = new cloudmanageEntities())
                    {
                        foreach (puerto_por_equipo _ppe in (List<puerto_por_equipo>)TempData["ppe"])
                        {



                            if (dbDinamo.puerto_por_equipo.Find(_ppe.equipo_fk, _ppe.puerto_fk) != null)
                            {
                                _ppe.equipo = null;
                                _ppe.puerto = null;


                                if (_ppe.numero_de_puertos == "DELETED")
                                {
                                    db.Entry(_ppe).State = EntityState.Deleted;
                                }
                                else
                                {
                                    db.Entry(_ppe).State = EntityState.Modified;
                                }


                            }
                            else
                            {
                                puerto_por_equipo newItem = new puerto_por_equipo();

                                newItem.puerto = null;
                                newItem.puerto_fk = _ppe.puerto_fk;
                                newItem.equipo_fk = equipo.idequipo;
                                newItem.numero_de_puertos = _ppe.numero_de_puertos;

                                db.Entry(newItem).State = EntityState.Added;
                            }
                        }
                    }
                }

                db.SaveChanges();



                return RedirectToAction("Index");
            }
            ViewBag.marca_modelo_fk = new SelectList(db.marca_modelo, "idmarca_modelo", "marca_modelo_string", equipo.marca_modelo_fk);
            ViewBag.proveedor_fk = new SelectList(db.proveedor, "idproveedor", "nombre_comercial", equipo.proveedor_fk);
            ViewBag.responsable_fk = new SelectList(db.responsable, "idresponsable", "cedula_ruc", equipo.responsable_fk);
            ViewBag.tipo_de_equipo_fk = new SelectList(db.tipo_de_equipo, "idtipo_de_equipo", "categoria", equipo.tipo_de_equipo_fk);
            ViewBag.ubicacion_fk = new SelectList(db.ubicacion, "idubicacion", "nombre", equipo.ubicacion_fk);
           
            return View(equipo);
        }

        // GET: equipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipo equipo = db.equipo.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // POST: equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            equipo equipo = db.equipo.Find(id);
            db.equipo.Remove(equipo);
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

        // Set additional attributes validation required information like regex and datatype
        public void SetRegExDictionary()
        {
           // ViewBag.listaTipo_Dato = (List<Inventario.Models.tipo_dato>)db.tipo_dato.ToList();
          
            Dictionary<int, string> tDict = new Dictionary<int, string>();
            Dictionary<int, string> tDictDataType = new Dictionary<int, string>();
            Dictionary<int, string> tDictDataTypeInput = new Dictionary<int, string>();
            foreach (tipo_dato td in db.tipo_dato.ToList())
            {
                tDict.Add(td.idtipo_dato, td.regex);          
                tDictDataType.Add(td.idtipo_dato, td.nombre);
       
                if(td.nombre == "Fecha")
                    tDictDataTypeInput.Add(td.idtipo_dato, "date");
                else
                    tDictDataTypeInput.Add(td.idtipo_dato, "text");
            }

            ViewBag.regexDict = tDict;
            ViewBag.tipoDatoDict = tDictDataType;
            ViewBag.tipoDatoInputDict = tDictDataTypeInput;
        }
        private static Dictionary<string, string> GetAttribute<TAttribute, TType>(
          Func<TAttribute, string> valueFunc)
          where TAttribute : Attribute
        {
            return typeof(TType).GetProperties()
                .SelectMany(p => p.GetCustomAttributes())
                .OfType<TAttribute>()
                .ToDictionary(a => a.GetType().Name.Replace("Attribute", ""), valueFunc);
        }


    }
}
