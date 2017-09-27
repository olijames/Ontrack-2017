using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.UserControls
{
    public partial class JobTimeSheets : UserControlBase
    {
        public DOJob Job { get; set; }
        public bool AdminMode { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            btnAdd.Enabled = (Job != null);

            if (Job != null)
            {
                List<DOBase> TimeSheets = ParentPage.CurrentBRJob.SelectTimeSheets(Job.JobID);

                if (!AdminMode)
                {
                    var vFiltered = from DOJobTimeSheet t in TimeSheets
                                    where t.ContactID == ParentPage.CurrentSessionContext.Owner.ContactID
                                    select t;
                    TimeSheets = vFiltered.ToList<DOBase>();
                }
                gvTimeSheets.DataSource = TimeSheets;
                gvTimeSheets.DataBind();
            }
        }

        protected string GetContactName(Guid ContactID)
        {
            DOContact Contact = ParentPage.CurrentBRContact.SelectContact(ContactID);
            if (Contact != null)
                return Contact.DisplayName;
            return string.Empty;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Guid TimeSheetID = new Guid(((Button)sender).CommandArgument.ToString());
            DOJobTimeSheet t = ParentPage.CurrentBRJob.SelectTimeSheet(TimeSheetID);

            ParentPage.CurrentSessionContext.CurrentTimeSheet = t;
            ParentPage.CurrentSessionContext.CurrentTimeSheetAdminMode = AdminMode;

            Response.Redirect(Constants.URL_TimeSheetDetails);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ParentPage.CurrentSessionContext.CurrentTimeSheetAdminMode = AdminMode;
            Response.Redirect(Constants.URL_TimeSheetDetails);
        }

    }
}