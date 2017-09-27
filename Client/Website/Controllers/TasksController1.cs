using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using Electracraft.Client.Website.UserControls;
using Electracraft.Framework.BusinessRules;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Web;


namespace Electracraft.Client.Website.Controllers
{

    public class TasksController1 : ApiController
    {
        List<DOBase> taskList = new List<DOBase>();
        readonly BRJob _currentBrJob = new BRJob();
        readonly BRContact _currentBrContact = new BRContact();
        PageBase pb = new PageBase();

        private string _errorMessage;
        private SessionContext _currentSessionContext;
        public SessionContext CurrentSessionContext
        {
            get
            {
                var session = HttpContext.Current.Session;
                _currentSessionContext = session[Constants.SessionCurrentContext] as SessionContext;

                
                //if (_currentSessionContext!=null)
                //{
                //    session["CurrentSession"] = session[Constants.SessionCurrentContext] as SessionContext;
                //}
                //else
                //{
                //    //_currentSessionContext =(CurrentSessionContext) HttpContext.Current.Session;
                //     _currentSessionContext = session["CurrentSession"] as SessionContext;     
                //    //HttpContextWrapper httpContextWrapper=new HttpContextWrapper(HttpContext.Current);
                //    //  httpContextWrapper = (HttpContextWrapper) Request.Properties["MS_HttpContext"];
                //    // session=httpContextWrapper.Session[]

                //}

              
                return _currentSessionContext;
            }
        }


        /// <summary>
        /// Get all the tasksby taking jobID from current session
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("TaskByJobID")]
        public List<DOBase> GetTasks()
        {
            taskList = _currentBrJob.SelectTasks(CurrentSessionContext.CurrentJob.JobID);
            return taskList;
        }


        /// <summary>
        /// Select all materials
        /// </summary>
        /// <param name="id">TaskID</param>
        /// <returns>Materials list</returns>
        [HttpGet]
        [ActionName("Materials")]
        public List<DOBase> GetMaterials(Guid id)
        {
            List<DOBase> materials = _currentBrJob.SelectTaskMaterialsList(id);
            return materials;
        }

        /// <summary>
        /// Get the contact information 
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns>contact info</returns>
        [HttpGet]
        [ActionName("Contact")]
        public DOContactInfo GetContact(Guid id)
        {
            return _currentBrContact.SelectAContact(id);
        }

        /// <summary>
        /// Get all the tasks by JobID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of tasks</returns>
        [HttpGet]
        [ActionName("TaskByJobID")]
        public List<DOBase> GetTasks(Guid id)
        {
            taskList = _currentBrJob.SelectTasks(id);
            return taskList;
        }

        /// <summary>
        /// Get all the tasks by TaskID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("TaskByTaskID")]
        public DOTask GetTaskbyTaskId(Guid id)
        {
            DOTask task = new DOTask();
            if (id != Guid.Empty)
            {
                task = _currentBrJob.SelectTask(id);
            }
            return task;
        }


        /// <summary>
        /// Get job details
        /// </summary>
        /// <param name="id">jobid</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Job")]
        public DOJob GetJob(Guid id)
        {
            DOJob job = _currentBrJob.SelectJob(id);
            return job;
        }
        /// <summary>
        /// Get all the contractors
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of job contractors</returns>
        [HttpGet]
        [ActionName("Contractors")]
        public List<DOBase> GetContractors(Guid id)
        {
            // return CurrentBRJob.SelectActiveJobContractors(id);
            List<DOBase> jobContractors = _currentBrContact.SelectJobContractorContacts(id);
            return jobContractors;
        }

