using System;

namespace BE
{
    public class Artista : EntidadBase
    {
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string UrlFoto { get; set; }
        public string Biografia { get; set; }

        public override string FormatoDVH =>
            $"{Id}|{Activo}|{Nombre}|{Nacionalidad}|{FechaNacimiento:yyyyMMdd}|{UrlFoto}";
    }
}
