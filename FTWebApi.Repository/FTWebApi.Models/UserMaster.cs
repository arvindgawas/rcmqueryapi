using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class UserMaster
    {
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string Status { get; set; }
        public string Createdby { get; set; }
    }
}


