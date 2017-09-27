using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
   public class BRContactTradeCategory:BRBase
    {
        private DAContactTradeCategory _CurrentDAContactTradeCategory;
        private DAContactTradeCategory CurrentDAContactTradeCategory
        {
            get
            {
                if (_CurrentDAContactTradeCategory == null)
                    _CurrentDAContactTradeCategory = new DAContactTradeCategory(ConnectionString);
                return _CurrentDAContactTradeCategory;
            }
        }
        //Save Trade category and Contact in ContactTradeCategory table
        public void SaveContactTradecategory(DOContactTradeCategory ctc)
        {
            
                CurrentDAContactTradeCategory.SaveObject(ctc);
           
        }
        //Select Existing trade categories for a contact
        public List<DOBase> SelectCTC(Guid contactID)
        {
            return CurrentDAContactTradeCategory.SelectObjects(typeof(DOContactTradeCategory),"ContactID={0}",contactID);
        }
        //Delete the existing mapping for contactID and tradecategoryID
        public void DeleteCTC(DOContactTradeCategory c)
        {
            CurrentDAContactTradeCategory.DeleteObject(c);
        }
        //Select all contractors for subscribed users according to sububrb and tradecategory
        public List<DOBase> SelectContractors(Guid SuburbID, Guid SubTradeCategoryID)
        {
            string query = "select c.* from Contact c, ContactTradeCategory t, OperatingSites s where "
                + " s.SuburbID={0} and t.SubTradeCategoryID = {1} and t.ContactID=s.ContactID and c.ContactID = t.ContactID and c.ContactID = s.ContactID";

            List<DOBase> contractors = 
                CurrentDAContactTradeCategory.SelectObjectsCustomQuery(typeof(DOContact), query, SuburbID, SubTradeCategoryID);
            return contractors;
        }
        ////select contractors for suburb and subtradecategory
        //public List<DOBase> SelectContractorsWithSuburbTradeCategory(Guid SuburbID, Guid SubTradeCategoryID)
        //{
        //    List<DOBase> contractors = new List<DOBase>();
        //    string query = "select t.ContactID from ContactTradeCategory t, OperatingSites s where  s.SuburbID={0} and t.SubTradeCategoryID = {1} and t.ContactID=s.ContactID";
        //    contractors = CurrentDAContactTradeCategory.SelectObjectsCustomQuery(typeof(DOContact), query, SuburbID, SubTradeCategoryID);
        //    return contractors;
        //}


        public List<DOBase> FindExistingSubTradeCategory(Guid subTradeCategoryID)
        {
          return CurrentDAContactTradeCategory.SelectObjects(typeof(DOContactTradeCategory), "SubTradeCategoryID = {0}", subTradeCategoryID);
        }
        //Find contacts who are searchable and with a particular tradecategory
        public List<DOBase> SelectContractorsbyTradeCat(Guid tradecategoryID)
        {
            string query = "select * from Contact c, ContactTradeCategory ct where ct.TradeCategoryID={0} and ct.ContactID=c.ContactID ";
            return CurrentDAContactTradeCategory.SelectObjectsCustomQuery(typeof(DOContactTradeCatInfo),query, tradecategoryID);
        }
        //-------------------------------------------------------------
        //                     cc trade categories below
        //-------------------------------------------------------------

        //Select Existing trade categories for a contact
        public List<DOBase> SelectMyContractorsTC(Guid contractorCustomerID)
        {
            return CurrentDAContactTradeCategory.SelectObjects(typeof(DOMyContractorsTradeCategory), "ContactorCustomerID={0}", contractorCustomerID);
        }

    }
}
