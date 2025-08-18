using System;
using System.Data.SqlTypes;

namespace BE
{
    public static class DvhTime
    {
        public static DateTime NormalizeForSqlDatetime(DateTime dt)
        {
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return new SqlDateTime(dt).Value;
        }
    }
}
