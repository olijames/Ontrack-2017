using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("EmployeeInfo")]
    public class DOEmployeeInfo : DOContactBase
    {

        //added by Jared
        //[Flags]
        //public enum EmployeeSettingsEnum
        //{

        //    CompanyMaximisedInHomeScreen = 1,
        //    CompanyAtTop = 2,
        //    CompanyAtBottom = 4

        //}

        //[Flags]
        //public enum EmployeeRightsEnum
        //{

        //    ViewAllTimeSheets = 1,
        //    ViewMyTeamsTimeSheets = 2,
        //    ViewEmployeeInfo = 4,
        //    ModifyAndAddEmployeeDetails = 8,
        //    EnterPromoteBusiness = 16,
        //    ViewJobSummary = 32,
        //    ViewTaskSummary = 64,
        //    AddLabourMaterials = 128,
        //    AddOrEditSites = 256,
        //    AddOrEditJobs = 512,
        //    AddOrEditTasks = 1024,
        //    DeleteJobs = 2048,
        //    DeleteTasks = 4096,
        //    ViewCostsAndRatesForTasks = 8192,
        //    DeleteCustomer = 16384,
        //    InvoiceTask = 32768,
        //    EditPermissions = 65536,
        //    AcceptQuotes = 131072,
        //    CreateQuotes = 262144
        //    ReadTaskValues = 524288,   //for quoted/invoiceable tasks labour cost/chargeout, material cost/chargeout to see profit per task
        //    ?  = 1048576




        //}
        //added above J


        [DatabaseField("EmployeeID", IsPrimaryKey = true)]
        public Guid EmployeeID { get; set; }

        [DatabaseField("ContactCompanyID")]
        public Guid ContactCompanyID { get; set; }

        //[DatabaseField("Settings")]
        //public EmployeeSettingsEnum Settings { get; set; }

        //[DatabaseField("Rights")]
        //public EmployeeRightsEnum Rights { get; set; }

        //[DatabaseField("Rate")]
        //public decimal Rate { get; set; }
       

        [DatabaseField("PayRate")]
        public decimal PayRate { get; set; }

        [DatabaseField("LabourRate")]
        public decimal LabourRate { get; set; }

        [DatabaseField("AccessFlags")]
        public long AccessFlags { get; set; }

        //Tony added 21.Feb.2017
        [DatabaseField("DefaultVehicleId")]
        public Guid DefaultVehicleId { get; set; }

        public override Guid ID
        {
            get {
                return EmployeeID;
            }
        }
        public override string ContactBaseType
        {
            get { return "Employee"; }
        }

        public override string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
