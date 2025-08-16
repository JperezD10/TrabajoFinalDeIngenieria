using BE;
using DAL;
using System;
using System.Collections.Generic;

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
                _bitacoraDAL.RegistrarAccion(bitacora);
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
