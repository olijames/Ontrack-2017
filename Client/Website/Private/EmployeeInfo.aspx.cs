using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using System.Net.Mail;
using System.Net;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class EmployeeInfo : PageBase
    {
        bool ShowEmployees = false;
        public DOContact Company = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            CheckCompanyQuerystring();

            //Must have a contact for this page.
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
            Company = CurrentSessionContext.CurrentContact;

            ShowEmployees = CheckEmployeePageStatus(CompanyPageFlag.ShowEmployeeInfo);
            //EnsureEmployees();
            EnsureIndividualIsEmployee();
            EnsureCompanyOwnerIsEmployee();
        }
        private void EnsureIndividualIsEmployee()
        {
            DOEmployeeInfo doe = CurrentBRContact.SelectEmployeeInfo(Company.ContactID, Company.ContactID);

        }

        private void EnsureCompanyOwnerIsEmployee()
        {


        }

        private void CheckCompanyQuerystring()
        {
            //Check contact from querystring.
            string strCompanyID = Request.QueryString["company"];
            if (!string.IsNullOrEmpty(strCompanyID))
            {
                try
                {
                    Guid CompanyContactID = new Guid(strCompanyID);
                    DOContact CompanyContact = CurrentBRContact.SelectContact(CompanyContactID);
                    if (CompanyContact.ContactID == CurrentSessionContext.Owner.ContactID || CurrentBRContact.CheckCompanyContact(CompanyContact.ContactID, CurrentSessionContext.Owner.ContactID))
                    {
                        CurrentSessionContext.CurrentContact = CompanyContact;
                    }
                }
                catch { }
            }
        }

        private void EnsureEmployees()
        {
            if (Company.ContactType != DOContact.ContactTypeEnum.Company) return;
            List<DOBase> NoInfo = CurrentBRContact.SelectCompanyEmployeesWithoutInfo(Company.ContactID);
            foreach (DOContact c in NoInfo)
            {
                //Create employee info for this contact.
                Guid contactCompanyID = CurrentBRContact.SelectContactCompany(c.ContactID, Company.ContactID);
                if (contactCompanyID == Guid.Empty) continue;

                DOEmployeeInfo ei = CurrentBRContact.CreateEmployeeInfo(contactCompanyID, CurrentSessionContext.Owner.ContactID);
                ei.Address1 = string.Empty;
                ei.Address2 = string.Empty;
                ei.Email = c.Email;
                ei.FirstName = c.FirstName;
                ei.LastName = c.LastName;
                ei.Phone = string.Empty;
                //ei.ContactID = c.ContactID;
                CurrentBRContact.SaveEmployeeInfo(ei);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            phPage.Visible = true;
            List<DOBase> Employees = CurrentBRContact.SelectCompanyEmployees(Company.ContactID, false);

            var ActiveEmployees = from DOContactEmployee ae in Employees where ae.ContactCompanyActive && !ae.ContactCompanyPending select ae;
            var OtherEmployees = from DOContactEmployee oe in Employees where !oe.ContactCompanyActive || oe.ContactCompanyPending orderby oe.ContactCompanyPending descending select oe;

            gvEmployees.DataSource = ActiveEmployees;
            gvEmployees.DataBind();
            //jared 31.1.17 start
            if (CheckEmployeePageStatus(CompanyPageFlag.EmployeeDetails))
            //if (CurrentSessionContext.CurrentContact.ManagerID == CurrentSessionContext.Owner.ContactID)
            //jared 31.1.17 end
            {
                gvEmployeesNotActive.DataSource = OtherEmployees;
                gvEmployeesNotActive.DataBind();
                
            }
            else
            {
                gvEmployeesNotActive.Visible = false;
                //Jared added 30.1.2017
                gvEmployees.Visible = false;
                
            }
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid ContactCompanyID = new Guid(b.CommandArgument.ToString());
            DOContactCompany cc = CurrentBRContact.SelectContactCompany(ContactCompanyID);

            if (cc.ContactID == Company.ManagerID)
            {
                ShowMessage("You cannot remove the manager from the company.", MessageType.Error);
                return;
            }
            else
            {
                cc.Active = false;
                cc.Pending = false;
                CurrentBRContact.SaveContactCompany(cc);
                //                CurrentBRContact.DeleteContactCompany(cc);
                ShowMessage("The contact was removed.", MessageType.Info);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid ContactCompanyID = new Guid(b.CommandArgument.ToString());
            DOContactCompany cc = CurrentBRContact.SelectContactCompany(ContactCompanyID);

            cc.Active = true;
            cc.Pending = false;
            CurrentBRContact.SaveContactCompany(cc);
            ShowMessage("The contact was approved.", MessageType.Info);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Guid ContactCompanyID = new Guid(b.CommandArgument.ToString());
            Response.Redirect("~/private/employeedetails.aspx?ccid=" + ContactCompanyID.ToString());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            DOContact employee;
            DOContactCompany cc;
            DOEmployeeInfo ei;
            try
            {
                //Save employee contact
                employee = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, DOContact.ContactTypeEnum.Individual);
                RE1.SaveForm(employee);
                //Check if existing contact for email.
                DOContact CheckContact = CurrentBRContact.SelectContactByUsername(employee.UserName);
                if (CheckContact == null)
                {
                    CurrentBRContact.SaveContact(employee);
                    //Link to company
                    cc = CurrentBRContact.CreateContactCompany(employee.ContactID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);
                    CurrentBRContact.SaveContactCompany(cc);
                }
                else
                {
                    //Link to company
                    cc = CurrentBRContact.CreateContactCompany(CheckContact.ContactID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);
                    CurrentBRContact.SaveContactCompany(cc);
                }


                //Create employee info.
                ei = CurrentBRContact.CreateEmployeeInfo(cc.ContactCompanyID, CurrentSessionContext.Owner.ContactID);
                ei.Address1 = employee.Address1;
                ei.Address2 = employee.Address2;
                ei.Email = employee.Email;
                ei.FirstName = employee.FirstName;
                ei.LastName = employee.LastName;
                ei.Phone = employee.Phone;
                //  ei.ContactID = employee.ContactID;
                CurrentBRContact.SaveEmployeeInfo(ei);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void gvEmployees_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //jared 31.1.17 start
            if (!CheckEmployeePageStatus(CompanyPageFlag.EmployeeDetails))
            //if (CurrentSessionContext.CurrentContact.ManagerID != CurrentSessionContext.Owner.ContactID)
            //jared 31.1.17 end
            {
                foreach (GridViewRow gvr in gvEmployees.Rows)
                {
                    Button btnEdit = gvr.FindControl("btnEdit") as Button;
                    Button btnRemove = gvr.FindControl("btnRemove") as Button;
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                }
            }
        }
    }
}