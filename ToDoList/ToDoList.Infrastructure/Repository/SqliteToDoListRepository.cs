using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ToDoList.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using ToDoList.Domain;






namespace ToDoList.Infrastructure.Repository
{


    public class SqliteToDoListRepository: IToDoListRepository
    {

        private readonly string connectionString;


        public SqliteToDoListRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public SqliteToDoListRepository() : this("Data Source=BazaDanych.db")
        {
        }



        public void Ensure()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
                    @"
            CREATE TABLE IF NOT EXISTS tasks (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                nazwa TEXT not null,
                isCompleted BOOLEAN DEFAULT 0,
                priority INTEGER NULL

            );
            ";

            createTableCommand.ExecuteNonQuery();
        }

        public List<ToDoTask> Sort(string? sortBy = null)
        {
            System.Diagnostics.Debug.WriteLine($"Otrzymano sortBy: '{sortBy}'");

            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            string query =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks
        """;

            //sortowanie - wybór

            if (sortBy == "alphabetical")
            {
                query += " ORDER BY nazwa ASC;"; //alfabytycznie od A do Z
            }
            else if (sortBy == "completed-first")
            {
                query += " ORDER BY isCompleted DESC;";

            }
            else if (sortBy == "uncompleted-first")
            {
                query += " ORDER BY isCompleted ASC;";

            }
            else
            {
                query += ";"; //domyślnie, bez sortowania
            }

            System.Diagnostics.Debug.WriteLine($"Wykonuję SQL: {query}");

            cmd.CommandText = query;


            using SqliteDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;
                Priority? priority = reader.IsDBNull(3) ? (Priority?)null : (Priority)reader.GetInt32(3);


                var task = new ToDoTask(id, nazwa, isCompleted, priority.HasValue ? (Priority?)priority.Value : null);


                tasks.Add(task);
            }

            return tasks;
        }


        public List<ToDoTask> Get()
        {
           
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            string query =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks;
        """;

    

            cmd.CommandText = query;


            using SqliteDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;
                Priority? priority = reader.IsDBNull(3) ? (Priority?)null : (Priority)reader.GetInt32(3);


                var task = new ToDoTask(id, nazwa, isCompleted, priority.HasValue ? (Priority?)priority.Value : null);


                tasks.Add(task);
            }

            return tasks;
        }



        public void Create(ToDoTask task)
        {
            Ensure();
         
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var insertCommand = connection.CreateCommand();

            insertCommand.CommandText =
            "INSERT INTO tasks (nazwa, priority) VALUES ($nazwa, $priority); ";

            insertCommand.Parameters.AddWithValue("$nazwa", task.Title);
            insertCommand.Parameters.AddWithValue("$priority", task.Priority.HasValue ? (object)task.Priority.Value : DBNull.Value);

            insertCommand.ExecuteNonQuery();
        }


        public ToDoTask Update(ToDoTask task)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            UPDATE tasks 
            SET nazwa = $nazwa,
        isCompleted = $isCompleted,
        priority = $priority
            WHERE id = $id;
        """;

            cmd.Parameters.AddWithValue("$nazwa", task.Title);
            cmd.Parameters.AddWithValue("$isCompleted", task.isCompleted);
            cmd.Parameters.AddWithValue("$id", task.Id);
            cmd.Parameters.AddWithValue("$priority", task.Priority.HasValue ? (object)task.Priority.Value : DBNull.Value);

            int rowsAffected = cmd.ExecuteNonQuery();
            return task;
        }
    




        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM tasks WHERE id = $ID;";
            cmd.Parameters.AddWithValue("$ID", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }


        public ToDoTask? GetById(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks
            WHERE id = $IDENTYFIKATOR;
        """;

            cmd.Parameters.AddWithValue("$IDENTYFIKATOR", id);

            using SqliteDataReader reader = cmd.ExecuteReader();
           
            ToDoTask? task = null;

            
            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;
                // int priority = reader.GetInt32(3);
                Priority? priority = reader.IsDBNull(3) ? (Priority?)null : (Priority)reader.GetInt32(3);
                

                task = new ToDoTask(Id, nazwa, isCompleted, priority);
            }
        

                      return task;

        }


        public bool DeleteAllTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // 1. Usuwamy wszystkie wiersze z tabeli
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM tasks;";
            int rowsAffected = cmd.ExecuteNonQuery();

            //// 2. Resetujemy autoinkrementację ID (zaczyna od nowa od 1)
            //var resetCmd = connection.CreateCommand();
            //resetCmd.CommandText = "DELETE FROM sqlite_sequence WHERE name = 'tasks';";
            //resetCmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

    }
}
