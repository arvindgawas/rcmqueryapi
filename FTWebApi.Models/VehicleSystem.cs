using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class vSystem
    {
        public decimal ID { get; set; }
        public string CompanyName { get; set; }
        public string VehicleSystemID { get; set; }
        public string SystemName { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string OperationCenter { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}
