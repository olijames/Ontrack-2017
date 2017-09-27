using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataAccess
{
   public class DASuburb:DABase
    {
        public DASuburb(string ConnectionString)
            :base(ConnectionString)
        { }
        //public List<DOBase> SelectSuburbs(Guid regionCode)
        //{
        //    return  SelectObject(typeof(DOSuburb), "RegionCode={0}", regionCode);
        //}
    }
}
