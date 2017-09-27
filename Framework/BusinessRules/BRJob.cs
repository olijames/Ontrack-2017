using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using System.Web;
using System.Drawing;
using System.Data.SqlClient; //jared added from here
using System.Data;
using System.Reflection;


namespace Electracraft.Framework.BusinessRules
{
    public class BRJob : BRBase
    {
        private DAJob _CurrentDAJob;
        private DAJob CurrentDAJob
        {
            get
            {
                if (_CurrentDAJob == null)
                    _CurrentDAJob = new DAJob(ConnectionString);
                return _CurrentDAJob;
            }
        }

        private BRContact _CurrentBRContact;
        private BRContact CurrentBRContact
        {
            get
            {
                if (_CurrentBRContact == null)
                    _CurrentBRContact = new BRContact();
                return _CurrentBRContact;
            }
        }

        /// <summary>
        /// Creates a new job.
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOJob CreateJob(Guid SiteID, Guid CreatedBy)
        {
            DOJob Job = new DOJob();
            Job.JobID = Guid.NewGuid();
            Job.JobType = DOJob.JobTypeEnum.ChargeUp;
            Job.CreatedBy = CreatedBy;
            Job.CompletedDate = DateAndTime.NoValueDate;
            Job.SiteID = SiteID;
            Job.ProjectManagerID = Constants.Guid_DefaultUser;
            Job.InvoiceTo = Constants.Guid_DefaultUser;
            Job.NoPoweredItems = true;
            return Job;
        }


        /// <summary>
        /// Saves a job.
        /// </summary>
        /// <param name="Job"></param>
        public void SaveJob(DOJob Job)
        {
            CurrentDAJob.SaveObject(Job);
        }

        /// <summary>
        /// Saves a invoice.
        /// </summary>
        /// <param name="Job"></param>
        public void SaveInvoice(DOInvoice Invoice)
        {
            CurrentDAJob.SaveObject(Invoice);
        }





        /// <summary>
        /// Deletes a job. There must be no objects in other tables linking to this job.
        /// </summary>
        /// <param name="Job"></param>
        public void DeleteJob(DOJob Job)
        {
            CurrentDAJob.DeleteObject(Job);
        }


        /// <summary>
        /// Deletes an invoice. There must be no objects in other tables linking to this invoice.
        /// </summary>
        /// <param name="Job"></param>
        public void DeleteInvoice(DOInvoice Invoice)
        {
            CurrentDAJob.DeleteObject(Invoice);
        }




        public void UpdateTask(DOTask task)
        {
            string query = @"update task set startdate={1},enddate={2} where taskid={0}";
            CurrentDAJob.ExecuteScalar(query, task.TaskID, task.StartDate, task.EndDate);
        }

        public void UpdateQueryTask()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public DOJob SelectJob(Guid JobID)
        {
            return CurrentDAJob.SelectObject(typeof(DOJob), "JobID = {0}", JobID) as DOJob;
        }

        /// <summary>
        /// Selects an invoice.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public DOInvoice SelectInvoice(Guid InvoiceID)
        {
            return CurrentDAJob.SelectObject(typeof(DOInvoice), "InvoiceID = {0}", InvoiceID) as DOInvoice;
        }

        public DOJobContractor FindJob(int JobNumberAuto, Guid ContactID)
        {
            //DOJob Job;
            //  string query = "SELECT * FROM Job WHERE Job.JobNumberAuto={0}";
            return CurrentDAJob.SelectObject(typeof(DOJobContractor), "JobNumberAuto={0} and contactid={1}", JobNumberAuto, ContactID) as DOJobContractor;

        }



        //6.6.17 Jared replaced with above
        public DOJob FindJobOld(int JobNumberAuto)
        {
            //DOJob Job;
            //  string query = "SELECT * FROM Job WHERE Job.JobNumberAuto={0}";
            return CurrentDAJob.SelectObject(typeof(DOJob), "JobNumberAuto={0}", JobNumberAuto) as DOJob;

        }
        //eob

