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

                var rSub = _subastas.ObtenerPorId(o.IdSubasta);
                if (rSub == null) return Response<int>.Error("err.oferta.subasta.noexiste", o.IdSubasta);

                var subasta = rSub;
                if (subasta.Estado != EstadoSubasta.EnCurso)
                    return Response<int>.Error("err.oferta.subasta.noencurso", subasta.Estado.ToString());

                var minimo = subasta.PrecioActual + subasta.IncrementoMinimo;
                if (o.Monto < minimo)
                    return Response<int>.Error("err.oferta.monto.insuficiente", minimo);


                var id = _dal.Crear(o);
                if (id <= 0) return Response<int>.Error("err.oferta.crear.reglasql");

                return Response<int>.Success(id, "ok.oferta.creada", id);
            }
            catch (Exception)
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
