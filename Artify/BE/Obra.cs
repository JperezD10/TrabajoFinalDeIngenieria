namespace BE
{
    public class Obra : EntidadBase
    {
        public string Titulo { get; set; }
        public int Anio { get; set; }
        public string Tecnica { get; set; }

        public bool EsOriginal { get; set; }

        public int ArtistaId { get; set; }
        public Artista Artista { get; set; }

        public decimal PrecioBase { get; set; }
        public decimal PrecioActual { get; set; }

        public string UrlImagen { get; set; }

        public override string FormatoDVH =>
            $"{Id}|{Activo}|{Titulo}|{Anio}|{Tecnica}|{(EsOriginal ? 1 : 0)}|{ArtistaId}|{PrecioBase:F2}|{PrecioActual:F2}|{UrlImagen}";
    }
}