        /// <summary>
        /// Get all the labour
        /// </summary>
        /// <param name="id">task id</param>
        /// <returns>List of labour</returns>
        [HttpGet]
        [ActionName("Labour")]
        public List<DOBase> GetLabour(Guid id)
        {
            List<DOBase> taskLabour = _currentBrJob.SelectTaskLabours(id);
            return taskLabour;
        }
        [HttpPut]
        [ActionName("Update")]
        public DOTask Save(Guid id, string startDate = "", string endDate = "", string description = "", string contractorID = "",
            string taskName = "", string status = "", string tradeCategoryId = "", string taskCustomerId = "")
        {
            bool taskchange = false;
            DOTask task = _currentBrJob.SelectTask(id);
            if (startDate != "")
            {
                task.StartDate = DateTime.Parse(startDate);
                taskchange = true;
            }
            if (endDate != "")
            {
                task.EndDate = DateTime.Parse(endDate);
                taskchange = true;
            }
            if (description != "")
            {
                task.Description = description;
                taskchange = true;
            }
            if (contractorID != "")
            {
                task.ContractorID = Guid.Parse(contractorID);
                taskchange = true;
            }
            if (taskName != "")
            {
                task.TaskName = taskName;
                taskchange = true;
            }
            if (status != "")
            {
                DOTask.TaskStatusEnum s = (DOTask.TaskStatusEnum)Enum.Parse(typeof(DOTask.TaskStatusEnum), status);
                if (s == DOTask.TaskStatusEnum.Complete)
                {
                    Validations.Validations validateObj = new Validations.Validations();
                    bool hasActual = validateObj.ValidateMaterialsLabour(task.TaskID);
                    if (!hasActual)
                    {
                        _errorMessage = "The task '" + task.TaskName +
                                       "' cannot be completed because no materials or labour have been assigned.";
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                        {
                            Content = new StringContent(_errorMessage)
                        });
                    }
                    task.Status = s;

                }
                else if (s == DOTask.TaskStatusEnum.Incomplete)
                {
                    task.Status = s;
                }
                else if (s == DOTask.TaskStatusEnum.Invoiced)
                {
                    if (task.Status == DOTask.TaskStatusEnum.Complete || task.Status == DOTask.TaskStatusEnum.Paid)
                        task.Status = s;
                }
                else if (s == DOTask.TaskStatusEnum.Paid)
                {
                    if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                        task.Status = s;
                }
                else if (s == DOTask.TaskStatusEnum.WrittenOff)
                {
                    if (task.Status == DOTask.TaskStatusEnum.Invoiced)
                        task.Status = s;
                }
                if (task.Status != s)
                {
                    _errorMessage = "The task '" + task.TaskName +
                                     "' cannot be " + s;
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent(_errorMessage)
                    });
                }
                taskchange = true;
            }
            if (tradeCategoryId != "")
            {
                task.TradeCategoryID = Guid.Parse(tradeCategoryId);
                taskchange = true;
            }
            if (taskCustomerId != "")
            {
                task.TaskCustomerID = Guid.Parse(taskCustomerId);
                taskchange = true;
            }
            if (taskchange)
            {
                if (CurrentSessionContext != null)
                {
                    DOTaskCompletion tc = _currentBrJob.CreateTaskCompletion(task.TaskID, CurrentSessionContext.Owner.ContactID);
                    DOTaskCompletion taskCompleted = _currentBrJob.SelectTaskCompletion(task.TaskID);
                    if (taskCompleted == null)
                        _currentBrJob.SaveTaskCompletion(tc);
                    else
                    {
                        _currentBrJob.DeleteTaskCompletion(taskCompleted);
                        _currentBrJob.SaveTaskCompletion(tc);
                    }
                }
                _currentBrJob.SaveTask(task);
                JobTasks jobTasks = new JobTasks();
                if (CurrentSessionContext != null)
                {
                    jobTasks.UpdateRecords(task.JobID, task.SiteID, task.TaskCustomerID);
                }
            }

            return task;
        }
        /// <author>Jared</author>
        /// <summary>
        /// Add a task through the Task Controller directly
        /// </summary>
        /// <param name="id">job id</param>
        /// <returns>Adds a new task</returns>
        [HttpPut]
        [ActionName("AddTask")]
        public DOTask AddTask(Guid id,string taskName = "", string taskType = "", string startDate = "", string endDate = "", string startMinute = "",
            string endMinute = "", string appointment = "", string description = "", string contractorID = "", string status = "",
            string tradeCategoryId = "", string taskCustomerId = "", string contactid = "", string siteid = "")
        {
            bool taskchange = false;


            DOTask AddTask = _currentBrJob.CreateTask(id, Guid.Parse(contactid));//here

            AddTask.TaskID = Guid.NewGuid();
            AddTask.JobID = id;
            AddTask.SiteID = Guid.Parse(siteid);
            AddTask.CreatedBy = Guid.Parse(contactid);
            AddTask.CreatedDate = DateTime.Now;
            AddTask.Active = true;
            AddTask.ParentTaskID = Guid.Parse("00000000-0000-0000-0000-000000000000");







            //below - figure out next task number
            //if (_currentSessionContext.CurrentTask == null)
            //{
            List<DOBase> tasksList = _currentBrJob.SelectTasks(CurrentSessionContext.CurrentJob.JobID);
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
            taskNumber++;
            AddTask = _currentBrJob.CreateTask(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.Owner.ContactID);
            AddTask.TaskNumber = taskNumber++;
            //}
            //else
            //{
            //    AddTask.TaskNumber = 0;
            //}


            if (appointment != "")
            {
                AddTask.Appointment = true;
                taskchange = true;
            }
            if (startMinute != "")
            {
                AddTask.StartMinute = int.Parse(startMinute);
                taskchange = true;
            }
            if (endMinute != "")
            {
                AddTask.EndMinute = int.Parse(endMinute);
                taskchange = true;
            }
            if (startDate != "")
            {
                AddTask.StartDate = DateTime.Parse(startDate);
                taskchange = true;
            }
            if (endDate != "")
            {
                AddTask.EndDate = DateTime.Parse(endDate);
                taskchange = true;
            }
            if (description != "")
            {
                AddTask.Description = description;
                taskchange = true;
            }
            if (contractorID != "")
            {
                AddTask.ContractorID = Guid.Parse(contractorID);
                taskchange = true;
            }
            else
            {
                return null;
            }



            if (taskName != "")
            {
                AddTask.TaskName = taskName;
                taskchange = true;
            }
            else
            {
                return null;
            }
            if (taskType != "")
            {
                DOTask.TaskTypeEnum tt = (DOTask.TaskTypeEnum)Enum.Parse(typeof(DOTask.TaskTypeEnum), taskType);
                AddTask.TaskType = tt;
                taskchange = true;
            }

            if (status != "")
            {
                DOTask.TaskStatusEnum s = (DOTask.TaskStatusEnum)Enum.Parse(typeof(DOTask.TaskStatusEnum), status);
                if (s == DOTask.TaskStatusEnum.Complete)
                {
                    Validations.Validations validateObj = new Validations.Validations();
                    bool hasActual = validateObj.ValidateMaterialsLabour(AddTask.TaskID);
                    if (!hasActual)
                    {
                        _errorMessage = "The task '" + AddTask.TaskName +
                                       "' cannot be completed because no materials or labour have been assigned.";
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                        {
                            Content = new StringContent(_errorMessage)
                        });
                    }

                }
                else if (s == DOTask.TaskStatusEnum.Incomplete)
                {
                    AddTask.Status = s;
                }
                else if (s == DOTask.TaskStatusEnum.Invoiced)
                {
                    if (AddTask.Status == DOTask.TaskStatusEnum.Complete || AddTask.Status == DOTask.TaskStatusEnum.Paid)
                        AddTask.Status = s;
                }
                else if (s == DOTask.TaskStatusEnum.Paid)
                {
                    if (AddTask.Status == DOTask.TaskStatusEnum.Invoiced)
                        AddTask.Status = s;
                }
                else
                {
                    _errorMessage = "The task '" + AddTask.TaskName +
                                     "' cannot be " + s + " unless it is " + AddTask.Status;
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent(_errorMessage)
                    });
                }
                taskchange = true;
            }

            if (tradeCategoryId != "")
            {
                AddTask.TradeCategoryID = Guid.Parse(tradeCategoryId);
                taskchange = true;
            }
            if (taskCustomerId != "")
            {
                AddTask.TaskCustomerID = Guid.Parse(taskCustomerId);
                taskchange = true;
            }
            if (taskchange)
            {
                _currentBrJob.SaveTask(AddTask);
            }

            return AddTask;
        }

    }
}
