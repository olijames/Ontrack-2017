using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Diagnostics;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using System.Web;
using Electracraft.Framework.Utility;
//using System.Collections;
//using System.Data.SqlClient;


namespace Electracraft.Client.Website
{


    public partial class JobTemplateInput : PageBase
    {


        protected void Page_Load(object sender, EventArgs e)//first page load and every postback
        {


            if (!IsPostBack)
            {
               
                LoadTCDropDownList();
                LoadTTDropDownList();
                LoadJTDropDownList();
            }
        }

        protected void JobTemplates_PreRender(object sender, EventArgs e)
        {
           // LoadJTDropDownList();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

        protected void TemplateTasks_PreRender(object sender, EventArgs e)
        {
           // LoadTTDropDownList();
        }

        protected void TradeCategoryAll_PreRender(object sender, EventArgs e)
        {
           // LoadTCDropDownList();
        }

        protected void LoadTTDropDownList()//template tasks
        {
            List<DOBase> LineItems = CurrentBRJob.SelectTemplateTasks(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
            ddTemplateTasks.DataSource = LineItems;
            ddTemplateTasks.DataTextField = "TaskName";//maybe more here
            ddTemplateTasks.DataValueField = "TaskID";
            ddTemplateTasks.DataBind();
      
        }

        protected void LoadJTDropDownList()//job templates
        {

            List<DOBase> LineItems = CurrentBRContact.SelectJobTemplatesByCompanyID(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"));//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
            ddJobTemplates.DataSource = LineItems;
            ddJobTemplates.DataTextField = "JobTemplateName";
            ddJobTemplates.DataValueField = "JobTemplateID";
            ddJobTemplates.DataBind();
        }



        protected void LoadTCDropDownList() //template tasks
        {
            List<DOBase> tradeCategories = CurrentBRTradeCategory.SelectTradeCategories();
            ddTradeCategory.DataSource = tradeCategories;
            ddTradeCategory.DataTextField = "TradeCategoryName";
            ddTradeCategory.DataValueField = "TradeCategoryID";
            ddTradeCategory.DataBind();
        }



        protected void btnAddJobTemplate_Click(object sender, EventArgs e)
        {

            if (tbJobTemplateName.Text != "")
            {
                DOJobTemplate JobTemplate = new DOJobTemplate();

                JobTemplate.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                JobTemplate.JobTemplateName = tbJobTemplateName.Text;
                JobTemplate.JobTemplateID = Guid.NewGuid();
                CurrentBRJob.SaveJobTemplate(JobTemplate);
                LoadJTDropDownList();
            }

        }

        protected void btnAddTemplateTask_Click(object sender, EventArgs e)
        {
            if (tbTaskName.Text != "")
            {
                //Create new task
                DOTask Task = new DOTask();
                Guid TaskGuid = Guid.NewGuid();
                Task.TaskID = TaskGuid;
                Task.Description = tbTaskDescription.Text;
                Task.TaskName = tbTaskName.Text;
                Task.Status = DOTask.TaskStatusEnum.TemplateTask;
                Task.JobID = Guid.Parse("99999999-9999-9999-9999-999999999999");
                Task.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                Task.ParentTaskID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                Task.TaskType = DOTask.TaskTypeEnum.Standard;
                Task.CreatedBy=Guid.Parse("00000000-0000-0000-0000-000000000000");
                Task.CreatedDate = DateTime.Now;
                Task.Active = false;
                Task.TaskOwner=Guid.Parse("00000000-0000-0000-0000-000000000000");
                Task.AmendedBy = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
                Task.AmendedTaskID = Guid.Parse("00000000-0000-0000-0000-000000000000");

                Task.TaskCustomerID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                Task.SiteID = Guid.Parse("99999999-9999-9999-9999-999999999999");
                Task.TaskOwner = Guid.Parse("00000000-0000-0000-0000-000000000000");
                
                CurrentBRJob.SaveTask(Task);
                LoadTTDropDownList();
            }
        }


        protected void btnAddTaskToJobTemplate_Click(object sender, EventArgs e)
        {

            //Create new JobTemplateTask
            DOJobTemplateTask JobTemplateTask = new DOJobTemplateTask();

            String JTID = ddJobTemplates.SelectedItem.Value;
            String TTID = ddTemplateTasks.SelectedItem.Value;
            JobTemplateTask.JobTemplateTaskID = Guid.NewGuid();
            JobTemplateTask.TemplateTaskID = Guid.Parse(TTID);
            JobTemplateTask.JobTemplateID = Guid.Parse(JTID);
            JobTemplateTask.Duration = decimal.Parse(tbDuration.Text);//need checks for input
            JobTemplateTask.StartDelay = decimal.Parse(tbStartDelay.Text);//need checks for input !fix!

            CurrentBRJob.SaveJobTemplateTask(JobTemplateTask);

        }










    }
}