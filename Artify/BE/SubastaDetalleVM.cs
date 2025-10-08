using System;

namespace BE
{
    public class SubastaDetalleVM
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string ArtistaNombre { get; set; }
        public int Anio { get; set; }
        public string Tecnica { get; set; }
        public string UrlImagen { get; set; }
        public string Moneda { get; set; } = "USD";
        public decimal PrecioBase { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal IncrementoMin { get; set; }
        public int CantidadOfertas { get; set; }
        public DateTime CierraEl { get; set; }
        public bool EstaAbierta { get; set; }
    }
}
