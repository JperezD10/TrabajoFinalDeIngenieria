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
    public partial class SuscripcionComprar : BasePage
    {
        private readonly SuscripcionBLL _bll = new SuscripcionBLL();
        private string _returnUrl;
        private Usuario _usuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var _usuario = SecurityManager.CheckAccess(this);  
            RegisterLocalizablesById(this, "suscompra"); 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (_usuario == null) return;

            _usuario = Session["Usuario"] as Usuario;
            _returnUrl = Request.QueryString["returnUrl"];

            litTitle.Text = T("sus.title", "Activar suscripción");
            litDesc.Text = T("sus.desc", "Elegí el plan para poder pujar en subastas.");
            litPlanName.Text = T("sus.plan.year", "Plan anual");
            litBadge.Text = T("sus.badge", "Acceso a pujas");
            btnConfirm.Text = T("sus.btnYearlyConfirm", "Activar anual (US$ 50)");

            var resp = _bll.ObtenerActiva(_usuario.Id);
            if (resp.Exito && resp.Data != null)
            {
                var vigente = resp.Data;
                var nuevoFin = vigente.FechaFin.AddYears(1);
                litDetail.Text = T("sus.detail.extend",
                    $"Actualmente activa hasta {vigente.FechaFin:dd/MM/yyyy}. Se extenderá hasta {nuevoFin:dd/MM/yyyy}");
                btnConfirm.Text = T("sus.btnYearlyExtend", "Extender 1 año (US$ 50)");
                phInfo.Visible = true;
                litInfo.Text = T("sus.info.renewHint", "Tu suscripción está activa; al confirmar, se extiende desde el vencimiento actual.");
            }
            else
            {
                var hoy = DateTime.Now;
                var nuevoFin = hoy.AddYears(1);
                litDetail.Text = T("sus.detail.activate",
                    $"Activación anual. Válida hasta {nuevoFin:dd/MM/yyyy}");
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            _usuario = Session["Usuario"] as Usuario;
            var resp = _bll.ActivarAnual(_usuario.Id);

            if (resp.Exito)
            {
                var go = string.IsNullOrEmpty(_returnUrl) ? "~/HomeCliente.aspx" : _returnUrl;
                Response.Redirect(go, false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            phError.Visible = true;
            litError.Text = T(resp.MessageKey ?? "err.suscripcion.crear", "No se pudo activar la suscripción.");
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }
    }
}