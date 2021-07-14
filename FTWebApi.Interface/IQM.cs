using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Interface
{
    public interface IQMRepo
    {
        IQueryable<FTWebApi.Models.qm> GetAllQM();
        FTWebApi.Models.qm GetQM(Int32 ID);
        void UpdatePlan(FTWebApi.Models.qm objqm);
        void InsertPlan(FTWebApi.Models.qm objqm);
        IQueryable<FTWebApi.Models.QBank> GetQBank();
        void InsertOptions(FTWebApi.Models.optionmaster objom);
    }
}
