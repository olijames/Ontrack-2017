using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOTimeSheetSummary : DOBase
    {
        [DatabaseField("ContactID", IsPrimaryKey = true)]
        public Guid ContactID { get; set; }

        [DatabaseField("FirstName")]
        public string FirstName { get; set; }

        [DatabaseField("LastName")]
        public string LastName { get; set; }

        [DatabaseField("TotalMinutes")]
        public int TotalMinutes { get; set; }

        [DatabaseField("ChargeableMinutes")]
        public int ChargeableMinutes { get; set; }
    }
}
