using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public abstract class DOContactBase : DOBase
    {
        [DatabaseField("FirstName")]
        public string FirstName { get; set; }

        [DatabaseField("LastName")]
        public string LastName { get; set; }

        [DatabaseField("Email")]
        public string Email { get; set; }

        [DatabaseField("Phone")]
        public string Phone { get; set; }

        [DatabaseField("Address1")]
        public string Address1 { get; set; }

        [DatabaseField("Address2")]
        public string Address2 { get; set; }
        [DatabaseField("Address3")]
        public string Address3 { get; set; }
        [DatabaseField("Address4")]
        public string Address4 { get; set; }
        [DatabaseField("CompanyName")]
        public string CompanyName { get; set; }

        public abstract string DisplayName { get; }
        public abstract string ContactBaseType { get; }
        public abstract Guid ID { get; }
    }
}
