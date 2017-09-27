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

namespace Electracraft.Client.Website.Private.Admin
{
    [AdminPage]
    public partial class Settings : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //  RegionDD.AppendDataBoundItems = true;
            if (!IsPostBack)
            {
                Txt_NewRegion.Text = "";
                Txt_NewSuburb.Text = "";
                TradeCategoryTextBox.Text = "";
                Loadregions();
                LoadDistricts();
                StatusText.Visible = false;
                LoadTradeCategories();
            }


        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DOGeneralSetting TestEmailRecipient = CurrentBRGeneral.SelectGeneralSetting(Constants.Setting_TestEmailRecipient);
            if (TestEmailRecipient != null)
                txtTestEmailRecipient.Text = TestEmailRecipient.SettingValue;

            DOGeneralSetting WebsiteBasePath = CurrentBRGeneral.SelectGeneralSetting(Constants.Setting_WebsiteBasePath);
            if (WebsiteBasePath != null)
                txtWebsiteBasePath.Text = WebsiteBasePath.SettingValue;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DOGeneralSetting TestEmailRecipient = CurrentBRGeneral.SelectGeneralSetting(Constants.Setting_TestEmailRecipient);
            if (TestEmailRecipient == null)
                TestEmailRecipient = CurrentBRGeneral.CreateGeneralSetting(Constants.Setting_TestEmailRecipient);
            TestEmailRecipient.SettingValue = txtTestEmailRecipient.Text;
            CurrentBRGeneral.SaveGeneralSetting(TestEmailRecipient);

