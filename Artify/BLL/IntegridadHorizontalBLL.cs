using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{

    public sealed class IntegridadHorizontalBLL
    {
        private readonly Dictionary<string, Func<IEnumerable<EntidadBase>>> _fetchers;

        public IntegridadHorizontalBLL()
        {
            _fetchers = new Dictionary<string, Func<IEnumerable<EntidadBase>>>(StringComparer.OrdinalIgnoreCase);
            RegistrarTablasPorDefecto();
        }

        public IntegridadHorizontalBLL(Dictionary<string, Func<IEnumerable<EntidadBase>>> fetchers)
        {
            _fetchers = fetchers ?? new Dictionary<string, Func<IEnumerable<EntidadBase>>>(StringComparer.OrdinalIgnoreCase);
        }

        public IntegridadHorizontalBLL RegistrarTabla(string nombreTabla, Func<IEnumerable<EntidadBase>> fetchAll)
        {
            _fetchers[nombreTabla] = fetchAll;
            return this;
        }

        public Response<List<TablaCheckResult>> VerificarTodo()
        {
            var resultados = new List<TablaCheckResult>();

            try
            {
                foreach (var kvp in _fetchers)
                {
                    var tabla = kvp.Key;
                    var fetch = kvp.Value;

                    var res = new TablaCheckResult { Tabla = tabla };

                    try
                    {
                        var registros = fetch();
                        foreach (var reg in registros)
                        {
                            res.TotalRegistros++;
                            var esperado = reg.CalcularDVH();
                            if (!reg.ValidarDVH(esperado))
                                res.IdsCorruptos.Add(reg.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Error = tabla + ": error al leer/verificar -> " + ex.Message;
                    }

                    resultados.Add(res);
                }

                return Response<List<TablaCheckResult>>.Success(resultados);
            }
            catch (Exception ex)
            {
                return Response<List<TablaCheckResult>>.Error("Fallo general de verificación: " + ex.Message);
            }
        }

        private void RegistrarTablasPorDefecto()
        {
            var usuarioDal = new UsuarioDAL();
            var bitacoraDal = new BitacoraDAL();
            var artistaDal = new ArtistaDAL();
            var obrasDal = new ObraDAL();
            var SuscripcionDAL = new SuscripcionDAL();
            var SubastaDAL = new SubastaDAL();

            _fetchers["Usuario"] = delegate { return usuarioDal.GetAllForDVH().Cast<EntidadBase>(); };
            _fetchers["Bitacora"] = delegate { return bitacoraDal.GetAllForDVH().Cast<EntidadBase>(); };
            _fetchers["Artista"] = delegate { return artistaDal.GetAllForDVH().Cast<EntidadBase>();};
            _fetchers["Obra"] = delegate { return obrasDal.GetAllForDVH().Cast<EntidadBase>(); };
            _fetchers["Suscripcion"] = delegate { return SuscripcionDAL.GetAllForDVH().Cast<EntidadBase>(); };
            _fetchers["Subasta"] = delegate { return SubastaDAL.GetAllForDVH().Cast<EntidadBase>(); };


            // Mañana agregás más:
            // var obraDal = new ObraDAL();
            // _fetchers["Obra"] = delegate { return obraDal.GetAllForDVH().Cast<EntidadBase>(); };
            // etc...
        }
    }
}
