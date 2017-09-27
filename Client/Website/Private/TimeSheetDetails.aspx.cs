using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class TimeSheetDetails : PageBase
    {
        DOJobTimeSheet TimeSheet;
        bool NewEntry;
        bool AdminMode = false;
        DateTime MinDate;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentJob == null)
            {
                Response.Redirect(Constants.URL_Home);
            }
            

            //Get edited time sheet or create new.
            TimeSheet = CurrentSessionContext.CurrentTimeSheet;
            AdminMode = CurrentSessionContext.CurrentTimeSheetAdminMode;
            
            if (TimeSheet == null)
                TimeSheet = CurrentBRJob.CreateTimeSheet(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentJob.JobID);

            NewEntry = TimeSheet.PersistenceStatus == ObjectPersistenceStatus.New;
            MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-Constants.TimeSheetEntryDays);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void LoadForm()
        {
            txtComment.Text = TimeSheet.Comment;
            LoadDate();
            LoadTimeControl(ddlStartTime, TimeSheet.StartMinute);
            LoadTimeControl(ddlEndTime, TimeSheet.EndMinute);
        }

        private void SaveForm()
        {
            TimeSheet.Comment = txtComment.Text;
            TimeSheet.StartMinute = int.Parse(Request.Form[ddlStartTime.UniqueID]);
            TimeSheet.EndMinute = int.Parse(Request.Form[ddlEndTime.UniqueID]);
            if (AdminMode)
                TimeSheet.TimeSheetDate = dateDate.GetDate();
            else
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlDate.UniqueID]))
                    TimeSheet.TimeSheetDate = DateTime.ParseExact(Request.Form[ddlDate.UniqueID], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            }
        }
        protected void LoadTimeControl(DropDownList ddl, int SelectedTime)
        {
            ddl.Items.Clear();
            for (int time = 0; time < 60 * 24; time += 15)
            {
                ddl.Items.Add(new ListItem(string.Format("{0:D2}:{1:D2}", time / 60, time % 60), time.ToString()) { Selected = time == SelectedTime });
            }
            ddl.Items.Insert(0, new ListItem("Select...", "-1"));
        }

        protected void LoadDate()
        {
            phAdminDate.Visible = AdminMode;
            phDate.Visible = !AdminMode;
            if (AdminMode)
            {
                dateDate.SetDate(TimeSheet.TimeSheetDate);
            }
            else
            {
                ddlDate.Items.Clear();
                if (TimeSheet.TimeSheetDate != DateAndTime.NoValueDate && TimeSheet.TimeSheetDate < MinDate)
                {
                    ddlDate.Items.Add(new ListItem(TimeSheet.TimeSheetDate.ToString("ddd dd MMM yyyy"), ""));
                    ddlDate.Enabled = false;
                }
                else
                {

                    for (DateTime d = MinDate; d < DateTime.Now; d = d.AddDays(1))
                    {
                        ddlDate.Items.Add(new ListItem(d.ToString("ddd dd MMM yyyy"), d.ToString("yyyyMMdd")));
                    }
                    if (TimeSheet.TimeSheetDate != DateAndTime.NoValueDate)
                    {
                        ListItem Selected = ddlDate.Items.FindByValue(TimeSheet.TimeSheetDate.ToString("yyyyMMdd"));
                        if (Selected != null)
                            Selected.Selected = true;
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_JobDetails);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveForm();
                CurrentBRJob.SaveTimeSheet(TimeSheet);
                ShowMessage("Saved successfully.", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}