using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ArtistaDAL : BaseDvvDAL
    {
        public override string TableName => "Artista";

        public IEnumerable<Artista> GetAllForDVH()
        {
            const string sql = @"
            SELECT * 
            FROM Artista
            ORDER BY Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
            {
                var u = MapperHelper.MapArtista(row);
                yield return u;
            }
        }

        public Artista Agregar(Artista artista)
        {
            artista.DVH = artista.CalcularDVH();
            const string insertSql = @"
                INSERT INTO Artista (Nombre, Nacionalidad, FechaNacimiento, UrlFoto, Biografia, Activo, DVH) 
                VALUES (@Nombre, @Nacionalidad, @FechaNacimiento, @UrlFoto, @Biografia, 1, 0)
                SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;";

            var table = Acceso.Leer(insertSql, new SqlParameter[]
            {
                new SqlParameter("@Nombre", artista.Nombre),
                new SqlParameter("@Nacionalidad", artista.Nacionalidad),
                new SqlParameter("@FechaNacimiento", (object)artista.FechaNacimiento ?? DBNull.Value),
                new SqlParameter("@UrlFoto", (object)artista.UrlFoto ?? DBNull.Value),
                new SqlParameter("@Biografia", (object)artista.Biografia ?? DBNull.Value),
                new SqlParameter("@DVH", artista.DVH)
            });

            if (table.Rows.Count == 0)
                throw new Exception("No se pudo obtener el Id insertado de Artista.");

            int newId = Convert.ToInt32(table.Rows[0]["NewId"]);
            artista.Id = newId;

            // 3) Recalcular DVH ya con el Id real
            artista.DVH = artista.CalcularDVH();

            // 4) Actualizar el DVH
            const string updateSql = "UPDATE Artista SET DVH=@DVH WHERE Id=@Id;";
            Acceso.Escribir(updateSql, new[]
            {
                new SqlParameter("@DVH", artista.DVH),
                new SqlParameter("@Id", artista.Id),
            });

            ActualizarDVV();

            return artista;
        }

        public Artista ObtenerPorId(int id)
        {
            const string sql = "SELECT * FROM Artista WHERE Id = @Id AND Activo = 1;";
            var t = Acceso.Leer(sql, new[] { new SqlParameter("@Id", id) }, CommandType.Text);
            if (t.Rows.Count == 0) return null;
            return MapperHelper.MapArtista(t.Rows[0]);
        }
    }
}
