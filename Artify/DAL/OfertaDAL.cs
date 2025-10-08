using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OfertaDAL : BaseDvvDAL
    {
        SubastaDAL subastaDal = new SubastaDAL();
        public override string TableName => $"Oferta";

        public int Crear(Oferta o)
        {
            // normalizo mínimos y seteo por defecto
            o.Activo = true;
            if (o.Fecha == default) o.Fecha = DateTime.Now;

            // 1) Actualizar PrecioActual de la Subasta
            const string updSubastaSql = @"
            UPDATE Subasta
               SET PrecioActual = @Monto
             WHERE Id = @IdSubasta AND Activo = 1;";

            Acceso.Escribir(
                updSubastaSql,
                new[]
                {
                new SqlParameter("@Monto", o.Monto),
                new SqlParameter("@IdSubasta", o.IdSubasta)
                },
                CommandType.Text
            );

            var subasta = subastaDal.ObtenerPorId(o.IdSubasta);
            subasta.DVH = subasta.CalcularDVH();
            const string updSubastaDvhSql = @"UPDATE Subasta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(
                updSubastaDvhSql,
                new[]
                {
                new SqlParameter("@DVH", subasta.DVH),
                new SqlParameter("@Id",  subasta.Id)
                },
                CommandType.Text
            );

            const string insOfertaSql = @"
            INSERT INTO Oferta (Activo, IdSubasta, IdCliente, Monto, Fecha, DVH)
            VALUES (@Activo, @IdSubasta, @IdCliente, @Monto, @Fecha, 0);
            SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;";

            var t = Acceso.Leer(
                insOfertaSql,
                new[]
                {
                new SqlParameter("@Activo",  o.Activo),
                new SqlParameter("@IdSubasta", o.IdSubasta),
                new SqlParameter("@IdCliente", o.IdCliente),
                new SqlParameter("@Monto",   o.Monto),
                new SqlParameter("@Fecha",   o.Fecha)
                },
                CommandType.Text
            );

            var newId = Convert.ToInt32(t.Rows[0]["NewId"]);
            o.Id = newId;

            // 3) Calcular DVH de la fila y actualizar
            var dvh = o.CalcularDVH();
            const string updDvhSql = @"UPDATE Oferta SET DVH = @DVH WHERE Id = @Id;";
            Acceso.Escribir(
                updDvhSql,
                new[]
                {
                new SqlParameter("@DVH", dvh),
                new SqlParameter("@Id",  newId)
                },
                CommandType.Text
            );

            // 4) Recalcular DVV de la tabla Oferta
            ActualizarDVV();
            ActualizarDVV_Subasta();

            return newId;
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

        private void ActualizarDVV_Subasta()
        {
            Acceso.Escribir("sp_ActualizarDVV", new[]
            {
                new SqlParameter("@Tabla", "Subasta")
            }, CommandType.StoredProcedure);
        }

        public int ContarPorSubasta(int idSubasta)
        {
            const string sql = "SELECT COUNT(1) FROM Oferta WHERE Activo = 1 AND IdSubasta = @Id;";
            var t = Acceso.Leer(sql, new[] { new SqlParameter("@Id", idSubasta) }, CommandType.Text);
            return Convert.ToInt32(t.Rows[0][0]);
        }

        private static Oferta Map(DataRow row) => new Oferta
        {
            Id = Convert.ToInt32(row["Id"]),
            Activo = Convert.ToBoolean(row["Activo"]),
            IdSubasta = Convert.ToInt32(row["Id"]),
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
