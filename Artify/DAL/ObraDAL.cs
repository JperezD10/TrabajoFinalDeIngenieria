using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ObraDAL : BaseDvvDAL
    {
        public override string TableName => "Obra";

        public IEnumerable<Obra> GetAllForDVH()
        {
            const string sql = @"
    SELECT  o.Id, o.Titulo, o.Anio, o.Tecnica, o.EsOriginal,
            o.ArtistaId, a.Nombre AS ArtistaNombre, o.Activo,
            o.PrecioBase, o.PrecioActual, o.UrlImagen, o.DVH
    FROM    Obra o
    LEFT JOIN Artista a ON a.Id = o.ArtistaId
    ORDER BY o.Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
                yield return MapperHelper.MapObraConArtista(row);
        }

        public Obra Agregar(Obra obra)
        {
            obra.DVH = obra.CalcularDVH();
            const string insertSql = @"
        INSERT INTO Obra
            (Titulo, Anio, Tecnica, EsOriginal, ArtistaId, PrecioBase, PrecioActual, UrlImagen, DVH)
        VALUES
            (@Titulo, @Anio, @Tecnica, @EsOriginal, @ArtistaId, @PrecioBase, @PrecioActual, @UrlImagen, 0);
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;";

            var table = Acceso.Leer(insertSql, new SqlParameter[]
            {
                new SqlParameter("@Titulo", obra.Titulo),
                new SqlParameter("@Anio", obra.Anio),
                new SqlParameter("@Tecnica", obra.Tecnica),
                new SqlParameter("@EsOriginal", obra.EsOriginal),
                new SqlParameter("@ArtistaId", obra.ArtistaId),
                new SqlParameter("@PrecioBase", obra.PrecioBase),
                new SqlParameter("@PrecioActual", obra.PrecioActual),
                new SqlParameter("@UrlImagen", obra.UrlImagen ?? (object)DBNull.Value)
            });

            if (table.Rows.Count == 0)
                throw new Exception("No se pudo obtener el Id insertado de obra.");

            int newId = Convert.ToInt32(table.Rows[0]["NewId"]);
            obra.Id = newId;

            // 3) Recalcular DVH ya con el Id real
            obra.DVH = obra.CalcularDVH();

            // 4) Actualizar el DVH
            const string updateSql = "UPDATE obra SET DVH=@DVH WHERE Id=@Id;";
            Acceso.Escribir(updateSql, new[]
            {
                new SqlParameter("@DVH", obra.DVH),
                new SqlParameter("@Id", obra.Id),
            });

            ActualizarDVV();

            return obra;
        }

        public Obra ObtenerPorId(int id)
        {
            const string sql = @"
        SELECT  o.Id, o.Titulo, o.Anio, o.Tecnica, o.EsOriginal,
                o.ArtistaId, a.Nombre AS ArtistaNombre, o.Activo,
                o.PrecioBase, o.PrecioActual, o.UrlImagen, o.DVH
        FROM    Obra o
        LEFT JOIN Artista a ON a.Id = o.ArtistaId
        WHERE o.Id = @Id;
";
            var tabla = Acceso.Leer(sql, new[]{
                new SqlParameter("@Id", id)} , CommandType.Text);
            if (tabla.Rows.Count == 0)
                return null;
            return MapperHelper.MapObraConArtista(tabla.Rows[0]);
        }


        public IEnumerable<Obra> ListarObrasParacurador()
        {
            const string sql = @"
SELECT  o.Id, o.Titulo, o.Anio, o.Tecnica, o.EsOriginal, o.Activo,
        o.ArtistaId, o.PrecioBase, o.PrecioActual, o.UrlImagen, o.DVH
FROM    Obra o
JOIN    Artista a ON a.Id = o.ArtistaId
ORDER BY o.Titulo;";

            var tabla = Acceso.Leer(sql, null, CommandType.Text);

            foreach (DataRow row in tabla.Rows)
                yield return MapperHelper.MapObraConArtista(row);
        }
    }
}
