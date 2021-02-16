using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Inventario.Models
{
    public class equipo_por_rack_service : IDisposable
    {
   
        private static bool UpdateDatabase = false;
        private cloudmanageEntities entities;

        public equipo_por_rack_service(cloudmanageEntities entities)
        {
            this.entities = entities;
        }



        public IList<equipo_por_rack> GetAll(int id)
        {

            List<equipo_por_rack> cleanList = new List<equipo_por_rack>();
            // var result = HttpContext.Current.Session["software_por_equipo"] as IList<software_por_equipo>;

            if (!UpdateDatabase)    // result == null || UpdateDatabase
            {
                var result = entities.equipo_por_rack.Include(s => s.equipo).Where(ss => ss.rack_fk  == id).ToList();

                foreach (equipo_por_rack s in result)
                {
                    equipo_por_rack it = new equipo_por_rack();
                    it.equipo_fk = s.equipo_fk;
                    it.rack_fk = s.rack_fk;
                    it.equipo_nombre = s.equipo.s_equipo_numero_serie + " - " + s.equipo.marca_modelo.modelo + " - (" + s.equipo.altura_u + "U)";
                    it.equipo_por_rack_unique_id = s.equipo_por_rack_unique_id;
                    it.observaciones = s.observaciones;
                    it.inicia_u = s.inicia_u;
                    cleanList.Add(it);
                }


                foreach (equipo_por_rack s in cleanList)
                {
                   // s.software_nombre = s.software.nombre;
                    s.equipo_por_rack_unique_id = s.rack_fk * 1000 + s.equipo_fk;
                     s.equipo = null;
                      s.rack = null;
                }


              //  HttpContext.Current.Session["software_por_equipo"] = cleanList;
            }

            return cleanList;
        }



        public IList<equipo_por_rack> GetAll()
        {

            var result = HttpContext.Current.Session["equipo_por_rack"] as IList<equipo_por_rack>;

            if (result == null || UpdateDatabase)
            {
                //result = entities.software_por_equipo.Include(s => s.equipo).Include(s => s.software).ToList();

                //result = entities.software_por_equipo.Where(s => s.software_fk > 0).ToList();

                result = entities.equipo_por_rack.Include(s => s.equipo).ToList();


                HttpContext.Current.Session["equipo_por_rack"] = result;
            }

            return result;
        }

        public IEnumerable<equipo_por_rack> Read(int id)
        {
            return GetAll(id);
        }

        public IEnumerable<equipo_por_rack> Read()
        {
            return GetAll();
        }

    


        public void Create(equipo_por_rack _equipo_por_rack)
        {
            entities.equipo_por_rack.Add(_equipo_por_rack);
            entities.SaveChanges();
        }


        public void Update(equipo_por_rack _equipo_por_rack)
        {
           
              
                entities.equipo_por_rack.Attach(_equipo_por_rack);
                entities.Entry(_equipo_por_rack).State = EntityState.Modified;
                entities.SaveChanges();
          
        }

        public void Destroy(equipo_por_rack soft)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.equipo_fk == soft.equipo_fk && p.rack_fk == soft.rack_fk);   // vallenato
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new equipo_por_rack();

                entity.equipo_fk = soft.equipo_fk;
                entity.rack_fk = soft.rack_fk;

                entities.equipo_por_rack.Attach(entity);

                entities.equipo_por_rack.Remove(entity);

             

                entities.SaveChanges();
            }
        }

        public equipo_por_rack One(Func<equipo_por_rack, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }

        public int GetNextEquipoId()
        {
            try
            {
                return entities.equipo.Max(eq => eq.idequipo) + 1;
            }
            catch
            {
                return 1;
            }
         
        }

    }

}