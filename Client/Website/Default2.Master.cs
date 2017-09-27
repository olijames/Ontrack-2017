using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;

namespace Electracraft.Client.Website
{
    public partial class Default2 : MasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
            DataBind();
            System.Diagnostics.Debug.WriteLine("Default2.Master Page_Prerender");

        }

        public override void ShowMessage(string Message, PageBase.MessageType mType)
        {
            pnlMessage.Visible = true;
            litMessage.Text = Message;

            if (mType != PageBase.MessageType.None)
            {
                switch (mType)
                {
                    case PageBase.MessageType.Error: pnlMessageClass.CssClass = "message-error"; break;
                    case PageBase.MessageType.Info: pnlMessageClass.CssClass = "message-info"; break;
                    case PageBase.MessageType.Warning: pnlMessageClass.CssClass = "message-warning"; break;
                }
            }
        }
    }
}