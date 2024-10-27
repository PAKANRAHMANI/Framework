using System;
using System.Collections.Generic;
using Framework.Core.Exceptions;

namespace Framework.Core.Utilities;

public struct TimeRange : IEqualityComparer<TimeRange>
{
    public TimeOnly StartTime { get; private set; }

    public TimeOnly EndTime { get; private set; }
    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            throw new EndDateShouldBeGreaterThanStartDateException();

        this.StartTime = startTime;
        this.EndTime = endTime;
    }
    public bool Includes(TimeOnly value) => value >= StartTime && value <= EndTime;
    public bool Equals(TimeRange x, TimeRange y)
    {
        return x.StartTime.Equals(y.StartTime) && x.EndTime.Equals(y.EndTime);
    }

    public int GetHashCode(TimeRange obj)
    {
        return HashCode.Combine(obj.StartTime, obj.EndTime);
    }

    public override string ToString() => $"{StartTime:HH:mm:ss} - {EndTime:HH:mm:ss}";


    public static implicit operator string(TimeRange timeRange) => timeRange.ToString();


    public static bool operator ==(TimeRange firstTime, TimeRange secondTime)
    {
        return firstTime != null && secondTime != null && (firstTime == secondTime);
    }

    public static bool operator !=(TimeRange firstTime, TimeRange secondTime)
    {
        return !(firstTime == secondTime);
    }

    public static bool operator <=(TimeRange firstTime, TimeRange secondTime)
    {
        return firstTime != null && firstTime <= secondTime;
    }

    public static bool operator >=(TimeRange firstTime, TimeRange secondTime)
    {
        return firstTime != null && firstTime >= secondTime;
    }

    public static bool operator >(TimeRange firstTime, TimeRange secondTime)
    {
        return firstTime > secondTime;
    }

    public static bool operator <(TimeRange firstTime, TimeRange secondTime)
    {
        return firstTime < secondTime;
    }
}