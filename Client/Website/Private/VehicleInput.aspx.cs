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
using System.Data;
//using System.Collections;
//using System.Data.SqlClient;


namespace Electracraft.Client.Website
{


    public partial class VehicleInput : PageBase
    {
        //Strings to hold dates to update date textboxes when "edit" button pressed
        private string WOFDueDateString, RegoDueDateString, InsDueDateString, VehNameString, VehRegoString;
        //Static list of drivers for binding to dd list when editing or adding new vehicle.
        static private List<string> driverNamesList;
        private bool GridViewHasNoData = false;

        //Tony added 21.Feb.2017
        const string DEFAULT_VEHICLE_CHANGED = "Default vehicle changed from ";


        protected void Page_Load(object sender, EventArgs e)//first page load and every postback
        {
            if (!IsPostBack)
            {
                driverNamesList = new List<string>();// re-setting static list every time page loads.
                LoadDriverList();
                PopulateGridView();
                divTransfer.Visible = false;
                divError.Visible = false;
            }

            //System.Diagnostics.Debug.WriteLine(CurrentSessionContext.CurrentContact.CompanyName.ToString() + CurrentSessionContext.CurrentContact.ContactID.ToString());
        }

        //Loads data to dropdown lists when transfering between vehicles.
        protected void LoadTransferDropDownLists(DOVehicle vehicle)
        {
            List<DOForVehicleInput> vehList = Session["GridData"] as List<DOForVehicleInput>;

            ddTransferFrom.DataSource = vehList;
            ddTransferFrom.DataTextField = "VehicleName";
            ddTransferFrom.DataValueField = "VehicleID";
            ddTransferFrom.SelectedValue = vehList.First(s => s.VehicleID == vehicle.VehicleID).VehicleID.ToString();
            ddTransferTo.DataSource = vehList;
            ddTransferTo.DataTextField = "VehicleName";
            ddTransferTo.DataValueField = "VehicleID";
            #region old code
            //List<DOContact> contactList = new List<DOContact>();
            //List<DOBase> LineItem = CurrentBRContact.SelectCompanyContacts(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"));//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
            ////Convert from DOBase to contact. Lame I know.
            //foreach (var item in LineItem)
            //{
            //    contactList.Add(item as DOContact);
            //}

            //ddCompanyContacts.DataSource = contactList;
            //ddCompanyContacts.DataTextField = "FirstName";//more here
            //ddCompanyContacts.DataValueField = "ContactID";                               
            //ddCompanyContacts.DataBind();
            //ddCompanyContacts.SelectedIndex = ddCompanyContacts.Items.IndexOf(ddCompanyContacts.Items.FindByText("Gregory"));
            #endregion old code

        }

        // Load list of drivers to static driver's name list to populate dd lists in edit or add mode.
        //        protected void LoadDriverList()
        //        {
        //            List<DOContact> contactList = new List<DOContact>();  // CurrentSessionContext.CurrentContact.ID
        //            List<DOBase> LineItem = CurrentBRContact.SelectCompanyContacts(CurrentSessionContext.CurrentContact.ID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"
        //            //Get list of drivers for populating the driver dropdown lists
        //            if (LineItem.Count > 0)
        //            {
        //                contactList = LineItem.Cast<DOContact>().ToList();
        //            }
        //            
        //            Session["EmployeeList"] = contactList; // save for session to be used in saving, updating, and deleting
        //
        //            for (int i = 0; i < contactList.Count; i++)
        //            {
        //                driverNamesList.Add(contactList[i].FirstName); //list for populating driver drop downs
        //            }              
        //        }


