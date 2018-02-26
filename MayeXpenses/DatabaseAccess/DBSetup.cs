using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;

namespace MayeXpenses.DatabaseAccess
{
    public class DBSetup
    {
        public static readonly string DBFileName = "Filename=MayeXpenses.db";
        public static readonly string costTable = "tb_cost";

        public static readonly string costID = "cost_id";
        public static readonly string costDate = "cost_date";
        public static readonly string costDesc = "cost_desc";
        public static readonly string costCurrency = "cost_currency";
        public static readonly string costValue = "cost_value";
        public static readonly string costReceipt = "cost_receipt";

        private static string dbSetup =
            "CREATE TABLE IF NOT EXISTS " + costTable + " (" + costID + " INTEGER PRIMARY KEY AUTOINCREMENT, " + costDate + " TEXT NOT NULL, " +
            costDesc + " TEXT NOT NULL, " + costCurrency + " INTEGER NOT NULL, " + costValue + " REAL NOT NULL, " + costReceipt + " TEXT);";

        public static void CreateDB()
        {
            SqliteEngine.UseWinSqlite3();

            using(SqliteConnection db = new SqliteConnection(DBFileName))
            {
                db.Open();
                using(SqliteCommand createTable = new SqliteCommand(dbSetup, db))
                {
                    try
                    {
                        createTable.ExecuteNonQuery();
                    }
                    catch(SqliteException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}
