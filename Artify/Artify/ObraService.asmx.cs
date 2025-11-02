using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Artify
{
    /// <summary>
    /// Descripción breve de ObraService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ObraService : WebService
    {
        private readonly ObraBLL _obraBLL = new ObraBLL();

        [WebMethod(Description = "Obtiene todas las obras activas disponibles para el curador.")]
        public List<Obra> ObtenerObrasActivas()
        {
            try
            {
                var obras = _obraBLL.ListarObrasActivas();
                return new List<Obra>(obras);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las obras activas: " + ex.Message);
            }
        }
    }
}
