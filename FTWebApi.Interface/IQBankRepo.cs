using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FTWebApi.Interface
{
    public interface IQBankRepo
    {
        IQueryable<FTWebApi.Models.QBank> GetAllQBank();
        FTWebApi.Models.QBank GetQBank(int ID);
        bool DeleteQBank(int QBankid);
        void UpdatePlan(FTWebApi.Models.QBank objQBank);
        void InsertPlan(FTWebApi.Models.QBank objQBank);
    }
}




