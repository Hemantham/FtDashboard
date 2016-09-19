using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public  class DataPoint
    {
        public string Data { get; set; }
        public string KeyCode { get; set; }
        public string KeyName { get; set; }
        public string Id { get; set; }
        public int XAxisId { get; set; }
        public string XAxisLable { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}