        /// <summary>
        /// Selects jobs for a site.
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobs(Guid SiteID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJob), "SiteID = {0}", SiteID);
        }

        //Tony added 11.11.2016
        public DOJobInfo SelectJobsInfo(Guid SiteID)
        {
            return CurrentDAJob.SelectObject(typeof(DOJobInfo), "SiteID = {0}", SiteID) as DOJobInfo;
        }
        //Tony added 11.11.2016

        /// <summary>
        /// Select jobs the contact has access to view.
        /// </summary>
        /// <param name="SiteID">The site ID.</param>
        /// <param name="ContactID">The contact.</param>
        /// <returns>The jobs that the contact can view for this site.</returns>
        public List<DOBase> SelectViewableJobs(DOSite Site, Guid ContactID)
        {
            //If you are the customer who owns the site you can see all jobs.
            //Otherwise, you can only see jobs where:
            //- you are the owner of the job.
            //- you are a contractor of the job or a task of the job.
            /***********Commented out to use job contractor table always***************/
            //Check if the viewing contact is the site customer. ContactID below is logged in user(or their company)

            //if (Site.ContactID == ContactID)
            //    return SelectJobs(Site.SiteID);
            /*******************************/
            //            string Query =
            //@"WITH JobIDs AS ( 
            //SELECT DISTINCT j.JobID FROM Site s
            //LEFT JOIN Job j ON s.SiteID = j.SiteID
            //LEFT JOIN JobContractor jc ON j.JobID = jc.JobID
            //LEFT JOIN Task t ON j.JobID = t.JobID
            //WHERE s.SiteID = {0} 
            //AND (j.JobOwner = {1} OR jc.ContactID = {1} OR t.ContractorID = {1}))
            //SELECT j.* FROM Job j INNER JOIN JobIDs ON j.JobID = JobIDs.JobID order by JobStatus";

            //New query using JobContractor functionality
            string query =
                @"With JobIds as (select JobID from Job where SiteID = {0}) 
select j.JobID,jc.status,j.JobNumberAuto,j.Name,j.JobType,j.SiteID,j.JobOwner,j.ProjectManagerID,
j.CompletedDate,j.CompletedBy,j.IncompleteTasksReason,j.Description,j.CreatedBy,j.CreatedDate,j.Active,j.NoPoweredItems,j.SiteNotes,j.StockRequired,
j.ProjectManagerText,j.ProjectManagerPhone from 
Job j inner join JobIds on j.JobID = JobIds.JobID 
left join JobContractor jc on jc.JobID = j.JobID 
where jc.ContactID = {1}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOJobInfo), query, Site.SiteID, ContactID);

        }
        /// <summary>
        /// Select jobs the contact has access to view.
        /// </summary>
        /// <param name="SiteID">The site ID.</param>
        /// <param name="ContactID">The contact.</param>
        /// <returns>The jobs that the contact can view for this site.</returns>
        public List<DOBase> SelectViewableJobs(Guid siteID, Guid ContactID, Guid LoggedInContact)
        {
            //If you are the customer who owns the site you can see all jobs.
            //Otherwise, you can only see jobs where:
            //- you are the owner of the job.
            //- you are a contractor of the job or a task of the job.
            // DOSite Site = CurrentDAJob.SelectObject(typeof(DOSite), "SiteID={0}", siteID) as DOSite;
            //DOSite Site = CurrentDAJob.SelectObject(typeof(DOSite), "SiteID={0}", siteID) as DOSite;
            //Check if the viewing contact is the site customer. ContactID below is logged in user(or their company)
            //if (Site.ContactID == ContactID)
            //    return SelectJobs(Site.SiteID);

            //            string Query =
            //@"WITH JobIDs AS ( 
            //SELECT DISTINCT j.JobID FROM Site s
            //LEFT JOIN Job j ON s.SiteID = j.SiteID
            //LEFT JOIN JobContractor jc ON j.JobID = jc.JobID
            //LEFT JOIN Task t ON j.JobID = t.JobID
            //WHERE s.SiteID = {0} 
            //AND (jc.ContactID = {1}))
            //SELECT distinct j.JobID,j.JobNumberAuto,j.Name,j.JobType,j.SiteID,j.JobOwner,j.ProjectManagerID,
            //j.CompletedDate,j.CompletedBy,j.IncompleteTasksReason,j.Description,j.CreatedBy,j.CreatedDate,j.Active,j.NoPoweredItems,j.SiteNotes,j.StockRequired,
            //j.ProjectManagerText,j.ProjectManagerPhone,jc.status FROM Job j INNER JOIN JobIDs ON j.JobID = JobIDs.JobID join JobContractor jc on 
            //jc.JobID=JobIDs.JobID where j.active=1";

            //Fixed corrupted query
            string Query = @"select distinct j.JobID,j.JobNumberAuto,j.Name,j.JobType,j.SiteID,j.JobOwner,j.ProjectManagerID,
j.CompletedDate,j.CompletedBy,j.IncompleteTasksReason,j.Description,j.CreatedBy,j.CreatedDate,j.Active,j.NoPoweredItems,j.SiteNotes,j.StockRequired,
j.ProjectManagerText,j.ProjectManagerPhone,jc.status,j.PoweredItems from Job j
join JobContractor jc
on jc.JobID=j.JobID
where j.SiteID={0} and jc.ContactID={1}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOJobInfo), Query, siteID, ContactID, LoggedInContact);

        }
        /// <summary>
        /// Select jobs the contact has access to view on a contact level.
        /// </summary>
        /// <param name="CustomerID">The contact to view jobs for.</param>
        /// <param name="ContactID">The contact viewing the jobs.</param>
        /// <returns>The jobs that the contact can view for this site.</returns>
        public List<DOBase> SelectViewableJobsAllSites(Guid CustomerID, Guid ContactID)
        {

            string Query =
@"WITH JobIDs AS (
SELECT DISTINCT j.JobID FROM Site s
LEFT JOIN Job j ON s.SiteID = j.SiteID
LEFT JOIN JobContractor jc ON j.JobID = jc.JobID
LEFT JOIN Task t ON j.JobID = t.JobID
WHERE (s.ContactID = {0} OR ({0} = {1}))
AND (j.JobOwner = {1} OR jc.ContactID = {1} OR t.ContractorID = {1}))
SELECT j.* FROM Job j INNER JOIN JobIDs ON j.JobID = JobIDs.JobID";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOJob), Query, CustomerID, ContactID);

        }

        /// <summary>
        /// Selects the active jobs that a contact is the job owner for.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectActiveJobs(Guid ContactID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJob), "JobOwner = {0} AND Active = 1 AND JobStatus = 0", ContactID);

        }


        /// <summary>
        /// Selects the active jobs for a site.
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public List<DOBase> SelectActiveJobsForSite(Guid siteID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJob), "JobStatus = 0 and SiteID={0}", siteID);

        }


        #region Job Contractors
        /// <summary>
        /// Assigns a contractor to a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="ContractorID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOJobContractor CreateJobContractor(Guid JobID, Guid ContractorID, Guid CreatedBy)
        {
            DOJobContractor JobContractor = new DOJobContractor();
            JobContractor.JobContractorID = Guid.NewGuid();
            JobContractor.JobID = JobID;
            JobContractor.ContactID = ContractorID;
            JobContractor.CreatedBy = CreatedBy;

            return JobContractor;
        }

        /// <summary>
        /// Saves a job contractor.
        /// </summary>
        /// <param name="JobContractor"></param>
        public void SaveJobContractor(DOJobContractor JobContractor)
        {
            CurrentDAJob.SaveObject(JobContractor);
        }

        public void SaveSupplier(DOSupplier supplier)
        {
            System.Diagnostics.Debug.WriteLine(supplier.SupplierID);
            CurrentDAJob.SaveObject(supplier);

        }
        public void SaveSupplierInvoice(DOSupplierInvoice SupplierInvoice)
        {
            CurrentDAJob.SaveObject(SupplierInvoice);
        }


        /// <summary>
        /// Selects all active invoices for a task.
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public List<DOBase> SelectContractorTaskInvoices(Guid TaskID, Guid ContractorID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                @"select [InvoiceID],[CustomerID],[ContractorID],[InvoiceStatus],[DueDate],[Terms],[invoicedescription],[SubmissionDetailLevel],[CreatedBy],[CreatedDate],[Active],[TaskID],[sentdate] from invoice where taskID={0} and invoice.contractorid={1}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOInvoice), query, TaskID, ContractorID);

        }


        /// <summary>
        /// Selects all customers with uninvoiced TM's or TL's for a contractor
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public List<DOBase> SelectInvoiceableTasks(Guid ContractorID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                // Tony modified on 6.Mar.2017
                //            @"select distinct c.[ContactID]
                //      ,c.[Username] ,c.[PasswordHash] ,c.[FirstName]  ,c.[LastName]  ,c.[CompanyName]  ,c.[Email]  ,c.[Phone]  ,c.[Address1]  ,c.[Address2]  ,c.[CreatedBy]  ,c.[CreatedDate],c.[Active],c.[ContactType]
                //      ,c.[BankAccount] ,c.[SubscriptionExpiryDate]  ,c.[SubscriptionPending]  ,c.[Subscribed]  ,c.[ManagerID]  ,c.[CompanyKey]  ,c.[PendingUser]  ,c.[CustomerExclude]   ,c.[DefaultQuoteRate]
                //      ,c.[DefaultChargeUpRate]  ,c.[JobNumberAuto]  ,c.[Address3]  ,c.[Address4]  ,c.[iCount]   ,c.[Searchable]  ,c.[PendingSiteOwner]  ,c.[DefaultRegion] 
                //        from TaskMaterial TM, contact c, TaskLabour TL, Task T, job j where t.ContractorID={0} and t.JobID=j.jobid and tl.taskid=t.TaskID 
                //                and tm.TaskID=t.TaskID and t.status!=2 and c.ContactID=t.TaskCustomerID and (tl.InvoiceID='00000000-0000-0000-0000-000000000000' or tm.InvoiceID='00000000-0000-0000-0000-000000000000') 
                //                and (tm.active=1 or tl.active=1)";
                @"SELECT  SUM(Total) Total, ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy,
                          CreatedDate, Active 
                    FROM(SELECT tl.CustomerID, tl.InvoiceQuantity*{1} Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName, c.Email,
                                c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.CreatedBy, c.CreatedDate, c.Active
                           FROM TaskLabour tl, Contact c, Task t, Job j
                          WHERE tl.ContractorID = {0} AND t.JobID = j.JobID AND tl.TaskID = t.TaskID AND tl.CustomerID = c.ContactID AND t.Status != 2
                            AND tl.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tl.Active = 1
                        UNION ALL
                         SELECT tm.CustomerID, tm.InvoiceQuantity* tm.SellPrice Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName,
                                c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone,  c.CreatedBy, c.CreatedDate, c.Active
                           FROM TaskMaterial tm, Contact c, Task t
                          WHERE t.ContractorID = {0} AND t.JobID = j.JobID AND tm.TaskID = t.TaskID AND tm.CustomerID = c.ContactID AND t.Status != 2
                            AND tm.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tm.Active = 1) t
                       GROUP BY ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy, CreatedDate, Active
                       ORDER BY FirstName, CompanyName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOContact), query, ContractorID);

        }

        /// <summary>
        /// Selects all customers with uninvoiced TM's or TL's for a contractor
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public List<DOBase> SelectCustomersWithInvoiceableTasks(Guid ContractorID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =

            @"select c.* from TaskLabour tl,contact c where tl.ContractorID = {0} and tl.CustomerID = c.ContactID and tl.InvoiceID = '00000000-0000-0000-0000-000000000000' union select c.* from TaskMaterial tm, contact c where tm.ContractorID = {0} and tm.CustomerID = c.ContactID and tm.InvoiceID = '00000000-0000-0000-0000-000000000000'";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOContact), query, ContractorID);

        }



        /// <summary>
        /// Selects all active tasks for contractor and customer and job info. 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskJobsContractorCustomer(Guid ContractorID, decimal LabourRate, Guid CustomerID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =


            //    @"select  task.* from Task, TaskMaterial where task.status!=2 and task.taskcustomerID = {1} and TaskMaterial.InvoiceID='00000000-0000-0000-0000-000000000000'  and TaskMaterial.TaskID = task.taskid union select task.* from Task, Tasklabour where task.status!=2 and task.taskcustomerID = {1} and  TaskLabour.InvoiceID='00000000-0000-0000-0000-000000000000' and TaskLabour.TaskID = task.taskid order by task.jobid, task.taskname";



            //         @" select t1.TaskID,  t1.TotalMaterial, t2.TotalLabour, t1.jobid, t1.JobNumberAuto, t1.tasknumber, t1.Name, t1.TaskCustomerID, t1.TaskName, t1.status, t1.createdby, t1.createddate, t1.active 
            // from(select tm.taskID as TaskID, sum(tm.InvoiceQuantity* sellprice) as TotalMaterial, 0 as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
            // from TaskMaterial TM, Task T, job j
            // where t.ContractorID={0} and t.JobID= j.jobid and tm.TaskID= t.TaskID and t.status!=2 and tm.InvoiceID= '00000000-0000-0000-0000-000000000000' and tm.active= 1 and
            // t.taskcustomerid= {2} group by tm.TaskID, t.TaskCustomerID, t.TaskName, j.JobNumberAuto, j.Name, t.TaskID, t.TaskNumber, t.Status, j.jobid,
            // j.CreatedBy, j.CreatedDate, j.Active) as t1
            // join(
            // select tl.TaskID as taskid, 0 as totalmaterial, sum(tl.InvoiceQuantity*{1}) as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
            //from  TaskLabour TL, Task T, job j
            // where t.ContractorID={0} and t.JobID= j.jobid and tl.taskid= t.TaskID and t.status!=2 and tl.InvoiceID= '00000000-0000-0000-0000-000000000000' and tl.active= 1 and
            // t.taskcustomerid= {2} group by tl.taskid, t.TaskCustomerID, t.TaskName, j.JobNumberAuto, j.Name, t.TaskID, t.TaskNumber, t.Status, j.jobid,
            // j.CreatedBy, j.CreatedDate, j.Active) as t2
            // on t1.taskid=t2.taskid";

            @"select t1.TaskID,  sum(t1.TotalMaterial) TotalMaterial, sum(t1.TotalLabour) TotalLabour, t1.jobid, t1.JobNumberAuto, t1.tasknumber, t1.Name, t1.TaskCustomerID, t1.TaskName, t1.status, t1.createdby, t1.createddate, t1.active
              from
              (select tm.taskID as TaskID, (tm.InvoiceQuantity * sellprice) as TotalMaterial, 0 as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
              from TaskMaterial TM, Task T, job j
              where t.ContractorID = {0} and t.JobID = j.jobid and tm.TaskID = t.TaskID and t.status != 2 and tm.InvoiceID = '00000000-0000-0000-0000-000000000000' and tm.active = 1 and
              t.taskcustomerid = {2}
              union all
              select tl.TaskID as taskid, 0 as totalmaterial, (tl.InvoiceQuantity * {1}) as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
              from TaskLabour TL, Task T, job j
              where t.ContractorID = {0} and t.JobID = j.jobid and tl.taskid = t.TaskID and t.status != 2 and tl.InvoiceID = '00000000-0000-0000-0000-000000000000' and tl.active = 1 and
              t.taskcustomerid = {2}
	          ) t1
              group by t1.TaskID,   t1.jobid, t1.JobNumberAuto, t1.tasknumber, t1.Name, t1.TaskCustomerID, t1.TaskName, t1.status, t1.createdby, t1.createddate, t1.active
              order by t1.jobid, t1.taskid";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskJob), query, ContractorID, LabourRate, CustomerID);

        }


        /// <summary>
        /// Selects all active tasks for contractor and customer. 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DOBase> SelectTasksContractorCustomer(Guid ContractorID, decimal LabourRate, Guid CustomerID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =


            //    @"select  task.* from Task, TaskMaterial where task.status!=2 and task.taskcustomerID = {1} and TaskMaterial.InvoiceID='00000000-0000-0000-0000-000000000000'  and TaskMaterial.TaskID = task.taskid union select task.* from Task, Tasklabour where task.status!=2 and task.taskcustomerID = {1} and  TaskLabour.InvoiceID='00000000-0000-0000-0000-000000000000' and TaskLabour.TaskID = task.taskid order by task.jobid, task.taskname";



            @" select t1.TaskID, '' as Name, '0' as jobnumberauto, t1.TotalMaterial, t2.TotalLabour, t1.jobid, t1.JobNumberAuto, t1.tasknumber, t1.Name, t1.TaskCustomerID, t1.TaskName, t1.status, t1.createdby, t1.createddate, t1.active 
    from(select tm.taskID as TaskID, sum(tm.InvoiceQuantity* sellprice) as TotalMaterial, 0 as TotalLabour, t.tasknumber, t.TaskCustomerID, t.TaskName, t.status, t1.createdby, t1.createddate, t1.active
    from TaskMaterial TM, Task T
    where t.ContractorID={0} and tm.TaskID= t.TaskID and t.status!=2 and tm.InvoiceID= '00000000-0000-0000-0000-000000000000' and tm.active= 1 and
    t.taskcustomerid= {2} group by tm.TaskID, t.TaskCustomerID, t.TaskName, t.TaskID, t.TaskNumber, t.Status, t1.CreatedBy, t1.CreatedDate, t1.Active, Name, jobautonumber) as t1
    join(
    select tl.TaskID as taskid, '' as Name, '0' as jobnumberauto,  0 as totalmaterial, sum(tl.InvoiceQuantity*{1}) as TotalLabour, t.tasknumber, t.TaskCustomerID, t.TaskName, t.status, t1.createdby, t1.createddate, t1.active
   from  TaskLabour TL, Task T
    where t.ContractorID={0} and tl.taskid= t.TaskID and t.status!=2 and tl.InvoiceID= '00000000-0000-0000-0000-000000000000' and tl.active= 1 and
    t.taskcustomerid= {2} group by tl.taskid, t.TaskCustomerID, t.TaskName, t.TaskID, t.TaskNumber, t.Status, t.CreatedBy, t.CreatedDate, t.Active, Name, jobautonumber) as t2
    on t1.taskid=t2.taskid";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskJob), query, ContractorID, LabourRate, CustomerID);

        }


        /// <summary>
        /// Selects all tasks for contractor
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DOBase> SelectAllContractorCustomersWithValue(Guid ContractorID, decimal LabourRate)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                        //duplicates:
                        //              @" select sum(tl.invoicequantity*{1}) as Total ,c.contactid,c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active from TaskLabour tl,contact c where tl.ContractorID = {0} and tl.CustomerID = c.ContactID and tl.InvoiceID = '00000000-0000-0000-0000-000000000000' and tl.active=1 group by c.ContactID, c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active 
                        //	union 
                        //	select sum(tm.invoicequantity*tm.SellPrice) as Total, c.contactid,c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active  from TaskMaterial tm, contact c where tm.ContractorID = {0} and tm.CustomerID = c.ContactID and tm.InvoiceID = '00000000-0000-0000-0000-000000000000' and tm.active=1 group by c.ContactID, c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active 
                        //";
                        //3/7/17 jared SOB. Altered because t.customerid now points to contractorcustomer.contactcustomerid instead on contact.contactid
                        //    // Tony fixed bug on 6.Mar.2017 : total amount now matches with subtotal of child items
                        //    @"SELECT  SUM(Total) Total, ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy,
                        //      CreatedDate, Active 
                        //FROM(SELECT t.TaskCustomerID,tl.CustomerID, tl.InvoiceQuantity*{1} Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName, c.Email,
                        //            c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.CreatedBy, c.CreatedDate, c.Active
                        //       FROM TaskLabour tl, Contact c, Task t, Job j
                        //      WHERE tl.ContractorID = {0} AND t.JobID = j.JobID AND tl.TaskID = t.TaskID AND t.taskCustomerID = c.ContactID AND t.Status != 2
                        //        AND tl.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tl.Active = 1
                        //    UNION ALL
                        //     SELECT t.TaskCustomerID, tm.CustomerID, tm.InvoiceQuantity* tm.SellPrice Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName,
                        //            c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone,  c.CreatedBy, c.CreatedDate, c.Active
                        //       FROM TaskMaterial tm, Contact c, Task t,Job j
                        //      WHERE t.ContractorID = {0} AND t.JobID = j.JobID AND tm.TaskID = t.TaskID AND t.TaskCustomerID = c.ContactID AND t.Status != 2
                        //        AND tm.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tm.Active = 1) t
                        //   GROUP BY t.taskCustomerID, ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy, CreatedDate, Active
                        //   ORDER BY FirstName, CompanyName";
                        //    
                        @"SELECT  SUM(Total) Total, ContactCustomerID, CustomerType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy,
                              CreatedDate, Active 
                        FROM(SELECT t.TaskCustomerID,tl.CustomerID, tl.InvoiceQuantity*{1} Total, c.ContactCustomerID, c.CustomerType, c.FirstName, c.LastName, c.CompanyName, c.Email,
                                    c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.CreatedBy, c.CreatedDate, c.Active
                               FROM TaskLabour tl, ContractorCustomer c, Task t, Job j
                              WHERE tl.ContractorID = {0} AND t.JobID = j.JobID AND tl.TaskID = t.TaskID AND t.taskCustomerID = c.ContactCustomerID AND t.Status != 2
                                AND tl.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tl.Active = 1
                            UNION ALL
                             SELECT t.TaskCustomerID, tm.CustomerID, tm.InvoiceQuantity* tm.SellPrice Total, c.ContactCustomerID, c.CustomerType, c.FirstName, c.LastName, c.CompanyName,
                                    c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone,  c.CreatedBy, c.CreatedDate, c.Active
                               FROM TaskMaterial tm, ContractorCustomer c, Task t,Job j
                              WHERE t.ContractorID = {0} AND t.JobID = j.JobID AND tm.TaskID = t.TaskID AND t.TaskCustomerID = c.ContactCustomerID AND t.Status != 2
                                AND tm.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tm.Active = 1) t
                           GROUP BY t.taskCustomerID, ContactCustomerID, CustomerType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy, CreatedDate, Active
                           ORDER BY FirstName, CompanyName";


            //    @"select  task.* from Task, TaskMaterial where task.status!=2 and task.taskcustomerID = {1} and TaskMaterial.InvoiceID='00000000-0000-0000-0000-000000000000'  and TaskMaterial.TaskID = task.taskid union select task.* from Task, Tasklabour where task.status!=2 and task.taskcustomerID = {1} and  TaskLabour.InvoiceID='00000000-0000-0000-0000-000000000000' and TaskLabour.TaskID = task.taskid order by task.jobid, task.taskname";



            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOContactWithJobValue), query, ContractorID, LabourRate);

        }




        /// <summary>
        /// Selects all tasks for contractor
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DOBase> SelectAllContactsWithValue(Guid ContractorID, decimal LabourRate)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                            //duplicates:
                            //              @" select sum(tl.invoicequantity*{1}) as Total ,c.contactid,c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active from TaskLabour tl,contact c where tl.ContractorID = {0} and tl.CustomerID = c.ContactID and tl.InvoiceID = '00000000-0000-0000-0000-000000000000' and tl.active=1 group by c.ContactID, c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active 
                            //	union 
                            //	select sum(tm.invoicequantity*tm.SellPrice) as Total, c.contactid,c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active  from TaskMaterial tm, contact c where tm.ContractorID = {0} and tm.CustomerID = c.ContactID and tm.InvoiceID = '00000000-0000-0000-0000-000000000000' and tm.active=1 group by c.ContactID, c.ContactType,c.FirstName,c.LastName,c.CompanyName, c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.createdby, c.createddate, c.active 
                            //";
                            //3 / 7 / 17 jared SOB. Altered because t.customerid now points to contractorcustomer.contactcustomerid instead on contact.contactid
                            // Tony fixed bug on 6.Mar.2017 : total amount now matches with subtotal of child items
                            @"SELECT  SUM(Total) Total, ContactID as ContactCustomerID, ContactType as CustomerType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy,
                              CreatedDate, Active 
                        FROM(SELECT t.TaskCustomerID,tl.CustomerID, tl.InvoiceQuantity*{1} Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName, c.Email,
                                    c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.CreatedBy, c.CreatedDate, c.Active
                               FROM TaskLabour tl, Contact c, Task t, Job j
                              WHERE tl.ContractorID = {0} AND t.JobID = j.JobID AND tl.TaskID = t.TaskID AND t.taskCustomerID = c.ContactID AND t.Status != 2
                                AND tl.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tl.Active = 1
                            UNION ALL
                             SELECT t.TaskCustomerID, tm.CustomerID, tm.InvoiceQuantity* tm.SellPrice Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName,
                                    c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone,  c.CreatedBy, c.CreatedDate, c.Active
                               FROM TaskMaterial tm, Contact c, Task t,Job j
                              WHERE t.ContractorID = {0} AND t.JobID = j.JobID AND tm.TaskID = t.TaskID AND t.TaskCustomerID = c.ContactID AND t.Status != 2
                                AND tm.InvoiceID = '00000000-0000-0000-0000-000000000000' AND tm.Active = 1) t
                           GROUP BY t.taskCustomerID, ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, CreatedBy, CreatedDate, Active
                           ORDER BY FirstName, CompanyName";

            

            //    @"select  task.* from Task, TaskMaterial where task.status!=2 and task.taskcustomerID = {1} and TaskMaterial.InvoiceID='00000000-0000-0000-0000-000000000000'  and TaskMaterial.TaskID = task.taskid union select task.* from Task, Tasklabour where task.status!=2 and task.taskcustomerID = {1} and  TaskLabour.InvoiceID='00000000-0000-0000-0000-000000000000' and TaskLabour.TaskID = task.taskid order by task.jobid, task.taskname";



            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOContactWithJobValue), query, ContractorID, LabourRate);

        }



        /// <summary>
        /// Selects all active tasks for contractor / customer
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DOBase> SelectAllTaskJobs(Guid ContractorID, decimal LabourRate)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =


              @" select t1.TaskID,  t1.TotalMaterial, t2.TotalLabour, t1.jobid, t1.JobNumberAuto, t1.tasknumber, t1.Name, t1.TaskCustomerID, t1.TaskName, t1.status, t1.createdby, t1.createddate, t1.active 
    from(select tm.taskID as TaskID, sum(tm.InvoiceQuantity* sellprice) as TotalMaterial, 0 as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
    from TaskMaterial TM, Task T, job j
    where t.ContractorID={0} and t.JobID= j.jobid and tm.TaskID= t.TaskID and t.status!=2 and tm.InvoiceID= '00000000-0000-0000-0000-000000000000' and tm.active= 1  
	group by tm.TaskID, t.TaskCustomerID, t.TaskName, j.JobNumberAuto, j.Name, t.TaskID, t.TaskNumber, t.Status, j.jobid,
    j.CreatedBy, j.CreatedDate, j.Active) as t1
    
	
	join(
    select tl.TaskID as taskid, 0 as totalmaterial, sum(tl.InvoiceQuantity*65) as TotalLabour, j.jobid, j.JobNumberAuto, t.tasknumber, j.Name, t.TaskCustomerID, t.TaskName, t.status, j.createdby, j.createddate, j.active
   from  TaskLabour TL, Task T, job j
    where t.ContractorID={0} and t.JobID= j.jobid and tl.taskid= t.TaskID and t.status!=2 and tl.InvoiceID= '00000000-0000-0000-0000-000000000000' and tl.active= 1 
    group by tl.taskid, t.TaskCustomerID, t.TaskName, j.JobNumberAuto, j.Name, t.TaskID, t.TaskNumber, t.Status, j.jobid,
    j.CreatedBy, j.CreatedDate, j.Active) as t2
    on t1.taskid=t2.taskid";

            //    @"select  task.* from Task, TaskMaterial where task.status!=2 and task.taskcustomerID = {1} and TaskMaterial.InvoiceID='00000000-0000-0000-0000-000000000000'  and TaskMaterial.TaskID = task.taskid union select task.* from Task, Tasklabour where task.status!=2 and task.taskcustomerID = {1} and  TaskLabour.InvoiceID='00000000-0000-0000-0000-000000000000' and TaskLabour.TaskID = task.taskid order by task.jobid, task.taskname";



            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskJob), query, ContractorID, LabourRate);

        }


        public DOInvoice SaveInvoice(DOTask myTask, DateTime PaymentDate, DOContact LoggedInUser)
        {//used
            DOInvoice myInvoice = new DOInvoice();
            myInvoice.InvoiceID = Guid.NewGuid();
            myInvoice.ContractorID = myTask.ContractorID; //todo confirm during creation of invoice
            myInvoice.CustomerID = myTask.TaskCustomerID; //todo confirm during creation of invoice
            myInvoice.DueDate = PaymentDate;
            myInvoice.InvoiceStatus = InvoiceStatusEnum.InProgress;
            myInvoice.Terms = "My Terms";
            myInvoice.DetailLevel = DetailLevelEnum.TotalsOnly;
            myInvoice.Description = "My Description";
            myInvoice.CreatedBy = LoggedInUser.ContactID;
            myInvoice.CreatedDate = DateTime.Now;
            myInvoice.Active = true;
            myInvoice.TaskID = myTask.TaskID;

            SaveInvoice(myInvoice);

            return myInvoice;

        }


        public void CreateInvoice(DOTask myTask, DOContact LoggedInUser, DateTime PaymentDate)
        {//used
            DateTime dt = DateTime.Today; //todo allow this to be changed
                                          //below is not working on the live site. There are two commands on this page that are not working live, but are local
                                          //DateTime PaymentDate = DateTime.Today;      //Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
                                          //DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
                                          //DOTask myTask = CurrentSessionContext.CurrentTask; //delete me when enable foreach task in job code

            List<DOBase> TaskMaterials = SelectTaskMaterialsListNotInvoiced(myTask.TaskID);
            List<DOBase> TaskLabour = SelectTaskLaboursNotInvoiced(myTask.TaskID);
            if (TaskLabour.Count > 0 || TaskMaterials.Count > 0)
            {
                DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
                // intMaterialCount = 0;
                //  intLabourCount = 0;
                foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
                {
                    if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);

                    }
                }
                foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
                {

                    if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);


                    }
                }
            }
        }

        //Tony addded 18.Jan.2017
        //        public void CreateInvoice(DOTask myTask, DOContact LoggedInUser, DateTime PaymentDate, Guid[] materialIDs, Guid[] labourIDs)
        //        {//used
        //            DateTime dt = DateTime.Today; //todo allow this to be changed
        //                                          //below is not working on the live site. There are two commands on this page that are not working live, but are local
        //                                          //DateTime PaymentDate = DateTime.Today;      //Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
        //                                          //DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
        //                                          //DOTask myTask = CurrentSessionContext.CurrentTask; //delete me when enable foreach task in job code
        //
        //            // Deliver ID list of TaskMaterial and TaskLabour
        //            List<DOBase> TaskMaterials = SelectTaskMaterialsListNotInvoiced(myTask.TaskID, materialIDs);
        //
        //            List<DOBase> TaskLabour = SelectTaskLaboursNotInvoiced(myTask.TaskID, labourIDs);
        //
        //
        //            //Tony modified 18.Jan.2017
        //            if (TaskMaterials != null)
        //            {
        //                if (TaskMaterials.Count > 0)
        //                {
        //                    DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
        //                    // intMaterialCount = 0;
        //                    //  intLabourCount = 0;
        //                    foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
        //                    {
        //                        if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                        {
        //                            UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);
        //
        //                        }
        //                    }
        //                } 
        //            }
        //
        //            if (TaskLabour != null)
        //            {
        //                if (TaskLabour.Count > 0)
        //                {
        //                    DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
        //                    foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
        //                    {
        //
        //                        if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                        {
        //                            UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);
        //
        //
        //                        }
        //                    }
        //                }
        //            }
        ////            if (TaskLabour.Count > 0 || TaskMaterials.Count > 0)
        ////            {
        ////                DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
        ////                // intMaterialCount = 0;
        ////                //  intLabourCount = 0;
        ////                foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
        ////                {
        ////                    if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        ////                    {
        ////                        UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);
        ////
        ////                    }
        ////                }
        ////                foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
        ////                {
        ////
        ////                    if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        ////                    {
        ////                        UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);
        ////
        ////
        ////                    }
        ////                }
        ////            }
        //        }

        //Tony added 25-Jan-2017 
        public void CreateInvoice(DOTask myTask, DOContact LoggedInUser, DateTime PaymentDate, Guid[] materialIDs, Guid[] labourIDs)
        {//used
            DateTime dt = DateTime.Today; //todo allow this to be changed
                                          //below is not working on the live site. There are two commands on this page that are not working live, but are working local
                                          //DateTime PaymentDate = DateTime.Today;      //Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
                                          //DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
                                          //DOTask myTask = CurrentSessionContext.CurrentTask; //delete me when enable foreach task in job code

            // Deliver ID list of TaskMaterial and TaskLabour
            List<DOBase> TaskMaterials = SelectTaskMaterialsListNotInvoiced(myTask.TaskID, materialIDs);

            List<DOBase> TaskLabour = SelectTaskLaboursNotInvoiced(myTask.TaskID, labourIDs);

            //Tony added 24.Jan.2017
            if (TaskMaterials != null && TaskLabour != null) 
            {
                DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
               
                foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
                {
                    if (myTaskMaterial.Active == true &&
                        myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);
                    }
                }
                foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
                {

                    if (myTaskLabour.Active == true &&
                        myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);
                    }
                }
            }
            else if (TaskMaterials != null && TaskLabour == null)
            {
                DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
                // intMaterialCount = 0;
                //  intLabourCount = 0;
                foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
                {
                    if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);
                    }
                }
            }
            else if (TaskMaterials == null && TaskLabour != null)
            {
                DOInvoice myInvoice = SaveInvoice(myTask, PaymentDate, LoggedInUser);
                foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
                {

                    if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);
                    }
                }
            }
        }

        /// <summary>
        /// Selects all active invoices for a task.
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public List<DOBase> SelectCustomerTaskInvoices(Guid TaskID, Guid ContractorID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                @"select [InvoiceID],[CustomerID],[ContractorID],[InvoiceStatus],[DueDate],[Terms],[invoicedescription],[SubmissionDetailLevel],[CreatedBy],[CreatedDate],[Active],[TaskID],[sentdate] from invoice where taskID={0} and invoice.customerid={1}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOInvoice), query, TaskID, ContractorID);

        }



        /// <summary>
        /// Selects a job contractor.
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="ContractorID"></param>
        /// <returns></returns>
        public DOJobContractor SelectJobContractor(Guid JobID, Guid ContractorID)
        {
            string query = @"
  select JobContractorID,JobID,ContactID,CreatedBy,CreatedDate,Active,status, JobNumberAuto from jobcontractor where jobid={0} 
  and contactid={1}";
            //return CurrentDAJob.SelectObject(typeof(DOJobContractor), "JobID = {0} AND ContactID = {1}", JobID, ContractorID) as DOJobContractor;
            List<DOBase> list = CurrentDAJob.SelectObjectCustomQuery(typeof(DOJobContractor), query, JobID, ContractorID);
            if (list.Count != 0)
            {
                DOJobContractor jobContractor = list[0] as DOJobContractor;
                return jobContractor;
            }
            return null;

        }

        /// <summary>
        /// For the new task/job/customer update the jobcontractor,contactsite tables to make the jobs visible to all the related people
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="contractorID"></param>
        /// <param name="createdBy"></param>
        public void UpdateContactRecords(Guid jobID, Guid contractorId, Guid customerId, Guid createdBy, Guid Creator)
        {
            UpdateJobContractorRecord(jobID, contractorId, createdBy);
            UpdateJobContractorRecord(jobID, customerId, createdBy);
            UpdateJobContractorRecord(jobID, createdBy, createdBy);
            UpdateContractorCustomerRecord(contractorId, customerId, Creator);//added creator 2017.4.25 needs testing

        }


        /// <summary>
        /// Create/Update contractorCustomer records to build a relationship between contractor and the customers
        /// </summary>
        /// <param name="contractorId"></param>
        /// <param name="customerId"></param>
        private void UpdateContractorCustomerRecord(Guid contractorId, Guid customerId, Guid Creator)
        {
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(contractorId, customerId) ??
                                     CurrentBRContact.CreateContactCustomer(contractorId, customerId, Creator); //added creator 2017.4.25 needs testing
            contractorCustomer.Active = true;
            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
        }

        /// <summary>
        /// Update/Create the jobcontractor record
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="contractorId"></param>
        public void UpdateJobContractorRecord(Guid jobId, Guid contactId, Guid createdBy)
        {
            var jobContractor = SelectJobContractor(jobId, contactId) ??
                                CreateJobContractor(jobId, contactId, createdBy);
            jobContractor.Active = true;
            jobContractor.Status = 0;
            SaveJobContractor(jobContractor);
        }

        /// <summary>
        /// Deletes a job contractor.
        /// </summary>
        /// <param name="JobContractor"></param>
        public void DeleteJobContractor(DOJobContractor JobContractor)
        {
            CurrentDAJob.DeleteObject(JobContractor);
        }
        #endregion

        #region Tasks
        /// <summary>
        /// Creates a task.
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOTask CreateTask(Guid JobID, Guid CreatedBy)
        {
            DOTask Task = new DOTask();
            Task.TaskID = Guid.NewGuid();
            Task.JobID = JobID;
            Task.CreatedBy = CreatedBy;
            Task.TaskType = DOTask.TaskTypeEnum.Standard;
            Task.AmendedBy = Constants.Guid_DefaultUser;
            Task.AmendedDate = DateAndTime.NoValueDate;

            Task.Status = DOTask.TaskStatusEnum.Incomplete;
            Task.StartDate = DateAndTime.NoValueDate;
            Task.StartMinute = -1;
            Task.EndDate = DateAndTime.NoValueDate;
            Task.EndMinute = -1;

            return Task;
        }

        /// <summary>
        /// Selects a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DOTask SelectTask(Guid TaskID)
        {
            string query = @"select TaskID,JobID, QuoteID, InvoiceNumber, ContractorID,ParentTaskID,TaskName,TaskType,Description,CreatedBy,CreatedDate,Active,TaskOwner,
 Status,StartDate,StartMinute,EndDate,EndMinute,Appointment,AmendedTaskID,LMVisibility,InvoiceToType,AmendedBy,AmendedDate
 ,TradeCategoryID,TaskNumber,TaskInvoiceStatus,TaskCustomerID,SiteID
 from task WHERE TaskID = {0}";
            List<DOBase> doBaseObj = CurrentDAJob.SelectObjectCustomQuery(typeof(DOTask), query, TaskID);
            if (doBaseObj.Count > 0)
            {
                return doBaseObj[0] as DOTask;
            }
            else
            {
                return null;
            }
            // return doBaseObj[0] as DOTask;
            //return CurrentDAJob.SelectObject(typeof(DOTask), "TaskID = {0}", TaskID) as DOTask;
        }


        ///// <summary>
        ///// Selects a task.
        ///// </summary>
        ///// <param name="TaskID"></param>
        ///// <returns></returns>
        //public DOTaskJob SelectTaskJob(Guid TaskID)
        //{
        //    string query = @"select task.*, Job.Name as Name, Job.JobNumberAuto as JobNumberAuto from task,job WHERE TaskID = {0} and job.jobid=task.jobid ";
        //    List<DOBase> doBaseObj = CurrentDAJob.SelectObjectCustomQuery(typeof(DOTaskJob), query, TaskID);
        //    if (doBaseObj.Count > 0)
        //    {
        //        return doBaseObj[0] as DOTaskJob;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    // return doBaseObj[0] as DOTask;
        //    //return CurrentDAJob.SelectObject(typeof(DOTask), "TaskID = {0}", TaskID) as DOTask;
        //}

        /// <summary>
        /// Selects all active tasks for a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTasksByCustomerID(Guid ContactID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                @"select taskid, JobID, QuoteID, InvoiceNumber, ContractorID, ParentTaskID, TaskName, TaskType, Description, CreatedBy, CreatedDate, Active, TaskOwner
      , Status, StartDate, StartMinute, EndDate, EndMinute, Appointment, AmendedTaskID, LMVisibility, InvoiceToType, AmendedBy
      , AmendedDate, TradeCategoryID, TaskNumber, TaskInvoiceStatus, TaskCustomerID, SiteID from task where TaskCustomerID = {0} and status!=2 ORDER BY CreatedDate";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), query, ContactID);

        }




        /// <summary>
        /// Selects all active tasks for a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTasks(Guid JobID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                @"select taskid, JobID, QuoteID, InvoiceNumber, ContractorID, ParentTaskID, TaskName, TaskType, Description, CreatedBy, CreatedDate, Active, TaskOwner
      , Status, StartDate, StartMinute, EndDate, EndMinute, Appointment, AmendedTaskID, LMVisibility, InvoiceToType, AmendedBy
      , AmendedDate, TradeCategoryID, TaskNumber, TaskInvoiceStatus, TaskCustomerID, SiteID from task where JobID = {0} and status!=2 ORDER BY CreatedDate";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), query, JobID);

        }
        /// <summary>
        /// Selects all tasks including amended for a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectAllTasks(Guid JobID)
        {
            //return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} ORDER BY CreatedDate", JobID);
            string query =
                @"select taskid, JobID, QuoteID, InvoiceNumber, ContractorID, ParentTaskID, TaskName, TaskType, Description, CreatedBy, CreatedDate, Active, TaskOwner
      , Status, StartDate, StartMinute, EndDate, EndMinute, Appointment, AmendedTaskID, LMVisibility, InvoiceToType, AmendedBy
      , AmendedDate, TradeCategoryID, TaskNumber, TaskInvoiceStatus, TaskCustomerID, SiteID from task where JobID = {0} ORDER BY CreatedDate";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), query, JobID);

        }

        /// <summary>
        /// Generates the next task number
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public int GetTaskNumber(Guid jobId)
        {
            List<DOBase> tasksList = SelectTasks(jobId);
            int taskNumber = 0;
            if (tasksList.Count != 0)
            {
                DOTask firstTask = tasksList[0] as DOTask;
                taskNumber = firstTask.TaskNumber;
            }
            foreach (DOTask task in tasksList)
            {
                if (task.TaskNumber > taskNumber)
                    taskNumber = task.TaskNumber;
            }
            return ++taskNumber;
        }
        public List<DOBase> SelectActiveJobContractors(Guid jobID)
        {
            string query =
                @"  select JobContractorID,JobID,ContactID,CreatedBy,CreatedDate,Active,status,JobNumberAuto from jobcontractor where JobID={0} and status=0";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOJobContractor), query, jobID);
        }

        public List<DOBase> SelectActiveJobsForContractorSite(Guid siteID, Guid contactID)
        {
            string query = @"select distinct jc.ContactID,jc.JobID,j.SiteID,jc.status,jc.CreatedBy,jc.CreatedDate,jc.Active,jc.JobContractorID
    from JobContractor jc
  join Job j  on jc.JobID=j.JobID
  where j.SiteID={0}
  and jc.ContactID={1}
  and jc.status=0";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOActiveJobsForContractor), query, siteID, contactID);
        }

        /// <summary>
        /// Selects all template tasks for a company.
        /// </summary>
        /// <param name="ContractorID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTemplateTasks(Guid ContractorID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTask), "ContractorID = {0} and status=7 ORDER BY CreatedDate", ContractorID);
        }

        /// <summary>
        /// Selects all template tasks for a company by job template.
        /// </summary>
        /// <param name="ContractorID"></param>
        /// <param name="JobTemplateID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobTemplateTasks(Guid JobTemplateID)
        {
            // return CurrentDAJob.SelectObjects(typeof(DOTask), "ContractorID = {0} and JobTemplateID={1} ORDER BY CreatedDate", ContractorID, JobTemplateID);



            string Query = "select task.TaskName, task.Description, task.TradeCategoryID, jobtemplatetask.TemplateTaskID, jobtemplatetask.StartDelay, jobtemplatetask.Duration, jobtemplatetask.createdby, jobtemplatetask.createddate, jobtemplatetask.active from jobtemplatetask, Task where JobTemplateID={0} and jobtemplatetask.TemplateTaskID=task.TaskID ORDER BY task.CreatedDate";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOMyJobTemplate), Query, JobTemplateID);
        }

        /// <summary>
        /// Selects all jobs that a contractor has access to. 14.2.17 Jared
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectSitesJobAsContractor(Guid SiteID, Guid ContactID)
        {
            string Query = @"select job.* from job, JobContractor where job.SiteID={0}
            and job.jobid=JobContractor.JobID and JobContractor.ContactID={1}";

            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOJob), Query, SiteID, ContactID);
        }

        /// <summary>
        /// Selects all tasks for a contractee for a specified contractor.
        /// </summary>
        /// <param name="CustomerContactID">The contact ID of the contractee.</param>
        /// <param name="ContactID">The task contractor contact ID</param>
        /// <returns></returns>
        public List<DOBase> SelectContracteeTasks(Guid CustomerContactID, Guid ContractorContactID)
        {
            //string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (s.ContactID = {0} OR t.TaskOwner = {0}) AND t.ContractorID = {1} ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (t.TaskOwner = {1}) AND (t.ContractorID = {0} OR s.ContactID = {0}) AND j.Active = 1 AND j.JobStatus = 0 AND t.Active = 1 AND s.Active = 1 ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, CustomerContactID, ContractorContactID);
        }

        /// <summary>
        /// Selects the next incomplete for a contractee for a specified contractor.
        /// </summary>
        /// <param name="CustomerContactID">The contact ID of the contractee.</param>
        /// <param name="ContactID">The task contractor contact ID</param>
        /// <returns></returns>
        public DOTask SelectContracteeTaskNextIncomplete(Guid CustomerContactID, Guid ContractorContactID)
        {
            //string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (s.ContactID = {0} OR t.TaskOwner = {0}) AND t.ContractorID = {1} ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            string Query = "SELECT TOP 1 t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (t.TaskOwner = {1}) AND (t.ContractorID = {0} OR s.ContactID = {0}) AND t.Status = 0 AND j.Active = 1 AND j.JobStatus = 0 AND t.Active = 1 AND s.Active = 1 AND AmendedTaskID = '00000000-0000-0000-0000-000000000000' ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            List<DOBase> ret = CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, CustomerContactID, ContractorContactID);
            if (ret.Count == 0) return null;
            return ret[0] as DOTask;
        }


        /// <summary>
        /// Selects all tasks for a customer for a specified contractor.
        /// </summary>
        /// <param name="CustomerContactID">The contact ID of the customer.</param>
        /// <param name="ContactID">The task contractor contact ID</param>
        /// <returns></returns>
        public List<DOBase> SelectCustomerTasks(Guid CustomerContactID, Guid ContractorContactID)
        {
            //string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (s.ContactID = {0} OR t.TaskOwner = {0}) AND t.ContractorID = {1} ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (t.ContractorID = {1} AND s.ContactID = {0}) AND j.Active = 1 AND j.JobStatus = 0 AND t.Active = 1 AND s.Active = 1 ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, CustomerContactID, ContractorContactID);
        }

        /// <summary>
        /// Selects the next incomplete for a customer for a specified contractor.
        /// </summary>
        /// <param name="CustomerContactID">The contact ID of the customer.</param>
        /// <param name="ContactID">The task contractor contact ID</param>
        /// <returns></returns>
        public DOTask SelectCustomerTaskNextIncomplete(Guid CustomerContactID, Guid ContractorContactID)
        {
            //string Query = "SELECT t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (s.ContactID = {0} OR t.TaskOwner = {0}) AND t.ContractorID = {1} ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            string Query = "SELECT TOP 1 t.* FROM Site s LEFT JOIN Job j ON s.SiteID = j.SiteID LEFT JOIN Task t ON j.JobID = t.JobID WHERE (t.ContractorID = {1} AND s.ContactID = {0}) AND j.Active = 1 AND j.JobStatus = 0 AND t.Status = 0 AND t.Active = 1 AND s.Active = 1 AND AmendedTaskID = '00000000-0000-0000-0000-000000000000' ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            List<DOBase> ret = CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, CustomerContactID, ContractorContactID);
            if (ret.Count == 0) return null;
            return ret[0] as DOTask;
        }

        /// <summary>
        /// Selects all tasks for a stie for a specified contractor.
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="ContractorContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectSiteTasksForContractor(Guid SiteID, Guid ContractorContactID)
        {
            string Query = "SELECT t.* FROM Job j LEFT JOIN Task t ON j.JobID = t.JobID WHERE j.SiteID = {0} AND t.ContractorID = {1} AND j.Active = 1 AND t.Active = 1 ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, SiteID, ContractorContactID);

        }

        /// <summary>
        /// Selects the tasks on a job for a specified contractor.
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="ContractorContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobTasksForContractor(Guid JobID, Guid ContractorContactID)
        {
            string Query = "SELECT t.* FROM Job j LEFT JOIN Task t ON j.JobID = t.JobID WHERE j.JobID = {0} AND t.ContractorID = {1} AND j.Active = 1 AND t.Active = 1 ORDER BY CASE WHEN t.StartDate = '1900-01-01' THEN '2999-01-01' ELSE t.StartDate END, t.StartMinute";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, JobID, ContractorContactID);
        }

        /// <summary>
        /// Selects tasks for a job with a specified parent task.
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTasks(Guid JobID, Guid ParentID, bool IncludeAmended)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTask), "JobID = {0} AND ParentTaskID = {1} AND (Status <> 2 OR 1 = {2}) ORDER BY CreatedDate", JobID, ParentID, IncludeAmended ? 1 : 0);
        }

        public List<DOBase> SelectTasks(Guid JobID, Guid ParentID)
        {
            return SelectTasks(JobID, ParentID, false);
        }

        /**
        When a new job is added, update the site to active in ContactSite table
        **/
        public void UpdateSite(Guid siteID)
        {
            List<DOBase> contactSite = CurrentDAJob.SelectObjects(typeof(DOContactSite), "SiteID={0}", siteID);
            foreach (DOContactSite cs in contactSite)
            {
                cs.Active = true;
                CurrentDAJob.SaveObject(cs);
            }

        }


        /// <summary>
        /// Saves a task.
        /// </summary>
        /// <param name="Task"></param>
        public void SaveTask(DOTask Task)
        {

            CurrentDAJob.SaveObject(Task);


        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="Task"></param>
        public void DeleteTask(DOTask Task)
        {
            List<DOBase> TaskQuotes = SelectTaskQuotesInternal(Task.TaskID);
            foreach (DOTaskQuote tq in TaskQuotes)
            {
                DeleteTaskQuote(tq);
            }
            List<DOBase> TaskMaterials = SelectTaskMaterials(Task.TaskID);
            foreach (DOTaskMaterial TM in TaskMaterials)
            {
                DeleteTaskMaterial(TM);
            }
            List<DOBase> TaskLabour = SelectTaskLabour(Task.TaskID);
            foreach (DOTaskLabour TL in TaskLabour)
            {
                DeleteTaskLabour(TL);
            }
            DOTaskPendingContractor TPC = SelectTaskPendingContractor(Task.TaskID);
            if (TPC != null) CurrentDAJob.DeleteObject(TPC);

            DeleteTaskAcknowledgements(Task.TaskID);
            DeleteTaskCompletions(Task.TaskID);
            CurrentDAJob.DeleteObject(Task);

        }


        public string GetTaskStartTimeText(DOTask Task)
        {
            if (Task.StartDate == DateAndTime.NoValueDate)
                return "Not Specified";
            return DateAndTime.DisplayShortDate(Task.StartDate) + (Task.StartMinute == -1 ? string.Empty : " " + string.Format("{0}:{1:D2}", Task.StartMinute / 60, Task.StartMinute % 60));
        }

        public string GetTaskEndTimeText(DOTask Task)
        {
            if (Task.EndDate == DateAndTime.NoValueDate)
                return "Not Specified";
            return DateAndTime.DisplayShortDate(Task.EndDate) + (Task.EndMinute == -1 ? string.Empty : " " + string.Format("{0}:{1:D2}", Task.EndMinute / 60, Task.EndMinute % 60));
        }


        /// <summary>
        /// Creates a task acknowledgement.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public DOTaskAcknowledgement CreateTaskAcknowledgement(Guid TaskID, Guid ContactID)
        {
            DOTaskAcknowledgement TA = new DOTaskAcknowledgement();
            TA.TaskAcknowledgementID = Guid.NewGuid();
            TA.TaskID = TaskID;
            TA.CreatedBy = ContactID;
            return TA;
        }

        /// <summary>
        /// Saves a task acknowledgement.
        /// </summary>
        /// <param name="TA"></param>
        public void SaveTaskAcknowledgement(DOTaskAcknowledgement TA)
        {
            CurrentDAJob.SaveObject(TA);
        }

        /// <summary>
        /// Selects a task acknowledgement.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public DOTaskAcknowledgement SelectTaskAcknowledgement(Guid TaskID, Guid ContactID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskAcknowledgement), "TaskID = {0} AND CreatedBy = {1}", TaskID, ContactID) as DOTaskAcknowledgement;
        }

        private void DeleteTaskAcknowledgements(Guid TaskID)
        {
            List<DOBase> TA = CurrentDAJob.SelectObjects(typeof(DOTaskAcknowledgement), "TaskID = {0}", TaskID);
            foreach (DOBase t in TA)
            {
                CurrentDAJob.DeleteObject(t);
            }
        }

        private void DeleteTaskCompletions(Guid TaskID)
        {
            List<DOBase> TC = CurrentDAJob.SelectObjects(typeof(DOTaskCompletion), "TaskID = {0}", TaskID);
            foreach (DOBase t in TC)
            {
                CurrentDAJob.DeleteObject(t);
            }
        }

        /// <summary>
        /// Selects all the active tasks a contract is the contractor for.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectActiveTasks(Guid ContactID)
        {
            //active job and task
            //job incomplete(status0)
            //not acknowledgement task(type0)
            //job not charge up (type2)
            //task incomplete (status 0)
            string Query = "SELECT t.* FROM Task t LEFT JOIN Job j ON t.JobID = j.JobID WHERE t.ContractorID = {0} AND t.Active = 1 AND j.Active = 1 AND t.TaskType != 2 AND j.JobStatus = 0 AND j.JobType != 2 AND t.Status = 0 ";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), Query, ContactID);
        }
        #endregion

        #region Materials
        /// <summary>
        /// Creates a material category.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public DOMaterialCategory CreateMaterialCategory(Guid CreatedBy, Guid ContactID)
        {
            DOMaterialCategory Category = new DOMaterialCategory();
            Category.MaterialCategoryID = Guid.NewGuid();
            Category.CreatedBy = CreatedBy;
            Category.ContactID = ContactID;
            return Category;
        }

        /// <summary>
        /// Saves a material category.
        /// </summary>
        /// <param name="Category"></param>
        public void SaveMaterialCategory(DOMaterialCategory Category)
        {
            CurrentDAJob.SaveObject(Category);
        }


        /// <summary>
        /// Selects a material category.
        /// </summary>
        /// <param name="MaterialCategoryID"></param>
        /// <returns></returns>
        public DOMaterialCategory SelectMaterialCategory(Guid MaterialCategoryID)
        {
            return CurrentDAJob.SelectObject(typeof(DOMaterialCategory), "MaterialCategoryID = {0}", MaterialCategoryID) as DOMaterialCategory;
        }





        /// <summary>
        /// Selects all material categories with a specific contact ID.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectMaterialCategories(Guid ContactID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOMaterialCategory), "(ContactID = {0}) ORDER BY CategoryName", ContactID);
        }

        /// <summary>
        /// Selects all materials for a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectMaterialsByContact(Guid ContactID)
        {
            string Query = "SELECT m.* FROM Material m INNER JOIN MaterialCategory mc ON m.MaterialCategoryID = mc.MaterialCategoryID WHERE m.ContactID = {0} ORDER BY m.MaterialName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOMaterial), Query, ContactID);
        }


        ///// <summary>
        ///// Selects a customer for an invoice.
        ///// </summary>
        ///// <param name="ContactID"></param>
        ///// <returns></returns>
        //public List<DOBase> SelectInvoiceCustomer(Guid InvoiceID)
        //{
        //    string Query = "Select contact.ContactID from contact, invoice where Invoice.CustomerID=Contact.ContactID and invoice.InvoiceID={0}";
        //    return CurrentDAJob.SelectObjectCustomQuery(typeof(DOContact), Query, InvoiceID);
        //}




        /// <summary>
        /// Selects all material categories for a contact, and the companies the contact is linked to.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectMaterialCategoriesLinked(Guid ContactID)
        {
            string Query =
                @"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOMaterialCategory), Query, ContactID);
        }

        /// <summary>
        /// Deletes a material category. The category must have no materials linked to it.
        /// </summary>
        /// <param name="Category"></param>
        public void DeleteMaterialCategory(DOMaterialCategory Category)
        {
            CurrentDAJob.DeleteObject(Category);
        }

        /// <summary>
        /// Saves a supplierInvoiceMaterial.
        /// </summary>
        /// <param name="SupplierInvoiceMaterial"></param>
        public void SaveSupplierInvoiceMaterial(DOSupplierInvoiceMaterial SupplierInvoiceMaterial)
        {
            CurrentDAJob.SaveObject(SupplierInvoiceMaterial);
        }
        /// <summary>
        /// Gets All UnAssigned supplierInvoiceMaterials by contactid
        /// </summary>
        /// <param name="SelectSupplierInvoices"></param>
        public List<DOBase> SelectSupplierInvoices(Guid ContractorID, bool Assigned)
        {
            string Query =
                 @"select Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, SupplierInvoice.SupplierReference,
SupplierInvoiceMaterial.QTY, Material.Description, Material.CostPrice, material.SellPrice, Material.RRP, Material.UOM, NEWID() as tempID, SupplierInvoice.CreatedBy,
SupplierInvoice.CreatedDate, SupplierInvoice.Active, Material.MaterialID, SupplierInvoiceMaterial.SupplierInvoiceMaterialID, Material.MaterialName
from Supplier, SupplierInvoiceMaterial, Material, SupplierInvoice 
where 
SupplierInvoiceMaterial.Assigned={1} and 
SupplierInvoiceMaterial.MaterialID = Material.MaterialID and
SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
order by SupplierReference";
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOUnassignedByContactID), Query, ContractorID, Assigned);
        }













        /// <summary>
        /// Gets All supplierInvoiceMaterials by contactid, qty and materialID
        /// </summary>
        /// <param name="SelectSIMsByContactID_Qty_materialid"></param>
        public List<DOBase> SelectSIMsByContactID_Qty_materialid(Guid ContractorID, Decimal Qty, Guid VehicleID)
        {
            string Query =

                @"select SupplierInvoiceMaterial.SupplierInvoiceMaterialID where qty={1} and contactid={0} and vehicleid={2}";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOSupplierInvoiceMaterial), Query, ContractorID, Qty, VehicleID);
        }

        /// <summary>
        /// Gets All invoice labour items by Invoiceid,
        /// </summary>
        /// <param name="SelectTaskLaboursByInvoiceID"></param>
        public List<DOBase> SelectTaskLaboursByInvoiceID(Guid InvoiceID)
        {
            string Query =

                @"select * from tasklabour where tasklabour.invoiceid={0}";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskLabourInfo), Query, InvoiceID);
        }

        /// <summary>
        /// Gets All invoice material items by Invoiceid,
        /// </summary>
        /// <param name="SelectTaskMaterialsByInvoiceID"></param>
        public List<DOBase> SelectTaskMaterialsByInvoiceID(Guid InvoiceID)
        {
            string Query =

                @"select * from taskmaterial where taskmaterial.invoiceid={0}";


            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskMaterialInfo), Query, InvoiceID);
        }















        /// <summary>
        /// Gets All supplierInvoiceMaterials by contractorid and assigned
        /// </summary>
        /// <param name="SelectSupplierInvoicesOrderByCRef"></param>
        public List<DOBase> SelectSupplierInvoicesOrderByCRef(Guid ContractorID, string Assigned)//
        {
            StringBuilder Query1 = new StringBuilder(
          @"select distinct SupplierInvoice.SupplierInvoiceID, Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, 
                    SupplierInvoice.SupplierReference, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, SupplierInvoice.ContractorReference, SupplierInvoice.TotalExGst
                    from Supplier, SupplierInvoiceMaterial, SupplierInvoice 
                    where 
                    SupplierInvoice.Status" + Assigned +
            @"and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
                    Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
                    SupplierInvoice.SupplierID !='CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC' and
                    supplierinvoice.contractorid={0} and supplierInvoice.active=1 
                    order by SupplierInvoice.ContractorReference");
            string Query = Query1.ToString();
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOUnassignedByContactIDAndSupplierInvoiceID), Query, ContractorID, Assigned);
        }




        /// <summary>
        /// Gets All supplierInvoiceMaterials by contractorid that are active
        /// </summary>
        /// <param name="SelectSupplierInvoicesOrderByCRefInActive"></param>
        public List<DOBase> SelectSupplierInvoicesOrderByCRefInActive(Guid ContractorID)//
        {
            StringBuilder Query1 = new StringBuilder(
          @"select distinct SupplierInvoice.SupplierInvoiceID, Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, 
                    SupplierInvoice.SupplierReference, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, SupplierInvoice.ContractorReference, SupplierInvoice.TotalExGst
                    from Supplier, SupplierInvoiceMaterial, SupplierInvoice 
                    where SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
                    Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
                    SupplierInvoice.SupplierID !='CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC' and
                    supplierinvoice.contractorid={0} and supplierInvoice.active=0 
                    order by SupplierInvoice.ContractorReference");
            string Query = Query1.ToString();
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOUnassignedByContactIDAndSupplierInvoiceID), Query, ContractorID);
        }


        /// <summary>
        /// Increment/Reduce by + or -
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceStatus(Guid SupplierInvoiceID, string Operator)
        {

            StringBuilder Query1 = new StringBuilder(
                 @"update SupplierInvoice set status=status" + Operator + "1 where SupplierInvoiceID={0}");
            string Query = Query1.ToString();

            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceID, Operator);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }



        /// <summary>
        /// make inactive
        /// </summary>
        /// <param name="Active"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceToInActive(Guid SupplierInvoiceID)
        {

            StringBuilder Query1 = new StringBuilder(
                 @"update SupplierInvoice set active=0 where SupplierInvoiceID={0}");
            string Query = Query1.ToString();

            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }

        /// <summary>
        /// make active
        /// </summary>
        /// <param name="Active"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceToActive(Guid SupplierInvoiceID)
        {

            StringBuilder Query1 = new StringBuilder(
                 @"update SupplierInvoice set active=1 where SupplierInvoiceID={0}");
            string Query = Query1.ToString();

            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }

        /// <summary>
        /// Increment/Reduce by + or -
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateTaskInvoiceNumber(Guid TaskID, string Operator)
        {

            StringBuilder Query1 = new StringBuilder(
                 @"update task set invoicenumber=invoicenumber" + Operator + "1 where taskID={0}");
            string Query = Query1.ToString();

            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskID, Operator);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }



        /// <summary>
        /// Increment/Reduce by + or -
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateInvoiceStatus(Guid TaskID, string Operator)
        {

            StringBuilder Query1 = new StringBuilder(
                 @"update invoice set invoicestatus=invoicestatus" + Operator + "1 where InvoiceID={0}");
            string Query = Query1.ToString();

            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskID, Operator);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }






        /// <summary>
        /// Gets All vehicles materials by driver id and vehicle owner id
        /// </summary>
        /// <param name="SelectVehicleMaterials"></param>
        public List<DOBase> SelectVehicleMaterials(Guid VehicleOwnerID, Guid VehicleDriverID, Guid VehicleID)//
        {
            string Query =
                 @"select NEWID() as tempid, vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,
                    material.uom, material.materialid, supplierinvoicematerial.qty, supplierinvoicematerial.qtyremainingtoassign, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, 
                    SupplierInvoice.Active, supplier.supplierid, supplierinvoicematerial.oldsupplierinvoicematerialid, supplierinvoicematerial.createdby as Creator,
                    SupplierInvoiceMaterial.SupplierInvoiceMaterialID, supplierinvoicematerial.supplierinvoiceid, supplierinvoice.ContractorReference
                    from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice, supplier
                    where 
                    SupplierInvoiceMaterial.VehicleID={2} 
                    and Vehicle.VehicleID={2}
                    and SupplierInvoiceMaterial.QtyRemainingToAssign > 0
                    and SupplierInvoiceMaterial.vehicleid!='00000000-0000-0000-0000-000000000000'
                    and supplier.supplierid=supplierinvoice.supplierid
					and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID
                    and supplierinvoiceMaterial.contractorid={0}
					and Vehicle.VehicleOwner={0}
					and Vehicle.VehicleDriver={1}
					and SupplierInvoiceMaterial.MaterialID=Material.MaterialID
                    and  (SupplierInvoiceMaterial.TaskMaterialID='00000000-0000-0000-0000-000000000000' 
                    or SupplierInvoiceMaterial.TaskMaterialID!='00000000-0000-0000-0000-000000000000' 
                    and SupplierInvoiceMaterial.OldSupplierInvoiceMaterialID='00000000-0000-0000-0000-000000000000')
                    order by material.MaterialName
                  ";
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOVehicleMaterials), Query, VehicleOwnerID, VehicleDriverID, VehicleID);

            //       string Query =
            //           @"select NEWID() as tempid, vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,
            //               material.uom, material.materialid, supplierinvoicematerial.qty, supplierinvoicematerial.qtyremainingtoassign, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, supplier.supplierid, 
            //               SupplierInvoiceMaterial.SupplierInvoiceMaterialID, supplierinvoicematerial.supplierinvoiceid, supplierinvoice.ContractorReference
            //               from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice, supplier
            //               where 
            //               SupplierInvoiceMaterial.VehicleID={2} 
            //               and SupplierInvoiceMaterial.QtyRemainingToAssign > 0
            //               and SupplierInvoiceMaterial.taskmaterialid='00000000-0000-0000-0000-000000000000'
            //               and supplier.supplierid=supplierinvoice.supplierid
            //and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID
            //               and supplierinvoice.contractorid={0}
            //and Vehicle.VehicleOwner={0}
            //and Vehicle.VehicleDriver={1}
            //and SupplierInvoiceMaterial.MaterialID=Material.MaterialID
            //               order by material.MaterialName
            //             ";
        }



        /// <summary>
        /// Gets All vehicle SIMs by vehicleid, supplierid and contractor
        /// </summary>
        /// <param name="SelectVehicleSIMs"></param>
        public List<DOBase> SelectVehicleSIMs(Guid VehicleOwnerID, Guid VehicleID)//
        {
            string Query =
                 @"select SupplierInvoiceMaterialID, MaterialID, VehicleID, QtyRemainingToAssign, CreatedDate, CreatedBy, Active from SupplierInvoiceMaterial where ContractorID={0} and VehicleID={1}";
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOVehicleMaterialQty), Query, VehicleOwnerID, VehicleID);
        }



        /// <summary>
        /// Sees if a vehicle can be deleted. (if false, don't delete)
        /// </summary>
        /// <param name="VehicleID">VehicleID Guid</param>
        /// <returns>bool(true  if none assigned, false if materials assigned</returns>
        public bool SeeIfCanDeleteVehicle(Guid VehicleID)
        // Checks if any materials assigned to vehicle. If none assigned, returns true and can delete.
        // If materials assigned to vehicle, returns false for can't delete
        {
            DOBase tempSIM = null;
            string query = @"VehicleID = {0}"; //return rubbish and end query after 1 found
            tempSIM = CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), query, VehicleID);
            if (tempSIM == null)
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// Deletes a vehicle from database : Added by Martin Falconer 09/07/16
        /// </summary>
        /// <param name="Vehicle">DOVehicle object with data to delete</param>
        public void DeleteVehicle(DOVehicle Vehicle)
        {
            CurrentDAJob.DeleteObject(Vehicle);
        }



        //Added by Martin Falconer
        /// <summary>
        /// Transfer All materials from one vehicle to another
        /// </summary>
        /// <param name="vehicleToID">(string)Vehicle to which materials are being transferred</param>
        /// <param name="vehicleFromID"></param>
        public void TransferAllMaterialsBetweenVehicles(string vehicleToID, string vehicleFromID)
        {
            string query = "UPDATE SupplierInvoiceMaterial SET VehicleID = {0} WHERE VehicleID = {1}";

            CurrentDAJob.ExecuteScalar(query, vehicleToID, vehicleFromID);
        }



        /// <summary>
        /// Update tasklabour qty
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void UpdateTaskLabourQuantity(Guid TaskLabourID, decimal newQuantity)
        {
            string query = "UPDATE tasklabour SET invoicequantity = {1} WHERE tasklabourID = {0}";

            CurrentDAJob.ExecuteScalar(query, TaskLabourID, newQuantity);
        }




        //   SelectMaterialsByContactIDAndSupplierInvoiceReferenceAndAssignment (temp)
        /// <summary>
        /// Gets All supplierInvoiceMaterials by contactid and supplierinvoice id and assigned/not
        /// </summary>
        /// <param name="SelectMaterialsByContactIDAndSupplierInvoiceReferenceAndAssigned"></param>
        public List<DOBase> SelectMaterialsByContactIDAndSupplierInvoiceReferenceAndAssigned(Guid ContractorID, string SupplierReference, bool Assigned)//1
        {
            StringBuilder Query = new StringBuilder(
                @"select Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, SupplierInvoice.SupplierReference,
                SupplierInvoiceMaterial.QTY, Material.Description, Material.CostPrice, material.SellPrice, Material.RRP, Material.UOM, NEWID() as tempID, SupplierInvoice.CreatedBy,
                SupplierInvoice.CreatedDate, SupplierInvoice.Active, Material.MaterialID, SupplierInvoiceMaterial.SupplierInvoiceMaterialID, Material.MaterialName, 
                SupplierInvoiceMaterial.MatchID, SupplierInvoiceMaterial.VehicleID, SupplierInvoiceMaterial.TaskMaterialID, SupplierInvoiceMaterial.SupplierInvoiceID,
                SupplierInvoiceMaterial.OldSupplierInvoiceMaterialID, SupplierInvoiceMaterial.QtyRemainingToAssign, SupplierInvoiceMaterial.createddate
                from Supplier, SupplierInvoiceMaterial, Material, SupplierInvoice 
                where 
                SupplierInvoiceMaterial.Assigned={2} and 
                SupplierInvoiceMaterial.MaterialID = Material.MaterialID and
                SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
                Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
                supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233' and
                SupplierReference like {1} order by material.materialid, SupplierInvoiceMaterial.createddate");
            //@"SELECT * FROM MaterialCategory WHERE (ContactID = {0} OR ContactID IN (SELECT CompanyID FROM ContactCompany WHERE ContactID = {0} AND Active = 1 AND Pending = 0) ORDER BY Cast(ContactID AS nvarchar(50)), CategoryName";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOUnassignedByContactID), Query.ToString(), ContractorID, SupplierReference, Assigned);
        }




        /// <summary>
        /// Selects the whole tradecategories table.
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectAllTradeCategories()
        {
            return CurrentDAJob.SelectObjectsOrderBy(typeof(DOTradeCategory), "TradeCategoryName");
        }





        /// <summary>
        /// Selects a SIM by SIM ID
        /// </summary>
        /// <param name="SupplierInvoiceMaterialID"></param>
        /// <returns></returns>
        public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterial(Guid SupplierInvoiceMaterialID)
        {
            return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID = {0}", SupplierInvoiceMaterialID) as DOSupplierInvoiceMaterial;
        }


        /// <summary>
        /// Selects a SIM by OldSIM ID
        /// </summary>
        /// <param name="OldSIMID"></param>
        /// <returns></returns>
        public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterialByOldSIMandV(Guid SupplierInvoiceMaterialID, Guid VehicleID)
        {
            return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID = {0} and vehicleid={1}", SupplierInvoiceMaterialID, VehicleID) as DOSupplierInvoiceMaterial;
        }


        /// <summary>
        /// Selects a SIM by OldSIM ID
        /// </summary>
        /// <param name="OldSIMID"></param>
        /// <returns></returns>
        public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterialByOldSIM(Guid SupplierInvoiceMaterialID)
        {
            return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "OldSupplierInvoiceMaterialID = {0}", SupplierInvoiceMaterialID) as DOSupplierInvoiceMaterial;
        }

        /// <summary>
        /// Selects a SIM by match ID
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterialByMatch(Guid SupplierInvoiceMaterialID)
        {
            return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "MatchID = {0}", SupplierInvoiceMaterialID) as DOSupplierInvoiceMaterial;
        }


        /// <summary>
        /// Selects SIMs by OldSIM ID
        /// </summary>
        /// <param name="OldSIMID"></param>
        /// <returns></returns>
        public List<DOBase> SelectSupplierInvoiceMaterialsByOldSIM(Guid OldSim)
        {

            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOSupplierInvoiceMaterial), "select * from supplierinvoicematerial where OldSupplierInvoiceMaterialID = {0}", OldSim);
        }


        public List<DOBase> SelectActiveTasksforContact(Guid contractorID, Guid customerID, Guid jobId)
        {
            string query =
                @" select QuoteID, InvoiceNumber, TaskID,JobID,ContractorID,ParentTaskID,TaskName,TaskType,Description,CreatedBy,CreatedDate,Active,TaskOwner,Status
      ,StartDate,StartMinute,EndDate,EndMinute,Appointment,AmendedTaskID,LMVisibility,InvoiceToType,AmendedBy
      ,AmendedDate,TradeCategoryID,TaskNumber,TaskInvoiceStatus,TaskCustomerID,SiteID from Task WHERE jobid={2} 
and Status!=2 and ((ContractorID={0} or ContractorID={1}) and ( TaskCustomerID={1} or TaskCustomerID={0}) )";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTask), query,
                contractorID, customerID, jobId);
        }




        ///// <summary>
        ///// Select a task by SIMid
        ///// </summary>
        ///// <param name="SelectTaskBySIMid"></param>
        //public DOTask SelectTaskBySIMid(Guid SIMID)//1
        //{
        //    string Query =
        //        @"select * from task, taskmaterial,supplierinvoicematerial where TaskMaterial.TaskID=task.taskid and SupplierInvoiceMaterial.TaskMaterialID=TaskMaterial.TaskMaterialID and SupplierInvoiceMaterial.SupplierInvoiceMaterialID={0}";
        //    return CurrentDAJob.SelectObject(typeof(DOTask), Query, SIMID) as DOTask;
        //}



        /// <summary>
        /// Selects a job.
        /// </summary>
        /// <param name="SupplierInvoiceMaterialID"></param>
        /// <returns></returns>
        public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterialByTM(Guid TaskMaterialID)
        {
            return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "TaskMaterialID = {0}", TaskMaterialID) as DOSupplierInvoiceMaterial;
        }



        /// <summary>
        /// Updates a task material's invoiceID
        /// </summary>
        /// <param name="TMID, InvoiceID"></param>
        /// <returns></returns>
        /// 
        public void UpdateTMInvoiceID(Guid TaskMaterialID, Guid InvoiceID)
        {

            string Query = @"update Taskmaterial set InvoiceID = {1} where TaskMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskMaterialID, InvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }


        /// <summary>
        /// Updates a task material's invoiceID
        /// </summary>
        /// <param name="TMID, InvoiceID"></param>
        /// <returns></returns>
        /// 
        public void UpdateTM(Guid TaskMaterialID, decimal Qty, decimal SellPrice, string MaterialName, string Description)
        {

            string Query = @"update Taskmaterial set Invoicequantity = {1}, sellprice={2}, materialname={3}, description={4} where TaskMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskMaterialID, Qty, SellPrice, MaterialName, Description);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }




        /// <summary>
        /// Updates a task material's invoiceID by invoiceid
        /// </summary>
        /// <param name="TMID, InvoiceID"></param>
        /// <returns></returns>
        /// 
        public void UpdateTMInvoiceIDToZero(Guid InvoiceID)
        {

            string Query = @"update Taskmaterial set InvoiceID = '00000000-0000-0000-0000-000000000000'  where InvoiceID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, InvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }



        /// <summary>
        /// Updates a task labour's invoiceID
        /// </summary>
        /// <param name="TLID, InvoiceID"></param>
        /// <returns></returns>
        /// 
        public void UpdateTLInvoiceID(Guid TaskLabourID, Guid InvoiceID)
        {

            string Query = @"update Tasklabour set InvoiceID = {1} where TasklabourID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskLabourID, InvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }


        /// <summary>
        /// Updates a task labour's info
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        public void UpdateInvoiceTL(Guid TaskLabourID, decimal InvoiceQuantity, decimal LabourRate, string InvoiceDescription)
        {

            string Query = @"update Tasklabour set invoicequantity = {1}, labourrate= {2}, description = {3} where TasklabourID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, TaskLabourID, InvoiceQuantity, LabourRate, InvoiceDescription);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }



        /// <summary>
        /// Updates a task labour's invoiceID to 000's with invoice id
        /// </summary>
        /// <param name="TLID, InvoiceID"></param>
        /// <returns></returns>
        /// 
        public void UpdateTLInvoiceIDToZero(Guid InvoiceID)
        {

            string Query = @"update Tasklabour set InvoiceID = '00000000-0000-0000-0000-000000000000' where InvoiceID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, InvoiceID);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }




        /// <summary>
        /// Updates a SupplierInvoiceMaterial qtyRTA Jared to fix
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceMaterialNewQtyRemaining(Guid SupplierInvoiceMaterialID, decimal newQtyRemainingToAssign)
        {

            string Query = @"update SupplierInvoiceMaterial set QtyRemainingToAssign = {1} where SupplierInvoiceMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceMaterialID, newQtyRemainingToAssign);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }


        /// <summary>
        /// Updates a SupplierInvoiceMaterial qty Jared to fix
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceMaterialNewQty(Guid SupplierInvoiceMaterialID, decimal newQty)
        {

            string Query = @"update SupplierInvoiceMaterial set Qty = {1} where SupplierInvoiceMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceMaterialID, newQty);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }



        ///// <summary>
        ///// Updates invoicestatus of tasklabour table
        ///// </summary>
        ///// <param name="CreatedBy"></param>
        ///// <returns></returns>
        ///// 
        //public void UpdateTaskLabourInvoiceStatus(Guid TaskLabourID, int newQty)
        //{

        //    string Query = @"update TaskLabour set InvoiceStatus = {1} where TaskLabourID = {0}";
        //    //SqlCommand cmd = new SqlCommand(Query.ToString());
        //    //Execute(cmd);
        //    CurrentDAJob.ExecuteScalar(Query, TaskLabourID, newQty);
        //    //SupplierInvoiceMaterialID Execute(cmd);
        //    //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
        //    //   SupplierInvoiceMaterial.

        //}

        ///// <summary>
        ///// Updates invoicestatus of taskmaterial table
        ///// </summary>
        ///// <param name="CreatedBy"></param>
        ///// <returns></returns>
        ///// 
        //public void UpdateTaskMaterialInvoiceStatus(Guid TaskMaterialID, int newQty)
        //{

        //    string Query = @"update TaskMaterial set InvoiceStatus = {1} where TaskMaterialID = {0}";
        //    //SqlCommand cmd = new SqlCommand(Query.ToString());
        //    //Execute(cmd);
        //    CurrentDAJob.ExecuteScalar(Query, TaskMaterialID, newQty);
        //    //SupplierInvoiceMaterialID Execute(cmd);
        //    //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
        //    //   SupplierInvoiceMaterial.

        //}


        /// <summary>
        /// Updates a SupplierInvoiceMaterial. sets assigned
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceMaterialAssigned(Guid SupplierInvoiceMaterialID, int intValue)
        {

            string Query = @"update SupplierInvoiceMaterial set Assigned = {1} where SupplierInvoiceMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceMaterialID, intValue);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }






        /// <summary>
        /// Updates a SupplierInvoiceMaterial.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public void UpdateSupplierInvoiceMaterialNewOldSIMID(Guid SupplierInvoiceMaterialID, Guid oldSIMid)
        {

            string Query = @"update SupplierInvoiceMaterial set oldsupplierinvoicematerialid = {1} where SupplierInvoiceMaterialID = {0}";
            //SqlCommand cmd = new SqlCommand(Query.ToString());
            //Execute(cmd);
            CurrentDAJob.ExecuteScalar(Query, SupplierInvoiceMaterialID, oldSIMid);
            //SupplierInvoiceMaterialID Execute(cmd);
            //   List<DOBase> SupplierInvoiceMaterial = CurrentDAJob.SelectObjects(typeof(DOSupplierInvoiceMaterial), "SupplierInvoiceMaterialID={0}");
            //   SupplierInvoiceMaterial.

        }




        /// <summary>
        /// Deletes a supplierinvoicematerial
        /// </summary>
        /// <param name="supplierinvoicematerial"></param>
        public void DeleteSupplierInvoiceMaterial(DOSupplierInvoiceMaterial SupplierInvoiceMaterial)
        {
            CurrentDAJob.DeleteObject(SupplierInvoiceMaterial);
        }


        private void Execute(SqlCommand cmd)
        {
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            cmd.Connection = sqlConn;

            try
            {
                sqlConn.Open();
                cmd.ExecuteNonQuery();
            }

            finally
            {
                sqlConn.Close();
            }
        }







        /// <summary>
        /// Creates a material.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOMaterial CreateMaterial(Guid CreatedBy)
        {
            DOMaterial Material = new DOMaterial();
            Material.MaterialID = Guid.NewGuid();
            Material.CreatedBy = CreatedBy;
            Material.MaterialCategoryID = Guid.Empty;
            return Material;
        }

        /// <summary>
        /// Selects a material.
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <returns></returns>
        public DOMaterial SelectMaterial(Guid MaterialID)
        {
            return CurrentDAJob.SelectObject(typeof(DOMaterial), "MaterialID = {0}", MaterialID) as DOMaterial;
        }


        /// <summary>
        /// Selects all materials.
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectMaterials()
        {
            return CurrentDAJob.SelectObjectsOrderBy(typeof(DOMaterial), "MaterialName");
        }


        /// <summary>
        /// Selects materials by category.
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectMaterials(Guid MaterialCategoryID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOMaterial), "MaterialCategoryID = {0} ORDER BY MaterialName", MaterialCategoryID);
        }


        /// <summary>
        /// Selects materials by contactID.
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectMaterialsbyContactID(Guid contactID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOMaterial), "ContactID = {0}", contactID);
            //return CurrentDAJob.SelectObjects(typeof(DOMaterial), "ContactID = {0} order by materialname", contactID);
        }

        /// <summary>
        /// Saves a material.
        /// </summary>
        /// <param name="Material"></param>
        public void SaveMaterial(DOMaterial Material)
        {
            CurrentDAJob.SaveObject(Material);
        }


        /// <summary>
        /// Selects supplierinvoices by supplierID
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectSupplierInvoicesInfoBySupplierID(Guid SupplierID, Guid ContractorID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOSupplierInvoice), "SupplierID = {0} and contractorID = {1}", SupplierID, ContractorID);
            //return CurrentDAJob.SelectObjects(typeof(DOMaterial), "ContactID = {0} order by materialname", contactID);
        }



        /// <summary>
        /// Creates a task material link.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="MaterialID"></param>
        /// <param name="CreatedBy"></param>
        public DOTaskMaterial CreateTaskMaterial(Guid TaskID, Guid MaterialID, Guid CreatedBy)
        {
            DOTaskMaterial TaskMaterial = new DOTaskMaterial();
            TaskMaterial.TaskMaterialID = Guid.NewGuid();
            TaskMaterial.CreatedBy = CreatedBy;
            TaskMaterial.TaskID = TaskID;
            TaskMaterial.MaterialID = MaterialID;
            return TaskMaterial;
        }

        /// <summary>
        /// Saves a task material.
        /// </summary>
        /// <param name="TaskMaterial"></param>
        public void SaveTaskMaterial(DOTaskMaterial TaskMaterial)
        {
            CurrentDAJob.SaveObject(TaskMaterial);
        }

        /// <summary>
        /// Deletes a task material
        /// </summary>
        /// <param name="TaskMaterial"></param>
        public void DeleteTaskMaterial(DOTaskMaterial TaskMaterial)
        {
            CurrentDAJob.DeleteObject(TaskMaterial);
        }


        /// <summary>
        /// Selects materials for a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskMaterials(Guid TaskID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTaskMaterial), "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        /// <summary>
        /// Selects materials for a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskMaterialsList(Guid TaskID)
        {
            StringBuilder query = new StringBuilder("select TaskMaterialID, InvoiceQuantity, InvoiceID, QuoteID, QuoteNumber, taskid,MaterialName,Quantity,SellPrice,Description,MaterialType,CreatedBy,CreatedDate,Active from TaskMaterial");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskMaterialInfo), query, "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
            //return CurrentDAJob.SelectObjects(typeof(DOTaskMaterial), "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        /// <summary>
        /// Selects materials for a task that arent on invoice.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        /// 
        //Tony added 18. Jan. 2017
        public List<DOBase> SelectTaskMaterialsListNotInvoiced(Guid TaskID)
        {
            StringBuilder query = new StringBuilder("select TaskMaterialID, InvoiceQuantity, InvoiceID, QuoteID, QuoteNumber, taskid,MaterialName,Quantity,SellPrice,Description,MaterialType,CreatedBy,CreatedDate,Active from TaskMaterial");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskMaterialInfo), query, "invoiceid='00000000-0000-0000-0000-000000000000' and active=1 and TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
            //return CurrentDAJob.SelectObjects(typeof(DOTaskMaterial), "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        //Tony added 18. Jan. 2017
        public List<DOBase> SelectTaskMaterialsListNotInvoiced(Guid TaskID, Guid[] pkIDs)
        {
            if (pkIDs.Length < 1)
                return null;

            string strMaterialIDs = ListOfMaterialID(pkIDs);

            StringBuilder query = new StringBuilder("select TaskMaterialID, InvoiceQuantity, InvoiceID, QuoteID, QuoteNumber, taskid,MaterialName,Quantity,SellPrice,Description,MaterialType,CreatedBy,CreatedDate,Active from TaskMaterial");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskMaterialInfo), query, "invoiceid='00000000-0000-0000-0000-000000000000' and active=1 and TaskID = {0} and TaskMaterialID in (" +
                                                                                             strMaterialIDs + ") ORDER BY CreatedDate DESC", TaskID);
            //return CurrentDAJob.SelectObjects(typeof(DOTaskMaterial), "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        /// <summary>
        /// Selects materials for a task.
        /// </summary>
        /// <param name="TaskMaterialID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskMaterialsByTMID(Guid TaskMaterialID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTaskMaterial), "TaskMaterialID = {0}", TaskMaterialID);
        }



        /// <summary>
        /// Selects materials for a task.
        /// </summary>
        /// <param name="TMID"></param>
        /// <returns></returns>


        public DOTaskMaterial SelectSingleTaskMaterial(Guid TMID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskMaterial), "TaskMaterialID = {0}", TMID) as DOTaskMaterial;
        }
        #endregion

        #region Labour
        public DOLabour CreateLabour(Guid CreatedBy)
        {
            DOLabour Labour = new DOLabour();
            Labour.LabourID = Guid.NewGuid();
            Labour.CreatedBy = CreatedBy;
            Labour.LabourCategoryID = Guid.Empty;
            return Labour;
        }

        /// <summary>
        /// Selects a labour item.
        /// </summary>
        /// <param name="LabourID"></param>
        /// <returns></returns>
        public DOLabour SelectLabour(Guid LabourID)
        {
            return CurrentDAJob.SelectObject(typeof(DOLabour), "LabourID = {0}", LabourID) as DOLabour;
        }


        /// <summary>
        /// Selects all labour items.
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectAllLabour()
        {
            return CurrentDAJob.SelectObjectsOrderBy(typeof(DOLabour), "LabourName");
        }

        /// <summary>
        /// Saves a labour item.
        /// </summary>
        /// <param name="Labour"></param>
        public void SaveLabour(DOLabour Labour)
        {
            CurrentDAJob.SaveObject(Labour);
        }


        /// <summary>
        /// Creates a task labour link.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="LabourID"></param>
        /// <param name="CreatedBy"></param>
        public DOTaskLabour CreateTaskLabour(Guid TaskID, Guid ContactID, Guid CreatedBy)
        {
            DOTaskLabour TaskLabour = new DOTaskLabour();
            TaskLabour.TaskLabourID = Guid.NewGuid();
            TaskLabour.ContactID = ContactID;
            TaskLabour.CreatedBy = CreatedBy;
            TaskLabour.TaskID = TaskID;
            return TaskLabour;
        }

        /// <summary>
        /// Saves a task labour link.
        /// </summary>
        /// <param name="TaskLabour"></param>
        public void SaveTaskLabour(DOTaskLabour TaskLabour)
        {
            CurrentDAJob.SaveObject(TaskLabour);
        }

        /// <summary>
        /// Deletes a task labour link.
        /// </summary>
        /// <param name="TaskLabour"></param>
        public void DeleteTaskLabour(DOTaskLabour TaskLabour)
        {
            CurrentDAJob.DeleteObject(TaskLabour);
        }


        /// <summary>
        /// Selects labour for a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskLabour(Guid TaskID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTaskLabour), "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }
        /// <summary>
        /// Selects labour for a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public List<DOBase> SelectTaskLabours(Guid TaskID)
        {
            StringBuilder query = new StringBuilder("select TaskID, invoiceid, InvoiceDescription, quotenumber, quoteid, invoicequantity, TaskLabourID,Description,CreatedBy,CreatedDate,Active,LabourDate,StartMinute,EndMinute,ContactID,LabourType,LabourRate from tasklabour");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskLabourInfo), query, "TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        //Tony added 17.Jan.2017
        public string ListOfMaterialID(Guid[] pkIDs)
        {
            string strIDs = "";

            if (pkIDs.Length > 0)
            {
                for (int i = 0; i < pkIDs.Length; i++)
                {
                    strIDs += "'" + pkIDs[i] + "'";

                    if (i < pkIDs.Length - 1)
                        strIDs += ",";
                }
            }
            return strIDs;
        }

        //Tony modified 17.Jan.2017
        public List<DOBase> SelectTaskLaboursNotInvoiced(Guid TaskID)
        {
            StringBuilder query = new StringBuilder("select TaskID, invoiceid, InvoiceDescription, quotenumber, quoteid, invoicequantity, TaskLabourID,Description,CreatedBy,CreatedDate,Active,LabourDate,StartMinute,EndMinute,ContactID,LabourType,LabourRate from tasklabour");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskLabourInfo), query,
                "invoiceid='00000000-0000-0000-0000-000000000000' and active=1 and TaskID = {0} ORDER BY CreatedDate DESC", TaskID);
        }

        //Tony added 18.Jan.2017
        public List<DOBase> SelectTaskLaboursNotInvoiced(Guid TaskID, Guid[] pkIDs)
        {
            if (pkIDs.Length < 1)
                return null;

            string strMaterialIDs = ListOfMaterialID(pkIDs);

            StringBuilder query = new StringBuilder("select TaskID, invoiceid, InvoiceDescription, quotenumber, quoteid, invoicequantity, TaskLabourID,Description,CreatedBy,CreatedDate,Active,LabourDate,StartMinute,EndMinute,ContactID,LabourType,LabourRate from tasklabour");
            return CurrentDAJob.SelectQueryListofObjects(typeof(DOTaskLabourInfo), query,
                "invoiceid='00000000-0000-0000-0000-000000000000' and active=1 and TaskID = {0} and TaskLabourID in (" +
                strMaterialIDs + ") ORDER BY CreatedDate DESC", TaskID);
        }


        public List<DOBase> FindContractorForAllTaskofJob(Guid jobID, Guid contractorID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTask), "JobId={0} and contractorid={1}", jobID, contractorID);
        }

        public DOTaskLabour SelectSingleTaskLabour(Guid TLID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskLabour), "TaskLabourID = {0}", TLID) as DOTaskLabour;
        }

        /// <summary>
        /// Selects time sheet (task labour) data for a company.
        /// </summary>
        /// <param name="CompanyContactID">The company contact ID.</param>
        /// <param name="StartDate">Start date of period to view (inclusive).</param>
        /// <param name="EndDate">End date of period to view (exclusive).</param>
        /// <returns></returns>
        public List<DOBase> SelectEmployeeTimeSheets(Guid CompanyContactID, DateTime StartDate, DateTime EndDate)
        {
            //tl.LabourType = 2: Actual
            //t.Status != 2: Not amended tasks
            //            string Query =
            //                @"WITH tl AS (
            //                    SELECT ContactID, SUM ( CASE WHEN tl.LabourType = 2 AND tl.Active = 1 THEN tl.EndMinute - tl.StartMinute END) TotalMinutes, SUM( CASE WHEN Chargeable = 1 AND tl.Active = 1 AND tl.LabourType = 2 THEN tl.EndMinute - tl.StartMinute END) ChargeableMinutes
            //                    FROM TaskLabour tl
            //                    INNER JOIN Task t ON tl.TaskID = t.TaskID AND t.AmendedTaskID = '00000000-0000-0000-0000-000000000000'
            //                    WHERE LabourDate >= {0} AND LabourDate < {1} AND ContactID IN (SELECT ContactID FROM ContactCompany WHERE CompanyID = {2} AND Active = 1 AND Pending = 0) AND t.ContractorID = {2} AND t.Status != 2
            //                    GROUP BY ContactID
            //                )
            //                SELECT tl.ContactID, c.FirstName, c.LastName, tl.TotalMinutes, tl.ChargeableMinutes, tl.ContactID CreatedBy, GETDATE() CreatedDate, CAST(1 AS bit) Active
            //                FROM tl 
            //                LEFT JOIN Contact c ON tl.ContactID = c.ContactID 
            //                ORDER BY c.LastName";
            //Tony modified to display name of employee who removed from employeeInfo
            string Query =
               @"WITH tl AS (
                    SELECT ContactID, SUM ( CASE WHEN tl.LabourType = 2 AND tl.Active = 1 THEN tl.EndMinute - tl.StartMinute END) TotalMinutes, SUM( CASE WHEN Chargeable = 1 AND tl.Active = 1 AND tl.LabourType = 2 THEN tl.EndMinute - tl.StartMinute END) ChargeableMinutes
                    FROM TaskLabour tl
                    INNER JOIN Task t ON tl.TaskID = t.TaskID AND t.AmendedTaskID = '00000000-0000-0000-0000-000000000000'
                    WHERE LabourDate >= {0} AND LabourDate < {1} AND ContactID IN (SELECT ContactID FROM ContactCompany WHERE CompanyID = {2} AND Pending = 0) AND t.ContractorID = {2} AND t.Status != 2
                    GROUP BY ContactID
                )
                SELECT tl.ContactID, c.FirstName, c.LastName, tl.TotalMinutes, tl.ChargeableMinutes, tl.ContactID CreatedBy, GETDATE() CreatedDate, CAST(1 AS bit) Active
                FROM tl 
                LEFT JOIN Contact c ON tl.ContactID = c.ContactID 
                ORDER BY c.LastName";

            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTimeSheetSummary), Query, StartDate, EndDate, CompanyContactID);
        }

        /// <summary>
        /// Selects time sheets for a single employee of a company.
        /// </summary>
        /// <param name="CompanyContactID">The company ID.</param>
        /// <param name="ContactID">The employee ID.</param>
        /// <param name="StartDate">Start date of period (inclusive)</param>
        /// <param name="EndDate">End date of period (exclusive)</param>
        /// <returns></returns>
        public List<DOBase> SelectSingleEmployeeTimeSheets(Guid CompanyContactID, Guid ContactID, DateTime StartDate, DateTime EndDate)
        {
            string Query =
@"SELECT tl.*, j.Name JobName, j.JobNumberAuto, j.JobID, j.JobNumber, t.TaskName, t.Description JobDescription, CASE WHEN c.CompanyName <> '' THEN c.CompanyName ELSE c.FirstName + ' ' + c.LastName END Customer
FROM TaskLabour tl
LEFT JOIN Task t ON tl.TaskID = t.TaskID AND t.Status != 2 AND tl.LabourType = 2
LEFT JOIN Job j ON j.JobID = t.JobID
LEFT JOIN Contact c ON t.TaskOwner = c.ContactID
WHERE t.ContractorID = {0} AND tl.ContactID = {1} AND tl.LabourDate >= {2} AND tl.LabourDate < {3} AND t.AmendedTaskID = '00000000-0000-0000-0000-000000000000'
ORDER BY tl.LabourDate";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskLabourFull), Query, CompanyContactID, ContactID, StartDate, EndDate);

        }
        #endregion


        /// <summary>
        /// Create a task completion.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOTaskCompletion CreateTaskCompletion(Guid TaskID, Guid CreatedBy)
        {
            DOTaskCompletion TC = new DOTaskCompletion();
            TC.TaskCompletionID = Guid.NewGuid();
            TC.CreatedBy = CreatedBy;
            TC.TaskID = TaskID;

            return TC;
        }

        /// <summary>
        /// Saves a task completion.
        /// </summary>
        /// <param name="TC"></param>
        public void SaveTaskCompletion(DOTaskCompletion TC)
        {
            CurrentDAJob.SaveObject(TC);
        }

        /// <summary>
        /// Selects a task completion for a task.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DOTaskCompletion SelectTaskCompletion(Guid TaskID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskCompletion), "TaskID = {0}", TaskID) as DOTaskCompletion;
        }
        /// <summary>
        /// Deletes task completion.
        /// </summary>
        /// <param name="TaskID"></param>
        public void DeleteTaskCompletion(DOTaskCompletion TC)
        {
            CurrentDAJob.DeleteObject(TC);
        }

        #region File Uploads


        /// <summary>
        /// Save an uploaded file.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <param name="PostedFile"></param>
        /// <returns></returns>
        public DOFileUpload SaveFile(Guid CreatedBy, Guid GroupID, HttpPostedFile PostedFile, long fileSize, Guid companyID) //long fileSize, Guid companyID added by jared
        {
            if (PostedFile == null) return null;

            DOFileUpload File = new DOFileUpload();
            File.FileID = Guid.NewGuid();
            File.GroupID = GroupID;
            File.Filename = PostedFile.FileName;
            File.CreatedBy = CreatedBy;
            File.FileSize = fileSize; //added jared
            File.CompanyID = companyID; //added jared
            string Path = GetFilePathAbsolute(File);
            CreateDirIfNotExists(System.IO.Path.GetDirectoryName(Path));


            //Rename file if duplicate name.
            if (System.IO.File.Exists(Path))
            {
                int CurrentVersion = 1;
                string BaseFilename = File.Filename;

                int dIndex = File.Filename.LastIndexOf('.');
                if (dIndex < 0)
                    throw new Exception("Invalid filename");

                while (System.IO.File.Exists(Path))
                {
                    File.Filename = string.Concat(BaseFilename.Substring(0, dIndex), "-" + (++CurrentVersion).ToString(), BaseFilename.Substring(dIndex));
                    Path = GetFilePathAbsolute(File);
                }
            }
            PostedFile.SaveAs(Path);
            File.FileType = CreateThumbnails(File, PostedFile);
            CurrentDAJob.SaveObject(File);

            return File;
        }

        /// <summary>
        /// Select a file upload.
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public DOFileUpload SelectFileUpload(Guid FileID)
        {
            return CurrentDAJob.SelectObject(typeof(DOFileUpload), "FileID = {0}", FileID) as DOFileUpload;
        }


        public string GetFilePathRelative(DOFileUpload File)
        {
            return System.IO.Path.Combine(Constants.FileUploadBasePath, File.GroupID.ToString(), File.Filename);
        }

        public string GetImagePathRelative(DOFileUpload File, DOFileUpload.ImageType iType)
        {
            if (iType == DOFileUpload.ImageType.Original)
                return GetFilePathRelative(File);
            int dIndex = File.Filename.LastIndexOf('.');
            if (dIndex < 0)
                throw new Exception("Invalid filename for image");
            string ThisSizeFilename = string.Concat(File.Filename.Substring(0, dIndex), "-" + ((int)iType).ToString(), File.Filename.Substring(dIndex));
            return System.IO.Path.Combine(Constants.FileUploadBasePath, File.GroupID.ToString(), ThisSizeFilename);
        }

        public string GetFilePathAbsolute(DOFileUpload File)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(GetFilePathRelative(File));
        }

        public string GetImagePathAbsolute(DOFileUpload File, DOFileUpload.ImageType iType)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(GetImagePathRelative(File, iType));
        }

        /// <summary>
        /// Create the thumbnails for an uploaded image (if the uploaded file is an image).
        /// </summary>
        /// <param name="File"></param>
        /// <param name="PostedFile"></param>
        /// <returns></returns>
        public DOFileUpload.FileTypeEnum CreateThumbnails(DOFileUpload File, HttpPostedFile PostedFile)
        {
            try
            {
                Image i = Image.FromStream(PostedFile.InputStream);

                if (i.Width < Constants.Image_StandardWidth)
                {
                    PostedFile.SaveAs(GetImagePathAbsolute(File, DOFileUpload.ImageType.Standard));
                }
                else
                {
                    Image sImage = ScaleImage(i, Constants.Image_StandardWidth);
                    string Path = GetImagePathAbsolute(File, DOFileUpload.ImageType.Standard);
                    CreateDirIfNotExists(System.IO.Path.GetDirectoryName(Path));
                    sImage.Save(Path);
                }

                if (i.Width < Constants.Image_ThumbWidth)
                {
                    PostedFile.SaveAs(GetImagePathAbsolute(File, DOFileUpload.ImageType.Thumb));
                }
                else
                {
                    Image sImage = ScaleImage(i, Constants.Image_ThumbWidth);
                    string Path = GetImagePathAbsolute(File, DOFileUpload.ImageType.Thumb);
                    CreateDirIfNotExists(System.IO.Path.GetDirectoryName(Path));
                    sImage.Save(Path);
                }

                return DOFileUpload.FileTypeEnum.Image;
            }
            catch
            {
                return DOFileUpload.FileTypeEnum.File;
            }
        }

        private Image ScaleImage(Image i, int Width)
        {
            float scale = (float)Width / i.Width;
            int Height = (int)(i.Height * scale);
            Image sImage = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(sImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(i, new Rectangle(0, 0, Width, Height));
            }

            return sImage;
        }

        public void DeleteFileUpload(DOFileUpload File)
        {
            if (File.FileType == DOFileUpload.FileTypeEnum.Image)
            {
                DeleteIfExists(GetImagePathAbsolute(File, DOFileUpload.ImageType.Standard));
                DeleteIfExists(GetImagePathAbsolute(File, DOFileUpload.ImageType.Thumb));
            }
            DeleteIfExists(GetFilePathAbsolute(File));

            CurrentDAJob.DeleteObject(File);
        }

        private void CreateDirIfNotExists(string Path)
        {
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
        }


        private void DeleteIfExists(string Path)
        {
            if (System.IO.File.Exists(Path))
                System.IO.File.Delete(Path);
        }


        public DOJobFile CreateJobFile(Guid CreatedBy, Guid JobID, Guid FileID)
        {
            DOJobFile jf = new DOJobFile();
            jf.JobFileID = Guid.NewGuid();
            jf.JobID = JobID;
            jf.FileID = FileID;
            jf.CreatedBy = CreatedBy;
            return jf;
        }

        public DOTaskFile CreateTaskFile(Guid CreatedBy, Guid TaskID, Guid FileID)
        {
            DOTaskFile tf = new DOTaskFile();
            tf.TaskFileID = Guid.NewGuid();
            tf.TaskID = TaskID;
            tf.FileID = FileID;
            tf.CreatedBy = CreatedBy;
            return tf;
        }


        public void SaveJobFile(DOJobFile jf)
        {
            CurrentDAJob.SaveObject(jf);
        }

        public void SaveTaskFile(DOTaskFile tf)
        {
            CurrentDAJob.SaveObject(tf);
        }

        public void DeleteJobFile(DOJobFile jf)
        {
            CurrentDAJob.DeleteObject(jf);
        }

        public DOJobFile SelectJobFileByFileID(Guid FileID)
        {
            return CurrentDAJob.SelectObject(typeof(DOJobFile), "FileID = {0}", FileID) as DOJobFile;
        }

        /// <summary>
        /// Selects the list of files linked to a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectFilesForJob(Guid JobID)
        {
            string Query = "SELECT f.* FROM JobFile jf INNER JOIN FileUpload f ON jf.FileID = f.FileID WHERE jf.JobID = {0}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOFileUpload), Query, JobID);
        }

        #endregion



        /// <summary>
        /// Selects the list of files linked to a task
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public List<DOBase> SelectFilesForTask(Guid taskID)
        {
            string Query = "SELECT f.* FROM taskFile tf INNER JOIN FileUpload f ON tf.FileID = f.FileID WHERE tf.taskID = {0}";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOFileUpload), Query, taskID);
        }








        #region JobQuoting
        /// <summary>
        /// Creates a quote for a job.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public DOJobQuote CreateJobQuote(Guid CreatedBy, Guid JobID)
        {
            DOJobQuote Quote = new DOJobQuote();
            Quote.QuoteID = Guid.NewGuid();
            Quote.JobID = JobID;
            Quote.ContactID = CreatedBy;
            Quote.CreatedBy = CreatedBy;
            Quote.QuoteStatusDate = DateAndTime.GetCurrentDateTime();
            return Quote;
        }

        /// <summary>
        /// Selects a quote.
        /// </summary>
        /// <param name="QuoteID"></param>
        /// <returns></returns>
        public DOJobQuote SelectJobQuote(Guid QuoteID)
        {
            return CurrentDAJob.SelectObject(typeof(DOJobQuote), "QuoteID = {0}", QuoteID) as DOJobQuote;
        }

        /// <summary>
        /// Selects all quotes for a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobQuotes(Guid JobID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJobQuote), "JobID = {0}", JobID);
        }

        /// <summary>
        /// Saves a quote.
        /// </summary>
        /// <param name="Quote"></param>
        public void SaveJobQuote(DOJobQuote Quote)
        {
            CurrentDAJob.SaveObject(Quote);
        }

        /// <summary>
        /// Deletes a quote.
        /// </summary>
        /// <param name="Quote"></param>
        public void DeleteJobQuote(DOJobQuote Quote)
        {
            CurrentDAJob.DeleteObject(Quote);
        }



        #endregion

        #region Job Change
        public DOJobChange CreateJobChange(Guid JobID, DOJobChange.JobChangeType ChangeType, Guid CreatedBy)
        {
            DOJobChange Change = new DOJobChange();
            Change.JobChangeID = Guid.NewGuid();
            Change.JobID = JobID;
            Change.ChangeType = ChangeType;
            Change.CreatedBy = CreatedBy;
            return Change;
        }
        #endregion

        public void SaveJobChange(DOJobChange Change)
        {
            CurrentDAJob.SaveObject(Change);
        }

        public DOJobChange SelectJobChange(Guid JobChangeID)
        {
            return CurrentDAJob.SelectObject(typeof(DOJobChange), "JobChangeID = {0}", JobChangeID) as DOJobChange;
        }

        public List<DOBase> SelectJobChanges(Guid JobID, DOJobChange.JobChangeType ChangeType)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJobChange), "JobID = {0} AND ChangeType = {1} ORDER BY CreatedDate DESC", JobID, ChangeType);
        }

        public List<DOBase> SelectJobChanges(Guid JobID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOJobChange), "JobID = {0} ORDER BY CreatedDate DESC", JobID);
        }

        #region Task Quote

        /// <summary>
        /// Gets the total task quoted amount for all quote accepted tasks on a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public void GetJobTaskQuotedAmount(Guid JobID, out decimal QuotedLabourAmount, out decimal QuotedMaterialAmount)
        {
            //Tasklabour: (endminute=startminute)/60 * labourrate
            //TaskQuote.status = 1 : accepted
            //TaskLabour.Labourtype = 0 : quoted;
            string QueryL =
@"SELECT SUM(CASE WHEN Chargeable = 1 THEN  ( ((Endminute - Startminute) * LabourRate * (100 + tq.Margin)) / (60 * 100) ) END) total 
FROM TaskLabour tl LEFT JOIN TaskQuote tq ON tl.TaskID = tq.TaskID
WHERE tl.LabourType = 0 AND tl.TaskID IN 
    (SELECT tq.TaskID FROM Job j 
    INNER JOIN Task t ON j.JobID = t.JobID 
    INNER JOIN TaskQuote tq ON t.TaskID = tq.TaskID
    WHERE j.JobID = {0} AND tq.Status = 1 AND tl.LabourType = 0 AND tl.Active = 1)";

            string QueryM =
@"SELECT SUM(Quantity * tm.SellPrice * ((100 + tq.Margin) / 100)) total 
FROM TaskMaterial tm
LEFT JOIN TaskQuote tq ON tm.TaskID = tq.TaskID
WHERE tm.MaterialType = 0 AND tm.TaskID IN 
    (SELECT tq.TaskID FROM Job j 
    INNER JOIN Task t ON j.JobID = t.JobID 
    INNER JOIN TaskQuote tq ON t.TaskID = tq.TaskID
    WHERE j.JobID = {0} AND tq.Status = 1 AND tm.MaterialType = 0 AND tm.Active = 1)";

            object oLabourAmount = CurrentDAJob.ExecuteScalar(QueryL, JobID);
            object oMaterialAmount = CurrentDAJob.ExecuteScalar(QueryM, JobID);
            decimal LabourAmount = 0, MaterialAmount = 0;
            if (!(oLabourAmount == DBNull.Value))
                LabourAmount = (decimal)oLabourAmount;
            if (!(oMaterialAmount == DBNull.Value))
                MaterialAmount = (decimal)oMaterialAmount;
            QuotedLabourAmount = LabourAmount;
            QuotedMaterialAmount = MaterialAmount;
        }

        public DOTaskQuote CreateTaskQuote(Guid TaskID, Guid OwnerID)
        {
            DOTaskQuote Quote = new DOTaskQuote();
            Quote.TaskQuoteID = Guid.NewGuid();
            Quote.TaskID = TaskID;
            Quote.CreatedBy = OwnerID;
            return Quote;
        }

        /// <summary>
        /// Selects vehicle driver id by vehicleid blah blah blah
        /// </summary>
        /// <param name="VehicleID"></param>
        /// <returns></returns>
        public DOBase SelectVehicleDriverByVehicleID(Guid ContactID)
        {
            string Query = @"select VehicleDriver from Vehicle where VehicleId={0}";
            // return CurrentDAJob.SelectObjectCustomQuery(typeof(DOVehicle), Query, ContactID);
            return null;
        }




        /// <summary>
        /// Selects a Vehicle object by vehicle id
        /// </summary>
        /// <param name="VehicleID"></param>
        /// <returns></returns>
        //        public DOVehicle SelectVehicle(Guid VID)

        public DOVehicle SelectVehicleByVehicleID(Guid VID)
        {
            return CurrentDAJob.SelectObject(typeof(DOVehicle), "VehicleID={0}", VID) as DOVehicle; //was vehicle driver
        }

        // Tony added 3.Mar.2017 begin
        //jared modified 9.3.17 there was an issue with the sql repeating select and where statements. Changed from selectobject to selectobjectcustomer query. CHanged from dovehicle to dobase<t>
        public DOVehicle SelectDefaultVehicleByDriverID(Guid DriverID)
        {
            string Query = "SELECT v.* FROM vehicle v INNER JOIN EmployeeInfo e ON e.EmployeeID = v.VehicleDriver " +
                           "WHERE v.VehicleDriver = {0} AND e.DefaultVehicleID = v.VehicleID";

            List<DOBase> list = CurrentDAJob.SelectObjectCustomQuery(typeof(DOVehicle), Query, DriverID);

            if (list.Count != 0)
            {
                DOVehicle myVehicle = list[0] as DOVehicle;
                return myVehicle;
            }
            return null;

        }

       
        /// <summary>
        /// Selects a vehicle by driver id
        /// </summary>
        /// <param name="DriverID"></param>
        /// <returns></returns>
        public DOVehicle SelectVehicleByDriverID(Guid DriverID)
        {
            return CurrentDAJob.SelectObject(typeof(DOVehicle), "VehicleDriver = {0}", DriverID) as DOVehicle;
        }


        /// <summary>
        /// Saves a vehicle
        /// </summary>
        /// <param name="Vehicle"></param>
        public void SaveVehicle(DOVehicle Vehicle)
        {

            CurrentDAJob.SaveObject(Vehicle);


        }















        /// <summary>
        /// Saves a Job Template
        /// </summary>
        /// <param name="JobTemplate"></param>
        public void SaveJobTemplate(DOJobTemplate JobTemplate)
        {

            CurrentDAJob.SaveObject(JobTemplate);


        }



        /// <summary>
        /// Saves a Task Job Template
        /// </summary>
        /// <param name="JobTemplateTask"></param>
        public void SaveJobTemplateTask(DOJobTemplateTask TaskJobTemplate)
        {

            CurrentDAJob.SaveObject(TaskJobTemplate);


        }

        //public DOBase SelectSupplierInvoiceMaterialQty(Guid SupplierInvoiceMaterialID)
        //{
        //    string Query = @"select * from SupplierInvoiceMaterial where OldSupplierInvoiceMaterialID={0}";
        //    return CurrentDAJob.SelectObjectCustomQuery(typeof(DOSupplierInvoiceMaterial), Query, SupplierInvoiceMaterialID);
        //}




        //public DOSupplierInvoiceMaterial SelectSupplierInvoiceMaterialByTM(Guid TaskMaterialID)
        //{
        //    return CurrentDAJob.SelectObject(typeof(DOSupplierInvoiceMaterial), "TaskMaterialID = {0}", TaskMaterialID) as DOSupplierInvoiceMaterial;
        //}









        public void SaveTaskQuote(DOTaskQuote Quote)
        {
            CurrentDAJob.SaveObject(Quote);
        }

        public void DeleteTaskQuote(DOTaskQuote Quote)
        {
            CurrentDAJob.DeleteObject(Quote);
        }

        public DOTaskQuote SelectTaskQuote(Guid TaskQuoteID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskQuote), "TaskQuoteID = {0}", TaskQuoteID) as DOTaskQuote;
        }

        public DOTaskQuote SelectTaskQuoteByTask(Guid TaskID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskQuote), "TaskID = {0}", TaskID) as DOTaskQuote;
        }

        private List<DOBase> SelectTaskQuotesInternal(Guid TaskID)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTaskQuote), "TaskID = {0}", TaskID);
        }

        public List<DOBase> SelectTaskQuotes(Guid JobID, bool IncludeAmendedTasks)
        {
            string Query = "SELECT tq.* FROM Job j LEFT JOIN Task t on j.JobID = t.JobID INNER JOIN TaskQuote tq ON t.TaskID = tq.TaskID WHERE j.JobID = {0} AND (t.AmendedTaskID = {1} OR 1 = {2})";
            return CurrentDAJob.SelectObjectsCustomQuery(typeof(DOTaskQuote), Query, JobID, Guid.Empty, IncludeAmendedTasks);
        }
        #endregion

        #region Task Pending Contractor
        public DOTaskPendingContractor CreateTaskPendingContractor(Guid CreatedBy)
        {
            DOTaskPendingContractor TPC = new DOTaskPendingContractor();
            TPC.CreatedBy = CreatedBy;
            TPC.TPCID = Guid.NewGuid();
            return TPC;
        }

        public void SaveTaskPendingContractor(DOTaskPendingContractor TPC)
        {
            CurrentDAJob.SaveObject(TPC);
        }

        public void DeleteTaskPendingContractor(DOTaskPendingContractor TPC)
        {
            CurrentDAJob.DeleteObject(TPC);
        }

        public DOTaskPendingContractor SelectTaskPendingContractor(Guid TaskID)
        {
            return CurrentDAJob.SelectObject(typeof(DOTaskPendingContractor), "TaskID = {0}", TaskID) as DOTaskPendingContractor;
        }

        /// <summary>
        /// Selects the list of pending contractor tasks an email is associated with.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public List<DOBase> SelectPendingContractorTasks(string Email)
        {
            return CurrentDAJob.SelectObjects(typeof(DOTaskPendingContractor), "ContractorEmail = {0}", Email);
        }
        #endregion


        public string SelectJobNumberAuto(Guid ContactID)
        {
            string query =
@"SELECT MAX(CASE WHEN JobNumberAuto NOT LIKE '%[^0-9]' THEN CAST(JobNumberAuto AS INT) END) FROM 
(SELECT JobNumberAuto FROM Job j 
INNER JOIN Task t on t.JobID = j.JobID 
WHERE t.TaskType = 1 AND t.ContractorID = {0})  x";

            object val = CurrentDAJob.ExecuteScalar(query, ContactID);
            try
            {
                return ((int)val + 1).ToString();
            }
            catch
            {
                return "1";
            }
        }

        public void UpdateJobContractor(Guid jobId)
        {
            string query = @"update jobcontractor set active=0 where jobid={0}";
            CurrentDAJob.ExecuteScalar(query, jobId);
        }

        //Tony added 11.11.2016
        //Move one job to another Site
        public void MoveJob(Guid jobID, Guid oldSiteID, Guid newSiteID)
        {
            string query = @"UPDATE job SET siteID = {0} WHERE siteID ={1} AND jobID = {2}";
            CurrentDAJob.ExecuteScalar(query, newSiteID, oldSiteID, jobID);
        }

        //Move all job to another Site
        public void MoveAllJob(Guid oldSiteID, Guid newSiteID)
        {
            string query = @"UPDATE job SET siteID = {0} WHERE siteID ={1}";
            CurrentDAJob.ExecuteScalar(query, newSiteID, oldSiteID);
        }

        //Move one task to another job
        public void MoveTask(Guid taskID, Guid oldJobID, Guid newJobID)
        {
            string query = @"UPDATE task SET jobID = {0} WHERE jobID ={1} AND taskID = {2}";
            CurrentDAJob.ExecuteScalar(query, newJobID, oldJobID, taskID);
        }

        //Move all tasks to another job
        public void MoveAllTask(Guid oldJobID, Guid newJobID)
        {
            string query = @"UPDATE task SET jobID = {0} WHERE jobID ={1}";
            CurrentDAJob.ExecuteScalar(query, newJobID, oldJobID);
        }
        //Tony added 11.11.2016
    }
}
