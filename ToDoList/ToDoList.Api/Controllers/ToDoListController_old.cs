using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/old-todolist")]
    public class ToDoListController_old : ControllerBase
    {
        private static int globalId = 0;

        private static List<ToDoTaskDto> TasksList = new List<ToDoTaskDto>();

        
        public ToDoListController_old(List<ToDoTaskDto> l)
        {
           
            TasksList = l;
        }

        public ToDoListController_old(ToDoTaskDto t)
        {
            if (TasksList == null)
            {
                TasksList = new List<ToDoTaskDto>();
            }

            if (t != null)
            {
                TasksList.Add(t);
            }

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

          
         

         
            if (TasksList == null)
            {
                return NotFound();
            }

            var task = TasksList.FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                return NotFound($"Task not found. Searching Id: {id}");
            }

           
            return Ok(task);
        }
    }



    }

