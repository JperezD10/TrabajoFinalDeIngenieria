using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OfertaDAL : BaseDvvDAL
    {
        public override string TableName => $"Oferta";

        public int Crear(Oferta o)
        {
            o.Fecha = DateTime.Now;   // usa tu normalizador si aplica
            o.Activo = true;

            const string sql = @"
DECLARE @IdOferta INT = 0;

BEGIN TRAN;

IF EXISTS (
    SELECT 1
    FROM Subasta s
    WHERE s.Id = @IdSubasta
      AND s.Estado = @EnCurso
      AND @Monto >= s.PrecioActual + s.IncrementoMinimo
)
BEGIN
    INSERT INTO Oferta (Activo, IdSubasta, IdCliente, Monto, Fecha, DVH)
    VALUES (1, @IdSubasta, @IdCliente, @Monto, @Fecha, 0);

    SET @IdOferta = CAST(SCOPE_IDENTITY() AS INT);

    -- actualizar precio actual de subasta con este monto
    UPDATE Subasta SET PrecioActual = @Monto WHERE Id = @IdSubasta;
END

IF @IdOferta > 0
    COMMIT;
ELSE
    ROLLBACK;

SELECT @IdOferta AS NewId;";

            var pars = new[]
            {
            new SqlParameter("@IdSubasta", o.IdSubasta),
            new SqlParameter("@IdCliente", o.IdCliente),
            new SqlParameter("@Monto", o.Monto),
            new SqlParameter("@Fecha", o.Fecha),
            new SqlParameter("@EnCurso", (byte)EstadoSubasta.EnCurso)
        };

            var t = Acceso.Leer(sql, pars, CommandType.Text);
            int id = Convert.ToInt32(t.Rows[0]["NewId"]);
            if (id <= 0) return 0; // no cumplió reglas

            o.Id = id;
            o.DVH = o.CalcularDVH();

            const string updDvhOferta = "UPDATE Oferta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(updDvhOferta, new[]
            {
            new SqlParameter("@DVH", o.DVH),
            new SqlParameter("@Id", o.Id)
        }, CommandType.Text);

            RecalcularDvhSubasta(o.IdSubasta);

            ActualizarDVV();                // DVV de Oferta
            ActualizarDVV_Subasta();        // DVV de Subasta

            return id;
        }

        public IEnumerable<Oferta> ListarPorSubasta(int idSubasta)
        {
            const string sql = @"
SELECT * FROM Oferta
WHERE Activo = 1 AND IdSubasta = @IdSubasta
ORDER BY Fecha DESC, Id DESC;";

            var t = Acceso.Leer(sql, new[] { new SqlParameter("@IdSubasta", idSubasta) }, CommandType.Text);
            foreach (DataRow r in t.Rows)
                yield return Map(r);
        }

        public Oferta ObtenerPorId(int id)
        {
            const string sql = "SELECT * FROM Oferta WHERE Id = @Id;";
            var t = Acceso.Leer(sql, new[] { new SqlParameter("@Id", id) }, CommandType.Text);
            if (t.Rows.Count == 0) return null;
            return Map(t.Rows[0]);
        }

        public Oferta ObtenerMejorOferta(int idSubasta)
        {
            const string sql = @"
SELECT TOP 1 * FROM Oferta
WHERE Activo = 1 AND IdSubasta = @IdSubasta
ORDER BY Monto DESC, Fecha ASC, Id ASC;";

            var t = Acceso.Leer(sql, new[] { new SqlParameter("@IdSubasta", idSubasta) }, CommandType.Text);
            if (t.Rows.Count == 0) return null;
            return Map(t.Rows[0]);
        }

        public bool Anular(int idOferta)
        {
            const string sql = "UPDATE Oferta SET Activo = 0 WHERE Id = @Id;";
            int rows = Acceso.Escribir(sql, new[] { new SqlParameter("@Id", idOferta) }, CommandType.Text);
            if (rows == 0) return false;

            ActualizarDVV();
            return true;
        }

        private void RecalcularDvhSubasta(int idSubasta)
        {
            const string sqlGet = "SELECT * FROM Subasta WHERE Id = @Id;";
            var t = Acceso.Leer(sqlGet, new[] { new SqlParameter("@Id", idSubasta) }, CommandType.Text);
            if (t.Rows.Count == 0) return;

            var s = MapSubasta(t.Rows[0]);
            s.DVH = s.CalcularDVH();

            const string sqlUpd = "UPDATE Subasta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(sqlUpd, new[]
            {
            new SqlParameter("@DVH", s.DVH),
            new SqlParameter("@Id", s.Id)
        }, CommandType.Text);
        }

        private void ActualizarDVV_Subasta()
        {
            // Reusar mecanismo estándar de tu BaseDvvDAL pero apuntando a "Subasta"
            Acceso.Escribir("sp_ActualizarDVV", new[]
            {
            new SqlParameter("@Tabla", "Subasta")
        }, CommandType.StoredProcedure);
        }

        private static Oferta Map(DataRow row) => new Oferta
        {
            Id = Convert.ToInt32(row["Id"]),
            Activo = Convert.ToBoolean(row["Activo"]),
            IdSubasta = Convert.ToInt32(row["IdSubasta"]),
            IdCliente = Convert.ToInt32(row["IdCliente"]),
            Monto = Convert.ToDecimal(row["Monto"]),
            Fecha = Convert.ToDateTime(row["Fecha"]),
            DVH = Convert.ToInt32(row["DVH"])
        };

        private static Subasta MapSubasta(DataRow row) => new Subasta
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
