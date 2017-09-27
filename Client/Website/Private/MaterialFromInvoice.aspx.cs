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


    public partial class MaterialFromInvoice : PageBase
    {
        protected bool CanDelete = false;
        protected DOTask thisTask;

        //Tony added 16.Feb.2017
        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentSessionContext.CurrentEmployee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
        }

        protected void Page_Load(object sender, EventArgs e)//first page load and every postback -happens first
        {
            System.Diagnostics.Debug.WriteLine("PageLoad");

        }

        //protected void btnVehicleSelect_Click(object sender, EventArgs e)
        //{


        //}



        //protected void gvVehicle_PreRender(object sender, EventArgs e)
        //{
        //    LoadVehicleLabels();

        //}










        //----------------------------------------------------------------------------------------------
        //                                 ADD from vehicle to task
        //----------------------------------------------------------------------------------------------
        //1. create New SIM with taskmaterialid
        //2. adjust existing qtyremaining from SIM
        //3. add TM

        //protected void btnAddFromVehicleToTask_Click(object sender, EventArgs e)//NOT COMPLETE
        //{
        //    //jared the two lines below probably not needed - check
        //    Button btn1 = sender as Button;
        //    GridViewRow Row = (GridViewRow)btn1.NamingContainer;
        //    // System.Diagnostics.Debug.WriteLine(Row.RowIndex);


        //    //------------------------------------------------------------------
        //    // CHECK WHICH ARE CHECKED(ticked)
        //    //-------------------------------------------------------------------
        //    bool ContinueFlag = true;
        //    foreach (GridViewRow gvVehicleRow in gvVehicle.Rows)
        //    {
        //        ContinueFlag = true;
        //        CheckBox CB = gvVehicleRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
        //        if (CB.Checked)
        //        {
        //            TextBox TB = gvVehicleRow.FindControl("tbAdd") as TextBox;    //2.
        //            decimal decQtyRemaining = decimal.Parse(gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["QtyRemainingToAssign"].ToString());
        //            string strSupplierInvoiceMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
        //            string strSupplierInvoiceID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceID"].ToString();
        //            string strMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialID"].ToString();
        //            string strOriginalSupplierInvoiceMaterialID = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
        //            string strMaterialName = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialName"].ToString();
        //            string strNewQty = "";

        //            //-------------------------------------------
        //            //        evaluate textbox
        //            //-------------------------------------------
        //            if (TB.Visible == true)
        //            {
        //                String TBtext = TB.Text;




        //                int intCount = 0;
        //                foreach (char c in TB.Text)
        //                {

        //                    strNewQty = strNewQty + c;
        //                    if (c == ',')//fix bug with textbox producing commas. Collects everything after the comma. Stuuuupid Bug
        //                    {
        //                        strNewQty = "";
        //                    }


        //                }
        //                decimal decNewQty = 0;
        //                foreach (char c in strNewQty)//fix double '-' in text or '-' in wrong place
        //                {

        //                    if (c == '-')
        //                    {
        //                        if (intCount != 0)
        //                        {
        //                            ContinueFlag = false;
        //                        }

        //                    }
        //                    if (!decimal.TryParse(strNewQty, out decNewQty))
        //                    {
        //                        ContinueFlag = false;
        //                    }

        //                    intCount++;
        //                }



        //                if (strNewQty == "")
        //                {
        //                    ContinueFlag = false;
        //                }

        //                //---------------------------------------------------------------------------------------
        //                //     ADD TO TASKMATERIAL TABLE
        //                //     UPDATING NEW SUPPLIERMATERIALID RECORDS THAT COME FROM EXISTING RECORDS
        //                //---------------------------------------------------------------------------------------

        //                if (ContinueFlag)
        //                {
        //                    decimal decPosNewQty = Math.Abs(decNewQty); //!make sure the new number is positive
        //                    decimal decPosQtyRemaining = Math.Abs(decQtyRemaining); //make sure the existing number is positive

        //                    decimal decSum = decNewQty + decQtyRemaining; //add new and existing numbers together
        //                    decimal decPosSum = Math.Abs(decSum); // make sure they are positive

        //                    decimal decLimit = 2 * decPosQtyRemaining; // this makes sure that the new number is not greater than the original





        //                    if (decPosSum <= decLimit)
        //                    {
        //                        if (decPosSum == decLimit)
        //                        {

        //                        }
        //                        if (decPosSum >= decPosQtyRemaining && decPosNewQty !=0 )//! CHECK IF VALUES ARE DIFFERENT
        //                        {
        //                            //1. Alter qty remaining value of original SIM
        //                            //2.Create TM
        //                            //3.create SIM with Vehicle and Task populated


        //                            //1. Alter qty remaining value of original SIM
        //                            //DOSupplierInvoiceMaterial SIM = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strSupplierInvoiceMaterialID));
        //                            //SIM.QtyRemainingToAssign = decSum - decQtyRemaining; //possible bug !here!
        //                            //CurrentBRJob.SaveSupplierInvoiceMaterial(SIM);



        //                            //2.Create TM
        //                            Guid id = Guid.NewGuid();
        //                            DOTaskMaterial TM = new DOTaskMaterial();
        //                            TM.TaskMaterialID = id;
        //                            TM.MaterialName = strMaterialName;
        //                            TM.MaterialID = Guid.Parse(strMaterialID);
        //                            TM.Description = "From Jared's vehicle";                        //!here! Jared to supply vehicledriver name query from vehicle table
        //                            TM.Quantity = decNewQty;
        //                            TM.TaskID = Guid.Parse("C5162813-8647-421C-9DF3-C3636F905CD1");   //!here! Mandeep to supply current logged in taskid
        //                            TM.MaterialType = TaskMaterialType.Actual;
        //                            TM.FromInvoice = false;
        //                            TM.FromVehicle = true;

        //                            CurrentBRJob.SaveTaskMaterial(TM);



        //                            //3.create SIM with Vehicle and Task populated
        //                            DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
        //                            SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();                                     //As is
        //                            SIMaterial.MaterialID = Guid.Parse(strMaterialID);                                         //stroriginalmaterialid                    
        //                            SIMaterial.TaskMaterialID = id;
        //                            // SIMaterial.VehicleID= Guid.Parse("08F88AD1-7960-4F57-A899-94BF89792EBD");
        //                            //SIMaterial.SupplierInvoiceID = Guid.Parse("AAAABBBB-AAAA-BBBB-AAAA-BBBBAAAABBBB");         //from a vehicle constant(?)
        //                            SIMaterial.SupplierInvoiceID = Guid.Parse(strSupplierInvoiceID);
        //                            SIMaterial.Qty = decNewQty;                                                             //decNewQty;
        //                            SIMaterial.ContractorID = Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233");              //Mandeep to supply current logged in company contactID !here!
        //                            SIMaterial.Assigned = true;                                                               //Jared to (maybe) change to "True;" !here! 
        //                            SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse(strOriginalSupplierInvoiceMaterialID);                                              //stroriginalsupplierinvoicematerialid
        //                            DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833"));//Jared to use logged in person
        //                            SIMaterial.VehicleID = V.VehicleID;
        //                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);

        //                            if (decNewQty != decQtyRemaining)
        //                            {

        //                                decimal decNewQtyRemaining = decQtyRemaining - decNewQty;

        //                                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), decNewQtyRemaining);
        //                            }
        //                            else
        //                            {
        //                                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), 0);
        //                            }
        //                        }

        //                        else
        //                        {
        //                            //code for when you have entered an amount too high in textbox2
        //                        }
        //                    }

        //                } //

        //            }


        //        }
        //    }



        //}

        //5. alter existing values when supplierinvoicematerial !=  blah blah. order by materialID





















        protected void Page_PreRender(object sender, EventArgs e) //first page load and every postback - happens second
        {
            //WAS WORKING


            System.Diagnostics.Debug.WriteLine("PreRender");
            LoadgvMainUnassigned();
            LoadTitle();
            //LoadgvVehicle();
            //LoadVehicleLabels();


        }



        //protected void LoadgvVehicle()
        //{
        //    DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833"));
        //    DOVehicle V = DOBV as DOVehicle;
        //    List<DOBase> DS = CurrentBRJob.SelectVehicleMaterials(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"),
        //                 V.vehicleDriver, V.VehicleID);

        //    gvVehicle.DataSource = DS;

        //    gvVehicle.DataBind();

        //}

        //protected void LoadVehicleLabels()
        //{

        //    foreach (GridViewRow gvVehicleRow in gvVehicle.Rows)
        //    {
        //        if (gvVehicleRow.RowType == DataControlRowType.DataRow)
        //        {
        //            string strMName = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["MaterialName"].ToString();
        //            string strQty = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["QtyRemainingToAssign"].ToString();
        //            string strCost = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["CostPrice"].ToString();
        //            string strSell = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SellPrice"].ToString();
        //            string strRRP = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["RRP"].ToString();
        //            string strSupplier = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["SupplierName"].ToString();
        //            string strUOM = gvVehicle.DataKeys[gvVehicleRow.RowIndex].Values["UOM"].ToString();



        //            //TextBox tb = (TextBox)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("tbAdd");
        //            Label LQty = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblQtyBody");
        //            Label LMName = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblMNameBody");
        //            Label LCost = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblCostBody");
        //            Label LSell = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblSellBody");
        //            Label LRRP = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblRRPBody");
        //            Label LSupplier = (Label)gvVehicle.Rows[gvVehicleRow.RowIndex].FindControl("lblSupplierBody");

        //            LSupplier.Text = strSupplier;
        //            LRRP.Text = strRRP;
        //            LSell.Text = strSell;
        //            LCost.Text = strCost;
        //            LMName.Text = strMName;
        //            LQty.Text = strQty;



        //        }





        //    }


        //}





        protected void LoadTitle()
        {

            thisTask = CurrentSessionContext.CurrentTask; //jared to add current task id
            if (thisTask != null)
            {
                DOJob thisJob = CurrentSessionContext.CurrentJob;
                DOJobContractor dojc = CurrentBRJob.SelectJobContractor(thisJob.JobID, CurrentSessionContext.CurrentContact.ContactID);
                DOSite thisSite = CurrentSessionContext.CurrentSite;
                string TID = dojc.JobNumberAuto + " " + thisTask.TaskNumber.ToString().PadLeft(3, '0') + ", " + thisTask.TaskName + ", " + thisJob.Name + ", " + thisSite.Address1;



                DOBase DOBt = CurrentBRJob.SelectTask(thisTask.TaskID);
                DOTask DOt = DOBt as DOTask;

                DOBase DOBj = CurrentBRJob.SelectJob(DOt.JobID);
                DOJob DOj = DOBj as DOJob;

                lblTitle.Text = TID;
            }

        }


        protected void LoadgvChildUnassigned(GridViewRowEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("LoadgvChildUnassigned");
            //THIS WAS WORKING FOR POPULATING CHILD DATA
            System.Diagnostics.Debug.WriteLine("gvonrowdatabound databind child start" + DateTime.Now.ToString("ss:fff"));
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvChild = e.Row.FindControl("gvChild") as GridView;
                string SupplierReference = gvParent.DataKeys[e.Row.RowIndex].Value.ToString();
                List<DOBase> LineItem = CurrentBRJob.SelectMaterialsByContactIDAndSupplierInvoiceReferenceAndAssigned(CurrentSessionContext.CurrentContact.ContactID, SupplierReference, false);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
                gvChild.DataSource = LineItem;
                gvChild.DataBind();
            }

            //populate text2, hide/show "Redo" button
            System.Diagnostics.Debug.WriteLine("gvonrowdatabound foreach start " + DateTime.Now.ToString("ss:fff"));
            foreach (GridViewRow gvParentRow in gvParent.Rows)
            {
                GridView gvChild = (GridView)gvParentRow.FindControl("gvChild");
                System.Diagnostics.Debug.WriteLine("");
                foreach (GridViewRow gvChildRow in gvChild.Rows)
                {
                    string strOriginalSupplierInvoiceMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["OldSupplierInvoiceMaterialID"].ToString();
                    TextBox tb = (TextBox)gvChild.Rows[gvChildRow.RowIndex].FindControl("textbox2");
                    Label L = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblQtyBody");
                    Label LTB = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblTextBox2");
                    Button btnFooterAddToTask = (Button)gvChild.FooterRow.FindControl("btnAddToTask");
                    Button btnHeaderAddToTask = (Button)gvChild.HeaderRow.FindControl("btnAddToTask");
                    if (thisTask == null)
                    {
                        btnHeaderAddToTask.Visible = false;  //comes from home screen
                        btnFooterAddToTask.Visible = false;  //comes from home screen
                    }
                    tb.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString();


                    if (strOriginalSupplierInvoiceMaterialID == "00000000-0000-0000-0000-000000000000")//Original supplierInvoiceMaterial
                    {
                        if (tb.Text == "0")
                        {
                            tb.ForeColor = System.Drawing.Color.Gray;
                            L.ForeColor = System.Drawing.Color.Gray;
                            foreach (TableCell TC in gvChildRow.Cells)
                            {

                                TC.ForeColor = System.Drawing.Color.Gray;
                                TC.Enabled = false;



                            }
                        }

                        Button B = (Button)gvChild.Rows[gvChildRow.RowIndex].FindControl("btnRedo");
                        B.Visible = false;
                        //string cell_2_Value = gvChild.DataKeys[gvChildRow.RowIndex].["Qty"].ToString(); commented 6/4/16 Jared
                       
                        //analogous
                        L.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString() + " / " + gvChild.DataKeys[gvChildRow.RowIndex].Values["Qty"].ToString();



                    }
                    else//SET PROPERTIES OF UnEDITABLE CELLS
                    {

                        foreach (TableCell TC in gvChildRow.Cells)
                        {
                            tb.ForeColor = System.Drawing.Color.LightGray;
                            LTB.ForeColor = System.Drawing.Color.LightGray;
                            L.ForeColor = System.Drawing.Color.LightGray;
                            TC.ForeColor = System.Drawing.Color.LightGray;
                            //assign lblQtyBody


                        }
                        Label lblAssignedTo = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblAssignedTo");
                        CheckBox cb = (CheckBox)gvChild.Rows[gvChildRow.RowIndex].FindControl("chkSelect");
                        LTB.Text = gvChild.DataKeys[gvChildRow.RowIndex].Values["Qty"].ToString();
                        LTB.Visible = true;
                        cb.Checked = false;
                        cb.Visible = false;
                        tb.Visible = false;

                        //string TID = gvChild.DataKeys[gvChildRow.RowIndex].Values["TaskID"].ToString();
                        string TMID = gvChild.DataKeys[gvChildRow.RowIndex].Values["TaskMaterialID"].ToString();
                        string VID = gvChild.DataKeys[gvChildRow.RowIndex].Values["VehicleID"].ToString();
                        string OldSimID = gvChild.DataKeys[gvChildRow.RowIndex].Values["OldSupplierInvoiceMaterialID"].ToString();


                        //When the supplierinvoicematerial is pointing to a task material
                        if (TMID != "00000000-0000-0000-0000-000000000000")
                        {
                            DOBase DOBtm = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(TMID));
                            DOTaskMaterial DOtm = DOBtm as DOTaskMaterial;

                            DOBase DOBt = CurrentBRJob.SelectTask(DOtm.TaskID);
                            DOTask DOt = DOBt as DOTask;

                            DOBase DOBj = CurrentBRJob.SelectJob(DOt.JobID);
                            DOJob DOj = DOBj as DOJob;

                            string TaskNumber = "";
                            if (DOt.TaskNumber > 100) TaskNumber = "0";
                            if (DOt.TaskNumber > 10) TaskNumber = "00";
                            TaskNumber=TaskNumber+DOt.TaskNumber.ToString();

                            DOJobContractor dojc = CurrentBRJob.SelectJobContractor(DOj.JobID, CurrentSessionContext.CurrentContact.ContactID);

                            lblAssignedTo.Text = DOt.TaskName + ", (" + DOj.Name + ")" + dojc.JobNumberAuto + '-' + TaskNumber;
                        }
                        //When the supplierinvoicematerial is pointing to a vehicle
                        if (VID != "00000000-0000-0000-0000-000000000000")
                        {

                            //DOBase OldSupplierInvoiceMaterial = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strOldSupplierInvoiceMaterialID));
                            //DOSupplierInvoiceMaterial dos = OldSupplierInvoiceMaterial as DOSupplierInvoiceMaterial;

                            DOBase a = CurrentBRJob.SelectVehicleByVehicleID(Guid.Parse( VID));

                            //DOBase a = CurrentBRJob.SelectVehicle( CurrentSessionContext.CurrentContact.ContactID); //jared to provide logged in id
                            DOVehicle V = a as DOVehicle;

                            //Tony commented 5.Mar.2017 
//                            DOBase b = CurrentBRContact.SelectContact(V.vehicleDriver);
//                            DOContact C = b as DOContact;

                            // Tony modified 5.Mar.2017
                            DOBase b = CurrentBRContact.SelectEmployeeInfo(V.vehicleDriver);
                            DOEmployeeInfo E = b as DOEmployeeInfo;

                            lblAssignedTo.Text = E.FirstName + "'s Vehicle. ";
                        }

                        if (TMID == "00000000-0000-0000-0000-000000000000")
                        {
                            if (VID == "00000000-0000-0000-0000-000000000000")
                            {
                                if (OldSimID != "00000000-0000-0000-0000-000000000000")
                                {
                                    lblAssignedTo.Text = "Returned";
                                    //todo more info here. Probably a link to the returned invoice
                                }
                            }
                        }
                    }
                }
            }



        }


        protected void LoadgvMainUnassigned()//Load list for gvMain
        {
            TextBox T = (TextBox)txtFromDate;

            //WORKING 
            CheckBox CBDeleted = (CheckBox)chkFilterDeleted;
            CheckBox CBAssigned = (CheckBox)chkFilterAssigned;
            List<DOBase> AllSupplierInvoicesUnAssigned = new List<DOBase>();
            //this is filtering between invoices that have been assigned and not.
            if (CBDeleted.Checked)
            {//show deleted SupplierInvoices

                AllSupplierInvoicesUnAssigned = CurrentBRJob.SelectSupplierInvoicesOrderByCRefInActive(CurrentSessionContext.CurrentContact.ContactID);//electracraft id
                
            }
            else
            {//Show non-deleted invoices
                if (CBAssigned.Checked)
                {
                    AllSupplierInvoicesUnAssigned = CurrentBRJob.SelectSupplierInvoicesOrderByCRef(CurrentSessionContext.CurrentContact.ContactID, "=0");//electracraft id
                }
                else
                {
                    AllSupplierInvoicesUnAssigned = CurrentBRJob.SelectSupplierInvoicesOrderByCRef(CurrentSessionContext.CurrentContact.ContactID, ">0");//electracraft id
                }
            }         

            gvParent.DataSource = AllSupplierInvoicesUnAssigned;
            gvParent.DataBind();

            DOContact c = CurrentSessionContext.Owner as DOContact;
            

            //todo need to group sc's and si's

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags; 
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.DeleteInvoices) == CompanyPageFlag.DeleteInvoices)
            {
               
                Guid SupplierInvoiceID = Guid.Parse(gvParent.DataKeys[rowIndex].Values[1].ToString());
                if (chkFilterDeleted.Checked)
                {
                   
                    //change record to active
                    
                    CurrentBRJob.UpdateSupplierInvoiceToActive(SupplierInvoiceID);
                }
                else
                { 
                   
                    //change record to inactive
                    CurrentBRJob.UpdateSupplierInvoiceToInActive(SupplierInvoiceID);
                }
            }
            

        }

        protected void JobFinder_Click(object sender, EventArgs e)
        {
           
          
        }

        protected void btnRedo_Click(object sender, EventArgs e)
        {

            //FIND CONTROL ROW
            //GET 'CHILD' LINE ITEM VALUE
            // SET 'PARENT LINE ITEM QTYREMAININGTOASSIGN + CHILDQTY
            //MAYBE SORT COLOURS

            GridViewRow gvChildRow = (GridViewRow)((Button)sender).NamingContainer;
            GridView gvChild = (GridView)(gvChildRow.Parent.Parent);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

            //Get the value of column from the DataKeys using the RowIndex.
            string strOldSupplierInvoiceMaterialID = (gvChild.DataKeys[rowIndex].Values["OldSupplierInvoiceMaterialID"].ToString());
            string strCurrentSupplierInvoiceMaterialID = (gvChild.DataKeys[rowIndex].Values["SupplierInvoiceMaterialID"].ToString());
            
            string strCurrentTaskMaterialID = (gvChild.DataKeys[rowIndex].Values["TaskMaterialID"].ToString());
            string strCurrentVehicleID = (gvChild.DataKeys[rowIndex].Values["VehicleID"].ToString());
            string strSupplierInvoiceID = (gvChild.DataKeys[rowIndex].Values["SupplierInvoiceID"].ToString());
            decimal decQty = (decimal.Parse(gvChild.DataKeys[rowIndex].Values["Qty"].ToString()));
            decimal decRemainingQty = (decimal.Parse(gvChild.DataKeys[rowIndex].Values["QtyRemainingToAssign"].ToString()));
            string strMatchID = (gvChild.DataKeys[rowIndex].Values["MatchID"].ToString());
            Label lblAssignedTo = (Label)gvChild.Rows[gvChildRow.RowIndex].FindControl("lblAssignedTo");
            DOTaskMaterial myTM = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(strCurrentTaskMaterialID));


            if (strMatchID == "00000000-0000-0000-0000-000000000000")

            {
                //set the qtyremainingtoassign to qty
                DOBase OldSupplierInvoiceMaterial = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strOldSupplierInvoiceMaterialID));//issue here
                DOSupplierInvoiceMaterial oldSim = OldSupplierInvoiceMaterial as DOSupplierInvoiceMaterial;

                //This logic is to prevent redo-ing a SIM assigned to a vehicle where the vehicle SIM has been assigned to a task already. OR has been invoiced
                if (strCurrentVehicleID != "00000000-0000-0000-0000-000000000000" ||  myTM.InvoiceID != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    if (decQty != decRemainingQty)
                    {
                        //todo !here! message to say that you cant redo because these have been assigned from the vehicle. What about task??
                        
                        return;
                    }
                }

                //Remove entry
                decimal decTotalQty = decQty + oldSim.QtyRemainingToAssign;
                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOldSupplierInvoiceMaterialID), decTotalQty);
                
                //UPDATED BELOW 13/9
                //updated from deleting SIM to changing QtyRTA to zero in order to keep the SIM that you REDO
                ////delete old supplierinvoicematerial record strCurrentSupplierInvoiceMaterialID
                //DOBase o = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strCurrentSupplierInvoiceMaterialID));
                //DOSupplierInvoiceMaterial o1 = o as DOSupplierInvoiceMaterial;
                //CurrentBRJob.DeleteSupplierInvoiceMaterial(o1);


                DOBase o = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strCurrentSupplierInvoiceMaterialID));
                DOSupplierInvoiceMaterial o1 = o as DOSupplierInvoiceMaterial;
                CurrentBRJob.UpdateSupplierInvoiceMaterialAssigned(o1.SupplierInvoiceMaterialID, 1);

                //commented below because instead of setting to zero, we are just deleting the record 25/9/16
                //CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(o1.SupplierInvoiceMaterialID,0);


                //only increment the status when all the line item qty has been assign. Not parts of it
                //if (o1.QtyRemainingToAssign == o1.Qty)
                if (oldSim.QtyRemainingToAssign==0 && o1.Qty>0)
                {
                    CurrentBRJob.UpdateSupplierInvoiceStatus(Guid.Parse(strSupplierInvoiceID), "+");
                }
                if(oldSim.OldSupplierInvoiceMaterialID!= Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    CurrentBRJob.UpdateSupplierInvoiceMaterialNewQty(oldSim.SupplierInvoiceMaterialID, decTotalQty + oldSim.Qty);
                    CurrentBRJob.UpdateSupplierInvoiceMaterialNewOldSIMID(oldSim.SupplierInvoiceMaterialID, Guid.Parse("00000000-0000-0000-0000-000000000000"));
                }

                //added below in place of line 590
                CurrentBRJob.DeleteSupplierInvoiceMaterial(o1);

                //delete taskmaterial
                //if (strCurrentTaskMaterialID != "00000000-0000-0000-0000-000000000000" || strOldSupplierInvoiceMaterialID == "00000000-0000-0000-0000-000000000000")//maybe this

               
                if (strCurrentTaskMaterialID != "00000000-0000-0000-0000-000000000000")
                {
                    DOBase a = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(strCurrentTaskMaterialID));
                    DOTaskMaterial a1 = a as DOTaskMaterial;
                    CurrentBRJob.DeleteTaskMaterial(a1);
                }
                //if (strCurrentVehicleID != 00000000-0000-0000-0000-000000000000)
                //{
                //    DOBase a = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(strCurrentTaskMaterialID));
                //    DOTaskMaterial a1 = a as DOTaskMaterial;
                //    CurrentBRJob.DeleteTaskMaterial(a1);
                //}



                //some story about cant delete this because some of the product has been assigned to a task already

            }
            else //sorting out "returned and matched" invoices
            {
                //0.get both ID's
               


                DOBase A1 = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strCurrentSupplierInvoiceMaterialID));
                DOSupplierInvoiceMaterial DeletedsimA = A1 as DOSupplierInvoiceMaterial;
                DOBase B1 = CurrentBRJob.SelectSupplierInvoiceMaterial(Guid.Parse(strMatchID));
                DOSupplierInvoiceMaterial DeletedsimB = B1 as DOSupplierInvoiceMaterial;

                DOBase A = CurrentBRJob.SelectSupplierInvoiceMaterial(DeletedsimA.OldSupplierInvoiceMaterialID);
                DOSupplierInvoiceMaterial simA = A as DOSupplierInvoiceMaterial;
                DOBase B = CurrentBRJob.SelectSupplierInvoiceMaterial(DeletedsimB.OldSupplierInvoiceMaterialID);
                DOSupplierInvoiceMaterial simB = B as DOSupplierInvoiceMaterial;


                ////1.change qtyremaintoassign to qty and set oldSIMID to 0's
                //CurrentBRJob.UpdateSupplierInvoiceMaterialNewOldSIMID(simA.SupplierInvoiceMaterialID, Guid.Parse("00000000-0000-0000-0000-000000000000"));
                //CurrentBRJob.UpdateSupplierInvoiceMaterialNewOldSIMID(simB.SupplierInvoiceMaterialID, Guid.Parse("00000000-0000-0000-0000-000000000000"));

                //update qtys and delete old matches
                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(simA.SupplierInvoiceMaterialID,  DeletedsimA.Qty+simA.QtyRemainingToAssign);
                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(simB.SupplierInvoiceMaterialID, DeletedsimB.Qty+simB.QtyRemainingToAssign);

                CurrentBRJob.DeleteSupplierInvoiceMaterial(DeletedsimA);
                CurrentBRJob.DeleteSupplierInvoiceMaterial(DeletedsimB);

                if (DeletedsimA.Qty + simA.QtyRemainingToAssign != simA.Qty || simA.QtyRemainingToAssign==0)
                {
                    CurrentBRJob.UpdateSupplierInvoiceStatus(simA.SupplierInvoiceID, "+");
                   
                }

                if (DeletedsimB.Qty + simB.QtyRemainingToAssign != simB.Qty || simB.QtyRemainingToAssign==0)
                {
                    CurrentBRJob.UpdateSupplierInvoiceStatus(simB.SupplierInvoiceID, "+");
                }

                //CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(simB.SupplierInvoiceMaterialID, simB.Qty);

                // with the line below we need to use it only if the qty going back to the original SIM is making that sim complete. i.e. qty=qtyRTA. Just not sure where to put it yet.
                //CurrentBRJob.UpdateSupplierInvoiceStatus(simB.SupplierInvoiceID, "+");

                //if (lblAssignedTo.Text=="Returnedssssssssssssssssss")
                //    {
                //    CurrentBRJob.UpdateSupplierInvoiceStatus(simA.SupplierInvoiceID, "+");
                //    CurrentBRJob.UpdateSupplierInvoiceStatus(simB.SupplierInvoiceID, "+");



                //    //code below is to change the Qty after a matching. When a matching occurs and part of the +ve amount is assigned then the qty is reduced for the positive match.
                //    // this needs to be fixed during freeing up "redoing" the match
                //    if (simA.Qty > 0)
                //    {
                //        decimal decSum = 0;
                //        List<DOBase> oldSims = CurrentBRJob.SelectSupplierInvoiceMaterialsByOldSIM(simA.SupplierInvoiceMaterialID);
                //        if (oldSims != null)
                //        {

                //            foreach (DOBase DOI in oldSims)
                //            {
                //                DOSupplierInvoiceMaterial SIM = DOI as DOSupplierInvoiceMaterial;
                //                decSum = decSum + SIM.Qty;
                //            }


                //        }


                //        simA.Qty = simA.Qty + decSum;
                //        CurrentBRJob.UpdateSupplierInvoiceMaterialNewQty(simA.SupplierInvoiceMaterialID, simA.Qty);
                //        //check all oldsims and sum them

                //    }

                //    if (simB.Qty > 0)
                //    {
                //        decimal decSum = 0;
                //        List<DOBase> oldSims = CurrentBRJob.SelectSupplierInvoiceMaterialsByOldSIM(simB.SupplierInvoiceMaterialID);
                //        if (oldSims != null)
                //        {

                //            foreach (DOBase DOI in oldSims)
                //            {
                //                DOSupplierInvoiceMaterial SIM = DOI as DOSupplierInvoiceMaterial;
                //                //  decSum = decSum + SIM.Qty;

                //                decSum = decSum + SIM.Qty;
                //            }


                //        }

                //        simB.Qty = simB.Qty + decSum;
                //        CurrentBRJob.UpdateSupplierInvoiceMaterialNewQty(simB.SupplierInvoiceMaterialID, simB.Qty);
                //    }


                //}


            }

        }


        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            decimal value;
            if (!decimal.TryParse(t.Text, out value))
                t.BackColor = System.Drawing.Color.Red;
        }

        protected void btnMatch_Click(object sender, EventArgs e)
        {
           



                }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentTask != null) Response.Redirect(Constants.URL_TaskSummary);
            else Response.Redirect(Constants.URL_Home);
        }




        protected void btnSelect_Click(object sender, EventArgs e)//NOT COMPLETE because we need to avoid postback jared to fix
        {
            foreach (GridViewRow gvParentRow in gvParent.Rows)
            {
                GridView gvChild = gvParentRow.FindControl("gvChild") as GridView;

                foreach (GridViewRow gvChildRow in gvChild.Rows)
                {
                    CheckBox CB = gvChildRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
                    if (CB.Checked == true)
                    {
                        CB.Checked = false;
                    }
                    else
                    {
                        CB.Checked = true;
                    }


                }
            }
        }



        public class MatchItem //: IEquatable<MatchList>
        {
            public Guid materialid { get; set; }
            public decimal qty { get; set; }
            public Guid SIM { get; set; }
            public Guid SI { get; set; }
            //public string value()
            //    {
            //    string s = "";
            //    s = materialid.ToString() + qty.ToString();

            //    return s;
            //    }



            //public bool comparestuff(cItem ci)
            //{
            //    string s = materialid.ToString() + qty.ToString();
            //    if (ci.value()==s)
            //    {
            //        return true;
            //    }

            //    return false;


            //}
        }

        protected void chk_Changed(object sender, EventArgs e)
        {
             

        }



        //------------------------------------------------------
        //              ADD TO TASK or VEHICLE
        //------------------------------------------------------
        protected void btnAdd_Click(object sender, EventArgs e)//NOT COMPLETE
        {
            //Need to limit "add to task" to just modify the grid where it is not all the grids.
            List<Guid> SimID = new List<Guid>();



            //jared the two lines below probably not needed - check
            Button btn1 = sender as Button;
         
                GridViewRow Row = (GridViewRow)btn1.NamingContainer;

            // System.Diagnostics.Debug.WriteLine(Row.RowIndex);

            List<MatchItem> MatchItemlist = new List<MatchItem>();
            List<Guid> gRemoveMaterials = new List<Guid>();
                //------------------------------------------------------------------
                //1. CHECK WHICH ARE CHECKED(ticked)
                //-------------------------------------------------------------------
                bool ContinueFlag = true;
                //List<DOSupplierInvoiceMaterial> MatchList = new List<DOSupplierInvoiceMaterial>();
                foreach (GridViewRow gvParentRow in gvParent.Rows)
                {
                    GridView gvChild = gvParentRow.FindControl("gvChild") as GridView;


                    foreach (GridViewRow gvChildRow in gvChild.Rows)
                    {

                        ContinueFlag = true;
                        CheckBox CB = gvChildRow.FindControl("chkSelect") as CheckBox; //2. Get values from text and datafield
                    DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
                    if (CB.Checked == true)
                    {
                        TextBox TB = gvChildRow.FindControl("TextBox2") as TextBox;    //2.

                        //string strOriginalQty = gvChild.Rows[gvChildRow.RowIndex].Cells[1].Text; //GET DATABOUND ID=QTY VALUE
                        //string strOriginalMaterialID = gvChild.Rows[gvChildRow.RowIndex].Cells[3].Text;
                        //string strOriginalSupplierInvoiceID = gvChild.Rows[gvChildRow.RowIndex].Cells[4].Text; 
                        //string strOriginalSupplierInvoiceMaterialID = gvChild.Rows[gvChildRow.RowIndex].Cells[6].Text;
                        

                        decimal decQtyRemaining = decimal.Parse(gvChild.DataKeys[gvChildRow.RowIndex].Values["QtyRemainingToAssign"].ToString());
                        string strSupplierInvoiceID = gvChild.DataKeys[gvChildRow.RowIndex].Values["SupplierInvoiceID"].ToString();
                        string strMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["MaterialID"].ToString();
                        string strOriginalSupplierInvoiceMaterialID = gvChild.DataKeys[gvChildRow.RowIndex].Values["SupplierInvoiceMaterialID"].ToString();
                        string strMaterialName = gvChild.DataKeys[gvChildRow.RowIndex].Values["MaterialName"].ToString();
                       




                        //SELECT ONLY THE SIM COPYS

                        string strNewQty = "";
                        if (TB.Visible == true)
                        {
                            int intCount = 0;
                            //WORK OUT THE NEW VALUE + FORMATTING AND CHECKING FOR TYPO'S
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





                            //decimal decNewQty = decimal.Parse(strNewQty);



                            //---------------------------------------------------------------------------------------
                            //     ADD TO TASKMATERIAL TABLE
                            //     UPDATING NEW SUPPLIERMATERIALID RECORDS THAT COME FROM EXISTING RECORDS
                            //---------------------------------------------------------------------------------------



                           

                            if (btn1.ID == "btnMatchReturn")
                            {
                                //matching code
                                MatchItem ML = new MatchItem();
                                ML.materialid = Guid.Parse(strMaterialID);
                                ML.qty = decQtyRemaining;
                                ML.SIM = Guid.Parse(strOriginalSupplierInvoiceMaterialID);
                                ML.SI = Guid.Parse(strSupplierInvoiceID);
                                MatchItemlist.Add(ML);

                              
                               
                            }
                            else
                            {
                                //DEALING WITH NEGATIVES
                                if (ContinueFlag && decQtyRemaining > 0)
                                {
                                    DOTask thisTask = CurrentSessionContext.CurrentTask;
                                    decimal decPosNewQty = Math.Abs(decNewQty);//make sure number is positive
                                    decimal decPosQtyRemaining = Math.Abs(decQtyRemaining);
                                    decimal decSum = decNewQty + decQtyRemaining;
                                    decimal decPosSum = Math.Abs(decSum);
                                    decimal decLimit = 2 * decPosQtyRemaining;
                                    if (decPosSum <= decLimit)
                                    {
                                        if (decPosSum >= decPosQtyRemaining)//3. CHECK IF VALUES ARE DIFFERENT
                                        {
                                            //4. FOR ALL DIFFERENT CREATE A NEW SUPPLIERINVOICEMATERIAL RECORD AND ASSIGN TASK and assign current supplierinvoicematerialid as oldsupplierinvoicematerialid
                                            //create new supplierinvoicematerial
                                            SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();                                     //As is
                                            SIMaterial.MaterialID = Guid.Parse(strMaterialID);                                         //stroriginalmaterialid                    
                                            SIMaterial.SupplierInvoiceID = Guid.Parse(strSupplierInvoiceID);                              //stroriginalinvoiceid                    
                                            SIMaterial.Qty = decNewQty;                                                             //decNewQty;
                                            SIMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                                            SIMaterial.Assigned = false;                                                               //Jared to change to "True;" !here!
                                            SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse(strOriginalSupplierInvoiceMaterialID);                                              //stroriginalsupplierinvoicematerialid
                                            SIMaterial.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                            SIMaterial.CreatedDate = DateTime.Now;

                                            Guid id = Guid.NewGuid();
                                            if (btn1.ID == "btnAddToTask")
                                            {
                                                //NEED TO CHECK THAT WHEN NEGATIVE THAT THERE IS ALREADY A POSITIVE NUMBER BEFORE
                                                //ALLOWING IT TO BE ADDED
                                                List<DOBase> DO = CurrentBRJob.SelectTaskMaterials(thisTask.TaskID);
                                                foreach (DOTaskMaterial DOBTM in DO)
                                                {
                                                    if (DOBTM.MaterialID == Guid.Parse(strMaterialID))
                                                    {
                                                        System.Diagnostics.Debug.WriteLine("DUPLICATE");
                                                    }


                                                }

                                                //create new taskmaterial
                                                DOMaterial material = CurrentBRJob.SelectMaterial(Guid.Parse(strMaterialID));

                                                DOTaskMaterial TM = new DOTaskMaterial();

                                                TM.TaskMaterialID = id;
                                                TM.MaterialName = strMaterialName;
                                                TM.MaterialID = Guid.Parse(strMaterialID);
                                                TM.Description = "From wholesaler: Corys";                        //Jared to supply supplier name query from supplier table
                                                TM.Quantity = decNewQty;
                                                TM.InvoiceQuantity = decNewQty;
                                                TM.TaskID = thisTask.TaskID;
                                                TM.SellPrice = material.SellPrice;
                                                TM.MaterialType = TaskMaterialType.Actual;
                                                TM.FromInvoice = true;
                                                TM.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                                TM.CreatedDate = DateTime.Now;
                                                CurrentBRJob.SaveTaskMaterial(TM);

                                                SIMaterial.TaskMaterialID = id;

                                            }
                                            if (btn1.ID == "btnAddToVehicle")//jared to fix if no records
                                            {
                                                //Jared 9/3/17
                                                //DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
                                                DOVehicle V = CurrentBRJob.SelectDefaultVehicleByDriverID(CurrentSessionContext.CurrentEmployee.EmployeeID);
                                                //Session owner has a Vehicle:
                                              
                                              
                                                if (V != null)
                                                {
                                                    SIMaterial.QtyRemainingToAssign = decNewQty;
                                                    //NEED TO CHECK THAT WHEN NEGATIVE THAT THERE IS ALREADY A POSITIVE NUMBER BEFORE
                                                    //ALLOWING IT TO BE ADDED

                                                    SIMaterial.VehicleID = V.VehicleID;
                                                }
                                                else
                                                {
                                                    return;
                                                }

                                            }
                                            
                                           // CurrentBRJob.UpdateSupplierInvoiceStatus(SIMaterial.SupplierInvoiceID, "+");

                                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);

                                            // System.Diagnostics.Debug.WriteLine("    textbox: " + strNewQty + "   Qty: " + decQtyRemaining);
                                            // System.Diagnostics.Debug.WriteLine("MID: " + strMaterialID + "    SIID: " + strSupplierInvoiceID + "   SIMID: " + strOriginalSupplierInvoiceMaterialID);

                                            if (decNewQty != decQtyRemaining)
                                            {

                                                decimal decNewQtyRemaining = decQtyRemaining - decNewQty;

                                                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), decNewQtyRemaining);
                                            }
                                            else
                                            {
                                                CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid.Parse(strOriginalSupplierInvoiceMaterialID), 0);
                                                CurrentBRJob.UpdateSupplierInvoiceStatus(Guid.Parse(strSupplierInvoiceID), "-");

                                            }
                                        }

                                        else
                                        {
                                            //code for when you have entered an amount too high in textbox2 or not +ve for vehicle
                                        }
                                    }

                                }

                            }


                        }
                    }
                    }



                }
            //deal with matches from matchlist
            if (btn1.ID == "btnMatchReturn" && MatchItemlist != null)
            {


                foreach (MatchItem itemA in MatchItemlist)
                {
                    foreach(MatchItem itemB in MatchItemlist)
                    {
                       
                        
                        
                        //todo find out which is positive and for the positive that the -ve number is at least the same as the positive number, so we can match even if the numbers arent the same.


                        //match condition
                        if(-itemB.qty == itemA.qty && itemB.materialid==itemA.materialid && itemB.SIM != itemA.SIM)
                        {
                            DOSupplierInvoiceMaterial SIMaterialA = new DOSupplierInvoiceMaterial();
                            DOSupplierInvoiceMaterial SIMaterialB = new DOSupplierInvoiceMaterial();
                            SIMaterialB.SupplierInvoiceMaterialID = Guid.NewGuid();
                            SIMaterialA.SupplierInvoiceMaterialID = Guid.NewGuid();              
                            SIMaterialA.MaterialID = itemA.materialid;                           
                            SIMaterialA.SupplierInvoiceID = itemA.SI;                            
                            SIMaterialA.Qty = itemA.qty;                                                             
                            SIMaterialA.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                            SIMaterialA.Assigned = false;                                        
                            SIMaterialA.OldSupplierInvoiceMaterialID = itemA.SIM;                
                            SIMaterialA.MatchID = SIMaterialB.SupplierInvoiceMaterialID;
                            SIMaterialA.CreatedBy = CurrentSessionContext.Owner.ContactID;
                            SIMaterialA.CreatedDate = DateTime.Now;

                          
                            SIMaterialB.MaterialID = itemB.materialid;
                            SIMaterialB.SupplierInvoiceID = itemB.SI;
                            SIMaterialB.Qty = itemB.qty;
                            SIMaterialB.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                            SIMaterialB.Assigned = false;
                            SIMaterialB.OldSupplierInvoiceMaterialID = itemB.SIM;
                            SIMaterialB.MatchID = SIMaterialA.SupplierInvoiceMaterialID;
                            SIMaterialB.CreatedBy = CurrentSessionContext.Owner.ContactID;
                            SIMaterialB.CreatedDate = DateTime.Now;

                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterialB);
                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterialA);


                            //set qty to qtyremainingtoassign
                            //if (itemA.qty > 0) CurrentBRJob.UpdateSupplierInvoiceMaterialNewQty(itemA.SIM, itemA.qty);
                            //if (itemB.qty > 0) CurrentBRJob.UpdateSupplierInvoiceMaterialNewQty(itemB.SIM, itemB.qty);

                            ////gRemoveMaterials.Add(ci1.SIM);
                            ////gRemoveMaterials.Add(ci2.SIM);

                            CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(itemA.SIM, 0);
                            CurrentBRJob.UpdateSupplierInvoiceMaterialNewQtyRemaining(itemB.SIM, 0);


                            //CurrentBRJob.UpdateSupplierInvoiceMaterialNewOldSIMID(itemA.SIM, itemB.SIM);
                            //CurrentBRJob.UpdateSupplierInvoiceMaterialNewOldSIMID(itemB.SIM, itemA.SIM);

                            //this is a flag in the SupplierInvoice table used to hide or show invoices if they are completely assigned or not.
                            CurrentBRJob.UpdateSupplierInvoiceStatus(itemA.SI, "-");
                            CurrentBRJob.UpdateSupplierInvoiceStatus(itemB.SI, "-");
                            //make sure that this loop isnt entered again for these 2 entries
                            itemB.SIM = itemA.SIM;

                        }

                    }



                }
               

            }
          
              
            

                //5. alter existing values when supplierinvoicematerial !=  blah blah. order by materialID














            }


        protected void gvParent_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("                           gvParent_OnRowdatabound + display...");
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine("gvonrowdatabound " + DateTime.Now.ToString("ss:fff"));
            LoadgvChildUnassigned(e);//use databind WAS WORKING
            
         
        }




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

        protected void btnSearchJobNumber_Click(object sender, EventArgs e)
        {
            int result=-1;
            int intJobNumber;
            int.TryParse(txtSearchJobNumber.Text,out result);
            if(result!=-1)
            {
                DOJobContractor dojc = CurrentBRJob.FindJob(result, CurrentSessionContext.CurrentContact.ContactID);
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(dojc.JobID);
                if(CurrentSessionContext.CurrentJob != null)
                Response.Redirect(Constants.URL_JobSummary + "?jobid=" + CurrentSessionContext.CurrentJob.JobID);
            }
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

 