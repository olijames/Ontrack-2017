using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.UserControls
{
    public partial class FileDisplayerToolBoxFiles : UserControlBase
    {
        public List<DOBase> FileList;
        public string FileTypeFilter { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var vFiles = from DOFileUpload f in FileList
                        where f.FileType == DOFileUpload.FileTypeEnum.File
                        select f;
            List<DOBase> Files = vFiles.ToList<DOBase>();
            var vImages = from DOFileUpload f in FileList
                         where f.FileType == DOFileUpload.FileTypeEnum.Image
                         select f;
            List<DOBase> Images = vImages.ToList<DOBase>();

            if (string.IsNullOrEmpty(FileTypeFilter) || FileTypeFilter.ToLower() != "images")
            {
                pnlFiles.Visible = true;
                rpFiles.DataSource = Files;
                rpFiles.DataBind();
            }
            else
            {
                pnlFiles.Visible = false;
            }
            if (string.IsNullOrEmpty(FileTypeFilter) || FileTypeFilter.ToLower() != "files")
            {
                pnlImages.Visible = true;
                rpImages.DataSource = Images;
                rpImages.DataBind();
            }
            else
            {
                pnlImages.Visible = false;
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Guid FileID = new Guid(b.CommandArgument);
            DOFileUpload RemoveFile = ParentPage.CurrentBRJob.SelectFileUpload(FileID);
            DOToolBoxFile ToolBoxFile = ParentPage.CurrentBRSite.SelectToolBoxFileByFileID(FileID);

            if (ToolBoxFile != null)
                ParentPage.CurrentBRSite.DeleteToolBoxFile(ToolBoxFile);
            if (RemoveFile != null)
                ParentPage.CurrentBRJob.DeleteFileUpload(RemoveFile);
        }
    }
}