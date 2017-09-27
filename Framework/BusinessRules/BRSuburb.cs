using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
   public class BRSuburb:BRBase
    {
        private DASuburb _CurrentDASuburb;
        private DASuburb CurrentDASuburb
        {
            get
            {
                if (_CurrentDASuburb == null)
                    _CurrentDASuburb = new DASuburb(ConnectionString);
                return _CurrentDASuburb;
            }
        }
        //Select suburb in the region
        public List<DOBase> SelectSuburbs(Guid districtID)
        {
            return CurrentDASuburb.SelectObjects(typeof(DOSuburb), "DistrictID={0}", districtID);
        }
        //Insert suburb
        public void AddSuburb(DOSuburb suburb)
        {
            CurrentDASuburb.SaveObject(suburb);
        }
        //Select suburb in the region sorted
        public List<DOBase> SelectSuburbsSorted(Guid districtID)
        {
            return CurrentDASuburb.SelectObjectsOrderByWhereClause(typeof(DOSuburb), "DistrictID={0}", "SuburbName", districtID);
        }
    }
}
