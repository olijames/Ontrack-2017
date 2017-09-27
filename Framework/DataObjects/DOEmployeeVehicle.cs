using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOEmployeeVehicle : DOBase
    {
        [DatabaseField("EmployeeID")]
        public Guid EmployeeID { get; set; }

        [DatabaseField("NewKey", IsPrimaryKey = true)]
        public string NewKey { get; set; }

        [DatabaseField("FirstName")]
        public string FirstName { get; set; }

        [DatabaseField("LastName")]
        public string LastName { get; set; }

        [DatabaseField("VehicleName")]
        public string VehicleName { get; set; }

        [DatabaseField("VehicleRegistration")]
        public string VehicleRegistration { get; set; }

        [DatabaseField("DisplayInfo")]
        public string DisplayInfo { get; set; }

        //9/3/2017 Jared added during merge
		[DatabaseField("VehicleID")]
        public Guid VehicleID { get; set; }

        //        [DatabaseField("ContactCompanyID")]
        //        public Guid ContactCompanyID { get; set; }
        //
        //        public string DisplayName
        //        {
        //            get { return FirstName + " " + LastName+","+ VehicleName+" "+VehicleRegistration; }
        //        }
    }
}
