using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class UsuarioPermisoBLL
    {
        private readonly UsuarioPermisoDAL _dal = new UsuarioPermisoDAL();

        public Response<List<string>> GetExtras(int idUsuario)
        {
            try
            {
                var data = _dal.ListarPorUsuario(idUsuario);
                return Response<List<string>>.Success(data);
            }
            catch (Exception)
            {
                return Response<List<string>>.Error("ERR_PERM_001: No se pudieron obtener los permisos adicionales.");
            }
        }

        public Response<bool> Replace(int idUsuario, IEnumerable<string> rutas)
        {
            try
            {
                var normalizadas = NormalizarRutas(rutas);
                _dal.ReemplazarPorUsuario(idUsuario, normalizadas);
                return Response<bool>.Success(true);
            }
            catch (Exception)
            {
                return Response<bool>.Error("ERR_PERM_002: No se pudieron guardar los permisos adicionales.");
            }
        }

        /// <summary>
        /// Sincroniza DIFERENCIALMENTE los permisos (inserta solo las nuevas y borra solo las removidas).
        /// Toca menos filas que Replace. Usa DVH por entidad en los INSERT.
        /// </summary>
        public Response<bool> Sync(int idUsuario, IEnumerable<string> rutas)
        {
            try
            {
                var normalizadas = NormalizarRutas(rutas);
                _dal.SincronizarPorUsuario(idUsuario, normalizadas);
                return Response<bool>.Success(true);
            }
            catch (Exception)
            {
                return Response<bool>.Error("ERR_PERM_003: No se pudieron sincronizar los permisos adicionales.");
            }
        }

        public Response<bool> Add(int idUsuario, string ruta)
        {
            try
            {
                var r = NormalizarRuta(ruta);
                if (r == null) return Response<bool>.Success(true); // nada que hacer
                _dal.Agregar(idUsuario, r);
                return Response<bool>.Success(true);
            }
            catch (Exception)
            {
                return Response<bool>.Error("ERR_PERM_004: No se pudo agregar el permiso.");
            }
        }

        public Response<bool> Remove(int idUsuario, string ruta)
        {
            try
            {
                var r = NormalizarRuta(ruta);
                if (r == null) return Response<bool>.Success(true); // nada que hacer
                _dal.Quitar(idUsuario, r);
                return Response<bool>.Success(true);
            }
            catch (Exception)
            {
                return Response<bool>.Error("ERR_PERM_005: No se pudo quitar el permiso.");
            }
        }


        private static IEnumerable<string> NormalizarRutas(IEnumerable<string> rutas)
        {
            if (rutas == null) return new List<string>();

            // Normaliza (trim) y quita vacíos/duplicados (case-insensitive)
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in rutas)
            {
                if (string.IsNullOrWhiteSpace(r)) continue;
                var t = r.Trim();
                if (t.Length == 0) continue;
                set.Add(t);
            }
            return set.ToList();
        }

        private static string NormalizarRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta)) return null;
            var t = ruta.Trim();
            return t.Length == 0 ? null : t;
        }
    }
}
