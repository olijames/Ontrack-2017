using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using System.Text;

namespace Electracraft.Client.Website.UserControls
{
    public partial class JobQuoteStatus : UserControlBase
    {
        public DOJob Job { get; set; }
        public DOJobQuote Quote { get; set; }


        public void Page_Load(object sender, EventArgs e)
        {
            if (Job == null) return;
            GetQuote();
        }

        void GetQuote()
        {
            List<DOBase> Quotes = ParentPage.CurrentBRJob.SelectJobQuotes(Job.JobID);
            //Only use most recent quote.
            if (Quotes.Count == 0)
            {
                Quote = null;
            }
            else if (Quotes.Count == 1)
            {
                Quote = Quotes[0] as DOJobQuote;
            }
            else if (Quotes.Count > 1)
            {
                var Latest = from DOJobQuote q in Quotes
                             orderby q.CreatedDate descending
                             select q;
                Quote = Latest.First<DOJobQuote>();
            }

        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            ShowQuoteStatus();
            ShowQuoteSubmitted();
            ShowQuoteTasks();

        }

        void ShowQuoteStatus()
        {
            if (Quote == null)
            {
                phQuoteStatus.Visible = false;
                return;
            }
            phQuoteStatus.Visible = true;
            litQuoteStatus.Text = Quote.QuoteStatus.ToString();
            litQuoteStatusDate.Text = DateAndTime.DisplayShortDate(Quote.QuoteStatusDate);

            decimal Labour, Materials;
            ParentPage.CurrentBRJob.GetJobTaskQuotedAmount(Job.JobID, out Labour, out Materials);

            //Apply margin for customer view.
            if (Quote.Margin != 0)
            {
                Labour = (Labour * (100 + Quote.Margin)) / 100;
                Materials = (Materials * (100 + Quote.Margin)) / 100;
            }
            Labour = ParentPage.CurrentBRGeneral.ApplyGST(Labour);
            Materials = ParentPage.CurrentBRGeneral.ApplyGST(Materials);

            litQuoteAmountWithMarginStatus.Text = string.Format("{0:C} incl. GST ({1:C} Labour, {2:C} Materials)", Labour + Materials, Labour, Materials);
            litTandCStatus.Text = Quote.TermsAndConditions.Replace("\r\n", "<br />"); 
        }

        void ShowQuoteSubmitted()
        {
            if (Quote != null && Quote.QuoteStatus == DOJobQuote.JobQuoteStatus.Quoted && IsCustomer())
            {
                phQuoteApprove.Visible = true;
                decimal Labour, Materials;
                ParentPage.CurrentBRJob.GetJobTaskQuotedAmount(Job.JobID, out Labour, out Materials);

                //Apply margin for customer view.
                if (Quote.Margin != 0)
                {
                    Labour = (Labour * (100 + Quote.Margin)) / 100;
                    Materials = (Materials * (100 + Quote.Margin)) / 100;
                }
                Labour = ParentPage.CurrentBRGeneral.ApplyGST(Labour);
                Materials = ParentPage.CurrentBRGeneral.ApplyGST(Materials);
                

                litQuoteAmountWithMarginApprove.Text = string.Format("{0:C} incl. GST ({1:C} Labour, {2:C} Materials)", Labour + Materials, Labour, Materials);
                litTandCApprove.Text = Quote.TermsAndConditions.Replace("\r\n", "<br />");
            }
            else
            {
                phQuoteApprove.Visible = false;
            }
        }

        void ShowQuoteTasks()
        {
            if ((Quote == null || Quote.QuoteStatus == DOJobQuote.JobQuoteStatus.Declined) && IsJobOwner())
            {
                phQuotedTasks.Visible = true;
                string TermsAndConditions;
                int Accepted;
                int NotAccepted;
                GetAcceptedTaskCount(out Accepted, out NotAccepted, out TermsAndConditions);
                litQuotedTaskCount.Text = Accepted.ToString();
                decimal Labour, Materials;
                ParentPage.CurrentBRJob.GetJobTaskQuotedAmount(Job.JobID, out Labour, out Materials);
                litQuoteTotalAmount.Text = string.Format("{0:C} ({1:C} Labour, {2:C} Materials)", Labour + Materials, Labour, Materials);

                cbeSubmitQuote.Enabled = (NotAccepted > 0);
                if (!IsPostBack)
                {
                    txtTermsAndConditions.Text = TermsAndConditions;
                }
            }
            else
            {
                phQuotedTasks.Visible = false;
            }
        }

