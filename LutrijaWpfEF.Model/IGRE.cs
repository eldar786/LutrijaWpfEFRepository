//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LutrijaWpfEF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class IGRE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IGRE()
        {
            this.UPLATA = new HashSet<UPLATA>();
        }
    
        public int ID { get; set; }
        public string NAZIV { get; set; }
        public Nullable<int> SIF_ISP { get; set; }
        public Nullable<int> SIF_UPL { get; set; }
        public Nullable<int> SIF_BAZ_IGR { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UPLATA> UPLATA { get; set; }
    }
}