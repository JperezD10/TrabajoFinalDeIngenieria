using System;
using System.Globalization;

namespace BE
{
    public class Oferta : EntidadBase
    {
        public int IdSubasta { get; set; }
        public int IdCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public override string FormatoDVH
        {
            get
            {
                return string.Concat(
                    Id,
                    Activo,
                    IdSubasta,
                    IdCliente,
                    Monto.ToString("F2", CultureInfo.InvariantCulture),
                    Fecha.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                );
            }
        }
    }
}