        private bool IsJobOwner()
        {
            if (Job.JobOwner == ParentPage.CurrentSessionContext.Owner.ContactID) return true;
            if (ParentPage.CurrentBRContact.CheckCompanyContact(Job.JobOwner, ParentPage.CurrentSessionContext.Owner.ContactID)) return true;
            return false;
        }

        private bool IsCustomer()
        {
            //if (IsJobOwner()) return true;
            DOSite site = ParentPage.CurrentBRSite.SelectSite(Job.SiteID);
            if (site.ContactID == ParentPage.CurrentSessionContext.Owner.ContactID) return true;
            if (ParentPage.CurrentBRContact.CheckCompanyContact(site.ContactID, ParentPage.CurrentSessionContext.Owner.ContactID)) return true;
            return false;
        }

        private void GetAcceptedTaskCount(out int Accepted, out int NotAccepted, out string TandC)
        {
            List<DOBase> TaskQuotes = ParentPage.CurrentBRJob.SelectTaskQuotes(Job.JobID, false);
            Accepted = 0;
            NotAccepted = 0;
            StringBuilder tc = new StringBuilder();

            foreach (DOTaskQuote tq in TaskQuotes)
            {
                if (tq.Status == DOTaskQuote.TaskQuoteStatus.Accepted)
                {
                    DOTask task = ParentPage.CurrentBRJob.SelectTask(tq.TaskID);
                    DOContact TaskContact = ParentPage.CurrentBRContact.SelectContact(task.ContractorID);

                    Accepted++;
                    tc.Append(TaskContact.DisplayName + " Terms and Conditions:\r\n" + tq.TermsAndConditions + "\r\n\r\n");
                }
                else 
                {
                    NotAccepted++;
                }
            }
            TandC = tc.ToString();
            
        }

        protected void btnSubmitQuote_Click(object sender, EventArgs e)
        {
            decimal Margin;
            if (string.IsNullOrEmpty(txtQuoteMarginPercent.Text))
            {
                Margin = 0;
            }
            else if (!decimal.TryParse(txtQuoteMarginPercent.Text, out Margin))
            {
                ParentPage.ShowMessage("Enter a valid margin percentage.");
                return;
            }
            string TermsAndConditions;
            int Accepted;
            int NotAccepted;
            GetAcceptedTaskCount(out Accepted, out NotAccepted, out TermsAndConditions);
            if (Accepted == 0)
            {
                ParentPage.ShowMessage("The quote cannot be submitted unless there is at least one task with an accepted quote.", PageBase.MessageType.Error);
                return;
            }
            TermsAndConditions = txtTermsAndConditions.Text;
            if(string.IsNullOrEmpty(TermsAndConditions))
            {
                ParentPage.ShowMessage("You must enter terms and conditions", PageBase.MessageType.Error);
                return;
            }

            DOJobQuote jq = ParentPage.CurrentBRJob.CreateJobQuote(ParentPage.CurrentSessionContext.Owner.ContactID, Job.JobID);
            jq.Margin = Margin;
            jq.TermsAndConditions = TermsAndConditions;
            ParentPage.CurrentBRJob.SaveJobQuote(jq);
            ParentPage.ShowMessage("The quote has been submitted.");

            //Notify customer by email that quote was submitted.
            DOCustomer customer = ParentPage.CurrentBRContact.SelectSiteCustomer(Job.SiteID, Job.JobOwner);
            if (customer != null)
            {
                DOContact CustomerContact = ParentPage.CurrentBRContact.SelectContact(customer.ContactID);
                DOContact JobOwner = ParentPage.CurrentBRContact.SelectContact(Job.JobOwner);

                string Body = "Job Name: " + Job.Name + "<br />";
                Body += "Quote Submitted By: " + JobOwner.DisplayName + "<br />";
                Body += "<br /><a href=\"" + ParentPage.CurrentBRGeneral.SelectWebsiteBasePath() + "/private/JobDetails.aspx?jobid=" + Job.JobID.ToString() + "\">View Job</a>";
                if (Constants.EMAIL__TESTMODE)
                {
                    Body += "<br/><br/>";
                    Body += "Sender: " + ParentPage.CurrentSessionContext.CurrentContact.DisplayName + "<br />";
                    Body += "Sender logged in as: " + ParentPage.CurrentSessionContext.Owner.DisplayName + " (" + ParentPage.CurrentSessionContext.Owner.UserName + ")<br />";
                    Body += "Recipient: " + CustomerContact.DisplayName + " (" + CustomerContact.Email + ") <br />";
                    //Body += "Job ID: " + Job.JobNumberAuto;
                }

                ParentPage.CurrentBRGeneral.SendEmail("no-reply@ontrack.co.nz", CustomerContact.Email, JobOwner.DisplayName + " has submitted a quote for your job on Ontrack", Body);

            }

            Response.Redirect(Constants.URL_JobSummary);
        }

