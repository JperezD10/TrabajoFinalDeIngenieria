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
    public partial class SubastasPendientes : BasePage
    {
        private readonly SubastaBLL _subastas = new SubastaBLL();
        private readonly ObraBLL _obras = new ObraBLL();

        private Usuario _user;
        private bool _blocked;
        private readonly Dictionary<int, string> _cacheTitulos = new Dictionary<int, string>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _user = SecurityManager.CheckAccess(this);
            if (_user == null) { _blocked = true; return; }
            RegisterLocalizablesById(this, "subpend");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_blocked) return;
            if (!IsPostBack)
            {
                ApplyNonObserverTexts();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            pnlMsgOk.Visible = pnlMsgErr.Visible = false;

            var r = _subastas.ListarPendientesPorCurador(_user.Id);
            if (!r.Exito)
            {
                ShowErr(r.MessageKey, r.MessageArgs);
                gvPendientes.DataSource = null;
                gvPendientes.DataBind();
                return;
            }

            gvPendientes.DataSource = r.Data.ToList();
            gvPendientes.DataBind();
        }

        protected void gvPendientes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType != System.Web.UI.WebControls.DataControlRowType.DataRow) return;

            // Columna 1 = Id, Columna 2 = IdObra
            int idObra;
            if (int.TryParse(e.Row.Cells[1].Text, out idObra))
            {
                string titulo;
                if (!_cacheTitulos.TryGetValue(idObra, out titulo))
                {
                    var resultadoObra = _obras.ListarObrasParacurador().FirstOrDefault(o => o.Id == idObra); 
                    titulo = resultadoObra != null ? resultadoObra.Titulo : $"Obra #{idObra}";
                    _cacheTitulos[idObra] = titulo;
                }
                e.Row.Cells[1].Text = titulo;
            }

            var btnStart = (System.Web.UI.WebControls.LinkButton)e.Row.FindControl("btnStart");
            if (btnStart != null) btnStart.Text = T("subpend.btn.start");
        }

        protected void gvPendientes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Start")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                var r = _subastas.Empezar(id);
                if (r.Exito) ShowOk(r.MessageKey, r.MessageArgs);
                else ShowErr(r.MessageKey, r.MessageArgs);

                BindGrid(); // refrescá lista
            }
        }

        private void ApplyNonObserverTexts()
        {
            litTitle.Text = T("subpend.title");
            gvPendientes.EmptyDataText = T("subpend.grid.empty");

            gvPendientes.Columns[0].HeaderText = T("subpend.col.id");
            gvPendientes.Columns[1].HeaderText = T("subpend.col.obra");
            gvPendientes.Columns[2].HeaderText = T("subpend.col.preini");
            gvPendientes.Columns[3].HeaderText = T("subpend.col.inc");
            gvPendientes.Columns[4].HeaderText = T("subpend.col.dur");
            gvPendientes.Columns[5].HeaderText = T("subpend.col.created");
            gvPendientes.Columns[6].HeaderText = T("subpend.col.scheduled");
            gvPendientes.Columns[7].HeaderText = T("subpend.col.actions");
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }
        private void ShowOk(string key, params object[] args)
        {
            pnlMsgErr.Visible = false; pnlMsgOk.Visible = true;
            litOk.Text = T(key, args);
        }
        private void ShowErr(string key, params object[] args)
        {
            pnlMsgOk.Visible = false; pnlMsgErr.Visible = true;
            litErr.Text = T(key, args);
        }
    }
}