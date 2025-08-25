using BE;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class AccessDenied : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "accden");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var need = Request.QueryString["need"]; // ej: "role:Webmaster"
            var back = Request.QueryString["back"];

            var usuario = Session["Usuario"] as Usuario;

            if (!string.IsNullOrEmpty(need) && need.StartsWith("role:", StringComparison.OrdinalIgnoreCase))
            {
                litRequired.Text = need.Substring("role:".Length); // saca "role:"
            }
            else
            {
                litRequired.Text = "N/D";
            }

            litCurrent.Text = usuario?.Rol.ToString() ?? "Anon";

            if (!string.IsNullOrEmpty(back))
            {
                litExtra.Text = $"Intentaste acceder a: {Server.UrlDecode(back)}";
            }
            else
            {
                litExtra.Text = "Si crees que se trata de un error, comunícate con el administrador.";
            }
        }
    }
}