using BE;
using BE.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Localize = BE.Observer.Localize;

namespace Artify
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Saludo con nombre (no observer porque lleva argumento dinámico)
            var usuario = Session["Usuario"] as Usuario;
            string nombre = usuario?.Nombre ?? "Test" ;
            litGreeting.Text = string.Format(IdiomaManager.Instance.T("site.greeting"), nombre);
            btnLogout.Text = IdiomaManager.Instance.T("site.logout");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}