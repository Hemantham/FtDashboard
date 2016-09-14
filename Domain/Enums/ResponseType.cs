using System;

namespace Dashboard.API.Domain
{
    [Flags]
    public enum ResponseType
    {
        Text = 0,
        YesNo = 1,
        NumericRange = 2
    }
}