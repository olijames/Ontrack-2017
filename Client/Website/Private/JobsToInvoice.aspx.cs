﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using System.Web.Services;
using System.IO;

namespace Electracraft.Client.Website.Private
{

    public partial class JobsToInvoice : PageBase
    {
        public Guid PreviousJobID;
        public Guid PreviousTaskCustomerID;
        public GridViewRow PreviousRow;
        public decimal RunningTotal;
        public DOTask PreviousTask;


        //Tony added 16.Feb.2017
        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentSessionContext.CurrentEmployee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Page pre render");
            if(CurrentSessionContext.Owner.ContactID==Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833"))
            {
                btnClearFault.Visible = true;
            }
            //if (!Page.IsPostBack)
            {
                LoadgvParent();
            }
        }



        protected void gvParent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //instead get everything from one query and then load a foreach of parent.prerender

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //   // System.Diagnostics.Debug.WriteLine("gvparent rowdatabinding... " + DateTime.Now.ToString("{0:ss}"));
            //    Guid CustomerID = Guid.Parse(gvParent.DataKeys[e.Row.RowIndex].Value.ToString());
            //    Label lblCustomer = e.Row.FindControl("lblCustomerNameBody") as Label;
            //    DOContactInfo myCustomer = CurrentBRContact.SelectAContact(CustomerID);
            //    lblCustomer.Text = myCustomer.DisplayName;
            //    List<DOBase> myTaskJobs = CurrentBRJob.SelectTaskJobs2(CurrentSessionContext.CurrentContact.ContactID, 65, CustomerID);
            //    GridView gvChild = e.Row.FindControl("gvChild") as GridView;


            //    gvChild.DataSource = myTaskJobs;
            //    gvChild.DataBind();


            //}
        }

        protected void LoadgvParent()
        {
            System.Diagnostics.Debug.WriteLine("Start first sql query " + DateTime.Now.ToString("{mm:ss}"));
            //List<DOBase> myCustomerList = CurrentBRJob.SelectCustomersWithInvoiceableTasks(CurrentSessionContext.CurrentContact.ContactID);
            List<DOBase> myCustomerList = CurrentBRJob.SelectAllContractorCustomersWithValue(CurrentSessionContext.CurrentContact.ContactID, 65);
            System.Diagnostics.Debug.WriteLine("Finish first sql query " + DateTime.Now.ToString("{mm:ss}"));
            //myTaskJobs = CurrentBRJob.SelectTaskJobs(CurrentSessionContext.CurrentContact.ContactID, 65);
            //gvParent.DataSource = myCustomerList;
            gvParent.DataSource = myCustomerList;

            gvParent.DataBind();
            System.Diagnostics.Debug.WriteLine("Parent gv databind complete. " + DateTime.Now.ToString("{mm:ss}"));
        }


        //protected void LoadgvChild(object sender, EventArgs e)
        //{
        //    ImageButton myImage = sender as ImageButton;
        //    GridViewRow row = (GridViewRow)myImage.NamingContainer;
        //    GridView gvChild = row.FindControl("gvChild") as GridView;
        //    Guid myCustomerID = Guid.Parse(gvParent.DataKeys[row.RowIndex].Values["ContactID"].ToString());
        //    List<DOBase> myList = CurrentBRJob.SelectTaskJobsContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, 65, myCustomerID);
        //    GridView gvChild = GVR.FindControl("gvChild") as GridView;
        //    gvChild.DataSource = myList;
        //    gvChild.DataBind();


        //}


