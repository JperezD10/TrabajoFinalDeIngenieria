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
    public partial class FeeParticipacion : BasePage
    {
        private readonly ParticipacionSubastaBLL _bll = new ParticipacionSubastaBLL();
        private readonly SubastaBLL _subastaBll = new SubastaBLL();
        private int _idSubasta;
        private Usuario _usuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
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

            var participacion = new ParticipacionSubasta
            {
                IdSubasta = _idSubasta,
                IdCliente = _usuario.Id
            };

            var res = _bll.GuardarParticipacion(participacion);

            if (res.Exito)
            {
                Response.Redirect($"~/SubastaDetalle.aspx?id={_idSubasta}", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                phError.Visible = true;
                litError.Text = T(res.Mensaje ?? "err.participacion.guardar");
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