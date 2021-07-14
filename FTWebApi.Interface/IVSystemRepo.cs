using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;

namespace FTWebApi.Interface
{

    public interface IVSystemRepo
    {
        IQueryable<vSystem> GetAllVSystem();
        vSystem GetVSystem(Int32 ID);
        void UpdatePlan(vSystem objvSystem);
        void InsertPlan(vSystem objvSystem);
    }
}
