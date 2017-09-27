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
    public partial class SiteVisibility : PageBase
    {
        protected DOSite Site;
        protected List<DOBase> SVCurrent;
        protected List<Guid> VisibleContactIDs;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentSite == null)
                Response.Redirect(Constants.URL_Home);
            Site = CurrentSessionContext.CurrentSite;
            ddlSV.SelectedValue = ((int)Site.VisibilityStatus).ToString();

            SVCurrent = CurrentBRSite.SelectSiteVisibilities(Site.SiteID);

            VisibleContactIDs = new List<Guid>();
            foreach (DOSiteVisibility sv in SVCurrent)
                VisibleContactIDs.Add(sv.ContactID);
        }

        private void SetVisibleIDs()
        {
            VisibleContactIDs = new List<Guid>();
            foreach (DOSiteVisibility sv in SVCurrent)
                VisibleContactIDs.Add(sv.ContactID);

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Select all users that have added the owner of the site as a customer directly.
            List<DOBase> Contacts = CurrentBRContact.SelectCustomerContacts(Site.ContactID);

            rpCustomers.DataSource = Contacts;
            rpCustomers.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Site.VisibilityStatus = (Framework.DataObjects.SiteVisibility)int.Parse(Request.Form[ddlSV.UniqueID]);
            CurrentBRSite.SaveSite(Site);

            if (Site.VisibilityStatus == Framework.DataObjects.SiteVisibility.Selected)
            {
                //Delete all currently selected.
                foreach (DOSiteVisibility sv in SVCurrent)
                    CurrentBRSite.DeleteSiteVisibility(sv);

                SVCurrent.Clear();

                //Create visibility for selected items.
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.StartsWith("sv"))
                    {
                        Guid ContactID = new Guid(key.Substring(2));
                        DOSiteVisibility svNew = CurrentBRSite.CreateSiteVisiblity(CurrentSessionContext.Owner.ContactID, Site.SiteID, ContactID);
                        CurrentBRSite.SaveSiteVisibility(svNew);
                        SVCurrent.Add(svNew);
                    }
                }
                SetVisibleIDs();
            }

            ShowMessage("Saved successfully.", MessageType.Info);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_SiteHome);
        }
    }
}