        protected void btnApproveQuote_Click(object sender, EventArgs e)
        {
            Quote.QuoteStatus = DOJobQuote.JobQuoteStatus.Accepted;
            Quote.QuoteStatusDate = DateAndTime.GetCurrentDateTime();
            ParentPage.CurrentBRJob.SaveJobQuote(Quote);

            //Change the job type to quoted.
            Job.JobType = DOJob.JobTypeEnum.Quoted;
            ParentPage.CurrentBRJob.SaveJob(Job);

            //Log that a quote was accepted.
            DOJobChange Change = ParentPage.CurrentBRJob.CreateJobChange(Job.JobID, DOJobChange.JobChangeType.QuoteAccepted, ParentPage.CurrentSessionContext.Owner.ContactID);
            ParentPage.CurrentBRJob.SaveJobChange(Change);
            ParentPage.CurrentSessionContext.CurrentJob = Job;

            //Copy all quoted materials and labour to required.
            CopyQuotedToRequired(Job);

            //Redirect to update job details.
            Response.Redirect(Constants.URL_JobSummary);



        }

        protected void btnDeclineQuote_Click(object sender, EventArgs e)
        {
            Quote.QuoteStatus = DOJobQuote.JobQuoteStatus.Declined;
            Quote.QuoteStatusDate = DateAndTime.GetCurrentDateTime();
            ParentPage.CurrentBRJob.SaveJobQuote(Quote);
        }




        protected void CopyQuotedToRequired(DOJob Job)
        {
            List<DOBase> Tasks = ParentPage.CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty, false);
            foreach (DOTask t in Tasks)
            {
                List<DOBase> TMs = ParentPage.CurrentBRJob.SelectTaskMaterials(t.TaskID);
                List<DOBase> TLs = ParentPage.CurrentBRJob.SelectTaskLabour(t.TaskID);

                foreach (DOTaskMaterial tm in TMs)
                {
                    if (tm.MaterialType == TaskMaterialType.Quoted)
                    {
                        DOTaskMaterial NewTM = ParentPage.CurrentBRJob.CreateTaskMaterial(t.TaskID, tm.MaterialID, tm.CreatedBy);
                        NewTM.Active = tm.Active;
                        NewTM.Description = tm.Description;
                        NewTM.MaterialType = TaskMaterialType.Required;
                        NewTM.Quantity = tm.Quantity;
                        ParentPage.CurrentBRJob.SaveTaskMaterial(NewTM);
                    }
                }

                foreach (DOTaskLabour tl in TLs)
                {
                    if (tl.LabourType == TaskMaterialType.Quoted)
                    {
                        DOTaskLabour NewTL = ParentPage.CurrentBRJob.CreateTaskLabour(t.TaskID, tl.LabourID, tl.CreatedBy);
                        NewTL.Active = tl.Active;
                        NewTL.Chargeable = tl.Chargeable;
                        NewTL.ContactID = tl.ContactID;
                        NewTL.EndMinute = tl.EndMinute;
                        NewTL.LabourDate = tl.LabourDate;
                        NewTL.LabourType = TaskMaterialType.Required;
                        NewTL.InvoiceQuantity = tl.InvoiceQuantity;
                        NewTL.StartMinute = tl.StartMinute;
                        NewTL.TaskLabourCategoryID = tl.TaskLabourCategoryID;
                        ParentPage.CurrentBRJob.SaveTaskLabour(NewTL);
                    }
                }
            }
        }

    }
}