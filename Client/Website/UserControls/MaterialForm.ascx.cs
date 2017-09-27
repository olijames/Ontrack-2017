using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.UserControls
{
    public partial class MaterialForm : UserControlBase
    {
        public Guid CategoryContactID { get; set; }
        public bool Cleared { get; set; }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            hidCategoryContactID.Value = CategoryContactID.ToString();
        }

        public void LoadForm(DOMaterial Material)
        {
            if (Material.PersistenceStatus == ObjectPersistenceStatus.Existing)
            {
                txtCostPrice.Text = Material.CostPrice.ToString();
                txtSellPrice.Text = Material.SellPrice.ToString();
                txtMaterialName.Text = Material.MaterialName;
                txtDescription.Text = Material.Description;
            }
            else if (IsPostBack && !Cleared)
            {                
                txtCostPrice.Text = Request.Form[txtCostPrice.UniqueID];
                txtSellPrice.Text = Request.Form[txtSellPrice.UniqueID];
                txtMaterialName.Text = Request.Form[txtMaterialName.UniqueID];
                txtDescription.Text = Request.Form[txtDescription.UniqueID];
            }

            List<DOBase> Categories = ParentPage.CurrentBRJob.SelectMaterialCategories(CategoryContactID);
            //List<DOBase> Categories = CurrentBRJob.SelectMaterialCategoriesLinked(CurrentSessionContext.Owner.ContactID);
            Categories.Insert(0, new DOMaterialCategory() { MaterialCategoryID = Guid.Empty, CategoryName = "Select..." });

            var selected = from DOMaterialCategory c in Categories where c.MaterialCategoryID == Material.MaterialCategoryID select c;
            if (selected.Count<DOMaterialCategory>() == 0)
            {
                Categories.Insert(0, ParentPage.CurrentBRJob.SelectMaterialCategory(Material.MaterialCategoryID));
            }

            ddlMaterialCategoryNew.Items.Clear();
            foreach (DOMaterialCategory Category in Categories)
            {
                if (Category.MaterialCategoryID == Guid.Empty)
                {
                    ddlMaterialCategoryNew.Items.Add(new ListItem("Select...", Guid.Empty.ToString()));
                }
                else
                {
                    DOContact Contact = GetContact(Category.ContactID);

                    ddlMaterialCategoryNew.Items.Add(new ListItem(
                        string.Format("{0}", Category.CategoryName),
                        Category.MaterialCategoryID.ToString()) { Selected = Category.MaterialCategoryID == Material.MaterialCategoryID }
                        );
                }
            }
            ddlMaterialCategoryNew.Items.Add(new ListItem("New Category...", Constants.Guid_DefaultUser.ToString()));

            if (IsPostBack)
            {
                string selectedCategory = Request.Form[ddlMaterialCategoryNew.UniqueID];
                ListItem li = ddlMaterialCategoryNew.Items.FindByValue(selectedCategory);
                if (li != null) li.Selected = true;
            }
        }

        public void SaveForm(DOMaterial Material)
        {
            bool NewMaterial = Material.PersistenceStatus == ObjectPersistenceStatus.New;
            Material.MaterialName = txtMaterialName.Text;
            if (string.IsNullOrEmpty(Material.MaterialName))
                throw new FieldValidationException("Material name is required.");
            Material.Description = txtDescription.Text;
            Material.CostPrice = ParentPage.GetDecimal(txtCostPrice, "Cost Price");
            if (Material.CostPrice == 0)
                throw new FieldValidationException("Cost price is required.");
            Material.SellPrice = ParentPage.GetDecimal(txtSellPrice, "Sell Price");
            if (Material.SellPrice == 0)
                throw new FieldValidationException("Sell price is required.");
            Material.MaterialCategoryID = ParentPage.GetDDLGuid(ddlMaterialCategoryNew);
            if (Material.MaterialCategoryID == Guid.Empty)
                throw new FieldValidationException("Please select a category.");

            if (Material.MaterialCategoryID == Constants.Guid_DefaultUser && !string.IsNullOrEmpty(hidCategoryContactID.Value))
            {
                //Add the new category.
                DOMaterialCategory mc = ParentPage.CurrentBRJob.CreateMaterialCategory(ParentPage.CurrentSessionContext.Owner.ContactID, new Guid(hidCategoryContactID.Value));
                mc.CategoryName = txtNewCategoryName.Text;
                if (string.IsNullOrEmpty(mc.CategoryName))
                    throw new FieldValidationException("Category name is required.");
                ParentPage.CurrentBRJob.SaveMaterialCategory(mc);
                Material.MaterialCategoryID = mc.MaterialCategoryID;
            }

            //Clear the form.
            ClearForm();
        }

        public void ClearForm()
        {
            txtCostPrice.Text = string.Empty;
            txtSellPrice.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtNewCategoryName.Text = string.Empty;
            txtMaterialName.Text = string.Empty;
            if (ddlMaterialCategoryNew.Items.Count > 0)
                ddlMaterialCategoryNew.SelectedIndex = 0;
            Cleared = true;
        }

        Dictionary<Guid, DOContact> Contacts = new Dictionary<Guid, DOContact>();
        private DOContact GetContact(Guid ContactID)
        {
            if (!Contacts.ContainsKey(ContactID))
                Contacts.Add(ContactID, ParentPage.CurrentBRContact.SelectContact(ContactID));
            return Contacts[ContactID];
        }

    }
}