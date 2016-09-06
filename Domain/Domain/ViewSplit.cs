using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Domain
{
    public class ViewSplit : DataEntity
    {
        public string SplitField { get; set; }
        public string SplitName { get; set; }
        public Filter Filter { get; set; } //Optional
        public SplitType SplitType { get; set; }
    }

    public enum SplitType : short
    {
        All = 0,
        Single = 1,
        Mutiple = 2,
    }
}
