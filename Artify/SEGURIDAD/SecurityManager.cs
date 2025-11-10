using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SEGURIDAD
{
    public static class SecurityManager
    {
        private static readonly Dictionary<RolUsuario, List<string>> _rolePages =
            new Dictionary<RolUsuario, List<string>>
            {
                { RolUsuario.Webmaster, new List<string> {
                    "~/HomeWebMaster.aspx",
                    "~/Bitacora.aspx",
                    "~/UsuariosBloqueados.aspx",
                    "~/BaseCorrupta.aspx",
                    "~/RestoreDB.aspx",
                    "~/BackupDB.aspx",
                    "~/PermisosUsuario.aspx"
                }},
                { RolUsuario.Cliente, new List<string> {
                    "~/HomeCliente.aspx",
                    "~/SubastasCliente.aspx",
                    "~/PagosPendientes.aspx",
                    "~/Mantenimiento.aspx",
                    "~/ObrasCliente.aspx",
                    "~/FeeParticipacion.aspx",
                    "~/FeeParticipacionSuccess.aspx",
                    "~/FeeParticipacionError.aspx",
                    "~/PagoSubastaSuccess.aspx",
                    "~/PagoSubastaCliente.aspx",
                    "~/SubastaDetalle.aspx",
                }},
                { RolUsuario.Curador, new List<string> {
                    "~/HomeCurador.aspx",
                    "~/ObrasCurador.aspx",
                    "~/CargarObras.aspx",
                    "~/Mantenimiento.aspx",
                    "~/VerObras.aspx",
                    "~/CargarArtistas.aspx",
                    "~/VerArtistas.aspx",
                    "~/CrearSubasta.aspx",
                    "~/EstadisticasCurador.aspx",
                    "~/SubastasPendientes.aspx"
                }},
                {  RolUsuario.BackUp, new List<string> {
                    "~/RestoreDB.aspx",
                } },
            };


        public static List<string> GetRolePages(RolUsuario rol)
        {
            if (_rolePages.ContainsKey(rol))
                return _rolePages[rol];
            return new List<string>();
        }

        public static Usuario CheckAccess(Page page)
        {
            var usuario = page.Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                var back = HttpUtility.UrlEncode(page.Request.RawUrl);
                page.Response.Redirect("~/Login.aspx?returnUrl=" + back, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return null;
            }

            var currentUrl = page.AppRelativeVirtualPath;

            List<string> rolePages;
            if (!_rolePages.TryGetValue(usuario.Rol, out rolePages))
            {
                RedirectDenied(page, "Rol no definido");
                return null;
            }

            var allowed = new HashSet<string>(rolePages, StringComparer.OrdinalIgnoreCase);

            var extras = page.Session["PermisosExtra"] as List<string>;
            if (extras != null)
            {
                foreach (var r in extras)
                {
                    if (!string.IsNullOrWhiteSpace(r))
                        allowed.Add(r.Trim());
                }
            }

            if (!allowed.Contains(currentUrl))
            {
                var required = new List<string>();
                foreach (var kvp in _rolePages)
                {
                    foreach (var p in kvp.Value)
                    {
                        if (string.Equals(p, currentUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            required.Add(kvp.Key.ToString());
                            break;
                        }
                    }
                }

                var need = required.Count > 0 ? string.Join(" | ", required.ToArray()) : "Desconocido";
                RedirectDenied(page, "role:" + need);
                return null;
            }

            return usuario;
        }

        private static void RedirectDenied(Page page, string need)
        {
            var back = HttpUtility.UrlEncode(page.Request.RawUrl);
            var url = page.ResolveUrl($"~/AccessDenied.aspx?need={need}&back={back}");
            page.Response.Redirect(url, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
