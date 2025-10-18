using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ParticipacionSubastaDAL : BaseDvvDAL
    {
        public override string TableName => "ParticipacionSubasta";

        public IEnumerable<ParticipacionSubasta> GetAllForDVH()
        {
            const string sql = @"
            SELECT * 
            FROM ParticipacionSubasta
            ORDER BY Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
            {
                var p = MapperHelper.MapParticipacionSubasta(row);
                yield return p;
            }
        }

        public bool GuardarParticipacion(ParticipacionSubasta participacion)
        {
            participacion.FechaPago = DateTime.Now;
            participacion.Monto = 5m;

            const string insertSql = @"
                INSERT INTO ParticipacionSubasta (IdSubasta, IdCliente, Monto, FechaPago, DVH)
                VALUES (@IdSubasta, @IdCliente, @Monto, @FechaPago, 0);
                SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;";

            var table = Acceso.Leer(insertSql, new[]
            {
                new SqlParameter("@IdSubasta", participacion.IdSubasta),
                new SqlParameter("@IdCliente", participacion.IdCliente),
                new SqlParameter("@Monto", participacion.Monto),
                new SqlParameter("@FechaPago", participacion.FechaPago)
            });

            if (table.Rows.Count == 0)
                throw new Exception("No se pudo obtener el Id insertado de ParticipacionSubasta.");

            int newId = Convert.ToInt32(table.Rows[0]["NewId"]);
            participacion.Id = newId;

            // 2) Recalcular DVH ya con el Id real
            var dvh = participacion.CalcularDVH();
            participacion.DVH = dvh;

            // 3) Actualizar el DVH en la tabla
            const string updateSql = "UPDATE ParticipacionSubasta SET DVH=@DVH WHERE Id=@Id;";
            Acceso.Escribir(updateSql, new[]
            {
                new SqlParameter("@DVH", dvh),
                new SqlParameter("@Id", newId)
            });

            // 4) Recalcular DVV general
            ActualizarDVV();

            return true;
        }

        public bool PuedeOfertar(int idCliente, int idSubasta)
        {
            var parametros = new[]
            {
            new SqlParameter("@IdCliente", idCliente),
            new SqlParameter("@IdSubasta", idSubasta)
            };

            var tabla = Acceso.Leer(
                "SELECT 1 FROM ParticipacionSubasta WHERE IdCliente = @IdCliente AND IdSubasta = @IdSubasta",
                parametros,
                CommandType.Text
            );

            return tabla.Rows.Count > 0;
        }
    }
}
