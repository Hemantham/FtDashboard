using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class FieldValueModel
    {
        public bool IsSelected { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Answer { get; set; }
        public long QuestionId { get; set; }

    }
}
