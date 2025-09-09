using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL : BaseDvvDAL
    {
        public override string TableName => "Bitacora";

        public void RegistrarAccion(Bitacora b)
        {
            // 1) Normalizar fecha como veníamos haciendo (DATETIME)
            b.Fecha = DvhTime.NormalizeForSqlDatetime(b.Fecha);

            // 2) Insertar sin DVH definitivo y recuperar el ID
            const string insertSql = @"
                INSERT INTO Bitacora (Fecha, Accion, Criticidad, Modulo, IdUsuario, DVH)
                VALUES (@Fecha, @Accion, @Criticidad, @Modulo, @IdUsuario, 0);
                SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;";

            var table = Acceso.Leer(insertSql, new[]
            {
                new SqlParameter("@Fecha", b.Fecha),
                new SqlParameter("@Accion", b.Accion),
                new SqlParameter("@Criticidad", b.Criticidad),
                new SqlParameter("@Modulo", b.Modulo),
                new SqlParameter("@IdUsuario", b.IdUsuario),
            });

            if (table.Rows.Count == 0)
                throw new Exception("No se pudo obtener el Id insertado de Bitacora.");

            int newId = Convert.ToInt32(table.Rows[0]["NewId"]);
            b.Id = newId;

            // 3) Recalcular DVH ya con el Id real
            b.DVH = b.CalcularDVH();

            // 4) Actualizar el DVH
            const string updateSql = "UPDATE Bitacora SET DVH=@DVH WHERE Id=@Id;";
            Acceso.Escribir(updateSql, new[]
            {
                new SqlParameter("@DVH", b.DVH),
                new SqlParameter("@Id", b.Id),
            });

            ActualizarDVV();
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

            var tabla = Acceso.Leer(query, new SqlParameter[]
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

        public IEnumerable<Bitacora> GetAllForDVH()
        {
            const string sql = @"
            SELECT * 
            FROM Bitacora
            ORDER BY Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
            {
                var b = new Bitacora();
                b.Id = Convert.ToInt32(row["Id"]);
                b.Fecha = DvhTime.NormalizeForSqlDatetime(Convert.ToDateTime(row["Fecha"]));
                b.Accion = row["Accion"] == DBNull.Value ? null : row["Accion"].ToString();
                b.Criticidad = row["Criticidad"] == DBNull.Value ? 0 : Convert.ToInt32(row["Criticidad"]);
                b.Modulo = row["Modulo"] == DBNull.Value ? null : row["Modulo"].ToString();
                b.IdUsuario = row["IdUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdUsuario"]);
                b.DVH = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt32(row["DVH"]);
                yield return b;
            }
        }
    }
}
