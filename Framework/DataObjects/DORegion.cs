using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Region")]
    public class DORegion : DOBase
    {
        [DatabaseField("RegionID", IsPrimaryKey = true)]
        public Guid RegionID
        {
            get;
            set;
        }

        [DatabaseField("RegionName")]
        public string RegionName
        {
            get;
            set;
        }
       
    }
}
