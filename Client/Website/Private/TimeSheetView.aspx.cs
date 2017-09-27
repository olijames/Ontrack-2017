using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using System.Collections;

namespace Electracraft.Client.Website.Private
{

    [PrivatePage]
    public partial class TimeSheetView : PageBase
    {
        struct timeSheetViews
        {
            public DOTaskLabourFull timeSheet { get; set; }
            public string day { get; set; }
            public bool visible { get; set; }
            public int totalHours { get; set; }
        }
        protected DOContact Contact;
        protected DateTime StartDate;
        protected bool Authorised = false;
        protected bool OwnTimeSheetsOnly = false;
        static int totaltime = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (CurrentSessionContext.CurrentContact == null) throw new Exception();
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

                StartDate = DateTime.ParseExact(Request.QueryString["weekstarting"], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                while (StartDate.DayOfWeek != DayOfWeek.Monday)
                {
                    StartDate = StartDate.AddDays(-1);
                }

                Contact = CurrentBRContact.SelectContact(new Guid(Request.QueryString["contact"]));
                if (OwnTimeSheetsOnly)
                {
                    if (Contact.ContactID != CurrentSessionContext.Owner.ContactID)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                Response.Redirect(Constants.URL_ContactHome);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Authorised)
            {
                List<DOBase> employeeTimeSheets = CurrentBRJob.SelectSingleEmployeeTimeSheets(CurrentSessionContext.CurrentContact.ContactID, Contact.ContactID, StartDate, StartDate.AddDays(7));
                gvTimeSheet.DataSource = employeeTimeSheets;
                List<timeSheetViews> tsv = new List<timeSheetViews>();
                for(int i=0;i<employeeTimeSheets.Count;i++)
                {
                    DOTaskLabourFull tm = employeeTimeSheets[i] as DOTaskLabourFull;
                    string d = DateAndTime.DisplayDay(tm.LabourDate);
                    bool vis = true;
                    int hours = 0;
                    for(int j=i+1;j<employeeTimeSheets.Count;j++)
                    {
                        
                    }
                    tsv.Add(new timeSheetViews() { timeSheet = tm, day = d, visible = vis, totalHours = hours });
                }
               // GridView1.DataSource = tsv;
            }
            DataBind();
        }
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            // ResolveClientUrl(HttpContext.Current.Request.UrlReferrer.ToString());
            string prevUrl = Request.Url.AbsoluteUri;
            Response.Redirect(prevUrl);
        }
    }
}