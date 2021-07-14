using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;

namespace FTWebApi.Interface
{
    public interface IQuestionMasterRepo
    {
        IQueryable<QuestionMaster> GetQestions();
        IQueryable<QOmodel> GetQOmodel();
    }
}
