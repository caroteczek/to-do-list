using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Controllers;
using ToDoList.Api.DTOs;
using ToDoList.Domain;
using ToDoList.Infrastructure.Repository;
using Xunit;


namespace ToDoListWebApiTests.Controllers
{

    public class ToDoListConttollersTest



    {


        [Fact]

        public void Get_Returns_All_Products()
        {
            //Arrange
            List<ToDoTask> list_of_tasks2 = [
                new ToDoTask (1, "Kupic maslo", true, Priority.High ),
                new ToDoTask(2, "Cwiczyć na rowerku", true , Priority.High),
                new ToDoTask(3, "Uczyć się angielskiego", true, Priority.Low ),
                new ToDoTask ( 4, "Być na diecie", true , Priority.Medium),
                new ToDoTask( 5,"Przeczytać powieść", true, Priority.High )

            ];


            //Act

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks2);

            ToDoListController sut = new ToDoListController(mockRepo);

          List <ToDoTaskDto> list_of_tasks = sut.Get("alphabetical");
        

            List<ToDoTaskDto> expected = list_of_tasks2
        .OrderBy(n => n.Title) 
        .Select(n => new ToDoTaskDto { Id = n.Id, Nazwa = n.Title, isCompleted = n.isCompleted, Priority = n.Priority }).ToList();


            //Assert

            //Assert.Equal(expected, list_of_tasks);

            Assert.Equal(expected.Count, list_of_tasks.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, list_of_tasks[i].Id);
                Assert.Equal(expected[i].Nazwa, list_of_tasks[i].Nazwa);
                Assert.Equal(expected[i].isCompleted, list_of_tasks[i].isCompleted);
                Assert.Equal(expected[i].Priority, list_of_tasks[i].Priority);
            }


        }

        [Fact]

        public void Remove_One_Task()
        {
            //Arrange
            List<ToDoTask> list_of_tasks3 = [
                new ToDoTask (1, "Kupic maslo", true, Priority.High ),
                new ToDoTask(2, "Cwiczyć na rowerku", true , Priority.High),
                new ToDoTask(3, "Uczyć się angielskiego", true , Priority.High),
                new ToDoTask ( 4, "Być na diecie", true, Priority.High ),
                new ToDoTask( 5,"Przeczytać powieść", true, Priority.High )

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks3);

            ToDoListController sut = new ToDoListController(mockRepo);




            // Act
            int id_to_remove = 2;
            IActionResult deletedId = sut.Delete(id_to_remove);


            Assert.IsType<NoContentResult>(deletedId);
        }

            [Fact]
        public void Remove_One_Not_Existing_Task()
        {

            //Arrange
            List<ToDoTask> list_of_tasks4 = [
                new ToDoTask (1, "Kupic maslo" , true, Priority.High),
                new ToDoTask(2, "Cwiczyć na rowerku", true , Priority.High),
                new ToDoTask(3, "Uczyć się angielskiego" , true, Priority.High),
                new ToDoTask ( 4, "Być na diecie", true , Priority.High),
                new ToDoTask( 5,"Przeczytać powieść" , true, Priority.High )

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks4);

            ToDoListController sut = new ToDoListController(mockRepo);


            // Act
            int id_to_remove = 10;
            IActionResult deletedId = sut.Delete(id_to_remove);


            //Assert

            Assert.IsType<NotFoundResult>(deletedId);


        }


        [Fact]
        public void UpdateExistTask()
        {
            //Arrange
            List<ToDoTask> list_of_tasks5 = [
                new ToDoTask (1, "Kupic maslo" , true, Priority.High),
                new ToDoTask(2, "Cwiczyć na rowerku" , true, Priority.High),
                new ToDoTask(3, "Uczyć się angielskiego", true , Priority.High),
                new ToDoTask ( 4, "Być na diecie", true , Priority.High),
                new ToDoTask( 5,"Przeczytać powieść" , true, Priority.High)

            ];

            MockTaskRepository mockRepo = new MockTaskRepository(list_of_tasks5);

            ToDoListController sut = new ToDoListController(mockRepo);

            //Act
            
            ToDoTaskDto updatedDto = new ToDoTaskDto { Nazwa = "Zapłacić rachunki", isCompleted = true, Priority = Priority.Low };
            IActionResult result = sut.Update(list_of_tasks5[3].Id, updatedDto);

            //Assert

            Assert.IsType<OkObjectResult>(result);

        }
        }
}


           


