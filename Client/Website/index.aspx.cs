using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website
{
    public partial class Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Remove session and login details if on login page.
                Logout();
            }
            string Action = Request.QueryString["action"];
            if (Action == "logout")
            {
                Logout();
                Response.Redirect("~/");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        //Jareds
        protected void btnMaterialInput_Click(object sender, EventArgs e)
        {
            Response.Redirect("MaterialInput.aspx", false);
        }

       






        //Jareds End
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Login(txtUsername.Text, txtPassword.Text, chkRememberMe.Checked);
            if (CurrentSessionContext==null || CurrentSessionContext.Owner == null)
            {
                //pnlLoginFailed.Visible = true;
                //litUsernameFailed.Text = txtUsername.Text;
                ShowMessage("Login failed for user " + txtUsername.Text + ".");
            }
            else
            {
                string ReturnURL = Request.QueryString["returnurl"];
                if (!string.IsNullOrEmpty(ReturnURL))
                    Response.Redirect(ReturnURL);
                else
                    Response.Redirect(Constants.URL_Home);
            }
        }

    }
}