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
    public partial class racksController : Controller
    {




        equipo_por_rack_service ers = new equipo_por_rack_service(new cloudmanageEntities());
        public ActionResult Editing_Inline()
        {
            PopulateEquipos();
            return View();
        }

        public ActionResult EditingInline_Read([DataSourceRequest] DataSourceRequest request)
        {

            PopulateEquipos();

            JsonResult jr;
            if (TempData["equipoId"] == null)
            {
                jr = Json(ers.Read().ToDataSourceResult(request));
            }
            else
            {
                int td_equipoId = (int) TempData["equipoId"];
                TempData["equipoId"] = td_equipoId;
                jr = Json(ers.Read(td_equipoId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet); // ,JsonRequestBehavior.AllowGet
                
            }

            return jr;
        }









        [AcceptVerbs(HttpVerbs.Post)]
                           
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, equipo_por_rack _equip)
        {
            List<equipo_por_rack> tCache;

            cloudmanageEntities ent = new cloudmanageEntities();

            if (_equip != null && ModelState.IsValid)
            {

                // Update internal field for equipo_fk
                if (Int32.TryParse(_equip.equipo_nombre, out int tval))
                {
                    _equip.equipo_fk = tval;
                }

                // If valid equipo_fk  update equipo_nombre
                if (_equip.equipo_fk > 0)
                {
                    try
                    {
                        // _equip.equipo_nombre = ent.equipo.Where(s => s.idequipo == _equip.equipo_fk).
                        //Select(s => s.s_equipo_numero_serie).First();

                        _equip.equipo_nombre = GetPersonalizedNameForEquipoNombre(_equip.equipo_fk);





                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                 
                }
                else
                {
                    _equip.equipo_nombre = "";
                    _equip.equipo = null;
                }

                //  Add new item 
                bool isPrex = false;

                tCache = (List<equipo_por_rack>)TempData["epr"];
                TempData["epr"] = tCache;

                foreach (equipo_por_rack sspe in tCache)
                {
                    if (sspe.equipo_fk == _equip.equipo_fk)
                        isPrex = true;
                }
                if (!isPrex)
                {
                    tCache = (List<equipo_por_rack>)TempData["epr"];
                    tCache.Add(_equip);
                    TempData["epr"] = tCache;
                }

            }


            return Json(new[] { _equip }.ToDataSourceResult(request, ModelState));
        }




        [AcceptVerbs(HttpVerbs.Post)]
                          
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, equipo_por_rack _equip)
        {

            List<equipo_por_rack> tCache;

            cloudmanageEntities ent = new cloudmanageEntities();
            
                if (_equip != null && ModelState.IsValid)
                {
                    //     ers.Update(_equip);
                    if (_equip.equipo_fk > 0)
                    {
                        _equip.equipo_nombre = GetPersonalizedNameForEquipoNombre(_equip.equipo_fk);  //  ent.equipo.Where(s => s.idequipo == _equip.equipo_fk).Select(s => s.s_equipo_numero_serie).First();
                }
                    else
                    {
                        _equip.equipo_nombre = "";
                    }



                //  Add new item 
                bool isPrex = false;

                tCache = (List<equipo_por_rack>)TempData["epr"];
                TempData["epr"] = tCache;

                for (int i = 0; i < tCache.Count; i++)
                {
                    if (tCache[i].equipo_fk == _equip.equipo_fk)
                    {
                        tCache[i] = _equip;   // Replace updated item.
                        isPrex = true;
                    }                      
                }

                TempData["epr"] = tCache;



                if (!isPrex)
                {
                    tCache = (List<equipo_por_rack>)TempData["epr"];
                    tCache.Add(_equip);
                    TempData["epr"] = tCache;
                }
                else   // Update fields
                { 
                    
                
                }


            }
            



            return Json(new[] { _equip }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, equipo_por_rack _equip)
        {

            List<equipo_por_rack> tCache;

            //PopulateSoftware();

            if (_equip != null)
            {
                tCache = (List<equipo_por_rack>)TempData["epr"];

                equipo_por_rack sxe_todelete = null;
                // Find item to be removed
                foreach (equipo_por_rack sxeitem in tCache)
                {
                    if (sxeitem.equipo_fk == _equip.equipo_fk && sxeitem.rack_fk == _equip.rack_fk)
                    {
                        sxe_todelete = sxeitem;
                        sxe_todelete.observaciones = "DELETED";

                    }

                }

         


                TempData["epr"] = tCache;


                ers.Destroy(_equip);
            }

            return Json(new[] { _equip }.ToDataSourceResult(request, ModelState));
        }


        private void PopulateEquipos()
        {
            var dataContext = new cloudmanageEntities();
            var equipos = dataContext.equipo
                        .Select(c => new equipo
                        {
                            idequipo = c.idequipo,
                            s_equipo_numero_serie = c.s_equipo_numero_serie
                        })
                        .OrderBy(e => e.s_equipo_numero_serie);

            ViewData["equipos"] = equipos;
            ViewData["defaultEquipo"] = dataContext.equipo.First();

          //*****//  ViewBag.equipos = new SelectList(db.equipo, "idequipo", "s_equipo_numero_serie");

        }



      

    }
}


   
