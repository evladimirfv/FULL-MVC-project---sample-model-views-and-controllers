using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Inventario.Models
{
    public class puerto_por_equipo_service : IDisposable
    {
   
        private static bool UpdateDatabase = false;
        private cloudmanageEntities entities;

        public puerto_por_equipo_service(cloudmanageEntities entities)
        {
            this.entities = entities;
        }



        public IList<puerto_por_equipo> GetAll(int id)
        {

            List<puerto_por_equipo> cleanList = new List<puerto_por_equipo>();

            // var result = HttpContext.Current.Session["software_por_equipo"] as IList<software_por_equipo>;

            

            if (!UpdateDatabase)    // result == null || UpdateDatabase
            {
 
                var result = entities.puerto_por_equipo.Include(s => s.puerto).Where(ss => ss.equipo_fk  == id).ToList();


                foreach (puerto_por_equipo s in result)
                {
                    puerto_por_equipo it = new puerto_por_equipo();
                    it.equipo_fk = s.equipo_fk;
                    it.puerto_fk = s.puerto_fk;
                    it.puerto_nombre = s.puerto.nombre;
                    it.puerto_por_equipo_unique_id = s.puerto_por_equipo_unique_id;
                    it.numero_de_puertos = s.numero_de_puertos;
                    cleanList.Add(it);
                }


                // test

                foreach (puerto_por_equipo s in cleanList)
                {
                   // s.software_nombre = s.software.nombre;
                    s.puerto_por_equipo_unique_id = s.equipo_fk * 1000 + s.puerto_fk;
                     s.puerto = null;
                      s.equipo = null;
                }


              //  HttpContext.Current.Session["software_por_equipo"] = cleanList;
            }

            return cleanList;
        }



        public IList<puerto_por_equipo> GetAll()
        {

            var result = HttpContext.Current.Session["puerto_por_equipo"] as IList<puerto_por_equipo>;

            if (result == null || UpdateDatabase)
            {
                //result = entities.software_por_equipo.Include(s => s.equipo).Include(s => s.software).ToList();

                //result = entities.software_por_equipo.Where(s => s.software_fk > 0).ToList();

                result = entities.puerto_por_equipo.Include(s => s.puerto).ToList();


                HttpContext.Current.Session["puerto_por_equipo"] = result;
            }

            return result;
        }

        public IEnumerable<puerto_por_equipo> Read(int id)
        {
            return GetAll(id);
        }

        public IEnumerable<puerto_por_equipo> Read()
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


        public void Create(puerto_por_equipo _puerto_por_equipo)
        {

       

            entities.puerto_por_equipo.Add(_puerto_por_equipo);
            entities.SaveChanges();

            //   product.ProductID = entity.ProductID;

        }


        public void Update(puerto_por_equipo _puerto_por_equipo)
        {
           
              
                entities.puerto_por_equipo.Attach(_puerto_por_equipo);
                entities.Entry(_puerto_por_equipo).State = EntityState.Modified;
                entities.SaveChanges();
          
        }

        public void Destroy(puerto_por_equipo soft)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.equipo_fk == soft.equipo_fk && p.puerto_fk == soft.puerto_fk);
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new puerto_por_equipo();

                entity.equipo_fk = soft.equipo_fk;
                entity.puerto_fk = soft.puerto_fk;

                entities.puerto_por_equipo.Attach(entity);

                entities.puerto_por_equipo.Remove(entity);

             

                entities.SaveChanges();
            }
        }

        public puerto_por_equipo One(Func<puerto_por_equipo, bool> predicate)
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