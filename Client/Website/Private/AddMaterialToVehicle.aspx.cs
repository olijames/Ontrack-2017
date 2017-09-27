using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Diagnostics;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using System.Web;
using Electracraft.Framework.Utility;
//using System.Collections;
//using System.Data.SqlClient;


namespace Electracraft.Client.Website
{
    public partial class AddMaterialToVehicle : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDownList();
            }
        }
        protected void LoadDropDownList()
        {

            List<DOBase> LineItem = CurrentBRContact.SelectCompanyContacts(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"));//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 



            ddCompanyContacts.DataSource = LineItem;
            ddCompanyContacts.DataTextField = "FirstName";//more here
            ddCompanyContacts.DataValueField = "ContactID";
            ddCompanyContacts.DataBind();


            // CurrentBRContact.SelectCompanyContacts



        }
    }
}