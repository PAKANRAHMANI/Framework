using System;
using System.Collections.Generic;
using Framework.Core.Exceptions;

namespace Framework.Core.Utilities
{
    public struct DateRange : IEqualityComparer<DateRange>
    {
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new EndDateShouldBeGreaterThanStartDateException();

            this.StartDate = startDate;
            this.EndDate = endDate;
        }
        public bool Includes(DateTime value) => value >= StartDate && value <= EndDate;
        public bool Equals(DateRange x, DateRange y)
        {
            return x.StartDate.Equals(y.StartDate) && x.EndDate.Equals(y.EndDate);
        }

        public int GetHashCode(DateRange obj)
        {
            return HashCode.Combine(obj.StartDate, obj.EndDate);
        }
    }
}
