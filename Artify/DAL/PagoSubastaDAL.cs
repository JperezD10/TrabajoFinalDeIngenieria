using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PagoSubastaDAL: BaseDvvDAL
    {
        public override string TableName => "PagoSubasta";

        public List<PagoSubasta> ListarPendientes(int idCliente)
        {
            const string sql = @"
SELECT p.Id,
       p.IdSubasta,
       p.IdCliente,
       p.Monto,
       p.FechaCreacion,
       p.FechaPago,
       p.Pagado,
       o.Titulo AS TituloObra,
       o.UrlImagen,
        o.Anio,
       a.Nombre AS ArtistaNombre,
       s.FechaFin AS FechaFinSubasta
FROM PagoSubasta p
JOIN Subasta s ON s.Id = p.IdSubasta
JOIN Obra o ON o.Id = s.IdObra
JOIN Artista a ON a.Id = o.ArtistaId
WHERE p.IdCliente = @IdCliente AND p.Pagado = 0
ORDER BY p.FechaCreacion DESC;";

            var pars = new[] { new SqlParameter("@IdCliente", idCliente) };
            var table = Acceso.Leer(sql, pars, CommandType.Text);

            var list = new List<PagoSubasta>();
            foreach (DataRow r in table.Rows)
            {
                list.Add(new PagoSubasta
                {
                    Id = Convert.ToInt32(r["Id"]),
                    IdSubasta = Convert.ToInt32(r["IdSubasta"]),
                    IdCliente = Convert.ToInt32(r["IdCliente"]),
                    Monto = Convert.ToDecimal(r["Monto"]),
                    FechaCreacion = Convert.ToDateTime(r["FechaCreacion"]),
                    FechaPago = r["FechaPago"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FechaPago"]),
                    Pagado = Convert.ToBoolean(r["Pagado"]),
                    TituloObra = r["TituloObra"]?.ToString(),
                    UrlImagen = r["UrlImagen"]?.ToString(),
                    ArtistaNombre = r["ArtistaNombre"]?.ToString(),
                    Anio = Convert.ToInt32(r["Anio"]),
                    FechaFinSubasta = r["FechaFinSubasta"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FechaFinSubasta"])
                });
            }

            return list;
        }

        public void MarcarComoPagado(int idPago)
        {
            const string sql = @"
UPDATE PagoSubasta
SET Pagado = 1, FechaPago = GETDATE()
WHERE Id = @Id;";

            var pars = new[] { new SqlParameter("@Id", idPago) };
            Acceso.Escribir(sql, pars, CommandType.Text);
        }

        public PagoSubasta ObtenerPorId(int id)
        {
            const string sql = @"
SELECT p.Id,
       p.IdSubasta,
       p.IdCliente,
       p.Monto,
       p.FechaCreacion,
       p.FechaPago,
       p.Pagado,
       o.Titulo AS TituloObra,
       a.Nombre AS ArtistaNombre,
       o.UrlImagen,
       s.FechaFin AS FechaFinSubasta
FROM PagoSubasta p
JOIN Subasta s ON s.Id = p.IdSubasta
JOIN Obra o ON o.Id = s.IdObra
JOIN Artista a ON a.Id = o.ArtistaId
WHERE p.Id = @Id;";

            var table = Acceso.Leer(sql, new[] { new SqlParameter("@Id", id) }, CommandType.Text);
            if (table.Rows.Count == 0)
                return null;

            var r = table.Rows[0];
            return new PagoSubasta
            {
                Id = Convert.ToInt32(r["Id"]),
                IdSubasta = Convert.ToInt32(r["IdSubasta"]),
                IdCliente = Convert.ToInt32(r["IdCliente"]),
                Monto = Convert.ToDecimal(r["Monto"]),
                FechaCreacion = Convert.ToDateTime(r["FechaCreacion"]),
                FechaPago = r["FechaPago"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FechaPago"]),
                Pagado = Convert.ToBoolean(r["Pagado"]),
                TituloObra = r["TituloObra"]?.ToString(),
                UrlImagen = r["UrlImagen"]?.ToString(),
                ArtistaNombre = r["ArtistaNombre"]?.ToString(),
                FechaFinSubasta = r["FechaFinSubasta"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FechaFinSubasta"])
            };
        }
    }
}
