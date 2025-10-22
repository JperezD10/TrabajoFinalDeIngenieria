using BE;
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
    public partial class HomeCliente : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "newhomecli");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

            if (!IsPostBack)
            {
                var usuario = (Usuario)Session["Usuario"];
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
            }
        }
    }
}