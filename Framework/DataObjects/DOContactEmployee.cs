using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOContactEmployee : DOEmployeeInfo
    {
        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("CCActive")]
        public bool ContactCompanyActive { get; set; }

        [DatabaseField("CCPending")]
        public bool ContactCompanyPending { get; set; }
    

    }
}
