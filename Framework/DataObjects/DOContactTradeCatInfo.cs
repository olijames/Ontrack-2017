using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
   public class DOContactTradeCatInfo:DOBase
    {
       
        [DatabaseField("ContactID", IsPrimaryKey = true)]
        public Guid ContactID { get; set; }
        [DatabaseField("TradeCategoryID")]
        public Guid TradeCategoryID
        { get; set; }
        [DatabaseField("FirstName")]
        public string FirstName { get; set; }
        [DatabaseField("LastName")]
        public string LastName { get; set; }
        [DatabaseField("CompanyName")]
        public string CompanyName { get; set; }
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(CompanyName))
                    return CompanyName;
                else
                    return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        [DatabaseField("Subscribed")]
        public bool Subscribed { get; set; }
        [DatabaseField("Searchable")]
        public int Searchable { get; set; }
        //[DatabaseField("ContactTradeCategoryID")]
        //public Guid ContactTradeCategoryID
        //{
        //    get;
        //    set;
        //}
        //public Guid SubTradeCategoryID
        //{
        //    get;
        //    set;
        //}
    }
}
