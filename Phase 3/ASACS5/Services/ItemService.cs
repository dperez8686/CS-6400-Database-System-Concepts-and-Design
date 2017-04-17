using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

using ASACS5.Models.Items;

namespace ASACS5.Services
{
    public static class ItemService
    {
        public static Item GetItemById(int ItemID)
        {
            Item response = null;

            string sql = "SELECT ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID FROM item WHERE ItemID = " + ItemID;

            // run the SQL
            object[] queryResponse = SqlHelper.ExecuteSingleSelect(sql, 7);

            if (queryResponse != null)
            {
                response = new Item
                {
                    ItemID = ItemID,
                    ItemName = queryResponse[0].ToString(),
                    NumberOfUnits = int.Parse(queryResponse[1].ToString()),
                    ExpirationDate = DateTime.Parse(queryResponse[2].ToString()),
                    StorageType = queryResponse[5].ToString(),
                    SiteID = int.Parse(queryResponse[6].ToString()),
                    Category1 = queryResponse[3].ToString(),
                    Category2 = queryResponse[4].ToString()
                };
            }

            return response;
        }

    }
}