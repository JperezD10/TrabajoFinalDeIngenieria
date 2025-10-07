using BE;
using DAL;
using System;

namespace BLL
{
    public class SuscripcionBLL
    {
        private readonly SuscripcionDAL _dal = new SuscripcionDAL();

        public Response<Suscripcion> ObtenerActiva(int idUsuario)
        {
            try
            {
                var sus = _dal.ObtenerActiva(idUsuario, DateTime.Now);

                if (sus == null)
                    return Response<Suscripcion>.Error("err.suscripcion.inactiva");

                return Response<Suscripcion>.Success(sus);
            }
            catch (Exception ex)
            {
                return Response<Suscripcion>.Error("err.suscripcion.obtener", ex.Message);
            }
        }

        public Response<Suscripcion> ObtenerUltima(int idUsuario)
        {
            try
            {
                var sus = _dal.ObtenerUltima(idUsuario);
                if (sus == null)
                    return Response<Suscripcion>.Error("err.suscripcion.noexiste");
                return Response<Suscripcion>.Success(sus);
            }
            catch (Exception ex)
            {
                return Response<Suscripcion>.Error("err.suscripcion.obtener", ex.Message);
            }
        }

        public Response<Suscripcion> ActivarMensual(int idUsuario)
            => Activar(idUsuario, TimeSpan.FromDays(30));

        public Response<Suscripcion> ActivarAnual(int idUsuario)
            => Activar(idUsuario, TimeSpan.FromDays(365));

        private Response<Suscripcion> Activar(int idUsuario, TimeSpan duracion)
        {
            try
            {
                var ahora = DateTime.Now;
                var vigente = _dal.ObtenerActiva(idUsuario, ahora);

                DateTime inicio, fin;

                if (vigente != null)
                {
                    inicio = vigente.FechaFin;
                    fin = vigente.FechaFin.Add(duracion);
                }
                else
                {
                    inicio = ahora;
                    fin = ahora.Add(duracion);
                }

                if (_dal.ExisteSolapada(idUsuario, inicio, fin))
                    return Response<Suscripcion>.Error("err.suscripcion.solapada");

                var s = new Suscripcion
                {
                    Activo = true,
                    IdUsuario = idUsuario,
                    FechaInicio = inicio,
                    FechaFin = fin
                };

                var id = _dal.Crear(s);
                s.Id = id;

                return Response<Suscripcion>.Success(s, "ok.suscripcion.creada", fin);
            }
            catch (Exception ex)
            {
                return Response<Suscripcion>.Error("err.suscripcion.crear", ex.Message);
            }
        }

        public Response<bool> PuedePujar(int idUsuario)
        {
            try
            {
                var sus = _dal.ObtenerActiva(idUsuario, DateTime.Now);
                bool puede = sus != null;
                return Response<bool>.Success(puede);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error("err.suscripcion.validar", ex.Message);
            }
        }
    }
}
