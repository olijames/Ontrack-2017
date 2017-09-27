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

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class MaterialCategoryDetails : PageBase
    {
        DOMaterialCategory Category;
        DOContact Contact;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentMaterialCategory == null)
            {
                if (CurrentSessionContext.CurrentContact == null)
                    Contact = CurrentSessionContext.Owner;
                else
                    Contact = CurrentSessionContext.CurrentContact;
                Category = CurrentBRJob.CreateMaterialCategory(CurrentSessionContext.Owner.ContactID, Contact.ContactID);
            }
            else
            {
                Category = CurrentSessionContext.CurrentMaterialCategory;
                Contact = CurrentBRContact.SelectContact(Category.ContactID);
            }


        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            litContactName.Text = Contact.DisplayName;
            txtCategoryName.Text = Category.CategoryName;
            chkActive.Checked = Category.Active;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_MaterialCategory);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Category.CategoryName = txtCategoryName.Text;
                Category.Active = chkActive.Checked;
                CurrentBRJob.SaveMaterialCategory(Category);
                CurrentSessionContext.CurrentMaterialCategory = Category;
                ShowMessage("Saved successfully", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
            
        }
    }
}