        //Tony is modifying 17.Feb.2017
        protected void LoadDriverList()
        {
            List<DOEmployeeInfo> employeeList = new List<DOEmployeeInfo>();  // CurrentSessionContext.CurrentContact.ID
                                                                             //            List<DOBase> LineItem = CurrentBRContact.SelectCompanyContacts(CurrentSessionContext.CurrentContact.ID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"

            //Tony modified 20.Feb.2017
            List<DOBase> LineItem = CurrentBRContact.SelectCompanyEmployee(CurrentSessionContext.CurrentContact.ID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"
            //Get list of drivers for populating the driver dropdown lists
            if (LineItem.Count > 0)
            {
                employeeList = LineItem.Cast<DOEmployeeInfo>().ToList();
            }

            Session["EmployeeList"] = employeeList; // save for session to be used in saving, updating, and deleting

            for (int i = 0; i < employeeList.Count; i++)
            {
                driverNamesList.Add(employeeList[i].FirstName); //list for populating driver drop downs
            }
        }

        //To populate gridview with data and set session data for GridView.DataSource
        protected void PopulateGridView()
        {
            //an empty vehicle to populate with "Rubbish" and bind to gridview, then not display row so that footer always shows even when no data to display
            DOForVehicleInput emptyVehicle = null;
            List<DOForVehicleInput> vehList = new List<DOForVehicleInput>();
            List<DOBase> contactVehicles = CurrentBRContact.SelectContactVehicles(CurrentSessionContext.CurrentContact.ContactID);
            //Cast from DOBase to DOForVehicleInput
            if (contactVehicles.Count > 0)
            {
                vehList = contactVehicles.Cast<DOForVehicleInput>().ToList();
                GridViewHasNoData = false;
            }
            else // if no data to bind to gridview, create pseudo vehicle and bind to gridview and not display it so footer always shows
            {
                emptyVehicle = new DOForVehicleInput
                {
                    DriverName = "None",
                    VehicleID = Guid.NewGuid(),
                    VehicleDriver = Guid.NewGuid(),
                    RegoDueDate = DateTime.Today,
                    VehicleRegistration = "none",
                    VehicleName = "none"
                };
                vehList.Add(emptyVehicle);
                GridViewHasNoData = true;
            }

            gvVehicleInput.DataSource = vehList;
            Session["GridData"] = vehList; //Persisting data across session for gridview
            gvVehicleInput.DataBind();

        }

        // Method for rebinding the data to the GridView Control after any changes.
        private void BindData()
        {
            gvVehicleInput.DataSource = Session["GridData"]; //Retrieve cached data for session list of data to bind to gridview
            gvVehicleInput.DataBind();
        }

        //Add a vehicle to db
        //        protected void btnAdd_Click(object sender, EventArgs e)
        //        {
        //            //Get local cache list already bound to gridview
        //            List<DOForVehicleInput> vList = Session["GridData"] as List<DOForVehicleInput>;
        //            DOVehicle newVeh = new DOVehicle(); // obj for new vehicle to be committed to db
        //            //Get all driver id's (employees) from Sessoin            
        //            List<DOContact> employees = Session["EmployeeList"] as List<DOContact>;
        //            
        //            //Get data from controls   ************ Validation on ASPX page  *****************
        //            string driverName = ((DropDownList)gvVehicleInput.FooterRow.FindControl("ddAddDriverName")).SelectedValue.ToString();
        //            string vehName = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddVehicleName")).Text.ToUpper();
        //            string vehRego = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddVehicleRego")).Text.ToUpper();
        //            string vehWOF = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddWOFDueDate")).Text;
        //            string vehRegoDate = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddRegoDueDate")).Text;
        //            string vehInsDate = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddInsDueDate")).Text; 
        //
        //            //Get Drivers Guid by name in dd list
        //            var driverGuid = from d in employees
        //                             where d.FirstName == driverName
        //                             select d.ContactID;
        //
        //            if (driverGuid != null)
        //            {
        //                newVeh.vehicleDriver = driverGuid.ElementAt(0);
        //            }
        //            //Check no rego already registered in company car list
        //            //db now has a constraint between VehicleOwner and VehicleRegistration in Vehicle Table
        //            foreach (var item in vList)
        //            {
        //                if (vehRego == item.VehicleRegistration.ToUpper())
        //                {
        //                    txtError.Text = "Vehicle Registration already exists. Cannot have duplicates.";
        //                    divError.Visible = true;
        //                    gvVehicleInput.EditIndex = -1; //Not editing anymore as it failed.
        //                    BindData();                    
        //                    return;
        //                }
        //                divError.Visible = false;
        //            }
        //            
        //            //Assign new data to newVeh obj
        //            newVeh.VehicleName = vehName;
        //            newVeh.VehicleRegistration = vehRego;
        //            newVeh.WOFDueDate = DateTime.Parse(vehWOF);
        //            newVeh.RegoDueDate = DateTime.Parse(vehRegoDate);
        //            newVeh.InsuranceDueDate = DateTime.Parse(vehInsDate);
        //            newVeh.VehicleID = Guid.NewGuid();
        //            newVeh.Active = true;
        //            newVeh.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;
        //
        //            //Commit new vehicle to db
        //            CurrentBRJob.SaveVehicle(newVeh);
        //            //ReBind data to GV and Re-Render page
        //            PopulateGridView();
        //            BindData();
        //            #region Old Code
        //            //if(tbVehicleMakeModel.Text!="")
        //            //{
        //            //    if(tbVehicleRegistration.Text!="")
        //            //    {
        //            //        DOVehicle Vehicle = new DOVehicle();
        //            //        String ContactID = ddCompanyContacts.SelectedItem.Value;
        //            //        String hjhk = ddCompanyContacts.SelectedItem.Text;
        //            //        Vehicle.Active = true;
        //            //        Vehicle.InsuranceDueDate = DateTime.Now;
        //            //        Vehicle.WOFDueDate = DateTime.Now;
        //            //        Vehicle.RegoDueDate = DateTime.Now;
        //            //        Vehicle.vehicleDriver = Guid.Parse(ContactID);
        //            //        Vehicle.VehicleID = Guid.NewGuid();
        //            //        Vehicle.VehicleName = tbVehicleMakeModel.Text;
        //            //        Vehicle.VehicleRegistration = tbVehicleRegistration.Text;
        //            //        Vehicle.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;
        //            //        CurrentBRJob.SaveVehicle(Vehicle);
        //
        //            //    }
        //            //}
        //            //LoadDropDownList();
        //            //PopulateGridView(); // Re-render the gridview to update newly added vehicle
        //            #endregion Old Code
        //        }

        //Tony modified on 17.Feb.2017 
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Tony added trigger 'tr_setDefaultCar' in database 21.Feb.2017
            // When inserted car, it checks if there is only one car, if it is , it will be set as default

            //Get local cache list already bound to gridview
            List<DOForVehicleInput> vList = Session["GridData"] as List<DOForVehicleInput>;
            DOVehicle newVeh = new DOVehicle(); // obj for new vehicle to be committed to db
            //Get all driver id's (employees) from Sessoin            
            List<DOEmployeeInfo> employees = Session["EmployeeList"] as List<DOEmployeeInfo>;

            //Get data from controls   ************ Validation on ASPX page  *****************
            string driverName = ((DropDownList)gvVehicleInput.FooterRow.FindControl("ddAddDriverName")).SelectedValue.ToString();
            string vehName = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddVehicleName")).Text.ToUpper();
            string vehRego = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddVehicleRego")).Text.ToUpper();
            string vehWOF = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddWOFDueDate")).Text;
            string vehRegoDate = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddRegoDueDate")).Text;
            string vehInsDate = ((TextBox)gvVehicleInput.FooterRow.FindControl("txtAddInsDueDate")).Text;

            //Get Drivers Guid by name in dd list
            var driverGuid = from d in employees
                             where d.FirstName == driverName
                             select d.EmployeeID;

            if (driverGuid != null)
            {
                newVeh.vehicleDriver = driverGuid.ElementAt(0);
            }
            //Check no rego already registered in company car list
            //db now has a constraint between VehicleOwner and VehicleRegistration in Vehicle Table
            foreach (var item in vList)
            {
                if (vehRego == item.VehicleRegistration.ToUpper())
                {
                    txtError.Text = "Vehicle Registration already exists. Cannot have duplicates.";
                    divError.Visible = true;
                    gvVehicleInput.EditIndex = -1; //Not editing anymore as it failed.
                    BindData();
                    return;
                }
                divError.Visible = false;
            }

            //Assign new data to newVeh obj
            newVeh.VehicleName = vehName;
            newVeh.VehicleRegistration = vehRego;
            newVeh.WOFDueDate = DateTime.Parse(vehWOF);
            newVeh.RegoDueDate = DateTime.Parse(vehRegoDate);
            newVeh.InsuranceDueDate = DateTime.Parse(vehInsDate);
            newVeh.VehicleID = Guid.NewGuid();
            newVeh.Active = true;
            newVeh.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;

            //Commit new vehicle to db
            CurrentBRJob.SaveVehicle(newVeh);
            //ReBind data to GV and Re-Render page
            PopulateGridView();
            BindData();
            #region Old Code
            //if(tbVehicleMakeModel.Text!="")
            //{
            //    if(tbVehicleRegistration.Text!="")
            //    {
            //        DOVehicle Vehicle = new DOVehicle();
            //        String ContactID = ddCompanyContacts.SelectedItem.Value;
            //        String hjhk = ddCompanyContacts.SelectedItem.Text;
            //        Vehicle.Active = true;
            //        Vehicle.InsuranceDueDate = DateTime.Now;
            //        Vehicle.WOFDueDate = DateTime.Now;
            //        Vehicle.RegoDueDate = DateTime.Now;
            //        Vehicle.vehicleDriver = Guid.Parse(ContactID);
            //        Vehicle.VehicleID = Guid.NewGuid();
            //        Vehicle.VehicleName = tbVehicleMakeModel.Text;
            //        Vehicle.VehicleRegistration = tbVehicleRegistration.Text;
            //        Vehicle.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;
            //        CurrentBRJob.SaveVehicle(Vehicle);

            //    }
            //}
            //LoadDropDownList();
            //PopulateGridView(); // Re-render the gridview to update newly added vehicle
            #endregion Old Code
        }
        //Back button page navigation
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

