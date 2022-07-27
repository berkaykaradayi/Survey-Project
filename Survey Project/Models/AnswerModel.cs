using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey_Project.Models
{
    public class AnswerModel
    {
        //public int Id {get; set;}

        public string Code { get; set; }
        public string NameSurname { get; set; }
 
        public string Question { get; set; }
        public string Answer { get; set; }



    }
}