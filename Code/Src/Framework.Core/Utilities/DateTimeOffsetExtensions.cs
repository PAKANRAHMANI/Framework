using System;
using System.Linq;

namespace Framework.Core.Utilities
{
    public static class DateTimeOffsetExtensions
    {
        public static byte[] GetBytes(this DateTimeOffset dateTimeOffset)
        {
            return BitConverter.GetBytes(dateTimeOffset.UtcTicks).Concat(BitConverter.GetBytes((short)dateTimeOffset.Offset.Minutes)).ToArray();
        }
    }
}
