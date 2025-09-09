using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class BaseCorrupta : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "corrupt");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Título
            this.Title = IdiomaManager.Instance.T("corrupt.pageTitle");

            // ===== DVH =====
            var resultados = Session["IntegridadResultados"] as List<TablaCheckResult>;
            if (resultados == null || resultados.Count == 0)
            {
                pnlEmpty.Visible = true;
                lblTablasAfectadas.Text = "0";
                lblRegistrosCorruptos.Text = "0";
                lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pResumen.InnerText = IdiomaManager.Instance.T("corrupt.banner.subtitleNoResults");
            }
            else
            {
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

            // ===== DVV =====
            var dvv = Session["IntegridadDVVResultados"] as List<DVV>;
            if (dvv == null)
            {
                // Fallback por si se entra directo a la página
                var v = new IntegridadVerticalBLL();
                dvv = v.ObtenerVerticalesCorruptos();
            }

            pnlDVVEmpty.Visible = dvv == null || dvv.Count == 0;
            rptDVV.DataSource = dvv;
            rptDVV.DataBind();
        }

        protected void rptTablas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var r = (TablaCheckResult)e.Item.DataItem;

            var pOk = (Panel)e.Item.FindControl("pOk");
            var pErr = (Panel)e.Item.FindControl("pErr");
            var pCorrupt = (Panel)e.Item.FindControl("pCorrupt");

            var lblErr = (Label)e.Item.FindControl("lblErr");
            var lblBadge = (Label)e.Item.FindControl("lblBadge");
            var statusDot = (HtmlGenericControl)e.Item.FindControl("statusDot");

            var rptIds = (Repeater)e.Item.FindControl("rptIds");
            var lblOkChip = (Label)e.Item.FindControl("lblOkChip");
            var litCorruptTitle = (Literal)e.Item.FindControl("litCorruptTitle");

            bool hasError = !string.IsNullOrEmpty(r.Error);
            bool hasCorrupt = (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0);

            statusDot.Attributes["class"] = "dot " + (hasError || hasCorrupt ? "bad" : "ok");

            if (hasError)
            {
                pErr.Visible = true;
                lblErr.Text = r.Error;
                lblBadge.CssClass = "badge err";
                lblBadge.Text = IdiomaManager.Instance.T("corrupt.badge.readError");
            }
            else if (hasCorrupt)
            {
                pCorrupt.Visible = true;

                litCorruptTitle.Text = IdiomaManager.Instance.T("corrupt.item.corruptTitle");

                rptIds.ItemDataBound += (s, ev) =>
                {
                    if (ev.Item.ItemType == ListItemType.Item || ev.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        var litChip = (Literal)ev.Item.FindControl("litChip");
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

        protected void rptDVV_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var dot = (HtmlGenericControl)e.Item.FindControl("dotDVV");
            var msg = (Label)e.Item.FindControl("lblDVVMsg");

            dot.Attributes["class"] = "dot bad";
            msg.Text = IdiomaManager.Instance.T("corrupt.dvv.mismatch");
        }
    }
}
