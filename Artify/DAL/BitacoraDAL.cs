using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL
    {
        Acceso acceso = Acceso.GetInstance;

        public void RegistrarAccion(Bitacora bitacora)
        {
            bitacora.DVH = bitacora.CalcularDVH();

            string query = @"
        INSERT INTO Bitacora
        (Fecha, Accion, Criticidad, Modulo, IdUsuario, DVH)
        VALUES
        (@Fecha, @Accion, @Criticidad, @Modulo, @IdUsuario, @DVH)";

            acceso.Escribir(query, new SqlParameter[]
            {
                new SqlParameter("@Fecha", bitacora.Fecha),
                new SqlParameter("@Accion", bitacora.Accion),
                new SqlParameter("@Criticidad", bitacora.Criticidad),
                new SqlParameter("@Modulo", bitacora.Modulo),
                new SqlParameter("@IdUsuario", bitacora.IdUsuario),
                new SqlParameter("@DVH", bitacora.DVH)
            });
        }

        public List<Bitacora> TraerBitacora(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            string query = @"
        SELECT 
            b.Id,
            b.Fecha,
            b.Accion,
            b.Modulo,
            b.Criticidad,
            b.IdUsuario,
            b.DVH,
            u.Email AS Usuario
        FROM Bitacora b
        LEFT JOIN Usuario u ON u.Id = b.IdUsuario
        WHERE (@FechaDesde IS NULL OR b.Fecha >= @FechaDesde)
          AND (@FechaHasta IS NULL OR b.Fecha <= @FechaHasta)
        ORDER BY b.Fecha DESC;";

            var tabla = acceso.Leer(query, new SqlParameter[]
            {
                new SqlParameter("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                new SqlParameter("@FechaHasta", (object)fechaHasta ?? DBNull.Value)
            });

            List<Bitacora> bitacoras = new List<Bitacora>();
            foreach (DataRow row in tabla.Rows)
            {
                Bitacora bitacora = MapperHelper.MapBitacora(row);
                bitacora.Usuario = row["Usuario"].ToString();
                bitacoras.Add(bitacora);
            }
            return bitacoras;
        }
    }
}
