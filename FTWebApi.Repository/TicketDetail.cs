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
    
    public partial class TicketDetail
    {
        public int ID { get; set; }
        public string TicketID { get; set; }
        public Nullable<System.DateTime> pickupdate { get; set; }
        public string pickupcode { get; set; }
        public string clientcode { get; set; }
        public string crnno { get; set; }
        public string customeruniquecode { get; set; }
        public string wronghcin { get; set; }
        public string actualhcin { get; set; }
        public string wrongdispis { get; set; }
        public string actualdispis { get; set; }
        public Nullable<decimal> wrongamt { get; set; }
        public Nullable<decimal> actualamt { get; set; }
        public string wrongpickupcode { get; set; }
        public string wrongclientcode { get; set; }
        public string wrongcustomeruniquecode { get; set; }
        public Nullable<System.DateTime> depostiondate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string soleid { get; set; }
        public string bankbranchlocation { get; set; }
    }
}
