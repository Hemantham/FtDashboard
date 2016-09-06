using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models;

namespace Dashboard.API.Domain
{
    public class Question : DataEntity
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public QuestionGroup QuestionGroup { get; set; }
        
    }
}
