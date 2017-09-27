﻿using System;
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
    public partial class JobTasks : UserControlBase
    {
        struct TaskListForRepeater
        {
         public   DOTask task { get; set; }
            public bool visible { get; set; }
            public bool visibleComp { get; set; }
            public bool enabledCompBtn { get; set; }
            public bool enabledInvBtn { get; set; }
            public bool enabledPaidBtn { get; set; }
            public string iconComp { get; set; }
            public string iconInv { get; set; }
            public string iconPaid { get; set; }
            public string taskClass { get; set; }
            public string taskStartDate { get; set; }
            public string taskEndDate { get; set; }
        }

        public DOJob Job { get; set; }
        public List<DOBase> TaskList { get; set; }
        public bool AmendedTasksIncluded { get; set; }
        private List<DOBase> FullTaskList;
        public bool ShowFull = false;
        public string JobNumberAuto, TaskNumber;
        bool JobOwner = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Error.Visible = false;
            if (Job != null)
            {
                JobOwner = ParentPage.CurrentSessionContext.Owner.ContactID == Job.JobOwner ||
                    ParentPage.CurrentBRContact.CheckCompanyContact(Job.JobOwner, ParentPage.CurrentSessionContext.Owner.ContactID);
            }

        }


        //added jared
        protected void btnAddTask_Click(object sender, EventArgs e)
        {

            Response.Redirect(Constants.URL_TaskDetails);
        }
        protected void btnAddMaterials_Click(object sender, EventArgs e)
        {
            //Jared redirect to materialinput page
            //Response.Redirect(Constants.URL_TaskDetails);
            Response.Redirect("MaterialInput.aspx", false);
        }

       
          
       

        protected string GetTaskClass(string taskID)
        {
            DOTask task = ParentPage.CurrentBRJob.SelectTask(Guid.Parse(taskID));
            if (task.Status == DOTask.TaskStatusEnum.Complete)
            {
                return "grey";
            }
            else if (task.Status == DOTask.TaskStatusEnum.Invoiced)
            {
                return "redTask";
            }
            else if (task.Status == DOTask.TaskStatusEnum.Paid)
                return "green";
            else
                return "btn-3 text-left";
          
        }
        protected string GetTaskClass(DOTask task)
        {
            //DOTask task = ParentPage.CurrentBRJob.SelectTask(Guid.Parse(taskID));
            if (task.Status == DOTask.TaskStatusEnum.Complete)
            {
                return "grey";
            }
            else if (task.Status == DOTask.TaskStatusEnum.Invoiced)
            {
                return "redTask";
            }
            else if (task.Status == DOTask.TaskStatusEnum.Paid)
                return "green";
            else
                return "btn-3 text-left";
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            DOJobContractor dojc = ParentPage.CurrentBRJob.SelectJobContractor(Job.JobID, ParentPage.CurrentSessionContext.CurrentContact.ContactID);

            JobNumberAuto = dojc.JobNumberAuto.ToString();
            List<DOBase> Tasks = TaskList;
            List<DOBase> taskListForInvSys = new List<DOBase>();
            if (!AmendedTasksIncluded && Tasks != null && Tasks.Count > 0)
            {
                FullTaskList = ParentPage.CurrentBRJob.SelectAllTasks(((DOTask)Tasks[0]).JobID);
                //FindAllTasks(((DOTask)Tasks[0]).JobID);
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
            //taskListForInvSys = FullTaskList;
            //For each add only amended task in the list.
            if(FullTaskList!=null)
            for (int i = 0; i < FullTaskList.Count;i++)
            {
                DOTask CheckTask = FullTaskList[i] as DOTask;
                //DOTask AmendedTask = GetAmendedTask(CheckTask, FullTaskList);
                if (CheckTask.AmendedTaskID == Guid.Empty)
                {
                    taskListForInvSys.Add(CheckTask);
                }
                
                //while (AmendedTask != null)
                //{
                //    //Tasks.Insert(i, AmendedTask);
                //    taskListForInvSys.RemoveAt(i);
                //    //AmendedTask = GetAmendedTask(AmendedTask, FullTaskList);
                //}
            }
            List<TaskListForRepeater> finalTasks = new List<TaskListForRepeater>();
            
          foreach(DOTask etask in taskListForInvSys)
            {
                bool visibility = false;
                //bool visibilitComp=false;
                
                if (etask.Status == DOTask.TaskStatusEnum.Complete)
                {
                    visibility = IsCompleteVisible(etask);             
                }
                else
                {
                    visibility = IfMobile();
                }
                if (etask.Status == DOTask.TaskStatusEnum.Incomplete)
                {
                    if (Request.Browser.IsMobileDevice)
                        visibility = true;
                }
                bool enabledComp,enabledInv,enableP;
                enabledComp = IfCompleted_btn_enabled(etask);
                enabledInv = IsInvoicedEnabled(etask);
                enableP = IfPaidEnabled(etask);
                string iconC,iconI,iconP;
                iconC= GetIconForComplete_btn(etask);
                iconI= GetIconForInvoiced_btn(etask);
                iconP= GetIconForPaid_btn(etask);
                //if (etask.Status == DOTask.TaskStatusEnum.Complete)
                //{
                    
                //    iconClass = GetIconForComplete_btn(etask);
                //}
                //else if (etask.Status == DOTask.TaskStatusEnum.Invoiced)
                //{
                //    //enabledComp = IfCompleted_btn_enabled(etask);
                //    //enabledInv = IsInvoicedEnabled(etask);
                //    //enableP = IfPaidEnabled(etask);
                //    iconClass = GetIconForInvoiced_btn(etask);
                //}
                //else if (etask.Status == DOTask.TaskStatusEnum.Paid)
                //{
                //    //enabled = IfPaidEnabled(etask);
                //    iconClass = GetIconForPaid_btn(etask);
                //}
                //else if (etask.Status == DOTask.TaskStatusEnum.Incomplete)
                //{
                //    //enabled = IfPaidEnabled(etask);
                //    iconClass = GetIconForPaid_btn(etask);
                //}
                //else
                //{
                //    //enabled = false;
                //    iconClass = "fi-x";
                //}
                    //string cssClass;
                    //if (enabled)
                    //    cssClass = "button radius tiny";
                    //else
                    //    cssClass = "button secondary radius tiny";         
                string taskClass = GetTaskClass(etask);
                string sd= ParentPage.CurrentBRJob.GetTaskStartTimeText(etask);
                string ed= ParentPage.CurrentBRJob.GetTaskEndTimeText(etask);
                //etask.StartDate = DateTime.Parse(sd);
                ///etask.EndDate= DateTime.Parse(ed);
                finalTasks.Add(new TaskListForRepeater() { task = etask, visible = visibility,enabledCompBtn=enabledComp, enabledInvBtn = enabledInv,enabledPaidBtn=enableP,iconComp= iconC,iconInv=iconI,iconPaid=iconP, taskClass= taskClass,taskStartDate=sd,taskEndDate=ed });
               // }
                //if(task.Status<=DOTask.TaskStatusEnum.Complete)
                //{

                //}
            }
            //phNoTasks.Visible = (Tasks == null || Tasks.Count == 0);
            rpTaskList.DataSource = null;
            rpTaskList.DataSource = finalTasks;
            rpTaskList.DataBind();
            //rpTasks.DataSource = Tasks;
           // rpTasks.DataBind();
            //rpTaskList.DataBind();
        }

        //protected List<DOBase> FindAllTasks(Guid jobID)
        //{
        //    FullTaskList = ParentPage.CurrentBRJob.SelectAllTasks(((DOTask)Tasks[0]).JobID);
        //    return FullTaskList;
        //}

        public bool IfMobile()
        {
            if (!Request.Browser.IsMobileDevice)
            {
                return true;
            }
            else
                return false;
        }
        public string GetIconForComplete_btn(DOTask task)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Complete)
                    return "fi-check";
                else
                    return "fi-x";
            }
            else
            { 
                return string.Empty;
            }

        }
        public string GetIconForInvoiced_btn(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            if (task.Status >= DOTask.TaskStatusEnum.Invoiced)
                return "fi-check";
            else
                return "fi-x";
            }
            else
                return string.Empty;
        }
        public string GetIconForInvoiced_btn(DOTask task)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Invoiced)
                    return "fi-check";
                else
                    return "fi-x";
            }
            else
                return string.Empty;
        }
        public string GetIconForPaid_btn(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status == DOTask.TaskStatusEnum.Paid)
                    return "fi-check";
                else
                    return "fi-x";
            }
            else
                return string.Empty;
        }
        public string GetIconForPaid_btn(DOTask task)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status == DOTask.TaskStatusEnum.Paid)
                    return "fi-check";
                else
                    return "fi-x";
            }
            else
                return string.Empty;
        }
        public string GetTaskNumber(DOTask task)
        {
            string taskNumber = task.TaskNumber.ToString().PadLeft(3, '0');
            return taskNumber;
        }
        protected bool HistoryVisible(Guid TaskID)
        {
            if (FullTaskList == null) return false;
            foreach (DOTask Task in FullTaskList)
            {
                //if (Task.AmendedTaskID == TaskID)
                if (Task.AmendedTaskID == TaskID)
                return true;
            }
            return false;
        }
        protected bool InvoiceTaskVisible(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                ////if (task.Status == DOTask.TaskStatusEnum.Complete && task.TaskInvoiceStatus == 0)
                ////{
                ////    return true;
                ////}
                //else
                return false;
            }
            else
                return false;
        }
        protected bool IfCompleted_btn_enabled(DOTask task)
        {
            //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            //if (task.Status >= DOTask.TaskStatusEnum.Complete)
            //{
            //    if (task.Status == DOTask.TaskStatusEnum.Paid)
            //        return false;
            //    else if (task.Status == DOTask.TaskStatusEnum.Invoiced)
            //        return false;
            //    else return true;
            //}
            if (task.Status == DOTask.TaskStatusEnum.Incomplete)
                return true;
            else if (task.Status == DOTask.TaskStatusEnum.Complete)
                return true;
            else if (task.Status >= DOTask.TaskStatusEnum.Paid)
                return false;
            else if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                return false;
            else
                return true;
            //else
            //{
            //    return false;
            //}
        }
        protected bool IsCompleteVisible(DOTask task)
        {
            if (Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status == DOTask.TaskStatusEnum.Incomplete)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return true;
            }
        }
        protected string GetDisableButtonClass(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b.Enabled == false)
                return "button secondary radius small";
            else
                return "button radius small";
        }
        protected bool IfInvoiced(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        protected bool IsInvoicedEnabled(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Complete)
                {
                    if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                        return true;
                    else if (task.Status == DOTask.TaskStatusEnum.Paid)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        protected bool IsInvoicedEnabled(DOTask task)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Complete)
                {
                    if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                        return true;
                    else if (task.Status == DOTask.TaskStatusEnum.Paid)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        protected bool IfPaidEnabled(Guid taskID)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Invoiced)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        protected bool IfPaidEnabled(DOTask task)
        {
            if (!Request.Browser.IsMobileDevice)
            {
                //DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
                if (task.Status >= DOTask.TaskStatusEnum.Invoiced)
                    return true;
                else
                    return false;
            }
            else
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
                if (TPC != null)
                    return TPC.ContractorEmail + " (pending)";
                else return "";
            }
            else
            {
                DOContact Contact = ParentPage.CurrentBRContact.SelectContact(ContactID);
                if (Contact != null)
                    return Contact.DisplayName;
            }
            return string.Empty;
        }
        //To find trade category name
        protected string GetTradeCategoryName(Guid tradeCategoryID)
        {
            DOTradeCategory tradeCategory = ParentPage.CurrentBRTradeCategory.FindTradeCategoryName(tradeCategoryID);
            if (tradeCategory != null)
                return tradeCategory.TradeCategoryName;
            else
                return "";
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
                if (TaskList.Count == 0)
                {
                    if (ParentPage.CurrentSessionContext.CurrentJob != null)
                    {
                        ParentPage.CurrentSessionContext.CurrentJob.JobStatus = DOJob.JobStatusEnum.Complete;
                        ParentPage.CurrentBRJob.SaveJob(ParentPage.CurrentSessionContext.CurrentJob);
                    }
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

        protected void Complete_Btn_Click(object sender, EventArgs e)
        {
           
            Guid taskID = Guid.Parse(((Button)sender).CommandArgument);
            //DOTask task =(sender)
            DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            //task.TaskInvoiceStatus =1;
            //Can't complete task if no materials and no labour.
            if (task.Status == DOTask.TaskStatusEnum.Incomplete)
            {
                List<DOBase> TM = ParentPage.CurrentBRJob.SelectTaskMaterials(task.TaskID);
                List<DOBase> TL = ParentPage.CurrentBRJob.SelectTaskLabours(task.TaskID);
                bool HasActual = false;
                foreach (DOTaskMaterial myTm in TM)
                {
                    if (myTm.MaterialType == TaskMaterialType.Actual && myTm.Active)
                    {
                        HasActual = true;
                        break;
                    }
                }
                if (!HasActual)
                {
                    //jared edited !here!   was:   foreach (DOTaskLabour myTL in TL)
                    foreach (DOTaskLabourInfo myTL in TL)
                    {
                        if (myTL.LabourType == TaskMaterialType.Actual && myTL.Active)
                        {
                            HasActual = true;
                            break;
                        }
                    }
                }
                if (!HasActual)
                {
                    //ParentPage.ShowMessage("The task '" + task.TaskName + "' cannot be completed because no materials or labour have been assigned.");
                    Error.Text = "The task '" + task.TaskName + "' cannot be completed because no materials or labour have been assigned.";
                    Error.Visible = true;

                    return;
                }
                else
                {
                    if (task.Status == DOTask.TaskStatusEnum.Incomplete)
                    {
                        DOTaskCompletion TC = ParentPage.CurrentBRJob.CreateTaskCompletion(task.TaskID, ParentPage.CurrentSessionContext.Owner.ContactID);
                        DOTaskCompletion TaskCompleted = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                        if (TaskCompleted == null)
                            ParentPage.CurrentBRJob.SaveTaskCompletion(TC);
                        else
                        {
                            ParentPage.CurrentBRJob.DeleteTaskCompletion(TaskCompleted);
                            ParentPage.CurrentBRJob.SaveTaskCompletion(TC);
                        }
                        task.Status = DOTask.TaskStatusEnum.Complete;
                    }
                    else
                    {
                        DOTaskCompletion TC = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                        ParentPage.CurrentBRJob.DeleteTaskCompletion(TC);
                        task.Status = DOTask.TaskStatusEnum.Incomplete;
                    }

                    ParentPage.CurrentBRJob.SaveTask(task);
                    UpdateRecords(task.JobID,task.SiteID,task.TaskCustomerID);
                    //if (task.Status == DOTask.TaskStatusEnum.Complete)
                    //{
                    //    List<DOBase> JobTasks = ParentPage.CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty);
                    //    bool Complete = true;
                    //    foreach (DOTask Task in JobTasks)
                    //    {
                    //        if (Task.Status == DOTask.TaskStatusEnum.Incomplete && (Task.TaskType == DOTask.TaskTypeEnum.Standard || Task.TaskType == DOTask.TaskTypeEnum.Reference))
                    //        {
                    //            Complete = false;
                    //            break;
                    //        }
                    //    }

                    //if (Complete)
                    //{
                    //    Job.JobStatus = DOJob.JobStatusEnum.Complete;
                    //    CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
                    //    Job.CompletedBy = CurrentSessionContext.Owner.ContactID;
                    //    Job.CompletedDate = DateAndTime.GetCurrentDateTime();

                    //    CurrentBRJob.SaveJob(Job);
                    //    SetSiteFlag(Job.SiteID, false);
                    //    //  LogChange(DOJobChange.JobChangeType.JobCompleted);
                    //    ShowIncomplete = false;
                    //}
                    // }
                    //Response.Redirect(Constants.URL_JobSummary);
                }
            }
            else if (task.Status == DOTask.TaskStatusEnum.Complete)
            {
                DOTaskCompletion TC = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                if (TC != null) ParentPage.CurrentBRJob.DeleteTaskCompletion(TC);
                task.Status = DOTask.TaskStatusEnum.Incomplete;
                ParentPage.CurrentBRJob.SaveTask(task);
              
                UpdateRecords(task.JobID,task.SiteID,task.TaskCustomerID);
            }
        }

        private void SaveTask(DOTask task)
        {
            ParentPage.CurrentBRJob.SaveTask(task);
        }

        protected void Invoiced_btn_Click(object sender, EventArgs e)
        {
            Guid taskID = Guid.Parse(((Button)sender).CommandArgument);
            DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                task.Status = DOTask.TaskStatusEnum.Complete;
            else
                task.Status = DOTask.TaskStatusEnum.Invoiced;
            SaveTask(task);
            DOJobContractor contractor = ParentPage.CurrentBRJob.SelectJobContractor(task.JobID,
                        ParentPage.CurrentSessionContext.CurrentContact.ContactID);
            contractor.Active = true;
            contractor.Status = 0;
            ParentPage.CurrentBRJob.SaveJobContractor(contractor);
            UpdateRecords(task.JobID,task.SiteID,task.TaskCustomerID);
        }

        protected void Paid_btn_Click(object sender, EventArgs e)
        {
            Guid taskID = Guid.Parse(((Button)sender).CommandArgument);
            DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            if (task.Status == DOTask.TaskStatusEnum.Paid)
                task.Status = DOTask.TaskStatusEnum.Invoiced;
            else
                task.Status = DOTask.TaskStatusEnum.Paid;
            //If siteid is '00000000-0000-0000-0000-000000000000' or Null, find the SiteID for the task
            if (task.SiteID == null || task.SiteID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                DOJob job = ParentPage.CurrentBRJob.SelectJob(task.JobID);
                task.SiteID = job.SiteID;
            }
            SaveTask(task);
            
            //update the job status
            UpdateRecords(task.JobID,task.SiteID,task.TaskCustomerID);
        }

        protected void UpdateRecords(Guid jobId,Guid siteID,Guid customerID)
        {
            bool complete = true;
            //If current customer is null, consider the customer of the task to find out tasks.
            if (ParentPage.CurrentSessionContext.CurrentCustomer==null)
            {
                ParentPage.CurrentSessionContext.CurrentCustomer = ParentPage.CurrentBRContact.SelectContact(customerID);
            }
            //check if other tasks exists for this contractor or customer
            if (ParentPage.CurrentSessionContext.CurrentContact != null)
            {
                //Find active tasks for the contractor-customer pair
                List<DOBase> tasks =
                    ParentPage.CurrentBRJob.SelectActiveTasksforContact(contractorID: ParentPage.CurrentSessionContext.CurrentContact.ContactID,
                        customerID: ParentPage.CurrentSessionContext.CurrentCustomer.ContactID, jobId: jobId);
                if (tasks.Count!=0)
                {
                    //If all tasks for that job is paid, make the job complete
                
               
                foreach (DOTask Task in tasks)
                {
                    if (Task.Status != DOTask.TaskStatusEnum.Paid)
                    {
                        complete = false;
                        
                        break;
                    }
                }
                if (complete)
                {
                    //DOJob job = ParentPage.CurrentBRJob.SelectJob(jobId);
                    //if (job != null)
                    //{
                    //    job.JobStatus = DOJob.JobStatusEnum.Complete;
                    //    ParentPage.CurrentBRJob.SaveJob(job);
                    //}
                    DOJobContractor contractor = ParentPage.CurrentBRJob.SelectJobContractor(jobId,
                        ParentPage.CurrentSessionContext.CurrentContact.ContactID);
                    if (contractor != null)
                    {
                        contractor.Status = 1;
                            ParentPage.CurrentBRJob.SaveJobContractor(contractor);
                    }
                }
                else
                {
                        //DOJob job = ParentPage.CurrentBRJob.SelectJob(jobId);
                        //if (job != null)
                        //{
                        //    job.JobStatus = DOJob.JobStatusEnum.Incomplete;
                        //    ParentPage.CurrentBRJob.SaveJob(job);
                        //}
                        DOJobContractor contractor = ParentPage.CurrentBRJob.SelectJobContractor(jobId,
                        ParentPage.CurrentSessionContext.CurrentContact.ContactID);
                        if (contractor != null)
                        {
                            contractor.Status = 0;
                            ParentPage.CurrentBRJob.SaveJobContractor(contractor);
                        }
                    }
                
                //if tasks count is 0 that means contractor-customer pair can become inactive 
                //if (tasks.Count == 0 || Complete)
                //{
                    //find the contractor-customer pair
                    DOContractorCustomer contractorCustomer =
                        ParentPage.CurrentBRContact.SelectContractorCustomer(
                            ContactID: ParentPage.CurrentSessionContext.CurrentContact.ContactID,
                            CustomerID: ParentPage.CurrentSessionContext.CurrentCustomer.ContactID);
                    //if a pair record is found- make it inactive
                    if (contractorCustomer != null)
                    {
                        contractorCustomer.Active = !complete;
                        ParentPage.CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                    }
                    // if task count=0, job can be completed for that contractor
                    DOJobContractor jobContractor = ParentPage.CurrentBRJob.SelectJobContractor(jobId,
                        ParentPage.CurrentSessionContext.CurrentContact.ContactID);
                    //if job is found- make it inactive
                    if (jobContractor != null)
                    {
                        jobContractor.Active = !complete;
                        ParentPage.CurrentBRJob.SaveJobContractor(JobContractor: jobContractor);
                    }
                    //find the contactsite record for the current contact (entity) and site
                    DOContactSite contactSite =
                        ParentPage.CurrentBRSite.SelectContactSite(siteID,
                            contractorId: ParentPage.CurrentSessionContext.CurrentContact.ContactID);
                    //if contactsite record is found- make it inactive
                    if (contactSite != null)
                        contactSite.Active = !complete;
                    ParentPage.CurrentBRSite.SaveContactSite(contactSite);
                    //}
                }
            }

        }

        protected void CompleteIconBtn_Click(object sender, EventArgs e)
        {
            Guid taskID = Guid.Parse(((LinkButton)sender).CommandArgument);
            //DOTask task =(sender)
            DOTask task = ParentPage.CurrentBRJob.SelectTask(taskID);
            //task.TaskInvoiceStatus =1;
            //Can't complete task if no materials and no labour.
            if (task.Status == DOTask.TaskStatusEnum.Incomplete)
            {
                List<DOBase> TM = ParentPage.CurrentBRJob.SelectTaskMaterials(task.TaskID);
                List<DOBase> TL = ParentPage.CurrentBRJob.SelectTaskLabour(task.TaskID);
                bool HasActual = false;
                foreach (DOTaskMaterial myTm in TM)
                {
                    if (myTm.MaterialType == TaskMaterialType.Actual && myTm.Active)
                    {
                        HasActual = true;
                        break;
                    }
                }
                if (!HasActual)
                {
                    foreach (DOTaskLabour myTL in TL)
                    {
                        if (myTL.LabourType == TaskMaterialType.Actual && myTL.Active)
                        {
                            HasActual = true;
                            break;
                        }
                    }
                }
                if (!HasActual)
                {
                    //ParentPage.ShowMessage("The task '" + task.TaskName + "' cannot be completed because no materials or labour have been assigned.");
                    Error.Text = "The task '" + task.TaskName + "' cannot be completed because no materials or labour have been assigned.";
                    Error.Visible = true;

                    return;
                }
                else
                {
                    if (task.Status == DOTask.TaskStatusEnum.Incomplete)
                    {
                        DOTaskCompletion TC = ParentPage.CurrentBRJob.CreateTaskCompletion(task.TaskID, ParentPage.CurrentSessionContext.Owner.ContactID);
                        DOTaskCompletion TaskCompleted = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                        if (TaskCompleted == null)
                            ParentPage.CurrentBRJob.SaveTaskCompletion(TC);
                        else
                        {
                            ParentPage.CurrentBRJob.DeleteTaskCompletion(TaskCompleted);
                            ParentPage.CurrentBRJob.SaveTaskCompletion(TC);
                        }
                        task.Status = DOTask.TaskStatusEnum.Complete;
                    }
                    else
                    {
                        DOTaskCompletion TC = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                        ParentPage.CurrentBRJob.DeleteTaskCompletion(TC);
                        task.Status = DOTask.TaskStatusEnum.Incomplete;
                    }

                    ParentPage.CurrentBRJob.SaveTask(task);
                }
            }
            else if (task.Status == DOTask.TaskStatusEnum.Complete)
            {
                DOTaskCompletion TC = ParentPage.CurrentBRJob.SelectTaskCompletion(task.TaskID);
                ParentPage.CurrentBRJob.DeleteTaskCompletion(TC);
                task.Status = DOTask.TaskStatusEnum.Incomplete;
               ParentPage.CurrentBRJob.SaveTask(task);

            }
        }

        protected void rpTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton b = e.Item.FindControl("btnSelectJob") as LinkButton;
            if (b != null)
            {
                string status = ((DOTask)e.Item.DataItem).Status.ToString().ToLower();
                b.CssClass += " " + status;
            }
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