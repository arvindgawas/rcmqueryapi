using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Interface
{
    public interface IUserMasterRepo
    {
        IQueryable<FTWebApi.Models.UserMaster> GetAllUser();
        FTWebApi.Models.UserMaster GetUser(String ID);
        bool DeleteUser(String ID);
        void UpdatePlan(FTWebApi.Models.UserMaster objExamMaster);
        void InsertPlan(FTWebApi.Models.UserMaster objExamMaster);
        
    }
}
