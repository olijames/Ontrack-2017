using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataAccess
{
    public class DAGeneral : DABase
    {
        public DAGeneral(string ConnectionString)
            : base(ConnectionString)
        { }

        /// <summary>
        /// Deletes all data from the database except for general settings and required contacts.
        /// </summary>
        public void DeleteAllData()
        {
            string Query =
@"
delete from taskpendingcontractor
delete from taskmaterial
delete from tasklabour
delete from labour
delete from material
delete from tasklabourcategory
delete from labourcategory
delete from materialcategory
delete from sitevisibility
delete from taskacknowledgement
delete from jobquote
delete from jobtimesheet
delete from jobfile
delete from fileupload
delete from employeeinfo
delete from contactcompany
delete from contactcustomer
delete from contactsite
delete from customerinvitation
delete from jobchange
delete from taskquote
delete from taskcompletion
delete from task
delete from jobcontractor
delete from job
delete from site
delete from customer
delete from contact where contactid <> '00000000-0000-0000-0000-000000000000' and contactid <> 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF'";
            ExecuteQuery(Query);
        }

    }
}
