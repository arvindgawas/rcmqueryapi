//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTWebApi.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class HublocationMast
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HublocationMast()
        {
            this.SubLocationMasts = new HashSet<SubLocationMast>();
        }
    
        public int id { get; set; }
        public string hublocationname { get; set; }
        public string hublocationcode { get; set; }
        public string locationcode { get; set; }
        public string CompanyCode { get; set; }
        public string StateCode { get; set; }
    
        public virtual LocationMast LocationMast { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubLocationMast> SubLocationMasts { get; set; }
    }
}
