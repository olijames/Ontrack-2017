using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("OperatingSites")]
   public class DOOperatingSites:DOBase
    {
        [DatabaseField("OSID",IsPrimaryKey =true)]
        public Guid OSID
        {
            get;
            set;
        }
        [DatabaseField("ContactID")]
        public Guid ContactID
        {
            get;
            set;
        }
        [DatabaseField("SuburbID")]
        public Guid SuburbID
        {
            get;
            set;
        }
    }
}
