using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class batch
    {
        public string batchid { get; set; }
        public DateTime batchdate { get; set; }
        public string fromemail { get; set; }
        public string emailsubject { get; set; }
        public string emailbody { get; set; }

    }
}

