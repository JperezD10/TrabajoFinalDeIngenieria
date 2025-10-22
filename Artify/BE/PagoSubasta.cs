using System;
using System.Globalization;

namespace BE
{
    public class PagoSubasta : EntidadBase
    {
        public int IdSubasta { get; set; }
        public int IdCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaPago { get; set; }
        public bool Pagado { get; set; }

        public string TituloObra { get; set; }
        public int Anio { get; set; }
        public DateTime? FechaFinSubasta { get; set; }
        public string UrlImagen { get; set; }
        public string ArtistaNombre { get; set; }

        public override string FormatoDVH
        {
            get
            {
                var f = DvhTime.NormalizeForSqlDatetime(FechaCreacion)
                               .ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                return string.Concat(
                    Id,
                    IdSubasta,
                    IdCliente,
                    Monto.ToString(CultureInfo.InvariantCulture),
                    f,
                    FechaPago?.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) ?? "",
                    Pagado ? 1 : 0
                );
            }
        }
    }
}
