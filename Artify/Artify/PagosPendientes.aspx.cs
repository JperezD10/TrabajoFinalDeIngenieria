using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class PagosPendientes : BasePage
    {
        private readonly PagoSubastaBLL _bll = new PagoSubastaBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "paypend");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPagosPendientes();
            }
        }

        protected void rptPagos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            var lnkPagar = (HyperLink)e.Item.FindControl("lnkPagar");
            var litBtnPagar = (Literal)e.Item.FindControl("litBtnPagar");

            litBtnPagar.Text = T("paypend.btnPay");

            lnkPagar.ToolTip = T("paypend.btnPay");
        }
        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }

        private void CargarPagosPendientes()
        {
            try
            {
                var usuario = (Usuario)Session["Usuario"];
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                var resp = _bll.ListarPendientes(usuario.Id);

                if (!resp.Exito || resp.Data == null || !resp.Data.Any())
                {
                    phEmpty.Visible = true;
                    rptPagos.Visible = false;
                    if (!resp.Exito)
                        litEmpty.Text = resp.Mensaje;
                    return;
                }

                rptPagos.Visible = true;
                phEmpty.Visible = false;

                rptPagos.DataSource = resp.Data.Select(p => new
                {
                    IdPago = p.Id,
                    TituloObra = p.TituloObra,
                    ArtistaNombre = p.ArtistaNombre,
                    FechaFinSubasta = p.FechaFinSubasta,
                    UrlImagen = p.UrlImagen,
                    Anio = p.Anio,
                    Monto = p.Monto
                });

                rptPagos.DataBind();
            }
            catch (Exception ex)
            {
                phEmpty.Visible = true;
                rptPagos.Visible = false;
                litEmpty.Text = "Error al cargar los pagos pendientes: " + ex.Message;
            }
        }
    }
}