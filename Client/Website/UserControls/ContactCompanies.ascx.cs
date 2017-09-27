using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.UserControls
{
    public partial class ContactCompanies : UserControlBase
    {
        protected DOContact _Contact = null;
        public DOContact Contact
        {
            get
            {
                if (_Contact == null)
                    return ParentPage.CurrentSessionContext.Owner;
                else
                    return _Contact;
            }
            set
            {
                _Contact = value;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<DOBase> Companies = ParentPage.CurrentBRContact.SelectContactCompanies(Contact.ContactID);
            if (Companies.Count == 0)
            {
                litNoCompanies.Visible = true;
                rpContactCompanies.Visible = false;
            }
            else
            {
                litNoCompanies.Visible = false;
                rpContactCompanies.Visible = true;
                rpContactCompanies.DataSource = Companies;
                rpContactCompanies.DataBind();
            }
            
        }

        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            DOContact NewCompany = ParentPage.CurrentBRContact.CreateContact(Contact.ContactID, DOContact.ContactTypeEnum.Company);
            try
            {
                rgCompany.SaveForm(NewCompany);
                NewCompany.UserName = Guid.NewGuid().ToString();
                NewCompany.ManagerID = Contact.ContactID;
                NewCompany.CompanyKey = ParentPage.CurrentBRContact.GenerateCompanyKey();
                ParentPage.CurrentBRContact.SaveContact(NewCompany);

                DOContactCompany ContactCompany = ParentPage.CurrentBRContact.CreateContactCompany(Contact.ContactID, NewCompany.ContactID, Contact.ContactID);
                ParentPage.CurrentBRContact.SaveContactCompany(ContactCompany);
               
                //2017.2.15 Jared create employee for this company. This will be the manager
                DOEmployeeInfo Employee = ParentPage.CurrentBRContact.CreateEmployeeInfo(ContactCompany.ContactCompanyID, Contact.ContactID);
                Employee.Address1 = Contact.Address1;
                Employee.Address2 = Contact.Address2;
                Employee.Address3 = Contact.Address3;
                Employee.Address4 = Contact.Address4;
                Employee.FirstName = Contact.FirstName;
                Employee.LastName = Contact.LastName;
                Employee.Email = Contact.Email;
                Employee.Phone = Contact.Phone;
                ParentPage.CurrentBRContact.SaveEmployeeInfo(Employee);
                //jared end of block
            }
            catch (FieldValidationException ex)
            {
                ParentPage.ShowMessage(ex.Message, PageBase.MessageType.Error);
            }
        }

        protected void btnLinkCompany_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCompanyKey.Text))
            {
                ParentPage.ShowMessage("Please enter a company key.", PageBase.MessageType.Error);
                return;
            }

            DOContact Company = ParentPage.CurrentBRContact.SelectContactByCompanyKey(txtCompanyKey.Text);
            if (Company == null)
            {
                ParentPage.ShowMessage("The company key " + txtCompanyKey.Text + " is not valid.", PageBase.MessageType.Error);
                return;
            }
			Guid contactCompanyID= ParentPage.CurrentBRContact.SelectContactCompany(Contact.ContactID, Company.ContactID);
	        DOContactCompany CheckCC = ParentPage.CurrentBRContact.SelectContactCompany(contactCompanyID);
			if (CheckCC != null)
            {
                if (!CheckCC.Active)
                {
                    ParentPage.ShowMessage("You have been declined from joining the requested company.", PageBase.MessageType.Error);
                }
                else if (CheckCC.Pending)
                {
                    ParentPage.ShowMessage("Your request to join " + Company.CompanyName + "is still pending.", PageBase.MessageType.Error);
                }
                else
                {
                    ParentPage.ShowMessage("You are already linked to company " + Company.CompanyName + ".", PageBase.MessageType.Error);
                }
                return;
            }
            DOContactCompany ContactCompany = ParentPage.CurrentBRContact.CreateContactCompany(Contact.ContactID, Company.ContactID, ParentPage.CurrentSessionContext.Owner.ContactID);
            ContactCompany.Pending = true;
            ParentPage.CurrentBRContact.SaveContactCompany(ContactCompany);
            ParentPage.CurrentBRGeneral.SendContactCompanyPendingEmail(Company, Contact);
        }


        protected void btnView_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b.CommandName == "View")
            {
                Response.Redirect(Constants.URL_CompanyDetails + "?id=" + b.CommandArgument.ToString());
            }
        }

        protected void btnRemoveLink_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b.CommandName == "RemoveLink")
                RemoveContactCompany(new Guid(b.CommandArgument.ToString()));
        }

        private void RemoveContactCompany(Guid companyId)
        {
            DOContact company = ParentPage.CurrentBRContact.SelectContact(companyId);
            if (Contact.ContactID == company.ManagerID)
            {
                ParentPage.CurrentSessionContext.CurrentContact = company;
                Response.Redirect("~/private/CompanyClose.aspx", true);
                return;
            }
			Guid contactCompanyID= ParentPage.CurrentBRContact.SelectContactCompany(Contact.ContactID, companyId);
	        DOContactCompany cc = ParentPage.CurrentBRContact.SelectContactCompany(contactCompanyID);
			if (cc != null)
            {
                //jared 2017.4.24
                //DOEmployeeInfo ei = ParentPage.CurrentBRContact.SelectEmployeeInfo(cc.ContactID, companyId);
                //if (ei != null)
                //{
                //    ParentPage.CurrentBRContact.DeleteEmployeeeInfo(ei);
                //}
                //ParentPage.CurrentBRContact.DeleteContactCompany(cc);
                

                //ParentPage.CurrentBRContact.UpdateContactCustomer
                if (cc.ContactID == company.ManagerID)
                {
                    ParentPage.ShowMessage("You cannot remove the manager from the company.", PageBase.MessageType.Error);
                    return;
                }
                else
                {
                    cc.Active = false;
                    cc.Pending = false;
                    ParentPage.CurrentBRContact.SaveContactCompany(cc);
                    //                CurrentBRContact.DeleteContactCompany(cc);
                    ParentPage.ShowMessage("The contact was removed.", PageBase.MessageType.Info);
                }
                //eob jared
            }
        }

    }
}