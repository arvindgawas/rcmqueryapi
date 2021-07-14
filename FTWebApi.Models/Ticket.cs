using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class Ticket
    {
        
        public string ticketno { get; set; }
        public DateTime ticketdate { get; set; }
        public TimeSpan tickettime { get; set; }
        public string tickettype { get; set; }
        public string assignedto { get; set; }
        public string batchno { get; set; }
        public string oldbatchno { get; set; }
        public string acceptstatus { get; set; }
        public DateTime resolveddate { get; set; }
        public string emailsubject { get; set; }
        public string emailfrom { get; set; }
        public string emailbody { get; set; }
        public string bank { get; set; }
        public string pickupcode { get; set; }
        public string clientcode { get; set; }
        public string crnno { get; set; }
        public string client { get; set; }
        public string area { get; set; }
        public string cdpncm { get; set; }
        public string customertype { get; set; }
        public string hierarchycode { get; set; }
        public string region { get; set; }
        public string location { get; set; }
        public string hublocation { get; set; }
        public string problem { get; set; }
        public string mistakedoneby { get; set; }
        public string errortype { get; set; }
        public string status { get; set; }
        public string rejectremark { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string createduser { get; set; }
        public DateTime createddate { get; set; }
        public string modifieduser { get; set; }
        public DateTime modifieddate { get; set; }

    }
}


