using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class CorrectAns
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public Nullable<int> ExamId { get; set; }
    }
}



