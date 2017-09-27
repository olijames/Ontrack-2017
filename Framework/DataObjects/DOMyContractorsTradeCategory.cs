using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("MyContractorsTradeCategory")]
   public class DOMyContractorsTradeCategory:DOBase
    {
        [DatabaseField("ContractorCustomerTradeCategoryID",IsPrimaryKey =true)]
        public Guid MyContractorsTradeCategoryID
        {
            get;
            set;
        }
        [DatabaseField("ContractorCustomerID")]
        public Guid ContractorCustomerID
        {
            get;
            set;
        }
        [DatabaseField("TradeCategoryID")]
        public Guid TradeCategoryID
        {
            get;
            set;
        }
        }
}
