using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOContactWithJobValue : DOContactBase
    {

        //added by Jared
        //[Flags]
        //public enum ContactSettingsEnum
        //{

        //    MyProfileMinimised = 1,
        //    MyProfileOnTop = 2,
        //    MyProfileOnBottom = 4,
        //    StartUpBubble = 8,
        //    TutorialOne = 16,
        //    TutorialTwo = 32,
        //    TutorialThree = 64,
        //    TutorialFour = 128,
        //    

        //}
        //added above J

        public enum ContactTypeEnum : int
        {
            Company = 0,
            Individual = 1
        }

        [DatabaseField("ContactCustomerID", IsPrimaryKey = true)]
        public Guid ContactID { get; set; }

        public override Guid ID
        {
            get { return ContactID; }
        }
        public override string ContactBaseType
        {
            get { return "Contact"; }
        }

        [DatabaseField("CustomerType")]
        public ContactTypeEnum ContactType { get; set; }

       

        public override string DisplayName
        {
            get
            {
                if (ContactType == ContactTypeEnum.Company)
                    return CompanyName;
                else
                    return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        

        [DatabaseField("Total")]
        public decimal TotalJobValue { get; set; }
    }
}
