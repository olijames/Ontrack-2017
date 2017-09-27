using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Suburb")]
    public class DOSuburb:DOBase
    {
        [DatabaseField("SuburbID", IsPrimaryKey = true)]
        public Guid SuburbID
        {
            get;
            set; 
        }
        [DatabaseField("SuburbName")]
        public string SuburbName
        {
            get;
            set;
        }
        [DatabaseField("DistrictID")]
        public Guid DistrictID
        {
            get;
            set;
        }
    }
}
