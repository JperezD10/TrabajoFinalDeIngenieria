using System;

namespace BE
{
    public class Suscripcion : EntidadBase
    {
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }  

        public bool EstaActivaEn(DateTime ahora) =>
            FechaInicio <= ahora && ahora <= FechaFin;

        public override string FormatoDVH =>
            $"{Id}|{Activo}|{IdUsuario}|{FechaInicio:yyyy-MM-dd HH:mm:ss}|{FechaFin:yyyy-MM-dd HH:mm:ss}";
    }
}
