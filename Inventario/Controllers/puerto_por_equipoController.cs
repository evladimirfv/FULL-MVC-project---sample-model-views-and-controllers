using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Inventario.Controllers
{
    public partial class equiposController : Controller
    {




        puerto_por_equipo_service pes = new puerto_por_equipo_service(new cloudmanageEntities());
        public ActionResult Editing_Inline_Puerto()
        {
            PopulatePuertos();
            return View();
        }

        public ActionResult EditingInline_Read_Puerto([DataSourceRequest] DataSourceRequest request)
        {

            PopulatePuertos();

            JsonResult jr;
            if (TempData["equipoId"] == null)
            {
                jr = Json(pes.Read().ToDataSourceResult(request));
            }
            else
            {
                jr = Json(pes.Read((int)TempData["equipoId"]).ToDataSourceResult(request), JsonRequestBehavior.AllowGet); // ,JsonRequestBehavior.AllowGet
                TempData["equipoId"] = null;
            }

            return jr;
        }









        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create_Puerto([DataSourceRequest] DataSourceRequest request, puerto_por_equipo soft)
        {
            List<puerto_por_equipo> pCache;

            cloudmanageEntities ent = new cloudmanageEntities();

            if (soft != null && ModelState.IsValid)
            {

                // Update internal field for software_fk
                if (Int32.TryParse(soft.puerto_nombre, out int tval))
                {
                    soft.puerto_fk = tval;
                }

                // If valid puerto_fk  update puerto_nombre
                if (soft.puerto_fk > 0)
                {
                    soft.puerto_nombre = ent.puerto.Where(s => s.idpuerto == soft.puerto_fk).
                        Select(s => s.nombre).First();
                }
                else
                {
                    soft.puerto_nombre = "";
                    soft.puerto = null;
                }

                //  Add new item 
                bool isPrex = false;

                pCache = (List<puerto_por_equipo>)TempData["ppe"];
                TempData["ppe"] = pCache;

                foreach (puerto_por_equipo sspe in pCache)
                {
                    if (sspe.puerto_fk == soft.puerto_fk)
                        isPrex = true;
                }
                if (!isPrex)
                {
                    pCache = (List<puerto_por_equipo>)TempData["ppe"];
                    pCache.Add(soft);
                    TempData["ppe"] = pCache;
                }

            }


            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }




        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update_Puerto([DataSourceRequest] DataSourceRequest request, puerto_por_equipo soft)
        {

            List<puerto_por_equipo> pCache;

            cloudmanageEntities ent = new cloudmanageEntities();

            if (soft != null && ModelState.IsValid)
            {
                //     pes.Update(soft);
                if (soft.puerto_fk > 0)
                {
                    soft.puerto_nombre = ent.puerto.Where(s => s.idpuerto == soft.puerto_fk).Select(s => s.nombre).First();
                }
                else
                {
                    soft.puerto_nombre = "";
                }



                //  Add new item 
                bool isPrex = false;

                pCache = (List<puerto_por_equipo>)TempData["ppe"];
                TempData["ppe"] = pCache;

                for (int i = 0; i < pCache.Count; i++)
                {
                    if (pCache[i].puerto_fk == soft.puerto_fk)
                    {
                        pCache[i] = soft;   // Replace updated item.
                        isPrex = true;
                    }
                }

                TempData["ppe"] = pCache;



                if (!isPrex)
                {
                    pCache = (List<puerto_por_equipo>)TempData["ppe"];
                    pCache.Add(soft);
                    TempData["ppe"] = pCache;
                }
                else   // Update fields
                {


                }


            }




            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy_Puerto([DataSourceRequest] DataSourceRequest request, puerto_por_equipo soft)
        {

            List<puerto_por_equipo> pCache;

            //PopulateSoftware();

            if (soft != null)
            {
                pCache = (List<puerto_por_equipo>)TempData["ppe"];

                puerto_por_equipo pxe_todelete = null;
                // Find item to be removed
                foreach (puerto_por_equipo pxeitem in pCache)
                {
                    if (pxeitem.equipo_fk == soft.equipo_fk && pxeitem.puerto_fk == soft.puerto_fk)
                    {
                        pxe_todelete = pxeitem;
                        pxe_todelete.numero_de_puertos = "DELETED";

                    }

                }

                //if (sxe_todelete != null)
                //{
                //    pCache.Remove(sxe_todelete);

                //}


                TempData["ppe"] = pCache;


                pes.Destroy(soft);
            }

            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }


        private void PopulatePuertos()
        {
            var dataContext = new cloudmanageEntities();
            var puertos = dataContext.puerto 
                        .Select(c => new puerto
                        {
                            idpuerto = c.idpuerto,
                            nombre = c.nombre
                        })
                        .OrderBy(e => e.nombre);

            ViewData["puertos"] = puertos;
            ViewData["defaultPuerto"] = dataContext.puerto.First();

            ViewBag.puertos = new SelectList(db.puerto, "idpuerto", "nombre");

        }


         // READ PUERTO

    }
}
