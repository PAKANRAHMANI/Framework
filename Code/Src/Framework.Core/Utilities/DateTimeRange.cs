using System;
using System.Collections.Generic;
using Framework.Core.Exceptions;

namespace Framework.Core.Utilities;

public struct DateTimeRange : IEqualityComparer<DateTimeRange>
{
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }

    public DateTimeRange(DateTime startDateTime, DateTime endDateTime)
    {
        if (startDateTime >= endDateTime)
            throw new EndDateShouldBeGreaterThanStartDateException();

        this.StartDateTime = startDateTime;
        this.EndDateTime = endDateTime;
    }

    public bool Includes(DateTime value) => value >= StartDateTime && value <= EndDateTime;
    public bool Equals(DateTimeRange x, DateTimeRange y) => x.StartDateTime.Equals(y.StartDateTime) && x.EndDateTime.Equals(y.EndDateTime);
    public int GetHashCode(DateTimeRange obj) => HashCode.Combine(obj.StartDateTime, obj.EndDateTime);

    public override string ToString() => $"{StartDateTime:O} - {EndDateTime:O}";
    public static implicit operator string(DateTimeRange timeRange) => timeRange.ToString();

    public static bool operator ==(DateTimeRange firstTime, DateTimeRange secondTime) => firstTime != null && secondTime != null && (firstTime == secondTime);
    public static bool operator !=(DateTimeRange firstTime, DateTimeRange secondTime) => !(firstTime == secondTime);
    public static bool operator <=(DateTimeRange firstTime, DateTimeRange secondTime) => firstTime != null && firstTime <= secondTime;
    public static bool operator >=(DateTimeRange firstTime, DateTimeRange secondTime) => firstTime != null && firstTime >= secondTime;
    public static bool operator >(DateTimeRange firstTime, DateTimeRange secondTime) => firstTime > secondTime;
    public static bool operator <(DateTimeRange firstTime, DateTimeRange secondTime) => firstTime < secondTime;

}