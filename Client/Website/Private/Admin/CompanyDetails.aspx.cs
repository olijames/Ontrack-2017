using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private.Admin
{
    [AdminPage]
    public partial class CompanyDetails : PageBase
    {
        const string NewUser = "New Company";
        const string EditUser = "Edit Company - {0}";

        public DOContact Contact;
        public DOContact.ContactTypeEnum ContactType = DOContact.ContactTypeEnum.Company;
        public DOContact CurrentManager;

        protected void Page_Init(object sender, EventArgs e)
        {
            GetContact();
            udCompany.AdminMode = CurrentBRContact.IsAdmin(CurrentSessionContext.Owner);
            
        }

        private void GetContact()
        {
            string strContactID = Request.QueryString["contactid"];
            if (string.IsNullOrEmpty(strContactID))
            {
                //No creating new companies here.
                //Companies must be linked to users.
                Response.Redirect("~/private/admin/manageusers.aspx",false);

                //heading.InnerText = NewUser;
                //Contact = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, ContactType);
            }
            else
            {
                Contact = CurrentBRContact.SelectContact(new Guid(strContactID));
                heading.InnerText = string.Format(EditUser, Contact.UserName);
            }

            udCompany.Contact = Contact;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            udCompany.LoadForm();
            btnActivate.Visible = !Contact.Active;
            btnDeactivate.Visible = Contact.Active;
            CurrentManager = CurrentBRContact.SelectContact(Contact.ManagerID);
            litCompanyManager.Text = CurrentManager.DisplayName;

            List<DOBase> AllContacts = CurrentBRContact.SearchContacts((int)DOContact.ContactTypeEnum.Individual, null, string.Empty, true);

            ddlNewManager.Items.Clear();
            ddlNewManager.Items.Add(new ListItem("Existing Manager", Constants.Guid_DefaultUser.ToString()));
            foreach (DOContact c in AllContacts)
            {
                ListItem li = new ListItem();
                li.Value = c.ContactID.ToString();
                li.Text = c.DisplayName + " (" + c.UserName + ")";
                ddlNewManager.Items.Add(li);
            }
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                bool NewContact = Contact.PersistenceStatus == ObjectPersistenceStatus.New;
                udCompany.SaveForm();
                //2017.2.15 Jared
                if (NewContact && Contact.ContactType == DOContact.ContactTypeEnum.Company || Contact.CompanyKey=="")
                    Contact.CompanyKey = CurrentBRContact.GenerateCompanyKey();
                //if (NewContact && Contact.ContactType == DOContact.ContactTypeEnum.Company)
                //    Contact.CompanyKey = CurrentBRContact.GenerateCompanyKey();



                Guid NewManagerID = new Guid(Request.Form[ddlNewManager.UniqueID]);
                if (NewManagerID != Constants.Guid_DefaultUser)
                {
                    Contact.ManagerID = NewManagerID;
                }
                
                CurrentBRContact.SaveContact(Contact);
                CurrentBRContact.SaveContactCompany(CurrentBRContact.CreateContactCompany(Contact.ManagerID, Contact.ContactID, CurrentSessionContext.Owner.ContactID));
                
                //here probably need contactcompany entry

                if (NewContact)
                {
                    Response.Redirect("CompanyDetails.aspx?contactid=" + Contact.ContactID.ToString());
                }
                else
                {
                    ShowMessage("Saved successfully.", MessageType.Info);
                }
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            Contact.Active = true;
            CurrentBRContact.SaveContact(Contact);
            ShowMessage("The user has been activated", MessageType.Info);
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            Contact.Active = false;
            CurrentBRContact.SaveContact(Contact);
            ShowMessage("The user has been deactivated", MessageType.Info);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentBRContact.DeleteContactComplete(Contact);
                Response.Redirect("~/Private/Admin/ManageUsers.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("The contact could not be deleted.<br />" + ex.Message, MessageType.Error);
            }
        }

    }
}