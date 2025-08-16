using System;
using System.Security.Cryptography;
using System.Text;

namespace BE
{
    public class Usuario : EntidadBase
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Bloqueado { get; set; }
        public int IntentosRestantes { get; set; }
        public RolUsuario Rol { get; set; }

        public override string FormatoDVH => $"{Id}{Nombre}{Apellido}{Email}{Password}{Bloqueado}{IntentosRestantes}{Rol}";
    }
    public enum RolUsuario
    {
        Webmaster = 1,
        Cliente = 2,
        Curador = 3
    }
}
