using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("FileUpload")]
    public class DOFileUpload : DOBase
    {
        public enum FileTypeEnum : int
        {
            File = 0,
            Image = 1
        }

        public enum ImageType : int
        {
            Thumb = 100,
            Standard = 200,
            Original = 300
        }

        [DatabaseField("FileID", IsPrimaryKey = true)]
        public Guid FileID { get; set; }

        [DatabaseField("GroupID")]
        public Guid GroupID { get; set; }

        [DatabaseField("Filename")]
        public string Filename { get; set; }

        [DatabaseField("FileType")]
        public FileTypeEnum FileType { get; set; }

        [DatabaseField("FileSize")]
        public long FileSize { get; set; }

        [DatabaseField("CompanyID")]
        public Guid CompanyID { get; set; }

    }
}
