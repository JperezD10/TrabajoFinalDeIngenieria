using System;

namespace BLL
{
    public static class DvhTime
    {
        public static DateTime NormalizeForSqlDatetime(DateTime dt)
        {
            long step = TimeSpan.TicksPerSecond / 300;
            DateTime u = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            long snapped = ((u.Ticks + (step / 2)) / step) * step;
            return new DateTime(snapped, DateTimeKind.Unspecified);
        }
    }
}
