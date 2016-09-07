using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class DataEntity
    {
        public DataEntity()
        {
            IsActive = true;
        }

        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
