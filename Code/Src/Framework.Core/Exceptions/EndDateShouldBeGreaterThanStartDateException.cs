﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Exceptions
{
    public class EndDateShouldBeGreaterThanStartDateException : FrameworkException
    {
        public EndDateShouldBeGreaterThanStartDateException():base(-1, "EndDate should be greater than StartDate")
        {
            
        }
    }
}
