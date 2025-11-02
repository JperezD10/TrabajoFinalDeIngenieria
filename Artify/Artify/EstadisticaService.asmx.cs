using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Artify
{
    /// <summary>
    /// Descripción breve de EstadisticaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class EstadisticaService : System.Web.Services.WebService
    {
        private readonly PagoSubastaBLL _pagoSubastaBLL = new PagoSubastaBLL();

        [WebMethod(Description = "Calcula el promedio de facturación entre dos fechas a partir de los pagos aprobados.")]
        public decimal CalcularPromedioFacturacion(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var pagos = _pagoSubastaBLL.ListarPagos();

                var pagosFiltrados = pagos
                    .Where(p => p.Pagado && p.FechaPago.HasValue &&
                                p.FechaPago.Value >= fechaInicio &&
                                p.FechaPago.Value <= fechaFin)
                    .ToList();

                if (!pagosFiltrados.Any())
                    return 0;

                return pagosFiltrados.Average(p => p.Monto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular promedio: " + ex.Message);
            }
        }
    }
}
