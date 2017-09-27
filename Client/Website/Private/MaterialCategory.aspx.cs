using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class MaterialCategory : PageBase
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);
            CurrentSessionContext.CurrentMaterialCategory = null;

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvCategories.DataSource = CurrentBRJob.SelectMaterialCategories(CurrentSessionContext.CurrentContact.ContactID);
            gvCategories.DataBind();
            litContactName.Text = CurrentSessionContext.CurrentContact.DisplayName;
        }


        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
            CurrentSessionContext.CurrentMaterialCategory = null;
            Response.Redirect(Constants.URL_MaterialCategoryDetails);
        }

        protected void btnEditCategory_Click(object sender, EventArgs e)
        {
            CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
            Guid CategoryID = new Guid(((Button)sender).CommandArgument.ToString());
            DOMaterialCategory Category = CurrentBRJob.SelectMaterialCategory(CategoryID);
            CurrentSessionContext.CurrentMaterialCategory = Category;
            Response.Redirect(Constants.URL_MaterialCategoryDetails);
        }

        protected void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            Guid CategoryID = new Guid(((Button)sender).CommandArgument.ToString());
            DOMaterialCategory Category = CurrentBRJob.SelectMaterialCategory(CategoryID);
            try
            {
                CurrentBRJob.DeleteMaterialCategory(Category);
            }
            catch 
            {
                ShowMessage("The category could not be deleted. A category can only be deleted if no materials are assigned to it.");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_MaterialList);
        }

    }
}