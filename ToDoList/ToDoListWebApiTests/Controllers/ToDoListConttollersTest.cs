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


namespace ToDoListWebApiTests.Controllers
{

    public class ToDoListConttollersTest



    {


        [Fact]

        public void Get_Returns_All_Products()
        {
            //Arrange
            List<ToDoTask> list_of_tasks2 = [
                new ToDoTask (1, "Kupic maslo" ),
                new ToDoTask(2, "Cwiczyć na rowerku" ),
                new ToDoTask(3, "Uczyć się angielskiego" ),
                new ToDoTask ( 4, "Być na diecie" ),
                new ToDoTask( 5,"Przeczytać powieść" )

            ];




            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks2);

            ToDoListController sut = new ToDoListController(mockRepo);

            foreach (ToDoTask toDo in list_of_tasks2.ToList())
            {


                ToDoTaskDto testTask = new ToDoTaskDto
                {
                    Id = toDo.Id,
                    Nazwa = toDo.Title
                };


            }




        }

        [Fact]

        public void Remove_One_Task()
        {
            //Arrange
            List<ToDoTask> list_of_tasks3 = [
                new ToDoTask (1, "Kupic maslo" ),
                new ToDoTask(2, "Cwiczyć na rowerku" ),
                new ToDoTask(3, "Uczyć się angielskiego" ),
                new ToDoTask ( 4, "Być na diecie" ),
                new ToDoTask( 5,"Przeczytać powieść" )

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks3);

            ToDoListController sut = new ToDoListController(mockRepo);


            foreach (ToDoTask toDo in list_of_tasks3.ToList())
            {


                ToDoTaskDto testTask = new ToDoTaskDto
                {
                    Id = toDo.Id,
                    Nazwa = toDo.Title
                };


                sut.Create(testTask);

            }

            // Act
            int id_to_remove = 2;
            var deletedId = sut.Delete(id_to_remove);

            // Assert
            var findid = sut.Get().Where(p => p.Id == id_to_remove).ToList();
        }

        [Fact]
        public void Remove_One_Not_Existing_Task()
        {

            //Arrange
            List<ToDoTask> list_of_tasks4 = [
                new ToDoTask (1, "Kupic maslo" ),
                new ToDoTask(2, "Cwiczyć na rowerku" ),
                new ToDoTask(3, "Uczyć się angielskiego" ),
                new ToDoTask ( 4, "Być na diecie" ),
                new ToDoTask( 5,"Przeczytać powieść" )

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks4);

            ToDoListController sut = new ToDoListController(mockRepo);


            foreach (ToDoTask toDo in list_of_tasks4.ToList())
            {


                ToDoTaskDto testTask = new ToDoTaskDto
                {
                    Id = toDo.Id,
                    Nazwa = toDo.Title
                };


                sut.Create(testTask);

            }

            // Act
            int id_to_remove = 10;
            var deletedId = sut.Delete(id_to_remove);

            // Assert
            var findIncorrectId = sut.Get().Where(p => p.Id == id_to_remove).ToList();
            Assert.Empty(findIncorrectId);
            Assert.Equal(0, findIncorrectId.Count);

        }


        [Fact]
        public void UpdateExistTask()
        {
            //Arrange
            List<ToDoTask> list_of_tasks5 = [
                new ToDoTask (1, "Kupic maslo" ),
                new ToDoTask(2, "Cwiczyć na rowerku" ),
                new ToDoTask(3, "Uczyć się angielskiego" ),
                new ToDoTask ( 4, "Być na diecie" ),
                new ToDoTask( 5,"Przeczytać powieść" )

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks5);

            ToDoListController sut = new ToDoListController(mockRepo);

            foreach (ToDoTask toDo in list_of_tasks5.ToList())
            {


                ToDoTaskDto testTask = new ToDoTaskDto
                {
                    Id = toDo.Id,
                    Nazwa = toDo.Title
                };


                sut.Create(testTask);

               

            }
            //Act

            int id = 2;

            ToDoTaskDto taskToUpdate = new ToDoTaskDto { Id = id, Nazwa = "Cwiczyć na orbiterku" };

            var result = sut.Update(2, taskToUpdate);

            //Assert

            Assert.IsType<OkObjectResult>(result);

            List<ToDoTaskDto> allTasks = sut.Get();
            List<ToDoTaskDto> findTask = allTasks.Where(p => p.Id == id).ToList();
   
            Assert.Equal(list_of_tasks5.Count, allTasks.Count);
            Assert.Equal(2, findTask.Count);
        }
        }
}


           


