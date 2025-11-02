using BE.Observer;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class EstadisticasCurador : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "estadcli");
            SecurityManager.CheckAccess(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litResultado.Text = "";
            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DateTime.TryParse(txtDesde.Text, out var desde) ||
                    !DateTime.TryParse(txtHasta.Text, out var hasta))
                {
                    litResultado.Text = $"<span style='color:#f87171'>{T("estadcli.msg.invalidDates")}</span>";
                    return;
                }

                var client = new EstadisticaService();
                var promedio = client.CalcularPromedioFacturacion(desde, hasta);

                litResultado.Text = promedio > 0
                    ? string.Format(T("estadcli.msg.resultOk"), promedio)
                    : T("estadcli.msg.noData");
            }
            catch (Exception ex)
            {
                litResultado.Text = $"<span style='color:#f87171'>{T("estadcli.msg.error")} ({ex.Message})</span>";
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