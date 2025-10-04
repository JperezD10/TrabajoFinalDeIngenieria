using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsuarioPermisoDAL : BaseDvvDAL
    {
        public override string TableName => "UsuarioPermiso";
        public List<string> ListarPorUsuario(int idUsuario)
        {
            const string sql = @"
                SELECT Ruta
                FROM UsuarioPermiso
                WHERE IdUsuario = @IdUsuario
                ORDER BY Ruta;";

            var parametros = new[] { new SqlParameter("@IdUsuario", idUsuario) };
            DataTable t = Acceso.Leer(sql, parametros);

            var list = new List<string>();
            foreach (DataRow r in t.Rows)
                list.Add(Convert.ToString(r["Ruta"]));
            return list;
        }
        public void ReemplazarPorUsuario(int idUsuario, IEnumerable<string> nuevasRutas)
        {
            // Normalizar/únicas
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (nuevasRutas != null)
            {
                foreach (var ruta in nuevasRutas)
                    if (!string.IsNullOrWhiteSpace(ruta)) set.Add(ruta.Trim());
            }

            try
            {
                // 1) Borrar actuales
                const string del = "DELETE FROM UsuarioPermiso WHERE IdUsuario = @IdUsuario;";
                Acceso.Escribir(del, new[] { new SqlParameter("@IdUsuario", idUsuario) }, CommandType.Text);

                // 2) Insertar nuevas calculando DVH desde la ENTIDAD
                if (set.Count > 0)
                {
                    const string ins = @"INSERT INTO UsuarioPermiso (IdUsuario, Ruta, DVH)
                                         VALUES (@IdUsuario, @Ruta, @DVH);";

                    foreach (string ruta in set)
                    {
                        var up = new UsuarioPermiso { IdUsuario = idUsuario, Ruta = ruta };
                        up.DVH = up.CalcularDVH();

                        var p = new[]
                        {
                            new SqlParameter("@IdUsuario", up.IdUsuario),
                            new SqlParameter("@Ruta", up.Ruta),
                            new SqlParameter("@DVH", up.DVH)
                        };
                        Acceso.Escribir(ins, p, CommandType.Text);
                    }
                }

                // 3) Recalcular DVV
                ActualizarDVV();
            }
            catch
            {
                throw;
            }
        }
        public void SincronizarPorUsuario(int idUsuario, IEnumerable<string> nuevasRutas)
        {
            var nuevoSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (nuevasRutas != null)
            {
                foreach (var r in nuevasRutas)
                    if (!string.IsNullOrWhiteSpace(r)) nuevoSet.Add(r.Trim());
            }

            var actualSet = new HashSet<string>(ListarPorUsuario(idUsuario), StringComparer.OrdinalIgnoreCase);

            // a eliminar: actuales - nuevos
            var aEliminar = new List<string>();
            foreach (var r in actualSet) if (!nuevoSet.Contains(r)) aEliminar.Add(r);

            // a insertar: nuevos - actuales
            var aInsertar = new List<string>();
            foreach (var r in nuevoSet) if (!actualSet.Contains(r)) aInsertar.Add(r);

            // Aplicar bajas
            foreach (var ruta in aEliminar)
            {
                const string del = "DELETE FROM UsuarioPermiso WHERE IdUsuario=@IdUsuario AND Ruta=@Ruta;";
                Acceso.Escribir(del, new[]
                {
                    new SqlParameter("@IdUsuario", idUsuario),
                    new SqlParameter("@Ruta", ruta)
                }, CommandType.Text);
            }

            // Aplicar altas con DVH desde la ENTIDAD
            foreach (var ruta in aInsertar)
            {
                var up = new UsuarioPermiso { IdUsuario = idUsuario, Ruta = ruta };
                up.DVH = up.CalcularDVH();

                const string ins = @"INSERT INTO UsuarioPermiso (IdUsuario, Ruta, DVH)
                                     VALUES (@IdUsuario, @Ruta, @DVH);";
                Acceso.Escribir(ins, new[]
                {
                    new SqlParameter("@IdUsuario", up.IdUsuario),
                    new SqlParameter("@Ruta", up.Ruta),
                    new SqlParameter("@DVH", up.DVH)
                }, CommandType.Text);
            }

            ActualizarDVV();
        }

        public void Agregar(int idUsuario, string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta)) return;
            ruta = ruta.Trim();

            // Si ya existe, nada
            const string exists = "SELECT 1 FROM UsuarioPermiso WHERE IdUsuario=@IdUsuario AND Ruta=@Ruta;";
            var p = new[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Ruta", ruta)
            };
            DataTable t = Acceso.Leer(exists, p);
            if (t.Rows.Count > 0) return;

            // Insert con DVH de la ENTIDAD
            var up = new UsuarioPermiso { IdUsuario = idUsuario, Ruta = ruta };
            up.DVH = up.CalcularDVH();

            const string ins = "INSERT INTO UsuarioPermiso (IdUsuario, Ruta, DVH) VALUES (@IdUsuario, @Ruta, @DVH);";
            Acceso.Escribir(ins, new[]
            {
                new SqlParameter("@IdUsuario", up.IdUsuario),
                new SqlParameter("@Ruta", up.Ruta),
                new SqlParameter("@DVH", up.DVH)
            }, CommandType.Text);

            ActualizarDVV();
        }
        public void Quitar(int idUsuario, string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta)) return;
            ruta = ruta.Trim();

            const string del = "DELETE FROM UsuarioPermiso WHERE IdUsuario=@IdUsuario AND Ruta=@Ruta;";
            var p = new[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Ruta", ruta)
            };
            Acceso.Escribir(del, p, CommandType.Text);

            ActualizarDVV();
        }
    }
}
