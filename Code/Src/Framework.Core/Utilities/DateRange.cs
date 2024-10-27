using System;
using System.Collections.Generic;
using Framework.Core.Exceptions;

namespace Framework.Core.Utilities;

public struct DateRange : IEqualityComparer<DateRange>
{
    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }
    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (startDate >= endDate)
            throw new EndDateShouldBeGreaterThanStartDateException();

        this.StartDate = startDate;
        this.EndDate = endDate;
    }
    public bool Includes(DateOnly value) => value >= StartDate && value <= EndDate;
    public bool Equals(DateRange x, DateRange y)
    {
        return x.StartDate.Equals(y.StartDate) && x.EndDate.Equals(y.EndDate);
    }

    public int GetHashCode(DateRange obj)
    {
        return HashCode.Combine(obj.StartDate, obj.EndDate);
    }
    public override string ToString()
    {
        return $"{StartDate:yy-MM-dd} - {EndDate:yy-MM-dd}";
    }

    public static implicit operator string(DateRange timeRange)
    {
        return timeRange.ToString();
    }

    public static bool operator ==(DateRange firstDate, DateRange secondDate)
    {
        return firstDate != null && secondDate != null && (firstDate == secondDate);
    }

    public static bool operator !=(DateRange firstDate, DateRange secondDate)
    {
        return !(firstDate == secondDate);
    }

    public static bool operator <=(DateRange firstDate, DateRange secondDate)
    {
        return firstDate != null && firstDate <= secondDate;
    }

    public static bool operator >=(DateRange firstDate, DateRange secondDate)
    {
        return firstDate != null && firstDate >= secondDate;
    }

    public static bool operator >(DateRange firstDate, DateRange secondDate)
    {
        return firstDate > secondDate;
    }

    public static bool operator <(DateRange firstDate, DateRange secondDate)
    {
        return firstDate < secondDate;
    }


}