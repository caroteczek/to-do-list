using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.DTOs;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        

        [HttpGet]
   
        public IEnumerable<ToDoTaskDto> Get()
        {
            List<ToDoTaskDto> lista = new List<ToDoTaskDto>();
            return lista;
        }

        [HttpPost]

        public ToDoTaskDto Post (ToDoTaskDto task)
        {
            throw new Exception();
        }

        [HttpPut("{id}")]

        public ToDoTaskDto Put(int id, ToDoTaskDto task)
        {
            throw new Exception();
        }

        [HttpDelete("{id}")]

        public bool Delete (int id)
        {
            throw new Exception();
        }
    }
}
