using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class CrearSubasta : BasePage
    {
        private readonly SubastaBLL _subastas = new SubastaBLL();
        private readonly ObraBLL _obras = new ObraBLL();

        private Usuario _user;
        private bool _blocked;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _user = SecurityManager.CheckAccess(this);
            if (_user == null)
            {
                _blocked = true; 
                return;
            }

            RegisterLocalizablesById(this, "subasta");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_blocked) return; 
            if (!IsPostBack)
            {
                ApplyNonObserverTexts();
                CargarObrasDelCurador();
                CargarCuradorActual(_user);
            }
        }

        private void CargarCuradorActual(Usuario u)
        {
            txtCurador.Text = $"{u.Nombre} {u.Apellido} (ID {u.Id})";
        }

        private void CargarObrasDelCurador()
        {
            var obras = _obras.ListarObrasActivas();

            ddlObra.DataSource = obras;
            ddlObra.DataBind();
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            pnlMsgOk.Visible = pnlMsgErr.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlObra.SelectedValue))
            { ShowErr("err.subasta.obra.requerida"); return; }

            if (!decimal.TryParse(txtPrecioInicial.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var precioInicial)
                || precioInicial <= 0)
            { ShowErr("err.subasta.precioinicial.min"); return; }

            if (!decimal.TryParse(txtIncremento.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var incremento)
                || incremento <= 0)
            { ShowErr("err.subasta.incremento.min"); return; }

            if (!int.TryParse(txtDuracion.Text, out var duracion) || duracion <= 0)
            { ShowErr("err.subasta.duracion.min"); return; }

            DateTime? fechaProg = null;
            if (!string.IsNullOrWhiteSpace(txtFechaProg.Text))
            {
                if (!DateTime.TryParseExact(txtFechaProg.Text.Trim(),
                        new[] { "dd/MM/yyyy HH:mm", "dd/MM/yyyy H:mm", "d/M/yyyy HH:mm" },
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var fp))
                { ShowErr("err.subasta.programada.pasado"); return; }
                fechaProg = fp;
            }

            var u = (Usuario)Session["Usuario"];

            var s = new Subasta
            {
                IdObra = int.Parse(ddlObra.SelectedValue),
                IdCurador = u.Id,
                PrecioInicial = precioInicial,
                IncrementoMinimo = incremento,
                DuracionMinutos = duracion,
                Estado = EstadoSubasta.Pendiente,
                FechaProgramadaInicio = fechaProg,
                FechaCreacion = DateTime.Now,
                PrecioActual = precioInicial,
                Activo = true
            };

            var r = _subastas.Crear(s);
            if (!r.Exito) { ShowErr(r.MessageKey, r.MessageArgs); return; }

            ddlObra.ClearSelection();
            txtPrecioInicial.Text = txtIncremento.Text = txtDuracion.Text = txtFechaProg.Text = string.Empty;

            ShowOk("ok.subasta.creada", r.Data);
        }

        protected void ddlObra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlObra.SelectedValue))
            {
                txtPrecioInicial.Text = "";
                // opcional: txtIncremento.Text = "";
                return;
            }

            if (!int.TryParse(ddlObra.SelectedValue, out var idObra)) return;

            // Obtenemos la obra para leer PrecioBase
            var obra = _obras.ListarObrasParacurador().FirstOrDefault(o => o.Id == idObra);

            var precioBase = obra.PrecioBase;
            var precioInicial = Math.Round(precioBase * 1.01m, 2);  // +1% comisión
            txtPrecioInicial.Text = precioInicial.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

            // OPCIONAL: setear incremento mínimo como 1% del precio inicial
            var incrementoMin = Math.Round(precioInicial * 0.01m, 2);
            if (incrementoMin < 1) incrementoMin = 1; // piso simbólico
            txtIncremento.Text = incrementoMin.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/HomeCurador.aspx");
        }

        private string T(string key, params object[] args)
        {
            var raw = IdiomaManager.Instance.T(key);
            if (string.IsNullOrEmpty(raw)) return key;
            return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
        }

        private void ShowOk(string key, params object[] args)
        {
            pnlMsgErr.Visible = false;
            pnlMsgOk.Visible = true;
            litOk.Text = T(key, args);
        }

        private void ShowErr(string key, params object[] args)
        {
            pnlMsgOk.Visible = false;
            pnlMsgErr.Visible = true;
            litErr.Text = T(key, args);
        }

        private void ApplyNonObserverTexts()
        {
            litPageTitle.Text = T("subasta.crear.title");             
            litObra.Text = T("subasta.crear.obra");                   
            litPrecioInicial.Text = T("subasta.crear.precioInicial"); 
            litInc.Text = T("subasta.crear.incrementoMin");           
            litDuracion.Text = T("subasta.crear.duracion");           
            litFechaProg.Text = T("subasta.crear.fechaProgramada");   
            litCurador.Text = T("subasta.crear.curador");             
            btnCrear.Text = T("subasta.crear.boton");                 
            btnVolver.Text = T("subasta.crear.volver");                 

            // Validadores
            rfvObra.ErrorMessage = IdiomaManager.Instance.T("subasta.rfvObra");

            rfvPI.ErrorMessage = IdiomaManager.Instance.T("subasta.rfvPI");
            revPI.ErrorMessage = IdiomaManager.Instance.T("subasta.revPI");

            rfvInc.ErrorMessage = IdiomaManager.Instance.T("subasta.rfvInc");
            revInc.ErrorMessage = IdiomaManager.Instance.T("subasta.revInc");

            rfvDur.ErrorMessage = IdiomaManager.Instance.T("subasta.rfvDur");
            revDur.ErrorMessage = IdiomaManager.Instance.T("subasta.revDur");
        }
    }
}