            DOGeneralSetting WebsiteBasePath = CurrentBRGeneral.SelectGeneralSetting(Constants.Setting_WebsiteBasePath);
            if (WebsiteBasePath == null)
                WebsiteBasePath = CurrentBRGeneral.CreateGeneralSetting(Constants.Setting_WebsiteBasePath);
            WebsiteBasePath.SettingValue = txtWebsiteBasePath.Text;
            if (WebsiteBasePath.SettingValue.EndsWith("/"))
                WebsiteBasePath.SettingValue = WebsiteBasePath.SettingValue.Remove(WebsiteBasePath.SettingValue.Length - 1);
            if (!WebsiteBasePath.SettingValue.StartsWith("http"))
            {
                ShowMessage("Website base path must begin with http", MessageType.Error);
            }
            else
            {
                CurrentBRGeneral.SaveGeneralSetting(WebsiteBasePath);
            }

        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            CurrentBRGeneral.DeleteAllData();
            ShowMessage("All data has been deleted.");
        }
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            StatusText.Visible = false;
            StatusText.Text = "";
            Txt_NewRegion.Text = "";
            Txt_NewSuburb.Text = "";
            TradeCategoryTextBox.Text = "";
            LoadDistricts();
            ADD_Suburb_btn.Focus();
        }
        protected void SuburbDD_SelectedIndexChanged(object sender, EventArgs e)
        {

            //SuccessText.Visible = false;
            //ErrorText.Visible = false;
            //if (SuburbDD.SelectedItem.Text == "Add new suburb")
            //{
            //    Txt_NewSuburb.Visible = true;
            //}
            //else
            //{
            //    Txt_NewSuburb.Visible = false;
            //}
        }
        protected void AddTradeCategory_Click(object sender, EventArgs e)
        {
            StatusText.Visible = true;
            StatusText.Text = "";
            Txt_NewRegion.Text = "";
            Txt_NewSuburb.Text = "";

            DOTradeCategory tradeCategory = new DOTradeCategory();

            tradeCategory.TradeCategoryID = Guid.NewGuid();
            tradeCategory.CreatedBy = CurrentSessionContext.Owner.ContactID;
            try
            {
                if (TradeCategoryTextBox.Text != "")
                {
                    tradeCategory.TradeCategoryName = TradeCategoryTextBox.Text.ToString();
                    CurrentBRTradeCategory.SaveTradeCategory(tradeCategory);
                    StatusText.Text = "Trade Category '"+tradeCategory.TradeCategoryName +"' added";
                }
                else
                    StatusText.Text = "Please enter Trade Category name";          
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                if (se.Number == 2601)
                    StatusText.Text = "Trade Category '" +tradeCategory.TradeCategoryName+"' already exists";
            }
            finally
            {
                TradeCategoryTextBox.Text = "";
                TradeCategoryTextBox.Focus();
                LoadTradeCategories();
            }

        }

        protected void RegionDD_DataBound(object sender, EventArgs e)
        {
            //if(!IsPostBack)      
            //RegionDD.Items.Add(new ListItem("Add new region", string.Empty));
        }

        protected void SuburbDD_DataBound(object sender, EventArgs e)
        {
            // if (!IsPostBack)
            //  SuburbDD.Items.Add(new ListItem("Add new suburb", string.Empty));
        }

        protected void AddRegion_Click(object sender, EventArgs e)
        {

            Txt_NewSuburb.Text = "";
            TradeCategoryTextBox.Text = "";
            StatusText.Visible = true;
            StatusText.Text = "";
            DORegion region = new DORegion();
            region.RegionID = Guid.NewGuid();
            try
            {

                if (Txt_NewRegion.Text != "")
            {
                region.RegionName = Txt_NewRegion.Text;
                region.CreatedBy = CurrentSessionContext.Owner.ContactID;

                CurrentBRRegion.AddRegion(region);

                StatusText.Text = "Region Added";
                Loadregions();
                    Txt_NewRegion.Focus();
                }
            else
                StatusText.Text = "Please enter region name";
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                if (se.Number == 2601)
                    StatusText.Text = "Region already exists";
                Txt_NewRegion.Focus();

            }

        }

        protected void AddSuburb_Click(object sender, EventArgs e)
        {
            Txt_NewRegion.Text = "";

            TradeCategoryTextBox.Text = "";
            SuburbStatusText.Visible = true;
            SuburbStatusText.Text = "";
            DOSuburb suburb = new DOSuburb();

            suburb.SuburbID = Guid.NewGuid();
            suburb.DistrictID = Guid.Parse(Districts_DDL.SelectedValue);
            try
            {
                if (Txt_NewSuburb.Text != "")
            {
                suburb.SuburbName = Txt_NewSuburb.Text;
                suburb.CreatedBy = CurrentSessionContext.Owner.ContactID;
                CurrentBRSuburb.AddSuburb(suburb);

                    SuburbStatusText.Text = "Suburb added";
            }
            else
                    SuburbStatusText.Text = "Please enter suburb name";
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                if (se.Number == 2601)
                    SuburbStatusText.Text = "Suburb already exists in" + Districts_DDL.SelectedItem.Text.ToString();
                if (se.Number == 2627)
                    SuburbStatusText.Text = "Suburb already exists";
            }
            Txt_NewSuburb.Focus();
        }
        public void Loadregions()
        {
            List<DOBase> Regions = CurrentBRRegion.SelectRegions();
            RegionDD.DataSource = Regions;
            RegionDD.DataTextField = "RegionName";
            RegionDD.DataValueField = "RegionID";
            RegionDD.DataBind();
            Ddl_RegionDD.DataSource = Regions;
            Ddl_RegionDD.DataTextField = "RegionName";
            Ddl_RegionDD.DataValueField = "RegionID";
            Ddl_RegionDD.DataBind();
        }
        public void LoadTradeCategories()
        {
            List<DOBase> tradeCategories = CurrentBRTradeCategory.SelectTradeCategories();
            TradeCategories_DDL.DataSource = tradeCategories;
            TradeCategories_DDL.DataTextField = "TradeCategoryName";
            TradeCategories_DDL.DataValueField = "TradeCategoryID";
            TradeCategories_DDL.DataBind();
        }
        public void LoadDistricts()
        {
            List<DOBase> Districts = CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
            Districts_DDL.DataSource = Districts;
            Districts_DDL.DataTextField = "DistrictName";
            Districts_DDL.DataValueField = "DistrictID";
            Districts_DDL.DataBind();
        }
        protected void AddDistrict_Click(object sender, EventArgs e)
        {
            Txt_NewSuburb.Text = "";
            TradeCategoryTextBox.Text = "";
            StatusText.Visible = true;
            StatusText.Text = "";
            DODistrict district = new DODistrict();
            district.DistrictID = Guid.NewGuid();
            try
            {
                if (TxtBx_District.Text != "")
                {
                    district.DistrictName = TxtBx_District.Text;
                    district.CreatedBy = CurrentSessionContext.Owner.ContactID;
                    district.RegionID=Guid.Parse(Ddl_RegionDD.SelectedValue.ToString());
                    CurrentBRDistrict.AddDistrict(district);
                    StatusText.Text = "District '"+district.DistrictName+"' Added";
                    Loadregions();
                    Txt_NewRegion.Focus();
                }
                else
                    StatusText.Text = "Please enter district name";
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                StatusText.Text = se.StackTrace.ToString();
                if (se.Number == 2601)
                    StatusText.Text = "District " + district.DistrictName + " already exists";
                Txt_NewRegion.Focus();

            }
            finally
            {
                ADD_Suburb_btn.Focus();
            }
        }

        protected void AddSubTradeCategory_Click(object sender, EventArgs e)
        {
            StatusText.Visible = true;
            StatusText.Text = "";
            Txt_NewRegion.Text = "";
            Txt_NewSuburb.Text = "";
            DOSubTradeCategory subTradeCategory = new DOSubTradeCategory();
            subTradeCategory.SubTradeCategoryID = Guid.NewGuid();
           subTradeCategory.CreatedBy = CurrentSessionContext.Owner.ContactID;
            try
            {
                if(SubTradeCategory_txt.Text!="")
                {
                   subTradeCategory.SubTradeCategoryName = SubTradeCategory_txt.Text.ToString();
           subTradeCategory.TradeCategoryID=Guid.Parse(TradeCategories_DDL.SelectedValue);
            CurrentBRTradeCategory.SaveSubTradeCategory(subTradeCategory);
                    StatusText.Text = "SubTradeCategory '" + subTradeCategory.SubTradeCategoryName + "' added";
                }
                else
                    StatusText.Text = "Please enter SubTradeCategory name";
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                if (se.Number == 2601)
                    StatusText.Text = "SubTradeCategory '" + subTradeCategory.SubTradeCategoryName + "' already exists in "+TradeCategories_DDL.SelectedItem.Text.ToString();
            }
            finally
            {
                SubTradeCategory_txt.Text = "";
                SubTradeCategory_txt.Focus();
            }
        }
    }
}