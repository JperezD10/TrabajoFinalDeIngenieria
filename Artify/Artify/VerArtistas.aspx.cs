using BE;
using BE.Observer;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class VerArtistas : BasePage
    {
        // Ruta a una imagen placeholder para cuando no hay UrlFoto
        private const string PlaceholderExternal = "https://picsum.photos/seed/artify-placeholder/800/450";
        ArtistaBLL ArtistaBLL = new ArtistaBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // Prefijo de traducción para esta página
            RegisterLocalizablesById(this, "listart");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplyNonObserverTexts();
                BindGrid();
            }
        }

        private void ApplyNonObserverTexts()
        {
            // Los textos de botones y confirmaciones no los agarra el observer
            ViewState["del.btn"] = IdiomaManager.Instance.T("listart.btnEliminar");
            ViewState["del.confirm"] = IdiomaManager.Instance.T("listart.confirmEliminar");
        }

        private void BindGrid()
        {
            // TODO: reemplazar por acceso real
            // var artistas = new ArtistaBLL().Listar();
            var artistas = ArtistaBLL.GetAllForDVH().Data?.ToList();
            bool hasItems = artistas != null && artistas.Count > 0;
            pnlEmpty.Visible = !hasItems;

            rptArtistas.DataSource = artistas;
            rptArtistas.DataBind();
        }

        protected void rptArtistas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var artista = e.Item.DataItem as Artista;

            // Imagen con fallback externo si está vacío o si la URL falla
            var img = (Image)e.Item.FindControl("imgFoto");
            if (img != null)
            {
                if (artista == null || string.IsNullOrWhiteSpace(artista.UrlFoto))
                    img.ImageUrl = PlaceholderExternal;

                // si la URL devuelve 404/rota → usar placeholder externo
                img.Attributes["onerror"] = "this.onerror=null;this.src='" + PlaceholderExternal + "';";
            }

            // Botón eliminar traducido + confirm
            var btn = (Button)e.Item.FindControl("btnEliminar");
            if (btn != null)
            {
                btn.Text = Convert.ToString(ViewState["del.btn"]) ?? "Eliminar";
                var confirm = HttpUtility.JavaScriptStringEncode(Convert.ToString(ViewState["del.confirm"]) ?? "¿Seguro?");
                btn.OnClientClick = "return confirm('" + confirm + "');";
            }
        }

        protected void rptArtistas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete" && int.TryParse(e.CommandArgument.ToString(), out int id))
            {
                try
                {
                    // 1) Eliminar artista
                    // new ArtistaBLL().Eliminar(id);

                    // 2) Actualizar DVV de tabla Artista
                    // new IntegridadVerticalBLL().ActualizarDvv("Artista");

                    // 3) Registrar en Bitácora
                    // new BitacoraBLL().Registrar( ... "Eliminación de artista Id=" + id, Criticidad.Media, Modulo.Curador, usuarioActual.Id );

                    BindGrid();
                }
                catch (Exception ex)
                {
                    // TODO: manejar error (mensaje al usuario + bitácora de error)
                    // new BitacoraBLL().RegistrarError("ERR_ART_DEL", ex);
                }
            }
        }

        // Helper para truncar biografía con seguridad
        protected string GetBioSnippet(object dataItem)
        {
            var a = dataItem as Artista;
            if (a == null || string.IsNullOrWhiteSpace(a.Biografia)) return string.Empty;

            var s = a.Biografia.Trim();
            if (s.Length > 1000) s = s.Substring(0, 997) + "...";
            return HttpUtility.HtmlEncode(s);
        }

        protected string GetBioFull(object dataItem)
        {
            var a = dataItem as Artista;
            if (a == null || string.IsNullOrWhiteSpace(a.Biografia)) return string.Empty;
            return HttpUtility.HtmlEncode(a.Biografia.Trim());
        }

    }
}