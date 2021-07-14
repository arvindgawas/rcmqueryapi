using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class EmpExamMap
    {
        public string EmployeeId { get; set; }
        public int ExamID { get; set; }
        public Nullable<int> CorrectAns { get; set; }
        public Nullable<int> PerScore { get; set; }
        public string Status { get; set; }
    }
}
