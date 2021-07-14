using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class customer
    {
        public Int32 ID { get; set; }
        public string Name { get; set; }

    }

    public class Userddl
    {
        public string ID { get; set; }
        public string UserName { get; set; }

    }

    public class ddlisttracker
    {
        public IQueryable<customer> customerlst { get; set; }
        public IQueryable<Userddl> Userlst { get; set; }
    }
}

