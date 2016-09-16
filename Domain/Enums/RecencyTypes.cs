using System;

namespace Dashboard.API.Enums
{
    [Flags]
    public enum RecencyTypes
    {
        Weekly = 0,
        Fortnightly = 1,
        Monthly = 2,
        Quarterly = 3
    }
}