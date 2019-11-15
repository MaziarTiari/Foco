using Microsoft.Data.Sqlite;

namespace Foco
{

    class DatabaseManager
    {

        private SqliteConnection sqliteConnection;
        private SqliteCommand sqliteCommand;

        /**
         * <summary>Erstellt ein neues Objekt zur Kommunikation mit der Datenbank.
         * Die Verbindung muss anschließend mit <c>Connect()</c> hergestellt und
         * am Ende wieder mit <c>Disconnect()</c> getrennt werden.</summary>
         * <param name="filePath">Der Dateipfad zur SQLite-Datenbank</param>
         */
        public DatabaseManager(string filePath)
        {
            SqliteConnectionStringBuilder stringBuilder = new SqliteConnectionStringBuilder();
            stringBuilder.DataSource = filePath;
            stringBuilder.ForeignKeys = true;
            stringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            sqliteConnection = new SqliteConnection(stringBuilder.ConnectionString);
            sqliteCommand = new SqliteCommand();
            sqliteCommand.Connection = sqliteConnection;
        }

        /**
         * <summary>Der Destruktor trennt die Verbindung zur Datenbank falls dies vergessen wurde.</summary>
         */
        ~DatabaseManager()
        {
            Disconnect();
        }

        /**
         * <summary>Versucht eine Verbindung zur SQLite-Datenbank herzustellen.</summary>
         * <returns>true bei erfolgreicher Verbindung, ansonsten false</returns>
         */
        public bool Connect()
        {
            try
            {
                sqliteConnection.Open();
                return CreateTables();
            }
            catch
            {
                return false;
            }
        }

        /**
         * <summary>Trennt eine zuvor erstellte Verbindung zur Datenbank wieder.</summary>
         */
        public void Disconnect()
        {
            sqliteConnection.Close();
        }

        /**
         * <summary>Erstellt alle benötigten Tabellen falls noch nicht vorhanden.</summary>
         * <returns>true bei Erfolg, false falls ein Fehler auftrat</returns>
         */
        private bool CreateTables()
        {
            sqliteCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS State(
                    id INTEGER NOT NULL,
                    bez TEXT NOT NULL,
                    PRIMARY KEY(id)
                );
                CREATE TABLE IF NOT EXISTS Priority(
        	        id INTEGER NOT NULL,
        	        bez TEXT NOT NULL,
        	        PRIMARY KEY(id)
                );
                CREATE TABLE IF NOT EXISTS Goal(
        	        id INTEGER NOT NULL,
        	        title TEXT NOT NULL,
        	        PRIMARY KEY(id)
                );
                CREATE TABLE IF NOT EXISTS Project(
        	        id INTEGER NOT NULL,
        	        title TEXT NOT NULL,
        	        goal_id INTEGER NOT NULL,
        	        PRIMARY KEY(id),
        	        FOREIGN KEY(goal_id) REFERENCES Goal(id)
                );
                CREATE TABLE IF NOT EXISTS TaskGroup(
	                id INTEGER NOT NULL,
	                title TEXT NOT NULL,
	                project_id INTEGER NOT NULL,
	                state_id INTEGER NOT NULL,
	                priority_id INTEGER, 
	                deadline DATETIME,
	                PRIMARY KEY(id),
	                FOREIGN KEY(project_id) REFERENCES Project(id),
	                FOREIGN KEY(state_id) REFERENCES State(id),
	                FOREIGN KEY(priority_id) REFERENCES Priority(id)
                );
                CREATE TABLE IF NOT EXISTS Task(
	                id INTEGER NOT NULL,
	                title TEXT NOT NULL,
	                done BOOLEAN NOT NULL,
	                taskgroup_id INTEGER NOT NULL,
	                PRIMARY KEY(id),
	                FOREIGN KEY(taskgroup_id) REFERENCES TaskGroup(id)
                );
                CREATE TABLE IF NOT EXISTS AttachmentType(
        	        id INTEGER NOT NULL,
        	        type TEXT NOT NULL,
        	        PRIMARY KEY(id)
                );
                CREATE TABLE IF NOT EXISTS Attachment(
	                id INTEGER NOT NULL,
	                taskgroup_id INTEGER,
	                task_id INTEGER,
	                content TEXT NOT NULL,
	                type_id INTEGER NOT NULL,
	                PRIMARY KEY(id),
	                FOREIGN KEY(taskgroup_id) REFERENCES TaskGroup(id),
	                FOREIGN KEY(task_id) REFERENCES Task(id),
                    FOREIGN KEY(type_id) REFERENCES AttachmenType(id)	
                );
            ";
            try
            {
                sqliteCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}