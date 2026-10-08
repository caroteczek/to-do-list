using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;
using ToDoList.Infrastructure.Repository;
using ToDoList.Api.DTOs;
using ToDoList.Api.Controllers;
using Xunit;


namespace ToDoListWebApiTests.Controllers
{

    public class ToDoListConttollersTest
    {
        [Fact]
        public void Get_Returns_All_Products()
        {

            //Arrange

            List<ToDoTaskDto> list_of_tasks2 = [
                new ToDoTaskDto { Id = 1, Nazwa = "Kupic maslo" },
                new ToDoTaskDto { Id = 2, Nazwa = "Cwiczyć na rowerku" },
                new ToDoTaskDto { Id = 3, Nazwa = "Uczyć się angielskiego" },
                new ToDoTaskDto { Id = 4, Nazwa = "Być na diecie" },
                new ToDoTaskDto { Id = 5, Nazwa = "Przeczytać powieść" }
            ];


            ToDoListController sut = new ToDoListController(list_of_tasks2);

            // Act
            List<ToDoTaskDto> result = sut.Get().ToList();

            // Assert
            Assert.Equal(5, result.Count());
            result.ForEach(x =>
            {
                ToDoTaskDto? task = result.Find(p => p.Nazwa == x.Nazwa);
                Assert.NotNull(task);
            });
        }

        [Fact]
        public void Add_A_New_Product()
        {
            //Arrange

            List<ToDoTaskDto> list_of_tasks_to_add = [
new ToDoTaskDto { Id = 2, Nazwa = "Kupic bilet" },
    ];



            //Act

            ToDoListController sut = new ToDoListController(list_of_tasks_to_add);

            IActionResult result = sut.Post(list_of_tasks_to_add[0]);

            // Assert
            Assert.NotNull(result);
        }



        [Fact]
        public void GetById_NotFound()
        {
            //arrange
            List<ToDoTaskDto> tasks = new List<ToDoTaskDto> {
        new ToDoTaskDto { Id = 1, Nazwa = "Uczyć się programowania" }
    };

            ToDoListController sut = new ToDoListController(tasks);

            int id = 5; 

            // Act
            IActionResult? x = sut.GetById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(x);
        }
    }

}
    

           


