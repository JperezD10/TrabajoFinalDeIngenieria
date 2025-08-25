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
    public partial class UsuariosBloqueados : BasePage
    {
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "blocked");
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
                Cargar();
            }
        }

        private void Cargar()
        {
            pnlMsg.Visible = false;

            var resp = _usuarioBLL.ListarBloqueados();
            var data = resp.Exito ? (resp.Data ?? new List<Usuario>()) : new List<Usuario>();

            if (!resp.Exito) Mostrar(resp.Mensaje, true);

            rpUsuarios.DataSource = data;
            rpUsuarios.DataBind();

            // Localizar los controles creados por el Repeater
            RegisterLocalizablesById(rpUsuarios, "blocked");

            var pnlEmpty = (Panel)rpUsuarios.Controls[rpUsuarios.Controls.Count - 1].FindControl("pnlEmpty");
            if (pnlEmpty != null)
                pnlEmpty.Visible = data.Count == 0;
        }

        protected void rpUsuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Unblock")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var resp = _usuarioBLL.DesbloquearUsuario(id);
                Mostrar(resp.Mensaje, !resp.Exito);
                Cargar();
            }
        }

        private void Mostrar(string mensajeKey, bool danger)
        {
            pnlMsg.Visible = true;
            pnlMsg.CssClass = "alert " + (danger ? "alert-danger" : "alert-success");
            pnlMsg.Controls.Clear();
            pnlMsg.Controls.Add(new Literal { Text = I18n.Text(mensajeKey) });
        }
    }
}