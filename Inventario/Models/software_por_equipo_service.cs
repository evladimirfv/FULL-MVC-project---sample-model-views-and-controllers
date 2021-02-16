using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Inventario.Models
{
    public class software_por_equipo_service : IDisposable
    {
   
        private static bool UpdateDatabase = false;
        private cloudmanageEntities entities;

        public software_por_equipo_service(cloudmanageEntities entities)
        {
            this.entities = entities;
        }



        public IList<software_por_equipo> GetAll(int id)
        {

            List<software_por_equipo> cleanList = new List<software_por_equipo>();

            // var result = HttpContext.Current.Session["software_por_equipo"] as IList<software_por_equipo>;

            

            if (!UpdateDatabase)    // result == null || UpdateDatabase
            {
 
                var result = entities.software_por_equipo.Include(s => s.software).Where(ss => ss.equipo_fk  == id).ToList();


                foreach (software_por_equipo s in result)
                {
                    software_por_equipo it = new software_por_equipo();
                    it.equipo_fk = s.equipo_fk;
                    it.software_fk = s.software_fk;
                    it.software_nombre = s.software.nombre;
                    it.software_por_equipo_unique_id = s.software_por_equipo_unique_id;
                    it.observaciones = s.observaciones;
                    cleanList.Add(it);
                }


                // test

                foreach (software_por_equipo s in cleanList)
                {
                   // s.software_nombre = s.software.nombre;
                    s.software_por_equipo_unique_id = s.equipo_fk * 1000 + s.software_fk;
                     s.software = null;
                      s.equipo = null;
                }


              //  HttpContext.Current.Session["software_por_equipo"] = cleanList;
            }

            return cleanList;
        }



        public IList<software_por_equipo> GetAll()
        {

            var result = HttpContext.Current.Session["software_por_equipo"] as IList<software_por_equipo>;

            if (result == null || UpdateDatabase)
            {
                //result = entities.software_por_equipo.Include(s => s.equipo).Include(s => s.software).ToList();

                //result = entities.software_por_equipo.Where(s => s.software_fk > 0).ToList();

                result = entities.software_por_equipo.Include(s => s.software).ToList();


                HttpContext.Current.Session["software_por_equipo"] = result;
            }

            return result;
        }

        public IEnumerable<software_por_equipo> Read(int id)
        {
            return GetAll(id);
        }

        public IEnumerable<software_por_equipo> Read()
        {
            return GetAll();
        }

        ////public void Create(software_por_equipo _software_por_equipo)
        ////{

        ////        var entity = new software_por_equipo();



        ////        entities.software_por_equipo.Add(_software_por_equipo);
        ////        entities.SaveChanges();

        ////     //   product.ProductID = entity.ProductID;

        ////}


        public void Create(software_por_equipo _software_por_equipo)
        {

       

            entities.software_por_equipo.Add(_software_por_equipo);
            entities.SaveChanges();

            //   product.ProductID = entity.ProductID;

        }


        public void Update(software_por_equipo _software_por_equipo)
        {
           
              
                entities.software_por_equipo.Attach(_software_por_equipo);
                entities.Entry(_software_por_equipo).State = EntityState.Modified;
                entities.SaveChanges();
          
        }

        public void Destroy(software_por_equipo soft)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.equipo_fk == soft.equipo_fk && p.software_fk == soft.software_fk);   // vallenato
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new software_por_equipo();

                entity.equipo_fk = soft.equipo_fk;
                entity.software_fk = soft.software_fk;

                entities.software_por_equipo.Attach(entity);

                entities.software_por_equipo.Remove(entity);

             

                entities.SaveChanges();
            }
        }

        public software_por_equipo One(Func<software_por_equipo, bool> predicate)
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