using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class Product : DataEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<ProductView> ProductViews { get; set; }
        public Filter Filter { get; set; }



    }
}
