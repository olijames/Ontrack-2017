using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using System.ComponentModel.Design;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;
namespace Electracraft.Client.Website
{
    public partial class RegisterIndividual : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Logout();
                CurrentSessionContext.RegisterCompany = null;
                CurrentSessionContext.RegisterContact = null;
            }
        }


        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (CurrentSessionContext.RegisterContact != null)
            {
                Response.Redirect(Constants.URL_RegisterCompany);
            }
            try
            {
                //Save the individual.
                DOContact Contact = CurrentBRContact.CreateContact(Guid.Empty, DOContact.ContactTypeEnum.Individual);
                Contact.Active = false;
                rgIndividual.SaveForm(Contact);
                CurrentBRContact.SaveContact(Contact);

                //2017.2.15 Jared create employee for this individual.
                DOContactCompany ContactCompany = CurrentBRContact.CreateContactCompany(Contact.ContactID, Contact.ContactID, Contact.ContactID);
                CurrentBRContact.SaveContactCompany(ContactCompany);
                DOEmployeeInfo Employee = CurrentBRContact.CreateEmployeeInfo(ContactCompany.ContactCompanyID, Contact.ContactID);
                Employee.Address1 = Contact.Address1;
                Employee.Address2 = Contact.Address2;
                Employee.Address3 = Contact.Address3;
                Employee.Address4 = Contact.Address4;
                Employee.FirstName = Contact.FirstName;
                Employee.LastName = Contact.LastName;
                Employee.Email = Contact.Email;
                Employee.Phone = Contact.Phone;
                CurrentBRContact.SaveEmployeeInfo(Employee);
                //jared end of above

                ////Log the contact in.
                //if (CurrentSessionContext.Owner == null)
                //{
                //    Login(Contact.ContactID, Contact.PasswordHash, false);
                //}
                CurrentSessionContext.RegisterContact = Contact;

                //2017.2.15 Jared
                //Go to company registration step.
                
                //Response.Redirect(Constants.URL_RegisterCompany);
                Response.Redirect("~/RegistrationComplete.aspx", false);
                //end of block jared

            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}