using System;
using Framework.Core.Clock;

namespace Framework.Testing.Core.Stubs
{
    public class DateTimeStub : IDateTime
    {
        private DateTime _now;

        public DateTimeStub(DateTime now)
        {
            this._now = now;
        }

        public DateTimeStub()
        {
            this._now = DateTime.UtcNow;
        }

        public static DateTimeStub CreateClockWhichSetsNowAs(DateTime now)
        {
            var clock = new DateTimeStub();
            clock.TimeTravelTo(now);
            return clock;
        }
        public static DateTimeStub CreateClockWhichSetsNowAs(string time)
        {
            var clock = new DateTimeStub();
            clock.TimeTravelTo(time);
            return clock;
        }

        public DateTime Now() => _now;
        public void TimeTravelTo(TimeSpan value)
        {
            TimeTravelTo(this._now.Add(value));
        }
        public void TimeTravelTo(string dateTime)
        {
            TimeTravelTo(DateTime.Parse(dateTime));
        }
        public void TimeTravelToSomeDateAfter(DateTime dateTime)
        {
            TimeTravelTo(dateTime.AddDays(1));
        }
        public void TimeTravelToSomeDateBefore(DateTime dateTime)
        {
            TimeTravelTo(dateTime.AddDays(-1));
        }
        public void TimeTravelTo(DateTime now)
        {
            this._now = now;
        }
    }
}
