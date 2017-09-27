using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class HealthAndSafety : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                        //DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, fileNew.PostedFile);
                        //DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, file);
                        //DOJobFile jf = CurrentBRJob.CreateJobFile(CurrentSessionContext.Owner.ContactID, Job.JobID, File.FileID);
                        //CurrentBRJob.SaveJobFile(jf);
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
        }
    }
}