using BE;
using BE.Observer;
using BLL;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class FeeParticipacionSuccess : BasePage
    {
        private readonly ParticipacionSubastaBLL _participacionBll = new ParticipacionSubastaBLL();
        private readonly BitacoraBLL _bitacoraBll = new BitacoraBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "feepartsuccess");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var usuario = Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Preferí el external_reference (la posta que devuelve MP)
            var extRef = Request.QueryString["external_reference"];
            int idSubasta;
            if (!int.TryParse(extRef, out idSubasta))
            {
                // fallback por si querés seguir mostrando ?id=...
                int.TryParse(Request.QueryString["id"], out idSubasta);
            }
            if (idSubasta <= 0)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            var paymentId = Request.QueryString["payment_id"];

            try
            {
                // (Opcional) Validá estado del pago en MP
                if (!string.IsNullOrEmpty(paymentId))
                {
                    var mp = new PaymentClient();
                    var payment = mp.Get(long.Parse(paymentId));
                    if (payment.Status != "approved")
                    {
                        msg.InnerHtml = $"<span class='err'>{T("feepartsuccess.msg.errEstado")} ({payment.Status})</span>";
                        return;
                    }
                }

                var res = _participacionBll.GuardarParticipacion(new ParticipacionSubasta
                {
                    IdCliente = usuario.Id,
                    IdSubasta = idSubasta
                });

                if (res.Exito)
                {
                    _bitacoraBll.RegistrarAccion(new BE.Bitacora
                    {
                        IdUsuario = usuario.Id,
                        Fecha = DateTime.Now,
                        Accion = $"Pago fee participación Subasta #{idSubasta}",
                        Criticidad = 1,
                        Modulo = "Subasta"
                    });

                    msg.InnerHtml = $"<span class='ok'>{T("feepartsuccess.msg.ok")}</span>";
                }
                else
                {
                    msg.InnerHtml = $"<span class='err'>{T("feepartsuccess.msg.err")}</span>";
                }
            }
            catch (Exception ex)
            {
                msg.InnerHtml = $"<span class='err'>{T("feepartsuccess.msg.ex")} ({ex.Message})</span>";
            }

            lnkVolver.Text = T("feepartsuccess.btnBack");
            lnkVolver.NavigateUrl = $"~/SubastaDetalle.aspx?id={idSubasta}";
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0)
                ? string.Format(raw, args)
                : raw;
        }
    }
}