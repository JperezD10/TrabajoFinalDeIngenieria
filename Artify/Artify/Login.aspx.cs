using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class Login : System.Web.UI.Page
    {
        UsuarioBLL usuarioBLL;

        public Login()
        {
            usuarioBLL = new UsuarioBLL();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var response = usuarioBLL.Login(txtEmail.Text, txtPassword.Text);
            if (response.Exito)
            {
                Session["Usuario"] = response.Data;
                pnlError.Visible = false;
                Response.Redirect("HomeWebMaster.aspx");
            }
            else
            {
                pnlError.Visible = true;
                lblError.Text = response.Mensaje;
            }
        }
    }
}