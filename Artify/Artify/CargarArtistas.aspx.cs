using BE;
using BE.Observer;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class CargarArtistas : BasePage
    {
        ArtistaBLL artistaBLL = new ArtistaBLL();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // Prefijo de traducción: "art."
            RegisterLocalizablesById(this, "art");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Rango de fecha: hasta hoy (evita futuras)
                rngFechaMax.CultureInvariantValues = true; // usa yyyy-MM-dd
                cmpFecha.CultureInvariantValues = true;
                rngFechaMax.MaximumValue = DateTime.Today.ToString("yyyy-MM-dd");
                rngFechaMax.MinimumValue = "1850-01-01";
                ApplyNonObserverTexts();
            }
        }

        private void ApplyNonObserverTexts()
        {
            reqNombre.ErrorMessage = IdiomaManager.Instance.T("art.reqNombre");
            reqNac.ErrorMessage = IdiomaManager.Instance.T("art.reqNac");

            cmpFecha.ErrorMessage = IdiomaManager.Instance.T("art.cmpFecha");
            rngFechaMax.ErrorMessage = IdiomaManager.Instance.T("art.rngFechaMax");

            revUrl.ErrorMessage = IdiomaManager.Instance.T("art.revUrl");
            btnGuardar.Text = IdiomaManager.Instance.T("art.btnGuardar");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DateTime? fecha = null;
            if (!string.IsNullOrWhiteSpace(txtFechaNacimiento.Text))
            {
                if (DateTime.TryParse(txtFechaNacimiento.Text, CultureInfo.InvariantCulture,
                                      DateTimeStyles.AssumeLocal, out var dt))
                {
                    fecha = dt.Date;
                }
            }

            var artista = new Artista
            {
                Nombre = txtNombre.Text?.Trim(),
                Nacionalidad = txtNacionalidad.Text?.Trim(),
                FechaNacimiento = fecha,
                UrlFoto = string.IsNullOrWhiteSpace(txtUrlFoto.Text) ? null : txtUrlFoto.Text.Trim(),
                Biografia = string.IsNullOrWhiteSpace(txtBio.Text) ? null : txtBio.Text.Trim(),
            };

            var result = artistaBLL.Agregar(artista);
            Response.Redirect("VerArtistas.aspx");
        }
    }
}