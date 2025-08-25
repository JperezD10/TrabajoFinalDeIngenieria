using BE;
using BE.Observer;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class BaseCorrupta : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "corrupt");   // <— igual que login pero con prefijo de pantalla
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // (opcional) Título de la página
            this.Title = IdiomaManager.Instance.T("corrupt.pageTitle");

            var resultados = Session["IntegridadResultados"] as List<TablaCheckResult>;

            if (resultados == null || resultados.Count == 0)
            {
                pnlEmpty.Visible = true;
                lblTablasAfectadas.Text = "0";
                lblRegistrosCorruptos.Text = "0";
                lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pResumen.InnerText = IdiomaManager.Instance.T("corrupt.banner.subtitleNoResults");
                return;
            }

            int tablasAfectadas = 0;
            int registrosCorruptos = 0;
            foreach (var r in resultados)
            {
                if (!string.IsNullOrEmpty(r.Error) || (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0))
                    tablasAfectadas++;
                if (r.IdsCorruptos != null)
                    registrosCorruptos += r.IdsCorruptos.Count;
            }

            lblTablasAfectadas.Text = tablasAfectadas.ToString();
            lblRegistrosCorruptos.Text = registrosCorruptos.ToString();
            lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            pResumen.InnerText = IdiomaManager.Instance.T("corrupt.banner.subtitleHasIssues");

            rptTablas.DataSource = resultados;
            rptTablas.DataBind();
        }

        protected void rptTablas_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item &&
                e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem)
                return;

            var r = (TablaCheckResult)e.Item.DataItem;

            var pOk = (System.Web.UI.WebControls.Panel)e.Item.FindControl("pOk");
            var pErr = (System.Web.UI.WebControls.Panel)e.Item.FindControl("pErr");
            var pCorrupt = (System.Web.UI.WebControls.Panel)e.Item.FindControl("pCorrupt");

            var lblErr = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblErr");
            var lblBadge = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblBadge");
            var statusDot = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("statusDot");

            var rptIds = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptIds");
            var lblOkChip = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblOkChip");
            var litCorruptTitle = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litCorruptTitle");

            bool hasError = !string.IsNullOrEmpty(r.Error);
            bool hasCorrupt = (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0);

            statusDot.Attributes["class"] = "dot " + (hasError || hasCorrupt ? "bad" : "ok");

            if (hasError)
            {
                pErr.Visible = true;
                lblErr.Text = r.Error; // viene del backend
                lblBadge.CssClass = "badge err";
                lblBadge.Text = IdiomaManager.Instance.T("corrupt.badge.readError");
            }
            else if (hasCorrupt)
            {
                pCorrupt.Visible = true;

                litCorruptTitle.Text = IdiomaManager.Instance.T("corrupt.item.corruptTitle");

                rptIds.ItemDataBound += (s, ev) =>
                {
                    if (ev.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item ||
                        ev.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
                    {
                        var litChip = (System.Web.UI.WebControls.Literal)ev.Item.FindControl("litChip");
                        litChip.Text = string.Format(
                            IdiomaManager.Instance.T("corrupt.item.corruptChipFmt"),
                            ev.Item.DataItem
                        );
                    }
                };
                rptIds.DataSource = r.IdsCorruptos;
                rptIds.DataBind();

                lblBadge.CssClass = "badge bad";
                lblBadge.Text = string.Format(
                    IdiomaManager.Instance.T("corrupt.badge.corruptCountFmt"),
                    r.IdsCorruptos.Count
                );
            }
            else
            {
                pOk.Visible = true;
                lblBadge.CssClass = "badge ok";
                lblBadge.Text = IdiomaManager.Instance.T("corrupt.badge.ok");

                if (lblOkChip != null)
                    lblOkChip.Text = string.Format(
                        IdiomaManager.Instance.T("corrupt.ok.chip"),
                        r.TotalRegistros
                    );
            }
        }
    }
}