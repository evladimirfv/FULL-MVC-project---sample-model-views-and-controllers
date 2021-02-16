using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;

namespace Inventario.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contacto.";

            return View();
        }


        public class HierarchicalViewModel
        {
            public int ID { get; set; }
            public string Name{ get; set; }
            public int? ParentID{ get; set; }
            public bool HasChildren{ get; set; }

            public bool Expanded { get; set; }

            public string SpriteCssClass{ get; set; }
        }



        public JsonResult Treeread(int? id)
        {




            using (var Context = new cloudmanageEntities())
            {
                var cQuery = Context.ubicacion.Select(c => new HierarchicalViewModel
                {
                    ID = c.idubicacion,
                    Name = c.nombre,
                    ParentID = null,
                    HasChildren = (c.rack.Any() || c.equipo.Any()),     // c.rack.Any(),
                    SpriteCssClass = "fa fa-map-marker"

                })

                    .Union(Context.rack.Select(r => new HierarchicalViewModel
                    {
                        ID = r.idrack,
                        Name = r.nombre,
                        ParentID = r.ubicacion_fk,
                        HasChildren = r.equipo_por_rack.Any(),
                        SpriteCssClass = "fa fa-server"
                    }))

                    .Union(Context.equipo.Select(q => new HierarchicalViewModel
                    {
                        ID = q.idequipo,
                        Name = q.s_equipo_numero_serie + " " + q.marca_modelo.marca_modelo_string,
                        ParentID = q.ubicacion_fk,
                        HasChildren = false,
                        SpriteCssClass = "fa fa-desktop"
                    }));
                
                var result = cQuery.ToList()
                    .Where(x => id.HasValue ? x.ParentID == id : x.ParentID == null)
                    .Select(item => new
                    {
                        id = item.ID,
                        Name = item.Name,
                       //  expanded = item.Expanded,
                       expanded = false,    //(item.SpriteCssClass == "fa fa-map-marker"),
                        hasChildren = item.HasChildren,
                        spriteCssClass = item.SpriteCssClass,
                        
 
                    });
 
                    JsonResult jr = Json(result, JsonRequestBehavior.AllowGet);

                    return jr;
            }
        }


        public bool expandedFunction(string[] nodeFromJS,string localNodeId,string filterVal)
        {

            if (filterVal != "")
                return true;

            if (nodeFromJS.Contains(localNodeId))
                return true;
            else
                return false;
        }
        public JsonResult Treereadfilter(int? id, string filterBy, string filterVal, string expandedNodes)
        {

            bool isFilterSettedUp = false;
            // "{\"9\":true}"
            string expNum = expandedNodes.Replace("\"", "").
                Replace(":", "").Replace("{", "").Replace("}","").Replace("true","");

            string[] jsExpandedNode = new string[0];
            if (expNum != "undefined")
            {
                jsExpandedNode = expNum.Split(',');
            }



            string filterMarca = "";
            string filterProveedor = "";
            string filterResponsable = "";
            string filterUbicacion = "";
            string filterCategoria = "";   // Tipo,  ej: ruteador


            // Check object to search for

            switch (filterBy)
            {
                case "ubicacion":
                    filterUbicacion = filterVal;
                    break;
                case "marca":
                    filterMarca = filterVal;
                    isFilterSettedUp = true;
                    break;
                case "proveedor":
                    filterProveedor = filterVal;
                    isFilterSettedUp = true;
                    break;
                case "responsable":
                    filterResponsable = filterVal;
                    isFilterSettedUp = true;
                    break;
                case "categoria":
                    filterCategoria = filterVal;
                    isFilterSettedUp = true;
                    break;

            }

            if (filterVal == "")
                isFilterSettedUp = false;

            using (var Context = new cloudmanageEntities())
            {


                var cQuery = Context.ubicacion.
                    Where(s => s.nombre.Contains(filterUbicacion)).
                    Select(c => new HierarchicalViewModel
                {
                    ID = c.idubicacion,
                    Name = c.nombre,
                    ParentID = null,
                    HasChildren = (c.rack.Any() || c.equipo.Any()),     // c.rack.Any(),
                    SpriteCssClass = "fa fa-map-marker"

                })

                    .Union(Context.rack.
                   // Where(r => r.proveedor.nombre_comercial.Contains(filterProveedor)).                   
                    Select(r => new HierarchicalViewModel
                    {
                        ID = r.idrack,
                        Name = r.nombre,
                        ParentID = r.ubicacion_fk,
                        HasChildren = false,           // r.equipo_por_rack.Any(),
                        SpriteCssClass = "fa fa-server"
                    }))

                    .Union(Context.equipo.
                    Where(s => s.proveedor.nombre_comercial.Contains(filterProveedor)).
                    Where(t => t.marca_modelo.marca.ToUpper().Contains(filterMarca.ToUpper()) || t.marca_modelo.modelo.ToUpper().Contains(filterMarca.ToUpper())).                
                    Where(t => t.responsable.nombre.Contains(filterResponsable)).
                    Where(t => t.tipo_de_equipo.categoria.Contains(filterCategoria)).
                    Where(t => t.equipo_por_rack.Count <= 0).
                    Select(q => new HierarchicalViewModel
                    {
                        ID = q.idequipo,
                        
                        Name = q.s_equipo_numero_serie + " - " + q.marca_modelo.marca + " - " +  q.marca_modelo.modelo,
                        ParentID = q.ubicacion_fk,
                        HasChildren = false,
                        SpriteCssClass = "fa fa-desktop"
                    }));

                var result = cQuery.ToList()
                    .Where(x => id.HasValue ? x.ParentID == id : x.ParentID == null)
                    .Select(item => new
                    {
                        id = item.ID,
                        Name = item.Name,
                        //  expanded = item.Expanded,
                        // expanded = isFilterSettedUp,     // false,    //(item.SpriteCssClass == "fa fa-map-marker"),
                        // expanded = true,
                        expanded = expandedFunction(jsExpandedNode,item.ID.ToString(),filterVal),
                        hasChildren = item.HasChildren,
                        spriteCssClass = item.SpriteCssClass,


                    });

                JsonResult jr = Json(result, JsonRequestBehavior.AllowGet);

                return jr;
            }
        }


    }
}