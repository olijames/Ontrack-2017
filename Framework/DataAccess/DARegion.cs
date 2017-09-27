using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Framework.DataAccess
{
  public  class DARegion:DABase
    {
        public DARegion(string ConnectionString)
            : base(ConnectionString)
        { }
        List<DOBase> region = new List<DOBase>();

        //Select all regions
        //public List<DOBase> SelectRegions()
        //{
        // return SelectObjects(typeof(DORegion));
        //}
    }
}
