using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Models
{
    public class QuestionMaster
    {
        public int QuestionID { get; set; }
        public int? QuestionNo { get; set; }
        public string QuestionName { get; set; }
        //public IQueryable<OptionMaster> OptionMasters { get; set; }
    }

    public class OptionMaster
    {
        public int OptionID { get; set; }
        public int QuestionID { get; set; }
        public string OptionNo { get; set; }
        public string OptionName { get; set; }
    }

    public class OptionMasterlist
    {
        public int OptionID { get; set; }
        public int QuestionID { get; set; }
        public string OptionNo { get; set; }
        public string OptionName { get; set; }
        public Nullable<bool> RightAnsFlag { get; set; }
    }

    public class QOmodel
    {
        public int QuestionID { get; set; }
        public int? QuestionNo { get; set; }
        public string QuestionName { get; set; }
        public int OptionID { get; set; }
        public string OptionNo { get; set; }
        public string OptionName { get; set; }
        public Nullable<bool> RightAnsFlag { get; set; }
    }

    public class QOmodelView
    {
        public int QuestionID { get; set; }
        public int? QuestionNo { get; set; }
        public string QuestionName { get; set; }
        public int OptionID { get; set; }
        public string OptionNo { get; set; }
        public string OptionName { get; set; }
        public Nullable<bool> RightAnsFlag { get; set; }
        public Nullable<bool> Selected { get; set; }
    }

}





