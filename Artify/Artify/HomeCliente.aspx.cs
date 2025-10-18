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
    public partial class HomeCliente : BasePage
    {
        private readonly SubastaBLL _subastaBll = new SubastaBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "homecli");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificación de integridad
            var integridad = new IntegridadHorizontalBLL();
            var resp = integridad.VerificarTodo();
            if (!resp.Exito) return;

            bool hayCorrupcion = resp.Data.Any(r =>
                !string.IsNullOrEmpty(r.Error) ||
                (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0));

            var integridadV = new IntegridadVerticalBLL();
            var dvvCorruptas = integridadV.ObtenerVerticalesCorruptos();
            if (dvvCorruptas != null && dvvCorruptas.Count > 0) hayCorrupcion = true;

            if (hayCorrupcion)
            {
                Response.Redirect("~/Mantenimiento.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (IsPostBack) return;

            var usuario = Session["Usuario"] as Usuario;
            var ahora = DateTime.Now;

            var vms = _subastaBll.GetParaHome(usuario?.Id ?? 0, ahora);

            phEmpty.Visible = vms.Count == 0;
            rptSubastas.DataSource = vms;
            rptSubastas.DataBind();

            if (vms.Count == 0)
                litEmpty.Text = T("homecli.empty");
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }

        private string BuildDelta(TimeSpan ts)
        {
            if (ts.TotalSeconds < 0) ts = TimeSpan.Zero;
            int d = (int)ts.TotalDays, h = ts.Hours, m = ts.Minutes;
            if (d > 0) return $"{d}{T("homecli.delta.d")} {h}{T("homecli.delta.h")} {m}{T("homecli.delta.m")}";
            if (ts.TotalHours >= 1) return $"{(int)ts.TotalHours}{T("homecli.delta.h")} {m}{T("homecli.delta.m")}";
            return $"{m}{T("homecli.delta.m")}";
        }

        protected void rptSubastas_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item &&
                e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem) return;

            var vm = (SubastaHomeVM)e.Item.DataItem;

            var lnkVer = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lnkVer");
            var btnDisabled = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnDisabled");
            var litEstado = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litEstado");
            var litTiempo = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litTiempo");

            lnkVer.NavigateUrl = $"SubastaDetalle.aspx?id={vm.Id}";
            lnkVer.Text = T("homecli.btn.view");

            litEstado.Text = T("homecli.badge." + vm.EstadoCodigo);

            if (vm.EstadoCodigo == "running")
                litTiempo.Text = T("homecli.info.endsIn", BuildDelta(vm.FechaFin - DateTime.Now));
            else if (vm.EstadoCodigo == "scheduled")
                litTiempo.Text = T("homecli.info.startsIn", BuildDelta(vm.FechaInicio - DateTime.Now));
            else
                litTiempo.Text = T("homecli.info.endedAt", vm.FechaFin.ToString("dd/MM/yyyy HH:mm"));

            bool estadoPermitePuja =
                vm.EstadoCodigo == "running" &&
                DateTime.Now >= vm.FechaInicio &&
                DateTime.Now <= vm.FechaFin;

            if (!estadoPermitePuja)
            {
                btnDisabled.Visible = true;
                var motivoKey =
                    vm.EstadoCodigo == "scheduled" ? "homecli.reason.not_started" :
                    vm.EstadoCodigo == "finished" ? "homecli.reason.finished" :
                                                     "homecli.reason.generic";
                btnDisabled.Text = T(motivoKey);
            }
        }
    }
}