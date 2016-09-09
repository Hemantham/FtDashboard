﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class DashboardView : DataEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string FieldOfInterest { get; set; }
        public string XAxislable { get; set; }
        public string XAxisId { get; set; }
        public DashboardView Parent { get; set; }
        public  short ViewOrder { get; set; }
        public ICollection<DashboardView> ChildrenViews { get; set; }
    }

    public enum ChartType : short
    {
      line = 0,
      bar = 1,
    }
}
