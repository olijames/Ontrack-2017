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
    public partial class MaterialList : PageBase
    {
        protected DOEmployeeInfo Employee;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
        }

                protected void Page_PreRender(object sender, EventArgs e)
        {
           
            List<DOBase> Materials = CurrentBRJob.SelectMaterialsByContact(CurrentSessionContext.CurrentContact.ContactID);
            gvMaterials.DataSource = Materials;
            gvMaterials.DataBind();
          

            litContactName.Text = CurrentSessionContext.CurrentContact.DisplayName;
        }

        protected void gvMaterials_PreRender(object sender, EventArgs e)
        {
           LoadDropDownList();
            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
                                                   // Reinstate the mask and re-test

            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.AddMaterialsManuallyToVehicle) == CompanyPageFlag.AddMaterialsManuallyToVehicle)
            {
                btnAddToContainer.Visible = true;
            }

        }

        protected void btnAddToContainer_Click(object sender, EventArgs e)
        {
            DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue));
            List<DOBase> SIMs = CurrentBRJob.SelectSIMsByContactID_Qty_materialid(CurrentSessionContext.CurrentContact.ContactID, 0, V.VehicleID);
            //List<DOBase> MaterialList = CurrentBRJob.SelectVehicleMaterials(CurrentSessionContext.CurrentContact.ContactID, V.vehicleDriver, V.VehicleID);
           
            foreach (GridViewRow gvR in gvMaterials.Rows)
            {
                bool MaterialMatches = false;
                TextBox T = gvR.FindControl("txtAdd") as TextBox;
                if(T.Text != "") //if the user has entered something in the textbox to add to their vehicle
                {
                    string strMaterialID = gvMaterials.DataKeys[gvR.RowIndex].Values["MaterialID"].ToString();
                    Guid G = Guid.Parse(strMaterialID);

                    foreach (DOSupplierInvoiceMaterial LI in SIMs)
                    {
                        if(LI.MaterialID==G)
                        {
                            MaterialMatches = true;
                        }
                        
                    }
                    if(MaterialMatches) //update qtyRTA to existing record's QtyRTA. Possibly not required.
                    {

                    }
                    else                //make new SIM.
                    {
                        DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
                        SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();                                     //As is
                        SIMaterial.MaterialID = Guid.Parse(strMaterialID);                                         //stroriginalmaterialid                    
                        SIMaterial.TaskMaterialID = Guid.Parse("00000000-0000-0000-0000-000000000000");

                        SIMaterial.VehicleID = V.VehicleID;
                        SIMaterial.SupplierInvoiceID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        SIMaterial.Qty = 0;    
                        SIMaterial.QtyRemainingToAssign = Decimal.Parse(T.Text);
                        SIMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;    
                        SIMaterial.Assigned = true;                                                  
                        SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse("00000000-0000-0000-0000-000000000000");                                              //stroriginalsupplierinvoicematerialid

                        //DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
                        //SIMaterial.VehicleID = V.VehicleID;

                        CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);
                    }















                    //Below is rubbish
                        Label L = gvR.FindControl("lblInVehicle") as Label;

                    System.Diagnostics.Debug.WriteLine(T.Text);

                   // string strMaterialID = gvMaterials.DataKeys[gvR.RowIndex].Values["MaterialID"].ToString();
                    System.Diagnostics.Debug.WriteLine(strMaterialID);
                    System.Diagnostics.Debug.WriteLine(ddVehicleSelect.SelectedValue.ToString());
                    //here use loadinvehicle function
                    //System.Diagnostics.Debug.WriteLine(i.ToString());

                 

                }
            }
        }
        

        protected void LoadDropDownList() //jared
        {
            int i = 0;
            if(IsPostBack)
            {
                i=ddVehicleSelect.SelectedIndex;
            }
            List<DOBase> theList = CurrentBRContact.SelectCompanyContactsWithAVehicle(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
           // DropDownList ddVehicleSelect = gvMaterials.HeaderRow.FindControl("ddVehicleSelect") as DropDownList;

            ddVehicleSelect.DataSource = theList;
            ddVehicleSelect.DataTextField = "FirstName"; //todo more here
            ddVehicleSelect.DataValueField = "ContactID";
            ddVehicleSelect.DataBind();
            // CurrentBRContact.SelectCompanyContacts
            if (!IsPostBack)
            {
                //    ddVehicleSelect.SelectedValue = ddVehicleSelect.SelectedItem.Value;
                if (ddVehicleSelect.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString()) != null);
                {
                    ddVehicleSelect.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                }
            }
            else
            {
                ddVehicleSelect.SelectedIndex = i;
            }

            if (gvMaterials.Rows.Count != 0)
            {
                Label L1 = gvMaterials.HeaderRow.FindControl("lblVehicleDriverName") as Label;
                L1.Text = ddVehicleSelect.SelectedItem.Text + "'s vehicle stock";

                Label L2 = gvMaterials.HeaderRow.FindControl("lblCompanyName") as Label;
                L2.Text = CurrentSessionContext.CurrentContact.DisplayName + "'s stock";
            }
            LoadVehicleStockToGV();


        }

        protected void LoadVehicleStockToGV() //jared. Loads the individual's vehicle's stock to the Gridview
        {
            System.Diagnostics.Debug.WriteLine(ddVehicleSelect.SelectedValue.ToString()); //contactid
            if (ddVehicleSelect.Items.Count > 0)
            {
                DOBase DOBV = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse(ddVehicleSelect.SelectedValue));
                DOVehicle V = DOBV as DOVehicle;
                if (V != null)
                {
                    List<DOBase> MaterialList = CurrentBRJob.SelectVehicleSIMs(CurrentSessionContext.CurrentContact.ContactID,
                                  V.VehicleID);
                    foreach (GridViewRow gvR in gvMaterials.Rows)
                    {
                        foreach (DOVehicleMaterialQty LI in MaterialList)
                        {
                            string strMaterialID = gvMaterials.DataKeys[gvR.RowIndex].Values["MaterialID"].ToString();
                            Guid G = Guid.Parse(strMaterialID);
                            Guid G1 = Guid.Parse(LI.MaterialID.ToString());
                            Decimal D = 0;
                            if (G == G1)
                            {
                                Label L = gvR.FindControl("lblInVehicle") as Label;
                                D = LI.QtyRemainingToAssign;
                                if (L.Text != "")
                                {
                                    D = D + Decimal.Parse(L.Text);

                                }
                                L.Text = D.ToString();
                            }
                        }
                    }

                }
            }
        }



        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/MaterialDetails.aspx",false);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Response.Redirect("~/Private/MaterialDetails.aspx?id=" + b.CommandArgument.ToString());
        }


        Dictionary<Guid, DOMaterialCategory> Categories = new Dictionary<Guid, DOMaterialCategory>();
        protected string GetCategoryName(Guid MaterialCategoryID)
        {
            if (!Categories.ContainsKey(MaterialCategoryID))
                Categories.Add(MaterialCategoryID, CurrentBRJob.SelectMaterialCategory(MaterialCategoryID));
            return Categories[MaterialCategoryID].CategoryName;
        }

        //Dictionary<Guid, DOContact> Contacts = new Dictionary<Guid, DOContact>();
        //protected string GetMaterialCategoryContactName(Guid MaterialCategoryID)
        //{
        //    if (!Categories.ContainsKey(MaterialCategoryID))
        //        Categories.Add(MaterialCategoryID, CurrentBRJob.SelectMaterialCategory(MaterialCategoryID));

        //    DOMaterialCategory Category = Categories[MaterialCategoryID];
        //    if (!Contacts.ContainsKey(Category.ContactID))
        //        Contacts.Add(Category.ContactID, CurrentBRContact.SelectContact(Category.ContactID));
        //    return Contacts[Category.ContactID].DisplayName;
        //}

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_MaterialCategory);
        }

       
    }
}