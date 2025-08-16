using System.Security.Cryptography;
using System.Text;
using System;

namespace BE
{
    public abstract class EntidadBase
    {
        public int Id { get; set; }
        public bool Activo { get; set; } = true;
        public int DVH { get; set; }

        public bool ValidarDVH(int dvh) => DVH == dvh;
        public abstract string FormatoDVH { get; }
        public int CalcularDVH()
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(FormatoDVH));

                return Math.Abs(BitConverter.ToInt32(hash, 0)) % 1000000;
            }
        }
    }
}
