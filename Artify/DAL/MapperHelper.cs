using BE;
using System;
using System.Data;

namespace DAL
{
    public class MapperHelper
    {
        public static Usuario MapUsuario(DataRow row) =>
            new Usuario
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Email = row["Email"].ToString(),
                Password = row["Contraseña"].ToString(),
                Rol = (RolUsuario)Convert.ToInt32(row["Rol"]),
                Bloqueado = Convert.ToBoolean(row["Bloqueado"]),
                IntentosRestantes = Convert.ToInt32(row["IntentosRestantes"]),
                DVH = Convert.ToInt32(row["DVH"]),
            };

        public static Bitacora MapBitacora(DataRow row) =>
            new Bitacora
            {
                Id = Convert.ToInt32(row["Id"]),
                Fecha = Convert.ToDateTime(row["Fecha"]),
                Usuario = row["Usuario"].ToString(),
                Accion = row["Accion"].ToString(),
                Criticidad = Convert.ToInt32(row["Criticidad"]),
                DVH = Convert.ToInt32(row["DVH"]),
            };

        public static Artista MapArtista(DataRow row) =>
            new Artista {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                Nacionalidad = row["Nacionalidad"].ToString(),
                Biografia = row["Biografia"].ToString(),
                Activo = Convert.ToBoolean(row["Activo"]),
                UrlFoto = row["UrlFoto"].ToString(),
                DVH = Convert.ToInt32(row["DVH"]),
            };

        public static Obra MapObraConArtista(DataRow row) =>
    new Obra
    {
        Id = Convert.ToInt32(row["Id"]),
        Titulo = row["Titulo"].ToString(),
        Anio = Convert.ToInt32(row["Anio"]),
        Tecnica = row["Tecnica"].ToString(),
        EsOriginal = Convert.ToBoolean(row["EsOriginal"]),
        ArtistaId = Convert.ToInt32(row["ArtistaId"]),
        Artista = new Artista
        {
            Id = Convert.ToInt32(row["ArtistaId"]),
            Nombre = row.Table.Columns.Contains("ArtistaNombre")
                        ? row["ArtistaNombre"]?.ToString()
                        : null
        },
        PrecioBase = Convert.ToDecimal(row["PrecioBase"]),
        PrecioActual = Convert.ToDecimal(row["PrecioActual"]),
        UrlImagen = row["UrlImagen"].ToString(),
        DVH = Convert.ToInt32(row["DVH"])
    };
    }
}