        protected void gvChild_PreRender(object sender, EventArgs e)
        {


            ////////System.Diagnostics.Debug.WriteLine("Parent gv done. " + DateTime.Now.ToString("{0:ss}"));
            GridView gvChild = sender as GridView;
            //GridViewRow CurrentRow;
            bool ContinueFlag = false;
            // Label lblCustomerTotal;
            ////bool ParentHasCompleteTask = false;
            //////permissions
            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags;
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.ViewInvoices) == CompanyPageFlag.ViewInvoices)
            {
                ContinueFlag = true;
            }

            System.Diagnostics.Debug.WriteLine("Child foreach start " + DateTime.Now.ToString("{mm:ss}"));
            foreach (GridViewRow GVR in gvChild.Rows)
            {

                ////    System.Diagnostics.Debug.WriteLine(GVR.RowIndex.ToString());
                Label lblTaskName = GVR.FindControl("lblTaskNameBody") as Label;
                Label lblJobName = GVR.FindControl("lblJobNameBody") as Label;
                Label lblJobNumber = GVR.FindControl("lblJobNumberBody") as Label;
                Label lblTaskStatus = GVR.FindControl("lblTaskStatusBody") as Label;
                Label lblTaskNumber = GVR.FindControl("lblTaskNumberBody") as Label;
                Label lblTaskValue = GVR.FindControl("lblTaskValueBody") as Label;
                //Label txtBox = (Label)GVR.FindControl("lblJobNameBody");
                //Button btn = GVR.FindControl("btnGotToTaskBody") as Button;
                Guid TaskID = Guid.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TaskID"].ToString());
                DOTask myCurrentTask = CurrentBRJob.SelectTask(TaskID);
                Guid TaskCustomerID = Guid.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TaskCustomerID"].ToString());
                string TaskName = gvChild.DataKeys[GVR.RowIndex].Values["TaskName"].ToString();
                string JobName = gvChild.DataKeys[GVR.RowIndex].Values["JobName"].ToString();
                string TaskNumber = gvChild.DataKeys[GVR.RowIndex].Values["TaskNumber"].ToString();
                string JobNumber = gvChild.DataKeys[GVR.RowIndex].Values["JobNumberAuto"].ToString();
                string Status = gvChild.DataKeys[GVR.RowIndex].Values["Status"].ToString();
                Guid JobID = Guid.Parse(gvChild.DataKeys[GVR.RowIndex].Values["JobID"].ToString());
                decimal MaterialValue = decimal.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TotalMaterial"].ToString());
                decimal LabourValue = decimal.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TotalLabour"].ToString());
                //decimal RowValue = decimal.Parse(gvChild.DataKeys[GVR.RowIndex].Values["Total"].ToString());
                decimal Total = MaterialValue + LabourValue;
                lblJobNumber.Text = TaskCustomerID.ToString();
                lblTaskName.Text = TaskName;
                lblTaskNumber.Text = TaskNumber;
                lblTaskStatus.Text = Status;
                decimal RowValue = MaterialValue + LabourValue;
                //        if(ContinueFlag)lblTaskValue.Text = RowValue.ToString("##########.##");

                //        RunningTotal = RunningTotal + RowValue;
                string value = myCurrentTask.Status.ToString();
                //        lblTaskStatus.Text = value;
                if (value == "Complete")
                {
                    lblJobName.ForeColor = System.Drawing.Color.Green;
                    lblJobNumber.ForeColor = System.Drawing.Color.Green;
                    lblTaskStatus.ForeColor = System.Drawing.Color.Green;
                    lblTaskName.ForeColor = System.Drawing.Color.Green;
                    lblTaskNumber.ForeColor = System.Drawing.Color.Green;
                    GridViewRow CurrentParentRow = (GridViewRow)((GridView)sender).NamingContainer;
                    //lblCustomerTotal = CurrentParentRow.FindControl("lblCustomerTotal") as Label;
                    CurrentParentRow.Cells[1].ForeColor = System.Drawing.Color.Green;
                    CurrentParentRow.Cells[2].ForeColor = System.Drawing.Color.Green;
                    lblTaskValue.ForeColor = System.Drawing.Color.Green;


                    //ParentHasCompleteTask = true;
                }

                ////        //Below is for populating invoice value



                // if (PreviousTaskCustomerID!=TaskCustomerID) // && PreviousTaskCustomerID!= Guid.Parse("00000000-0000-0000-0000-000000000000"))
                //{
                //  RunningTotal = RowValue;

                //}

                //  Label lblCustomerTotal = CurrentRow.FindControl("lblCustomerTotalBody") as Label;
                //      lblCustomerTotal = CurrentRow.FindControl("lblCustomerTotalBody") as Label;
                //   lblCustomerTotal.Text = RunningTotal.ToString("##########.##");
                //PreviousRow = CurrentRow;
                //  lblCustomerTotal = CurrentRow.FindControl("lblCustomerTotalBody") as Label;
                //  lblCustomerTotal.Text = RunningTotal.ToString("##########.##");

                //Label lblCustomerTotal = CurrentRow.FindControl("lblCustomerTotalBody") as Label;
                // if (ParentHasCompleteTask) lblCustomerTotal.ForeColor = System.Drawing.Color.Green;
                // if (PreviousTaskCustomerID != TaskCustomerID) // && PreviousTaskCustomerID!= Guid.Parse("00000000-0000-0000-0000-000000000000"))
                // {
                //if (PreviousRow != null)
                //{//first record
                //    RunningTotal = RowValue;
                //    ParentHasCompleteTask = false;
                //    lblCustomerTotal.ForeColor = System.Drawing.Color.Green;
                //    //  PreviousRow = (GridViewRow)((GridView)sender).NamingContainer; //for first record
                //}
                //Label lblCustomerTotal = PreviousRow.FindControl("lblCustomerTotalBody") as Label;

                //RunningTotal = RunningTotal - RowValue;


                //  PreviousRow = CurrentRow;

                if (ContinueFlag)//has permission
                {
                    lblTaskValue.Text = Total.ToString("##########.##");
                    //lblCustomerTotal.Text = 
                }
                else
                {
                    lblTaskValue.Text = "N/A";
                }
                PreviousTaskCustomerID = TaskCustomerID;
                if (PreviousJobID != JobID)
                {
                    lblJobName.Text = JobName;//this code is so job name only appears once on the list
                    //lblJobNumber.Text = JobNumber;
                }
                PreviousJobID = JobID;

            }
            //System.Diagnostics.Debug.WriteLine("Child foreach end " + DateTime.Now.ToString("{mm:ss}"));
        }







        protected void btnGotToTaskBody_Click(object sender, EventArgs e)
        {
            Button btn1 = sender as Button;
            GridView gvChild = btn1.NamingContainer.NamingContainer as GridView;
            GridViewRow GVR = (GridViewRow)btn1.NamingContainer;
            Guid TaskID = Guid.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TaskID"].ToString());
            

            DOTask myTask = CurrentBRJob.SelectTask(TaskID);
            CurrentSessionContext.CurrentTask = myTask;
            CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(myTask.JobID);
            Response.Redirect(Constants.URL_TaskSummary + "?taskid=" + TaskID);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        protected void btnInvoice_Click(object sender, EventArgs e)
        {


            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags;
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.ViewInvoices) == CompanyPageFlag.ViewInvoices)
            {
                Button btn1 = sender as Button;
                GridView gvChild = btn1.NamingContainer.NamingContainer as GridView;
                GridViewRow GVR = (GridViewRow)btn1.NamingContainer;
                Guid TaskID = Guid.Parse(gvChild.DataKeys[GVR.RowIndex].Values["TaskID"].ToString());
                DOTask myTask = CurrentBRJob.SelectTask(TaskID);
                DateTime PaymentDate = DateTime.Today;

                //Tony modified 17.Jan.2017
                //was			CurrentBRJob.CreateInvoice(myTask,CurrentSessionContext.Owner,PaymentDate);
                CurrentBRJob.CreateInvoice(myTask, CurrentSessionContext.Owner, PaymentDate, null, null);


            }
        }

        protected void gvParent_PreRender(object sender, EventArgs e)
        {

            ////System.Diagnostics.Debug.WriteLine("selectAllTaskJobs start " + DateTime.Now.ToString("{mm:ss}"));
            ////List<DOBase> myList = CurrentBRJob.SelectAllTaskJobs(CurrentSessionContext.CurrentContact.ContactID, 65);
            ////System.Diagnostics.Debug.WriteLine("selectAllTaskJobs complete " + DateTime.Now.ToString("{mm:ss}"));
            //System.Diagnostics.Debug.WriteLine("Parent prerender start. " + DateTime.Now.ToString("{mm:ss}"));
            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            //if(Employee==null && CurrentSessionContext.Owner.ContactID== CurrentSessionContext.CurrentContact.ContactID) //if true then an employee record is missing for yourself
            //{
            //    DOContactCompany docc = CurrentBRContact.CreateContactCompany(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            //    CurrentBRContact.SaveContactCompany(docc);
            //    Employee = CurrentBRContact.CreateEmployeeInfo(docc.ContactCompanyID, CurrentSessionContext.CurrentContact.ContactID);
            //    Employee.AccessFlags = 1048577;
            //    Employee.Address1 = CurrentSessionContext.CurrentContact.Address1;
            //    Employee.Address2 = CurrentSessionContext.CurrentContact.Address2;
            //    Employee.Address3 = CurrentSessionContext.CurrentContact.Address3;
            //    Employee.Address4 = CurrentSessionContext.CurrentContact.Address4;
            //    Employee.Email = CurrentSessionContext.CurrentContact.Email;
            //    Employee.FirstName = CurrentSessionContext.CurrentContact.FirstName;
            //    Employee.LastName = CurrentSessionContext.CurrentContact.LastName;
            //    Employee.Phone = CurrentSessionContext.CurrentContact.Phone;
            //    CurrentBRContact.SaveEmployeeInfo(Employee);
            //}
            long storedVal = Employee.AccessFlags;
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;

            foreach (GridViewRow GVR in gvParent.Rows)
            {
                Guid myCustomerID = Guid.Parse(gvParent.DataKeys[GVR.RowIndex].Values["ContactID"].ToString());
                List<DOBase> myList = CurrentBRJob.SelectTaskJobsContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, 65, myCustomerID);
                GridView gvChild = GVR.FindControl("gvChild") as GridView;
                gvChild.DataSource = myList;
                gvChild.DataBind();
                TableCell bf = GVR.Cells[2] as TableCell;
                if ((myFlags & CompanyPageFlag.ViewInvoices) != CompanyPageFlag.ViewInvoices)
                {

                    bf.Text = "N/A";

                }
            }

            //System.Diagnostics.Debug.WriteLine("Parent prerender finished " + DateTime.Now.ToString("{mm:ss}"));
            ////    Guid CustomerContactID = Guid.Parse(gvParent.DataKeys[GVR.RowIndex].Values["ContactID"].ToString());
            ////    //string myDisplayName = gvParent.DataKeys[GVR.RowIndex].Values["DisplayName"].ToString();
            ////    Label lblCustomer = GVR.FindControl("lblCustomerNameBody") as Label;
            ////    foreach(DOBase LI in myList)
            ////    {
            ////        DOTaskJob myTaskJob = LI as DOTaskJob;
            ////        if (myTaskJob.TaskCustomerID == CustomerContactID)
            ////        {
            ////            GridView gvChild = GVR.FindControl("gvChild") as GridView;



            ////            foreach (GridViewRow GVRChild in gvChild.Rows)
            ////            {
            ////                gvChild.Rows



            ////                Label lblTaskName = GVRChild.FindControl("lblTaskNameBody") as Label;
            ////                Label lblJobName = GVRChild.FindControl("lblJobNameBody") as Label;
            ////                Label lblJobNumber = GVRChild.FindControl("lblJobNumberBody") as Label;
            ////                Label lblTaskStatus = GVRChild.FindControl("lblTaskStatusBody") as Label;
            ////                Label lblTaskNumber = GVRChild.FindControl("lblTaskNumberBody") as Label;
            ////                Label lblTaskValue = GVRChild.FindControl("lblTaskValueBody") as Label;
            ////                lblTaskName.Text = ;
            ////                //        lblTaskNumber.Text = TaskNumber;
            ////                //        lblTaskStatus.Text = Status;
            ////                //        decimal RowValue = MaterialValue + LabourValue;
            ////                ////        if(ContinueFlag)lblTaskValue.Text = RowValue.ToString("##########.##");

            ////                ////        RunningTotal = RunningTotal + RowValue;
            ////                //        string value = myCurrentTask.Status.ToString();
            ////                ////        lblTaskStatus.Text = value;
            ////                //        if (value == "Complete")
            ////                //        {
            ////                //            lblTaskStatus.ForeColor = System.Drawing.Color.Green;
            ////                //            //ParentHasCompleteTask = true;
            ////                //        }  



            ////            }


            ////        }
            ////        //if(myDisplayName=="")
            ////        //{

            ////        //}
            //    }


            //}


        }

        protected void btnBody_Click(object sender, EventArgs e)
        {

            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags;
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.ViewInvoices) == CompanyPageFlag.ViewInvoices)
            {
                Button btn1 = sender as Button;
                GridViewRow GVR = btn1.NamingContainer as GridViewRow;
                GridView gvChild = GVR.FindControl("gvChild") as GridView;

                DateTime PaymentDate = DateTime.Today;

                foreach (GridViewRow gvRChild in gvChild.Rows)
                {
                    CheckBox myCheckBox = gvRChild.FindControl("chkBody") as CheckBox;
                    if (myCheckBox.Checked == true)
                    {
                        Guid TaskID = Guid.Parse(gvChild.DataKeys[gvRChild.RowIndex].Values["TaskID"].ToString());
                        DOTask myTask = CurrentBRJob.SelectTask(TaskID);

                        //Tony modified 17.Jan.2017
                        CurrentBRJob.CreateInvoice(myTask, CurrentSessionContext.Owner, PaymentDate);
                    }
                }
            }
            //LinkButton btn = (LinkButton)sender;
            //GridViewRow row = (GridViewRow)btn.NamingContainer;
            //int i = Convert.ToInt32(row.RowIndex);



        }

        protected void clearFault_Click(object sender, EventArgs e)
        {
            //function added 10.7.2017 Jared. Designed to sort out any old tasks that refer to contactid rather that contractorcustomerid
            DOContractorCustomer doCC;
            List<DOBase> myContactList = CurrentBRJob.SelectAllContactsWithValue(CurrentSessionContext.CurrentContact.ContactID, 65);
            foreach(DOContactWithJobValue myContact in myContactList)
            {
                doCC = null;
                doCC = CurrentBRContact.SelectContractorCustomer(myContact.ContactID,myContact.ContactID);
                if (doCC == null)
                {
                    doCC = CurrentBRContact.CreateContractorCustomer(myContact.ContactID, myContact.ContactID,
                          myContact.ContactID, myContact.Address1, myContact.Address2, myContact.Address3, myContact.Address4, "",
                          DOContractorCustomer.LinkedEnum.NotLinked, "", Guid.NewGuid() , "", "", 1);
                    CurrentBRContact.SaveContractorCustomer(doCC);
                }
                List<DOBase> myTasks = CurrentBRJob.SelectTasksByCustomerID(myContact.ContactID);
                {
                    foreach(DOTask myTask in myTasks)
                    {
                        myTask.TaskCustomerID = doCC.ContactCustomerId;
                        CurrentBRJob.SaveTask(myTask);
                    }
                }
            }
            
            //List<DOBase> myContractorCustomerList = CurrentBRJob.SelectAllContractorCustomersWithValue(CurrentSessionContext.CurrentContact.ContactID, 65);

        }
        //protected void btnViewTask_Click(object sender, EventArgs e)
        //{
        //    string strTaskID = ((LinkButton)sender).CommandArgument.ToString();
        //    Guid TaskID = new Guid(strTaskID);
        //    ParentPage.CurrentSessionContext.CurrentTask = ParentPage.CurrentBRJob.SelectTask(TaskID);
        //    Response.Redirect(Constants.URL_TaskSummary);
        //}
    }

}