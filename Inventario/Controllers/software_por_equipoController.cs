//https://docs.telerik.com/aspnet-mvc/getting-started/helper-basics/custom-datasource?_ga=2.104394819.239324408.1607357030-1165796308.1605732201

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




        software_por_equipo_service ses = new software_por_equipo_service(new cloudmanageEntities());
        public ActionResult Editing_Inline()
        {
            PopulateSoftware();
            return View();
        }

        public ActionResult EditingInline_Read([DataSourceRequest] DataSourceRequest request)
        {

            PopulateSoftware();

            JsonResult jr;
            if (TempData["equipoId"] == null)
            {
                jr = Json(ses.Read().ToDataSourceResult(request));
            }
            else
            {
                int td_equipoId = (int) TempData["equipoId"];
                TempData["equipoId"] = td_equipoId;
                jr = Json(ses.Read(td_equipoId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet); // ,JsonRequestBehavior.AllowGet
                
            }

            return jr;
        }









        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, software_por_equipo soft)
        {
            List<software_por_equipo> tCache;

            cloudmanageEntities ent = new cloudmanageEntities();

            if (soft != null && ModelState.IsValid)
            {

                // Update internal field for software_fk
                if (Int32.TryParse(soft.software_nombre, out int tval))
                {
                    soft.software_fk = tval;
                }

                // If valid software_fk  update software_nombre
                if (soft.software_fk > 0)
                {
                    soft.software_nombre = ent.software.Where(s => s.idsoftware == soft.software_fk).
                        Select(s => s.nombre).First();
                }
                else
                {
                    soft.software_nombre = "";
                    soft.software = null;
                }

                //  Add new item 
                bool isPrex = false;

                tCache = (List<software_por_equipo>)TempData["spe"];
                TempData["spe"] = tCache;

                foreach (software_por_equipo sspe in tCache)
                {
                    if (sspe.software_fk == soft.software_fk)
                        isPrex = true;
                }
                if (!isPrex)
                {
                    tCache = (List<software_por_equipo>)TempData["spe"];
                    tCache.Add(soft);
                    TempData["spe"] = tCache;
                }

            }


            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }




        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, software_por_equipo soft)
        {

            List<software_por_equipo> tCache;

            cloudmanageEntities ent = new cloudmanageEntities();
            
                if (soft != null && ModelState.IsValid)
                {
                    //     ses.Update(soft);
                    if (soft.software_fk > 0)
                    {
                        soft.software_nombre = ent.software.Where(s => s.idsoftware == soft.software_fk).Select(s => s.nombre).First();
                    }
                    else
                    {
                        soft.software_nombre = "";
                    }



                //  Add new item 
                bool isPrex = false;

                tCache = (List<software_por_equipo>)TempData["spe"];
                TempData["spe"] = tCache;

                for (int i = 0; i < tCache.Count; i++)
                {
                    if (tCache[i].software_fk == soft.software_fk)
                    {
                        tCache[i] = soft;   // Replace updated item.
                        isPrex = true;
                    }                      
                }

                TempData["spe"] = tCache;



                if (!isPrex)
                {
                    tCache = (List<software_por_equipo>)TempData["spe"];
                    tCache.Add(soft);
                    TempData["spe"] = tCache;
                }
                else   // Update fields
                { 
                    
                
                }


            }
            



            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, software_por_equipo soft)
        {

            List<software_por_equipo> tCache;

            //PopulateSoftware();

            if (soft != null)
            {
                tCache = (List<software_por_equipo>)TempData["spe"];

                software_por_equipo sxe_todelete = null;
                // Find item to be removed
                foreach (software_por_equipo sxeitem in tCache)
                {
                    if (sxeitem.equipo_fk == soft.equipo_fk && sxeitem.software_fk == soft.software_fk)
                    {
                        sxe_todelete = sxeitem;
                        sxe_todelete.observaciones = "DELETED";

                    }

                }

                //if (sxe_todelete != null)
                //{
                //    tCache.Remove(sxe_todelete);
                
                //}


                TempData["spe"] = tCache;


                ses.Destroy(soft);
            }

            return Json(new[] { soft }.ToDataSourceResult(request, ModelState));
        }


        private void PopulateSoftware()
        {
            var dataContext = new cloudmanageEntities();
            var softwares = dataContext.software
                        .Select(c => new software
                        {
                            idsoftware = c.idsoftware,
                            nombre = c.nombre
                        })
                        .OrderBy(e => e.nombre);

            ViewData["softwares"] = softwares;
            ViewData["defaultSoftware"] = dataContext.software.First();

            ViewBag.softwares = new SelectList(db.software, "idsoftware", "nombre");

        }



        public JsonResult ReadSoftware()
        {
            var dataContext = new cloudmanageEntities();
            var softwares = dataContext.software
                        .Select(c => new software
                        {
                            idsoftware = c.idsoftware,
                            nombre = c.nombre
                        })
                        .OrderBy(e => e.nombre);

            //var softwares = db.software.Include(s => s.tipo_de_software);


            ViewData["softwares"] = softwares;
            ViewData["defaultSoftware"] = dataContext.software.First();

            JsonResult s = Json(softwares, JsonRequestBehavior.AllowGet);

            string ss = s.ToString();

            return s;

        }

    }
}


   
