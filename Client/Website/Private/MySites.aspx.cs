using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class MySites : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> Sites = CurrentBRSite.SelectMySites(CurrentSessionContext.CurrentContact.ContactID);
            rpSites.DataSource = Sites;
            rpSites.DataBind();

            DataBind();
        }

        protected void btnAddSite_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_SiteDetails + "?for=self");
        }

        protected void btnSelectSite_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                if (b.CommandName == "SelectSite")
                {
                    DOSite Site = CurrentBRSite.SelectSite(new Guid(b.CommandArgument.ToString()));
                    CurrentSessionContext.CurrentSite = Site;
                    Response.Redirect(Constants.URL_SiteHome);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

    }
}