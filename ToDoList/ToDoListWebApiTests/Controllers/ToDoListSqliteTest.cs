using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;
using ToDoList.Infrastructure.Repository;
using ToDoList.Api.DTOs;
using ToDoList.Api.Controllers;
using Xunit;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using Microsoft.Data.Sqlite;

namespace ToDoListWebApiTests.Controllers
{
    public class ToDoListSqliteTest: IDisposable
    {
        private IToDoListRepository sut { get;  }

        private string DbName { get; }

        public ToDoListSqliteTest()
        {
            (IToDoListRepository serviceSQLiteTest, string dbname) = NewRepository();

            sut = serviceSQLiteTest;
            DbName = dbname;
        }

        private (IToDoListRepository, string) NewRepository()
        {
            string dbName = $"{Guid.NewGuid()}_BazaDanych";
            string connectionString = $"Data Source={dbName}";

            SqliteToDoList repository = new SqliteToDoList(connectionString);
            repository.Ensure();

            return (repository, dbName);
        }

        public void Dispose()
        {
            Clear(DbName);


        }

        private void Clear(string DbName)
        {
            try
            {
                if (File.Exists(DbName))
                {
                    File.Delete(DbName);
                }
            }
            catch
            {

            }
        }


        [Fact]
        public void Add_ValidTask()
        {


            // Arrange
            ToDoTask task = new ToDoTask(30, "Umyć okna", true);

            // Act

            bool result = sut.Create(task);

            List<ToDoTask> tasks = sut.Get();

            
            // Assert
            Assert.True(result);
            ToDoTask ?actual = tasks.FirstOrDefault(n => n.Title == task.Title && n.isCompleted == task.isCompleted && n.Priority == task.Priority);
            Assert.NotNull(actual);
          


        }

    }
}
