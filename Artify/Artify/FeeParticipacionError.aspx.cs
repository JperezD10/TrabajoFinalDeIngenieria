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
    public partial class FeeParticipacionError : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "feepartfail");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            litTitle.Text = T("feepartfail.litTitle");
            litDesc.Text = T("feepartfail.litDesc");

            if (!int.TryParse(Request.QueryString["id"], out var idSubasta))
            {
                lnkVolver.NavigateUrl = "~/HomeCliente.aspx";
            }
            else
            {
                lnkVolver.NavigateUrl = $"~/SubastaDetalle.aspx?id={idSubasta}";
            }

            lnkVolver.Text = T("feepartfail.btnBack");
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