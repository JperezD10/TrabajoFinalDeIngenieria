using BE;
using BE.Observer;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class VerObras : BasePage
    {
        ObraBLL obraBLL = new ObraBLL();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterLocalizablesById(this, "verobras");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Context.Items["priceLabel"] = IdiomaManager.Instance.T("verobras.litPriceLabel");

                var data = obraBLL.GetAllForDVH();
                BindObras(data);
            }
        }

        private void BindObras(IEnumerable<Obra> obras)
        {
            var list = obras?.ToList() ?? new List<Obra>();
            rptObras.DataSource = list;
            rptObras.DataBind();
            pnlEmpty.Visible = list.Count == 0;
        }
    }
}