using System;

namespace BE
{
    public class Oferta : EntidadBase
    {
        public int IdSubasta { get; set; }
        public int IdCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public override string FormatoDVH => $"{Id}{Activo}{IdSubasta}{IdCliente}{Monto}{Fecha}";
    }
}
