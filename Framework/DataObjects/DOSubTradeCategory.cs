using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("SubTradeCategory")]
   public class DOSubTradeCategory:DOBase
    {
        [DatabaseField("SubTradeCategoryID", IsPrimaryKey = true)]
        public Guid SubTradeCategoryID
        {
            get;
            set;
        }

        [DatabaseField("SubTradeCategoryName")]
        public string SubTradeCategoryName
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
