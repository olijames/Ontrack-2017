using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("CustomerInvitation")]
    public class DOCustomerInvitation : DOBase
    {
        [DatabaseField("CIID", IsPrimaryKey = true)]
        public Guid CIID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("InviterID")]
        public Guid InviterID { get; set; }

    }
}
