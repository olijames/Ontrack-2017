using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskAcknowledgement")]
    public class DOTaskAcknowledgement : DOBase
    {
        [DatabaseField("TaskAcknowledgementID", IsPrimaryKey=true)]
        public Guid TaskAcknowledgementID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }
       
    }
}
