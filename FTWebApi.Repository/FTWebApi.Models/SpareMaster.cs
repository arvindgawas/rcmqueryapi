using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class clsSpareMaster
    {
        public int ID { get; set; }
        public string CompanyCode { get; set; }
        public Nullable<int> VehicleModelID { get; set; }
        public string ManufacturerName { get; set; }
        public string VehicleModel { get; set; }
        public string FuelType { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Spare { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}
