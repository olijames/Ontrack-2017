using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.BusinessRules;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class SiteDetails : PageBase
    {
        DOContact Customer;
        DOContact Contact;
        bool SelfSite = false;
        bool HaveSiteOwnerDetails = true;
        DOSite EditSite = null;
        bool customerOwn = false;
        DOContact ExistingContact = null;
        //DOContact selectedSiteOwner; jared 20.2.17
        static bool newCustomer = false;
        static bool suburbReset = true;
        static bool existingCustomer = false;
        DOContact ContactSiteOwner; //Jared
        DOContractorCustomer ContractorCustomerSiteOwner; //Jared 2017.4.6

        //Tony added 5.11.2016
        private Guid siteID;
        private Guid fromContactID;
        private Guid toContactID;
        //Tony added 5.11.2016

        // DOCustomer newCustomer;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
            {
                Response.Redirect(Constants.URL_Home);
            }
            Contact = CurrentSessionContext.CurrentContact;
            Customer = CurrentSessionContext.CurrentCustomer;
            if (CurrentSessionContext.CurrentContractee == null)
                CurrentSessionContext.CurrentContractee = null;
            ClearJob();

            if (Request.QueryString["for"] == "self")
            {
                SelfSite = true;
            }

            if (CurrentSessionContext.CurrentSite != null)
            {
                EditSite = CurrentBRSite.SelectSite(CurrentSessionContext.CurrentSite.SiteID);
            }
        }

        //	2017.2.15 commented relocated. jared
        //Tony added 5.11.2016
        //private void shareSite(Guid siteID, Guid fromContactID, Guid toContactID)
        //{
        //    //if any value is selected from company list, do copy and paste current site info
        //    if (ContactDD.SelectedIndex >= 0)
        //    {
        //        CurrentBRSite.ShareContactSite(siteID, fromContactID, toContactID);
        //    }
        //}
        //Tony added 5.11.2016

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //	2017.2.15 commented. jared
                //Tony Added 5.11.2016
                //LoadContact();
                //Tony Added 5.11.2016

                if (CurrentSessionContext.CurrentContractee != null)
                {
                    if (CurrentSessionContext.CurrentContractee != null && 
                        CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentContractee.ContactID)
                        CurrentSessionContext.CurrentCustomer = CurrentSessionContext.CurrentContractee;
                }
                Customer = CurrentSessionContext.CurrentCustomer;
                Loadregions();
                LoadDistrict();
                // LoadSuburbs();
                //For Edit Site, existing site's address is used.
                if (EditSite != null)
                {
                    if (EditSite.Address3 != null && EditSite.Address3 != "")
                    {
                        string district = EditSite.Address3;
                        foreach (ListItem item in District_DDL.Items)
                        {
                            item.Selected = false;
                            System.Diagnostics.Debug.WriteLine(item.Text);
                        }
                        if (District_DDL.Items.Contains(new ListItem(district)))
                        {
                            District_DDL.Items.FindByText(district).Selected = true;
                            District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                            District_DDL.DataBind();
                            District_DDL.PreRender += (object sender1, EventArgs e1) =>
                            {
                                //LoadDistrict();
                                foreach (ListItem item in District_DDL.Items)
                                    item.Selected = false;
                                // District_DDL.Items.FindByText(Contact.Address3).Selected = true;
                                District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                                if (District_DDL.SelectedIndex == -1)
                                    District_DDL.SelectedIndex = 2;
                                District_DDL.DataBind();
                            };
                            LoadSuburbs();
                        }
                        else
                            District_DDL.DataSource = null;
                    }

                    if (EditSite.Address2 != null && EditSite.Address2 != "")
                    {
                        string suburb = EditSite.Address2;
                        foreach (ListItem item in SuburbDD.Items)
                        {
                            item.Selected = false;
                            System.Diagnostics.Debug.WriteLine(item.Text);
                            if (item.Text == suburb)
                            {
                                System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            }

                        }
                        //if (SuburbDD.Items.Contains(new ListItem(suburb)))
                        //{


                        if (SuburbDD.Items.FindByText(suburb) != null)//not working !here!
                        {  //    SuburbDD.Items.FindByText(suburb).Selected = true;
                            SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                            SuburbDD.DataBind();
                            SuburbDD.PreRender += (object sender1, EventArgs e1) =>
                            {
                                //LoadSuburbs();
                                foreach (ListItem item in SuburbDD.Items)
                                    item.Selected = false;
                                //SuburbDD.Items.FindByText(Contact.Address2).Selected = true;
                                SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                                SuburbDD.DataBind();
                                SuburbDD.Enabled = false;
                            };
                        }

                        else
                        {
                            SuburbDD.Items.Clear();
                            SuburbDD.DataSource = null;
                            //  SuburbDD.Items.Add(EditSite.Address2);
                            DataBind();
                        }

                    }
                    else

                    {
                        SuburbDD.Items.Clear();
                        SuburbDD.DataSource = null;
                        DataBind();
                    }
                    SiteAddBtns.Visible = false;
                    EditSiteBtns.Visible = true;
                }
                //For New Site, customer's address is used
                else
                {
                    if (CurrentSessionContext.CurrentCustomer != null)
                    {
                        if (CurrentSessionContext.CurrentCustomer.Address3 != null && CurrentSessionContext.CurrentCustomer.Address3 != "")
                        {
                            string district = CurrentSessionContext.CurrentCustomer.Address3;
                            foreach (ListItem item in District_DDL.Items)
                                item.Selected = false;
                            if (District_DDL.Items.Contains(new ListItem(district)))
                            {
                                District_DDL.Items.FindByText(district).Selected = true;
                                District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                                District_DDL.DataBind();
                                District_DDL.PreRender += (object sender1, EventArgs e1) =>
                                {
                                    //LoadDistrict();
                                    foreach (ListItem item in District_DDL.Items)
                                        item.Selected = false;
                                    District_DDL.Items.FindByText(Contact.Address3).Selected = true;
                                    District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                                    if (District_DDL.SelectedIndex == -1)
                                        District_DDL.SelectedIndex = 2;
                                    District_DDL.DataBind();
                                };
                                LoadSuburbs();
                            }
                            else
                                District_DDL.DataSource = null;
                        }

                        else
                        {
                            District_DDL.PreRender += (object sender1, EventArgs e1) =>
                            {
                                //LoadDistrict();
                                foreach (ListItem item in District_DDL.Items)
                                    item.Selected = false;
                                if (District_DDL.SelectedIndex == -1 || District_DDL.SelectedIndex == 0)
                                    District_DDL.SelectedIndex = 2;
                                District_DDL.DataBind();
                            };
                        }
                        if (CurrentSessionContext.CurrentCustomer.Address2 != null && CurrentSessionContext.CurrentCustomer.Address2 != "")
                        {
                            string suburb = CurrentSessionContext.CurrentCustomer.Address2;
                            foreach (ListItem item in SuburbDD.Items)
                                item.Selected = false;
                            if (SuburbDD.Items.Contains(new ListItem(suburb)))
                            {
                                SuburbDD.Items.FindByText(suburb).Selected = true;
                                SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                                SuburbDD.DataBind();
                                //SuburbDD.PreRender += (object sender1, EventArgs e1) =>
                                //{
                                ////LoadSuburbs();
                                //foreach (ListItem item in SuburbDD.Items)
                                //        item.Selected = false;
                                //    SuburbDD.Items.FindByText(Contact.Address2).Selected = true;
                                //    SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                                //    SuburbDD.DataBind();
                                //};
                            }
                        }

                    }
                }
            }
            //if (CurrentSessionContext.CurrentCustomer != null)
            //{
            //    string custname;
            //    if (CurrentSessionContext.CurrentCustomer.FirstName == "" && CurrentSessionContext.CurrentCustomer.FirstName == "")
            //        custname = CurrentSessionContext.CurrentCustomer.DisplayName;
            //    else
            //        custname = CurrentSessionContext.CurrentCustomer.FirstName + " " + CurrentSessionContext.CurrentCustomer.LastName;
            //    Btn_CustomerOwn.Text = "'" + custname + "' LEGALLY owns this site?";
            //}
            if (SelfSite)
            {
                if (!IsPostBack)
                {
                    //jared 20.2.2017
                    //if (CurrentSessionContext.CurrentContractee.ContactID == CurrentSessionContext.CurrentCustomer.ContactID)
                    //    phCustomer.Visible = false;
                    //jared end of block
                }
                phOwner.Visible = false;
                if (!IsPostBack)
                    SetTextFields(CurrentSessionContext.CurrentContact);
            }
            else
            {
                if (!IsPostBack)
                {
                    if (EditSite != null)
                    {
                        SetTextFields(EditSite);
                    }
                    else if (Customer != null)
                    {
                        SetTextFields(Customer);
                    }
                }
                BindCustomerDropDown();
            }
        }

        private void SetTextFields(DOSite Site)
        {
            SetIfEmpty(txtCustomerAddress1, Site.Address1);
            SetIfEmpty(txtCustomerAddress2, Site.Address2);
            SetIfEmpty(txtCustomerEmail, Site.OwnerEmail);
            SetIfEmpty(txtCustomerFirstName, Site.OwnerFirstName);
            SetIfEmpty(txtCustomerLastName, Site.OwnerLastName);
            SetIfEmpty(txtCustomerPhone, Site.OwnerPhone);
            SetIfEmpty(txtSiteAddress1, Site.Address1);
            //SetIfEmpty(txtSiteAddress2, Site.Address2);
        }

        private void SetTextFields(DOContact Contact)
        {
            SetIfEmpty(txtCustomerAddress1, Contact.Address1);
            SetIfEmpty(txtCustomerAddress2, Contact.Address2);
            SetIfEmpty(txtCustomerEmail, Contact.Email);
            SetIfEmpty(txtCustomerFirstName, Contact.FirstName);
            SetIfEmpty(txtCustomerLastName, Contact.LastName);
            SetIfEmpty(txtCustomerPhone, Contact.Phone);
            SetIfEmpty(txtSiteAddress1, Contact.Address1);
            //SetIfEmpty(txtSiteAddress2, Contact.Address2);
        }

        private void SetTextFields(DOCustomer DisplayCustomer)
        {
            //SetIfEmpty(txtCustomerAddress1, DisplayCustomer.Address1);
            //SetIfEmpty(txtCustomerAddress2, DisplayCustomer.Address2);
            //SetIfEmpty(txtCustomerEmail, DisplayCustomer.Email);
            // SetIfEmpty(txtCustomerFirstName, DisplayCustomer.FirstName);
            // SetIfEmpty(txtCustomerLastName, DisplayCustomer.LastName);
            // SetIfEmpty(txtCustomerPhone, DisplayCustomer.Phone);
            SetIfEmpty(txtSiteAddress1, DisplayCustomer.Address1);
            //SetIfEmpty(txtSiteAddress2, DisplayCustomer.Address2);
        }


        private void SetIfEmpty(TextBox txt, string value)
        {
            //if (string.IsNullOrEmpty(txt.Text))
            txt.Text = value;
        }

        private void BindCustomerDropDown()
        {
            bool DontHaveOwnContractorCustomerEntry = true; //2017.4.20 jared
            List<DOBase> Customers = CurrentBRContact.SelectAllCustomers(Contact.ContactID);
            ddlCustomer.Items.Clear();
            //ddlCustomer.Items.Add(new ListItem("Select...", string.Empty));
           
            
                foreach (DOContactInfo c in Customers)
                {
                    ddlCustomer.Items.Add(new ListItem(c.DisplayName, c.ContactID.ToString()));
                    if (c.ContactID == Contact.ContactID) DontHaveOwnContractorCustomerEntry = false; //2017.4.20 jared
                }
            
            //if (!string.IsNullOrEmpty(Request.Form[ddlCustomer.UniqueID]))
            //    ddlCustomer.SelectedValue = Request.Form[ddlCustomer.UniqueID];

            if (DontHaveOwnContractorCustomerEntry) //2017.4.20 jared
            {
                DOContractorCustomer docc = CurrentBRContact.CreateContractorCustomer(Contact.ContactID, Contact.ContactID, Contact.ContactID, Contact.Address1, 
                              Contact.Address2, Contact.Address3, Contact.Address4, Contact.CompanyName, DOContractorCustomer.LinkedEnum.Linked, Contact.Phone, Guid.Empty,Contact.FirstName,Contact.LastName, (int)Contact.ContactType);
                CurrentBRContact.SaveContractorCustomer(docc);
                ddlCustomer.Items.Add(new ListItem(Contact.DisplayName, Contact.ContactID.ToString()));

            }
            if (CurrentSessionContext.CurrentContractee == this.Contact)//2017.4.24 Jared. for adding self site
            {
                ddlCustomer.Enabled = false;
                btnNoDetails.Visible = false;
                Btn_CustomerOwn.Text = "Add my property";
            }
            ddlCustomer.DataBind();
            ddlCustomer.SelectedValue = CurrentSessionContext.CurrentContractee.ContactID.ToString();

            //jared 20.2.17 
            ////if (Customer != null)
            ///* Contractee */
            ////if (CurrentSessionContext.CurrentCustomer != null
            //if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentContractee.ContactID)
            //{

            //    //ddlCustomer.Items.Add(new ListItem(Customer.DisplayName, Customer.ContactID.ToString()));
            //    string custname;
            //    DOContact contact = CurrentSessionContext.CurrentContractee;
            //    if (contact.FirstName == "" && contact.LastName == "")
            //        custname = contact.DisplayName;
            //    else
            //        custname = contact.FirstName + " " + contact.LastName;
            //    ddlCustomer.Items.Add(new ListItem(custname, contact.ContactID.ToString()));
            //    ddlCustomer.Enabled = false;

            //    ddlCustomer.DataBind();
            //}
            //else
            //{
            //    List<DOBase> Customers = CurrentBRContact.SelectAllCustomers(Contact.ContactID);
            //    ddlCustomer.Items.Clear();
            //    ddlCustomer.Items.Add(new ListItem("Select...", string.Empty));
            //    foreach (DOContactInfo c in Customers)
            //    {
            //        ddlCustomer.Items.Add(new ListItem(c.DisplayName, c.ContactID.ToString()));
            //    }
            //    if (!string.IsNullOrEmpty(Request.Form[ddlCustomer.UniqueID]))
            //        ddlCustomer.SelectedValue = Request.Form[ddlCustomer.UniqueID];
            //    ddlCustomer.Enabled = true;
            //}

            //if (EditSite != null)
            //{
            //    //Can only select customer when creating site.
            //    ddlCustomer.Enabled = false;
            //}
            //jared end of block
        }


        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Form[ddlCustomer.UniqueID]))
            {
                Guid CustomerID = new Guid(Request.Form[ddlCustomer.UniqueID]);
                DOContact DisplayCustomer = CurrentBRContact.SelectCustomer(CustomerID);
                DOContact SelectedContact = CurrentBRContact.SelectContact(DisplayCustomer.ContactID);
                setContactDetails(SelectedContact);
                CurrentSessionContext.CurrentContractee = DisplayCustomer;
                //SetTextFields(DisplayCustomer);
            }
        }

        private void setContactDetails(DOContact selectedContact)
        {
            SetIfEmpty(txtSiteAddress1, selectedContact.Address1);
            // SetIfEmpty(RegionDD.SelectedIndex,)
            if (!string.IsNullOrEmpty(selectedContact.Address3))
            {
                string district = selectedContact.Address3;
                foreach (ListItem item in District_DDL.Items)
                    item.Selected = false;
                District_DDL.Items.FindByText(district).Selected = true;
                District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                District_DDL.DataBind();
                District_DDL.PreRender += (object sender1, EventArgs e1) =>
                {
                    //LoadDistrict();
                    foreach (ListItem item in District_DDL.Items)
                        item.Selected = false;
                    District_DDL.Items.FindByText(selectedContact.Address3).Selected = true;
                    District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                    if (District_DDL.SelectedIndex == -1)
                        District_DDL.SelectedIndex = 2;
                    District_DDL.DataBind();
                };
                LoadSuburbs();

                string suburb = selectedContact.Address2;
                foreach (ListItem item in SuburbDD.Items)
                    item.Selected = false;
                SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                SuburbDD.DataBind();
                SuburbDD.PreRender += (object sender1, EventArgs e1) =>
                {
                    //LoadSuburbs();
                    foreach (ListItem item in SuburbDD.Items)
                        item.Selected = false;
                    SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                    SuburbDD.DataBind();
                };

            }
            lbl1.Text = "'" + selectedContact.DisplayName + "' is the legal owner of this new site?";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //CheckSiteData(); 
            DOSite Site = Save();

            if (Site != null)
                Redirect();
            //}
            //catch(FieldValidationException ex)
            //{
            //    ShowMessage(ex.Message);
            //}
        }

        private void CheckSiteData()
        {
            if (SuburbDD.SelectedItem == null)
            {
                Error.Text = "Please select district and suburb";
                Error.Visible = true;
            }
            if (SuburbDD.SelectedItem.ToString() == "-Select-")
            {
                Error.Text = "Please select suburb";
                Error.Visible = true;
            }
            if (string.IsNullOrEmpty(District_DDL.SelectedItem.ToString()))
            {
                Error.Text = "Please select district and suburb";
                Error.Visible = true;
            }
            if (string.IsNullOrEmpty(RegionDD.SelectedItem.ToString()))
            {
                Error.Text = "Please select district and suburb";
                Error.Visible = true;
            }
        }

        protected void btnSaveAddJob_Click(object sender, EventArgs e)
        {
            try
            {
                DOSite Site = Save();
                if (Site != null)
                {
                    CurrentSessionContext.CurrentSite = Site;
                    Response.Redirect(Constants.URL_JobSummary);
                }
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        //private DOSite Save()
        //{
        //    DOSite Site;
        //    bool NewSite;
        //    if (EditSite == null)
        //    {
        //        NewSite = true;
        //        Site = CurrentBRSite.CreateSite(CurrentSessionContext.Owner.ContactID);//createdby only
        //    }
        //    else
        //    {
        //        NewSite = false;
        //        Site = EditSite;
        //    }

        //    try
        //    {
        //        //Guid JobOwner, CustomerID;
        //        Guid CustomerContactID = Guid.Empty;
        //        Guid CustomerID = Guid.Empty;
        //        if (NewSite)
        //        {
        //            if (SelfSite)
        //            {
        //                //Site.OwnerAddress1 = Contact.Address1;
        //                //Site.OwnerAddress2 = Contact.Address2;
        //                //Site.OwnerEmail = Contact.Email;
        //                //Site.OwnerFirstName = Contact.FirstName;
        //                //Site.OwnerLastName = Contact.LastName;
        //                //Site.OwnerPhone = Contact.Phone;
        //                Site.VisibilityStatus = Framework.DataObjects.SiteVisibility.None;
        //                //CustomerContactID = CurrentSessionContext.CurrentContact.ContactID;
        //                CustomerContactID = CurrentSessionContext.CurrentCustomer.ContactID;
        //                /*Current customer is now current contractee*/
        //                // CustomerContactID = CurrentSessionContext.CurrentContractee.ContactID;
        //            }
        //            else if (selectedSiteOwner != null)
        //            {
        //                CustomerID = selectedSiteOwner.ContactID;
        //                CustomerContactID = selectedSiteOwner.ContactID;
        //            }
        //            else if (NoDetails)//jared !here!
        //            {

        //                DOContact SiteOwner = CreateNoDetailsContactForCustomer();

        //                CustomerID = Contact.ContactID;
        //                CustomerContactID = Contact.ContactID;

        //                //add pending contact here

        //                //siteownerpending
        //                //notification flag
        //            }
        //            else
        //            {
        //                if (Customer != null)
        //                {
        //                    CustomerID = Customer.ContactID;
        //                }
        //                else
        //                {
        //                    //if (string.IsNullOrEmpty(Request.Form[ddlCustomer.UniqueID]))
        //                    //    throw new FieldValidationException("Customer is required.");
        //                    // CustomerID = new Guid(Request.Form[ddlCustomer.UniqueID]);
        //                    CustomerID = Guid.Parse(ddlCustomer.SelectedValue.ToString());
        //                }

        //                //DOContact SelectedCustomer = CurrentBRContact.SelectCustomer(CustomerID);
        //                //CustomerContactID = SelectedCustomer.ContactID;
        //                // CustomerContactID = CurrentSessionContext.CurrentCustomer.ContactID;
        //            }
        //            //if (customerOwn)
        //            //{
        //            if (SelfSite)
        //            {
        //                //if (selectedSiteOwner == null)
        //                Site.SiteOwnerID = CustomerContactID;
        //            }
        //            else if (selectedSiteOwner != null)
        //            {
        //                Site.SiteOwnerID = selectedSiteOwner.ContactID;
        //            }
        //            else if (NoDetails)
        //            {
        //                Site.SiteOwnerID = SiteOwner.ContactID;


        //            }
        //            else
        //            {
        //                //If no contact found, create a new contact.
        //                if (!existingCustomer)
        //                    Contact = CreateContactForCustomer(Txt_Email.Text);
        //                //Create the customer.
        //                Customer = CurrentBRContact.CreateCustomer(CurrentSessionContext.Owner.ContactID);
        //                Customer.ContactID = Contact.ContactID;
        //                //Save the customers details.
        //                //Customer.CompanyName = txtCustomerCompanyName.Text;
        //                //Customer.FirstName = txtCustomerFirstName.Text;
        //                //Customer.LastName = txtCustomerLastName.Text;
        //                //Customer.Address1 = txtCustomerAddress1.Text;
        //                //Customer.Address2 = txtCustomerAddress2.Text;
        //                //Customer.Phone = txtCustomerPhone.Text;
        //                //Customer.Email = txtCustomerEmail.Text;
        //                if (Contact.CompanyName != null)
        //                    Customer.CompanyName = Contact.CompanyName;
        //                else
        //                    Customer.CompanyName = "";
        //                Customer.FirstName = Contact.FirstName;
        //                Customer.LastName = Contact.LastName;
        //                Customer.Address1 = Contact.Address1;
        //                Customer.Address2 = Contact.Address2;
        //                Customer.Address3 = Contact.Address3;
        //                Customer.Address4 = Contact.Address4;
        //                Customer.Phone = Contact.Phone;
        //                Customer.Email = Contact.Email;
        //                //CurrentBRContact.SaveCustomer(Customer);
        //                //* Assign the current customer as contractor and make current customer as new customer which is created *//
        //                if (CurrentSessionContext.CurrentCustomer != null)
        //                    CurrentSessionContext.CurrentContractee = CurrentSessionContext.CurrentCustomer;
        //                CurrentSessionContext.CurrentCustomer = Customer;
        //                DOContractorCustomer contractorCustomer = new DOContractorCustomer();
        //                contractorCustomer = CurrentBRContact.SelectContactCustomer(CurrentSessionContext.CurrentContact.ContactID, Customer.ContactID);
        //                //contractorCustomer = CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, Customer.CustomerID);
        //                if (contractorCustomer == null)
        //                {
        //                    contractorCustomer = CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContractee.ContactID, Customer.ContactID);
        //                    CurrentBRContact.SaveContactCustomer(contractorCustomer);
        //                }
        //                Site.SiteOwnerID = Contact.ContactID;
        //            }
        //            //else
        //            //    Site.SiteOwnerID = selectedSiteOwner.ContactID;
        //            //}
        //            Site.ContactID = CustomerContactID;
        //        }
        //        //Site.JobOwner = JobOwner;
        //        // if (!string.IsNullOrEmpty(txtSiteAddress1.Text))
        //        //|| string.IsNullOrWhiteSpace(SuburbDD.SelectedItem.ToString()) || string.IsNullOrWhiteSpace(District_DDL.SelectedItem.ToString()) || 
        //        //{
        //        Site.Address1 = txtSiteAddress1.Text;
        //        //}
        //        //if(EditSite==null)
        //        //{
        //        //   if(EditSite!=null)
        //        //{
        //        //    if (SuburbDD.DataSource == null)
        //        //        LoadSuburbs();
        //        //    Error.Text = "Please select suburb";
        //        //    Error.Visible = true;
        //        //    SuburbDD.SelectedIndex =-1;
        //        //}
        //        if (SuburbDD.SelectedItem != null)
        //        {
        //            if (!string.IsNullOrEmpty(SuburbDD.SelectedItem.ToString()))
        //            //Site.Address2 = txtSiteAddress2.Text;
        //            {
        //                if (SuburbDD.SelectedItem.ToString() == "-Select-")
        //                {
        //                    Error.Text = "Please select suburb";
        //                    Error.Visible = true;
        //                    throw new FieldValidationException("Please select suburb for site");
        //                }
        //                else
        //                    Site.Address2 = SuburbDD.SelectedItem.ToString();
        //            }
        //            else
        //                ShowMessage("Please select suburb");
        //        }
        //        else
        //        {
        //            //ShowMessage("Please select suburb");
        //            Error.Text = "Please select suburb";
        //            Error.Visible = true;
        //            LoadSuburbs();
        //            if (NewSite)
        //                RollBackCustomer(CurrentSessionContext.CurrentCustomer, Customer);
        //            // throw new FieldValidationException("Please select suburb");
        //        }
        //        if (!string.IsNullOrEmpty(District_DDL.SelectedItem.ToString()))
        //            //Site.Address3 = txtSiteAdd3.Text;
        //            Site.Address3 = District_DDL.SelectedItem.ToString();
        //        else
        //        {
        //            ShowMessage("Please select District");
        //        }
        //        if (!string.IsNullOrEmpty(RegionDD.SelectedItem.ToString()))
        //            //Site.Address4 = txtSiteAdd4.Text;
        //            Site.Address4 = RegionDD.SelectedItem.ToString();
        //        else
        //            ShowMessage("Please select Region");
        //        // }
        //        //if (!SelfSite)
        //        //{
        //        //    Site.OwnerAddress1 = txtCustomerAddress1.Text;
        //        //    Site.OwnerAddress2 = txtCustomerAddress2.Text;
        //        //    Site.OwnerEmail = txtCustomerEmail.Text;
        //        //    Site.OwnerFirstName = txtCustomerFirstName.Text;
        //        //    Site.OwnerLastName = txtCustomerLastName.Text;
        //        //    Site.OwnerPhone = txtCustomerPhone.Text;
        //        //}
        //        CurrentBRSite.SaveSite(Site);
        //        if (EditSite == null)
        //        {
        //            DOContactSite contactSite = new DOContactSite();
        //            contactSite.ContactSiteID = Guid.NewGuid();
        //            //contactSite.ContactID = Site.ContactID;
        //            contactSite.ContactID = Site.SiteOwnerID;
        //            contactSite.SiteID = Site.SiteID;
        //            contactSite.CreatedBy = Site.CreatedBy;
        //            contactSite.Active = false;
        //            CurrentBRSite.SaveContactSite(contactSite);
        //            DOContactSite contactSiteForCreatingContact = new DOContactSite();
        //            contactSiteForCreatingContact.ContactSiteID = Guid.NewGuid();
        //            //contactSiteForCreatingContact.PersistenceStatus;
        //            contactSiteForCreatingContact.ContactID = CurrentSessionContext.CurrentContact.ContactID;
        //            contactSiteForCreatingContact.SiteID = Site.SiteID;
        //            contactSiteForCreatingContact.CreatedBy = Site.CreatedBy;
        //            contactSiteForCreatingContact.Active = false;
        //            CurrentBRSite.SaveContactSite(contactSiteForCreatingContact);
        //            //if(Site.ContactID!=Site.SiteOwnerID && Site.ContactID!= CurrentSessionContext.CurrentContact.ContactID)
        //            if (CurrentSessionContext.CurrentContractee != null)
        //            {
        //                if (Site.SiteOwnerID != CurrentSessionContext.CurrentContractee.ContactID && CurrentSessionContext.CurrentContractee.ContactID != CurrentSessionContext.CurrentContact.ContactID)
        //                {
        //                    DOContactSite contactSiteForCreatingCompanyContact = new DOContactSite();
        //                    contactSiteForCreatingCompanyContact.ContactSiteID = Guid.NewGuid();
        //                    //contactSiteForCreatingContact.PersistenceStatus;
        //                    //contactSiteForCreatingCompanyContact.ContactID = Site.ContactID;
        //                    contactSiteForCreatingCompanyContact.ContactID = CurrentSessionContext.CurrentContractee.ContactID;
        //                    contactSiteForCreatingCompanyContact.SiteID = Site.SiteID;
        //                    contactSiteForCreatingCompanyContact.CreatedBy = Site.CreatedBy;
        //                    contactSiteForCreatingCompanyContact.Active = false;
        //                    DOContactSite checksite = CurrentBRSite.SelectContactSite(Site.SiteID,
        //                        CurrentSessionContext.CurrentContractee.ContactID);
        //                    if (checksite == null)
        //                    {
        //                        CurrentBRSite.SaveContactSite(contactSiteForCreatingCompanyContact);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (FieldValidationException ex)
        //    {
        //        Site = null;
        //        if (Site == null && customerOwn == false)
        //        {
        //            // Tony modified 31.Oct.2016
        //            if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentContact.ContactID != null && CurrentSessionContext.CurrentCustomer.ContactID != null)
        //            // Tony modified 31.Oct.2016
        //            {
        //                if (CurrentSessionContext.CurrentContractee != Customer && CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentCustomer.ContactID)
        //                {
        //                    if (Customer != null)
        //                        RollBackCustomer(CurrentSessionContext.CurrentContractee, Customer);
        //                }
        //            }

        //        }
        //        if (ex.Message.Contains("Customer"))
        //        {
        //            Error.Text = ex.Message;
        //            Error.Visible = true;
        //            ddlCustomer.Focus();
        //        }
        //        else if (!ex.Message.Contains("site"))
        //        {
        //            ErrorCustomer.Text = ex.Message;
        //            ErrorCustomer.Visible = true;
        //            if (!SelfSite)
        //                Txt_Phone.Focus();
        //        }
        //        //ShowMessage(ex.Message, MessageType.Error);
        //        //CurrentBRContact.DeleteContact(Contact);
        //        //throw new FieldValidationException(ex.Message);
        //    }
        //    return Site;

        //}

        private DOSite Save()
        {
            DOSite Site = EditSite;
            bool NewSite = false;
            if (EditSite == null)
            {
                NewSite = true;
                Site = CurrentBRSite.CreateSite(CurrentSessionContext.Owner.ContactID);//createdby only
            }
            try
            {
                if (NewSite)
                {
                    if (!HaveSiteOwnerDetails)
                    {
                        ContactSiteOwner = CreateNoDetailsContactForCustomer();
                        // ContractorCustomerSiteOwner = CurrentBRContact.SelectContractorCustomerByCCID()
                    }
                    Site.Address1 = txtSiteAddress1.Text;
                    if (SuburbDD.SelectedItem != null)
                    {
                        if (!string.IsNullOrEmpty(SuburbDD.SelectedItem.ToString()))
                        {
                            if (SuburbDD.SelectedItem.ToString() == "-Select-")
                            {
                                Error.Text = "Please select suburb";
                                Error.Visible = true;
                                throw new FieldValidationException("Please select suburb for site");
                            }
                            else
                                Site.Address2 = SuburbDD.SelectedItem.ToString();
                        }
                        else ShowMessage("Please select suburb");
                    }
                    else
                    {
                        Error.Text = "Please select suburb";
                        Error.Visible = true;
                        LoadSuburbs();
                        if (NewSite) RollBackCustomer(CurrentSessionContext.CurrentCustomer, Customer);
                    }
                    if (!string.IsNullOrEmpty(District_DDL.SelectedItem.ToString())) Site.Address3 = District_DDL.SelectedItem.ToString();
                    else
                    {
                        ShowMessage("Please select District");
                    }
                    if (!string.IsNullOrEmpty(RegionDD.SelectedItem.ToString()))
                        Site.Address4 = RegionDD.SelectedItem.ToString();
                    else
                        ShowMessage("Please select Region");
                    //Site.SiteOwnerID = ContractorCustomerSiteOwner.ContactCustomerId;
                    //check contractorcustomer relationship exists already
                    DOContractorCustomer Selfdocc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentContact.ContactID);
                    DOContractorCustomer CreatorAndSiteOwnerdocc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, ContactSiteOwner.ContactID);
                    DOContractorCustomer SiteOwnerCC = CurrentBRContact.SelectContractorCustomer(ContactSiteOwner.ContactID, ContactSiteOwner.ContactID);
                    //siteowner and themselves
                    if (SiteOwnerCC == null)
                    {
                        SiteOwnerCC = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, ContactSiteOwner.ContactID,
                           ContactSiteOwner.ContactID, ContactSiteOwner.Address1, ContactSiteOwner.Address2, ContactSiteOwner.Address3, ContactSiteOwner.Address4, "",
                           DOContractorCustomer.LinkedEnum.NotLinked, "", Selfdocc.ContactCustomerId, "", "", 1);// 3/7/17 modified Selfdocc.ContactCustomerId from guid.empty Jared
                        CurrentBRContact.SaveContractorCustomer(SiteOwnerCC);
                    }
                    //you and new siteowner
                    if (CreatorAndSiteOwnerdocc == null)
                    { 
                        //if (SiteOwnerCC.ContactCustomerId != CreatorAndSiteOwnerdocc.ContactCustomerId)
                        //{
                            CreatorAndSiteOwnerdocc = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, ContactSiteOwner.ContactID,
                                    CurrentSessionContext.CurrentContact.ContactID, Site.Address1, Site.Address2, Site.Address3, Site.Address4, "",
                                    DOContractorCustomer.LinkedEnum.NotLinked, "", Selfdocc.ContactCustomerId, "", "", 1);
                            CurrentBRContact.SaveContractorCustomer(CreatorAndSiteOwnerdocc);
                       // }
                    }
                    if (CurrentSessionContext.CurrentContractee.ContactID != CurrentSessionContext.CurrentContact.ContactID)
                    {
                        //the new siteowner and your 'customer'
                        DOContractorCustomer ThirdPartydocc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContractee.ContactID, ContactSiteOwner.ContactID);
                        if (ThirdPartydocc == null)
                        {
                            ThirdPartydocc = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, ContactSiteOwner.ContactID,
                                CurrentSessionContext.CurrentContractee.ContactID, Site.Address1, Site.Address2, Site.Address3, Site.Address4, "",
                                DOContractorCustomer.LinkedEnum.NotLinked,ContactSiteOwner.Phone , Selfdocc.ContactCustomerId, ContactSiteOwner.FirstName, ContactSiteOwner.LastName,(int)CurrentSessionContext.CurrentContractee.ContactType);
                            CurrentBRContact.SaveContractorCustomer(ThirdPartydocc);
                        }
                    }

                    Site.SiteOwnerID = SiteOwnerCC.ContactCustomerId;
                    CurrentBRSite.SaveSite(Site);
                    if (EditSite == null)
                    {

                        //create a contactsite for creator
                        DOContactSite contactSite = new DOContactSite();
                        contactSite.ContactSiteID = Guid.NewGuid();
                        //contactSite.ContactID = Site.ContactID;
                        //DOContractorCustomer CC = CurrentBRContact.SelectContractorCustomerByCCID(Site.SiteOwnerID);//2017.4.6 jared

                        contactSite.ContactID = ContactSiteOwner.ContactID;  //CC.CustomerID;
                        contactSite.SiteID = Site.SiteID;
                        contactSite.CreatedBy = Site.CreatedBy;
                        contactSite.Active = false;
                        CurrentBRSite.SaveContactSite(contactSite);
                        DOContactSite contactSiteForCreatingContact = new DOContactSite();
                        contactSiteForCreatingContact.ContactSiteID = Guid.NewGuid();
                        //contactSiteForCreatingContact.PersistenceStatus;
                        contactSiteForCreatingContact.ContactID = CurrentSessionContext.CurrentContact.ContactID;
                        contactSiteForCreatingContact.SiteID = Site.SiteID;
                        contactSiteForCreatingContact.CreatedBy = Site.CreatedBy;
                        contactSiteForCreatingContact.Active = false;
                        CurrentBRSite.SaveContactSite(contactSiteForCreatingContact);
                        //if(Site.ContactID!=Site.SiteOwnerID && Site.ContactID!= CurrentSessionContext.CurrentContact.ContactID)

                        //if the creator is not the same as the pathway contactid(your customer normally)
                        if (Site.SiteOwnerID != CurrentSessionContext.CurrentContractee.ContactID && CurrentSessionContext.CurrentContractee.ContactID != CurrentSessionContext.CurrentContact.ContactID)
                        {
                            DOContactSite contactSiteForCreatingCompanyContact = new DOContactSite();
                            contactSiteForCreatingCompanyContact.ContactSiteID = Guid.NewGuid();
                            //contactSiteForCreatingContact.PersistenceStatus;
                            //contactSiteForCreatingCompanyContact.ContactID = Site.ContactID;
                            contactSiteForCreatingCompanyContact.ContactID = CurrentSessionContext.CurrentContractee.ContactID;
                            contactSiteForCreatingCompanyContact.SiteID = Site.SiteID;
                            contactSiteForCreatingCompanyContact.CreatedBy = Site.CreatedBy;
                            contactSiteForCreatingCompanyContact.Active = false;
                            DOContactSite checksite = CurrentBRSite.SelectContactSite(Site.SiteID,
                                CurrentSessionContext.CurrentContractee.ContactID);
                            if (checksite == null)
                            {
                                CurrentBRSite.SaveContactSite(contactSiteForCreatingCompanyContact);
                            }
                           
                        }
                        
                    }
                }

            }
            catch (FieldValidationException ex)
            {
                Site = null;
                if (Site == null && customerOwn == false)
                {
                    // Tony modified 31.Oct.2016
                    if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentContact.ContactID != null && CurrentSessionContext.CurrentCustomer.ContactID != null)
                    // Tony modified 31.Oct.2016
                    {
                        if (CurrentSessionContext.CurrentContractee != Customer && CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentCustomer.ContactID)
                        {
                            if (Customer != null)
                                RollBackCustomer(CurrentSessionContext.CurrentContractee, Customer);
                        }
                    }

                }
                if (ex.Message.Contains("Customer"))
                {
                    Error.Text = ex.Message;
                    Error.Visible = true;
                    ddlCustomer.Focus();
                }
                else if (!ex.Message.Contains("site"))
                {
                    ErrorCustomer.Text = ex.Message;
                    ErrorCustomer.Visible = true;
                    if (!SelfSite)
                        Txt_Phone.Focus();
                }
                //ShowMessage(ex.Message, MessageType.Error);
                //CurrentBRContact.DeleteContact(Contact);
                //throw new FieldValidationException(ex.Message);
            }
            return Site;

        }

        private void RollBackCustomer(DOContact contact, DOContact customer)
        {
            CurrentBRContact.DeleteCustomer(contact, customer);
            //CurrentBRContact.DeleteContact(contact);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Redirect();
            pnlCustomerDetails.Visible = false;
            pnlCustomerExists.Visible = false;
            phNew.Visible = false;
            NewCustomerButtons.Visible = false;
        }

        private void Redirect()
        {
            //If site selected, go to job.
            //If customer selected, return to customer home.
            //Otherwise, return to contact home.

            if (CurrentSessionContext.CurrentSite != null && EditSite == null)
                Response.Redirect(Constants.URL_JobSummary);
            else if (SelfSite)
                Response.Redirect("~/private/mysites.aspx", false);
            else if (CurrentSessionContext.CurrentCustomer == null && CurrentSessionContext.LastContactPageType != SessionContext.LastContactPageTypeEnum.Customer)
            {
                CurrentSessionContext.CurrentContact = null;
                Response.Redirect(Constants.URL_ContactHome);

            }
            else
                Response.Redirect(Constants.URL_CustomerHome);
        }


        protected void Btn_NoDetails_Click(object sender, EventArgs e)//method by jared not complete or even close, probably need new public flag
        {

            if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentCustomer == null)
                CurrentSessionContext.CurrentCustomer = CurrentSessionContext.CurrentContractee;
            HaveSiteOwnerDetails = false;
            try
            {

                // CheckSiteData();
                DOSite Site = Save();
                CurrentSessionContext.CurrentSite = Site;
                //add to contractorcustomer table
                //DOContractorCustomer docc =  CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, ContactSiteOwner.ContactID, CurrentSessionContext.CurrentContact.ContactID, Site.Address1, Site.Address2, Site.Address3, Site.Address4, "", DOContractorCustomer.LinkedEnum.NotLinked, "");
                //Site.SiteOwnerID = docc.ContactCustomerId;
                //CurrentBRContact.SaveContractorCustomer(docc);
                if (SelfSite && Site != null)
                    Response.Redirect(Constants.URL_SiteHome);
                if (Site != null)
                    Redirect();
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message);
            }
        }














        protected void Btn_CustomerOwn_Click(object sender, EventArgs e)
        {
            try
            {
                Guid x = Guid.Empty;
                HaveSiteOwnerDetails = Guid.TryParse(ddlCustomer.SelectedValue, out x);
                if (HaveSiteOwnerDetails) ContactSiteOwner = CurrentBRContact.SelectContact(Guid.Parse(ddlCustomer.SelectedValue));
                DOSite Site = Save();
                CurrentSessionContext.CurrentSite = Site;
                if (SelfSite && Site != null)
                    Response.Redirect(Constants.URL_SiteHome);
                if (Site != null)
                    Redirect();
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message);
            }

            //jared 20.2.17
            //if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentCustomer == null)
            //    CurrentSessionContext.CurrentCustomer = CurrentSessionContext.CurrentContractee;
            //customerOwn = true;
            //SelfSite = true;

            //try
            //{

            //    // CheckSiteData();
            //    //jared 20.2.2017
            //    if(selectedSiteOwner!=null) selectedSiteOwner = CurrentBRContact.SelectContact(Guid.Parse(ddlCustomer.SelectedValue));
            //    DOSite Site = Save();
            //    CurrentSessionContext.CurrentSite = Site;
            //    if (SelfSite && Site != null)
            //        Response.Redirect(Constants.URL_SiteHome);
            //    if (Site != null)
            //        Redirect();
            //}
            //catch (FieldValidationException ex)
            //{
            //    ShowMessage(ex.Message);
            //}
            //Jared end of block
        }
        //20.2.17 jared
        //protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        //{
        //    newCustomer = true;
        //    if (string.IsNullOrEmpty(txtNewCustomerEmail.Text))
        //    {
        //        ShowMessage("Please enter the customer email address.");
        //        txtNewCustomerEmail.Focus();
        //        return;
        //    }
        //    SelfSite = false;
        //    CheckIfCustomerExists();
        //    NewCustomerButtons.Visible = true;
        //    btnSave.Focus();
        //    LoadregionsForCustomer();
        //    // Response.Redirect(Constants.URL_CustomerDetails + "?new=" + Server.UrlEncode(txtNewCustomerEmail.Text));
        //}

        //private void CheckIfCustomerExists()
        //{
        //    List<DOBase> MatchingContacts = null;
        //    suburbReset = true;
        //    string NewCustomer = txtNewCustomerEmail.Text;
        //    if (!string.IsNullOrEmpty(NewCustomer))
        //    {
        //        MatchingContacts = CurrentBRContact.SelectContactsByEmail(NewCustomer);
        //        //Exclude companies who have opted out of being selected as customer.
        //        //Individuals cannot opt out.
        //        if (MatchingContacts.Count != 0)
        //        {
        //            for (int i = MatchingContacts.Count - 1; i >= 0; i--)
        //            {
        //                DOContact c = MatchingContacts[i] as DOContact;
        //                if (c.ContactType == DOContact.ContactTypeEnum.Company && c.CustomerExclude)
        //                    MatchingContacts.RemoveAt(i);
        //                if (c.ContactID == Guid.Empty || c.ContactID == Constants.Guid_DefaultUser)
        //                    MatchingContacts.RemoveAt(i);
        //            }
        //        }


        //    }

        //    if (MatchingContacts == null || MatchingContacts.Count == 0)
        //    {
        //        txtCustomerEmail.Text = NewCustomer;
        //        btnSave.Enabled = true;
        //        //btnSaveAddSite.Enabled = true;
        //        pnlCustomerDetails.Visible = true;
        //        Txt_Email.Text = NewCustomer;
        //        newCustomer = true;
        //        phNew.Visible = true;
        //        pnlCustomerDetails.Focus();
        //    }
        //    else if (MatchingContacts.Count == 1)
        //    {
        //        existingCustomer = true;
        //        ExistingContact = MatchingContacts[0] as DOContact;
        //        pnlCustomerExists.Visible = true;
        //        pnlCustomerDetails.Visible = true;
        //        phOneExisting.Visible = true;
        //        phMultipleExisting.Visible = false;
        //        btnSave.Enabled = false;
        //        //btnSaveAddSite.Enabled = false;
        //        PrefillFromContact(MatchingContacts[0] as DOContact);
        //        CurrentSessionContext.CurrentCustomer = null;
        //        CurrentSessionContext.CurrentCustomer = new DOContact();
        //        if (ExistingContact != null)
        //        {
        //            CurrentSessionContext.CurrentCustomer.ContactID = ExistingContact.ContactID;
        //            CurrentSessionContext.CurrentCustomer.CompanyName = ExistingContact.CompanyName;
        //            CurrentSessionContext.CurrentCustomer.Email = ExistingContact.Email;
        //            CurrentSessionContext.CurrentCustomer.Address1 = ExistingContact.Address1;
        //            CurrentSessionContext.CurrentCustomer.Address2 = ExistingContact.Address2;
        //            CurrentSessionContext.CurrentCustomer.Address3 = ExistingContact.Address3;
        //            CurrentSessionContext.CurrentCustomer.Address4 = ExistingContact.Address4;
        //            CurrentSessionContext.CurrentCustomer.FirstName = ExistingContact.FirstName;
        //            CurrentSessionContext.CurrentCustomer.LastName = ExistingContact.LastName;
        //            CurrentSessionContext.CurrentCustomer.Phone = ExistingContact.Phone;
        //            //DOCustomer cust = CurrentBRContact.SelectCustomerbyContactID(ExistingContact.ContactID);
        //            //CurrentSessionContext.CurrentCustomer.co = cust.CustomerID;
        //            //new logic, no customer only contact
        //            // DOContact cust = CurrentBRContact.SelectCustomerbyContactID(ExistingContact.ContactID);
        //            // CurrentSessionContext.CurrentCustomer.ContactID = cust.ContactID;

        //        }
        //        //    ////CurrentSessionContext
        //        //   CurrentSessionContext.CurrentCustomer = CurrentBRContact.SelectCustomer(ExistingContact.ContactID);
        //        //CurrentSessionContext.CurrentCustomer = CurrentBRContact.SelectContactsByEmail(ExistingContact.e);
        //        // Customer.ContactID = CustomerContact.ContactID;
        //        //CurrentSessionContext.CurrentCustomer = MatchingContacts[0];
        //        // CurrentSessionContext.CurrentCustomer = null;
        //        //Guid contactID = ExistingContact.ContactID;
        //        //CurrentSessionContext.CurrentCustomer = CurrentBRContact.SelectCustomers(contactID);
        //    }
        //    else
        //    {
        //        pnlCustomerExists.Visible = true;
        //        pnlCustomerExists.Focus();
        //        pnlCustomerDetails.Visible = false;
        //        phOneExisting.Visible = false;
        //        phMultipleExisting.Visible = true;
        //        btnSave.Enabled = false;
        //        btnSaveAddJob.Enabled = false;
        //        //btnSaveAddSite.Enabled = false;
        //        //CurrentSessionContext.CurrentCustomer = null;
        //        litExistingEmail.Text = NewCustomer;

        //        rpExistingCustomers.DataSource = MatchingContacts;
        //        rpExistingCustomers.DataBind();
        //    }
        //    if (CurrentSessionContext.CurrentCustomer != null)
        //        litContactName.Text = CurrentSessionContext.CurrentCustomer.DisplayName;
        //}
        //jared end of block
        private DOContact CreateContactForCustomer(string Email)
        {
            DOContact Contact = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, DOContact.ContactTypeEnum.Individual);
            Contact.Email = Email;
            Contact.FirstName = Txt_FirstName.Text;
            Contact.LastName = Txt_LastName.Text;
            Contact.Phone = Txt_Phone.Text;
            Contact.Address1 = Txt_Add1.Text;
            if (DDL_Add2.SelectedItem.ToString() != "-Select-")
                Contact.Address2 = DDL_Add2.SelectedItem.ToString();
            else
            {
                ErrorCustomer.Text = "Please select district and suburb";
                ErrorCustomer.Visible = true;
                throw new FieldValidationException("Please select suburb");
                // return null;
            }
            Contact.Address3 = DDL_Address3.SelectedItem.ToString();
            Contact.Address4 = DDL_Address4.SelectedItem.ToString();
            Contact.PendingUser = true;
            //jared here pendingsiteowner
            //if (NoDetails == true) Contact.PendingSiteOwner = true;
            CurrentBRContact.SaveContact(Contact);
            return Contact;
        }

        private DOContact CreateNoDetailsContactForCustomer()
        {
            //DOContact 
            ContactSiteOwner = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, DOContact.ContactTypeEnum.Individual);//!here! need to allow for companies
            ContactSiteOwner.Email = "@." + (ContactSiteOwner.ContactID);
            ContactSiteOwner.FirstName = "";
            ContactSiteOwner.LastName = "";
            ContactSiteOwner.Phone = "pending";
            ContactSiteOwner.Address1 = "pending";
            ContactSiteOwner.Address2 = "pending";
            ContactSiteOwner.Address3 = "pending";
            ContactSiteOwner.Address4 = "pending";
            ContactSiteOwner.PendingUser = true;
            ContactSiteOwner.PendingSiteOwner = true;

            CurrentBRContact.SaveContact(ContactSiteOwner);
            return ContactSiteOwner;
        }




        protected void btnNo_Click(object sender, EventArgs e)
        {
            txtCustomerAddress1.Text = string.Empty;
            txtCustomerAddress2.Text = string.Empty;
            txtCustomerCompanyName.Text = string.Empty;
            txtCustomerFirstName.Text = string.Empty;
            txtCustomerLastName.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;

            txtCustomerAddress1.Attributes.Remove("readonly");
            txtCustomerAddress2.Attributes.Remove("readonly");
            txtCustomerCompanyName.Attributes.Remove("readonly");
            txtCustomerFirstName.Attributes.Remove("readonly");
            txtCustomerLastName.Attributes.Remove("readonly");
            txtCustomerPhone.Attributes.Remove("readonly");
            txtCustomerEmail.Text = Request.QueryString["new"];
            txtCustomerEmail.Enabled = false;
            pnlCustomerExists.Visible = false;
            pnlCustomerDetails.Visible = true;
            btnSave.Enabled = true;
            //btnSaveAddSite.Enabled = false;
        }
        private void PrefillFromContact(DOContact Contact)
        {
            Txt_Add1.Text = Contact.Address1;
            Txt_Add1.ReadOnly = true;
            //txtCustomerAddress2.Text = Contact.Address2;
            // TB_Address3.Text = Contact.Address3;
            //TB_Address4.Text = Contact.Address4;
            if (Contact.Address3 != null && Contact.Address3 != "")
            {
                string district = Contact.Address3;
                foreach (ListItem item in DDL_Address3.Items)
                    item.Selected = false;
                if (DDL_Address3.Items.Contains(new ListItem(district)))
                {
                    DDL_Address3.Items.FindByText(district).Selected = true;
                    DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                    DDL_Address3.DataBind();
                    DDL_Address3.PreRender += (object sender1, EventArgs e1) =>
                    {
                        //LoadDistrict();
                        foreach (ListItem item in DDL_Address3.Items)
                            item.Selected = false;
                        DDL_Address3.Items.FindByText(Contact.Address3).Selected = true;
                        DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                        if (DDL_Address3.SelectedIndex == -1)
                            DDL_Address3.SelectedIndex = 2;
                        DDL_Address3.DataBind();
                    };
                    LoadSuburbsForCustomer();
                }
            }
            txtCustomerCompanyName.Text = Contact.CompanyName;
            Txt_Email.Text = Contact.Email;
            Txt_FirstName.Text = Contact.FirstName;
            Txt_LastName.Text = Contact.LastName;
            Txt_Phone.Text = Contact.Phone;

            if (string.IsNullOrEmpty(Txt_FirstName.Text))
                Txt_FirstName.Text = "-";
            if (string.IsNullOrEmpty(Txt_LastName.Text))
                Txt_LastName.Text = "-";
            txtCustomerCompanyName.Attributes.Add("readonly", "readonly");
            Txt_Email.Attributes.Add("readonly", "readonly");
            Txt_FirstName.Attributes.Add("readonly", "readonly");
            Txt_LastName.Attributes.Add("readonly", "readonly");
            Txt_Phone.Attributes.Add("readonly", "readonly");

        }
        protected void btnSaveFromMulti_Click(object sender, EventArgs e)
        {
            Guid ContactID = new Guid(((Button)sender).CommandArgument);
            DOContact c = CurrentBRContact.SelectContact(ContactID);
            // PrefillFromContact(c);
            // ExistingContact = c;
            ContactSiteOwner = c;
            SelfSite = false;
            btnSave_Click(sender, e);
        }

        protected void CancelAddSite_Click(object sender, EventArgs e)
        {
            Redirect();
        }
        protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbs();
            Error.Visible = false;
            // SuburbDD.Focus();
        }
        protected void LoadSuburbs()
        {

            //PageBase pb = new PageBase();
            if (District_DDL.SelectedValue != "")
            {
                List<DOBase> Suburbs = CurrentBRSuburb.SelectSuburbsSorted(Guid.Parse(District_DDL.SelectedValue));
                SuburbDD.DataSource = Suburbs;
                SuburbDD.DataTextField = "SuburbName";
                SuburbDD.DataValueField = "SuburbID";
                SuburbDD.DataBind();
            }

        }
        protected void LoadSuburbsForCustomer()
        {
            if (DDL_Address3.SelectedValue != "")
            {
                List<DOBase> Suburbs = CurrentBRSuburb.SelectSuburbsSorted(Guid.Parse(DDL_Address3.SelectedValue));
                DDL_Add2.DataSource = Suburbs;
                DDL_Add2.DataTextField = "SuburbName";
                DDL_Add2.DataValueField = "SuburbID";
                DDL_Add2.DataBind();
            }

        }
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrict();
            LoadSuburbs();

        }
        public void LoadregionsForCustomer()
        {

            List<DOBase> Regions = CurrentBRRegion.SelectRegions();
            DDL_Address4.DataSource = Regions;
            DDL_Address4.DataTextField = "RegionName";
            DDL_Address4.DataValueField = "RegionID";
            DDL_Address4.DataBind();
            DDL_Address4.SelectedValue = CurrentSessionContext.CurrentContact.DefaultRegion.ToString();
            LoadDistrictForCustomer();

        }
        public void Loadregions()
        {

            List<DOBase> Regions = CurrentBRRegion.SelectRegions();
            RegionDD.DataSource = Regions;
            RegionDD.DataTextField = "RegionName";
            RegionDD.DataValueField = "RegionID";

            RegionDD.DataBind();
            RegionDD.SelectedValue = CurrentSessionContext.CurrentContact.DefaultRegion.ToString();
        }
        public void LoadDistrictForCustomer()
        {

            List<DOBase> Districts = CurrentBRDistrict.SelectDistricts(Guid.Parse(DDL_Address4.SelectedValue));
            DDL_Address3.DataSource = Districts;
            DDL_Address3.DataTextField = "DistrictName";
            DDL_Address3.DataValueField = "DistrictID";
            //District_DDL.Items.Insert(0, new ListItem("-Select-", ""));
            //District_DDL.SelectedItem.Value = "Christchurch";
            //District_DDL.SelectedValue = "-Select-";
            DDL_Address3.DataBind();
            DDL_Address3.SelectedIndex = 2;
            LoadSuburbsForCustomer();
        }
        public void LoadDistrict()
        {
            PageBase pb = new PageBase();
            List<DOBase> Districts = pb.CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
            // Districts.Insert(-1, new ListItem("-Select-", ""));
            //District_DDL.DataBind();
            District_DDL.DataSource = Districts;
            District_DDL.DataTextField = "DistrictName";
            District_DDL.DataValueField = "DistrictID";
            //District_DDL.Items.Insert(0, new ListItem("-Select-", ""));
            //District_DDL.SelectedItem.Value = "Christchurch";
            //District_DDL.SelectedValue = "-Select-";
            //District_DDL.SelectedIndex = 2;
            District_DDL.DataBind();
            District_DDL.PreRender += (object sender, EventArgs e) =>
            {
                //LoadDistrict();
                foreach (ListItem item in District_DDL.Items)
                    item.Selected = false;
                if ((District_DDL.SelectedIndex == -1 || District_DDL.SelectedIndex == 0) && District_DDL.Items.Count >= 2)
                    District_DDL.SelectedIndex = 2;
                District_DDL.DataBind();
                LoadSuburbs();
            };

        }

        ////Tony Added function for populating dropdown list of customer. 5/11/2016
        //protected void LoadContact()
        //{
        //    List<DOBase> contact = CurrentBRContact.SelectContacts();
        //    ContactDD.DataSource = contact;
        //    ContactDD.DataTextField = "CompanyName";
        //    ContactDD.DataValueField = "ContactID";
        //    ContactDD.DataBind();
        //}


        //protected void LoadContact()
        //{
        //    List<DOBase> contact = CurrentBRContact.SelectCustomers(CurrentSessionContext.CurrentContact.ContactID);
        //    ContactDD.DataSource = contact;
        //    ContactDD.DataTextField = "DisplayName";
        //    ContactDD.DataValueField = "ContactID";
        //    ContactDD.DataBind();
        //}

        protected void DDL_Address3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbsForCustomer();
        }

        protected void SuburbDD_PreRender(object sender, EventArgs e)
        {
            DOSite mysite = CurrentSessionContext.CurrentSite;

            if (mysite != null)
            {
                string suburb = mysite.Address2;
                string district = mysite.Address3;
                string region = mysite.Address4;

                if (!IsPostBack)
                {

                    if (CurrentSessionContext.CurrentCustomer != null)
                    {
                        SuburbDD.SelectedIndex =
                            SuburbDD.Items.IndexOf(
                                SuburbDD.Items.FindByText(CurrentSessionContext.CurrentCustomer.Address2));
                        if (SuburbDD.SelectedIndex == -1 ||
                            (SuburbDD.SelectedIndex == 0 &&
                             CurrentSessionContext.CurrentCustomer.Address2 != SuburbDD.SelectedItem.ToString()))
                        {
                            SuburbDD.Items.Insert(0, new ListItem("-Select-", ""));
                            foreach (ListItem item in SuburbDD.Items)
                                item.Selected = false;
                            //                        SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText("-Select-"));
                        }
                    }
                    else
                    {
                        //Tony modified 3.Nov.2016
                        //                    SuburbDD.Items.Insert(0, new ListItem("-Select-", ""));
                        //                    foreach (ListItem item in SuburbDD.Items)
                        //                        item.Selected = false;
                        //                    SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText("-Select-"));
                        //Tony modified 3.Nov.2016
                    }
                    //Tony modified 3.Nov.2016
                    RegionDD.SelectedIndex = RegionDD.Items.IndexOf(RegionDD.Items.FindByText(region));
                    LoadDistrict();

                    District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                    LoadSuburbs();

                    SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
                    //Tony modified 3.Nov.2016


                }
            }
        }

        protected void DDL_Add2_PreRender(object sender, EventArgs e)
        {
            if (suburbReset)
            {
                bool found = false;
                foreach (ListItem i in DDL_Add2.Items)
                {
                    DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText("-Select-"));
                    if (DDL_Add2.SelectedItem.ToString() == "-Select-")
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    if (DDL_Add2.SelectedIndex == -1 || DDL_Add2.SelectedIndex == 0)
                    {
                        DDL_Add2.Items.Insert(0, new ListItem("-Select-", ""));
                        foreach (ListItem item in DDL_Add2.Items)
                            item.Selected = false;
                        DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText("-Select-"));
                    }
                }
            }
        }

        protected void DDL_Add2_SelectedIndexChanged(object sender, EventArgs e)
        {
            suburbReset = false;
            btnSave.Focus();
            ErrorCustomer.Visible = false;
        }
        // 	2017.2.15 commented. Jared
        //        protected void btnShare_Click(object sender, EventArgs e)
        //        {
        //            //Tony added 5.11.2016
        //            //If confirm check box for share site information checked, call shareSite method
        //            DOSite mysite = CurrentSessionContext.CurrentSite;

        //            siteID = mysite.SiteID;
        //            fromContactID = mysite.ContactID;
        //            toContactID = new Guid(ContactDD.SelectedItem.Value);

        //            shareSite(siteID, fromContactID, toContactID);
        ////Tony added 5.11.2016
        //        }
    }
}