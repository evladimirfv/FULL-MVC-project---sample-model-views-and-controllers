//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class rack
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rack()
        {
            this.equipo_por_rack = new HashSet<equipo_por_rack>();
        }
    
        public int idrack { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public Nullable<int> ubicacion_fk { get; set; }
        public Nullable<int> orden { get; set; }
        public Nullable<int> altura_u { get; set; }
        public Nullable<int> prof_inch { get; set; }
        public Nullable<int> proveedor_fk { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<equipo_por_rack> equipo_por_rack { get; set; }
        public virtual proveedor proveedor { get; set; }
        public virtual ubicacion ubicacion { get; set; }
    }
}