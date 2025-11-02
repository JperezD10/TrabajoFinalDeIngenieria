using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Artify
{
    public partial class VerObras : BasePage
    {
        ObraBLL obraBLL = new ObraBLL();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "verobras");
            SecurityManager.CheckAccess(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Context.Items["priceLabel"] = IdiomaManager.Instance.T("verobras.litPriceLabel");

            var data = obraBLL.ListarObrasActivas();
            BindObras(data);

            ExportarObrasPorArtistaXml(data);
        }
        private void ExportarObrasPorArtistaXml(IEnumerable<Obra> obras)
        {
            try
            {
                if (obras == null || !obras.Any())
                    return;

                var xml = new XDocument(
                    new XElement("Obras",
                        obras.Select(o =>
                            new XElement("Obra",
                                new XAttribute("Id", o.Id),
                                new XAttribute("Titulo", o.Titulo),
                                new XAttribute("Anio", o.Anio),
                                new XAttribute("ArtistaId", o.ArtistaId),
                                new XAttribute("Activo", o.Activo)
                            )
                        )
                    )
                );

                string folderPath = @"C:\SqlBackups\Artify";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"ObrasPorArtista_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string filePath = Path.Combine(folderPath, fileName);

                xml.Save(filePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al exportar XML de obras: " + ex.Message);
            }
        }
        private void BindObras(IEnumerable<Obra> obras)
        {
            var list = obras?.ToList() ?? new List<Obra>();
            rptObras.DataSource = list;
            rptObras.DataBind();
            pnlEmpty.Visible = list.Count == 0;
        }
    }
}