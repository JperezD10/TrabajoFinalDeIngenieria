using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class UsuariosBloqueados : System.Web.UI.Page
    {
        // ajustá a tu BLL real
        private readonly BLL.UsuarioBLL _usuarioBLL = new BLL.UsuarioBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
        }

        private void Cargar()
        {
            pnlMsg.Visible = false;

            var resp = _usuarioBLL.ListarBloqueados();
            var data = resp.Exito ? (resp.Data ?? new List<Usuario>()) : new List<Usuario>();

            if (!resp.Exito) Mostrar(resp.Mensaje, true);

            rpUsuarios.DataSource = data;
            rpUsuarios.DataBind();

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

        private void Mostrar(string mensaje, bool danger)
        {
            pnlMsg.Visible = true;
            pnlMsg.CssClass = "alert " + (danger ? "alert-danger" : "alert-success");
            pnlMsg.Controls.Clear();
            pnlMsg.Controls.Add(new Literal { Text = mensaje });
        }
    }
}