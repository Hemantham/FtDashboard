using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;

namespace Dashboard.API.Domain
{
   public class RecencyType :DataEntity
    {
        public RecencyTypes RecencyTypes { get; set; }
        public string Name { get; set; }
    }
}
