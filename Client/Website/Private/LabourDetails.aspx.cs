using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class LabourDetails : PageBase
    {
        DOLabour Labour;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    Labour = CurrentBRJob.SelectLabour(new Guid(Request.QueryString["id"]));
                    if (Labour == null) throw new Exception();
                }
                catch
                {
                    ShowMessage("The Labour Item ID is invalid.", MessageType.Error);
                    btnSave.Enabled = false;
                }
            }
            else
            {
                Labour = CurrentBRJob.CreateLabour(CurrentSessionContext.Owner.ContactID);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadForm();
        }

        private void LoadForm()
        {
            txtCostPrice.Text = Labour.CostPrice.ToString();
            txtSellPrice.Text = Labour.SellPrice.ToString();
            txtLabourName.Text = Labour.LabourName;
            txtDescription.Text = Labour.Description;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/LabourList.aspx",false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool NewLabour = Labour.PersistenceStatus == ObjectPersistenceStatus.New;
                Labour.LabourName = txtLabourName.Text;
                Labour.Description = txtDescription.Text;
                Labour.CostPrice = GetDecimal(txtCostPrice, "Cost Price");
                Labour.SellPrice = GetDecimal(txtSellPrice, "Sell Price");

                CurrentBRJob.SaveLabour(Labour);
                if (NewLabour)
                    Response.Redirect("LabourDetails.aspx?id=" + Labour.LabourID.ToString());

                ShowMessage("Saved successfully.", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

    }
}