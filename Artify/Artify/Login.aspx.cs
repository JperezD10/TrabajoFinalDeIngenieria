using BE;
using BE.Observer;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Localize = BE.Observer.Localize;

namespace Artify
{
    public partial class Login : BasePage
    {
        private readonly UsuarioBLL usuarioBLL;

        public Login()
        {
            usuarioBLL = new UsuarioBLL();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "login");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Idioma lang;
                if (Session["Idioma"] is Idioma)
                {
                    lang = (Idioma)Session["Idioma"];
                    ddlLang.SelectedValue = lang.ToString();
                    IdiomaManager.Instance.SetIdioma(lang);
                }
                else
                {
                    ddlLang.SelectedValue = IdiomaManager.Instance.IdiomaActual.ToString();
                }

                ApplyNonObserverTexts();

                pnlError.Visible = false;
            }
        }

        private void ApplyNonObserverTexts()
        {
            rfvEmail.ErrorMessage = IdiomaManager.Instance.T("login.email.required");
            revEmail.ErrorMessage = IdiomaManager.Instance.T("login.email.invalid");
            rfvPassword.ErrorMessage = IdiomaManager.Instance.T("login.password.required");
            revPassword.ErrorMessage = IdiomaManager.Instance.T("login.password.invalid");
            btnTogglePwd.Attributes["aria-label"] = IdiomaManager.Instance.T("login.password.aria");
        }

        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma selected = (ddlLang.SelectedValue == "EN") ? Idioma.EN : Idioma.ES;
            Session["Idioma"] = selected;
            IdiomaManager.Instance.SetIdioma(selected);   // hace Notify() adentro
            ApplyNonObserverTexts();                      // refresca validadores/ARIA
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var response = usuarioBLL.Login(txtEmail.Text, txtPassword.Text);
            if (response.Exito)
            {
                Session["Usuario"] = response.Data;
                pnlError.Visible = false;
                Response.Redirect("HomeWebMaster.aspx");
            }
            else
            {
                pnlError.Visible = true;
                lblError.Text = I18n.Text(response.MessageKey ?? "login.lblError", response.MessageArgs);
            }
        }
    }
}