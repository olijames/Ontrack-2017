using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class PromoteBusiness : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            StatusText.Visible = false;
            SuburbStatusText.Visible = false;
            

            if (!IsPostBack)
            {
                if (CurrentSessionContext.CurrentContact.Searchable == 1)
                    SearchTick.Checked = true;
                List<DOBase> Regions = CurrentBRRegion.SelectRegions();
                RegionDD.DataSource = Regions;
                RegionDD.DataTextField = "RegionName";
                RegionDD.DataValueField = "RegionID";
                RegionDD.DataBind();
                LoadDistrict();
                LoadSuburbs();
                //gets the trade categories
                LoadTradeCategories();
               // LoadSubTradeCategories();
            }
        }
        public void LoadDistrict()
        {
            List<DOBase> Districts = CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
            District_DDL.DataSource = Districts;
            District_DDL.DataTextField = "DistrictName";
            District_DDL.DataValueField = "DistrictID";
            District_DDL.DataBind();
        }
        protected void btn_SaveTradeCategories_Click(object sender, EventArgs e)
        {
            if (TradeCategories_DDL.SelectedValue != "-Select-")
            {
                DOContactTradeCategory ContTradCat = new DOContactTradeCategory();
                ContTradCat.ContactID = CurrentSessionContext.CurrentContact.ContactID;
                    ContTradCat.TradeCategoryID = Guid.Parse(TradeCategories_DDL.SelectedValue.ToString());
                ContTradCat.SubTradeCategoryID = Guid.Empty;
                ContTradCat.ContactTradeCategoryID = Guid.NewGuid();
                //Saves the trade category
                try
                {
                    CurrentBRContactTradecategory.SaveContactTradecategory(ContTradCat);
                    StatusText.Text = "Trade categories updated";
                    StatusText.Visible = true;
                }
                catch (Exception ex)
                {
                    StatusText.Text = "Trade Category already mapped";
                    StatusText.Visible = true;
                    //ShowMessage("Trade Category already mapped");
                }


                LoadTradeCategories();
                //gets the existing tradecategories for the loggedin contactID
                List<DOBase> existingTradecategories =
                                             CurrentBRContactTradecategory.SelectCTC(CurrentSessionContext.Owner.ContactID);

                foreach (DOContactTradeCategory ctc in existingTradecategories)
                {
                    //Deletes the existing mapping once
                    // CurrentBRContactTradecategory.DeleteCTC(ctc);
                }
                if (SubtradeCategoryRdBtn.SelectedValue == "All")
                {
                    SUbTradeCategory_div.Visible = false;
                    // Suburb_CBList.
                    foreach (ListItem item in SubTradeCategories_CBList.Items)
                    {
                        item.Selected = true;
                    }

                }
                foreach (ListItem item in SubTradeCategories_CBList.Items)
                {
                    if (item.Selected)
                    {
                        try
                        {
                            DOContactTradeCategory newCtc = new DOContactTradeCategory();
                            newCtc.ContactID = CurrentSessionContext.CurrentContact.ContactID;
                            newCtc.SubTradeCategoryID = Guid.Parse(item.Value);
                            newCtc.ContactTradeCategoryID = Guid.NewGuid();
                            //Saves the trade category
                            CurrentBRContactTradecategory.SaveContactTradecategory(newCtc);
                            StatusText.Text = "Trade categories updated";
                            //StatusText.Visible = true;
                        }
                        catch (System.Data.SqlClient.SqlException se)
                        {
                            if (se.Number == 2601)
                            {
                                StatusText.Text = "Trade Category already mapped";
                            }
                        }
                        finally
                        {
                            StatusText.Visible = true;
                        }
                    }
                    else
                    {
                        List<DOBase> oldCTC = CurrentBRContactTradecategory.FindExistingSubTradeCategory(Guid.Parse(item.Value));
                        if (oldCTC.Count >= 1)
                        {
                            // DOContactTradeCategory ctc = oldCTC as DOContactTradeCategory;
                            foreach (DOContactTradeCategory ctc in oldCTC)
                                CurrentBRContactTradecategory.DeleteCTC(ctc);
                            StatusText.Text = "Trade categories updated";
                            StatusText.Visible = true;
                        }
                    }

                }
            }
            else
            {
                ShowMessage("Please select trade category");
            }
        }
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrict();
            LoadSuburbs();
            btn_SaveOperatingSites.Focus();
        }
        protected void LoadSuburbs()
        {
            Guid districtID;
            districtID = Guid.Parse(District_DDL.SelectedValue);
            List<DOBase> Suburbs = CurrentBRSuburb.SelectSuburbsSorted(districtID);
            Suburb_CBList.DataSource = Suburbs;
            Suburb_CBList.DataTextField = "SuburbName";
            Suburb_CBList.DataValueField = "SuburbID";
            Suburb_CBList.DataBind();
        }

        protected void btn_SaveOperatingSites_Click(object sender, EventArgs e)
        {
            if (SuburbSelection_rdbtn.SelectedValue == "All")
            {
                Suburb_cbl_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = true;
                }

            }
            foreach (ListItem item in Suburb_CBList.Items)
            {
                if (item.Selected)
                {
                    try { 

                    DOOperatingSites os = new DOOperatingSites();
                    os.ContactID = CurrentSessionContext.CurrentContact.ContactID;
                    os.OSID = Guid.NewGuid();
                    os.SuburbID = Guid.Parse(item.Value);

                    CurrentBROperatingSites.SaveOS(os);
                       
                    }
                    catch(System.Data.SqlClient.SqlException se)
                    {
                        se.StackTrace.ToString();
                    }
                    finally
                    {
                        SuburbStatusText.Visible = true;
                        SuburbStatusText.Text = "Areas of operation updated";
                    }
                }
               
            }
        }
        private void LoadSubTradeCategories()
        {
            List<DOBase> subTradeCategories = CurrentBRTradeCategory.SelectSubTradeCategories(Guid.Parse(TradeCategories_DDL.SelectedValue));
            SubTradeCategories_CBList.DataSource = subTradeCategories;
            SubTradeCategories_CBList.DataTextField = "SubTradeCategoryName";
            SubTradeCategories_CBList.DataValueField = "SubTradeCategoryID";
            SubTradeCategories_CBList.DataBind();
            ExistingSubTradeCategories();
           
        }
    public void ExistingSubTradeCategories()
        {
            List<DOBase> existingSubTradecategories =
             CurrentBRContactTradecategory.SelectCTC(CurrentSessionContext.CurrentContact.ContactID);
            SubTradeCategories_CBList.PreRender += (object sender, EventArgs e) =>
            {
                existingSubTradecategories.ForEach(
                 (Item) =>
                 {
                     var TradeCategory = Item as DOContactTradeCategory;
                     if (TradeCategory != null)
                     {
                         var match = SubTradeCategories_CBList.Items.FindByValue(TradeCategory.SubTradeCategoryID.ToString());
                         if (match != null)
                         {
                             match.Selected = true;
                         }
                     }
                 });
            };
        }
        public void LoadTradeCategories()
        {
            List<DOBase> tradeCategories = new List<DOBase>();
            tradeCategories = CurrentBRTradeCategory.SelectTradeCategories();


            //TradeCategories_CBList.DataSource = tradeCategories;
            //TradeCategories_CBList.DataTextField = "TradeCategoryName";
            //TradeCategories_CBList.DataValueField = "TradeCategoryID";
            //TradeCategories_CBList.DataBind();
            TradeCategories_DDL.DataSource = tradeCategories;
            TradeCategories_DDL.DataTextField = "TradeCategoryName";
            TradeCategories_DDL.DataValueField = "TradeCategoryID";
            TradeCategories_DDL.DataBind();
            List<DOBase> exisTradCatList = new List<DOBase>();
            List<DOBase> existingTradecategories =
           CurrentBRContactTradecategory.SelectCTC(CurrentSessionContext.CurrentContact.ContactID);
            if (existingTradecategories.Count == 0)
            {
                ExistTradCat.Items.Clear();
                ExistTradCat.Items.Insert(0, "No Trade Category Selected");
            }
            else
            { 
            for (int i = 0; i < tradeCategories.Count; i++)
            {
                DOTradeCategory TradCat = tradeCategories[i] as DOTradeCategory;
               foreach(var item in existingTradecategories)
                {
                    DOContactTradeCategory exisTradcat = item as DOContactTradeCategory;
                    if (TradCat.TradeCategoryID == exisTradcat.TradeCategoryID)
                        exisTradCatList.Add(tradeCategories[i]);
                }
            }
            if(exisTradCatList.Count>1)
                { 
            for(int i=0;i<exisTradCatList.Count;i++)
                {
                    DOTradeCategory cat1 = exisTradCatList[i] as DOTradeCategory;
                    for(int j=i+1;j<exisTradCatList.Count;j++)
                    {
                        DOTradeCategory cat2 = exisTradCatList[j] as DOTradeCategory;
                        if (cat1.TradeCategoryID == cat2.TradeCategoryID)
                            exisTradCatList.RemoveAt(j);
                    }

                }
                }
                ExistTradCat.DataSource = exisTradCatList;
            ExistTradCat.DataTextField= "TradeCategoryName";
            ExistTradCat.DataValueField = "TradeCategoryID";
            }
            ExistTradCat.DataBind();
            //   IEnumerable<DOBase> both = existingTradecategories.Except(tradeCategories);
            // foreach(ListItem item in existingTradecategories)
            //{
            //    ListItem currentcheckbox = TradeCategories_CBList.Items.FindByValue(item.ToString());
            //}

        }

        protected void TradeCategories_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubTradeCategories();
        }

        protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbs();
            btn_SaveOperatingSites.Focus();
        }
        protected void SuburbSelection_rdbtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SuburbSelection_rdbtn.SelectedValue == "Select")
            {
                Suburb_cbl_div.Visible = true;
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = false;
                }

            }
            else if (SuburbSelection_rdbtn.SelectedValue == "All")
            {
                Suburb_cbl_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = true;
                }
              
            }
       

        }
        protected void SubtradeCategoryRdBtn_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            LoadSubTradeCategories();
            if (SubtradeCategoryRdBtn.SelectedValue == "Select")
            {
                SUbTradeCategory_div.Visible = true;
                foreach (ListItem item in SubTradeCategories_CBList.Items)
                {
                    item.Selected = false;
                }
                LoadSubTradeCategories();
            }
            else if (SubtradeCategoryRdBtn.SelectedValue == "All")
            {
                SUbTradeCategory_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in SubTradeCategories_CBList.Items)
                {
                    item.Selected = true;
                }
               
            }
        }

        protected void Yes_Click(object sender, EventArgs e)
        {
            if (SearchTick.Checked == true)
            {
                DOContact cont = new DOContact();
                cont = CurrentSessionContext.CurrentContact;
                cont.Searchable = 1;
                CurrentBRContact.SaveContact(cont);
                ShowMessage("Thank you! You will now appear as a contractor");
            }
            else if (SearchTick.Checked == false)
            {
                DOContact cont = new DOContact();
                cont = CurrentSessionContext.CurrentContact;
                cont.Searchable = 0;
                CurrentBRContact.SaveContact(cont);
                ShowMessage("Thank you! You are removed from the contractors list");
            }
            /// searchable.Visible = false;
        }

        protected void No_Click(object sender, EventArgs e)
        {
            searchable.Visible = false;
        }

        protected void TradeCategories_DDL_PreRender(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                TradeCategories_DDL.Items.Insert(0, "-Select-");
            }
        }

        //protected void ExistTradCat_PreRender(object sender, EventArgs e)
        //{
        //    List<DOBase> existingTradecategories =
        //    CurrentBRContactTradecategory.SelectCTC(CurrentSessionContext.CurrentContact.ContactID);
        //    if (existingTradecategories.Count == 0)
        //    { 
        //     ExistTradCat.Items.Clear();
        //        ExistTradCat.Items.Insert(0,"No Trade Category Selected");
        //    }
        //    for(int i=0;i< ExistTradCat.Items.Count;i++)
        //    {
        //        DOContactTradeCategory exisTradCat = item as DOContactTradeCategory;
        //           ListItem found = ExistTradCat.Items.FindByValue(exisTradCat.TradeCategoryID.ToString());
        //        if (found == null)
        //            ExistTradCat.Items.Remove(exisTradCat.ToString());
        //    }

        //}
    }
}