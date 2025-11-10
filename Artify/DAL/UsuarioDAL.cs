using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsuarioDAL: BaseDvvDAL
    {
        public override string TableName => "Usuario";

        public Usuario Login(string email)
        {
            Usuario resultado = null;
            var user = Acceso.Leer("SELECT * FROM Usuario WHERE Email = @Email", new SqlParameter[]
            {
                new SqlParameter("@Email", email),
            }).Rows;

            if (user.Count > 0)
            {
                resultado = MapperHelper.MapUsuario(user[0]);
            }

            return resultado;
        }

        public bool ExisteInfraestructuraUsuarios()
        {
            try
            {
                var existeTabla = Acceso.Leer(
                    "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuario'", null
                ).Rows.Count > 0;

                if (!existeTabla)
                    return false;

                // Verificar existencia de al menos un Webmaster
                var existeWebmaster = Acceso.Leer(
                    "SELECT TOP 1 1 FROM Usuario WHERE Rol = @Rol",
                    new SqlParameter[]
                    {
                new SqlParameter("@Rol", (int)RolUsuario.Webmaster)
                    }
                ).Rows.Count > 0;

                return existeWebmaster;
            }
            catch
            {
                return false;
            }
        }

        public int RestarIntentos(Usuario usuario)
        {
            var intentosRestantes = usuario.IntentosRestantes;
            if (intentosRestantes > 1)
            {
                intentosRestantes--;
                usuario.IntentosRestantes = intentosRestantes;
                usuario.DVH = usuario.CalcularDVH();
                Acceso.Escribir("UPDATE Usuario SET IntentosRestantes = @IntentosRestantes, DVH = @DVH WHERE Id = @Id", new SqlParameter[]
                {
                    new SqlParameter("@IntentosRestantes", intentosRestantes),
                    new SqlParameter("@DVH", usuario.DVH),
                    new SqlParameter("@Id", usuario.Id)
                });

                ActualizarDVV();
            }
            else
            {
                usuario.Bloqueado = true;
                usuario.IntentosRestantes = 0;
                usuario.DVH = usuario.CalcularDVH();
                Acceso.Escribir("UPDATE Usuario SET Bloqueado = @Bloqueado, IntentosRestantes = 0, DVH = @DVH  WHERE Id = @Id", new SqlParameter[]
                {
                    new SqlParameter("@Bloqueado", usuario.Bloqueado),
                    new SqlParameter("@DVH", usuario.DVH),
                    new SqlParameter("@Id", usuario.Id)
                });
                intentosRestantes = 0;
                ActualizarDVV();
            }
            return intentosRestantes;
        }

        public void ReestablecerIntentos(Usuario usuario)
        {
            usuario.IntentosRestantes = 3;
            usuario.DVH = usuario.CalcularDVH();
            Acceso.Escribir("UPDATE Usuario SET IntentosRestantes = 3, Bloqueado = 0, DVH = @DVH WHERE Id = @Id", new SqlParameter[]
            {
                new SqlParameter("@Id", usuario.Id),
                new SqlParameter("@DVH", usuario.DVH)
            });
            ActualizarDVV();
        }

        public List<Usuario> ListarBloqueados()
        {
            List<Usuario> resultado = new List<Usuario>();
            var tabla = Acceso.Leer("SELECT * FROM Usuario WHERE Bloqueado = 1", null);

            foreach (DataRow item in tabla.Rows)
            {
                resultado.Add(MapperHelper.MapUsuario(item));
            }

            return resultado;
        }

        public void DesbloquearUsuario(Usuario usuario)
        {
            usuario.Bloqueado = false;
            usuario.IntentosRestantes = 3;
            usuario.DVH = usuario.CalcularDVH();
            Acceso.Escribir("UPDATE Usuario set Bloqueado = 0, IntentosRestantes = 3, DVH = @DVH WHERE Id = @Id", new SqlParameter[]
            {
                new SqlParameter("@Id", usuario.Id),
                new SqlParameter("@DVH", usuario.DVH)
            });
            ActualizarDVV();
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario resultado = null;
            var user = Acceso.Leer("SELECT * FROM Usuario WHERE Id = @Id", new SqlParameter[]
            {
                new SqlParameter("@Id", id),
            }).Rows;
            if (user.Count > 0)
            {
                resultado = MapperHelper.MapUsuario(user[0]);
            }
            return resultado;
        }

        public IEnumerable<Usuario> GetAllForDVH()
        {
            const string sql = @"
            SELECT * 
            FROM Usuario
            ORDER BY Id;";

            var tabla = Acceso.Leer(sql, null);
            foreach (DataRow row in tabla.Rows)
            {
                var u = MapperHelper.MapUsuario(row);
                yield return u;
            }
        }
    }
}
