using Foco.models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Foco.sqlite
{

    class DatabaseManager
    {

        private const string DELETE_ALL = "DELETE FROM Goal"; // Reicht aus, wegen "ON DELETE CASCADE"
        private const string INSERT_GOAL = "INSERT INTO Goal(title) VALUES('{0}')";
        private const string INSERT_PROJECT = "INSERT INTO Project(title, color, goal_id) VALUES ('{0}', '{1}', {2})";
        private const string INSERT_TASKGROUP = "INSERT INTO Taskgroup(title, project_id, state_id, priority_id, deadline) VALUES ('{0}', {1}, {2}, {3}, {4})";
        private const string INSERT_TASK = "INSERT INTO Task(title, description, done, taskgroup_id) VALUES ('{0}', '{1}', {2}, {3})";
        private const string INSERT_ATTACHMENT = "INSERT INTO Attachment(title, content, type_id, task_id, taskgroup_id, date) VALUES ('{0}', '{1}', {2}, {3}, {4}, {5})";
        private const string SELECT_LAST_ID = "SELECT LAST_INSERT_ROWID()";
        private const string SELECT_GOAL = "SELECT id, title FROM Goal";
        private const string SELECT_PROJECT = "SELECT id, title, color FROM Project WHERE goal_id = {0}";
        private const string SELECT_TASKGROUP = "SELECT id, title, deadline, state_id, priority_id FROM Taskgroup WHERE project_id = {0}";
        private const string SELECT_TASK = "SELECT id, title, description, done FROM Task WHERE taskgroup_id = {0}";
        private const string SELECT_TASK_ATTACHMENT = "SELECT id, title, content, type_id, date FROM Attachment WHERE task_id = {0}";
        private const string SELECT_TASKGROUP_ATTACHMENT = "SELECT id, title, content, type_id, date FROM Attachment WHERE taskgroup_id = {0}";
        private const string FIRST_SETUP = @"
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
            CREATE TABLE IF NOT EXISTS AttachmentType(
        	    id INTEGER NOT NULL,
        	    type TEXT NOT NULL,
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
                color TEXT NOT NULL,
        	    goal_id INTEGER NOT NULL,
        	    PRIMARY KEY(id),
        	    FOREIGN KEY(goal_id) REFERENCES Goal(id) ON DELETE CASCADE
            );
            CREATE TABLE IF NOT EXISTS TaskGroup(
	            id INTEGER NOT NULL,
	            title TEXT NOT NULL,
	            project_id INTEGER NOT NULL,
	            state_id INTEGER,
	            priority_id INTEGER, 
	            deadline DATETIME,
	            PRIMARY KEY(id),
	            FOREIGN KEY(project_id) REFERENCES Project(id) ON DELETE CASCADE,
	            FOREIGN KEY(state_id) REFERENCES State(id) ON DELETE CASCADE,
	            FOREIGN KEY(priority_id) REFERENCES Priority(id) ON DELETE CASCADE
            );
            CREATE TABLE IF NOT EXISTS Task(
	            id INTEGER NOT NULL,
	            title TEXT NOT NULL,
                description TEXT,
	            done BOOLEAN NOT NULL,
	            taskgroup_id INTEGER NOT NULL,
	            PRIMARY KEY(id),
	            FOREIGN KEY(taskgroup_id) REFERENCES TaskGroup(id) ON DELETE CASCADE
            );
            CREATE TABLE IF NOT EXISTS Attachment(
	            id INTEGER NOT NULL,
	            taskgroup_id INTEGER,
	            task_id INTEGER,
                title TEXT,
                date DATETIME,
	            content TEXT NOT NULL,
	            type_id INTEGER NOT NULL,
	            PRIMARY KEY(id),
	            FOREIGN KEY(taskgroup_id) REFERENCES TaskGroup(id) ON DELETE CASCADE,
	            FOREIGN KEY(task_id) REFERENCES Task(id) ON DELETE CASCADE,
                FOREIGN KEY(type_id) REFERENCES AttachmentType(id) ON DELETE CASCADE	
            );
            INSERT INTO AttachmentType(id, type) SELECT 0, 'Link'    WHERE NOT EXISTS (SELECT * FROM AttachmentType WHERE id = 0);
            INSERT INTO AttachmentType(id, type) SELECT 1, 'Comment' WHERE NOT EXISTS (SELECT * FROM AttachmentType WHERE id = 1);
            INSERT INTO Priority(id, bez) SELECT 0, 'Low'  WHERE NOT EXISTS (SELECT * FROM Priority WHERE id = 0);
            INSERT INTO Priority(id, bez) SELECT 1, 'Mid'  WHERE NOT EXISTS (SELECT * FROM Priority WHERE id = 1);
            INSERT INTO Priority(id, bez) SELECT 2, 'High' WHERE NOT EXISTS (SELECT * FROM Priority WHERE id = 2);
            INSERT INTO State(id, bez) SELECT 0, 'Todo'       WHERE NOT EXISTS (SELECT * FROM State WHERE id = 0);
            INSERT INTO State(id, bez) SELECT 1, 'InProgress' WHERE NOT EXISTS (SELECT * FROM State WHERE id = 1);
            INSERT INTO State(id, bez) SELECT 2, 'Done'       WHERE NOT EXISTS (SELECT * FROM State WHERE id = 2);
        ";

        private readonly SqliteConnection sqliteConnection;

        /**
         * <summary>
         * Erstellt ein neues Objekt zur Kommunikation mit der Datenbank.
         * Die Verbindung muss anschließend mit <c>Connect()</c> hergestellt
         * und am Ende wieder mit <c>Disconnect()</c> getrennt werden.
         * </summary>
         * <param name="filePath">Der Dateipfad zur SQLite-Datenbank</param>
         */
        public DatabaseManager(string filePath)
        {
            SqliteConnectionStringBuilder stringBuilder = new SqliteConnectionStringBuilder();
            stringBuilder.DataSource = filePath;
            stringBuilder.ForeignKeys = true;
            stringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            sqliteConnection = new SqliteConnection(stringBuilder.ConnectionString);
        }

        /**
         * <summary>
         * Der Destruktor trennt die Verbindung zur Datenbank falls dies vergessen wurde.
         * </summary>
         */
        ~DatabaseManager()
        {
            Disconnect();
        }

        /**
         * <summary>
         * Versucht eine Verbindung zur SQLite-Datenbank herzustellen.
         * Sind noch keine Tabellen vorhanden, werden diese erstellt und mit ein
         * paar Daten zur Demonstration gefüllt.
         * </summary>
         * <returns>true bei erfolgreicher Verbindung, ansonsten false</returns>
         */
        public bool Connect()
        {
            try
            {
                sqliteConnection.Open();
                SqliteCommand sqliteCommand = new SqliteCommand(FIRST_SETUP, sqliteConnection);
                sqliteCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**
         * <summary>
         * Trennt eine zuvor erstellte Verbindung zur Datenbank wieder.
         * </summary>
         */
        public void Disconnect()
        {
            sqliteConnection.Close();
        }

        /**
         * <summary>
         * Lädt alle Daten (Ziele, Projekte, Aufgabengruppe, Aufgaben und
         * Anhänge) aus der Datenbank und gibt sie als Liste zurück.
         * </summary>
         * <returns>Liste mit Zielen oder null wenn ein Fehler auftrat</returns>
         */
        public List<Goal> LoadAll()
        {
            try
            {

                List<Goal> goals = new List<Goal>();

                SqliteCommand goalCommand = new SqliteCommand(SELECT_GOAL, sqliteConnection);
                SqliteDataReader goalReader = goalCommand.ExecuteReader();

                while (goalReader.Read())
                {
                    long goalId = goalReader.GetInt64(0);
                    Goal goal = new Goal(goalReader.GetString(1));
                    goals.Add(goal);

                    SqliteCommand projectCommand = new SqliteCommand(string.Format(SELECT_PROJECT, goalId), sqliteConnection);
                    SqliteDataReader projectReader = projectCommand.ExecuteReader();

                    while (projectReader.Read())
                    {
                        long projectId = projectReader.GetInt64(0);
                        Project project = new Project(projectReader.GetString(1), projectReader.GetString(2));
                        goal.Projects.Add(project);

                        SqliteCommand taskgroupCommand = new SqliteCommand(string.Format(SELECT_TASKGROUP, projectId), sqliteConnection);
                        SqliteDataReader taskgroupReader = taskgroupCommand.ExecuteReader();

                        while (taskgroupReader.Read())
                        {
                            long taskgroupId = taskgroupReader.GetInt64(0);
                            Taskgroup taskgroup = new Taskgroup(taskgroupReader.GetString(1));
                            if (!taskgroupReader.IsDBNull(2))
                                taskgroup.Deadline = DateTime.Parse(taskgroupReader.GetString(2));
                            taskgroup.State = (State)taskgroupReader.GetInt32(3);
                            taskgroup.Prio = (Priority)taskgroupReader.GetInt32(4);
                            project.Taskgroups.Add(taskgroup);

                            SqliteCommand taskCommand = new SqliteCommand(string.Format(SELECT_TASK, taskgroupId), sqliteConnection);
                            SqliteDataReader taskReader = taskCommand.ExecuteReader();

                            while (taskReader.Read())
                            {
                                long taskId = taskReader.GetInt64(0);
                                Task task = new Task(taskReader.GetString(1));
                                task.Description = taskReader.GetString(2);
                                task.Done = taskReader.GetBoolean(3);
                                taskgroup.Tasks.Add(task);

                                SqliteCommand taskAttachmentCommand = new SqliteCommand(string.Format(SELECT_TASK_ATTACHMENT, taskId), sqliteConnection);
                                SqliteDataReader taskAttachmentReader = taskAttachmentCommand.ExecuteReader();

                                while (taskAttachmentReader.Read())
                                {
                                    long taskAttachmentId = taskAttachmentReader.GetInt64(0);
                                    Attachment attachment = null;
                                    switch ((AttachmentType)taskAttachmentReader.GetInt32(3))
                                    {
                                        case AttachmentType.Comment:
                                            attachment = new CommentAttachment(taskAttachmentReader.GetString(2), DateTime.Parse(taskAttachmentReader.GetString(4)));
                                            break;
                                        case AttachmentType.Link:
                                            attachment = new LinkAttachment(taskAttachmentReader.GetString(1), taskAttachmentReader.GetString(2));
                                            break;
                                    }
                                    task.Attachments.Add(attachment);
                                }

                                taskAttachmentReader.Close();

                            }

                            taskReader.Close();

                            SqliteCommand taskgroupAttachmentCommand = new SqliteCommand(string.Format(SELECT_TASKGROUP_ATTACHMENT, taskgroupId), sqliteConnection);
                            SqliteDataReader taskgroupAttachmentReader = taskgroupAttachmentCommand.ExecuteReader();

                            while (taskgroupAttachmentReader.Read())
                            {
                                long taskgroupAttachmentId = taskgroupAttachmentReader.GetInt64(0);
                                Attachment attachment = null;
                                switch ((AttachmentType)taskgroupAttachmentReader.GetInt32(3))
                                {
                                    case AttachmentType.Comment:
                                        attachment = new CommentAttachment(taskgroupAttachmentReader.GetString(2), DateTime.Parse(taskgroupAttachmentReader.GetString(4)));
                                        break;
                                    case AttachmentType.Link:
                                        attachment = new LinkAttachment(taskgroupAttachmentReader.GetString(1), taskgroupAttachmentReader.GetString(2));
                                        break;
                                }
                                taskgroup.Attachments.Add(attachment);
                            }

                            taskgroupAttachmentReader.Close();

                        }

                        taskgroupReader.Close();

                    }

                    projectReader.Close();

                }

                goalReader.Close();

                return goals;
            }
            catch
            {
                return null;
            }
        }

        /**
         * <summary>
         * Speichert alle Daten in der Datenbank ab. Vorherige Daten werden
         * aus der Datenbank gelöscht. Damit es bei einem Fehler zu keinem
         * Datenverlust kommt, geschieht das Löschen und Einfügen als Transaktion.
         * Im Fehlerfall wird also die vorherige Datenbank wiederhergestellt.
         * </summary>
         * <returns>true wenn alles gespeichert wurde, false bei einem Rollback</returns>
         */
        public bool SaveAll(List<Goal> goals)
        {

            try
            {

                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = sqliteConnection;
                sqliteCommand.Transaction = sqliteConnection.BeginTransaction();

                try
                {

                    sqliteCommand.CommandText = DELETE_ALL;
                    sqliteCommand.ExecuteNonQuery();

                    foreach (Goal goal in goals)
                    {

                        sqliteCommand.CommandText = string.Format(INSERT_GOAL, goal.Title);
                        sqliteCommand.ExecuteNonQuery();
                        sqliteCommand.CommandText = SELECT_LAST_ID;
                        long goalId = (long)sqliteCommand.ExecuteScalar();

                        foreach (Project project in goal.Projects)
                        {

                            sqliteCommand.CommandText = string.Format(INSERT_PROJECT, project.Name, project.Color, goalId);
                            sqliteCommand.ExecuteNonQuery();
                            sqliteCommand.CommandText = SELECT_LAST_ID;
                            long projectId = (long)sqliteCommand.ExecuteScalar();

                            foreach (Taskgroup taskgroup in project.Taskgroups)
                            {
                                string deadline = taskgroup.Deadline == DateTime.MinValue ? "NULL" : "'" + taskgroup.Deadline.ToString() + "'";
                                sqliteCommand.CommandText = string.Format(INSERT_TASKGROUP, taskgroup.Title, projectId, (int)taskgroup.State, (int)taskgroup.Prio, deadline);
                                sqliteCommand.ExecuteNonQuery();
                                sqliteCommand.CommandText = SELECT_LAST_ID;
                                long taskgroupId = (long)sqliteCommand.ExecuteScalar();

                                foreach (Task task in taskgroup.Tasks)
                                {

                                    sqliteCommand.CommandText = string.Format(INSERT_TASK, task.Title, task.Description, task.Done, taskgroupId);
                                    sqliteCommand.ExecuteNonQuery();
                                    sqliteCommand.CommandText = SELECT_LAST_ID;
                                    long taskId = (long)sqliteCommand.ExecuteScalar();

                                    foreach (Attachment attachment in task.Attachments)
                                    {
                                        string date = (attachment.Type == AttachmentType.Comment) ? "'" + ((CommentAttachment)attachment).Date.ToString() + "'" : "NULL";
                                        sqliteCommand.CommandText = string.Format(INSERT_ATTACHMENT, attachment.Title, attachment.Content, (int)attachment.Type, taskId, "NULL", date);
                                        sqliteCommand.ExecuteNonQuery();
                                    }

                                }

                                foreach (Attachment attachment in taskgroup.Attachments)
                                {
                                    string date = (attachment.Type == AttachmentType.Comment) ? "'" + ((CommentAttachment)attachment).Date.ToString() + "'" : "NULL";
                                    sqliteCommand.CommandText = string.Format(INSERT_ATTACHMENT, attachment.Title, attachment.Content, (int)attachment.Type, "NULL", taskgroupId, date);
                                    sqliteCommand.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                    sqliteCommand.Transaction.Commit();
                    return true;
                }
                catch
                {
                    sqliteCommand.Transaction.Rollback();
                    return false;
                }

            }
            catch
            {
                return false;
            }

        }

    }


}