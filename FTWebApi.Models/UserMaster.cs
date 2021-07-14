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
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string Bank { get; set; }
        public string UserType { get; set; }
        public string UserPriority { get; set; }
        public string UserRole { get; set; }
        public string Createdby { get; set; }
        public DateTime Createddate { get; set; }
        public string Modifiedby { get; set; }
        public DateTime Modifieddate { get; set; }
        public userbankmap[] lstuserbankmap  { get; set; }

}


    


}

