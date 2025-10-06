using System;

namespace BE
{
    public class SubastaHomeVM
    {
        public int Id { get; set; }
        public string TituloObra { get; set; }
        public string ArtistaNombre { get; set; }
        public int Anio { get; set; }
        public string Tecnica { get; set; }
        public string UrlImagen { get; set; }
        public decimal PrecioInicial { get; set; }
        public decimal PrecioActual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoCodigo { get; set; }
        public bool HuboPujas => PrecioActual > PrecioInicial;
        public bool PuedePujar { get; set; }
    }

}
