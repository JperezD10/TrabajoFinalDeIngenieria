using BE.Observer;

namespace Artify
{
    public static class I18n
    {
        public static string Text(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            string raw = IdiomaManager.Instance.T(key);
            try
            {
                return (args != null && args.Length > 0) ? string.Format(raw, args) : raw;
            }
            catch
            {
                return raw; // si falla el formato, mostrás la cadena base
            }
        }
    }
}