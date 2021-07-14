using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class user
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Int32?  questcount { get; set; }
        public Int32?  NoofQuestPerPage { get; set; }
    }
}



