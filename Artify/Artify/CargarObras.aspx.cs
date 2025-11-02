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
    public partial class CargarObras : BasePage
    {
        private readonly ArtistaBLL _artistaBll = new ArtistaBLL();
        private readonly ObraBLL _obraBll = new ObraBLL();
        private readonly BitacoraBLL _bitacora = new BitacoraBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // Enlaza todos los Literals/Labels/Button.Text por ID con prefijo "obra.*"
            RegisterLocalizablesById(this, "obra");
            SecurityManager.CheckAccess(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarArtistas();

                // Textos que NO vienen del observer automático (validadores, summary, seeds)
                ApplyNonObserverTexts();

                phOk.Visible = false;
                phErr.Visible = false;
                litErr.Text = "";
            }
        }

        private void ApplyNonObserverTexts()
        {
            // Validation summary header
            valSummary.HeaderText = IdiomaManager.Instance.T("obra.val.header");

            // Validadores y mensajes de error
            valTitulo.ErrorMessage = IdiomaManager.Instance.T("obra.val.titulo.required");
            valArtista.ErrorMessage = IdiomaManager.Instance.T("obra.val.artista.required");
            valAnioReq.ErrorMessage = IdiomaManager.Instance.T("obra.val.anio.required");
            valAnioRng.ErrorMessage = IdiomaManager.Instance.T("obra.val.anio.range");
            valTec.ErrorMessage = IdiomaManager.Instance.T("obra.val.tecnica.required");
            valPBReq.ErrorMessage = IdiomaManager.Instance.T("obra.val.pbase.required");
            valPBRegex.ErrorMessage = IdiomaManager.Instance.T("obra.val.pbase.format");
            cvPrecio.ErrorMessage = IdiomaManager.Instance.T("obra.val.pbase.gt0");
            valUrlReq.ErrorMessage = IdiomaManager.Instance.T("obra.val.url.required");
            valUrlRegex.ErrorMessage = IdiomaManager.Instance.T("obra.val.url.format");
            chkOriginal.InputAttributes["aria-label"] = IdiomaManager.Instance.T("obra.lblOriginal");

            // Seeds para el preview cuando los inputs están vacíos
            prevTitleSeed.Value = IdiomaManager.Instance.T("obra.prev.title.seed");
            prevYearSeed.Value = IdiomaManager.Instance.T("obra.prev.year.seed");
            prevTechSeed.Value = IdiomaManager.Instance.T("obra.prev.tech.seed");
        }

        private void CargarArtistas()
        {
            ddlArtistas.Items.Clear();
            ddlArtistas.Items.Insert(0, new System.Web.UI.WebControls.ListItem(
                IdiomaManager.Instance.T("obra.ddl.placeholder"), ""
            ));

            var resp = _artistaBll.GetAllForDVH();
            if (resp.Exito && resp.Data != null)
            {
                ddlArtistas.DataSource = resp.Data;
                ddlArtistas.DataBind();
            }
        }

        protected void cvPrecio_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = TryParseMoney(args.Value, out var v) && v > 0m;
        }

        private bool TryParseMoney(string input, out decimal value)
        {
            var norm = (input ?? "").Replace(',', '.');
            return decimal.TryParse(norm, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            phOk.Visible = false; phErr.Visible = false; litErr.Text = "";

            if (!Page.IsValid) return;

            if (!int.TryParse(txtAnio.Text.Trim(), out var anio))
            {
                litErr.Text = IdiomaManager.Instance.T("obra.err.anio");
                phErr.Visible = true; return;
            }
            if (string.IsNullOrEmpty(ddlArtistas.SelectedValue))
            {
                litErr.Text = IdiomaManager.Instance.T("obra.err.artista");
                phErr.Visible = true; return;
            }
            if (!TryParseMoney(txtPrecioBase.Text.Trim(), out var precioBase) || precioBase <= 0m)
            {
                litErr.Text = IdiomaManager.Instance.T("obra.err.precioBase");
                phErr.Visible = true; return;
            }

            var obra = new Obra
            {
                Titulo = txtTitulo.Text.Trim(),
                Anio = anio,
                Tecnica = txtTecnica.Text.Trim(),
                EsOriginal = chkOriginal.Checked,
                ArtistaId = int.Parse(ddlArtistas.SelectedValue),
                PrecioBase = decimal.Round(precioBase, 2),
                PrecioActual = decimal.Round(precioBase, 2),
                UrlImagen = txtUrlImagen.Text.Trim(),
                Activo = true
            };

            var resp = _obraBll.CargarObra(obra);

            if (!resp.Exito)
            {
                litErr.Text = resp.Mensaje?? IdiomaManager.Instance.T("obra.err.save");
                phErr.Visible = true;
                return;
            }

            try
            {
                _bitacora.RegistrarAccion(new BE.Bitacora
                {
                    Usuario = (Session["Usuario"] as Usuario)?.Email ?? "N/A",
                    Accion = $"Se ha registrado la obra {obra.Id}",
                    Fecha = DateTime.Now,
                    Criticidad = (int)Criticidad.Leve,
                    IdUsuario = UsuarioActualId()
                });
            }
            catch { /* no bloquear por bitácora */ }

            phOk.Visible = true;
            LimpiarFormulario();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            phOk.Visible = phErr.Visible = false;
            litErr.Text = "";
        }

        private void LimpiarFormulario()
        {
            txtTitulo.Text = "";
            txtAnio.Text = "";
            txtTecnica.Text = "";
            ddlArtistas.SelectedIndex = 0;
            txtPrecioBase.Text = "";
            txtPrecioActual.Text = "";
            txtUrlImagen.Text = "";
        }

        private int UsuarioActualId()
        {
            var u = Session["Usuario"] as Usuario;
            return u?.Id ?? 0;
        }
    }
}