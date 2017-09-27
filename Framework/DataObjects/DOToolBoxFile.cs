using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ToolBoxFile")]
    public class DOToolBoxFile: DOBase
    {
        [DatabaseField("ToolBoxFileID", IsPrimaryKey = true)]
        public Guid ToolBoxFileID { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID { get; set; }

        [DatabaseField("FileID")]
        public Guid FileID { get; set; }
    }
}
