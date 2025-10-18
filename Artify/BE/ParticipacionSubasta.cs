using System;
using System.Globalization;

namespace BE
{
    public class ParticipacionSubasta : EntidadBase
    {
        public int IdSubasta { get; set; }
        public int IdCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public override string FormatoDVH
        {
            get
            {
                var f = DvhTime.NormalizeForSqlDatetime(FechaPago)
                               .ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                return string.Concat(Id, IdSubasta, IdCliente,
                    Monto.ToString("F2", CultureInfo.InvariantCulture), f);
            }
        }
    }
}
