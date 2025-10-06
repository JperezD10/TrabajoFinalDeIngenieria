using System;

namespace BE
{
    public class Subasta : EntidadBase
    {
        public int IdObra { get; set; }
        public int IdCurador { get; set; }

        // Configuración básica
        public decimal PrecioInicial { get; set; }
        public decimal IncrementoMinimo { get; set; }  // p.ej. 100
        public int DuracionMinutos { get; set; }       // p.ej. 60

        public EstadoSubasta Estado { get; set; }
        public DateTime? FechaProgramadaInicio { get; set; } // opcional
        public DateTime? FechaInicio { get; set; }           // se setea al "Empezar"
        public DateTime? FechaFin { get; set; }              // FechaInicio + Duracion
        public DateTime FechaCreacion { get; set; }

        // Dinámica
        public decimal PrecioActual { get; set; }    // arranca en PrecioInicial
        public int? IdClienteGanador { get; set; }
        public override string FormatoDVH => $"{Id}{Activo}{IdObra}{IdCurador}{PrecioInicial}{IncrementoMinimo}" +
            $"{DuracionMinutos}{Estado}{FechaProgramadaInicio}{FechaInicio}{FechaFin}{FechaCreacion}{PrecioActual}{IdClienteGanador}";
    }

    public enum EstadoSubasta
    {
        Pendiente = 0,   // creada, aún no comenzó
        EnCurso = 1,   // ya comenzó
        Finalizada = 2,   // terminó por tiempo o cierre manual
        Cancelada = 3    // anulada por el curador/webmaster
    }
}
