using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
	public class BRTradeCategory : BRBase
	{
		private DATradeCategory _CurrentDATradeCategory;
		private DATradeCategory CurrentDATradeCategory
		{
			get
			{
				if (_CurrentDATradeCategory == null)
					_CurrentDATradeCategory = new DATradeCategory(ConnectionString);
				return _CurrentDATradeCategory;
			}
		}
		private DASubTradeCategory _CurrentDASubTradeCategory;
		private DASubTradeCategory CurrentDASubTradeCategory
		{
			get
			{
				if (_CurrentDASubTradeCategory == null)
					_CurrentDASubTradeCategory = new DASubTradeCategory(ConnectionString);
				return _CurrentDASubTradeCategory;
			}
		}

		//Insert Trade Category
		public void SaveTradeCategory(DOTradeCategory TradeCategory)
		{

			CurrentDATradeCategory.SaveObject(TradeCategory);


		}
		//Get trade category
		public List<DOBase> SelectTradeCategories()
		{
			return CurrentDATradeCategory.SelectObjectsOrderBy(typeof(DOTradeCategory), "TradeCategoryName");
		}

		//Get SubTradeCategories according to tradecategory
		public List<DOBase> SelectSubTradeCategories(Guid tradeCategoryID)
		{
			return CurrentDASubTradeCategory.SelectObjectsOrderByWhereClause(typeof(DOSubTradeCategory), "TradeCategoryID = {0}", "SubTradeCategoryName", tradeCategoryID);
		}

		//Add SubTradeCategory
		public void SaveSubTradeCategory(DOSubTradeCategory subTradeCategory)
		{
			CurrentDASubTradeCategory.SaveObject(subTradeCategory);
		}

		/// <summary>
		/// Find the default trade category for a contact
		/// </summary>
		/// <param name="contactId">contact id</param>
		/// <returns></returns>
		public DOBase SelectDefaultTradeCategory(Guid contactId)
		{
			StringBuilder query = new StringBuilder(@" SELECT tg.TradeCategoryName,
																tg.TradeCategoryID,
																tg.Active,
																tg.CreatedBy,
																tg.CreatedDate 
														FROM TradeCategory tg
														INNER JOIN ContactTradeCategory ctg ON tg.TradeCategoryID = ctg.TradeCategoryID AND ctg.DefaultTradeCategory=1");
			return CurrentDATradeCategory.SelectQuery(typeof(DOTradeCategory), query, "ctg.ContactID= {0}", contactId);
		}

		//Find Trade Category name by ID
		public DOTradeCategory FindTradeCategoryName(Guid tradeCategoryID)
		{
			return CurrentDASubTradeCategory.SelectObject(typeof(DOTradeCategory), "TradeCategoryID={0}", tradeCategoryID) as DOTradeCategory;
		}

		//Find Trade Category name by subtradecategoryID
		public List<DOBase> FindTradeCategoryNamebySubTrade(Guid subTradeCategoryID)
		{
			string query = "SELECT * FROM TradeCategory WHERE TradeCategoryID = (SELECT TradeCategoryID FROM SubTradeCategory WHERE SubTradeCategoryID={0})";
			return CurrentDASubTradeCategory.SelectObjectsCustomQuery(typeof(DOTradeCategory), query, subTradeCategoryID);
		}
	}
}
