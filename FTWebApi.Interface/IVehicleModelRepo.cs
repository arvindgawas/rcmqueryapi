using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;

namespace FTWebApi.Interface
{
    public interface IVehicleModelRepo
    {
        IQueryable<VCategory> GetCategory();
        IQueryable<VModel> GetVModel();
        IQueryable<clsSpareMaster> GetAllSpares();
        clsSpareMaster GetSpares(Int32 ID);
        void UpdatePlan(clsSpareMaster objclsSpareMaster);
        void InsertPlan(clsSpareMaster objclsSpareMaster);
    }
}
