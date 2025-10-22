using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PagoSubastaBLL
    {
        private readonly PagoSubastaDAL _dal = new PagoSubastaDAL();

        public Response<List<PagoSubasta>> ListarPendientes(int idCliente)
        {
            try
            {
                var list = _dal.ListarPendientes(idCliente);
                return Response<List<PagoSubasta>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<PagoSubasta>>.Error($"Error al listar pagos pendientes: {ex.Message}");
            }
        }

        public Response<PagoSubasta> ObtenerPorId(int id)
        {
            try
            {
                var pago = _dal.ObtenerPorId(id);
                if (pago == null)
                    return Response<PagoSubasta>.Error("Pago no encontrado.");

                return Response<PagoSubasta>.Success(pago);
            }
            catch (Exception ex)
            {
                return Response<PagoSubasta>.Error($"Error al obtener el pago: {ex.Message}");
            }
        }

        public Response<bool> MarcarComoPagado(int idPago)
        {
            try
            {
                _dal.MarcarComoPagado(idPago);
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error($"Error al marcar el pago como completado: {ex.Message}");
            }
        }
    }
}
