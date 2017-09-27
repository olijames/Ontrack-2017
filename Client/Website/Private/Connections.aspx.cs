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
    public partial class Connections : PageBase
    {
        //2017.2.15 Jared, The purpose of this page is to give the company/individual an overview of who has access to them

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadContractors();
            LoadEmployees();
            LoadCustomers();
        }
        protected void LoadCustomers()
        {
            List<DOBase> MyList = CurrentBRContact.SelectAllCustomers(CurrentSessionContext.CurrentContact.ContactID);
            gvCustomers.DataSource = MyList;
            gvCustomers.DataBind();
        }
        protected void LoadContractors()
        {
            List<DOBase> MyList = CurrentBRContact.SelectContractors(CurrentSessionContext.CurrentContact.ContactID);
            gvContractors.DataSource = MyList;
            gvContractors.DataBind();
        }
        protected void LoadEmployees()
        {
            List<DOBase> MyList = CurrentBRContact.SelectCompanyEmployees (CurrentSessionContext.CurrentContact.ContactID, false);
            var ActiveEmployees = from DOContactEmployee ae in MyList where ae.ContactCompanyActive && !ae.ContactCompanyPending select ae;
            gvEmployees.DataSource = ActiveEmployees;
            gvEmployees.DataBind();
        }
    }
}