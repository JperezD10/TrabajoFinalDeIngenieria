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
    public partial class HomeCurador : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "homecur");
            SecurityManager.CheckAccess(this);
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
                if (string.IsNullOrEmpty(lnkList.NavigateUrl)) lnkList.NavigateUrl = "~/VerArtistas.aspx";
                if (string.IsNullOrEmpty(lnkArtistas.NavigateUrl)) lnkArtistas.NavigateUrl = "~/CargarArtistas.aspx";
                if (string.IsNullOrEmpty(lnkObras.NavigateUrl)) lnkObras.NavigateUrl = "~/CargarObras.aspx";
                if (string.IsNullOrEmpty(lnkStats.NavigateUrl)) lnkStats.NavigateUrl = "~/EstadisticasCurador.aspx";
            }
        }
    }
}