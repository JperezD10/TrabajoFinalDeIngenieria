using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Artify
{
    /// <summary>
    /// Descripción breve de BitacoraService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class BitacoraService : System.Web.Services.WebService
    {

        [WebMethod(Description = "Exporta la bitácora actual a un archivo XML en el servidor.")]
        public string ExportarBitacoraXml(List<BE.Bitacora> registros)
        {
            try
            {
                if (registros == null || registros.Count == 0)
                    return "No se recibieron registros para exportar.";

                var dt = new DataTable("Bitacora");
                dt.Columns.Add("Fecha", typeof(string));
                dt.Columns.Add("Accion", typeof(string));
                dt.Columns.Add("Criticidad", typeof(int));
                dt.Columns.Add("Modulo", typeof(string));
                dt.Columns.Add("Usuario", typeof(string));
                dt.Columns.Add("IdUsuario", typeof(int));

                foreach (var r in registros)
                {
                    dt.Rows.Add(
                        r.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                        r.Accion,
                        r.Criticidad,
                        r.Modulo,
                        r.Usuario,
                        r.IdUsuario
                    );
                }

                string folderPath = @"C:\SqlBackups\Artify";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"Bitacora_Page_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string filePath = Path.Combine(folderPath, fileName);

                dt.WriteXml(filePath, XmlWriteMode.WriteSchema);
                return $"Archivo XML generado correctamente en: {filePath}";
            }
            catch (Exception ex)
            {
                return $"Error al generar XML: {ex.Message}";
            }
        }
    }
}
