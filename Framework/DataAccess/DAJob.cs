using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;

namespace Electracraft.Framework.DataAccess
{
    public class DAJob : DABase
    {
        public DAJob(string ConnectionString)
            : base(ConnectionString)
        { }
        

        public override void Validate(DOBase obj)
        {
            DOJob Job = obj as DOJob;
            if (Job != null)
            {
                if (string.IsNullOrEmpty(Job.Name))
                    throw new FieldValidationException("Job Name is required.");

                if (Job.ProjectManagerID == Constants.Guid_DefaultUser && string.IsNullOrEmpty(Job.ProjectMangerText))
                {
                    throw new FieldValidationException("Project Manager must be specified when selecting 'Other' option.");
                }
            }

            DOTask Task = obj as DOTask;
            if (Task != null)
            {
                if (string.IsNullOrEmpty(Task.TaskName))
                    throw new FieldValidationException("Task Name is required.");
                if (string.IsNullOrEmpty(Task.Description))
                    throw new FieldValidationException("Task Description is required.");

                if (Task.Appointment)
                {
                    if (Task.StartDate == DateAndTime.NoValueDate || Task.StartMinute < 0)
                    {
                        throw new FieldValidationException("Task Start Date and Start Time must be specified for an appointment task.");
                    }
                }
            }

            DOTaskLabour TaskLabour = obj as DOTaskLabour;
            if (TaskLabour != null)
            {
                if (TaskLabour.StartMinute < 0)
                    throw new FieldValidationException("Start Time is required.");
                if (TaskLabour.EndMinute < 0)
                    throw new FieldValidationException("End Time is required.");
                if (TaskLabour.EndMinute < TaskLabour.StartMinute)
                    throw new FieldValidationException("End Time cannot be earlier that Start Time.");
                if (TaskLabour.LabourDate <= DateAndTime.NoValueDate)
                    throw new FieldValidationException("Labour Date is required.");
            }
            //DOJobTimeSheet TimeSheet = obj as DOJobTimeSheet;
            //if (TimeSheet != null)
            //{
            //    if (TimeSheet.StartMinute < 0)
            //        throw new FieldValidationException("Start Time is required.");
            //    if (TimeSheet.EndMinute < 0)
            //        throw new FieldValidationException("End Time is required.");
            //    if (TimeSheet.TimeSheetDate == DateAndTime.NoValueDate)
            //        throw new FieldValidationException("Date is required.");
            //}
        }

       
        
    }
}
