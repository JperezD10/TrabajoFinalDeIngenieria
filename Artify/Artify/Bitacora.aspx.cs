using BE;
using BE.Observer;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class Bitacora : BasePage
    {
        private List<BE.Bitacora> _lista;
        private readonly BitacoraBLL _bitacoraBLL;

        public Bitacora()
        {
            _bitacoraBLL = new BitacoraBLL();
        }

        // i18n por convención
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "log");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuario = Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (usuario.Rol != RolUsuario.Webmaster)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // primera carga, sin filtro
                CargarDatos(null, null);
                gvBitacora.DataSource = _lista;
                gvBitacora.DataBind();
            }
        }

        private void CargarDatos(DateTime? desde, DateTime? hasta)
        {
            // guardar filtros para paginado
            ViewState["f_desde"] = desde;
            ViewState["f_hasta"] = hasta;

            var resp = _bitacoraBLL.ListarBitacoras(desde, hasta);
            _lista = resp?.Data ?? new List<BE.Bitacora>();
        }

        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBitacora.PageIndex = e.NewPageIndex;

            var desde = ViewState["f_desde"] as DateTime?;
            var hasta = ViewState["f_hasta"] as DateTime?;

            CargarDatos(desde, hasta);
            gvBitacora.DataSource = _lista;
            gvBitacora.DataBind();
        }

        protected void gvBitacora_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbl = (Label)e.Row.FindControl("lblCriticidad");
                if (lbl != null && int.TryParse(lbl.Text, out var nivel))
                {
                    switch (nivel)
                    {
                        case (int)BE.Criticidad.Leve:
                            lbl.CssClass += " badge badge-leve";
                            lbl.Text = "Leve";
                            break;
                        case (int)BE.Criticidad.Moderada:
                            lbl.CssClass += " badge badge-moderada";
                            lbl.Text = "Moderada";
                            break;
                        case (int)BE.Criticidad.Alta:
                            lbl.CssClass += " badge badge-alta";
                            lbl.Text = "Alta";
                            break;
                        case (int)BE.Criticidad.Critica:
                            lbl.CssClass += " badge badge-critica";
                            lbl.Text = "Crítica";
                            break;
                        default:
                            lbl.CssClass += " badge badge-leve";
                            lbl.Text = "Leve";
                            break;
                    }
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            var desde = ParseHtml5Date(txtDesde.Text);
            var hasta = ParseHtml5Date(txtHasta.Text);

            if (desde.HasValue && hasta.HasValue && desde > hasta)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "range",
                    "alert('Rango inválido: \"Desde\" no puede ser mayor que \"Hasta\"');", true);
                return;
            }

            // hacer "hasta" inclusivo (fin del día)
            if (hasta.HasValue) hasta = hasta.Value.Date.AddDays(1).AddTicks(-1);

            gvBitacora.PageIndex = 0;
            CargarDatos(desde, hasta);
            gvBitacora.DataSource = _lista;
            gvBitacora.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDesde.Text = string.Empty;
            txtHasta.Text = string.Empty;

            gvBitacora.PageIndex = 0;
            CargarDatos(null, null);
            gvBitacora.DataSource = _lista;
            gvBitacora.DataBind();
        }

        private DateTime? ParseHtml5Date(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                       DateTimeStyles.AssumeLocal, out var dt))
                return dt;

            if (DateTime.TryParse(value, out dt)) return dt;

            return null;
        }
    }
}