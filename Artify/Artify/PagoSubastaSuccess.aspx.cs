using BE;
using BLL;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class PagoSubastaSuccess : BasePage
    {
        private readonly PagoSubastaBLL _pagoSubastaBLL = new PagoSubastaBLL();
        private readonly SubastaBLL _subastaBLL = new SubastaBLL();
        private readonly BitacoraBLL _bitacoraBLL = new BitacoraBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "pagosubsucc");
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

            ProcesarPago(idPago);
        }

        private void ProcesarPago(int idPago)
        {
            if (IsPostBack) return;

            var usuario = Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var extRef = Request.QueryString["external_reference"];
            int idPagoUrl;

            if (!int.TryParse(extRef, out idPagoUrl))
            {
                int.TryParse(Request.QueryString["id"], out idPagoUrl);
            }

            if (idPagoUrl <= 0)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            var paymentId = Request.QueryString["payment_id"];

            try
            {
                if (!string.IsNullOrEmpty(paymentId))
                {
                    MercadoPagoConfig.AccessToken = "APP_USR-8649679236800274-101722-5287d67dadff0ac3d94ed00c73ed3156-2932397436";
                    var paymentClient = new PaymentClient();
                    var payment = paymentClient.Get(long.Parse(paymentId));

                    if (payment.Status != "approved")
                    {
                        litMensaje.Text = $"<span style='color:#f87171'>El pago no fue aprobado ({payment.Status}).</span>";
                        return;
                    }
                }

                var respPago = _pagoSubastaBLL.ObtenerPorId(idPagoUrl);
                if (!respPago.Exito || respPago.Data == null)
                {
                    litMensaje.Text = "No se encontró el pago especificado.";
                    return;
                }

                var pago = respPago.Data;
                pago.Pagado = true;
                pago.FechaPago = DateTime.Now;
                pago.DVH = pago.CalcularDVH();

                var resUpd = _pagoSubastaBLL.MarcarComoPagado(pago.Id);
                if (!resUpd.Exito)
                {
                    litMensaje.Text = "El pago fue procesado, pero no se pudo actualizar la base.";
                }

                var detalle = _subastaBLL.GetDetalleVM(pago.IdSubasta);
                if (detalle != null)
                {
                    litObra.Text = $"Obra: {detalle.Titulo}";
                }

                _bitacoraBLL.RegistrarAccion(new BE.Bitacora
                {
                    IdUsuario = usuario.Id,
                    Fecha = DateTime.Now,
                    Accion = $"Pago subasta #{pago.IdSubasta} por ${pago.Monto}",
                    Criticidad = (int)Criticidad.Moderada,
                    Modulo = "Pagos"
                });

                litMensaje.Text = "Tu pago fue procesado correctamente.";
                litDetalle.Text = "En breve recibirás la confirmación y el artista será notificado.";
            }
            catch (Exception ex)
            {
                litMensaje.Text = $"<span style='color:#f87171'>Error al procesar el pago: {ex.Message}</span>";
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomeCliente.aspx");
        }
    }
}