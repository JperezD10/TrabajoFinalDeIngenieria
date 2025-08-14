using System;
using System.Security.Cryptography;
using System.Text;

namespace BE
{
    public class Bitacora : EntidadBase
    {
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public string Criticidad { get; set; }

        public override int CalcularDVH()
        {
            using (var sha = SHA256.Create())
            {
                string data = $"{Fecha:O}{IdUsuario}{Descripcion}{Criticidad}";
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(data));

                return Math.Abs(BitConverter.ToInt32(hash, 0)) % 1000000;
            }
        }
    }

    public enum Criticidad
    {
        Baja,
        Media,
        Alta
    }
}
