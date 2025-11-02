using BE;
using BLL;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class PagoSubastaCliente : BasePage
    {
        private readonly PagoSubastaBLL _pagoSubastaBLL = new PagoSubastaBLL();
        private readonly SubastaBLL _subastaBLL = new SubastaBLL();

        private int IdPago
        {
            get => ViewState["IdPago"] != null ? (int)ViewState["IdPago"] : 0;
            set => ViewState["IdPago"] = value;
        }

        private int IdSubasta
        {
            get => ViewState["IdSubasta"] != null ? (int)ViewState["IdSubasta"] : 0;
            set => ViewState["IdSubasta"] = value;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "pagosubcli");
            SecurityManager.CheckAccess(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["id"], out int idPago))
            {
                Response.Redirect("HomeCliente.aspx");
                return;
            }

            CargarPago(idPago);
        }

        private void CargarPago(int idPago)
        {
            var respPago = _pagoSubastaBLL.ObtenerPorId(idPago);
            if (!respPago.Exito || respPago.Data == null)
            {
                Response.Redirect("HomeCliente.aspx");
                return;
            }

            var pago = respPago.Data;
            IdPago = pago.Id;
            IdSubasta = pago.IdSubasta;

            var detalle = _subastaBLL.GetDetalleVM(pago.IdSubasta);
            if (detalle == null)
            {
                Response.Redirect("HomeCliente.aspx");
                return;
            }

            litObraTitulo.Text = detalle.Titulo;
            litObraArtista.Text = detalle.ArtistaNombre;
            litObraMonto.Text = $"US$ {pago.Monto:N2}";
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            var respPago = _pagoSubastaBLL.ObtenerPorId(IdPago);
            if (!respPago.Exito || respPago.Data == null)
            {
                Response.Redirect("HomeCliente.aspx");
                return;
            }

            var pago = respPago.Data;
            var detalle = _subastaBLL.GetDetalleVM(IdSubasta);
            if (detalle == null)
            {
                Response.Redirect("HomeCliente.aspx");
                return;
            }

            try
            {
                MercadoPagoConfig.AccessToken = "APP_USR-8649679236800274-101722-5287d67dadff0ac3d94ed00c73ed3156-2932397436";

                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                    {
                        new PreferenceItemRequest
                        {
                            Title = detalle.Titulo,
                            Quantity = 1,
                            CurrencyId = "USD",
                            UnitPrice = pago.Monto
                        }
                    },
                    Payer = new PreferencePayerRequest
                    {
                        Email = "test_user_653940919069735644@testuser.com"
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/PagoSubastaSuccess.aspx?id={pago.Id}",
                        Failure = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/PagoSubastaFail.aspx",
                        Pending = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/PagoSubastaPending.aspx"
                    },
                    AutoReturn = "approved",
                    ExternalReference = pago.Id.ToString()
                };

                var client = new PreferenceClient();
                var preference = client.Create(request);

                Response.Redirect(preference.InitPoint);
            }
            catch (Exception ex)
            {
                litInfoFinal.Text = $"<span style='color:#f87171'>Error al iniciar el pago: {ex.Message}</span>";
            }
        }
    }
}