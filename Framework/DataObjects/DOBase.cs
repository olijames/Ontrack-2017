using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Electracraft.Framework.DataObjects
{
    public enum ObjectPersistenceStatus
    {
        /// <summary>New object not yet saved in database</summary>
        New,
        /// <summary>Existing object loaded from database</summary>
        Existing
    }

    public abstract class DOBase
    {
        public ObjectPersistenceStatus PersistenceStatus { get; set; }

        protected DOBase()
        {
            this.PersistenceStatus = ObjectPersistenceStatus.New;
            this.CreatedDate = GetCurrentDateTime();
            this.Active = true;
        }

        #region Common Fields

        [DatabaseField("CreatedBy")]
        public Guid CreatedBy { get; set; }

        [DatabaseField("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DatabaseField("Active")]
        public bool Active { get; set; }
        #endregion

        private static DateTime GetCurrentDateTime()
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
        }

    }
}