        //Delete a vehicle TODO: Need to display error to user that delete didn't work       
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Tony added database trigger 'tr_setDefaultCar'
            // When deleted, tr_setDefaultCar trigger will be fired

            LinkButton lb = sender as LinkButton;
            string[] arg = new string[2];
            arg = lb.CommandArgument.ToString().Split(',');

            DOVehicle deleteVeh = new DOVehicle(); // obj for new vehicle details to be deleted.
            Guid vID;
            bool vIDBool = Guid.TryParse(arg[0], out vID);//commandArgument contains the VehicleID to delete

            if (vIDBool)
            {
                deleteVeh.VehicleID = vID;
                deleteVeh.VehicleName = arg[1];

            }
            else
            {
                txtError.Text = "Delete failed because of invalid data.";
                BindData();
                divError.Visible = true;
                return;
            }

            //Check if Vehicle has Materials assigned to it. If it does, fail and display option to assign materials to another vehicle.
            bool canDeleteVehicle = CurrentBRJob.SeeIfCanDeleteVehicle(deleteVeh.VehicleID);
            ////update to db
            if (canDeleteVehicle)
            {
                CurrentBRJob.DeleteVehicle(deleteVeh);
                divError.Visible = false;
            }
            else
            {
                txtError.Text = "Before deleting a vehicle, materials must be removed or transferred to another vehicle.";
                LoadTransferDropDownLists(deleteVeh);
                divError.Visible = true;
                divTransfer.Visible = true;
            }

