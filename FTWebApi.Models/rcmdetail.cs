using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class rcmdetail
    {
        public string crn { get; set; }
        public string clientname { get; set; }
        public string cdpncm { get; set; }
        public string area { get; set; }
        public string region { get; set; }
        public string location { get; set; }
        public string hublocation { get; set; }
        public string customertype { get; set; }
        public string hierarchycode { get; set; }
        public string pickupcode { get; set; }
    }
}
