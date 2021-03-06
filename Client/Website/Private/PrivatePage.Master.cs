﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;

namespace Electracraft.Client.Website.Private
{
    public partial class PrivatePage : MasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
			DataBind();
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