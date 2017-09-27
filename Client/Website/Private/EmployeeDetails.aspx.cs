using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class EmployeeDetails : PageBase
    {
        protected DOEmployeeInfo Employee;
        protected bool ShowEmployee = false;
        public DOContact Company = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect("~/private/employeeinfo.aspx",false);
            Company = CurrentSessionContext.CurrentContact;
            try
            {
                Guid ContactCompanyID = new Guid(Request.QueryString["ccid"]);
                DOContactCompany cc = CurrentBRContact.SelectContactCompany(ContactCompanyID);
                Employee = CurrentBRContact.SelectEmployeeInfo(cc.ContactID, cc.CompanyID);
                if (Employee == null)
                    Employee = CreateEmployee(cc.ContactID);
                ShowEmployee = CheckEmployeePageStatus(CompanyPageFlag.EmployeeDetails);
            }
            catch
            {
                ShowMessage("Invalid employee contact ID.", MessageType.Error);
            }
            
        }

        private DOEmployeeInfo CreateEmployee(Guid ContactID)
        {
			Guid contactCompanyID= CurrentBRContact.SelectContactCompany(ContactID, Company.ContactID);
	        DOContactCompany cc = CurrentBRContact.SelectContactCompany(contactCompanyID);
            DOEmployeeInfo Employee = CurrentBRContact.CreateEmployeeInfo(cc.ContactCompanyID, CurrentSessionContext.Owner.ContactID);
            DOContact Contact = CurrentBRContact.SelectContact(ContactID);
            if (Contact == null)
                throw new Exception();
            Employee.FirstName = Contact.FirstName;
            Employee.LastName = Contact.LastName;
            Employee.Phone = Contact.Phone;
            Employee.Email = Contact.Email;
            Employee.Address1 = Contact.Address1;
            Employee.Address2 = Contact.Address2;

            CurrentBRContact.SaveEmployeeInfo(Employee);
            return Employee;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!ShowEmployee) return;
            if (!IsPostBack)
            {
                txtAddress1.Text = Employee.Address1;
                txtAddress2.Text = Employee.Address2;
                txtEmail.Text = Employee.Email;
                txtFirstName.Text = Employee.FirstName;
                txtLastName.Text = Employee.LastName;
                txtPhone.Text = Employee.Phone;
                txtPayRate.Text = Employee.PayRate.ToString();
                txtLabourRate.Text = Employee.LabourRate.ToString();

                chkEmployeeDetails.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.EmployeeDetails) > 0;
                chkEmployeeInfo.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.ShowEmployeeInfo) > 0;
                chkTimeSheet.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.TimeSheet) > 0;
                chkTradeCategories.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.TradeCategories) > 0;
                chkDeleteMaterials.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.DeleteMaterialsFromVehicle) > 0;
                chkMoveMaterials.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.MoveMaterialsFromOtherVehicle) > 0;
                chkViewImportButtons.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.ImportInvoices) > 0;
                chkMaterialsManually.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.AddMaterialsManuallyToVehicle) > 0;
                chkShowInvoices.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.ViewInvoices) > 0;
                chkCanDeleteInvoices.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.DeleteInvoices) > 0;
               //Tony Added
                chkShareSite.Checked = (Employee.AccessFlags & (int) CompanyPageFlag.ShareSiteToAnotherCustomer) > 0;
                chkMoveJob.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.MoveJobToAnotherSite) > 0;
                chkMoveTask.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.MoveTaskToAnotherJob) > 0;
                //Jared added 30.1.17
                chkVehicles.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.AddAndEditVehicles) > 0;
                chkAccounts.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.AccountsScreen) > 0;
                chkJobTemplates.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.CreateJobTemplates) > 0;
                chkPromoteBusiness.Checked = (Employee.AccessFlags & (int)CompanyPageFlag.PromoteBusinessScreenAndAddons) > 0;



            }            
            DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/private/employeeinfo.aspx",false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Employee.Address1 = txtAddress1.Text;
                Employee.Address2 = txtAddress2.Text;
                Employee.Email = txtEmail.Text;
                Employee.FirstName = txtFirstName.Text;
                Employee.LastName = txtLastName.Text;
                Employee.Phone = txtPhone.Text;
                Employee.PayRate = decimal.Parse(txtPayRate.Text);
                Employee.LabourRate = decimal.Parse(txtLabourRate.Text);

                int AccessFlags =
                    (chkEmployeeDetails.Checked ? (int)CompanyPageFlag.EmployeeDetails : 0) |
                    (chkEmployeeInfo.Checked ? (int)CompanyPageFlag.ShowEmployeeInfo : 0) |
                    (chkTimeSheet.Checked ? (int)CompanyPageFlag.TimeSheet : 0) |
                    (chkTradeCategories.Checked ? (int)CompanyPageFlag.TradeCategories : 0) |
                    (chkDeleteMaterials.Checked ? (int)CompanyPageFlag.DeleteMaterialsFromVehicle : 0) |
                    (chkMoveMaterials.Checked ? (int)CompanyPageFlag.MoveMaterialsFromOtherVehicle : 0) |
                    (chkMaterialsManually.Checked ? (int)CompanyPageFlag.AddMaterialsManuallyToVehicle : 0) |
                    (chkShowInvoices.Checked ? (int)CompanyPageFlag.ViewInvoices : 0) |
                    (chkCanDeleteInvoices.Checked ? (int)CompanyPageFlag.DeleteInvoices : 0) |
                    (chkViewImportButtons.Checked ? (int)CompanyPageFlag.ImportInvoices : 0) |
                    //Tony added
                    (chkShareSite.Checked ? (int)CompanyPageFlag.ShareSiteToAnotherCustomer : 0) |
                    (chkMoveJob.Checked ? (int)CompanyPageFlag.MoveJobToAnotherSite : 0) |
                    (chkMoveTask.Checked ? (int)CompanyPageFlag.MoveTaskToAnotherJob : 0) |
                    //jared 30.1.17
                    (chkVehicles.Checked ? (int)CompanyPageFlag.AddAndEditVehicles : 0) |
                    (chkAccounts.Checked ? (int)CompanyPageFlag.AccountsScreen : 0) |
                    (chkJobTemplates.Checked ? (int)CompanyPageFlag.CreateJobTemplates : 0) |
                    (chkPromoteBusiness.Checked ? (int)CompanyPageFlag.PromoteBusinessScreenAndAddons : 0);


            Employee.AccessFlags = AccessFlags;

                CurrentBRContact.SaveEmployeeInfo(Employee);
                ShowMessage("Saved successfully.", MessageType.Info);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

    }
}