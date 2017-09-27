using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class CompanyClose : PageBase
    {
        protected DOContact Company;
        bool Outstanding;
        List<DOBase> Jobs;
        List<DOBase> Tasks;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCompanyOwner();

            CheckOutstanding();

            if (Outstanding)
            {
                ShowOutstanding();
            }
            else
            {
                phConfirm.Visible = true;
                phOutstanding.Visible = false;
            }
        }

        private void ShowOutstanding()
        {
            phOutstanding.Visible = true;
            phConfirm.Visible = false;

            if (Jobs.Count > 0)
            {
                phJobs.Visible = true;
                rpJobs.DataSource = Jobs;
                rpJobs.DataBind();
            }
            else
            {
                phJobs.Visible = false;
            }

            if (Tasks.Count > 0)
            {
                phTasks.Visible = true;
                rpTasks.DataSource = Tasks;
                rpTasks.DataBind();
            }
        }

        private void CheckOutstanding()
        {
            Jobs = CurrentBRJob.SelectActiveJobs(Company.ContactID);
            Tasks = CurrentBRJob.SelectActiveTasks(Company.ContactID);
            Outstanding = (Jobs.Count > 0 || Tasks.Count > 0);
            
        }

        private void CheckCompanyOwner()
        {
            if (CurrentSessionContext.CurrentContact == null || CurrentSessionContext.CurrentContact.ContactType != DOContact.ContactTypeEnum.Company)
            {
                Response.Redirect(Constants.URL_Home);
            }
            //Make sure company is latest db version.
            Company = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
            if (CurrentSessionContext.Owner.ContactID != Company.ManagerID)
            {
                Response.Redirect(Constants.URL_Home);
            }
        }

        protected DOJob GetJob(DOTask task)
        {
            DOJob job = CurrentBRJob.SelectJob(task.JobID);
            return job;
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            //De link owner.
			Guid contactCompanyID= CurrentBRContact.SelectContactCompany(CurrentSessionContext.Owner.ContactID, Company.ContactID);
	        DOContactCompany cc = CurrentBRContact.SelectContactCompany(contactCompanyID);
            CurrentBRContact.DeleteContactCompany(cc);

            Company.Active = false;
            CurrentBRContact.SaveContact(Company);

            Response.Redirect(Constants.URL_Home);
        }
    }
}