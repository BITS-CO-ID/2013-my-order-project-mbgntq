using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ecms.Website.Site.MBGN
{
	public partial class test : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			List<string> lst = new List<string>() { "HungDV", "DungTT", "MaiNT", "SauTT", "DongNT", "NuPT" };
			cbbCombobox.DataSource = lst;
			cbbCombobox.DataBind();
		}
	}
}