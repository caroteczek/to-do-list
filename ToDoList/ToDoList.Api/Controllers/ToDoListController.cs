using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;
using ToDoList.Domain;
using ToDoList.Infrastructure;
using ToDoList.Infrastructure.Repository;
using ToDoList.Api.Controllers;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
 



       private readonly IToDoListRepository _toDoListRepository;

        public ToDoListController(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }


        [HttpGet]

        public List<ToDoTaskDto> Get([FromQuery] string? sortBy)
        {
            List<ToDoTask> domainTasks = _toDoListRepository.Get(sortBy);

            List<ToDoTaskDto> taskDtos = domainTasks.Select(task => new ToDoTaskDto
            {
                Id = task.Id,
                Nazwa = task.Title,
               isCompleted = task.isCompleted,
               Priority = task.Priority
              
            }).ToList();

        

            return taskDtos;
        }



        [HttpPost]

        public IActionResult Create([FromBody] ToDoTaskDto taskDto)
        {

            ToDoTask domainTask = new ToDoTask(taskDto.Id, taskDto.Nazwa, taskDto.isCompleted = false, taskDto.Priority);
            
            if (String.IsNullOrWhiteSpace(domainTask.Title))
            {
                return BadRequest();
            }
            
            _toDoListRepository.Create(domainTask);

            return Created("Created", taskDto);


           




        }

        [HttpPut("{id}")]

        public IActionResult Update(int id, [FromBody] ToDoTaskDto task)
        {

            ToDoTask domainTask = new ToDoTask(id, task.Nazwa, task.isCompleted, task.Priority);
            

            ToDoTask updatedDomaininTask = _toDoListRepository.Update(domainTask);

            if (updatedDomaininTask == null)
            {
                return NotFound();
            }

            ToDoTaskDto responseDto = new ToDoTaskDto
            {
                Id = updatedDomaininTask.Id,
                Nazwa = updatedDomaininTask.Title,
                isCompleted = updatedDomaininTask.isCompleted,
                Priority = updatedDomaininTask.Priority
            };

            return Ok(responseDto);

           
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {


            bool isDeleted = _toDoListRepository.Delete(id);

            if (!isDeleted)

            {
                return NotFound();
            }
            
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
                Nazwa = domainTask.Title,
                isCompleted = domainTask.isCompleted,
                Priority = domainTask.Priority
            };

            return Ok(taskDto);

    }

        [HttpDelete]

        public IActionResult DeleteAll()
        {

                bool isDeleted = _toDoListRepository.DeleteAllTasks();

                if (!isDeleted)
                {
                    return NotFound("Brak zadań do usunięcia lub tabela była pusta.");
                }

                return NoContent(); // Status 204 - sukces, wszystko skasowane
            }
        }
    }


