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
        

        [HttpGet]
   
        public List<ToDoTaskDto> Get()
        {

            return TasksList;
        }

        

        [HttpPost]
       
        public ToDoTaskDto Post (ToDoTaskDto  task)
        {

            int newId = globalId++;
           


                task = new ToDoTaskDto(newId, task.Nazwa);



            TasksList.Add(task);
            
             
        
         
          

            // ToDoTaskDto tasks = (ToDoTaskDto)task[0];



            return task;


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
    }
}
