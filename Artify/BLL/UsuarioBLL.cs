using BE;
using DAL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var usuario = _usuarioDAL.Login(email);
            var passwordHash = Encriptacion.EncriptadoPermanente(password);
            var dvh = usuario.CalcularDVH();

            if (usuario == null)
            {
                return Response<Usuario>.Error("err.login.invalid");
            }
            if (usuario.Bloqueado)
                return Response<Usuario>.Error("err.login.alreadyBlocked");

            if (usuario.Password == passwordHash)
            {
                _usuarioDAL.ReestablecerIntentos(usuario);
                _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Inicio de Sesion", (int)Criticidad.Leve, "Login", usuario.Id));
                return Response<Usuario>.Success(usuario);
            }

            var intentosRestantes = _usuarioDAL.RestarIntentos(usuario);
            if (intentosRestantes > 0)
            {
                // Con placeholder {0} para intentos
                return Response<Usuario>.Error("err.login.invalid.withAttempts", intentosRestantes);
            }

            _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Bloqueo de usuario", (int)Criticidad.Moderada, "Login", usuario.Id));
            return Response<Usuario>.Error("err.login.blocked");
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
    }
}
