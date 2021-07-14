using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class errortype
    {
        public Int32 ID { get; set; }
        public string ErrorName { get; set; }

    }

    public class Userdd
    {
        public string ID { get; set; }
        public string UserName { get; set; }

    }

    public class ddlist
    {
        public IQueryable<errortype> errortypelst { get; set; }
        public IQueryable<Userdd>  Userddlist { get; set; }
    }

}


