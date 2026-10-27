using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;
using ToDoList.Api.Controllers;
using ToDoList.Domain;
using System.Security.Cryptography.X509Certificates;
using ToDoList.Infrastructure.Repository;


using ToDoList.Infrastructure.Repository;
using System.Diagnostics.Metrics;
using System.Diagnostics.Eventing.Reader;


namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {


        public SqliteToDoList sql = new SqliteToDoList();
     
        static int counter = 0;

        [HttpGet]
        public IActionResult Get()
        {

            counter = sql.Get().Count;
            

        string json = $$"""{"taskCount": {{counter}}}""";

          

                return Ok(json);
            
          
    }






        }
    }


