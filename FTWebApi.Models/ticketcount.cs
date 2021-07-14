using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class ticketcount
    {
        public string userid { get; set; }
        public DateTime date { get; set; }
        public string bank { get; set; }

        public Int32 opencount { get; set; }

        public Int32 acceptcount { get; set; }

        public Int32 duplicatecount { get; set; }

        public Int32 closecount { get; set; }

        public Int32 totalcount { get; set; }

        public Int32 pendingcount { get; set; }

        public Int32 rejectcount { get; set; }
    }
}

