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
    
    public partial class LocationEmailHistory
    {
        public int ID { get; set; }
        public string Ticketno { get; set; }
        public string toemailid { get; set; }
        public string emailsubject { get; set; }
        public string emailbody { get; set; }
        public Nullable<System.DateTime> emailsentdate { get; set; }
        public string ccemailid { get; set; }
    }
}