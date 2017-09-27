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

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class MaterialDetails : PageBase
    {
        DOMaterial Material;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    Material = CurrentBRJob.SelectMaterial(new Guid(Request.QueryString["id"]));
                    if (Material == null) throw new Exception();
                }
                catch
                {
                    ShowMessage("The material ID is invalid.", MessageType.Error);
                    btnSave.Enabled = false;
                }
            }
            else
            {
                Material = CurrentBRJob.CreateMaterial(CurrentSessionContext.Owner.ContactID);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadForm();
            litContactName.Text = CurrentSessionContext.CurrentContact.DisplayName;

        }

        private void LoadForm()
        {
            txtCostPrice.Text = Material.CostPrice.ToString();
            txtSellPrice.Text = Material.SellPrice.ToString();
            txtMaterialName.Text = Material.MaterialName;
            txtDescription.Text = Material.Description;
            List<DOBase> Categories = CurrentBRJob.SelectMaterialCategories(CurrentSessionContext.CurrentContact.ContactID);
            //List<DOBase> Categories = CurrentBRJob.SelectMaterialCategoriesLinked(CurrentSessionContext.Owner.ContactID);
            Categories.Insert(0, new DOMaterialCategory() { MaterialCategoryID = Guid.Empty, CategoryName = "Select..." });

            var selected = from DOMaterialCategory c in Categories where c.MaterialCategoryID == Material.MaterialCategoryID select c;
            if (selected.Count<DOMaterialCategory>() == 0)
            {
                Categories.Insert(0, CurrentBRJob.SelectMaterialCategory(Material.MaterialCategoryID));
            }

            ddlMaterialCategory.Items.Clear();
            foreach (DOMaterialCategory Category in Categories)
            {
                if (Category.MaterialCategoryID == Guid.Empty)
                {
                    ddlMaterialCategory.Items.Add(new ListItem("Select...", Guid.Empty.ToString()));
                }
                else
                {
                    DOContact Contact = GetContact(Category.ContactID);

                    ddlMaterialCategory.Items.Add(new ListItem(
                        string.Format("{0}", Category.CategoryName),
                        Category.MaterialCategoryID.ToString()) { Selected = Category.MaterialCategoryID == Material.MaterialCategoryID }
                        );
                }
            }
        }

        Dictionary<Guid, DOContact> Contacts = new Dictionary<Guid, DOContact>();
        private DOContact GetContact(Guid ContactID)
        {
            if (!Contacts.ContainsKey(ContactID))
                Contacts.Add(ContactID, CurrentBRContact.SelectContact(ContactID));
            return Contacts[ContactID];
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/MaterialList.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool NewMaterial = Material.PersistenceStatus == ObjectPersistenceStatus.New;
                Material.MaterialName = txtMaterialName.Text;
                Material.Description = txtDescription.Text;
                Material.CostPrice = GetDecimal(txtCostPrice, "Cost Price");
                Material.SellPrice = GetDecimal(txtSellPrice, "Sell Price");
                Material.MaterialCategoryID = GetDDLGuid(ddlMaterialCategory);
                if (Material.MaterialCategoryID == Guid.Empty)
                    throw new FieldValidationException("Please select a category.");

                CurrentBRJob.SaveMaterial(Material);
                if (NewMaterial)
                    Response.Redirect("MaterialDetails.aspx?id=" + Material.MaterialID.ToString());

                ShowMessage("Saved successfully.", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

    }
}