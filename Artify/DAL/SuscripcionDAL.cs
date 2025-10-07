using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SuscripcionDAL : BaseDvvDAL
    {
        public override string TableName => "Suscripcion";

        public IEnumerable<Suscripcion> GetAllForDVH()
        {
            const string sql = @"
    SELECT  *
    FROM    Suscripcion o
    ORDER BY o.Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
                yield return MapperHelper.MapSuscripcion(row);
        }
        public bool TieneActiva(int idUsuario, DateTime ahora)
        {
            if (idUsuario <= 0) return false;

            const string sql = @"
SELECT TOP 1 1
FROM Suscripcion
WHERE IdUsuario = @IdUsuario
  AND @Ahora BETWEEN FechaInicio AND FechaFin;";

            var pars = new[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Ahora", ahora)
            };

            var table = Acceso.Leer(sql, pars, CommandType.Text);
            return table.Rows.Count > 0;
        }

        public Suscripcion ObtenerActiva(int idUsuario, DateTime ahora)
        {
            if (idUsuario <= 0) return null;

            const string sql = @"
SELECT TOP 1 *
FROM Suscripcion
WHERE IdUsuario = @IdUsuario
  AND Activo = 1
  AND @Ahora BETWEEN FechaInicio AND FechaFin
ORDER BY FechaFin DESC;";

            var pars = new[]
            {
        new SqlParameter("@IdUsuario", idUsuario),
        new SqlParameter("@Ahora", ahora)
    };

            var tabla = Acceso.Leer(sql, pars, CommandType.Text);
            if (tabla.Rows.Count == 0) return null;

            return MapperHelper.MapSuscripcion(tabla.Rows[0]);
        }
        public Suscripcion ObtenerUltima(int idUsuario)
        {
            const string sql = @"
SELECT TOP 1 * FROM Suscripcion
WHERE IdUsuario = @IdUsuario AND Activo = 1
ORDER BY FechaFin DESC;";
            var t = Acceso.Leer(sql, new[] { new SqlParameter("@IdUsuario", idUsuario) }, CommandType.Text);
            return t.Rows.Count == 0 ? null : MapperHelper.MapSuscripcion(t.Rows[0]);
        }
        public bool ExisteSolapada(int idUsuario, DateTime ini, DateTime fin)
        {
            const string sql = @"
SELECT TOP 1 1
FROM Suscripcion
WHERE IdUsuario=@IdUsuario AND Activo=1
  AND NOT (@Fin <= FechaInicio OR @Ini >= FechaFin);";
            var t = Acceso.Leer(sql, new[]
            {
        new SqlParameter("@IdUsuario", idUsuario),
        new SqlParameter("@Ini", ini),
        new SqlParameter("@Fin", fin)
    }, CommandType.Text);
            return t.Rows.Count > 0;
        }

        public int Crear(Suscripcion s)
        {
            s.FechaInicio = s.FechaInicio == default ? DateTime.Now : s.FechaInicio;
            if (s.FechaFin == default)
                s.FechaFin = s.FechaInicio.AddMonths(1);

            const string sql = @"
INSERT INTO Suscripcion (Activo, IdUsuario, FechaInicio, FechaFin, DVH)
VALUES (@Activo, @IdUsuario, @FechaInicio, @FechaFin, 0);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var pars = new[]
            {
                new SqlParameter("@Activo", s.Activo),
                new SqlParameter("@IdUsuario", s.IdUsuario),
                new SqlParameter("@FechaInicio", s.FechaInicio),
                new SqlParameter("@FechaFin", s.FechaFin)
            };

            var table = Acceso.Leer(sql, pars, CommandType.Text);
            int id = Convert.ToInt32(table.Rows[0][0]);
            s.Id = id;

            s.DVH = s.CalcularDVH();

            const string sqlUpdDvh = "UPDATE Suscripcion SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(sqlUpdDvh, new[]
            {
                new SqlParameter("@DVH", s.DVH),
                new SqlParameter("@Id", s.Id)
            }, CommandType.Text);

            ActualizarDVV();
            return id;
        }
    }
}
