using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
   public class BRDistrict:BRBase
    {
        private DADistrict _CurrentDADistrict;
        private DADistrict CurrentDADistrict
        {
            get
            {
                if (_CurrentDADistrict == null)
                    _CurrentDADistrict = new DADistrict(ConnectionString);
                return _CurrentDADistrict;
            }
        }


        //Add District
        public void AddDistrict(DODistrict district)
        {
            CurrentDADistrict.SaveObject(district);
        }

        //Select districts
        public List<DOBase> SelectDistricts(Guid regionID)
        {
            return CurrentDADistrict.SelectObjectsOrderByWhereClause(typeof(DODistrict),"RegionID = {0}", "DistrictName", regionID);
        }
    }
}
