using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Vehicle")]
    public class DOVehicle : DOBase
    {
        [DatabaseField("VehicleID", IsPrimaryKey = true)]
        public Guid VehicleID { get; set; }

        [DatabaseField("VehicleOwner")]
        public Guid VehicleOwner { get; set; }

        [DatabaseField("vehicleDriver")]
        public Guid vehicleDriver { get; set; }

        [DatabaseField("VehicleName")]
        public string VehicleName { get; set; }

        [DatabaseField("VehicleRegistration")]
        public string VehicleRegistration{ get; set; }

        [DatabaseField("WOFDueDate")]
        public DateTime WOFDueDate { get; set; }

        [DatabaseField("RegoDueDate")]
        public DateTime RegoDueDate { get; set; }

        [DatabaseField("InsuranceDueDate")]
        public DateTime InsuranceDueDate { get; set; }
        

    }
}
