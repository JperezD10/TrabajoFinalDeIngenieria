using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class BaseCorrupta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Traemos los resultados desde Session (los setea HomeWebMaster antes del Redirect)
            List<TablaCheckResult> resultados = Session["IntegridadResultados"] as List<TablaCheckResult>;

            if (resultados == null || resultados.Count == 0)
            {
                pnlEmpty.Visible = true;
                lblTablasAfectadas.Text = "0";
                lblRegistrosCorruptos.Text = "0";
                lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pResumen.InnerText = "No se recibieron resultados de verificación.";
                return;
            }

            // Estadísticas
            int tablasAfectadas = 0;
            int registrosCorruptos = 0;
            for (int i = 0; i < resultados.Count; i++)
            {
                TablaCheckResult r = resultados[i];
                if (!string.IsNullOrEmpty(r.Error) || (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0))
                    tablasAfectadas++;

                if (r.IdsCorruptos != null)
                    registrosCorruptos += r.IdsCorruptos.Count;
            }

            lblTablasAfectadas.Text = tablasAfectadas.ToString();
            lblRegistrosCorruptos.Text = registrosCorruptos.ToString();
            lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            pResumen.InnerText = "Se detectaron inconsistencias en el cálculo del DVH. Revise cada tabla a continuación.";

            // Bind
            rptTablas.DataSource = resultados;
            rptTablas.DataBind();
        }

        protected void rptTablas_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            // Evitar cabecera / pie
            if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item &&
                e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem)
                return;

            TablaCheckResult r = (TablaCheckResult)e.Item.DataItem;

            System.Web.UI.WebControls.Panel pOk =
                (System.Web.UI.WebControls.Panel)e.Item.FindControl("pOk");
            System.Web.UI.WebControls.Panel pErr =
                (System.Web.UI.WebControls.Panel)e.Item.FindControl("pErr");
            System.Web.UI.WebControls.Panel pCorrupt =
                (System.Web.UI.WebControls.Panel)e.Item.FindControl("pCorrupt");
            System.Web.UI.WebControls.Label lblErr =
                (System.Web.UI.WebControls.Label)e.Item.FindControl("lblErr");
            System.Web.UI.WebControls.Label lblBadge =
                (System.Web.UI.WebControls.Label)e.Item.FindControl("lblBadge");
            System.Web.UI.HtmlControls.HtmlGenericControl statusDot =
                (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("statusDot");
            System.Web.UI.WebControls.Repeater rptIds =
                (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptIds");

            bool hasError = !string.IsNullOrEmpty(r.Error);
            bool hasCorrupt = (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0);

            string dotClass = "dot " + (hasError || hasCorrupt ? "bad" : "ok");
            statusDot.Attributes["class"] = dotClass;

            if (hasError)
            {
                pErr.Visible = true;
                lblErr.Text = r.Error;
                lblBadge.CssClass = "badge err";
                lblBadge.Text = "Error de lectura";
            }
            else if (hasCorrupt)
            {
                pCorrupt.Visible = true;
                rptIds.DataSource = r.IdsCorruptos;
                rptIds.DataBind();
                lblBadge.CssClass = "badge bad";
                lblBadge.Text = "Registros corruptos: " + r.IdsCorruptos.Count;
            }
            else
            {
                pOk.Visible = true;
                lblBadge.CssClass = "badge ok";
                lblBadge.Text = "OK";
            }
        }
    }
}