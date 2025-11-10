using BE;
using DAL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;

namespace BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly BitacoraBLL _bitacoraBLL;
        public UsuarioBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _bitacoraBLL = new BitacoraBLL();
        }

        public Response<Usuario> Login(string email, string password)
        {
            if (!_usuarioDAL.ExisteInfraestructuraUsuarios())
            {
                var backupUser = CargarUsuarioBackup();
                if (backupUser != null)
                    return Response<Usuario>.Success(backupUser);

                return Response<Usuario>.Error("err.login.fallback");
            }

            var usuario = _usuarioDAL.Login(email);
            if (usuario == null)
                return Response<Usuario>.Error("err.login.invalid");

            if (usuario.Bloqueado)
                return Response<Usuario>.Error("err.login.alreadyBlocked");

            var passwordHash = Encriptacion.EncriptadoPermanente(password);

            if (usuario.Password == passwordHash)
            {
                _usuarioDAL.ReestablecerIntentos(usuario);

                // Solo registramos en bitácora si la base no está corrupta
                if (!BaseCorrupta())
                {
                    _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Inicio de Sesion",
                        (int)Criticidad.Leve, "Login", usuario.Id));
                }

                return Response<Usuario>.Success(usuario);
            }

            var intentosRestantes = _usuarioDAL.RestarIntentos(usuario);
            if (intentosRestantes > 0)
            {
                return Response<Usuario>.Error("err.login.invalid.withAttempts", intentosRestantes);
            }

            if (!BaseCorrupta())
            {
                _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Bloqueo de usuario",
                    (int)Criticidad.Moderada, "Login", usuario.Id));
            }

            return Response<Usuario>.Error("err.login.blocked");
        }


        private bool BaseCorrupta()
        {
            try
            {
                var integridadH = new IntegridadHorizontalBLL();
                var respH = integridadH.VerificarTodo();

                bool hayCorrupcionH = !respH.Exito || respH.Data.Any(r =>
                    !string.IsNullOrEmpty(r.Error) ||
                    (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0));

                var integridadV = new IntegridadVerticalBLL();
                var dvvCorruptas = integridadV.ObtenerVerticalesCorruptos();

                bool hayCorrupcionV = dvvCorruptas != null && dvvCorruptas.Count > 0;

                return hayCorrupcionH || hayCorrupcionV;
            }
            catch
            {
                return true; // Si algo explota, asumimos que está corrupta
            }
        }

        public Response<List<Usuario>> ListarBloqueados()
        {
            try
            {
                var usuarios = _usuarioDAL.ListarBloqueados();
                return Response<List<Usuario>>.Success(usuarios);
            }
            catch (Exception)
            {
                return Response<List<Usuario>>.Error("err.user.listBlocked"); // sin filtrar ex.Message al usuario
            }
        }

        public Response<bool> DesbloquearUsuario(int id)
        {
            try
            {
                var usuario = _usuarioDAL.ObtenerPorId(id);
                if (usuario == null)
                    return Response<bool>.Error("err.user.notFound");

                _usuarioDAL.DesbloquearUsuario(usuario);
                _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Desbloqueo de usuario", (int)Criticidad.Moderada, "DesbloquearUsuario", usuario.Id));
                return Response<bool>.Success(true, "ok.user.unblocked");
            }
            catch (Exception)
            {
                return Response<bool>.Error("err.user.unblock");
            }
        }

        public Response<List<Usuario>> GetAll()
        {
            return Response<List<Usuario>>.Success(_usuarioDAL.GetAllForDVH().ToList());
        }

        private Usuario CargarUsuarioBackup()
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath("~/UsuariosBackup.xml");

                if (!File.Exists(path))
                    return null;

                var doc = XDocument.Load(path);
                var u = doc.Element("UsuarioBackup");
                if (u == null) return null;

                return new Usuario
                {
                    Nombre = (string)u.Element("Nombre"),
                    Apellido = (string)u.Element("Apellido"),
                    Email = (string)u.Element("Email"),
                    Password = (string)u.Element("Password"),
                    Rol = RolUsuario.BackUp,
                    Bloqueado = false,
                    IntentosRestantes = 3
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
