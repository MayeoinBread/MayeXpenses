using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;

namespace MayeXpenses.DatabaseAccess
{
    public class DBFunctions
    {

        #region DB Get
        public static ObservableCollection<Cost> GetAllCosts()
        {
            ObservableCollection<Cost> ret = new ObservableCollection<Cost>();

            string query = "SELECT * FROM " + DBSetup.costTable + " ORDER BY " + DBSetup.costDate + " DESC;";

            using (SqliteConnection db = new SqliteConnection(DBSetup.DBFileName))
            {
                db.Open();

                using (SqliteCommand dbQuery = new SqliteCommand(query, db))
                {
                    try
                    {
                        using(SqliteDataReader reader = dbQuery.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.Add(new Cost(
                                    (long)reader[DBSetup.costID],
                                    reader[DBSetup.costDate].ToString(),
                                    reader[DBSetup.costDesc].ToString(),
                                    (Currencies)Int64.Parse(reader[DBSetup.costCurrency].ToString()),
                                    // Envisage this might have a problem
                                    System.Single.Parse(reader[DBSetup.costValue].ToString()),
                                    reader[DBSetup.costReceipt].ToString()));
                            }
                            reader.Dispose();
                        }
                    }
                    catch(SqliteException error)
                    {
                        System.Diagnostics.Debug.WriteLine(error);
                        return null;
                    }
                }
            }

            return ret;
        }

        public static Cost GetCostById(long id)
        {
            string query = "SELECT * FROM " + DBSetup.costTable + " WHERE " + DBSetup.costID + " = " + id + ";";
            Cost c = null;

            using(SqliteConnection db = new SqliteConnection(DBSetup.DBFileName))
            {
                db.Open();

                using(SqliteCommand selectCommand = new SqliteCommand(query, db))
                {
                    try
                    {
                        using(SqliteDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                                c = new Cost(
                                    (long)reader[DBSetup.costID],
                                    reader[DBSetup.costDate].ToString(),
                                    reader[DBSetup.costDesc].ToString(),
                                    (Currencies)Int64.Parse(reader[DBSetup.costCurrency].ToString()),
                                    // Envisage this might have a problem
                                    System.Single.Parse(reader[DBSetup.costValue].ToString()),
                                    reader[DBSetup.costReceipt].ToString());
                            reader.Dispose();
                        }
                    }
                    catch(SqliteException error)
                    {
                        System.Diagnostics.Debug.WriteLine(error);
                        return null;
                    }
                }
            }
            return c;
        }
        #endregion

        #region DB Save
        private static void ExecuteDBCommand(string command, Cost c)
        {
            using(SqliteConnection db = new SqliteConnection(DBSetup.DBFileName))
            {
                db.Open();
                using(SqliteCommand insertUpdateCommand = new SqliteCommand(command, db))
                {
                    insertUpdateCommand.Parameters.AddWithValue("@date", c.Date.ToString());
                    insertUpdateCommand.Parameters.AddWithValue("@description", c.Description);
                    insertUpdateCommand.Parameters.AddWithValue("@currency", (int)c.Currency);
                    insertUpdateCommand.Parameters.AddWithValue("@value", c.Value);
                    insertUpdateCommand.Parameters.AddWithValue("@receipt", c.ReceiptName);
                    if (c.Id > -1)
                        insertUpdateCommand.Parameters.AddWithValue("@Id", c.Id);

                    try
                    {
                        insertUpdateCommand.ExecuteNonQuery();
                    }
                    catch(SqliteException error)
                    {
                        System.Diagnostics.Debug.WriteLine(error);
                        return;
                    }
                }
            }
        }

        public static void SaveCost(Cost cost)
        {
            if(cost.Id == -1)
            {
                InsertNewCost(cost);
            }
            else
            {
                UpdateCost(cost);
            }
        }

        private static void InsertNewCost(Cost cost)
        {
            string command = "INSERT INTO " + DBSetup.costTable + " VALUES (NULL, @date, @description, @currency, @value, @receipt);";
            ExecuteDBCommand(command, cost);
        }

        private static void UpdateCost(Cost cost)
        {
            string command = "UPDATE " + DBSetup.costTable +
                " SET " + DBSetup.costDate + " = @date, " + DBSetup.costDesc + " = @description, " +
                DBSetup.costCurrency + " = @currency, " + DBSetup.costValue + " = @value,  " +
                DBSetup.costReceipt + " = @receipt" +
                " WHERE " + DBSetup.costID + " = @Id;";
            ExecuteDBCommand(command, cost);
        }
        #endregion

        #region DB Delete
        public static void DeleteCost(Cost cost)
        {
            string query = "DELETE FROM " + DBSetup.costTable + " WHERE " + DBSetup.costID + " = " + cost.Id + ";";

            using(SqliteConnection db = new SqliteConnection(DBSetup.DBFileName))
            {
                db.Open();

                using (SqliteCommand deleteCommand = new SqliteCommand(query, db))
                {
                    try
                    {
                        deleteCommand.ExecuteReader();
                    }
                    catch(SqliteException error)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ToString());
                        return;
                    }
                }
                db.Close();
            }
        }
        #endregion
    }
}
