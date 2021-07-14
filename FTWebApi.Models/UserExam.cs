using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class UserExam
    {
        public string EmployeeId { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public Nullable<DateTime> ExamDate { get; set; }
        public Nullable<System.TimeSpan> Timeline { get; set; }
        public Nullable<int> CorrectAns { get; set; }
        public Nullable<int> PerScore { get; set; }
        public string Status { get; set; }
    }
}



