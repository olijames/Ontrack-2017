using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskFile")]
    public class DOTaskFile : DOBase
    {
        [DatabaseField("TaskFileID", IsPrimaryKey = true)]
        public Guid TaskFileID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("FileID")]
        public Guid FileID { get; set; }
    }
}
