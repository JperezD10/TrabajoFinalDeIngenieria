using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DAL
{
    public class SubastaDAL : BaseDvvDAL
    {
        public override string TableName => "Subasta";

        public int Crear(Subasta s)
        {
            s.Estado = EstadoSubasta.Pendiente;
            s.FechaCreacion = DateTime.Now;
            s.PrecioActual = s.PrecioInicial;

            const string sql = @"
INSERT INTO Subasta
(Activo, IdObra, IdCurador, PrecioInicial, IncrementoMinimo, DuracionMinutos,
 Estado, FechaProgramadaInicio, FechaInicio, FechaFin, FechaCreacion, PrecioActual, IdClienteGanador, DVH)
VALUES
(@Activo, @IdObra, @IdCurador, @PrecioInicial, @IncrementoMinimo, @DuracionMinutos,
 @Estado, @FechaProgramadaInicio, NULL, NULL, @FechaCreacion, @PrecioActual, NULL, 0);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var pars = new[]
            {
            new SqlParameter("@Activo", s.Activo),
            new SqlParameter("@IdObra", s.IdObra),
            new SqlParameter("@IdCurador", s.IdCurador),
            new SqlParameter("@PrecioInicial", s.PrecioInicial),
            new SqlParameter("@IncrementoMinimo", s.IncrementoMinimo),
            new SqlParameter("@DuracionMinutos", s.DuracionMinutos),
            new SqlParameter("@Estado", (byte)s.Estado),
            new SqlParameter("@FechaProgramadaInicio", (object)s.FechaProgramadaInicio ?? DBNull.Value),
            new SqlParameter("@FechaCreacion", s.FechaCreacion),
            new SqlParameter("@PrecioActual", s.PrecioActual),
        };

            var table = Acceso.Leer(sql, pars, CommandType.Text);
            int id = Convert.ToInt32(table.Rows[0][0]);

            s.Id = id;
            s.DVH = s.CalcularDVH();

            const string sqlUpdDvh = "UPDATE Subasta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(sqlUpdDvh, new[]
            {
            new SqlParameter("@DVH", s.DVH),
            new SqlParameter("@Id", s.Id)
        }, CommandType.Text);

            // DVV
            ActualizarDVV();

            return id;
        }

        public void FinalizarSubastasVencidas(DateTime ahora)
        {
            // Estados según tu enum
            const byte EN_CURSO = 2;
            const byte FINALIZADA = 3;

            const string sql = @"
DECLARE @Ahora DATETIME = @pAhora, @EnCurso TINYINT = @pEnCurso, @Finalizada TINYINT = @pFinalizada;

DECLARE @Afectadas TABLE (Id INT);

;WITH UltimaOferta AS (
    SELECT o.IdSubasta,
           o.IdCliente,
           o.Monto,
           ROW_NUMBER() OVER (PARTITION BY o.IdSubasta ORDER BY o.Monto DESC, o.Fecha ASC) AS rn
    FROM Oferta o
)
UPDATE s
   SET s.Estado = @Finalizada,
       s.IdClienteGanador = u.IdCliente,
       s.PrecioActual = ISNULL(u.Monto, s.PrecioInicial)
OUTPUT inserted.Id INTO @Afectadas(Id)
FROM Subasta s
LEFT JOIN UltimaOferta u ON u.IdSubasta = s.Id AND u.rn = 1
WHERE s.Activo = 1
  AND s.Estado = @EnCurso
  AND s.FechaFin <= @Ahora;

SELECT Id FROM @Afectadas;";

            var pars = new[]
            {
        new SqlParameter("@pAhora", ahora),
        new SqlParameter("@pEnCurso", EN_CURSO),
        new SqlParameter("@pFinalizada", FINALIZADA),
    };

            var table = Acceso.Leer(sql, pars, CommandType.Text);
            if (table.Rows.Count == 0)
                return;

            foreach (DataRow r in table.Rows)
            {
                int id = Convert.ToInt32(r["Id"]);

                const string q = @"
SELECT Id, Activo, IdObra, IdCurador, PrecioInicial, IncrementoMinimo, DuracionMinutos,
       Estado, FechaProgramadaInicio, FechaInicio, FechaFin, FechaCreacion, PrecioActual, IdClienteGanador
FROM Subasta WHERE Id = @Id;";

                var t = Acceso.Leer(q, new[] { new SqlParameter("@Id", id) }, CommandType.Text);
                var row = t.Rows[0];

                var s = new Subasta
                {
                    Id = id,
                    Activo = Convert.ToBoolean(row["Activo"]),
                    IdObra = Convert.ToInt32(row["IdObra"]),
                    IdCurador = Convert.ToInt32(row["IdCurador"]),
                    PrecioInicial = Convert.ToDecimal(row["PrecioInicial"]),
                    IncrementoMinimo = Convert.ToDecimal(row["IncrementoMinimo"]),
                    DuracionMinutos = Convert.ToInt32(row["DuracionMinutos"]),
                    Estado = (EstadoSubasta)Convert.ToByte(row["Estado"]),
                    FechaProgramadaInicio = row["FechaProgramadaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaProgramadaInicio"]),
                    FechaInicio = row["FechaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaInicio"]),
                    FechaFin = row["FechaFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaFin"]),
                    FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
                    PrecioActual = Convert.ToDecimal(row["PrecioActual"]),
                    IdClienteGanador = row["IdClienteGanador"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["IdClienteGanador"])
                };

                s.DVH = s.CalcularDVH();

                const string upd = "UPDATE Subasta SET DVH=@DVH WHERE Id=@Id;";
                Acceso.Escribir(upd, new[]
                {
            new SqlParameter("@DVH", s.DVH),
            new SqlParameter("@Id", s.Id)
        }, CommandType.Text);
            }

            // 3) DVV de la tabla
            ActualizarDVV();
        }

        public bool Empezar(int idSubasta)
        {
            DateTime ahora = DateTime.Now;

            const string sql = @"
UPDATE Subasta
   SET Estado = @EnCurso,
       FechaInicio = @Ahora,
       FechaFin = DATEADD(MINUTE, DuracionMinutos, @Ahora)
 WHERE Id = @Id AND Estado = @Pendiente;";

            int rows = Acceso.Escribir(sql, new[]
            {
            new SqlParameter("@EnCurso", EstadoSubasta.EnCurso),
            new SqlParameter("@Pendiente", EstadoSubasta.Pendiente),
            new SqlParameter("@Ahora", ahora),
            new SqlParameter("@Id", idSubasta)
        }, CommandType.Text);

            if (rows == 0) return false;

            RecalcularDvhYActualizarDvv(idSubasta);
            return true;
        }

        public bool Finalizar(int idSubasta, bool cierreManual)
        {
            const string sql = @"
UPDATE Subasta
   SET Estado = @Finalizada
 WHERE Id = @Id AND Estado = @EnCurso
   AND (GETDATE() >= FechaFin OR @CierreManual = 1);";

            int rows = Acceso.Escribir(sql, new[]
            {
            new SqlParameter("@Finalizada", (byte)EstadoSubasta.Finalizada),
            new SqlParameter("@EnCurso", (byte)EstadoSubasta.EnCurso),
            new SqlParameter("@Id", idSubasta),
            new SqlParameter("@CierreManual", cierreManual ? 1 : 0)
        }, CommandType.Text);

            if (rows == 0) return false;

            RecalcularDvhYActualizarDvv(idSubasta);
            return true;
        }

        public bool Cancelar(int idSubasta)
        {
            const string sql = @"UPDATE Subasta SET Estado = @Cancelada WHERE Id = @Id AND Estado IN (@Pendiente, @EnCurso);";
            int rows = Acceso.Escribir(sql, new[]
            {
            new SqlParameter("@Cancelada", (byte)EstadoSubasta.Cancelada),
            new SqlParameter("@Pendiente", (byte)EstadoSubasta.Pendiente),
            new SqlParameter("@EnCurso", (byte)EstadoSubasta.EnCurso),
            new SqlParameter("@Id", idSubasta)
        }, CommandType.Text);

            if (rows == 0) return false;

            RecalcularDvhYActualizarDvv(idSubasta);
            return true;
        }

        public Subasta ObtenerPorId(int id)
        {
            const string sql = "SELECT * FROM Subasta WHERE Id = @Id;";
            var t = Acceso.Leer(sql, new[] { new SqlParameter("@Id", id) }, CommandType.Text);
            if (t.Rows.Count == 0) return null;
            return Map(t.Rows[0]);
        }

        public IEnumerable<Subasta> ListarPendientesPorCurador(int idCurador)
        {
            const string sql = @"
SELECT * FROM Subasta
WHERE Activo = 1 AND IdCurador = @IdCurador AND Estado = @Pendiente
ORDER BY FechaCreacion DESC;";

            var t = Acceso.Leer(sql, new[]
            {
            new SqlParameter("@IdCurador", idCurador),
            new SqlParameter("@Pendiente", EstadoSubasta.Pendiente)
        }, CommandType.Text);

            foreach (DataRow r in t.Rows)
                yield return Map(r);
        }

        public List<SubastaHomeRow> ListarParaHome(DateTime ahora)
        {
            FinalizarSubastasVencidas(ahora);

            const string sql = @"
DECLARE @Ahora DATETIME = @pAhora;
DECLARE @Programada TINYINT = @pProgramada;

SELECT s.Id,
       s.PrecioInicial, s.PrecioActual,
       s.Estado,
       s.FechaInicio,
       s.FechaFin,
       s.FechaProgramadaInicio,
       CASE
         WHEN s.FechaFin IS NOT NULL THEN s.FechaFin
         WHEN s.FechaProgramadaInicio IS NOT NULL THEN DATEADD(MINUTE, s.DuracionMinutos, s.FechaProgramadaInicio)
         ELSE NULL
       END AS FechaFinEfectiva,
       o.Titulo AS TituloObra, o.Anio, o.Tecnica, o.UrlImagen,
       a.Nombre AS ArtistaNombre
FROM Subasta s
JOIN Obra o    ON o.Id = s.IdObra
JOIN Artista a ON a.Id = o.ArtistaId
WHERE s.Activo = 1
  AND (
        (s.FechaInicio IS NOT NULL AND s.FechaFin IS NOT NULL AND @Ahora <= s.FechaFin)
        OR
        (s.Estado = @Programada AND s.FechaProgramadaInicio IS NOT NULL)
      )
ORDER BY
    CASE WHEN s.FechaInicio IS NOT NULL THEN 0 ELSE 1 END,
    ISNULL(s.FechaInicio, s.FechaProgramadaInicio) ASC;";

            var pars = new[]
            {
        new SqlParameter("@pAhora", ahora),
        new SqlParameter("@pProgramada", EstadoSubasta.Pendiente)
    };

            var table = Acceso.Leer(sql, pars, CommandType.Text);

            var list = new List<SubastaHomeRow>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SubastaHomeRow
                {
                    Id = Convert.ToInt32(row["Id"]),
                    PrecioInicial = Convert.ToDecimal(row["PrecioInicial"]),
                    PrecioActual = Convert.ToDecimal(row["PrecioActual"]),
                    Estado = Convert.ToByte(row["Estado"]),
                    FechaInicio = row["FechaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaInicio"]),
                    FechaFin = row["FechaFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaFin"]),
                    FechaProgramadaInicio = row["FechaProgramadaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaProgramadaInicio"]),
                    FechaFinEfectiva = row["FechaFinEfectiva"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaFinEfectiva"]),
                    TituloObra = row["TituloObra"]?.ToString(),
                    ArtistaNombre = row["ArtistaNombre"]?.ToString(),
                    Anio = Convert.ToInt32(row["Anio"]),
                    Tecnica = row["Tecnica"]?.ToString(),
                    UrlImagen = row["UrlImagen"]?.ToString()
                });
            }

            return list;
        }

        public IEnumerable<Subasta> ListarEnCurso()
        {
            const string sql = @"
SELECT * FROM Subasta
WHERE Activo = 1 AND Estado = @EnCurso
ORDER BY FechaInicio DESC;";

            var t = Acceso.Leer(sql, new[]
            {
            new SqlParameter("@EnCurso", (byte)EstadoSubasta.EnCurso)
        }, CommandType.Text);

            foreach (DataRow r in t.Rows)
                yield return Map(r);
        }

        private void RecalcularDvhYActualizarDvv(int id)
        {
            var sub = ObtenerPorId(id);
            if (sub == null) return;

            sub.DVH = sub.CalcularDVH();

            const string sql = "UPDATE Subasta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(sql, new[]
            {
            new SqlParameter("@DVH", sub.DVH),
            new SqlParameter("@Id", id)
        }, CommandType.Text);

            ActualizarDVV();
        }

        private static Subasta Map(DataRow row) => new Subasta
        {
            Id = Convert.ToInt32(row["Id"]),
            Activo = Convert.ToBoolean(row["Activo"]),
            IdObra = Convert.ToInt32(row["IdObra"]),
            IdCurador = Convert.ToInt32(row["IdCurador"]),
            PrecioInicial = Convert.ToDecimal(row["PrecioInicial"]),
            IncrementoMinimo = Convert.ToDecimal(row["IncrementoMinimo"]),
            DuracionMinutos = Convert.ToInt32(row["DuracionMinutos"]),
            Estado = (EstadoSubasta)Convert.ToByte(row["Estado"]),
            FechaProgramadaInicio = row["FechaProgramadaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaProgramadaInicio"]),
            FechaInicio = row["FechaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaInicio"]),
            FechaFin = row["FechaFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaFin"]),
            FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
            PrecioActual = Convert.ToDecimal(row["PrecioActual"]),
            IdClienteGanador = row["IdClienteGanador"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["IdClienteGanador"]),
            DVH = Convert.ToInt32(row["DVH"])
        };
    }
}
