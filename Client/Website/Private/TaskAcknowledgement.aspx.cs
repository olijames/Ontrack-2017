using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class TaskAcknowledgement : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentJob == null || CurrentSessionContext.CurrentTask == null)
            {
                Response.Redirect(Constants.URL_Home);
            }
            //if (CurrentSessionContext.CurrentTask.TaskType != Framework.DataObjects.DOTask.TaskTypeEnum.Acknowledgement)
            //{
            //    Response.Redirect(Constants.URL_JobSummary);
            //}

            litTaskName.Text = CurrentSessionContext.CurrentTask.TaskName;
            litTaskDescription.Text = CurrentSessionContext.CurrentTask.Description;
        }

        protected void btnAcknowledge_Click(object sender, EventArgs e)
        {
            DOTaskAcknowledgement TA = CurrentBRJob.CreateTaskAcknowledgement(CurrentSessionContext.CurrentTask.TaskID,
                CurrentSessionContext.Owner.ContactID);
            CurrentBRJob.SaveTaskAcknowledgement(TA);
            CurrentSessionContext.CurrentTask = null;
            Response.Redirect(Constants.URL_JobSummary);
        }
    }
}