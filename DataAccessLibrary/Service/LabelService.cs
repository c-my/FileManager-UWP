using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.Sqlite;

namespace DataAccessLibrary.Service {
    public class LabelService {
        public static void InitializeDatabase() {
            using (SqliteConnection db =
                new SqliteConnection("Filename=awesomefilemanager.db")) {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS Labels (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Path NVARCHAR(2048) NOT NULL, LABEL NVARCHAR(2048) NOT NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddLabel(string path, string label) {
            using (SqliteConnection db =
                new SqliteConnection("Filename=awesomefilemanager.db")) {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Labels (Path, LABEL) VALUES (@Path, @Label);";
                insertCommand.Parameters.AddWithValue("@Path", path);
                insertCommand.Parameters.AddWithValue("@Label", label);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static List<String> GetLabels(string path) {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=awesomefilemanager.db")) {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT LABEL from Labels where path=@Path", db);
                selectCommand.Parameters.AddWithValue("@Path", path);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read()) {
                    entries.Add(query.GetString(0));
                }
                db.Close();
            }
            return entries;
        }

        public static void RemoveLabel(string path, string label) {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=awesomefilemanager.db")) {
                db.Open();

                SqliteCommand removeCommand = new SqliteCommand
                    ("DELETE from Labels where path=@Path and label=@Label", db);
                removeCommand.Parameters.AddWithValue("@Path", path);
                removeCommand.Parameters.AddWithValue("@Label", label);

                SqliteDataReader query = removeCommand.ExecuteReader();

                db.Close();
            }
        }
    }
}
