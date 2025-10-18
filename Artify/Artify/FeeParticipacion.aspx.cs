using Antlr.Runtime;
using BE;
using BE.Observer;
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
    public partial class FeeParticipacion : BasePage
    {
        private readonly ParticipacionSubastaBLL _bll = new ParticipacionSubastaBLL();
        private readonly SubastaBLL _subastaBll = new SubastaBLL();
        private int _idSubasta;
        private Usuario _usuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            MercadoPagoConfig.AccessToken = "APP_USR-8649679236800274-101722-5287d67dadff0ac3d94ed00c73ed3156-2932397436";
            _usuario = SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "feepart");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            _usuario = Session["Usuario"] as Usuario;
            if (_usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["id"], out _idSubasta) || _idSubasta <= 0)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            var vm = _subastaBll.GetDetalleVM(_idSubasta);
            if (vm == null)
            {
                Response.Redirect("~/HomeCliente.aspx");
                return;
            }

            litSubastaName.Text = vm.Titulo;

            var yaPago = _bll.PuedeOfertar(_usuario.Id, _idSubasta);
            if (yaPago.Data)
            {
                phInfo.Visible = true;
                litInfo.Text = T("feepart.info.already", "You already paid the participation fee for this auction.");
                btnConfirm.Visible = false;
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            _usuario = Session["Usuario"] as Usuario;
            if (_usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["id"], out _idSubasta)) return;

            try
            {
                var vm = _subastaBll.GetDetalleVM(_idSubasta);
                if (vm == null)
                {
                    phError.Visible = true;
                    litError.Text = "Subasta no encontrada.";
                    return;
                }

                var baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Request.ApplicationPath?.TrimEnd('/')}";

                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = $"Fee de participación - {vm.Titulo}",
                    Quantity = 1,
                    CurrencyId = "USD",
                    UnitPrice = 5m
                }
            },
                    Payer = new PreferencePayerRequest
                    {
                        Email =  "test_user_653940919069735644@testuser.com"
                    },

                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = $"{baseUrl}/FeeParticipacionSuccess.aspx?id={_idSubasta}",
                        Failure = $"{baseUrl}/FeeParticipacionError.aspx?id={_idSubasta}",
                        Pending = $"{baseUrl}/FeeParticipacionError.aspx?id={_idSubasta}"
                    },

                    ExternalReference = _idSubasta.ToString(),

                    AutoReturn = "approved"
                };

                var client = new PreferenceClient();
                var preference = client.Create(request);

                Response.Redirect(preference.InitPoint, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                phError.Visible = true;
                litError.Text = "Error al crear la preferencia de pago: " + ex.Message;
            }
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }
    }
}