using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    enum CompanyRelationship
    {
        Self,
        Owner,
        Manager
    }
    [DatabaseTable("ContactCompany")]
    public class DOContactCompany : DOBase
    {
        [DatabaseField("ContactCompanyID", IsPrimaryKey = true)]
        public Guid ContactCompanyID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("CompanyID")]
        public Guid CompanyID { get; set; }

        [DatabaseField("Pending")]
        public bool Pending { get; set; }

        [DatabaseField("Settings")]
        public long Settings { get; set; }


    }
}
