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
    

    public partial class VehicleInput : PageBase
    {


        protected void Page_Load(object sender, EventArgs e)//first page load and every postback
        {
            if (!IsPostBack)
            {
                LoadDropDownList();
            }
        }


        protected void LoadDropDownList()
        {

            List<DOBase> LineItem = CurrentBRContact.SelectCompanyContacts(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
           

            
            ddCompanyContacts.DataSource = LineItem;
            ddCompanyContacts.DataTextField = "FirstName";//more here
            ddCompanyContacts.DataValueField = "ContactID";
            ddCompanyContacts.DataBind();


            // CurrentBRContact.SelectCompanyContacts



        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
            if(TextBox1.Text!="")
            {
                if(TextBox2.Text!="")
                {
                    DOVehicle Vehicle = new DOVehicle();
                    String ContactID = ddCompanyContacts.SelectedItem.Value;
                    String hjhk = ddCompanyContacts.SelectedItem.Text;
                    Vehicle.Active = true;
                    Vehicle.InsuranceDueDate = DateTime.Now;
                    Vehicle.WOFDueDate = DateTime.Now;
                    Vehicle.RegoDueDate = DateTime.Now;
                    Vehicle.vehicleDriver = Guid.Parse(ContactID);
                    Vehicle.VehicleID = Guid.NewGuid();
                    Vehicle.VehicleName = TextBox1.Text;
                    Vehicle.VehicleRegistration = TextBox2.Text;
                    Vehicle.VehicleOwner = CurrentSessionContext.CurrentContact.ContactID;
                    CurrentBRJob.SaveVehicle(Vehicle);


                }
            }

            LoadDropDownList();




        }







   }
}