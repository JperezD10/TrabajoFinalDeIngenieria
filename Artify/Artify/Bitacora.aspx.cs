using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class Bitacora : System.Web.UI.Page
    {
        private List<BE.Bitacora> _lista;
        BitacoraBLL _bitacoraBLL;
        public Bitacora()
        {
            _bitacoraBLL = new BitacoraBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuario = Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (usuario.Rol != RolUsuario.Webmaster)
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                CargarDatos();
                gvBitacora.DataSource = _lista;
                gvBitacora.DataBind();
            }
        }

        private void CargarDatos()
        {
            _lista = new List<BE.Bitacora>();
            _lista = _bitacoraBLL.ListarBitacoras(null, null).Data;
        }

        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBitacora.PageIndex = e.NewPageIndex;
            CargarDatos();
            gvBitacora.DataSource = _lista;
            gvBitacora.DataBind();
        }
        protected void gvBitacora_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbl = (Label)e.Row.FindControl("lblCriticidad");
                if (lbl != null)
                {
                    int nivel;
                    if (int.TryParse(lbl.Text, out nivel))
                    {
                        switch (nivel)
                        {
                            case (int)BE.Criticidad.Leve:
                                lbl.CssClass += " badge badge-leve";
                                lbl.Text = "Leve";
                                break;
                            case (int)BE.Criticidad.Moderada:
                                lbl.CssClass += " badge badge-moderada";
                                lbl.Text = "Moderada";
                                break;
                            case (int)BE.Criticidad.Alta:
                                lbl.CssClass += " badge badge-alta";
                                lbl.Text = "Alta";
                                break;
                            case (int)BE.Criticidad.Critica:
                                lbl.CssClass += " badge badge-critica";
                                lbl.Text = "Crítica";
                                break;
                            default:
                                lbl.CssClass += " badge badge-leve";
                                lbl.Text = "Leve";
                                break;
                        }
                    }
                }
            }
        }
    }
}