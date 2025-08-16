using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class HomeWebMaster : System.Web.UI.Page
    {
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