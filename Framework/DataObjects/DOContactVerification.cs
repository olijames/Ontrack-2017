using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ContactVerification")]
    public class DOContactVerification : DOBase
    {
        [DatabaseField("ID", IsPrimaryKey = true)]
        public Guid ID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("VerificationCode")]
        public string VerificationCode { get; set; }

    }
}
