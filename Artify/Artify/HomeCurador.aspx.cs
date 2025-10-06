using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class HomeCurador : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "homecur");
            SecurityManager.CheckAccess(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(lnkList.NavigateUrl)) lnkList.NavigateUrl = "~/VerArtistas.aspx";
                if (string.IsNullOrEmpty(lnkArtistas.NavigateUrl)) lnkArtistas.NavigateUrl = "~/CargarArtistas.aspx";
                if (string.IsNullOrEmpty(lnkObras.NavigateUrl)) lnkObras.NavigateUrl = "~/CargarObras.aspx";
                if (string.IsNullOrEmpty(lnkStats.NavigateUrl)) lnkStats.NavigateUrl = "~/EstadisticasCurador.aspx";
            }
        }
    }
}