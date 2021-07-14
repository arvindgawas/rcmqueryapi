using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class optionmaster
    {
        public int QuestionID { get; set; }
        public int OptionID { get; set; }
        public string OptionName1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionName4 { get; set; }
        public string OptionName5 { get; set; }
        public Nullable<bool> RightAnsFlag { get; set; }
    }
}

