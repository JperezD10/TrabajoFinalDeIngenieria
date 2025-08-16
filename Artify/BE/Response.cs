namespace BE
{
    public class Response<T>
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }

        public static Response<T> Success(T data, string mensaje = "Operación exitosa")
        {
            return new Response<T>
            {
                Exito = true,
                Mensaje = mensaje,
                Data = data
            };
        }

        public static Response<T> Error(string mensaje)
        {
            return new Response<T>
            {
                Exito = false,
                Mensaje = mensaje,
                Data = default
            };
        }
    }
}
