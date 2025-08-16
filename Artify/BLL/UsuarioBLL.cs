using BE;
using DAL;
using SEGURIDAD;
using System;
using System.Collections.Generic;

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
            if (usuario == null)
            {
                return Response<Usuario>.Error("Credenciales invalidas");
            }

            if (usuario.Password == passwordHash)
            {
                _usuarioDAL.ReestablecerIntentos(usuario);
                _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Inicio de Sesion", (int)Criticidad.Leve,"Login",usuario.Id));
                return Response<Usuario>.Success(usuario);
            }

            var resultado = _usuarioDAL.RestarIntentos(usuario);
            if (resultado > 0)
                return Response<Usuario>.Error($"Credenciales invalidas. Intentos restantes: {resultado}");
            _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Bloqueo de usuario", (int)Criticidad.Moderada, "Login", usuario.Id));
            return Response<Usuario>.Error("Usuario bloqueado");
        }

        public Response<List<Usuario>> ListarBloqueados()
        {
            try
            {
                var usuarios = _usuarioDAL.ListarBloqueados();
                return Response<List<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return Response<List<Usuario>>.Error($"Error al buscar los usuarios: {ex.Message}"); ;
            }
        }

        public Response<bool> DesbloquearUsuario(int id)
        {
            try
            {
                var usuario = _usuarioDAL.ObtenerPorId(id);
                if (usuario == null)
                {
                    return Response<bool>.Error("Usuario no encontrado");
                }
                _usuarioDAL.DesbloquearUsuario(usuario);
                _bitacoraBLL.RegistrarAccion(new Bitacora(DateTime.Now, "Desbloqueo de usuario", (int)Criticidad.Moderada, "DesbloquearUsuario", usuario.Id));
                return Response<bool>.Success(true, "Usuario desbloqueado con exito");
            }
            catch (Exception ex)
            {
                return Response<bool>.Error($"Error al desbloquear el usuario: {ex.Message}");
            }
        }
    }
}
