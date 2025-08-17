using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BE.Observer
{
    public class BasePage : System.Web.UI.Page
    {
        private readonly List<IIdiomaObserver> _locals = new List<IIdiomaObserver>();

        public void RegisterLocalizable(IIdiomaObserver observer)
        {
            _locals.Add(observer);
            IdiomaManager.Instance.Subscribe(observer);
        }

        protected override void OnUnload(EventArgs e)
        {
            foreach (var o in _locals)
                IdiomaManager.Instance.Unsubscribe(o);
            base.OnUnload(e);
        }

        protected void RegisterLocalizablesById(Control root, string pagePrefix)
        {
            foreach (Control c in root.Controls)
            {
                if (!string.IsNullOrEmpty(c.ID))
                {
                    string key = $"{pagePrefix}.{c.ID}";

                    if (c is System.Web.UI.WebControls.Literal lit)
                        RegisterLocalizable(BE.Observer.Localize.Literal(lit, key));
                    else if (c is System.Web.UI.WebControls.Label lbl)
                        RegisterLocalizable(BE.Observer.Localize.Label(lbl, key));
                    else if (c is System.Web.UI.WebControls.Button btn)
                        RegisterLocalizable(BE.Observer.Localize.Button(btn, key));
                    else if (c is System.Web.UI.WebControls.LinkButton lbtn)
                        RegisterLocalizable(BE.Observer.Localize.LinkButton(lbtn, key));
                    else if (c is System.Web.UI.WebControls.HyperLink hl)
                    {
                        // Solo si el HyperLink NO contiene otros controles (texto plano).
                        if (hl.Controls.Count == 0)
                            RegisterLocalizable(BE.Observer.Localize.HyperLink(hl, key));
                        // Si tiene hijos, dejalo: localizás los Literals/Labels internos por su propio ID.
                    }
                }

                if (c.HasControls())
                    RegisterLocalizablesById(c, pagePrefix);
            }
        }
    }
}
