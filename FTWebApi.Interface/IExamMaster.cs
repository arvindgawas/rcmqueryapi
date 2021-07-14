using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;

namespace FTWebApi.Interface
{
    public interface IExamMasterRepo
    {
        IQueryable<FTWebApi.Models.ExamMaster> GetAllExam();
        FTWebApi.Models.ExamMaster GetExam(Int32 ID);
        void UpdatePlan(FTWebApi.Models.ExamMaster objExamMaster);
        void InsertPlan(FTWebApi.Models.ExamMaster objExamMaster);
        IQueryable<FTWebApi.Models.QBank> GetQBank();
    }
}




