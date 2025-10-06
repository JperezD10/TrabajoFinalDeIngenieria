using System;

namespace BE
{
    public sealed class SubastaHomeRow
    {
        public int Id { get; set; }
        public decimal PrecioInicial { get; set; }
        public decimal PrecioActual { get; set; }
        public byte Estado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaProgramadaInicio { get; set; }
        public DateTime? FechaFinEfectiva { get; set; }
        public string TituloObra { get; set; }
        public string ArtistaNombre { get; set; }
        public int Anio { get; set; }
        public string Tecnica { get; set; }
        public string UrlImagen { get; set; }
    }
}
