using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
   public class BRRegion:BRBase
    {
        private DARegion _CurrentDARegion;
        private DARegion CurrentDARegion
        {
            get
            {
                if (_CurrentDARegion == null)
                    _CurrentDARegion = new DARegion(ConnectionString);
                return _CurrentDARegion;
            }
        }
        //Select all regions
        public List<DOBase> SelectRegions()
        {
            return CurrentDARegion.SelectObjectsOrderBy(typeof(DORegion),"RegionName");
        }
       //Add new region
       public void AddRegion(DORegion region)
        {
            CurrentDARegion.SaveObject(region);
        }
        
    }
}
