using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BitacoraBLL
    {
        private readonly BitacoraDAL _bitacoraDAL;
        public BitacoraBLL()
        {
            _bitacoraDAL = new BitacoraDAL();
        }
        public Response<bool> RegistrarAccion(Bitacora bitacora)
        {
            try
            {
                var integridadH = new IntegridadHorizontalBLL();
                var respH = integridadH.VerificarTodo();

                bool hayCorrupcionH = !respH.Exito || respH.Data.Any(r =>
                    !string.IsNullOrEmpty(r.Error) ||
                    (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0));

                var integridadV = new IntegridadVerticalBLL();
                var dvvCorruptas = integridadV.ObtenerVerticalesCorruptos();

                bool hayCorrupcionV = dvvCorruptas != null && dvvCorruptas.Count > 0;

                bool baseCorrupta = hayCorrupcionH || hayCorrupcionV;

                _bitacoraDAL.RegistrarAccion(bitacora, baseCorrupta);
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error($"Error al registrar la acción: {ex.Message}");
            }
        }

        public Response<List<Bitacora>> ListarBitacoras(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                var bitacoras = _bitacoraDAL.TraerBitacora(fechaDesde, fechaHasta);
                return Response<List<Bitacora>>.Success(bitacoras);
            }
            catch (Exception ex)
            {
                return Response<List<Bitacora>>.Error($"Error al registrar la acción: {ex.Message}");
            }
        }
    }
}
