namespace BE
{
    public class Response<T>
    {
        public bool Exito { get; private set; }
        public T Data { get; private set; }

        // i18n
        public string MessageKey { get; private set; } // ej: "err.login.invalid"
        public object[] MessageArgs { get; private set; } // ej: nuevos object[]{ 2 }

        // legacy (opcional para logs)
        public string Mensaje { get; private set; }

        private Response() { }

        public static Response<T> Success(T data, string messageKey = null, params object[] args)
        {
            var r = new Response<T>();
            r.Exito = true; r.Data = data;
            r.MessageKey = messageKey; r.MessageArgs = args;
            return r;
        }

        public static Response<T> Error(string messageKey, params object[] args)
        {
            var r = new Response<T>();
            r.Exito = false;
            r.MessageKey = messageKey; r.MessageArgs = args;
            return r;
        }
    }
}
