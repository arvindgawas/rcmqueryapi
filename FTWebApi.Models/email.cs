using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class locemail
    {
        public string ticketno { get; set; }
        public string toemailid { get; set; }
        public string ccemailid { get; set; }
        public string emailsubject { get; set; }
        public string emailbody { get; set; }
        public string emailattachment { get; set; }
        public string emailsubject1 { get; set; }
        public DateTime? sentdate { get; set; }
        public string filepath { get; set; }
        public Boolean location { get; set; }
    }
}
