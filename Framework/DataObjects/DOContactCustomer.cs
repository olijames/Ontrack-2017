using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ContractorCustomer")]
    public class DOContactCustomer : DOBase
    {
        [DatabaseField("ContactCustomerID", IsPrimaryKey = true)]
        public Guid ContactCustomerID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }
        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }
       
             [DatabaseField("ContactIDCustomer")]
        public Guid ContactIDCustomer { get; set; }
    }
}
