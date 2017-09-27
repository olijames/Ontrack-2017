using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("GeneralSetting")]
    public class DOGeneralSetting : DOBase
    {
        [DatabaseField("SettingID", IsPrimaryKey = true)]
        public Guid SettingID { get; set; }

        [DatabaseField("SettingName")]
        public string SettingName { get; set; }

        [DatabaseField("SettingValue")]
        public string SettingValue { get; set; }
    }
}
