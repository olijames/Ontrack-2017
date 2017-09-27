using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("District")]
   public class DODistrict:DOBase
    {
        [DatabaseField("DistrictID", IsPrimaryKey =true)]
        public Guid DistrictID
        {
            get;
            set;
        }
        [DatabaseField("DistrictName")]
        public string DistrictName
        {
            get;
            set;
        }
        [DatabaseField("RegionID")]
        public Guid RegionID
        {
            get;
            set;
        }
    }
}
