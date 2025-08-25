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
                    "~/BackupDB.aspx"
                }},
                { RolUsuario.Cliente, new List<string> {
                    "~/HomeCliente.aspx",
                    "~/Mantenimiento.aspx",
                    "~/ObrasCliente.aspx"
                }},
                { RolUsuario.Curador, new List<string> {
                    "~/HomeCurador.aspx",
                    "~/ObrasCurador.aspx"
                }}
            };

        public static Usuario CheckAccess(Page page)
        {
            var usuario = page.Session["Usuario"] as Usuario;
            if (usuario == null)
            {
                var back = HttpUtility.UrlEncode(page.Request.RawUrl);
                page.Response.Redirect($"~/Login.aspx?returnUrl={back}", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return null;
            }

            var currentUrl = page.AppRelativeVirtualPath;

            if (!_rolePages.TryGetValue(usuario.Rol, out var allowedPages))
            {
                RedirectDenied(page, "Rol no definido");
                return null;
            }

            bool ok = allowedPages.Any(p => p.Equals(currentUrl, StringComparison.OrdinalIgnoreCase));
            if (!ok)
            {
                var requiredRoles = _rolePages
                    .Where(kvp => kvp.Value.Any(p =>
                        p.Equals(currentUrl, StringComparison.OrdinalIgnoreCase)))
                    .Select(kvp => kvp.Key.ToString());

                string need = requiredRoles.Any()
                    ? string.Join(" | ", requiredRoles)
                    : "Desconocido";

                RedirectDenied(page, $"role:{need}");
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
