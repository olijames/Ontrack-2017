using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOContractorCustomerLinkedInfo : DOBase
    {
        [DatabaseField("ContactCustomerID", IsPrimaryKey = true)]
        public Guid ContactCustomerID { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorId { get; set; }

        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }

        [DatabaseField("Linked")]
        public DOContractorCustomer.LinkedEnum Linked
        { get; set; }
    }
}
