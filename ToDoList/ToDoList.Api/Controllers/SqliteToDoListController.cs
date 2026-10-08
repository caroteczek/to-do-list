using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;
using ToDoList.Domain;
using ToDoList.Infrastructure;
using ToDoList.Infrastructure.Repository;

namespace ToDoList.Api.Controllers
{
    [Route("ToDoList")]
    [ApiController]
    public class SqliteToDoListController : ControllerBase
    {
        private static int globalId = 0;

        private static List<ToDoTaskDto> TasksList = new List<ToDoTaskDto>();


        private readonly IToDoListRepository _toDoListRepository;

        public SqliteToDoListController(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }


        [HttpGet]

        public List<ToDoTaskDto> Get()
        {
            List<ToDoTask> domainTasks = _toDoListRepository.Get();

            List<ToDoTaskDto> taskDtos = domainTasks.Select(task => new ToDoTaskDto
            {
                Id = task.Id,
                Nazwa = task.Title
            }).ToList();


            return taskDtos;
        }



        [HttpPost]

        public IActionResult Post([FromBody] ToDoTaskDto taskDto)
        {

            ToDoTask domainTask = new ToDoTask(taskDto.Id, taskDto.Nazwa);
            

            _toDoListRepository.Post(domainTask);

            return Created("task created", taskDto);

            //int newId = ++globalId;


            //    task.Id = newId;

            //TasksList.Add(task);

            //// ToDoTaskDto tasks = (ToDoTaskDto)task[0];



            //    return Created("Task Created", task);





        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromBody] ToDoTaskDto task)
        {

            ToDoTask domainTask = new ToDoTask(id, task.Nazwa);
            

            ToDoTask updatedDomaininTask = _toDoListRepository.Put(id, domainTask);

            if (updatedDomaininTask == null)
            {
                return NotFound();
            }

            ToDoTaskDto responseDto = new ToDoTaskDto
            {
                Id = updatedDomaininTask.Id,
                Nazwa = updatedDomaininTask.Title
            };

            return Ok(responseDto);

            //var t = TasksList.FirstOrDefault(t => t.Id == id);
            //// task  = new ToDoTaskDto(id, task.Nazwa);
            //t.Nazwa = task.Nazwa;





            //return t;
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {


            bool isDeleted = _toDoListRepository.Delete(id);

            if (!isDeleted)

            {
                return NotFound();
            }
            //var t = TasksList.RemoveAll(t => t.Id == id);
            //// task  = new ToDoTaskDto(id, task.Nazwa);

            //return true;
            return NoContent();
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {


            ToDoTask domainTask = _toDoListRepository.GetById(id);
            
            if (domainTask is null)
            {
                return NotFound($"Task not found. Searching Id: {id}");
            }

            ToDoTaskDto taskDto = new ToDoTaskDto
            {
                Id = domainTask.Id,
                Nazwa = domainTask.Title
            };

            return Ok(taskDto);

    }

}
}
