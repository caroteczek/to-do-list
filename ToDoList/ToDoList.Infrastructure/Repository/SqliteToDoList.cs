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
                nazwa TEXT not null
            );
            ";

            createTableCommand.ExecuteNonQuery();
        }

        public List<ToDoTask> Get()
        {
           // Ensure();
            // Tutaj kod zapytania SQLite zwracający List<ToDoTask>
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            SELECT id, nazwa
            FROM tasks;
        """;

            using SqliteDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);


                var task = new ToDoTask(id, nazwa);
                

                tasks.Add(task);
            }

            return tasks;
        }






        public void Post(ToDoTask task)
        {
            Ensure();
            // Tutaj kod zapytania SQLite zwracający List<ToDoTask>
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var insertCommand = connection.CreateCommand();

            insertCommand.CommandText =
            "INSERT INTO tasks (nazwa) VALUES ($nazwa); ";

            insertCommand.Parameters.AddWithValue("$nazwa", task.Title);
            
            insertCommand.ExecuteNonQuery();
        }


        public ToDoTask Put(int id, ToDoTask task)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            UPDATE tasks 
            SET nazwa = $nazwa
            WHERE id = $id;
        """;

            cmd.Parameters.AddWithValue("$nazwa", task.Title);
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
            SELECT id, nazwa
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

                task = new ToDoTask(Id, nazwa);
                               tasks.Add(task);
            }

                      return task;

        }


    }
}
