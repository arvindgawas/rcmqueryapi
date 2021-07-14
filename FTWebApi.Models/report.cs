using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class report
    {
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string user { get; set; }
        public string region { get; set; }
        public string location { get; set; }
        public string hublocation { get; set; }
        public string customer { get; set; }
        public string customertype { get; set; }
        public string cdpncm { get; set; }
        public string issuetype { get; set; }
        public string sla { get; set; }


    }
}


