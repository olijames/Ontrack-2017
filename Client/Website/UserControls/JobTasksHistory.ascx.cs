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
    public partial class JobTasksHistory : UserControlBase
    {
        public DOJob Job { get; set; }
        public List<DOBase> TaskList { get; set; }
        public bool AmendedTasksIncluded { get; set; }
        private List<DOBase> FullTaskList;
        public bool ShowFull = false;

        bool JobOwner = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Job != null)
            {
                JobOwner = ParentPage.CurrentSessionContext.Owner.ContactID == Job.JobOwner ||
                    ParentPage.CurrentBRContact.CheckCompanyContact(Job.JobOwner, ParentPage.CurrentSessionContext.Owner.ContactID);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> Tasks = TaskList;
            //Reverse order for history.
            Tasks.Reverse();

            if (!AmendedTasksIncluded && Tasks != null && Tasks.Count > 0)
            {
                FullTaskList = ParentPage.CurrentBRJob.SelectTasks(((DOTask)Tasks[0]).JobID);

                if (ShowFull)
                {
                    //For each task. Insert amended tasks before it in the list.
                    for (int i = Tasks.Count - 1; i >= 0; i--)
                    {
                        DOTask CheckTask = Tasks[i] as DOTask;
                        DOTask AmendedTask = GetAmendedTask(CheckTask, FullTaskList);
                        while (AmendedTask != null)
                        {
                            Tasks.Insert(i, AmendedTask);
                            AmendedTask = GetAmendedTask(AmendedTask, FullTaskList);
                        }
                    }
                }
            }
            phNoTasks.Visible = (Tasks == null || Tasks.Count == 0);
            rpTasks.DataSource = Tasks;
            rpTasks.DataBind();

        }

        protected bool HistoryVisible(Guid TaskID)
        {
            if (FullTaskList == null) return false;
            foreach (DOTask Task in FullTaskList)
            {
                if (Task.AmendedTaskID == TaskID)
                    return true;
            }
            return false;
        }

        private DOTask GetAmendedTask(DOTask CheckTask, List<DOBase> FullTaskList)
        {
            var a = from DOTask t in FullTaskList
                    where t.AmendedTaskID == CheckTask.TaskID
                    select t;

            return a.SingleOrDefault<DOTask>();
        }

        private Dictionary<Guid, List<DOBase>> _TaskMaterials = new Dictionary<Guid, List<DOBase>>();

        protected List<DOBase> GetTaskMaterials(DOTask Task)
        {
            if (!_TaskMaterials.ContainsKey(Task.TaskID))
                _TaskMaterials.Add(Task.TaskID, ParentPage.CurrentBRJob.SelectTaskMaterials(Task.TaskID));
            return _TaskMaterials[Task.TaskID];
        }

        private Dictionary<Guid, List<DOBase>> _TaskLabour = new Dictionary<Guid, List<DOBase>>();
        protected List<DOBase> GetTaskLabour(DOTask Task)
        {
            if (!_TaskLabour.ContainsKey(Task.TaskID))
                _TaskLabour.Add(Task.TaskID, ParentPage.CurrentBRJob.SelectTaskLabour(Task.TaskID));
            return _TaskLabour[Task.TaskID];
        }

        protected decimal GetQuoteLabour(DOTask Task)
        {
            DOTaskQuote Quote = GetQuote(Task);
            if (Quote == null) return 0;

            decimal total = 0;
            foreach (DOTaskLabour tl in GetTaskLabour(Task))
            {
                if (!tl.Chargeable) continue;
                if (!tl.Active) continue;
                if (tl.LabourType != TaskMaterialType.Quoted) continue;

                decimal linetotal = ((decimal)(tl.EndMinute - tl.StartMinute) * tl.LabourRate) / 60;
                if (JobOwner)
                    linetotal = (linetotal * (100 + Quote.Margin)) / 100;
                total += linetotal;
            }

            return total;
        }


        private Dictionary<Guid, DOMaterial> Materials = new Dictionary<Guid, DOMaterial>();
        protected DOMaterial GetMaterial(Guid MaterialID)
        {
            if (!Materials.ContainsKey(MaterialID))
                Materials.Add(MaterialID, ParentPage.CurrentBRJob.SelectMaterial(MaterialID));
            return Materials[MaterialID];
        }

        protected decimal GetQuoteMaterial(DOTask Task)
        {
            DOTaskQuote Quote = GetQuote(Task);
            if (Quote == null) return 0;
            decimal total = 0;
            foreach (DOTaskMaterial tm in GetTaskMaterials(Task))
            {
                if (!tm.Active) continue;
                if (tm.MaterialType != TaskMaterialType.Quoted) continue;

                decimal linetotal = tm.SellPrice * tm.Quantity;
                if (JobOwner)
                    linetotal = (linetotal * (100 + Quote.Margin)) / 100;
                total += linetotal;

            }
            return total;
        }

        protected decimal GetQuoteTotal(DOTask Task)
        {
            return GetQuoteLabour(Task) + GetQuoteMaterial(Task);
        }

        List<Guid> CheckedQuotes = new List<Guid>();
        Dictionary<Guid, DOTaskQuote> Quotes = new Dictionary<Guid, DOTaskQuote>();
        protected DOTaskQuote GetQuote(DOTask Task)
        {

            if (!Quotes.ContainsKey(Task.TaskID) && !CheckedQuotes.Contains(Task.TaskID))
                Quotes.Add(Task.TaskID, ParentPage.CurrentBRJob.SelectTaskQuoteByTask(Task.TaskID));

            if (Quotes.ContainsKey(Task.TaskID))
                return Quotes[Task.TaskID];
            else
                return null;
        }
        protected string GetQuoteStatus(DOTask Task)
        {
            DOTaskQuote tq = GetQuote(Task);
            if (tq == null) return string.Empty;
            return tq.Status.ToString();
        }

        protected bool QuoteVisible(DOTask Task)
        {
            //return GetQuote(Task) != null;

            Guid ContactID = ParentPage.CurrentSessionContext.Owner.ContactID;

            bool QuoteVisible = false;
            //Only job owner, task owner or task contractor can see quote.
            if (JobOwner)
                QuoteVisible = true;
            if (!QuoteVisible && Task.ContractorID == ContactID || ParentPage.CurrentBRContact.CheckCompanyContact(Task.ContractorID, ContactID))
                QuoteVisible = true;
            if (!QuoteVisible && Task.TaskOwner == ContactID || ParentPage.CurrentBRContact.CheckCompanyContact(Task.TaskOwner, ContactID))
                QuoteVisible = true;

            if (QuoteVisible)
            {
                return GetQuote(Task) != null;
            }
            else
            {
                return false;
            }
        }

        protected bool LMSplitVisible(DOTask Task)
        {
            Guid ContactID = ParentPage.CurrentSessionContext.Owner.ContactID;

            bool SplitVisible = false;
            //Only task contractor can see split.
            if (Task.ContractorID == ContactID || ParentPage.CurrentBRContact.CheckCompanyContact(Task.ContractorID, ContactID))
                SplitVisible = true;

            return SplitVisible;
        }

        protected bool CanDecline(DOTask Task)
        {
            if (Job == null) return false;
            if (Job.JobType == DOJob.JobTypeEnum.Quoted) return false;

            //only job owner can accept.
            if (Job.JobOwner == ParentPage.CurrentSessionContext.Owner.ContactID ||
                ParentPage.CurrentBRContact.CheckCompanyContact(Job.JobOwner, ParentPage.CurrentSessionContext.Owner.ContactID))
            {
                DOTaskQuote tq = GetQuote(Task);
                return (tq != null && tq.Status != DOTaskQuote.TaskQuoteStatus.Declined);
            }
            else
            {
                return false;
            }
        }


        protected bool CanAccept(DOTask Task)
        {
            if (Job == null) return false;
            if (Job.JobType == DOJob.JobTypeEnum.Quoted) return false;

            //only job owner can accept.
            if (Job.JobOwner == ParentPage.CurrentSessionContext.Owner.ContactID ||
                ParentPage.CurrentBRContact.CheckCompanyContact(Job.JobOwner, ParentPage.CurrentSessionContext.Owner.ContactID))
            {
                DOTaskQuote tq = GetQuote(Task);
                return (tq != null && tq.Status != DOTaskQuote.TaskQuoteStatus.Accepted);
            }
            else
            {
                return false;
            }
        }

        protected string GetContactName(Guid ContactID, Guid TaskID)
        {
            if (ContactID == Constants.Guid_DefaultUser)
            {
                DOTaskPendingContractor TPC = ParentPage.CurrentBRJob.SelectTaskPendingContractor(TaskID);
                return TPC == null ? string.Empty : TPC.ContractorEmail + " (pending)";
            }
            else
            {
                DOContact Contact = ParentPage.CurrentBRContact.SelectContact(ContactID);
                if (Contact != null)
                    return Contact.DisplayName;
            }
            return string.Empty;
        }

        protected void btnDeclineQuote_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid TaskID = new Guid(b.CommandArgument);
            DOTaskQuote tq = ParentPage.CurrentBRJob.SelectTaskQuoteByTask(TaskID);

            tq.Status = DOTaskQuote.TaskQuoteStatus.Declined;

            ParentPage.CurrentBRJob.SaveTaskQuote(tq);
            Response.Redirect(Constants.URL_JobSummary);

        }

        protected void btnAcceptQuote_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid TaskID = new Guid(b.CommandArgument);
            DOTaskQuote tq = ParentPage.CurrentBRJob.SelectTaskQuoteByTask(TaskID);
            decimal Margin = 0;
            string strMargin = Request.Form["txtTaskMargin" + TaskID.ToString()];
            if (!string.IsNullOrEmpty(strMargin))
            {
                if (!decimal.TryParse(strMargin, out Margin))
                {
                    ParentPage.ShowMessage("Margin must be a valid number.", PageBase.MessageType.Error);
                    return;
                }
            }

            tq.Status = DOTaskQuote.TaskQuoteStatus.Accepted;
            tq.Margin = Margin;

            ParentPage.CurrentBRJob.SaveTaskQuote(tq);
            Response.Redirect(Constants.URL_JobSummary);
        }
        protected void btnDeleteTask_Click(object sender, EventArgs e)
        {
            try
            {
                string strTaskID = ((Button)sender).CommandArgument.ToString();
                Guid TaskID = new Guid(strTaskID);

                DOTask DeleteTask = null;
                foreach (DOTask Task in TaskList)
                {
                    if (Task.TaskID == TaskID)
                    {
                        DeleteTask = Task;
                        break;
                    }
                }

                if (DeleteTask != null)
                {
                    ParentPage.CurrentBRJob.DeleteTask(DeleteTask);
                    TaskList.Remove(DeleteTask);
                }
            }
            catch (Exception ex)
            {
                ParentPage.ShowMessage("The task could not be deleted.<br />" + ex.Message, PageBase.MessageType.Error);
            }
        }

        /// <summary>
        /// TODO: cache this or combine material name into same query as task amterials
        /// </summary>
        /// <param name="MaterialID"></param>
        protected string GetMaterialName(Guid MaterialID)
        {
            DOMaterial Material = ParentPage.CurrentBRJob.SelectMaterial(MaterialID);
            return Material.MaterialName;
        }

        protected void btnViewTask_Click(object sender, EventArgs e)
        {
            string strTaskID = ((LinkButton)sender).CommandArgument.ToString();
            Guid TaskID = new Guid(strTaskID);
            ParentPage.CurrentSessionContext.CurrentTask = ParentPage.CurrentBRJob.SelectTask(TaskID);
            Response.Redirect(Constants.URL_TaskSummary);
        }

        protected void btnViewTaskHistory_Click(object sender, EventArgs e)
        {
            string strTaskID = ((Button)sender).CommandArgument.ToString();
            Guid TaskID = new Guid(strTaskID);
            ParentPage.CurrentSessionContext.CurrentTask = ParentPage.CurrentBRJob.SelectTask(TaskID);
            Response.Redirect(Constants.URL_TaskHistory);
        }

        //protected void btnAddSubTask_Click(object sender, EventArgs e)
        //{
        //    Button b = sender as Button;
        //    if (b.CommandName == "AddSubTask")
        //    {
        //        DOTask Task = ParentPage.CurrentBRJob.SelectTask(new Guid(b.CommandArgument.ToString()));
        //        ParentPage.CurrentSessionContext.CurrentTask = Task;
        //        Response.Redirect(Constants.URL_TaskDetails);
        //    }
        //}

        //protected List<DOBase> GetChildTasks(DOTask Task)
        //{
        //    return ParentPage.CurrentBRJob.SelectTasks(Task.JobID, Task.TaskID);
        //}

        //protected void rpTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    List<DOBase> ChildTasks = GetChildTasks(e.Item.DataItem as DOTask);
        //    PlaceHolder phSubTasks = e.Item.FindControl("phSubTasks") as PlaceHolder;
        //    if (phSubTasks != null && ChildTasks.Count > 0)
        //    {
        //        JobTasks ctlSubTasks = LoadControl("~/UserControls/JobTasks.ascx") as JobTasks;
        //        ctlSubTasks.TaskList = ChildTasks;
        //        phSubTasks.Controls.Add(ctlSubTasks);
        //    }
        //}
    }
}