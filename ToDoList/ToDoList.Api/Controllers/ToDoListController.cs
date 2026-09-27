using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private static int globalId = 0;

        private static List<ToDoTaskDto> TasksList = new List<ToDoTaskDto>();

        
        public override string ToString()
        {
            // Zwracamy czytelny format tekstowy z użyciem interpolacji stringa ($)
            return $"Zadanie [ID: {globalId}, Nazwa: {TasksList}]";
        }


        [HttpGet]
   
        public List<ToDoTaskDto> Get()
        {


            return TasksList;
        }

        

        [HttpPost]
       
        public IActionResult Post (ToDoTaskDto  task)
        {

            int newId = globalId++;


                task.Id = newId;

            TasksList.Add(task);

            // ToDoTaskDto tasks = (ToDoTaskDto)task[0];

            if (task is not null)
            {

                return Created("Task Created", task);
            }

            else
            {
                //var nowa_nazwa = task.Nazwa;
                return null;
            }

        }

        [HttpPut("{id}")]

        public ToDoTaskDto Put(int id, ToDoTaskDto task)
        {

            var t = TasksList.FirstOrDefault(t => t.Id == id);
            // task  = new ToDoTaskDto(id, task.Nazwa);
            t.Nazwa = task.Nazwa;





            return t;
        }

        [HttpDelete("{id}")]

        public bool Delete (int id)
        {

            var t = TasksList.RemoveAll(t => t.Id == id);
            // task  = new ToDoTaskDto(id, task.Nazwa);
            
             return true;

        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            List<ToDoTaskDto> lista_tasków = Get();

          
            var task = lista_tasków.FirstOrDefault(t => t.Id == id);

            if (task.Nazwa == "")
            {
                return NotFound($"Task not found. Searching Id: {id}");
            }
            
            else
            {

                return Ok("Name of task: " + task.Nazwa);
            }
        }



    }
}
