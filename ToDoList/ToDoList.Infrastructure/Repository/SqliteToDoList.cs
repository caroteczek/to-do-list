using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ToDoList.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using ToDoList.Domain;






namespace ToDoList.Infrastructure.Repository
{


    public class SqliteToDoList: IToDoListRepository
    {

        private readonly string connectionString;


        public SqliteToDoList(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public SqliteToDoList() : this("Data Source=BazaDanych.db")
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
                isCompleted BOOLEAN DEFAULT 0

            );
            ";

            createTableCommand.ExecuteNonQuery();
        }

        public List<ToDoTask> Get()
        {
        
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            SELECT id, nazwa, isCompleted
            FROM tasks;
        """;

            using SqliteDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;


                var task = new ToDoTask(id, nazwa, isCompleted);
                

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
            "INSERT INTO tasks (nazwa) VALUES ($nazwa); ";

            insertCommand.Parameters.AddWithValue("$nazwa", task.Title);
            
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
        isCompleted = $isCompleted
            WHERE id = $id;
        """;

            cmd.Parameters.AddWithValue("$nazwa", task.Title);
            cmd.Parameters.AddWithValue("$isCompleted", task.isCompleted);
            cmd.Parameters.AddWithValue("$id", task.Id);

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


        public ToDoTask GetById(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            SELECT id, nazwa, isCompleted
            FROM tasks
            WHERE id = $IDENTYFIKATOR;
        """;

            cmd.Parameters.AddWithValue("$IDENTYFIKATOR", id);

            using SqliteDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            ToDoTask? task = null;

            
            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;


                task = new ToDoTask(Id, nazwa, isCompleted);
                               tasks.Add(task);
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
