using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskCompletion")]
    public class DOTaskCompletion : DOBase
    {
        [DatabaseField("TaskCompletionID", IsPrimaryKey = true)]
        public Guid TaskCompletionID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }        
    }
}
