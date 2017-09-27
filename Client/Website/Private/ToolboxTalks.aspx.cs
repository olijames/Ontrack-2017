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
    public partial class ToolboxTalks : PageBase
    {
        protected DOSite CurrentSite;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (CurrentSessionContext.CurrentSite == null)
                Response.Redirect(Constants.URL_Home);
            DOSite Site = CurrentSessionContext.CurrentSite;
            CurrentSite = Site;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            FileDisplayer1.FileList = CurrentBRSite.SelectToolBoxFiles(CurrentSite.SiteID);
        }
       
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            { 
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile file = uploadedFiles[i];
                    if (file.ContentLength < 10000000)
                    {
                        DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, CurrentSite.SiteID, file, file.ContentLength, CurrentSessionContext.CurrentContact.ContactID);
                        DOToolBoxFile tbf = CurrentBRSite.CreateToolBoxFile(CurrentSessionContext.Owner.ContactID, CurrentSite.SiteID, File.FileID);
                        CurrentBRSite.SaveToolBoxFile(tbf);
                       
                    }
                    else
                    {
                        ShowMessage(file.FileName + " - Upload Size exceeded");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                ShowMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                fileNew.Attributes.Clear();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_SiteHome);
        }
    }
}