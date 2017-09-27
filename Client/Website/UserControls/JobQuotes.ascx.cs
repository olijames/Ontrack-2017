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
    public partial class JobQuotes : UserControlBase
    {
        public DOJob Job { get; set; }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid QuoteID = new Guid(b.CommandArgument.ToString());
            //Accept this quote.
            DOJobQuote AcceptQuote = ParentPage.CurrentBRJob.SelectJobQuote(QuoteID);
            AcceptQuote.QuoteStatus = DOJobQuote.JobQuoteStatus.Accepted;
            ParentPage.CurrentBRJob.SaveQuote(AcceptQuote);

            //Decline all other quotes.
            List<DOBase> Quotes = ParentPage.CurrentBRJob.SelectJobQuotes(AcceptQuote.JobID);
            foreach (DOJobQuote Quote in Quotes)
            {
                if (Quote.QuoteStatus == DOJobQuote.JobQuoteStatus.Quoted)
                {
                    Quote.QuoteStatus = DOJobQuote.JobQuoteStatus.Declined;
                    ParentPage.CurrentBRJob.SaveQuote(Quote);
                }
            }

            //Change the job type to quoted.
            Job = ParentPage.CurrentBRJob.SelectJob(AcceptQuote.JobID);
            Job.JobType = DOJob.JobTypeEnum.Quoted;
            ParentPage.CurrentBRJob.SaveJob(Job);

            //Log that a quote was accepted.
            DOJobChange Change = ParentPage.CurrentBRJob.CreateJobChange(Job.JobID, DOJobChange.JobChangeType.QuoteAccepted, ParentPage.CurrentSessionContext.Owner.ContactID);
            ParentPage.CurrentBRJob.SaveJobChange(Change);
            ParentPage.CurrentSessionContext.CurrentJob = Job;

            //Copy all quoted materials and labour to required.
            CopyQuotedToRequired(Job);

            //Redirect to update job details.
            Response.Redirect(Constants.URL_JobDetails);
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
                        NewTL.LabourType =  TaskMaterialType.Required;
                        NewTL.Quantity = tl.Quantity;
                        NewTL.StartMinute = tl.StartMinute;
                        NewTL.TaskLabourCategoryID = tl.TaskLabourCategoryID;
                        ParentPage.CurrentBRJob.SaveTaskLabour(NewTL);
                    }
                }
            }
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid QuoteID = new Guid(b.CommandArgument.ToString());
            DOJobQuote Quote = ParentPage.CurrentBRJob.SelectJobQuote(QuoteID);
            Quote.QuoteStatus = DOJobQuote.JobQuoteStatus.Declined;
            ParentPage.CurrentBRJob.SaveQuote(Quote);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> Quotes = ParentPage.CurrentBRJob.SelectJobQuotes(Job.JobID);
            rpQuotes.DataSource = Quotes;
            rpQuotes.DataBind();
        }


    }
}