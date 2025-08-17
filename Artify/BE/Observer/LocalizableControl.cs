using System;
using System.Collections.Generic;
using System.Web.UI;

namespace BE.Observer
{
    public class LocalizableControl : IIdiomaObserver
    {
        private readonly Control _control;
        private readonly string _key;
        private readonly Action<Control, string> _apply;

        public LocalizableControl(Control control, string key, Action<Control, string> applyText)
        {
            _control = control;
            _key = key;
            _apply = applyText;
        }

        public void OnIdiomaChanged(IReadOnlyDictionary<string, string> textos)
        {
            var value = textos.TryGetValue(_key, out var v) ? v : $"[{_key}]";
            _apply(_control, value);
        }
    }

    // Helpers para tipos comunes:
    public static class Localize
    {
        public static LocalizableControl Label(System.Web.UI.WebControls.Label lbl, string key)
            => new LocalizableControl(lbl, key, (c, v) => ((System.Web.UI.WebControls.Label)c).Text = v);

        public static LocalizableControl Button(System.Web.UI.WebControls.Button btn, string key)
            => new LocalizableControl(btn, key, (c, v) => ((System.Web.UI.WebControls.Button)c).Text = v);

        public static LocalizableControl LinkButton(System.Web.UI.WebControls.LinkButton btn, string key)
            => new LocalizableControl(btn, key, (c, v) => ((System.Web.UI.WebControls.LinkButton)c).Text = v);

        public static LocalizableControl Literal(System.Web.UI.WebControls.Literal lit, string key)
            => new LocalizableControl(lit, key, (c, v) => ((System.Web.UI.WebControls.Literal)c).Text = v);

        public static LocalizableControl HyperLink(System.Web.UI.WebControls.HyperLink hl, string key)
            => new LocalizableControl(hl, key, (c, v) => ((System.Web.UI.WebControls.HyperLink)c).Text = v);

        // Agregá más según necesites (Literal, HyperLink, etc.)
    }

}
