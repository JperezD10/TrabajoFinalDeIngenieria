namespace BE
{
    public class UsuarioPermiso : EntidadBase
    {
        public int IdUsuario { get; set; }
        public string Ruta { get; set; }
        public override string FormatoDVH => $"{IdUsuario}{Ruta}";
    }
}
