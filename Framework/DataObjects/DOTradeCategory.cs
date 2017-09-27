using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TradeCategory")]
   public class DOTradeCategory:DOBase
    {
        [DatabaseField("TradeCategoryID", IsPrimaryKey =true)]
        public Guid TradeCategoryID
        {
            get;
            set;
        }
       

        [DatabaseField("TradeCategoryName")]
        public string TradeCategoryName
        {
            get;
            set;
        }


    }
}
