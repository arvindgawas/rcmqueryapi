using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class qm
    {
        public int QuestionID { get; set; }
        public int? QuestionNo { get; set; }
        public string QuestionName { get; set; }
        public Nullable<int> TotalMarks { get; set; }
        public Nullable<bool> MustFlag { get; set; }
        public Nullable<int> DifficultLevel { get; set; }
        public Nullable<int> QuestionBankId { get; set; }
    }
}