            ////ReBind data to GV and Re-Render page
            PopulateGridView();
            BindData();
        }

        //Event: On row editing, takes info from lables of row being edited and transfers to textboxes for editing
        protected void gvVehicleInput_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //put the the date currently in the lbl in gridview into these strings to then load it into texbox for editing.
            //just an anuance, but makes it better I think.
            WOFDueDateString = ((Label)gvVehicleInput.Rows[e.NewEditIndex].FindControl("lblWOFDueDate")).Text;
            RegoDueDateString = ((Label)gvVehicleInput.Rows[e.NewEditIndex].FindControl("lblRegoDueDate")).Text;
            InsDueDateString = ((Label)gvVehicleInput.Rows[e.NewEditIndex].FindControl("lblInsDueDate")).Text;
            VehNameString = ((Label)gvVehicleInput.Rows[e.NewEditIndex].FindControl("lblVehicleName")).Text;
            VehRegoString = ((Label)gvVehicleInput.Rows[e.NewEditIndex].FindControl("lblVehicleRego")).Text;
            //Set the index that is to be updated
            gvVehicleInput.EditIndex = e.NewEditIndex;
            BindData();
        }

        //Cancel the editing of a vehicle
        protected void gvVehicleInput_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVehicleInput.EditIndex = -1;
            BindData();
        }

        //Event for tansfer button. Transfers all materials from one vehicle to another.
        protected void btnTransferMaterials_Click(object sender, EventArgs e)
        {
            string vehFrom = ddTransferFrom.SelectedValue;
            string vehTo = ddTransferTo.SelectedValue;

            CurrentBRJob.TransferAllMaterialsBetweenVehicles(vehTo, vehFrom);

            PopulateGridView();
            BindData();

        }

        //btn to clear error div
        protected void btnClearError_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            BindData();

        }

        //Event driven button to cancel transfer and hide transfer options
        protected void btnCancelTransfer_Click(object sender, EventArgs e)
        {
            BindData();
            divTransfer.Visible = false;
        }

        //for updating a Vehicle's details.
        protected void gvVehicleInput_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Get local cache list already bound to gridview
            List<DOForVehicleInput> vList = Session["GridData"] as List<DOForVehicleInput>;
            DOVehicle updateVeh = new DOVehicle(); // obj for new vehicle to be committed to db
                                                   //Get all driver id's (employees) from Sessoin           

            //Tony modified 17.Feb.2017 
            //            List<DOContact> employees = Session["EmployeeList"] as List<DOContact>;

            List<DOEmployeeInfo> employees = Session["EmployeeList"] as List<DOEmployeeInfo>;

            //Get data from controls   ************ Validation done through aspx page  *****************
            string driverName = ((DropDownList)gvVehicleInput.Rows[e.RowIndex].FindControl("ddDriverName")).SelectedValue.ToString();
            string vehName = ((TextBox)gvVehicleInput.Rows[e.RowIndex].FindControl("txtVehicleName")).Text.ToUpper();
            string vehRego = ((TextBox)gvVehicleInput.Rows[e.RowIndex].FindControl("txtVehicleRego")).Text.ToUpper();
            string vehWOF = ((TextBox)gvVehicleInput.Rows[e.RowIndex].FindControl("txtWOFDueDate")).Text;
            string vehRegoDate = ((TextBox)gvVehicleInput.Rows[e.RowIndex].FindControl("txtRegoDueDate")).Text;
            string vehInsDate = ((TextBox)gvVehicleInput.Rows[e.RowIndex].FindControl("txtInsDueDate")).Text;

            //Get Drivers Guid by name in dd list
            // Tony has a question here. ContactID => EmployeeID OK?
            var driverGuid = from d in employees
                             where d.FirstName == driverName
                             select d.EmployeeID;

            if (driverGuid != null)
            {
                updateVeh.vehicleDriver = driverGuid.ElementAt(0);
            }

            //Get Guid associated with this vehicle            
            string vehGuid = gvVehicleInput.DataKeys[e.RowIndex]["VehicleID"].ToString();

            //Check no rego already registered in company car list
            foreach (var item in vList)
            {
                //Tony modified 31/01/2016
                string strVehicleID = item.VehicleID.ToString();


                if (item.VehicleID.ToString() != vehGuid)
                {
                    if (vehRego == item.VehicleRegistration.ToUpper())
                    {
                        txtError.Text = "Vehicle Registration already exists. Cannot have duplicates.";
                        gvVehicleInput.EditIndex = -1;  //Not editing anymore as it failed               
                        BindData();
                        divError.Visible = true;
                        return;
                    }
                }


                divError.Visible = false;
            }

            //Assign new data to newVeh obj
            updateVeh.VehicleName = vehName;
            updateVeh.VehicleRegistration = vehRego;
            updateVeh.WOFDueDate = DateTime.Parse(vehWOF);
            updateVeh.RegoDueDate = DateTime.Parse(vehRegoDate);
            updateVeh.InsuranceDueDate = DateTime.Parse(vehInsDate);
            updateVeh.VehicleID = Guid.Parse(vehGuid);
            updateVeh.Active = true;
            updateVeh.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;
            updateVeh.PersistenceStatus = ObjectPersistenceStatus.Existing; // so next line will update and not try to save a new one.

            //update to db
            CurrentBRJob.SaveVehicle(updateVeh);
            //ReBind data to GV and Re-Render page
            gvVehicleInput.EditIndex = -1; // Finished editing. w/o this line, throws error in RowDataBound event below
            PopulateGridView();
            BindData();

        }

        //Event fired for every row dataBound
        protected void gvVehicleInput_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                //Add drivers to dd list for editing purposes
                DropDownList ddlDrivers = e.Row.FindControl("ddDriverName") as DropDownList;
                foreach (string s in driverNamesList)
                {
                    ddlDrivers.Items.Add(s);
                }
                //Set value for existing driver of this record
                DOForVehicleInput drv = e.Row.DataItem as DOForVehicleInput;
                ddlDrivers.SelectedValue = drv.DriverName;
                ddlDrivers.Width = Unit.Pixel(125);

                //Tony testing
                DateTime d = DateTime.Parse(WOFDueDateString);
                TextBox TBox = e.Row.FindControl("txtWOFDueDate") as TextBox;
                TBox.Text = d.ToString("yyyy-MM-dd"); //Have to use int date time format or shows up weird stuff due to browser settings

                d = DateTime.Parse(RegoDueDateString);
                TBox = e.Row.FindControl("txtRegoDueDate") as TextBox;
                TBox.Text = d.ToString("yyyy-MM-dd");

                d = DateTime.Parse(InsDueDateString);
                TBox = e.Row.FindControl("txtInsDueDate") as TextBox;
                TBox.Text = d.ToString("yyyy-MM-dd");

                TBox = e.Row.FindControl("txtVehicleName") as TextBox;
                TBox.Text = VehNameString;

                TBox = e.Row.FindControl("txtVehicleRego") as TextBox;
                TBox.Text = VehRegoString;

            }

            //Display drivers dropdown list in footer for adding new vehicle
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddl = e.Row.FindControl("ddAddDriverName") as DropDownList;

                foreach (string item in driverNamesList)
                {
                    ddl.Items.Add(item);
                }

                ddl.Width = Unit.Pixel(125);
            }


        }

        //Cluncky way of hiding row if no data to bind to gridview, but still show footer
        //Hide row once binding finished.
        protected void gvVehicleInput_DataBound(object sender, EventArgs e)
        {
            if (GridViewHasNoData)
            {
                Label lab = gvVehicleInput.Rows[0].FindControl("lblVehicleName") as Label;

                if (lab != null && lab.Text == "none")
                {
                    gvVehicleInput.Rows[0].Visible = false; //hide first row with rubbish but display footer.
                }
            }
        }

        //Tony added 21.Feb.2017 
        //This method updates the default car id in employeeInfo table
        protected void btnSetDefault_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string[] arg = new string[2];
            arg = b.CommandArgument.ToString().Split(',');

            //Get vehicleID
            Guid vehicleID;
            bool vIDBool = Guid.TryParse(arg[0], out vehicleID);

            //Get vehicleDriverID
            Guid vDriverID;
            bool vDriverIDBool = Guid.TryParse(arg[1], out vDriverID);

            Guid originalVehicleID;

            //Before update, get existing default vehicle ID
            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(vDriverID);

            originalVehicleID = Employee.DefaultVehicleId;

            //UPDATE existing default car to new one
            CurrentBRContact.UpdateDefaultVehicle(vehicleID, vDriverID);

            //After updates defaultVehicleID in employeeInfo, displays popup
            ShowMessage(DEFAULT_VEHICLE_CHANGED + originalVehicleID.ToString() + " to " + vehicleID);

            PopulateGridView();
            
        }

        //Tony added 21.Feb.2017 
        //This method controls the visibility of 'Set Default Car' button
        protected bool ChkDefaultVehicle(Guid vehicleID)
        {
            DOEmployeeInfo Employee = CurrentBRContact.FindEmployeeByCar(vehicleID);

            //If selected vehicleID is default vehicle ID, hide 'Set Default button'
            if (Employee != null)
            {
                return false;
            }
            return true;
        }
    }
}