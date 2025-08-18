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
    }
}
