using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class CompanyDetails : PageBase
    {
        protected DOContact Company;
        protected bool ManagerOrAdmin
        {
            get
            {
                return (CurrentBRContact.IsAdmin(CurrentSessionContext.Owner) || Company.ManagerID == CurrentSessionContext.Owner.ContactID);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            GetCompany();
            //User must be admin, or assigned to this company.
            if (!CurrentBRContact.IsAdmin(CurrentSessionContext.Owner))
            {
                if (Company != null)
                {
                    Guid contactCompanyID = CurrentBRContact.SelectContactCompany(CurrentSessionContext.Owner.ContactID, Company.ContactID);
                    if (contactCompanyID==Guid.Empty)
                    {
                        Response.Redirect("~/Private/Settings.aspx",false);
                    }
                }
            }
            udCompany.Contact = Company;
            udCompany.AdminMode = CurrentBRContact.IsAdmin(CurrentSessionContext.Owner);
        }

        private void GetCompany()
        {
            try
            {
                Guid CompanyID = new Guid(Request.QueryString["id"]);
                Company = CurrentBRContact.SelectContact(CompanyID);
                if (Company.ContactType != DOContact.ContactTypeEnum.Company) throw new Exception();
            }
            catch
            {
                Response.Redirect("~/Private/Settings.aspx",false);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            trManager1.Visible = ManagerOrAdmin;
            trManager2.Visible = ManagerOrAdmin;
            litCompanyName.Text = Company.CompanyName;
            udCompany.LoadForm();
            if (!ManagerOrAdmin)
                btnSaveContact.Visible = false;
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                udCompany.SaveForm();
                if (!string.IsNullOrEmpty(txtNewManager.Text))
                {
                    DOContact NewManager = CurrentBRContact.SelectContactByUsername(txtNewManager.Text.Trim());
                    if (NewManager == null)
                        throw new FieldValidationException("There is no user with the email " + txtNewManager.Text.Trim());
                    Guid contactCompanyID = CurrentBRContact.SelectContactCompany(NewManager.ContactID, Company.ContactID);
                    if (contactCompanyID == null)
                        throw new FieldValidationException(NewManager.UserName + " is not linked to this company.");
                    Company.ManagerID = NewManager.ContactID;
                }
                CurrentBRContact.SaveContact(Company);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

    }
}