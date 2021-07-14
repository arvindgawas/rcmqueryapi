using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class ticketdetails
    {
        public int id { get; set; }
        public string ticketno { get; set; }
        public string crnno { get; set; }
        public DateTime? pickupdate { get; set; }
        public string pickupcode { get; set; }
        public string clientcode { get; set; }
        public string customeruniquecode { get; set; }
        public string wronghcin { get; set; }
        public string actualhcin { get; set; }
        public string wrongdispis { get; set; }
        public string actualdispis { get; set; }
        public decimal? wrongamt { get; set; }
        public decimal? actualamt { get; set; }
        public string wrongpickupcode { get; set; }
        public string wrongclientcode { get; set; }
        public string wrongcustomeruniquecode { get; set; }
        public DateTime?  depositiondate { get; set; }
        public string soleid { get; set; }
        public string bankbranchlocation { get; set; }

    }

}
