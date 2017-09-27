using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class LabourList : PageBase
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> Labours = CurrentBRJob.SelectAllLabour();
            gvLabour.DataSource = Labours;
            gvLabour.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_Home);
        }

        protected void btnAddLabour_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/LabourDetails.aspx",false);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Response.Redirect("~/Private/LabourDetails.aspx?id=" + b.CommandArgument.ToString());
        }
    }
}