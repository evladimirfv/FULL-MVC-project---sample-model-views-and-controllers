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
    
    public partial class tipo_dato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tipo_dato()
        {
            this.atributo = new HashSet<atributo>();
        }
    
        public int idtipo_dato { get; set; }
        public string nombre { get; set; }
        public string formato_tabla { get; set; }
        public string regex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<atributo> atributo { get; set; }
    }
}
