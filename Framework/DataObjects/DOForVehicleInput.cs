using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    //Added by Martin Falconer 29/06
    //Data object for VehicleInput.aspx where Vehicle and driver of vehicle mashed into
    //one object for display puposes of GridView.
    /*Vehicle.
        VehicleName
        VehicleRegistration
        WOFDueDate
        RegoDueDate
        InsuranceDueDate
        Active
      Contact.FirstName        
    */

    [DatabaseTable("")]
    public class DOForVehicleInput : DOBase
    {
        [DatabaseField("VehicleID", IsPrimaryKey = true)]
        public Guid VehicleID { get; set; }

        [DatabaseField("VehicleDriver")]
        public Guid VehicleDriver { get; set; }

        [DatabaseField("VehicleRegistration")]
        public string VehicleRegistration { get; set; }

        [DatabaseField("VehicleName")]
        public string VehicleName { get; set; }

        [DatabaseField("WOFDueDate")]
        public DateTime WOFDueDate { get; set; }

        [DatabaseField("RegoDueDate")]
        public DateTime RegoDueDate { get; set; }

        [DatabaseField("InsuranceDueDate")]
        public DateTime InsuranceDueDate { get; set; }
        
        //Prop from the Contact table for the driver's name
        [DatabaseField("FirstName")]
        public string DriverName { get; set; }

        
        public string EmployeeAndVehicle
        {
            get
            {
                    return string.Format("{0}, {1}, {2}", DriverName, VehicleName, VehicleRegistration);
            }

        }



    }
}
