using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public  class ExamMaster
    {
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public Nullable<int> QuestionBankId { get; set; }
        public Nullable<int> MinQuestionLimit { get; set; }
        public Nullable<int> MinPassingScore { get; set; }
        public Nullable<int> MaxAttempts { get; set; }
        public Nullable<System.TimeSpan> Timeline { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime>  ExamDate;
        public Nullable<System.DateTime> TargetDate;
        public Nullable<int> NoofQuestPerPage { get; set; }
    }
}
