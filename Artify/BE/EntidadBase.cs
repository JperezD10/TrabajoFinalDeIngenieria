namespace BE
{
    public abstract class EntidadBase
    {
        public int Id { get; set; }
        public bool Activo { get; set; } = true;
        public int DVH { get; set; }

        public bool ValidarDVH(int dvh) => DVH == dvh;

        public abstract int CalcularDVH();
    }
}
