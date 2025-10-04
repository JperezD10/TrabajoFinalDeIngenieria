using BE;
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
    public partial class PermisosUsuario : BasePage
    {
        private readonly UsuarioBLL usuarioBLL = new UsuarioBLL();
        private readonly UsuarioPermisoBLL permisoUsuarioBLL = new UsuarioPermisoBLL();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "permusr");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsuarios();
                BindPermisos();
                CargarUsuarioSeleccionado();
            }
        }


        private void BindUsuarios()
        {
            var usuarios = GetUsuariosList();
            ddlUsuarios.DataSource = usuarios;
            ddlUsuarios.DataValueField = "Key";
            ddlUsuarios.DataTextField = "Value";
            ddlUsuarios.DataBind();

            if (ddlUsuarios.Items.Count > 0 && ddlUsuarios.SelectedIndex < 0)
                ddlUsuarios.SelectedIndex = 0;
        }

        private void BindPermisos(string filtro = null)
        {
            var rutas = GetAllAssignableRoutes(); // List<string>

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();
                rutas = rutas.Where(p => p.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            var seleccionPrev = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (ListItem item in cblPermisos.Items)
                if (item.Selected) seleccionPrev.Add(item.Value);

            cblPermisos.Items.Clear();
            foreach (var path in rutas)
            {
                var it = new ListItem(path, path);
                it.Selected = seleccionPrev.Contains(path);
                cblPermisos.Items.Add(it);
            }
        }

        private void CargarUsuarioSeleccionado()
        {
            int userId;
            if (!int.TryParse(ddlUsuarios.SelectedValue, out userId))
            {
                litRolActual.Text = "-";
                foreach (ListItem it in cblPermisos.Items) { it.Enabled = false; it.Selected = false; }
                return;
            }

            var rolName = GetRolNameByUserId(userId) ?? "-";
            litRolActual.Text = rolName;

            var basePages = GetRolePagesByRolName(rolName) ?? new List<string>();
            var extras = GetUserAdditionalPermissions(userId) ?? new List<string>();

            var baseSet = new HashSet<string>(basePages, StringComparer.OrdinalIgnoreCase);
            var extraSet = new HashSet<string>(extras, StringComparer.OrdinalIgnoreCase);

            foreach (ListItem it in cblPermisos.Items)
            {
                if (baseSet.Contains(it.Value))
                {
                    it.Selected = true;
                    it.Enabled = false; // del rol (no editable acá)
                }
                else
                {
                    it.Enabled = true;
                    it.Selected = extraSet.Contains(it.Value);
                }
            }
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPermisos(txtBuscar.Text);
            CargarUsuarioSeleccionado();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            BindPermisos(txtBuscar.Text);
            CargarUsuarioSeleccionado();
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            BindPermisos();
            CargarUsuarioSeleccionado();
        }

        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem it in cblPermisos.Items)
                if (it.Enabled) it.Selected = true;
        }

        protected void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem it in cblPermisos.Items)
                if (it.Enabled) it.Selected = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int userId;
            if (!int.TryParse(ddlUsuarios.SelectedValue, out userId))
                return;

            var seleccionExtra = new List<string>();
            foreach (ListItem it in cblPermisos.Items)
                if (it.Enabled && it.Selected)
                    seleccionExtra.Add(it.Value);

            var resp = permisoUsuarioBLL.Replace(userId, seleccionExtra);
            if (resp != null && resp.Exito)
            {
                // refresco sesión si es el usuario logueado
                var uSesion = Session["Usuario"] as Usuario;
                if (uSesion != null && uSesion.Id == userId)
                    Session["PermisosExtra"] = new List<string>(seleccionExtra);

                ShowFlashOk("Cambios guardados ✔");
                CargarUsuarioSeleccionado();
            }
            else
            {
                ShowFlashErr((resp != null && !string.IsNullOrEmpty(resp.Mensaje))
                    ? resp.Mensaje
                    : "No se pudieron guardar los permisos.");
            }
        }


        private List<KeyValuePair<int, string>> GetUsuariosList()
        {
            var all = usuarioBLL.GetAll();
            if (all == null || all.Data == null) return new List<KeyValuePair<int, string>>();
            var list = new List<KeyValuePair<int, string>>();
            foreach (var u in all.Data)
                list.Add(new KeyValuePair<int, string>(u.Id, u.Email));
            return list;
        }

        private string GetRolNameByUserId(int userId)
        {
            // Si tenés GetById, usalo. Mantengo GetAll() por compatibilidad con tu snippet.
            var resp = usuarioBLL.GetAll();
            if (resp == null || resp.Data == null) return null;
            var u = resp.Data.FirstOrDefault(x => x.Id == userId);
            return u != null ? u.Rol.ToString() : null;
        }

        private List<string> GetRolePagesByRolName(string rolName)
        {
            if (string.IsNullOrEmpty(rolName))
                return new List<string>();

            RolUsuario rol;
            if (!Enum.TryParse<RolUsuario>(rolName, true, out rol))
                return new List<string>();

            return SecurityManager.GetRolePages(rol);
        }

        private List<string> GetAllAssignableRoutes()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var restricted = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "~/PermisosUsuario.aspx"
            };

            foreach (RolUsuario rol in Enum.GetValues(typeof(RolUsuario)))
            {
                var pages = SecurityManager.GetRolePages(rol);
                foreach (string p in pages)
                    if (!restricted.Contains(p))
                        set.Add(p);
            }

            var list = new List<string>(set);
            list.Sort(StringComparer.OrdinalIgnoreCase);
            return list;
        }

        private List<string> GetUserAdditionalPermissions(int userId)
        {
            var resp = permisoUsuarioBLL.GetExtras(userId);
            if (resp == null || resp.Data == null) return new List<string>();
            return resp.Data;
        }

        private void SaveUserAdditionalPermissions(int userId, List<string> rutasExtra)
        {
            permisoUsuarioBLL.Replace(userId, rutasExtra);
        }
        private void ShowFlashOk(string msg)
        {
            pnlFlash.Visible = true;
            pnlFlash.CssClass = "flash flash--ok";
            litFlash.Text = Server.HtmlEncode(msg);
            ScriptManager.RegisterStartupScript(this, GetType(), "flashHideOk",
                "setTimeout(function(){var el=document.getElementById('" + pnlFlash.ClientID +
                "'); if(el){ el.style.opacity='0'; setTimeout(function(){ el.style.display='none'; }, 320);}}, 2000);", true);
        }

        private void ShowFlashErr(string msg)
        {
            pnlFlash.Visible = true;
            pnlFlash.CssClass = "flash flash--err";
            litFlash.Text = Server.HtmlEncode(msg);
            // no lo auto-oculto: que el usuario lea el error. Si querés, copiá el script de arriba.
        }
    }
}