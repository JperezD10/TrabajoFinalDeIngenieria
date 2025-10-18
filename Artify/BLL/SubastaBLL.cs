using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class SubastaBLL
    {
        private readonly SubastaDAL _subastaDal = new SubastaDAL();
        private readonly ObraDAL _obraDal = new ObraDAL();
        private readonly ArtistaDAL _artistaDal = new ArtistaDAL();
        private readonly OfertaDAL _ofertaDal = new OfertaDAL();

        public SubastaDetalleVM GetDetalleVM(int idSubasta)
        {
            var r = ObtenerPorId(idSubasta);
            if (!r.Exito || r.Data == null) return null;
            var s = r.Data;

            var obra = _obraDal.ObtenerPorId(s.IdObra);
            var artista = _artistaDal.ObtenerPorId(obra.ArtistaId);
            var cantOfertas = _ofertaDal.ContarPorSubasta(idSubasta);

            var ahora = DateTime.Now;
            var fechaFin = s.FechaFin ?? (s.FechaInicio?.AddMinutes(s.DuracionMinutos) ?? ahora);
            var estaAbierta = s.Estado == EstadoSubasta.EnCurso && ahora < fechaFin;

            return new SubastaDetalleVM
            {
                Id = s.Id,
                Titulo = obra.Titulo,
                ArtistaNombre = artista.Nombre,
                Anio = obra.Anio,
                Tecnica = obra.Tecnica,
                UrlImagen = obra.UrlImagen,
                Moneda = "USD",
                PrecioBase = s.PrecioInicial,
                PrecioActual = s.PrecioActual,
                IncrementoMin = s.IncrementoMinimo,
                CantidadOfertas = cantOfertas,
                CierraEl = fechaFin,
                EstaAbierta = estaAbierta
            };
        }
        public Response<int> Crear(Subasta s)
        {
            try
            {
                if (s == null) return Response<int>.Error("err.subasta.crear.empty");

                if (s.IdObra <= 0) return Response<int>.Error("err.subasta.obra.requerida");
                if (s.IdCurador <= 0) return Response<int>.Error("err.subasta.curador.requerido");
                if (s.PrecioInicial <= 0) return Response<int>.Error("err.subasta.precioinicial.min");
                if (s.IncrementoMinimo <= 0) return Response<int>.Error("err.subasta.incremento.min");
                if (s.DuracionMinutos <= 0) return Response<int>.Error("err.subasta.duracion.min");

                if (s.FechaProgramadaInicio.HasValue && s.FechaProgramadaInicio.Value < DateTime.Now)
                    return Response<int>.Error("err.subasta.programada.pasado");

                var id = _subastaDal.Crear(s);
                return Response<int>.Success(id, "ok.subasta.creada", id);
            }
            catch (Exception)
            {
                return Response<int>.Error("err.subasta.crear.ex");
            }
        }

        public Response<bool> Empezar(int idSubasta)
        {
            try
            {
                var sub = _subastaDal.ObtenerPorId(idSubasta);
                if (sub == null) return Response<bool>.Error("err.subasta.noexiste", idSubasta);
                if (sub.Estado != EstadoSubasta.Pendiente)
                    return Response<bool>.Error("err.subasta.empezar.estado", sub.Estado.ToString());

                var ok = _subastaDal.Empezar(idSubasta);
                if (!ok) return Response<bool>.Error("err.subasta.empezar.update");
                return Response<bool>.Success(true, "ok.subasta.empezada", idSubasta);
            }
            catch (Exception)
            {
                return Response<bool>.Error("err.subasta.empezar.ex");
            }
        }

        public Response<bool> Finalizar(int idSubasta, bool cierreManual = false)
        {
            try
            {
                var sub = _subastaDal.ObtenerPorId(idSubasta);
                if (sub == null) return Response<bool>.Error("err.subasta.noexiste", idSubasta);
                if (sub.Estado != EstadoSubasta.EnCurso)
                    return Response<bool>.Error("err.subasta.finalizar.estado", sub.Estado.ToString());

                var ok = _subastaDal.Finalizar(idSubasta, cierreManual);
                if (!ok) return Response<bool>.Error("err.subasta.finalizar.update");
                return Response<bool>.Success(true, "ok.subasta.finalizada", idSubasta);
            }
            catch (Exception)
            {
                return Response<bool>.Error("err.subasta.finalizar.ex");
            }
        }

        public Response<bool> Cancelar(int idSubasta)
        {
            try
            {
                var sub = _subastaDal.ObtenerPorId(idSubasta);
                if (sub == null) return Response<bool>.Error("err.subasta.noexiste", idSubasta);
                if (sub.Estado == EstadoSubasta.Finalizada || sub.Estado == EstadoSubasta.Cancelada)
                    return Response<bool>.Error("err.subasta.cancelar.estado", sub.Estado.ToString());

                var ok = _subastaDal.Cancelar(idSubasta);
                if (!ok) return Response<bool>.Error("err.subasta.cancelar.update");
                return Response<bool>.Success(true, "ok.subasta.cancelada", idSubasta);
            }
            catch (Exception)
            {
                return Response<bool>.Error("err.subasta.cancelar.ex");
            }
        }

        public Response<Subasta> ObtenerPorId(int idSubasta)
        {
            try
            {
                var sub = _subastaDal.ObtenerPorId(idSubasta);
                if (sub == null) return Response<Subasta>.Error("err.subasta.noexiste", idSubasta);
                return Response<Subasta>.Success(sub, "ok.subasta.get", idSubasta);
            }
            catch (Exception)
            {
                return Response<Subasta>.Error("err.subasta.get.ex");
            }
        }

        public Response<IEnumerable<Subasta>> ListarPendientesPorCurador(int idCurador)
        {
            try
            {
                if (idCurador <= 0) return Response<IEnumerable<Subasta>>.Error("err.curador.requerido");
                var list = _subastaDal.ListarPendientesPorCurador(idCurador).ToList() ?? new List<Subasta>();
                return Response<IEnumerable<Subasta>>.Success(list, "ok.subasta.list.pendientes", list.Count);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<Subasta>>.Error("err.subasta.list.pendientes.ex");
            }
        }

        public Response<IEnumerable<Subasta>> ListarEnCurso()
        {
            try
            {
                var list = _subastaDal.ListarEnCurso()?.ToList() ?? new List<Subasta>();
                return Response<IEnumerable<Subasta>>.Success(list, "ok.subasta.list.encurso", list.Count);
            }
            catch (Exception)
            {
                return Response<IEnumerable<Subasta>>.Error("err.subasta.list.encurso.ex");
            }
        }

        public List<SubastaHomeVM> GetParaHome(int idUsuario, DateTime ahora)
        {
            var rows = _subastaDal.ListarParaHome(ahora);
            var participacionDal = new ParticipacionSubastaDAL();

            return rows.Select(r =>
            {
                string estado;
                if (ahora < r.FechaInicio) estado = "scheduled";
                else if (ahora <= r.FechaFin) estado = "running";
                else estado = "finished";

                bool puedePujar = false;
                if (idUsuario > 0 && estado == "running")
                    puedePujar = participacionDal.PuedeOfertar(idUsuario, r.Id);

                return new SubastaHomeVM
                {
                    Id = r.Id,
                    TituloObra = r.TituloObra,
                    ArtistaNombre = r.ArtistaNombre,
                    Anio = r.Anio,
                    Tecnica = r.Tecnica,
                    UrlImagen = r.UrlImagen,
                    PrecioInicial = r.PrecioInicial,
                    PrecioActual = r.PrecioActual,
                    FechaInicio = (DateTime)r.FechaInicio,
                    FechaFin = (DateTime)r.FechaFin,
                    EstadoCodigo = estado,
                    PuedePujar = puedePujar,
                    EsOriginal = r.EsOriginal
                };
            })
            .OrderByDescending(x => x.EstadoCodigo == "running")
            .ThenBy(x => x.EstadoCodigo == "scheduled" ? 0 : 1)
            .ThenBy(x => x.FechaInicio)
            .ToList();
        }
    }
}
