using BE;
using BE.Observer;
using System;
using System.Web.UI;

namespace Artify
{
    public partial class HomeWebMaster : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "home");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["Usuario"] as BE.Usuario;
                if (usuario == null || usuario.Rol != RolUsuario.Webmaster)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
            }
        }
    }
}