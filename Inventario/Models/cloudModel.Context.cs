﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventario.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class cloudmanageEntities : DbContext
    {
        public cloudmanageEntities()
            : base("name=cloudmanageEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<atributo> atributo { get; set; }
        public virtual DbSet<atributo_por_equipo> atributo_por_equipo { get; set; }
        public virtual DbSet<ciudad> ciudad { get; set; }
        public virtual DbSet<compania> compania { get; set; }
        public virtual DbSet<equipo> equipo { get; set; }
        public virtual DbSet<equipo_por_rack> equipo_por_rack { get; set; }
        public virtual DbSet<marca_modelo> marca_modelo { get; set; }
        public virtual DbSet<pais> pais { get; set; }
        public virtual DbSet<proveedor> proveedor { get; set; }
        public virtual DbSet<puerto> puerto { get; set; }
        public virtual DbSet<puerto_por_equipo> puerto_por_equipo { get; set; }
        public virtual DbSet<rack> rack { get; set; }
        public virtual DbSet<responsable> responsable { get; set; }
        public virtual DbSet<software> software { get; set; }
        public virtual DbSet<software_por_equipo> software_por_equipo { get; set; }
        public virtual DbSet<tipo_dato> tipo_dato { get; set; }
        public virtual DbSet<tipo_de_equipo> tipo_de_equipo { get; set; }
        public virtual DbSet<tipo_de_software> tipo_de_software { get; set; }
        public virtual DbSet<ubicacion> ubicacion { get; set; }
    }
}
