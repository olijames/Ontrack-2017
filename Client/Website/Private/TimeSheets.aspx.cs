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
    public partial class TimeSheets : PageBase
    {
        protected DateTime StartDate;
        protected bool Authorised;
        protected bool OwnTimeSheetsOnly = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_ContactHome);

            GetStartDate();
            Authorised = CheckEmployeePageStatus(CompanyPageFlag.TimeSheet, true);
            if (!Authorised)
            {
                //An employee can view their own time sheets if they do not have admin access to this page.
                if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID))
                {
                    OwnTimeSheetsOnly = true;
                    Authorised = true;
                }
            }            
        }

        private void GetStartDate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["weekstarting"]))
            {
                StartDate = DateTime.ParseExact(Request.QueryString["weekstarting"], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime current = DateAndTime.GetCurrentDateTime();
                StartDate = new DateTime(current.Year, current.Month, current.Day);
            }

            //Week always starts on Monday.
            while (StartDate.DayOfWeek != DayOfWeek.Monday)
            {
                StartDate = StartDate.AddDays(-1);
            }

            litStartDate.Text = DateAndTime.DisplayShortDate(StartDate);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> TimeSheetList = CurrentBRJob.SelectEmployeeTimeSheets(CurrentSessionContext.CurrentContact.ContactID, StartDate, StartDate.AddDays(7));

            if (OwnTimeSheetsOnly)
            {
                var myTimeSheets = from DOTimeSheetSummary ts in TimeSheetList where ts.ContactID == CurrentSessionContext.Owner.ContactID select ts;
                TimeSheetList = myTimeSheets.ToList<DOBase>();
            }

            gvTimeSheets.DataSource = TimeSheetList;
            gvTimeSheets.DataBind();
            this.DataBind();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Response.Redirect(string.Format("~/private/TimeSheetView.aspx?contact={0}&weekstarting={1}", b.CommandArgument, StartDate.ToString("yyyyMMdd")));
        }
    }
}