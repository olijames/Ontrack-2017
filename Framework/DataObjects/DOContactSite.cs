using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ContactSite")]
   public class DOContactSite:DOBase
    {
        [DatabaseField("ContactSiteID",IsPrimaryKey =true)]
        public Guid ContactSiteID
        { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID
        { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID
        { get; set; }

        //Tony Added 5.11.2016
        [DatabaseField("Flag")]
        public int Flag { get; set; }
        
    }
}
