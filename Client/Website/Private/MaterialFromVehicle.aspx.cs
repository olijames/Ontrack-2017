﻿using System;
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


    public partial class MaterialFromVehicle : PageBase
    {
        protected DOEmployeeInfo Employee;
        protected bool HasRightToMove = false;
        protected bool HasRightToDelete = false;
        //Tony added 3.Feb.2017
        private string checkValue = "";
        private string targetURL = "VehicleInput.aspx";


        protected void Page_Load(object sender, EventArgs e)//first page load and every postback -happens first
        {
            System.Diagnostics.Debug.WriteLine("PageLoad");

            //Tony added 3.Feb.2017 begin
            checkValue = Request["checkValue"];

            //if user has no vehicle, no move material from vehicle but has add vehicle permission
            if (checkValue == "addVehicle")
            {
                string linkPart = "<a href=\"" + targetURL + "\"> here </a>";

                ShowMessage("You do not have a vehicle click" + linkPart + "now to create one");
            }
            //Tony added 3.Feb.2017 end
        }

        protected void Page_PreRender(object sender, EventArgs e) //first page load and every postback - happens second
        {
            //WAS WORKING
            System.Diagnostics.Debug.WriteLine("MFV PreRender");
            if (!IsPostBack)
            {

                // LoadgvMainUnassigned();
                LoadDropDownList();
                //select user
                LoadTitle();
                LoadgvVehicle();
                LoadVehicleLabels();
            }


        }

        protected void btnVehicleSelect_Click(object sender, EventArgs e)
        {

        }
        protected void ddVehicleSelect_SelectedIndexChanged(Object sender, EventArgs e)
        {
            LoadgvVehicle();

        }

        protected void ddTransferToVehicle_SelectedIndexChanged(Object sender, EventArgs e)
        {
            LoadgvVehicle();

        }

        protected void gvVehicle_PreRender(object sender, EventArgs e)
        {

            LoadVehicleLabels();
            //if (CurrentSessionContext.Owner.ContactID == Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833") && !IsPostBack) //permissions
            //{
            // make sure your have permission to delete from vehicle, 
            if (HasRightToDelete && !IsPostBack) ddTransferToVehicle.Items.Insert(ddTransferToVehicle.Items.Count, "Trash");
            if (ddTransferToVehicle.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString()) != null)
            {
                ListItem i = ddTransferToVehicle.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString());
                ddTransferToVehicle.Items.Remove(i);
            }
        }

        protected void LoadDropDownList()
        {
            //check if employee has the right to add from other vehicles
            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
                                                   // Reinstate the mask and re-test

            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.MoveMaterialsFromOtherVehicle) == CompanyPageFlag.MoveMaterialsFromOtherVehicle)
            {
                HasRightToMove = true;
            }
            if ((myFlags & CompanyPageFlag.DeleteMaterialsFromVehicle) == CompanyPageFlag.DeleteMaterialsFromVehicle)
            {
                HasRightToDelete = true;
            }
            //else
            //{
            //    int i = (int)CompanyPageFlag.TimeSheet;
            //    storedVal = storedVal + i;
            //}

            String L = "";
            if (IsPostBack)
            {
                L = ddVehicleSelect.SelectedItem.Value;

            }
            //            List<DOBase> LineItem = CurrentBRContact.SelectCompanyContactsWithAVehicle(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 

            // Tony modified 22.Feb.2017
            List<DOBase> LineItem = CurrentBRContact.SelectCompanyEmployeeWithVehicle(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233,

            ddVehicleSelect.DataSource = LineItem;
            //            ddVehicleSelect.DataTextField = "FirstName";//more here

            // Tony modified 22.Feb.2017
            ddVehicleSelect.DataTextField = "DisplayInfo";

            //            ddVehicleSelect.DataValueField = "ContactID";
            //Tony modified 23.Feb.2017
            //            ddVehicleSelect.DataValueField = "EmployeeID";
            // ==> Since one user can have several vehicles, EmployeeID cannot be the key value
            // NewKey is new added composite key which combines employeeID and vehicleID
            ddVehicleSelect.DataValueField = "VehicleID";

            ddVehicleSelect.DataBind();

            if (HasRightToMove)
            {
                ddTransferToVehicle.DataSource = LineItem;
                //                ddTransferToVehicle.DataTextField = "FirstName";

                //Tony modified 23.Feb.2017
                ddTransferToVehicle.DataTextField = "DisplayInfo";

                //Tony modified 23.Feb.2017
                //                ddTransferToVehicle.DataValueField = "ContactID";
                //                ddTransferToVehicle.DataValueField = "EmployeeID";
                ddTransferToVehicle.DataValueField = "VehicleID";

                //ddTransferToVehicle.Items.Insert(ddTransferToVehicle.Items.Count, new ListItem("Trash", "01"));//issue here. Not putting 'trash' into dropdown
                ddTransferToVehicle.DataBind();
            }

            bool HaveVehicle = false;
            foreach (DOBase LI in LineItem)
            {
                //Tony modified 22.Feb.2017
                //                DOContact C = LI as DOContact;
                //                if (C.ContactID == CurrentSessionContext.Owner.ContactID) HaveVehicle = true;

                //                DOEmployeeInfo E = LI as DOEmployeeInfo;
                // Tony modified on 23.Feb.2017
                DOEmployeeVehicle E = LI as DOEmployeeVehicle;

                if (E.EmployeeID == CurrentSessionContext.CurrentEmployee.EmployeeID) HaveVehicle = true;
            }

            if (HaveVehicle)
            {
                if (!IsPostBack)
                {
                    //    ddVehicleSelect.SelectedValue = ddVehicleSelect.SelectedItem.Value;
                    //                    if (ddVehicleSelect.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString()) != null) ;

                    //Tony modified 22.Feb.2017
                    if (ddVehicleSelect.Items.FindByValue(CurrentSessionContext.CurrentEmployee.EmployeeID.ToString()) != null)
                    {

                        //                        ddVehicleSelect.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                        ddVehicleSelect.SelectedValue = CurrentSessionContext.CurrentEmployee.EmployeeID.ToString();
                    }
                    //if (HasRightToMove)
                    //{
                    //    if (ddTransferToVehicle.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString()) != null) ;
                    //    {
                    //        ddTransferToVehicle.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                    //    }
                    //}


                }
            }

            // CurrentBRContact.SelectCompanyContacts
        }
        //----------------------------------------------------------------------------------------------
        //                                 ADD from vehicle to task
        //----------------------------------------------------------------------------------------------
        //1. create New SIM with taskmaterialid
        //2. adjust existing qtyremaining from SIM
        //3. add TM

        protected void btnAddFromVehicleToTask_Click(object sender, EventArgs e)//NOT COMPLETE
        {
            //jared the two lines below probably not needed - check
            Button btn1 = sender as Button;
            if (btn1.ID == "btnAddToTask")
            {
                GridViewRow Row = (GridViewRow)btn1.NamingContainer;
            }
            // System.Diagnostics.Debug.WriteLine(Row.RowIndex);

            bool SupplierInvoiceUsed = false;
            //------------------------------------------------------------------
            // CHECK WHICH ARE CHECKED(ticked)
            //-------------------------------------------------------------------
            bool ContinueFlag = true;
            bool OldSIM = false;


            Guid SupplierInvoiceID = Guid.NewGuid();

            foreach (GridViewRow gvVehicleRow in gvVehicle.Rows)
            {
                DOSupplierInvoiceMaterial PreviousSIM = new DOSupplierInvoiceMaterial();
                OldSIM = false;
                ContinueFlag = true;
                CheckBox CB = gvVehicleRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
                if (CB.Checked)
                {
                    TextBox TB = gvVehicleRow.FindControl("tbAdd") as TextBox;    //2.
                    decimal decQtyRemaining = decimal.Parse(gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["QtyRemainingToAssign"].ToString());
                    string strSupplierInvoiceMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
                    string strSupplierInvoiceID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceID"].ToString();
                    string strMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialID"].ToString();
                    string strPreviousSupplierInvoiceMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["OldSupplierInvoiceMaterialID"].ToString();
                    string strOriginalSupplierInvoiceMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
                    string strMaterialName = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialName"].ToString();
                    string strNewQty = "";

                    //-------------------------------------------
                    //        evaluate textbox
                    //-------------------------------------------
                    if (TB.Visible == true)
                    {
                        String TBtext = TB.Text;
                        int intCount = 0;
                        foreach (char c in TB.Text)
                        {

                            strNewQty = strNewQty + c;
                            if (c == ',')//fix bug with textbox producing commas. Collects everything after the comma. Stuuuupid Bug
                            {
                                strNewQty = "";
                            }


                        }
                        decimal decNewQty = 0;
                        foreach (char c in strNewQty)//fix double '-' in text or '-' in wrong place
                        {

                            if (c == '-')
                            {
                                if (intCount != 0)
                                {
                                    ContinueFlag = false;
                                }

                            }
                            if (!decimal.TryParse(strNewQty, out decNewQty))
                            {
                                ContinueFlag = false;
                            }

                            intCount++;
                        }



                        if (strNewQty == "")
                        {
                            ContinueFlag = false;
                        }

                        //---------------------------------------------------------------------------------------
                        //     ADD TO TASKMATERIAL TABLE
                        //     UPDATING NEW SUPPLIERMATERIALID RECORDS THAT COME FROM EXISTING RECORDS
                        //---------------------------------------------------------------------------------------

                        if (ContinueFlag)
                        {
                            ContinueFlag = false;
                            decimal decPosNewQty = Math.Abs(decNewQty); //make sure the new number is positive
                            decimal decPosQtyRemaining = Math.Abs(decQtyRemaining); //make sure the existing number is positive

                            decimal decSum = decNewQty + decQtyRemaining; //add new and existing numbers together
                            decimal decPosSum = Math.Abs(decSum); // make sure they are positive

                            decimal decLimit = 2 * decPosQtyRemaining; // this makes sure that the new number is not greater than the original





                            if (decPosSum <= decLimit)
                            {
                                if (decPosSum == decLimit)
                                {

                                }
                                if (decPosSum >= decPosQtyRemaining && decPosNewQty != 0)//! CHECK IF VALUES ARE DIFFERENT
                                {
                                    //1. Alter qty remaining value of original SIM
                                    //2.Create TM
                                    //3.create SIM with Vehicle and Task populated


                                    //1. Alter qty remaining value of original SIM
                                    //DOSupplierInvoiceMaterial SIM = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strSupplierInvoiceMaterialID));
                                    //SIM.QtyRemainingToAssign = decSum - decQtyRemaining; //possible bug !here!
                                    //CurrentBRJob.SaveSupplierInvoiceMaterial(SIM);

                                    System.Diagnostics.Debug.WriteLine(sender.ToString());
                                    string btnName = ((Button)sender).ID;
                                    //3.create SIM with Vehicle and Task populated
                                    DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
                                    SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();
                                    SIMaterial.MaterialID = Guid.Parse(strMaterialID);

                                    // SIMaterial.VehicleID= Guid.Parse("08F88AD1-7960-4F57-A899-94BF89792EBD");
                                    //SIMaterial.SupplierInvoiceID = Guid.Parse("AAAABBBB-AAAA-BBBB-AAAA-BBBBAAAABBBB");         //from a vehicle constant(?)

                                    SIMaterial.Qty = decNewQty;
                                    SIMaterial.QtyRemainingToAssign = decPosNewQty;
                                    SIMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                                    SIMaterial.Assigned = true;
                                    SIMaterial.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                    SIMaterial.CreatedDate = DateTime.Now;
                                    SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse(strOriginalSupplierInvoiceMaterialID);
                                    DOVehicle V = new DOVehicle();
                                    //                                    V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);

                                    //Tony modified 2.Mar.2017
                                    //                                    V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);

                                    //Tony modified 3.Mar.2017

                                    //Tony modified 22.Apr.2017 
                                    // Gets employeeID as parameter instead of contactID
                                    //                                    V = CurrentBRJob.SelectDefaultVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
                                    V = CurrentBRJob.SelectDefaultVehicleByDriverID(CurrentSessionContext.CurrentEmployee.EmployeeID);

                                    Guid id = Guid.NewGuid();

                                    //see if you have permission to transfer

                                    if (btnName == "btnAddToTask")//jul 2016 jared
                                    {
                                        //2.Create TM
                                        DOMaterial myMaterial = CurrentBRJob.SelectMaterial(Guid.Parse(strMaterialID));
                                        ContinueFlag = true;
                                        DOTaskMaterial TM = new DOTaskMaterial();
                                        TM.SellPrice = myMaterial.SellPrice;
                                        TM.TaskMaterialID = id;
                                        TM.MaterialName = strMaterialName;
                                        TM.MaterialID = Guid.Parse(strMaterialID);
                                        TM.Description = "From " + CurrentSessionContext.Owner.FirstName + " " + CurrentSessionContext.Owner.LastName + "'s vehicle";
                                        TM.Quantity = decNewQty;
                                        TM.InvoiceQuantity = decNewQty;
                                        TM.TaskID = CurrentSessionContext.CurrentTask.TaskID;
                                        TM.MaterialType = TaskMaterialType.Actual;
                                        TM.FromInvoice = false;
                                        TM.FromVehicle = true;
                                        TM.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                        TM.CreatedDate = DateTime.Now;

                                        CurrentBRJob.SaveTaskMaterial(TM);

                                        SIMaterial.TaskMaterialID = id;
                                        SIMaterial.SupplierInvoiceID = Guid.Parse(strSupplierInvoiceID);
                                    }

                                    else //Transfer to either trash or vehicle
                                    {
                                        if (ddTransferToVehicle.Items.Count > 0)
                                        {
                                            ContinueFlag = true;
                                            //                                            DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue));

                                            //Tony modified 2.Mar.2017


                                            if (ddTransferToVehicle.SelectedItem.Text == "Trash")
                                            {

                                                SupplierInvoiceID = Guid.Parse("00000000-0000-0000-0000-000000000002"); //"deleted from V"
                                                SIMaterial.QtyRemainingToAssign = 0;
                                                DOBase DOBV = CurrentBRJob.SelectVehicleByVehicleID(Guid.Parse(ddVehicleSelect.SelectedValue));
                                                V = DOBV as DOVehicle;

                                                System.Diagnostics.Debug.WriteLine(V.VehicleID.ToString());


                                            }
                                            else  //Transfer from one Vehicle to another Vehicle:
                                            {
                                                if (ddTransferToVehicle.SelectedItem.Text != null)
                                                {
                                                    //                                                    V = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddTransferToVehicle.SelectedValue));

                                                    //Tony modified 2.Mar.2017
                                                    V = CurrentBRJob.SelectVehicleByVehicleID(Guid.Parse(ddTransferToVehicle.SelectedValue));
                                                    //here we need new SI with "From x's vehicle" etc
                                                    if (SupplierInvoiceUsed == false)
                                                    {

                                                        PreviousSIM = CurrentBRJob.SelectSupplierInvoiceMaterialByOldSIMandV(Guid.Parse(strPreviousSupplierInvoiceMaterialID), V.VehicleID);
                                                        if (PreviousSIM != null)
                                                        {

                                                            //if it does then dont create a new sim, but instead update the qty only.
                                                            OldSIM = true;

                                                            System.Diagnostics.Debug.WriteLine(V.VehicleID.ToString());

                                                        }
                                                        else
                                                        {

                                                            DOSupplierInvoice SupplierInvoice = new DOSupplierInvoice();
                                                            SupplierInvoice.SupplierInvoiceID = SupplierInvoiceID;
                                                            SupplierInvoice.SupplierID = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC");// Materials[iCount, jCount]; //Corys supplierID
                                                            SupplierInvoice.InvoiceDate = DateTime.Now;
                                                            SupplierInvoice.ContractorReference = "Vehicle transfer";
                                                            SupplierInvoice.TotalExGst = 0;
                                                            SupplierInvoice.SupplierReference = "";
                                                            SupplierInvoice.ContractorID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");

                                                            //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierID);
                                                            //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierName);
                                                            CurrentBRJob.SaveSupplierInvoice(SupplierInvoice);

                                                            //Check if the vehicle transferring too already has a SIM of this OLDsim
                                                        }



                                                    }
                                                }
                                            }
                                            ContinueFlag = true;
                                        }

                                    }
                                    //SIMaterial.SiteID = Guid.NewGuid();
                                    //SIMaterial.SupplierInvoiceID = SupplierInvoiceID;

                                    if (ContinueFlag)
                                    {
                                        SIMaterial.VehicleID = V.VehicleID;
                                        SupplierInvoiceUsed = true;//maybe not here
                                        if (OldSIM)
                                        {

                                            CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(PreviousSIM.SupplierInvoiceMaterialID, PreviousSIM.QtyRemainingToAssign + decNewQty);
                                        }
                                        else
                                        {
                                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);
                                        }


                                        if (decNewQty != decQtyRemaining)
                                        {
                                            decimal decNewQtyRemaining = decQtyRemaining - decNewQty;
                                            CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), decNewQtyRemaining);

                                        }
                                        else
                                        {
                                            CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), 0);
                                        }
                                    }

                                }

                                else
                                {
                                    //code for when you have entered an amount too high in textbox2
                                }
                            }

                        } //

                    }


                }
            }


            LoadgvVehicle();
        }

        //5. alter existing values when supplierinvoicematerial !=  blah blah. order by materialID
        protected void LoadgvVehicle() //todo multi vehicle select???
        {
            //jared DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
            if (ddVehicleSelect.Items.Count != 0)
            {
                //                DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue));

                // Tony modified 23.Feb.2017 begin
                //                DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue.Substring(0,36)));

                //Tony commented 2.Mar.2017
                //                DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue));

                //Tony modified 2.Mar.2017
                DOBase DOBV = CurrentBRJob.SelectVehicleByVehicleID(Guid.Parse(ddVehicleSelect.SelectedValue));
                // Tony modified 23.Feb.2017 end
                DOVehicle V = DOBV as DOVehicle;
                if (V != null)
                {

                    List<DOBase> DS = CurrentBRJob.SelectVehicleMaterials(CurrentSessionContext.CurrentContact.ContactID,
                                V.vehicleDriver, V.VehicleID);
                    //List<DOBase> DS = CurrentBRJob.SelectVehicleSIMs(CurrentSessionContext.CurrentContact.ContactID,
                    //              V.VehicleID);


                    gvVehicle.DataSource = DS;

                    gvVehicle.DataBind();
                }
            }

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_TaskSummary);
        }

        protected void LoadVehicleLabels()
        {

            foreach (GridViewRow gvVehicleRow in gvVehicle.Rows)
            {
                if (gvVehicleRow.RowType == DataControlRowType.DataRow)
                {
                    string strMName = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialName"].ToString();
                    string strQty = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["QtyRemainingToAssign"].ToString();
                    string strCost = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["CostPrice"].ToString();
                    string strSell = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SellPrice"].ToString();
                    string strRRP = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["RRP"].ToString();
                    string strSupplier = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierName"].ToString();
                    string strUOM = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["UOM"].ToString();
                    string strSupRef = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["ContractorReference"].ToString();
                    Guid guidCreator = Guid.Parse(gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["Creator"].ToString());



                    //TextBox tb = (TextBox)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("tbAdd");
                    Label LQty = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblQtyBody");
                    Label LMName = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblMNameBody");
                    Label LCost = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblCostBody");
                    Label LSell = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblSellBody");
                    Label LRRP = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblRRPBody");
                    Label LSupplier = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblSupplierBody");
                    Label LSupplierRef = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblSupplierReferenceBody");


                    LSupplier.Text = strSupplier;
                    LRRP.Text = strRRP;
                    LSell.Text = strSell;
                    LCost.Text = strCost;
                    LMName.Text = strMName;
                    LQty.Text = strQty;
                    LSupplierRef.Text = strSupRef;
                    if (strSupRef == "Deleted from task")
                    {

                        //Guid G = Guid.Parse(gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString());
                        // DOTask T = CurrentBRJob.SelectTaskBySIMid(G);

                        if (guidCreator != CurrentSessionContext.Owner.ContactID) //shows who deleted the product if it wasnt you.
                        {
                            DOContactInfo C = CurrentBRContact.SelectAContact(guidCreator) as DOContactInfo;
                            LSupplierRef.Text += " by " + C.FirstName;
                        }


                        //get task and job autonumber for text and taskid for redirect(what happens to current cust - becomes task.customer?)
                        //use text to display something
                    }
                }
            }
        }

        protected void LoadTitle()
        {
            DOTask thisTask = CurrentSessionContext.CurrentTask; //jared to add current task id
            DOJob thisJob = CurrentSessionContext.CurrentJob;
            DOSite thisSite = CurrentSessionContext.CurrentSite;
            DOJobContractor dojc = CurrentBRJob.SelectJobContractor(thisJob.JobID, CurrentSessionContext.CurrentContact.ContactID);
            string TID = dojc.JobNumberAuto + " " + thisTask.TaskNumber.ToString().PadLeft(3, '0') + ", " + thisTask.TaskName + ", " + thisJob.Name + ", " + thisSite.Address1;


            DOBase DOBt = CurrentBRJob.SelectTask(thisTask.TaskID);
            DOTask DOt = DOBt as DOTask;

            DOBase DOBj = CurrentBRJob.SelectJob(DOt.JobID);
            DOJob DOj = DOBj as DOJob;

            lblTitle.Text = TID;

        }


        //protected void LoadgvChildUnassigned(GridViewRowEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("LoadgvChildUnassigned");
        //    //THIS WAS WORKING FOR POPULATING CHILD DATA
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        GridView gvChild = e.Row.FindControl("gvChild") as GridView;
        //        string SupplierReference = gvParent.DataKeys[e.Row.RowIndex].Value.ToString();
        //        List<DOBase> LineItem = CurrentBRJob.SelectMaterialsByContactIDAndSupplierInvoiceReferenceAndAssigned(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"), SupplierReference, false);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
        //        gvChild.DataSource = LineItem;
        //        gvChild.DataBind();
        //    }

        //    //populate text2, hide/show "Redo" button
        //    foreach (GridViewRow gvParentRow in gvParent.Rows)
        //    {
        //        GridView gvChild = (GridView)gvParentRow.FindControl("gvChild");

        //        foreach (GridViewRow gvChildRow in gvChild.Rows)
        //        {
        //            string strOriginalSupplierInvoiceMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["OldSupplierInvoiceMaterialID"].ToString();
        //            TextBox tb = (TextBox)gvChild.Rows[gvChildRow.RowIndex].FindControl("textbox2");
        //            Label L = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblQtyBody");
        //            Label LTB = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblTextBox2");


        //            if (strOriginalSupplierInvoiceMaterialID == "00000000-0000-0000-0000-000000000000")//Original supplierInvoiceMaterial
        //            {
        //                if (tb.Text == "0")
        //                {
        //                    tb.ForeColor = System.Drawing.Color.Gray;
        //                    L.ForeColor = System.Drawing.Color.Gray;
        //                    foreach (TableCell TC in gvChildRow.Cells)
        //                    {

        //                        TC.ForeColor = System.Drawing.Color.Gray;
        //                        TC.Enabled = false;



        //                    }
        //                }

        //                Button B = (Button)gvChild.Rows[gvChildRow.RowIndex].FindControl("btnRedo");
        //                B.Visible = false;
        //                //string cell_2_Value = gvChild.DataKeys[gvChildRow.RowIndex].["Qty"].ToString(); commented 6/4/16 Jared
        //                tb.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString();
        //                //analogous
        //                L.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString() + " / " + gvChild.DataKeys[gvChildRow.RowIndex].Values["Qty"].ToString();



        //            }
        //            else//SET PROPERTIES OF UnEDITABLE CELLS
        //            {

        //                foreach (TableCell TC in gvChildRow.Cells)
        //                {
        //                    tb.ForeColor = System.Drawing.Color.LightGray;
        //                    LTB.ForeColor = System.Drawing.Color.LightGray;
        //                    L.ForeColor = System.Drawing.Color.LightGray;
        //                    TC.ForeColor = System.Drawing.Color.LightGray;
        //                    //assign lblQtyBody


        //                }
        //                Label lblAssignedTo = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblAssignedTo");
        //                CheckBox cb = (CheckBox)gvChild.Rows[gvChildRow.RowIndex].FindControl("chkSelect");
        //                LTB.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["Qty"].ToString();
        //                LTB.Visible = true;
        //                cb.Checked = false;
        //                cb.Visible = false;
        //                tb.Visible = false;

        //                //string TID = gvChild.DataKeys[gvChildRow.RowIndex].Values["TaskID"].ToString();
        //                string TMID = gvChild.DataKeys[gvChildRow.RowIndex].Values["TaskMaterialID"].ToString();
        //                string VID = gvChild.DataKeys[gvChildRow.RowIndex].Values["VehicleID"].ToString();


        //                //When the supplierinvoicematerial is pointing to a task material
        //                if (TMID != "00000000-0000-0000-0000-000000000000")
        //                {
        //                    DOBase DOBtm = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(TMID));
        //                    DOTaskMaterial DOtm = DOBtm as DOTaskMaterial;

        //                    DOBase DOBt = CurrentBRJob.SelectTask(DOtm.TaskID);
        //                    DOTask DOt = DOBt as DOTask;

        //                    DOBase DOBj = CurrentBRJob.SelectJob(DOt.JobID);
        //                    DOJob DOj = DOBj as DOJob;

        //                    lblAssignedTo.Text = DOt.TaskName + ", (" + DOj.Name + ")";
        //                }
        //                //When the supplierinvoicematerial is pointing to a vehicle
        //                if (VID != "00000000-0000-0000-0000-000000000000")
        //                {

        //                    //DOBase OldSupplierInvoiceMaterial = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strOldSupplierInvoiceMaterialID));
        //                    //DOSupplierInvoiceMaterial dos = OldSupplierInvoiceMaterial as DOSupplierInvoiceMaterial;


        //                    DOBase a = CurrentBRJob.SelectVehicle(Guid.Parse("08F88AD1-7960-4F57-A899-94BF89792EBD")); //jared to provide logged in id
        //                    DOVehicle V = a as DOVehicle;

        //                    DOBase b = CurrentBRContact.SelectContact(V.vehicleDriver);
        //                    DOContact C = b as DOContact;

        //                    lblAssignedTo.Text = C.FirstName + "'s Vehicle. ";
        //                }
        //            }
        //        }
        //    }



        //}


        //protected void LoadgvMainUnassigned()//Load list for gvMain
        //{

        //    //WORKING 
        //    List<DOBase> AllSupplierInvoicesUnAssigned = CurrentBRJob.SelectSupplierInvoicesOrderByCRef(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"), false);//electracraft id
        //    gvParent.DataSource = AllSupplierInvoicesUnAssigned;
        //    gvParent.DataBind();



        //}
        ////----------------------------------------------------------------------------
        ////                             REDO
        ////----------------------------------------------------------------------------



        //protected void btnRedo_Click(object sender, EventArgs e)//NOT COMPLETE
        //{

        //    //FIND CONTROL ROW
        //    //GET 'CHILD' LINE ITEM VALUE
        //    // SET 'PARENT LINE ITEM QTYREMAININGTOASSIGN + CHILDQTY
        //    //MAYBE SORT COLOURS

        //    GridViewRow gvChildRow = (GridViewRow)((Button)sender).NamingContainer;
        //    GridView gvChild = (GridView)(gvChildRow.Parent.Parent);

        //    //Determine the RowIndex of the Row whose Button was clicked.
        //    int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //    //Get the value of column from the DataKeys using the RowIndex.
        //    string strOldSupplierInvoiceMaterialID = (gvChild.DataKeys[rowIndex].Values["OldSupplierInvoiceMaterialID"].ToString());
        //    string strCurrentSupplierInvoiceMaterialID = (gvChild.DataKeys[rowIndex].Values["SupplierInvoiceMaterialID"].ToString());




        //    //if it is a taskmaterial/vehicle/mysite
        //    string strCurrentTaskMaterialID = (gvChild.DataKeys[rowIndex].Values["TaskMaterialID"].ToString());
        //    string strCurrentVehicleID = (gvChild.DataKeys[rowIndex].Values["VehicleID"].ToString());
        //    decimal decQty = (decimal.Parse(gvChild.DataKeys[rowIndex].Values["Qty"].ToString()));
        //    decimal decRemainingQty = (decimal.Parse(gvChild.DataKeys[rowIndex].Values["QtyRemainingToAssign"].ToString()));


        //    //set the qtyremainingtoassign to qty
        //    DOBase OldSupplierInvoiceMaterial = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strOldSupplierInvoiceMaterialID));//issue here
        //    DOSupplierInvoiceMaterial dos = OldSupplierInvoiceMaterial as DOSupplierInvoiceMaterial;

        //    //This logic is to prevent redo-ing a SIM assigned to a vehicle where the vehicle SIM has been assigned to a task already
        //    if (strCurrentVehicleID != "00000000-0000-0000-0000-000000000000")
        //    {
        //        if (decQty != decRemainingQty)
        //        {
        //            return;
        //        }
        //    }

        //    //Remove entry
        //    decimal decTotalQty = decQty + dos.QtyRemainingToAssign;
        //    CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOldSupplierInvoiceMaterialID), decTotalQty);

        //    //delete old supplierinvoicematerial record strCurrentSupplierInvoiceMaterialID
        //    DOBase o = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strCurrentSupplierInvoiceMaterialID));
        //    DOSupplierInvoiceMaterial o1 = o as DOSupplierInvoiceMaterial;
        //    CurrentBRJob.DeleteSupplierInvoiceMaterial(o1);

        //    //delete taskmaterial
        //    if (strCurrentTaskMaterialID != "00000000-0000-0000-0000-000000000000")
        //    {
        //        DOBase a = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(strCurrentTaskMaterialID));
        //        DOTaskMaterial a1 = a as DOTaskMaterial;
        //        CurrentBRJob.DeleteTaskMaterial(a1);
        //    }
        //    //if (strCurrentVehicleID != 00000000-0000-0000-0000-000000000000)
        //    //{
        //    //    DOBase a = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(strCurrentTaskMaterialID));
        //    //    DOTaskMaterial a1 = a as DOTaskMaterial;
        //    //    CurrentBRJob.DeleteTaskMaterial(a1);
        //    //}



        //            //some story about cant delete this because some of the product has been assigned to a task already







        //}


        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            decimal value;
            if (!decimal.TryParse(t.Text, out value))
                t.BackColor = System.Drawing.Color.Red;
        }
        //protected void btnSelect_Click(object sender, EventArgs e)//NOT COMPLETE because we need to avoid postback jared to fix
        //{
        //    foreach (GridViewRow gvParentRow in gvParent.Rows)
        //    {
        //        GridView gvChild = gvParentRow.FindControl("gvChild") as GridView;

        //        foreach (GridViewRow gvChildRow in gvChild.Rows)
        //        {
        //            CheckBox CB = gvChildRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
        //            if (CB.Checked == true)
        //            {
        //                CB.Checked = false;
        //            }
        //            else
        //            {
        //                CB.Checked = true;
        //            }


        //        }
        //    }
        //}





        //------------------------------------------------------
        //              ADD TO TASK or VEHICLE
        //------------------------------------------------------


        //protected void btnAdd_Click(object sender, EventArgs e)//NOT COMPLETE
        //{
        //    //Need to limit "add to task" to just modify the grid where it is not all the grids.




        //    //jared the two lines below probably not needed - check
        //    Button btn1 = sender as Button;
        //    GridViewRow Row = (GridViewRow)btn1.NamingContainer;
        //    // System.Diagnostics.Debug.WriteLine(Row.RowIndex);


        //        //------------------------------------------------------------------
        //        //1. CHECK WHICH ARE CHECKED(ticked)
        //        //-------------------------------------------------------------------
        //        bool ContinueFlag = true;
        //        foreach (GridViewRow gvParentRow in gvParent.Rows)
        //        {
        //            GridView gvChild = gvParentRow.FindControl("gvChild") as GridView;


        //            foreach (GridViewRow gvChildRow in gvChild.Rows)
        //            {
        //                ContinueFlag = true;
        //                CheckBox CB = gvChildRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
        //                if (CB.Checked == true)
        //                {
        //                    TextBox TB = gvChildRow.FindControl("TextBox2") as TextBox;    //2.

        //                    //string strOriginalQty = gvChild.Rows[gvChildRow.RowIndex].Cells[1].Text; //GET DATABOUND ID=QTY VALUE
        //                    //string strOriginalMaterialID = gvChild.Rows[gvChildRow.RowIndex].Cells[3].Text;
        //                    //string strOriginalSupplierInvoiceID = gvChild.Rows[gvChildRow.RowIndex].Cells[4].Text; 
        //                    //string strOriginalSupplierInvoiceMaterialID = gvChild.Rows[gvChildRow.RowIndex].Cells[6].Text;


        //                    decimal decQtyRemaining = decimal.Parse(gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString());
        //                    string strSupplierInvoiceID = gvChild.DataKeys[gvChildRow.RowIndex].Values["SupplierInvoiceID"].ToString();
        //                    string strMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["MaterialID"].ToString();
        //                    string strOriginalSupplierInvoiceMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
        //                    string strMaterialName = gvChild.DataKeys[gvChildRow.RowIndex].Values["MaterialName"].ToString();




        //                    //SELECT ONLY THE SIM COPYS

        //                    string strNewQty = "";
        //                    if (TB.Visible == true)
        //                    {
        //                        int intCount = 0;
        //                        //WORK OUT THE NEW VALUE + FORMATTING AND CHECKING FOR TYPO'S
        //                        foreach (char c in TB.Text)
        //                        {

        //                            strNewQty = strNewQty + c;
        //                            if (c == ',')//fix bug with textbox producing commas. Collects everything after the comma. Stuuuupid Bug
        //                            {
        //                                strNewQty = "";
        //                            }


        //                        }
        //                        decimal decNewQty = 0;
        //                        foreach (char c in strNewQty)//fix double '-' in text or '-' in wrong place
        //                        {

        //                            if (c == '-')
        //                            {
        //                                if (intCount != 0)
        //                                {
        //                                    ContinueFlag = false;
        //                                }

        //                            }
        //                            if (!decimal.TryParse(strNewQty, out decNewQty))
        //                            {
        //                                ContinueFlag = false;
        //                            }

        //                            intCount++;
        //                        }



        //                        if (strNewQty == "")
        //                        {
        //                            ContinueFlag = false;
        //                        }





        //                    //decimal decNewQty = decimal.Parse(strNewQty);



        //                    //---------------------------------------------------------------------------------------
        //                    //     ADD TO TASKMATERIAL TABLE
        //                    //     UPDATING NEW SUPPLIERMATERIALID RECORDS THAT COME FROM EXISTING RECORDS
        //                    //---------------------------------------------------------------------------------------



        //                    //DEALING WITH NEGATIVES



        //                    if (ContinueFlag && decQtyRemaining>0)
        //                        {
        //                            decimal decPosNewQty = Math.Abs(decNewQty);//make sure number is positive
        //                            decimal decPosQtyRemaining = Math.Abs(decQtyRemaining);
        //                            decimal decSum = decNewQty + decQtyRemaining;
        //                            decimal decPosSum = Math.Abs(decSum);
        //                            decimal decLimit = 2 * decPosQtyRemaining;
        //                            if (decPosSum <= decLimit)
        //                            {
        //                                if (decPosSum >= decPosQtyRemaining)//3. CHECK IF VALUES ARE DIFFERENT
        //                                {
        //                                //4. FOR ALL DIFFERENT CREATE A NEW SUPPLIERINVOICEMATERIAL RECORD AND ASSIGN TASK and assign current supplierinvoicematerialid as oldsupplierinvoicematerialid











        //                                Guid id = Guid.NewGuid();
        //                                if (btn1.ID == "btnAddToTask")
        //                                {
        //                                    //NEED TO CHECK THAT WHEN NEGATIVE THAT THERE IS ALREADY A POSITIVE NUMBER BEFORE
        //                                    //ALLOWING IT TO BE ADDED
        //                                    List<DOBase> DO = CurrentBRJob.SelectTaskMaterials(Guid.Parse("48C0380A-C3C2-454D-87A4-805A711A3A26"));
        //                                    foreach (DOTaskMaterial DOBTM in DO)
        //                                    {
        //                                        if (DOBTM.MaterialID == Guid.Parse(strMaterialID))
        //                                        {
        //                                            System.Diagnostics.Debug.WriteLine("DUPLICATE");
        //                                        }


        //                                    }



        //                                    //create new taskmaterial
        //                                    DOTaskMaterial TM = new DOTaskMaterial();

        //                                    TM.TaskMaterialID = id;
        //                                    TM.MaterialName = strMaterialName;
        //                                    TM.MaterialID = Guid.Parse(strMaterialID);
        //                                    TM.Description = "From wholesaler: Corys";                        //Jared to supply supplier name query from supplier table
        //                                    TM.Quantity = decNewQty;
        //                                    TM.TaskID = Guid.Parse("C5162813-8647-421C-9DF3-C3636F905CD1");   //Mandeep to supply current logged in taskid
        //                                    TM.MaterialType = TaskMaterialType.Actual;
        //                                    TM.FromInvoice = true;

        //                                    CurrentBRJob.SaveTaskMaterial(TM);

        //                                }

        //                                    //Jared Need to get customer/site/job/task to populate "assigned to" gv column




        //                                    //create new supplierinvoicematerial

        //                                    DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
        //                                    SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();                                     //As is
        //                                    SIMaterial.MaterialID = Guid.Parse(strMaterialID);                                         //stroriginalmaterialid                    
        //                                    SIMaterial.SupplierInvoiceID = Guid.Parse(strSupplierInvoiceID);                              //stroriginalinvoiceid                    
        //                                    SIMaterial.Qty = decNewQty;                                                             //decNewQty;
        //                                    SIMaterial.ContractorID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");              //Mandeep to supply current logged in company contactID
        //                                    SIMaterial.Assigned = false;                                                               //Jared to change to "True;"
        //                                    SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse(strOriginalSupplierInvoiceMaterialID);                                              //stroriginalsupplierinvoicematerialid


        //                                    if (btn1.ID == "btnAddToTask")
        //                                    {
        //                                        SIMaterial.TaskMaterialID = id;

        //                                    }
        //                                    if (btn1.ID == "btnAddToVehicle")//jared to fix if no records
        //                                    {

        //                                        SIMaterial.QtyRemainingToAssign = decNewQty;


        //                                        //NEED TO CHECK THAT WHEN NEGATIVE THAT THERE IS ALREADY A POSITIVE NUMBER BEFORE
        //                                        //ALLOWING IT TO BE ADDED











        //                                        DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833"));//Jared to use logged in person

        //                                        SIMaterial.VehicleID = V.VehicleID;


        //                                    }

        //                                CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);

        //                                    // System.Diagnostics.Debug.WriteLine("    textbox: " + strNewQty + "   Qty: " + decQtyRemaining);
        //                                    // System.Diagnostics.Debug.WriteLine("MID: " + strMaterialID + "    SIID: " + strSupplierInvoiceID + "   SIMID: " + strOriginalSupplierInvoiceMaterialID);

        //                                    if (decNewQty != decQtyRemaining)
        //                                    {

        //                                        decimal decNewQtyRemaining = decQtyRemaining - decNewQty;

        //                                        CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), decNewQtyRemaining);
        //                                    }
        //                                    else
        //                                    {
        //                                        CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), 0);
        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    //code for when you have entered an amount too high in textbox2 or not +ve for vehicle
        //                                }
        //                            }

        //                        }

        //                    }


        //                }
        //            }



        //        }

        //        //5. alter existing values when supplierinvoicematerial !=  blah blah. order by materialID

















        //protected void gvParent_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("                           gvParent_OnRowdatabound + display...");

        //    LoadgvChildUnassigned(e);//use databind WAS WORKING

        //}




        //protected void ShowHideGV_Click(object sender, EventArgs e)
        //{
        //    if (gvParent.Visible == false)
        //    {
        //        ShowHideGVButton.Text = "Hide";
        //        gvParent.Visible = true;
        //    }
        //    else
        //    {
        //        ShowHideGVButton.Text = "Show";
        //        gvParent.Visible = false;
        //    }

        //    System.Diagnostics.Debug.WriteLine("Import unassigned materials");


        //}








        //---------------------------------------------------------------------------------------------
        //
        //                                   BELOW IS ADDING TO DATABASE FROM CSV FILES
        //
        //---------------------------------------------------------------------------------------------


        protected void CorysDownload(DOMaterialFilePass DOM, string[,] Materials)
        {
            //-------------------
            //CORYS SPECIFIC fill array              Jared to make sure no overload of array
            //-------------------


            //Get each char
            DOM.intChar = (DOM.F.ReadByte());
            DOM.c = (char)DOM.intChar;
            // System.Diagnostics.Debug.Write(c);

            if (DOM.CommaCount > 10)
            {   //line feed(last char of record)
                if (DOM.intChar == 10)
                {
                    DOM.CommaCount = 0;
                    DOM.ColCount = 0;
                    //new row
                    DOM.RowCount++;
                }
            }

            //if char = comma character then... i.e. a new column
            if (DOM.intChar == 44)
            {

                DOM.ColCount++;
                DOM.CommaCount++;

                //last column of the row...

            }
            else //if char != comma
            {
                if (DOM.intChar != 13)//CR
                {
                    if (DOM.intChar != 10)//LF
                    {
                        if (DOM.intChar != 34)// was intCount instead of intchar"
                        {
                            Materials[DOM.RowCount, DOM.ColCount] = Materials[DOM.RowCount, DOM.ColCount] + DOM.c;
                            //
                            // 
                            //
                        }
                    }
                }
            }
            return;
        }
        /////array is full





        //protected void UploadButton_Click(object sender, EventArgs e) commented by Jared 26/4
        //{

        //    if (FileUploadControl.HasFile)//WORK WITH FILE
        //    {

        //        HttpPostedFile G;
        //        HttpFileCollection uploadedFiles = Request.Files;
        //        for (int i = 0; i < uploadedFiles.Count; i++)
        //        {
        //            G = uploadedFiles[i];
        //            long Length = G.ContentLength;
        //        }
        //        G = uploadedFiles[0];

        //        System.IO.Stream F = G.InputStream;

        //        //System.IO.FileStream F = new System.IO.FileStream(@"D:\Projects\Electracraft App\trunk\test2.csv", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);



        //        // alter above

        //        try
        //        {


        //            // if (FileUploadControl.PostedFile.ContentType == "image/jpeg/doc")
        //            //{


        //            //FileUploadControl.SaveAs(Server.MapPath("~/") + fileName);

        //            //StatusLabel.Text = "Upload status: File uploaded!";
        //            //long Length = F.Length;
        //            long Length = F.Length;
        //            string[,] Materials = new string[999, 12];

        //            DOMaterialFilePass DOM = new DOMaterialFilePass();
        //            DOM.F = F;
        //            DOM.intChar = 0;
        //            DOM.ColCount = 0;
        //            DOM.CommaCount = 1;


        //            for (int iCount = 1; iCount <= Length; iCount++)
        //            {
        //                CorysDownload(DOM, Materials);
        //            }



        //            //Mandeep to change this to currentLoggedInUser instead of contactid="eca7b...
        //            List<DOBase> AllContactMaterials = CurrentBRJob.SelectMaterialsbyContactID(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"));//electracraft id
        //            List<DOBase> AllSupplierInvoices = CurrentBRJob.SelectSupplierInvoicesInfoBySupplierID(Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"));//Corys id jARED 

        //            bool ContinueFlag = false;
        //            string strExistingMaterial = "";
        //            //int jCount = 0; 7/4
        //            string StrMaterialID = "";
        //            string StrSuppierInvoice = "";
        //           // string RecentSupplierID = "";

        //            //Loop through all cols and rows...
        //            // LOOP THROUGH ALL THE ROWS
        //            for (int iCount = 0; iCount < DOM.RowCount; iCount++)  
        //            {


        //                ContinueFlag = true;
        //                if (Materials[iCount, 0] == "HDR")   //CHECK hdr line
        //                {

        //                    //SEE IF THIS SUPPLIERINVOICE ALREADY EXISTS
        //                    foreach (DOSupplierInvoice SI in AllSupplierInvoices)
        //                    {

        //                        if (SI.SupplierReference == Materials[iCount, 7])
        //                        {
        //                            ContinueFlag = false;
        //                            System.Diagnostics.Debug.WriteLine("Record already exists: " + SI.SupplierReference);
        //                            break;

        //                        }

        //                    }
        //                }
        //                if (ContinueFlag == true) //SUPPLIERINVOICE DOES NOT ALREADY EXIST
        //                {
        //                    if (Materials[iCount, 0] == "HDR")   //POPULATE SUPPLIERINVOICE TABLE
        //                    {
        //                        System.Diagnostics.Debug.WriteLine("");
        //                       System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------");
        //                       System.Diagnostics.Debug.WriteLine("                       Adding record: " + Materials[iCount, 7] + "    " + Materials[iCount, 8]);
        //                        System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------");
        //                        System.Diagnostics.Debug.WriteLine("");

        //                        //good here. includes 00000000000-0000-0000-0000-0000000000000

        //                        DOSupplierInvoice SupplierInvoice = new DOSupplierInvoice();
        //                        SupplierInvoice.SupplierInvoiceID = Guid.NewGuid();
        //                        StrSuppierInvoice =  SupplierInvoice.SupplierInvoiceID.ToString();
        //                        SupplierInvoice.SupplierID = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB");// Materials[iCount, jCount]; //Corys supplierID
        //                        SupplierInvoice.InvoiceDate = DateTime.Parse(Materials[iCount, 4]);
        //                        SupplierInvoice.ContractorReference = Materials[iCount, 5];
        //                        SupplierInvoice.TotalExGst = decimal.Parse(Materials[iCount, 8]);
        //                        SupplierInvoice.SupplierReference = Materials[iCount, 7];
        //                        SupplierInvoice.ContractorID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");
        //                        //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierID);
        //                        //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierName);
        //                        CurrentBRJob.SaveSupplierInvoice(SupplierInvoice);

        //                        //for (int jCount = 0; jCount < 12; jCount++) //loop through columns
        //                        //{
        //                    }









        //                    //ADDING MATERIALS, SEE IF THEY EXIST FIRST

        //                    if (Materials[iCount, 0] != "HDR") //IF IT IS A LINE ITEM
        //                    {
        //                        //jCount = 0; 7/4
        //                        foreach (DOMaterial mat in AllContactMaterials) //go thru all my materials
        //                        {
        //                            //jCount++; 7/4
        //                            strExistingMaterial = mat.SupplierProductCode;
        //                            string strNewMaterial = Materials[iCount,1];

        //                            if (strExistingMaterial == strNewMaterial)//new and old have the same supplierproductcode
        //                            {
        //                                    //System.Diagnostics.Debug.WriteLine("");
        //                                    //System.Diagnostics.Debug.WriteLine("--------");
        //                                    //System.Diagnostics.Debug.WriteLine(mat.SupplierProductCode + "   This material matches existing item: " + Materials[iCount,1] +  ",  So has NOT been added   " + Materials[iCount, 2]);
        //                                    //System.Diagnostics.Debug.WriteLine("--------");
        //                                    //System.Diagnostics.Debug.WriteLine("");

        //                                    decimal newCostPrice = decimal.Parse(Materials[iCount, 8] + '0');


        //                                if (mat.CostPrice == newCostPrice)//IF SUPPLIERPRODUCTCODE IS SAME AND PRICE IS THE SAME THEN DONT ADD IT
        //                                {
        //                                    StrMaterialID = mat.MaterialID.ToString();
        //                                    ContinueFlag = false; //removed 5/4/16 probably wrong to be here causing only on material per supplierinvoice to be added
        //                                    break;

        //                                }

        //                            }

        //                        }



        //                        if (ContinueFlag == true)//add supplier invoice material to material table if not already there
        //                        {
        //                            System.Diagnostics.Debug.WriteLine("Product added:    " + Materials[iCount, 2] + ",    " + Materials[iCount,1]);

        //                            //add supplier invoice material to material table if not already there
        //                            DOMaterial MaterialItem = new DOMaterial();
        //                            MaterialItem.SupplierID= Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB");
        //                            MaterialItem.MaterialID = Guid.NewGuid();
        //                            StrMaterialID = MaterialItem.MaterialID.ToString();
        //                            MaterialItem.MaterialName = Materials[iCount, 2];
        //                            MaterialItem.SupplierProductCode = Materials[iCount, 1];
        //                            MaterialItem.UOM = Materials[iCount, 5];
        //                            MaterialItem.CostPrice = decimal.Parse(Materials[iCount, 8]);
        //                            MaterialItem.RRP = decimal.Parse(Materials[iCount, 7]);
        //                            //WORK OUT SELL PRICE. - to do logic at some point. user defined variables.





        //                            MaterialItem.SellPrice = MaterialItem.RRP;
        //                            MaterialItem.MaterialCategoryID = Guid.Parse("ccccdddd-dddd-dddd-dddd-ddddccccdddd");
        //                            MaterialItem.ContactID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");
        //                            CurrentBRJob.SaveMaterial(MaterialItem);
        //                            AllContactMaterials.Add(MaterialItem);
        //                            System.Diagnostics.Debug.WriteLine("                          __________________________________");
        //                            System.Diagnostics.Debug.WriteLine("                          Amount of Allcontactmaterials: " +AllContactMaterials.Count());
        //                            System.Diagnostics.Debug.WriteLine("                          __________________________________");



        //                        }

        //                        //add to table SupplierInvoiceMaterial  

        //                        DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
        //                        SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();
        //                        SIMaterial.MaterialID = Guid.Parse(StrMaterialID);
        //                        SIMaterial.SupplierInvoiceID = Guid.Parse(StrSuppierInvoice);
        //                        SIMaterial.Qty = decimal.Parse(Materials[iCount, 6]);
        //                        SIMaterial.QtyRemainingToAssign = decimal.Parse(Materials[iCount, 6]);
        //                        SIMaterial.ContractorID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");
        //                        SIMaterial.Assigned = false;
        //                        CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);

        //                        //1. NEED TO CHECK IF MATERIAL EXISTS IN MATERIALS TABLE. IF NOT THEN ENTER NEW MATERIAL-----done


        //                        //2. SELECT MATERIAL AND ADD NEW ENTRY INTO SUPPLIERINVOICEMATERIAL and CONTAINERINVOICE TABLES and qty. THIS HAPPENS
        //                        //   REGARDLESS IF MATERIAL EXISTS, BUT NOT IF SUPPLIERINVOICE EXISTS




        //                        // System.Diagnostics.Debug.WriteLine(AllMaterials[0]);




        //                    }






        //                }


        //            }

        //            //F.close();
        //        }

        //        //Console.ReadKey();

        //        //   else
        //        // StatusLabel.Text = "Upload status: Only JPEG files are accepted!";


        //        catch (Exception ex)
        //        {
        //            //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message+ex.StackTrace.ToString();

        //            //StatusLabel.Text=
        //        }//
        //        finally
        //        {
        //           // F.Close();
        //        }
        //        LoadgvMainUnassigned();
        //    }//this
    }
}