using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.UserControls
{
    public partial class UserDetails : UserControlBase
    {
        public DOContact Contact { get; set; }
        public bool AdminMode { get; set; }
        private bool CompanyView
        {
            get
            {
                return (Contact == null || Contact.ContactType == DOContact.ContactTypeEnum.Company);
            }
        }

        //Tony added on 18.Apr.2017
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Loadregions();
                LoadDistrict();
            }
        }

        public void LoadForm()
        {
            if (Contact == null) return;
            txtUsername.Text = Contact.Email;
            txtUsername.Enabled = AdminMode;

            txtPhone.Text = Contact.Phone;
            txtAddress1.Text = Contact.Address1;
//            RegionDD.SelectedIndex = 
//            District_DDL.SelectedIndex = 
//            SuburbDD.SelectedIndex = 

//            RegionDD.SelectedIndex = RegionDD.Items.IndexOf(RegionDD.Items.FindByText(Contact.Address2));
            phName.Visible = !CompanyView;
            txtFirstName.Text = Contact.FirstName;
            txtLastName.Text = Contact.LastName;

            phCompany.Visible = CompanyView;
            chkCustomerExclude.Checked = Contact.CustomerExclude;
            txtCompanyName.Text = Contact.CompanyName;
            txtBankAccount.Text = Contact.BankAccount;
            txtCompanyKey.Text = Contact.CompanyKey;

            phAdmin.Visible = AdminMode;
            chkSubscribed.Checked = Contact.Subscribed;
            chkSubscriptionPending.Checked = Contact.SubscriptionPending;
            ((DateControl)dateSubscription).SetDate(Contact.SubscriptionExpiryDate);
            
            litSubscriptionStatus.Text = 
                Contact.SubscriptionPending ? "Pending" : 
                Contact.Subscribed ? "Subscribed until " + DateAndTime.DisplayShortDate(Contact.SubscriptionExpiryDate, "[not specified]") : 
                "Not subscribed";

        }

        // Tony added on 14.Apr.2017
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrict();
            LoadSuburbs();
        }

        public void Loadregions()
        {
            PageBase pb = new PageBase();

            List<DOBase> Regions = pb.CurrentBRRegion.SelectRegions();
            RegionDD.DataSource = Regions;
            RegionDD.DataTextField = "RegionName";
            RegionDD.DataValueField = "RegionID";

            RegionDD.DataBind();
            RegionDD.SelectedValue = Contact.DefaultRegion.ToString();
        }

        protected void LoadSuburbs()
        {

            //PageBase pb = new PageBase();
            if (District_DDL.SelectedValue != "")
            {
                PageBase pb = new PageBase();

                List<DOBase> Suburbs = pb.CurrentBRSuburb.SelectSuburbsSorted(Guid.Parse(District_DDL.SelectedValue));
                SuburbDD.DataSource = Suburbs;
                SuburbDD.DataTextField = "SuburbName";
                SuburbDD.DataValueField = "SuburbID";
                SuburbDD.DataBind();
            }

        }
        protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbs();
        }

        protected void SuburbDD_PreRender(object sender, EventArgs e)
        {
            DOContact myContact = Contact;

            if (myContact != null)
            {
                string suburb = myContact.Address4;
                string district = myContact.Address3;
                string region = myContact.Address2;

                if (!IsPostBack)
                {

                    if (Contact != null)
                    {
                        RegionDD.SelectedIndex =
                            RegionDD.Items.IndexOf(
                                RegionDD.Items.FindByText(Contact.Address2));
                        if (RegionDD.SelectedIndex == -1 ||
                            (RegionDD.SelectedIndex == 0 &&
                             Contact.Address2 != RegionDD.SelectedItem.ToString()))
                        {
                            //RegionDD.Items.Insert(0, new ListItem("-Select-", "")); 17.4.20 jared was causing a fault
                            foreach (ListItem item in RegionDD.Items)
                                item.Selected = false;
                        }
                    }

                    RegionDD.SelectedIndex = RegionDD.Items.IndexOf(RegionDD.Items.FindByText(region));
                    LoadDistrict();

                    District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
                    LoadSuburbs();

                    SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
   


                }
            }
        }
        public void LoadDistrict()
        {
            PageBase pb = new PageBase();
            List<DOBase> Districts = pb.CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
            // Districts.Insert(-1, new ListItem("-Select-", ""));
            //District_DDL.DataBind();
            District_DDL.DataSource = Districts;
            District_DDL.DataTextField = "DistrictName";
            District_DDL.DataValueField = "DistrictID";
            //District_DDL.Items.Insert(0, new ListItem("-Select-", ""));
            //District_DDL.SelectedItem.Value = "Christchurch";
            //District_DDL.SelectedValue = "-Select-";
            //District_DDL.SelectedIndex = 2;
            District_DDL.DataBind();
            District_DDL.PreRender += (object sender, EventArgs e) =>
            {
                //LoadDistrict();
                foreach (ListItem item in District_DDL.Items)
                    item.Selected = false;
                if ((District_DDL.SelectedIndex == -1 || District_DDL.SelectedIndex == 0) && District_DDL.Items.Count >= 2)
                    District_DDL.SelectedIndex = 2;
                District_DDL.DataBind();
                LoadSuburbs();
            };

        }

        public void SaveForm()
        {
            //Contact.UserName = txtUsername.Text;
            Contact.Email = txtUsername.Text;
            Contact.Phone = txtPhone.Text;
            Contact.Address1 = txtAddress1.Text;

            //Tony added on 18.Apr.2017
            Contact.Address2 = RegionDD.SelectedItem.ToString();
            Contact.Address3 = District_DDL.SelectedItem.ToString();
            Contact.Address4 = SuburbDD.SelectedItem.ToString();

//            Contact.Address2 = txtAddress2.Text;
            Contact.FirstName = txtFirstName.Text;
            Contact.LastName = txtLastName.Text;
            Contact.CompanyName = txtCompanyName.Text;
            Contact.CustomerExclude = chkCustomerExclude.Checked;
           // Contact.CompanyKey = txtCompanyKey.Text;
            Contact.BankAccount = txtBankAccount.Text;

            if (AdminMode)
            {
                Contact.SubscriptionExpiryDate = ((DateControl)dateSubscription).GetDate();

                Contact.SubscriptionPending = chkSubscriptionPending.Checked;
                Contact.Subscribed = chkSubscribed.Checked;
                if (Contact.Subscribed) 
                    Contact.SubscriptionPending = false;                
            }
        }

    }
}