using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
    public struct CustomersList
    {
        public DOContact Contact { get; set; }
        public string Type { get; set; }
        public string Visible { get; set; }
    }

    [PrivatePage]
    public partial class CustomerDetails : PageBase
    {
        List<DOBase> MatchingContacts = null;
        DOContact ExistingContact = null;
        static bool SaveExistingContact = false;
        private static bool individualContactExists = false;
        private static bool companyContactExists = false;
        private static bool showDetails = false;
        DOContact contactCompany = null;
        public List<CustomersList> CustomersLists { get; } = new List<CustomersList>();
        public static bool newCompany;
        protected void Page_Init(object sender, EventArgs e)
        {
            //The customer must be added to a contact.
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
            if (!IsPostBack)
            {
                string newCustomer = Request.QueryString["new"];
                if (!string.IsNullOrEmpty(newCustomer))
                {
                    FindMatchingContacts(newCustomer);
                    //Exclude companies who have opted out of being selected as customer.
                    //Individuals cannot opt out.
                    for (int i = MatchingContacts.Count - 1; i >= 0; i--)
                    {
                        DOContact c = MatchingContacts[i] as DOContact;
                        if (c.ContactType == DOContact.ContactTypeEnum.Company && c.CustomerExclude)
                            MatchingContacts.RemoveAt(i);
                        if (c.ContactID == Guid.Empty || c.ContactID == Constants.Guid_DefaultUser)
                            MatchingContacts.RemoveAt(i);
                    }
                }
                if (MatchingContacts == null || MatchingContacts.Count == 0)
                {
                    txtCustomerEmail.Text = newCustomer;
                    pnlCustomerDetails.Visible = true;
                    txtCustomerEmail.Enabled = false;
                    phMultipleExisting.Visible = false;
                    SaveCancelPnl.Visible = true;
                    btnSave_Notify.Visible = false;
                    btnSave.Visible = true;

                }
                else
                {
                    if (CustomersLists.Count > 0)
                    {
                        pnlCustomerExists.Visible = true;
                        Linked_lbl.Visible = true;
                        pnlCustomerDetails.Visible = false;
                        phMultipleExisting.Visible = true;
                        RepCustomersList.DataSource = CustomersLists;
                        foreach (CustomersList customer in CustomersLists)
                        {
                            /// Verify if Individual and company already exists for an email Id, if individual already exists, 
                            /// no more individual can be added with the same email id again, another company can be added.
                            if (customer.Contact.ContactType == DOContact.ContactTypeEnum.Individual)
                                newCompany = true;
                        }
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                    }
                    else
                    {
                        pnlCustomerDetails.Visible = true;
                        txtCustomerEmail.Text = newCustomer;
                        litExistingEmail.Visible = false;
                        litContactName.Visible = false;
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                    }
                    CurrentSessionContext.CurrentCustomer = null;
                    litExistingEmail.Text = newCustomer;
                }

                if (CurrentSessionContext.CurrentContact != null)
                    litContactName.Text = CurrentSessionContext.CurrentContact.DisplayName;
            }
        }

        /// <summary>
        /// if the customer already exists in the database and details are available populate that to the Customer details form
        /// </summary>
        /// <param name="Contact"></param>
        private void PrefillFromContact(DOContact Contact)
        {
            txtCustomerAddress1.Text = Contact.Address1;
            if (!string.IsNullOrEmpty(Contact.Address3))
            {
                string district = Contact.Address3;
                foreach (ListItem item in DDL_Address3.Items)
                    item.Selected = false;
                if (DDL_Address3.Items.Contains(new ListItem(district)))
                {
                    DDL_Address3.Items.FindByText(district).Selected = true;
                    DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                    DDL_Address3.DataBind();
                }
                DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                LoadSuburbsForCustomer();
                DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText(Contact.Address2));
            }
            //txtCustomerCompanyName.Text = Contact.CompanyName; jared 16/1/17
            txtCustomerEmail.Text = Contact.Email;
            txtCustomerFirstName.Text = Contact.FirstName;
            txtCustomerLastName.Text = Contact.LastName;
            txtCustomerPhone.Text = Contact.Phone;

            if (string.IsNullOrEmpty(txtCustomerFirstName.Text))
                txtCustomerFirstName.Text = "-";
            if (string.IsNullOrEmpty(txtCustomerLastName.Text))
                txtCustomerLastName.Text = "-";
            DDL_Add2.Enabled = true;
            DDL_Address3.Enabled = true;
            DDL_Address4.Enabled = true;
            pnlCustomerDetails.Visible = true;
            btnCancel.Visible = true;
            btnNoMultiple.Visible = false;
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {

            txtCustomerAddress1.Text = string.Empty;
            //txtCustomerAddress2.Text = string.Empty;
            //txtCustomerCompanyName.Text = string.Empty; jared 16/1/17
            txtCustomerFirstName.Text = string.Empty;

            txtCustomerLastName.Text = string.Empty;

            if (newCompany)
                Cust_name.Visible = false;
            txtCustomerPhone.Text = string.Empty;

            txtCustomerAddress1.Attributes.Remove("readonly");
            //txtCustomerAddress2.Attributes.Remove("readonly");
            //txtCustomerCompanyName.Attributes.Remove("readonly"); jared 16/1/17
            txtCustomerFirstName.Attributes.Remove("readonly");
            txtCustomerLastName.Attributes.Remove("readonly");
            txtCustomerPhone.Attributes.Remove("readonly");
            txtCustomerEmail.Text = Request.QueryString["new"];
            txtCustomerEmail.Enabled = false;
            pnlCustomerExists.Visible = false;
            pnlCustomerDetails.Visible = true;
            SaveCancelPnl.Visible = true;
            btnEditSave.Visible = true;
            btnCancel.Visible = true;
            newCompany = true;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadregionsForCustomer();
            }
            if (CustomersLists.Count > 0)
            {
                foreach (var customer in CustomersLists)
                {
                    if (CurrentSessionContext.CurrentCustomer == null)
                    {
                        if (customer.Type == DOContactInfo.ContactTypeEnum.Individual.ToString())
                        {
                            CurrentSessionContext.CurrentCustomer = customer.Contact;
                        }
                    }
                    else
                    {
                        if (CurrentSessionContext.CurrentCustomer.ContactType.ToString() == DOContactInfo.ContactTypeEnum.Company.ToString())
                        {
                            Cust_name.Visible = false;
                        }
                    }
                }
            }
            //Edit customer if existing customer selected.
            if (!IsPostBack && CurrentSessionContext.CurrentCustomer != null)
            {
                //Get details of the customer from ContractorCustomerTable
                DOContractorCustomer contractorCustomer =
                    CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID,
                        CurrentSessionContext.CurrentCustomer.ContactID);
                if (contractorCustomer != null)
                {
                    txtCustomerFirstName.Text = contractorCustomer.FirstName;
                    txtCustomerLastName.Text = contractorCustomer.LastName;
                    //txtCustomerCompanyName.Text = contractorCustomer.CompanyName; jared 16/1/17
                    txtCustomerEmail.Text = CurrentSessionContext.CurrentCustomer.Email;
                    txtCustomerPhone.Text = contractorCustomer.Phone;
                    txtCustomerAddress1.Text = contractorCustomer.Address1;
                    if (!string.IsNullOrEmpty(contractorCustomer.Address3))
                    {
                        string district = contractorCustomer.Address3;
                        foreach (ListItem item in DDL_Address3.Items)
                            item.Selected = false;
                        DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                        DDL_Address3.DataBind();
                        DDL_Address3.PreRender += (object sender1, EventArgs e1) =>
                        {
                            foreach (ListItem item in DDL_Address3.Items)
                                item.Selected = false;
                            DDL_Address3.SelectedIndex = DDL_Address3.Items.IndexOf(DDL_Address3.Items.FindByText(district));
                            if (DDL_Address3.SelectedIndex == -1)
                                DDL_Address3.SelectedIndex = 2;
                            DDL_Address3.DataBind();
                        };
                    }
                }
                if (contractorCustomer != null)
                {
                    string suburb = contractorCustomer.Address2;
                    DDL_Add2.PreRender += (object sender1, EventArgs e1) =>
                    {
                        foreach (ListItem item in DDL_Add2.Items)
                            item.Selected = false;
                        DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText(suburb));
                        DDL_Add2.DataBind();
                    };
                }
            }
            phNew.Visible = CurrentSessionContext.CurrentCustomer == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentCustomer != null)
                SaveExistingContact = true;
            if (newCompany)
                CurrentSessionContext.CurrentCustomer = null;
            try
            {
                DOContact Customer = Save(ExistingContact);
                if (Customer != null)
                {
                    CurrentSessionContext.CurrentCustomer = Customer;
                    Response.Redirect(Constants.URL_CustomerHome);
                }
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message);
            }
        }
        /// <summary>
        /// To use the customer from the list of matching contacts provided 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveFromMulti_Click(object sender, EventArgs e)
        {
            Guid ContactID = new Guid(((Button)sender).CommandArgument);
            DOContact c = CurrentBRContact.SelectContact(ContactID);
            IfExistingCustomer(c);
            PrefillFromContact(c);
            ExistingContact = c;
            CurrentSessionContext.CurrentCustomer = ExistingContact;
        }

        /// <summary>
        /// If customer already exists as your active existing customer, then redirect it to the customer home
        /// </summary>
        /// <param name="contact"></param>
        private void IfExistingCustomer(DOContact contact)
        {
            DOContractorCustomer contractorCustomer =
                CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID,
                    contact.ContactID);

            if (contractorCustomer != null)
            {
                AssignContractorAndCustomerDetails(contractorCustomer, contact);
                Response.Redirect(Constants.URL_CustomerHome);
            }
        }

        /// <summary>
        /// If contractorcustomer is found , assign the current customer as current contractor in the currentsession
        /// </summary>
        /// <param name="contractorCustomer"></param>
        private void AssignContractorAndCustomerDetails(DOContractorCustomer contractorCustomer, DOContact contact)
        {
            CurrentSessionContext.CurrentContractee = CurrentSessionContext.CurrentContact;
            DOContact currentCustomer = contact;
            currentCustomer.FirstName = contractorCustomer.FirstName;
            currentCustomer.LastName = contractorCustomer.LastName;
            currentCustomer.Address1 = contractorCustomer.Address1;
            currentCustomer.Address2 = contractorCustomer.Address2;
            currentCustomer.Address3 = contractorCustomer.Address3;
            currentCustomer.Address4 = contractorCustomer.Address4;
            currentCustomer.CompanyName = contractorCustomer.CompanyName;
            CurrentSessionContext.CurrentCustomer = currentCustomer;
        }


        /// <summary>
        /// If the customer is promoting herself, then details will be visible to everyone. But contractors can still add
        /// their own specific details, for that particular customer. 
        /// </summary>
        private void ShowCustomerDetailsForm()
        {
            pnlCustomerDetails.Visible = true;
            phMultipleExisting.Visible = true;
            SaveCancelPnl.Visible = true;
            //companyName.Visible = CurrentSessionContext.CurrentCustomer.ContactType == DOContact.ContactTypeEnum.Company; jared 16/1/17
            Cust_name.Visible = CurrentSessionContext.CurrentCustomer.ContactType ==
                                DOContact.ContactTypeEnum.Individual;
            btnSave_Notify.Visible = false;
            btnEditSave.Visible = false;
            btnSave.Visible = false;
            btnSaveIndividual.Visible = true;
        }

        /// <summary>
        /// Find matching contacts with the email id from the contact table
        /// </summary>
        /// <param name="email"></param>
        protected void FindMatchingContacts(string email)
        {
            MatchingContacts = CurrentBRContact.SelectContactsByEmail(email);
            if (MatchingContacts.Count > 0)
            {
                foreach (var item in MatchingContacts)
                {
                    DOContact existingContact = item as DOContact;
                    if (CurrentSessionContext.CurrentContact != null && existingContact != null)
                    {
                        DOContractorCustomer contractorCustomer =
                            CurrentBRContact.SelectContractorCustomer(
                                CurrentSessionContext.CurrentContact.ContactID,
                                existingContact.ContactID);
                        if (contractorCustomer != null)
                        {
                            if (contractorCustomer.FirstName != "")
                                existingContact.FirstName = contractorCustomer.FirstName;
                            if (contractorCustomer.LastName != "")
                                existingContact.LastName = contractorCustomer.LastName;
                            existingContact.Phone = contractorCustomer.Phone;
                            existingContact.Address1 = contractorCustomer.Address1;
                            existingContact.Address2 = contractorCustomer.Address2;
                            existingContact.Address3 = contractorCustomer.Address3;
                            existingContact.Address4 = contractorCustomer.Address4;
                            if (contractorCustomer.CompanyName != "")
                                existingContact.CompanyName = contractorCustomer.CompanyName;
                        }
                        else
                        {
                            SetCustomerFormToEmpty();
                        }
                        if ((contractorCustomer != null && contractorCustomer.Deleted == true) || existingContact.Searchable == 1)
                        {
                            if (existingContact.ContactType == DOContact.ContactTypeEnum.Individual)
                            {
                                PrefillFromContact(existingContact);
                            }
                        }
                        else if (contractorCustomer != null && contractorCustomer.Deleted != true)
                        {
                            CustomersLists.Add(new CustomersList()
                            {
                                Contact = existingContact,
                                Type = existingContact.ContactType.ToString(),
                            });
                        }
                    }
                }
            }
        }
        /// <summary>
        /// If the customer details are not visible, then let the contractor put the details they have about the customer, so set the customer details to empty
        /// </summary>
        private void SetCustomerFormToEmpty()
        {
            //txtCustomerCompanyName.Text = ""; jared 16/1/17
            txtCustomerAddress1.Text = "";
            txtCustomerPhone.Text = "";

        }

        protected void btnSaveAddSite_Click(object sender, EventArgs e)
        {
            DOContact Customer = Save();
            if (Customer != null)
            {
                CurrentSessionContext.CurrentCustomer = Customer;
                Response.Redirect(Constants.URL_SiteDetails);
            }
        }

        private DOContact Save()
        {
            return Save(null);
        }

        private DOContact Save(DOContact CustomerContact)
        {
            string oldCompanyName = null;
            bool NewCustomerExistingContact = false;
            bool newCustomer = true;
            //New logic for customer and use contact instead of customer.
            DOContact customer = CurrentSessionContext.CurrentCustomer;
            bool existing = false;
            DOContractorCustomer contractorCustomer = null;
            //If the customer is not null, that means, customer exists but is not visible to this contractor.
            if (CustomerContact != null)
            {
                NewCustomerExistingContact = true;
                newCustomer = false;
            }
            else
            {
                if (CurrentSessionContext.CurrentCustomer != null)
                {
                    if (contractorCustomer == null)
                    {
                        try
                        {
                            //Create new contractor-customer record for the exisiting contact but may be different details for this contractor, and save in COntractor-Customer tabe
                            contractorCustomer = CreateContractorCustomer(CurrentSessionContext.CurrentCustomer);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(exception.Message);
                        }
                        //Send email to the customer to link to the contractor
                        if (contractorCustomer != null)
                            InformCustomerByEmail();
                    }
                    CustomerContact = CurrentSessionContext.CurrentCustomer;
                }
            }
            if (customer != null)
                newCustomer = false;
            try
            {
                //Steps for a new customer.
                if (newCustomer)
                {
                    //Check if there is an existing contact for this customer.
                    string Email = txtCustomerEmail.Text.Trim();
                    if (string.IsNullOrEmpty(Email))
                        throw new FieldValidationException("Customer Email is required.");
                    existing = CustomerContact != null;
                    //If there is no contact, create a contact for this customer.
                    if (!existing)
                    {
                        //If no contact found, create a new contact.
                        if (CustomerContact == null)
                        {
                            try
                            {
                                CustomerContact = CreateContactForCustomer(Email);
                            }
                            catch (FieldValidationException ex)
                            {

                                if (ex.Message.Contains("The email address"))
                                {
                                    DOContact contact = CurrentBRContact.SelectContactByUsername(Email);
                                }
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                }
                if (newCustomer || NewCustomerExistingContact ||
                    (string.IsNullOrEmpty(oldCompanyName) && CurrentSessionContext.CurrentCustomer != null))
                {
                    if (!string.IsNullOrEmpty(CurrentSessionContext.CurrentCustomer?.CompanyName))
                    {
                        if (CustomerContact == null)
                            CustomerContact =
                                CurrentBRContact.SelectContact(CurrentSessionContext.CurrentCustomer.ContactID);
                        DOContact Company = null;
                        if (existing)
                        {
                            Company = CustomerContact;
                        }
                        else
                        {
                            bool companyHasExistingContact = false;
                            //Check if there is a company with the email already.
                            if (customer != null)
                            {
                                List<DOBase> checkCompanies = CurrentBRContact.SelectContactsByEmail(customer.Email);
                                foreach (var doBase in checkCompanies)
                                {
                                    var checkContact = (DOContact)doBase;
                                    if (checkContact.ContactType == DOContact.ContactTypeEnum.Individual) continue;
                                    // If we get to here, there is an existing company.
                                    // This doesn't handle multiple companies with the email. 
                                    // SelectContactsByEmail sorts by Created Date desc so the oldest contact will be linked to.
                                    Company = checkContact;
                                    companyHasExistingContact = true;
                                    break;
                                }
                            }

                            if (!companyHasExistingContact)
                            {
                                //Create a new company if company name specified.
                                Company = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID,
                                    DOContact.ContactTypeEnum.Company);
                                Company.CompanyName = customer.CompanyName;
                                Company.CompanyKey = CurrentBRContact.GenerateCompanyKey();
                                Company.Address1 = customer.Address1;
                                Company.Address2 = customer.Address2;
                                Company.Email = customer.Email;
                                Company.Phone = customer.Phone;
                                Company.UserName = Company.ContactID.ToString();
                                Company.ManagerID = CustomerContact.ContactID;

                                CurrentBRContact.SaveContact(Company);
                            }
                        }
                        if (contractorCustomer != null)
                        {
                            //Check to see if the company is linked to the contact already. 
                            Guid contactCompanyID = CurrentBRContact.SelectContactCompany(CustomerContact.ContactID,
                                Company.ContactID);
                            if (contactCompanyID == Guid.Empty && (CustomerContact.ContactID != Company.ContactID))
                            {
                                //Link the new company to the customer contact.
                                DOContactCompany contactCompany = CurrentBRContact.CreateContactCompany(CustomerContact.ContactID, Company.ContactID,
                                    CurrentSessionContext.Owner.ContactID);
                                CurrentBRContact.SaveContactCompany(contactCompany);
                                //Link the customer to the company contact instead.
                                customer.ContactID = Company.ContactID;
                                //CurrentBRContact.SaveCustomer(Customer);
                            }
                        }
                    }
                    //Link customer to creating contact if not linked already.
                    if (!NewCustomerExistingContact)
                    {
                        if (contractorCustomer == null)
                        {
                            contractorCustomer = CreateContractorCustomer(CurrentSessionContext.CurrentCustomer);
                        }
                        else
                        {
                            contractorCustomer.FirstName = txtCustomerFirstName.Text;
                            contractorCustomer.LastName = txtCustomerLastName.Text;
                            contractorCustomer.Address1 = txtCustomerAddress1.Text;
                            contractorCustomer.Linked = DOContractorCustomer.LinkedEnum.AwaitingCust;
                            if (DDL_Add2.SelectedItem.ToString() != "-Select-")
                                contractorCustomer.Address2 = DDL_Add2.SelectedItem.ToString();
                            else
                            {
                                throw new FieldValidationException("Please select suburb");
                            }
                            contractorCustomer.Address3 = DDL_Address3.SelectedItem.ToString();
                            contractorCustomer.Address4 = DDL_Address4.SelectedItem.ToString();
                            //contractorCustomer.CompanyName = txtCustomerCompanyName.Text;  jared 16/1/17
                            contractorCustomer.Phone = txtCustomerPhone.Text;
                        }
                        CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                    }
                }
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                customer = null;
                CustomerContact = null;
            }
            return CustomerContact;
        }

        /// <summary>
        /// Create a new contractor customer record, with details provided on the form.
        /// </summary>
        /// <returns></returns>
        private DOContractorCustomer CreateContractorCustomer(DOContact customer)
        {
            DOContractorCustomer contractorCustomer = null;
            try
            {
                if (customer != null)
                {
                    contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, customer.ContactID);
                    if (contractorCustomer == null)
                    {
                        contractorCustomer = new DOContractorCustomer
                        {
                            ContactCustomerId = Guid.NewGuid(),
                            CustomerID = customer.ContactID,
                            ContractorId = CurrentSessionContext.CurrentContact.ContactID,
                            Active = false,
                            Linked = DOContractorCustomer.LinkedEnum.AwaitingCust,
                            CustomerType = DOContractorCustomer.CustomerTypeEnum.Individual
                        };

                    }
                    else
                    {
                        contractorCustomer.Deleted = false;
                    }
                }
                else
                {
                    //If the customer is not found, create a new customer
                   customer= CreateContactForCustomer(txtCustomerEmail.Text);

                    //Create contractor-customer link for the new customer
                    contractorCustomer = CreateContractorCustomer(customer);
                }
                if (txtCustomerFirstName.Visible)
                {
                    if (txtCustomerFirstName.Text != null)
                    {
                        contractorCustomer.FirstName = txtCustomerFirstName.Text;
                    }
                    else
                        throw new FieldValidationException("Please provide first name");

                    if (txtCustomerLastName.Text != null)
                    {
                        contractorCustomer.LastName = txtCustomerLastName.Text;
                    }
                    else
                        throw new FieldValidationException("Please provide last name");
                }
                //jared 16/1/17
                //if (txtCustomerCompanyName.Visible)
                //{
                //    if (!string.IsNullOrEmpty(txtCustomerCompanyName.Text))
                //        contractorCustomer.CompanyName = txtCustomerCompanyName.Text;
                //}
                if (!string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
                {
                    contractorCustomer.Phone = txtCustomerPhone.Text;
                }
                else
                    throw new FieldValidationException("Please provide Phone number");
                if (!string.IsNullOrWhiteSpace(txtCustomerAddress1.Text))
                    contractorCustomer.Address1 = txtCustomerAddress1.Text;
                else
                {
                    throw new FieldValidationException("Please provide street address");
                }
                if (DDL_Add2.SelectedItem.ToString() != "-Select-")
                    contractorCustomer.Address2 = DDL_Add2.SelectedItem.ToString();
                else
                {
                    throw new FieldValidationException("Please select suburb");
                }
                contractorCustomer.Address3 = DDL_Address3.SelectedItem.ToString();
                contractorCustomer.Address4 = DDL_Address4.SelectedItem.ToString();
                CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            }
            catch (FieldValidationException exception)
            {
                ShowMessage(exception.Message);
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return contractorCustomer;
        }

        /// <summary>
        /// Create a contact by taking data from front end
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        private DOContact CreateContactForCustomer(string Email)
        {
            DOContact Contact;
            Contact = CurrentBRContact.SelectContactByUsername(Email);
            if (Contact == null)
            {
                Contact = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID,
                     DOContact.ContactTypeEnum.Individual);
                Contact.Email = Email;
                Contact.FirstName = txtCustomerFirstName.Text;
                Contact.LastName = txtCustomerLastName.Text;
                //Contact.CompanyName = txtCustomerCompanyName.Text; jared 16/1/17
                //CurrentBRContact.ValidateName(txtCustomerFirstName.Text, txtCustomerLastName.Text, txtCustomerCompanyName.Text); jared 16/1/17 this needs looking at. Had to comment due to removing txtcustomercompanyname.text)
                CurrentBRContact.ValidateName(txtCustomerFirstName.Text, txtCustomerLastName.Text, "");//jared 16/1/17
                Contact.Phone = txtCustomerPhone.Text;
                Contact.Address1 = txtCustomerAddress1.Text;
                if (DDL_Add2.SelectedItem.ToString() != "-Select-")
                    Contact.Address2 = DDL_Add2.SelectedItem.ToString();
                else
                {
                    throw new FieldValidationException("Please select suburb");
                }
                Contact.Address3 = DDL_Address3.SelectedItem.ToString();
                Contact.Address4 = DDL_Address4.SelectedItem.ToString();
                Contact.PendingUser = true;
                CurrentBRContact.SaveContact(Contact);
            }
            CurrentSessionContext.CurrentCustomer = Contact;
            return Contact;
        }

        /// <summary>
        /// Go back to home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// On load of regions, load districts and suburbs as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrictForCustomer();
            LoadSuburbsForCustomer();
        }

        /// <summary>
        /// Load suburbs on the basis of districts
        /// </summary>
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

        /// <summary>
        /// Load districts on the basis of regions
        /// </summary>
        public void LoadDistrictForCustomer()
        {
            List<DOBase> Districts = CurrentBRDistrict.SelectDistricts(Guid.Parse(DDL_Address4.SelectedValue));
            DDL_Address3.DataSource = Districts;
            DDL_Address3.DataTextField = "DistrictName";
            DDL_Address3.DataValueField = "DistrictID";
            DDL_Address3.DataBind();
        }

        /// <summary>
        /// On change of districts, load suburbs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DDL_Address3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbsForCustomer();
        }

        /// <summary>
        /// Load regions initially
        /// </summary>
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

        /// <summary>
        /// If customer already have suburb in the database, make it selected or fill '-Select-' in the dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DDL_Add2_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (CurrentSessionContext.CurrentCustomer != null)
                {
                    DDL_Add2.SelectedIndex =
                        DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText(CurrentSessionContext.CurrentCustomer.Address2));
                    if ((DDL_Add2.SelectedIndex == 0 &&
                         CurrentSessionContext.CurrentCustomer.Address2 != DDL_Add2.SelectedItem.ToString()) ||
                        DDL_Add2.SelectedItem == null)
                    {
                        DDL_Add2.Items.Insert(0, new ListItem("-Select-", ""));
                        foreach (ListItem item in DDL_Add2.Items)
                            item.Selected = false;
                        DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText("-Select-"));
                    }
                }
            }
            else
            {
                DDL_Add2.Items.Insert(0, new ListItem("-Select-", ""));
                foreach (ListItem item in DDL_Add2.Items)
                    item.Selected = false;
                DDL_Add2.SelectedIndex = DDL_Add2.Items.IndexOf(DDL_Add2.Items.FindByText("-Select-"));
            }

        }

        /// <summary>
        ///Default district to Christchurch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DDL_Address3_PreRender(object sender, EventArgs e)
        {
            if (DDL_Address3.SelectedIndex == 0 && !IsPostBack)
            {
                DDL_Address3.SelectedIndex = 2;
            }
            LoadSuburbsForCustomer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnYesExistingEmail_OnClick(object sender, EventArgs e)
        {
            Guid ContactID = new Guid(((Button)sender).CommandArgument);
            DOContact c = CurrentBRContact.SelectContact(ContactID);
            ExistingContact = c;
            CurrentSessionContext.CurrentCustomer = ExistingContact;
            InformCustomerByEmail();
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// Send email to customer
        /// </summary>
        private void InformCustomerByEmail()
        {
            string subject = "You have been invited to Ontrack";
            StringBuilder body = new StringBuilder();
            body.AppendFormat("<strong>{0} </strong>has added you ( {1} ) as a customer of theirs. As you will be aware " +
                              "this gives you many benefits including:-" +
                              "<ul><li>You can view the progress of any job that {0} is working on for you.</li>" +
                              "<li>You can keep track of all the jobs done on your site in the past.</li>" +
                              "<li>You can request other contractors through OnTrack to work on this job or any other jobs " +
                              "and track their progress too.</li>" +
                              "<li>You can create you own projects to work on.</li></ul>",
                              CurrentSessionContext.CurrentContact.DisplayName, CurrentSessionContext.CurrentCustomer.Email);
            body.AppendFormat("<br/><br/>  Please <a href='http://localhost:63323/'> click here </a>to be taken to<a href='http://localhost:63323/'> OnTrack</a> to log in and link to Electracraft Ltd. <br/><br/>");
            CurrentBRGeneral.SendConfirmationEmail(Constants.EmailSender, "jared@ecraft.co.nz", subject, body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveCompanyOnClick(object sender, EventArgs e)
        {
            CurrentSessionContext.CurrentCustomer = null;

            DOContact companyContact;
            companyContact = CreateCompanyContactForCustomer(txtCustomerEmail.Text);
            CurrentSessionContext.CurrentCustomer = companyContact;
            CreateContractorCustomer(CurrentSessionContext.CurrentCustomer);
            //Link the new company to the customer contact.
            DOContactCompany contactCompany = CurrentBRContact.CreateContactCompany(CurrentSessionContext.CurrentContact.ContactID, companyContact.ContactID,
                CurrentSessionContext.Owner.ContactID);
            CurrentBRContact.SaveContactCompany(contactCompany);
            string notify = ((Button)sender).CommandArgument;
            if (notify == "Yes")
            {
                InformCustomerByEmail();
            }
            Response.Redirect(Constants.URL_CustomerHome);
        }

        private DOContact CreateCompanyContactForCustomer(string email)
        {
            DOContact Contact = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID,
                   DOContact.ContactTypeEnum.Company);
            Contact.Email = email;
            if (companyName_txt.Text != "") Contact.CompanyName = companyName_txt.Text;
            else
            {
                throw new FieldValidationException("Please enter company name");
            }
            Contact.Phone = txtCustomerPhone.Text;
            Contact.Address1 = txtCustomerAddress1.Text;
            if (DDL_Add2.SelectedItem.ToString() != "-Select-")
                Contact.Address2 = DDL_Add2.SelectedItem.ToString();
            else
            {
                throw new FieldValidationException("Please select suburb");

            }

            Contact.Address3 = DDL_Address3.SelectedItem.ToString();
            Contact.Address4 = DDL_Address4.SelectedItem.ToString();
            Contact.PendingUser = true;
            try
            {
                CurrentBRContact.SaveContact(Contact);
            }
            catch (Exception exception)
            {
                throw new Exception("Saving company was not successfull");
            }
            return Contact;
        }

        /// <summary>
        /// Get the customer's details from Contractor (or whoever is adding customer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            //Find the existing customer with provided email id
            List<DOBase> contactsList = CurrentBRContact.SelectContactsByEmail(txtCustomerEmail.Text);
            foreach (DOContact contact in contactsList)
            {
                if (contact.ContactType == DOContact.ContactTypeEnum.Individual)
                    CurrentSessionContext.CurrentCustomer = contact;
            }

            //Create a contractor customer record
            DOContractorCustomer contractorCustomer = CreateContractorCustomer(CurrentSessionContext.CurrentCustomer);
            if (contractorCustomer == null)
                return ;
            //Create a contact company record for individual
            Guid contactCompanyID =
                CurrentBRContact.SelectContactCompany(CurrentSessionContext.CurrentCustomer.ContactID,
                    CurrentSessionContext.CurrentCustomer.ContactID);
            if (contactCompanyID == Guid.Empty)
                SaveContactCompany(CurrentSessionContext.CurrentCustomer.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);

            pnlCustomerDetails.Visible = false;
            if (contractorCustomer != null && string.IsNullOrEmpty(contractorCustomer.CompanyName))
            {
                AskIfCompanyOfCustomer.Visible = true;
            }
            else
            {
                IsOwner_pnl.Visible = true;
                if (contractorCustomer != null) companyName_txt.Text = contractorCustomer.CompanyName;
                companyName_pnl.Visible = true;
            }

            SaveCancelPnl.Visible = false;
        }

        /// <summary>
        /// If company needs to be created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CompanyYes_OnClick(object sender, EventArgs e)
        {
            IsOwner_pnl.Visible = true;
            AskIfCompanyOfCustomer.Visible = false;
            companyName_pnl.Visible = true;
        }

        /// <summary>
        /// If no company with the customer then send the customer email notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CompanyNo_OnClick(object sender, EventArgs e)
        {
            InformCustomerByEmail();
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// If the customer is owner of the company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Owner_btn_OnClick(object sender, EventArgs e)
        {
            Guid managerID = Guid.Empty;
            CreateCompanyContactForCustomer(CurrentSessionContext.CurrentCustomer.ContactID);
            contactCompany.PendingUser = true;
            DOContact individual = CurrentBRContact.SelectContactByUsername(CurrentSessionContext.CurrentCustomer.Email);
            SaveContactCompany(contactCompany.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
            SaveContactCompany(contactCompany.ContactID, individual.ContactID);
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID) ??
                                     CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID, Guid.Empty);
            contractorCustomer = SetContractorCustomer(contractorCustomer, contactCompany);
            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            InformCustomerByEmail();
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// Create the contact for company using details from the customer
        /// </summary>
        /// <param name="contact"></param>
        private void CreateCompanyContactForCustomer(Guid managerID)
        {
            CurrentSessionContext.CurrentCustomer.ContactType = DOContact.ContactTypeEnum.Company;
            CurrentSessionContext.CurrentCustomer.ContactID = Guid.NewGuid();
            CurrentSessionContext.CurrentCustomer.UserName = CurrentSessionContext.CurrentCustomer.ContactID.ToString();
            CurrentSessionContext.CurrentCustomer.PersistenceStatus = ObjectPersistenceStatus.New;
            CurrentSessionContext.CurrentCustomer.CompanyName = companyName_txt.Text;
            CurrentSessionContext.CurrentCustomer.ManagerID = managerID;
            companyName_pnl.Visible = true;
            CurrentBRContact.SaveContact(CurrentSessionContext.CurrentCustomer);
            contactCompany = CurrentSessionContext.CurrentCustomer;

        }

        /// <summary>
        /// If the customer is employee of the company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Emp_btn_OnClick(object sender, EventArgs e)
        {
           
            pnl_CompanyOwnerDetails.Visible = true;
            IsOwner_pnl.Visible = false;
            Guid managerID = Guid.Empty;
            CreateCompanyContactForCustomer(managerID);
            contactCompany.PendingUser = true;
            DOContact individual = CurrentBRContact.SelectContactByUsername(CurrentSessionContext.CurrentCustomer.Email);
            SaveContactCompany(contactCompany.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
            SaveContactCompany(contactCompany.ContactID, individual.ContactID);
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID) ??
                                     CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID, Guid.Empty);//added guid.empty 2017.4.25 needs testing
            contractorCustomer = SetContractorCustomer(contractorCustomer, contactCompany);
            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            InformCustomerByEmail();
        }

        /// <summary>
        /// Create new company for the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void saveCompany_btn_OnClick(object sender, EventArgs e)
        {
            contactCompany.CompanyName = companyName_txt.Text;
            contactCompany.ManagerID = CurrentSessionContext.CurrentCustomer.ContactID;
            contactCompany.PendingUser = true;
            CurrentBRContact.SaveContact(contactCompany);
            SaveContactCompany(contactCompany.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);

            CurrentSessionContext.CurrentCustomer = contactCompany;
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID) ??
                                     CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, contactCompany.ContactID, Guid.Empty);//added guid.empty 2017.4.25 needs testing
            contractorCustomer = SetContractorCustomer(contractorCustomer, contactCompany);


            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            InformCustomerByEmail();
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// Create company contact record for the employee or owner of the company
        /// </summary>
        /// <param name="contactID1"></param>
        /// <param name="contactID2"></param>
        private void SaveContactCompany(Guid companyId, Guid contactId)
        {
            DOContactCompany contactCompany = new DOContactCompany();
            contactCompany.ContactID = contactId;
            contactCompany.CompanyID = companyId;
            contactCompany.ContactCompanyID = Guid.NewGuid();
            contactCompany.Pending = false;
            CurrentBRContact.SaveContactCompany(contactCompany);
        }

        /// <summary>
        /// Creat contractorcustomer record for the company and contractor
        /// </summary>
        /// <param name="contractorCustomer"></param>
        /// <param name="contactCompany"></param>
        /// <returns></returns>
        private DOContractorCustomer SetContractorCustomer(DOContractorCustomer contractorCustomer, DOContact contactCompany)
        {
            contractorCustomer.Active = false;
            contractorCustomer.Address1 = contactCompany.Address1;
            contractorCustomer.Address2 = contactCompany.Address2;
            contractorCustomer.Address3 = contactCompany.Address3;
            contractorCustomer.Address4 = contactCompany.Address4;
            contractorCustomer.CompanyName = contactCompany.CompanyName;
            contractorCustomer.Phone = contactCompany.Phone;
            contractorCustomer.Linked = DOContractorCustomer.LinkedEnum.AwaitingCust;
            contractorCustomer.CustomerType = DOContractorCustomer.CustomerTypeEnum.Company;
            return contractorCustomer;
        }

        /// <summary>
        /// Empoyee has company owner's details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OwnerYes_OnClick(object sender, EventArgs e)
        {
            pnl_CompanyOwnerEmailID.Visible = true;
            pnl_CompanyOwnerDetails.Visible = false;
        }

        /// <summary>
        /// Get emailId to find out if a record exists in our database of not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitEmailID_OnClick(object sender, EventArgs e)
        {
            FindMatchingContacts(txt_emailId.Text);
            if (CustomersLists.Count >= 0)
            {
                pnlCustomerDetails.Visible = true;
                pnl_CompanyOwnerEmailID.Visible = false;
                pnl_CompanyOwnerDetails.Visible = false;
                companyName_pnl.Visible = false;
                SetCustomerFormToEmpty();
                SetFormEmpty();
            }
        }

        /// <summary>
        /// Set form empty to fill in company owner's details
        /// </summary>
        protected void SetFormEmpty()
        {
            txtCustomerFirstName.Text = "";
            txtCustomerLastName.Text = "";
            txtCustomerEmail.Text = txt_emailId.Text;
            //txtCustomerCompanyName.Text = companyName_txt.Text; jared 16/1/17
            pnl_lbl_forCustomerOrOwner.Text = "Owner's details";
            pnl_CompanyOwnerSave.Visible = true;
        }

        protected void btn_CompanyOwner_OnClick(object sender, EventArgs e)
        {
         
           DOContact contact= CreateCompanyContactForCustomer(txtCustomerEmail.Text);
            CreateContractorCustomer(contact);
            Response.Redirect(Constants.URL_CustomerHome);
        }

        //Create company even if the employee doesn't have company owner's details
        protected void btn_OwnerNo_OnClick(object sender, EventArgs e)
        {
            try
            {
                contactCompany = CreateCompanyContactForCustomer(CurrentSessionContext.CurrentCustomer.Email);
                //Create individual-company link in the contactcompany table
                SaveContactCompany(contactCompany.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
                CurrentSessionContext.CurrentCustomer = contactCompany;
                Response.Redirect(Constants.URL_CustomerHome);
            }
            catch (FieldValidationException fieldValidationException)
            {
                ShowMessage(fieldValidationException.Message);
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message);
            }

        }
    }
}