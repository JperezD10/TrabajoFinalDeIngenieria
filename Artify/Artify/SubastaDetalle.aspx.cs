using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class SubastaDetalle : BasePage
    {
        SubastaBLL SubastaBLL = new SubastaBLL();
        OfertaBLL OfertaBLL = new OfertaBLL();
        SuscripcionBLL SuscripcionBLL = new SuscripcionBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "subdet");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            lnkBack.Text = T("subdet.btn.back");
            lnkBack.NavigateUrl = "~/HomeCliente.aspx";

            var usuario = Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Login.aspx?returnUrl={returnUrl}");
                return;
            }

            if (!int.TryParse(Request.QueryString["id"], out var id) || id <= 0)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            hfSubastaId.Value = id.ToString();
            CargarDetalle(id);
        }

        private void CargarDetalle(int idSubasta)
        {
            var vm = SubastaBLL.GetDetalleVM(idSubasta);
            if (vm == null)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            imgObra.ImageUrl = vm.UrlImagen;
            litTitulo.Text = vm.Titulo;
            litArtista.Text = vm.ArtistaNombre;
            litAnio.Text = vm.Anio.ToString();
            litTecnica.Text = vm.Tecnica;

            litPrecioBase.Text = Formatear(vm.Moneda, vm.PrecioBase);
            litOfertasCount.Text = vm.CantidadOfertas.ToString();
            litCierraFecha.Text = vm.CierraEl.ToString("dd/MM/yyyy HH:mm");

            var precioActualShown = vm.CantidadOfertas == 0
                ? vm.PrecioBase
                : vm.PrecioActual;
            litPrecioActual.Text = Formatear(vm.Moneda, precioActualShown);

            litEstado.Text = vm.EstaAbierta ? T("subdet.litEstado.open") : T("subdet.litEstado.closed");
            litHintIncremento.Text = string.Format(T("subdet.litHintIncremento.text"), vm.IncrementoMin.ToString("N2"));

            hfEndsAtIso.Value = vm.CierraEl.ToString("yyyy-MM-ddTHH:mm:ssK");

            var usuario = (Usuario)Session["Usuario"];
            var tieneSuscripcion = SuscripcionBLL.PuedePujar(usuario.Id).Data;
            pnlPuja.Visible = vm.EstaAbierta && tieneSuscripcion;
            pnlCerrada.Visible = !pnlPuja.Visible;

            if (!vm.EstaAbierta)
                litCerradaMsg.Text = T("subdet.litCerradaMsg.closed");
            else if (!tieneSuscripcion)
                litCerradaMsg.Text = T("subdet.litCerradaMsg.needSub");
        }

        protected void valMin_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            var vm = SubastaBLL.GetDetalleVM(int.Parse(hfSubastaId.Value));
            if (vm == null) { args.IsValid = false; return; }

            if (!decimal.TryParse(args.Value.Replace(',', '.'),
                                  NumberStyles.Number, CultureInfo.InvariantCulture, out var monto))
            {
                args.IsValid = false;
                (source as System.Web.UI.WebControls.CustomValidator).ErrorMessage = T("subdet.validation.invalidNumber");
                return;
            }

            var minimo = vm.CantidadOfertas == 0
                ? vm.PrecioBase
                : vm.PrecioActual + vm.IncrementoMin;

            args.IsValid = monto >= minimo;
            if (!args.IsValid)
                (source as System.Web.UI.WebControls.CustomValidator).ErrorMessage =
                    string.Format(T("subdet.validation.minBid"), Formatear(vm.Moneda, minimo));
        }

        protected void btnPujar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (!decimal.TryParse(txtMonto.Text.Replace(',', '.'),
                                  NumberStyles.Number, CultureInfo.InvariantCulture, out var monto))
            {
                lblMensaje.Text = "<span class='val-msg'>" + T("subdet.validation.invalidNumber") + "</span>";
                return;
            }

            var id = int.Parse(hfSubastaId.Value);
            var usuario = (Usuario)Session["Usuario"];
            var oferta = new Oferta
            {
                IdSubasta = id,
                IdCliente = usuario.Id,
                Monto = monto,
                Fecha = DateTime.Now,
                Activo = true
            };

            var res = OfertaBLL.Crear(oferta);
            if (!res.Exito)
            {
                lblMensaje.Text = "<span class='val-msg'>" + T(res.Mensaje) + "</span>";
                return;
            }

            lblMensaje.Text = "<span class='ok-msg'>" + T("ok.oferta.creada", res.Data) + "</span>";
            txtMonto.Text = string.Empty;
            CargarDetalle(id);
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }

        private string Formatear(string moneda, decimal valor)
            => (moneda ?? "USD").ToUpper() + " " + valor.ToString("N2");
    }
}