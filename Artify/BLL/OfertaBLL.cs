using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class OfertaBLL
    {
        private readonly OfertaDAL _dal;
        private readonly SubastaDAL _subastas;
        private readonly SuscripcionDAL _suscripcionDal = new SuscripcionDAL();
        private readonly OfertaDAL _ofertaDal = new OfertaDAL();

        public OfertaBLL()
        {
            _dal = new OfertaDAL();
            _subastas = new SubastaDAL();
        }

        public OfertaBLL(OfertaDAL ofertaDal, SubastaDAL subastaDal)
        {
            _dal = ofertaDal ?? new OfertaDAL();
            _subastas = subastaDal ?? new SubastaDAL();
        }

        public Response<int> Crear(Oferta o)
        {
            try
            {
                if (o == null) return Response<int>.Error("err.oferta.empty");
                if (o.IdSubasta <= 0) return Response<int>.Error("err.oferta.subasta.requerida");
                if (o.IdCliente <= 0) return Response<int>.Error("err.oferta.cliente.requerido");
                if (o.Monto <= 0) return Response<int>.Error("err.oferta.monto.min");

                var sub = _subastas.ObtenerPorId(o.IdSubasta);
                if (sub == null) return Response<int>.Error("err.oferta.subasta.noexiste", o.IdSubasta);

                var ahora = DateTime.Now;
                var fechaFin = sub.FechaFin ?? (sub.FechaInicio?.AddMinutes(sub.DuracionMinutos) ?? ahora);
                if (!sub.Activo || sub.Estado != EstadoSubasta.EnCurso)
                    return Response<int>.Error("err.oferta.subasta.noencurso", sub.Estado.ToString());
                if (ahora >= fechaFin) return Response<int>.Error("err.oferta.subasta.finalizada");

                // Suscripción activa
                if (!_suscripcionDal.TieneActiva(o.IdCliente, ahora))
                    return Response<int>.Error("err.oferta.suscripcion.inactiva");

                // Reglas de monto (primera vs siguientes)
                var cantOfertas = _ofertaDal.ContarPorSubasta(o.IdSubasta);
                if (cantOfertas == 0)
                {
                    if (o.Monto < sub.PrecioInicial) return Response<int>.Error("err.oferta.monto.base", sub.PrecioInicial);
                }
                else
                {
                    var precioActual = sub.PrecioActual;
                    var minimo = precioActual + sub.IncrementoMinimo;
                    if (o.Monto < minimo) return Response<int>.Error("err.oferta.monto.insuficiente", minimo);
                }

                o.Fecha = ahora; 
                var id = _ofertaDal.Crear(o); 
                if (id <= 0) return Response<int>.Error("err.oferta.crear.reglasql");

                return Response<int>.Success(id, "ok.oferta.creada", id);
            }
            catch
            {
                return Response<int>.Error("err.oferta.crear.ex");
            }
        }

        public Response<IEnumerable<Oferta>> ListarPorSubasta(int idSubasta)
        {
            try
            {
                if (idSubasta <= 0) return Response<IEnumerable<Oferta>>.Error("err.oferta.subasta.requerida");

                var list = _dal.ListarPorSubasta(idSubasta)?.ToList() ?? new List<Oferta>();
                return Response<IEnumerable<Oferta>>.Success(list, "ok.oferta.list", list.Count);
            }
            catch (Exception)
            {
                return Response<IEnumerable<Oferta>>.Error("err.oferta.list.ex");
            }
        }

        public Response<Oferta> ObtenerPorId(int idOferta)
        {
            try
            {
                if (idOferta <= 0) return Response<Oferta>.Error("err.oferta.id.requerido");

                var o = _dal.ObtenerPorId(idOferta);
                if (o == null) return Response<Oferta>.Error("err.oferta.noexiste", idOferta);

                return Response<Oferta>.Success(o, "ok.oferta.get", idOferta);
            }
            catch (Exception)
            {
                return Response<Oferta>.Error("err.oferta.get.ex");
            }
        }

        public Response<Oferta> ObtenerMejorOferta(int idSubasta)
        {
            try
            {
                if (idSubasta <= 0) return Response<Oferta>.Error("err.oferta.subasta.requerida");

                var o = _dal.ObtenerMejorOferta(idSubasta);
                if (o == null) return Response<Oferta>.Error("err.oferta.mejor.vacia", idSubasta);

                return Response<Oferta>.Success(o, "ok.oferta.mejor", o.Monto);
            }
            catch (Exception)
            {
                return Response<Oferta>.Error("err.oferta.mejor.ex");
            }
        }

        public Response<bool> Anular(int idOferta)
        {
            try
            {
                if (idOferta <= 0) return Response<bool>.Error("err.oferta.id.requerido");

                var ok = _dal.Anular(idOferta);
                if (!ok) return Response<bool>.Error("err.oferta.anular.update");

                return Response<bool>.Success(true, "ok.oferta.anulada", idOferta);
            }
            catch (Exception)
            {
                return Response<bool>.Error("err.oferta.anular.ex");
            }
        }
    }
}
