using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ContactTradeCategory")]
   public class DOContactTradeCategory:DOBase
    {
        [DatabaseField("ContactTradeCategoryID",IsPrimaryKey =true)]
        public Guid ContactTradeCategoryID
        {
            get;
            set;
        }
        [DatabaseField("SubTradeCategoryID")]
        public Guid SubTradeCategoryID
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
        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }
    }
}
