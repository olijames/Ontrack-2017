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
    public partial class TaskHistory : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentTask == null)
            {
                Response.Redirect(Constants.URL_JobSummary);
            }
            List<DOBase> Tasks = new List<DOBase>();
            Tasks.Add(CurrentSessionContext.CurrentTask);
            TaskList.TaskList = Tasks;
        }
    }
}