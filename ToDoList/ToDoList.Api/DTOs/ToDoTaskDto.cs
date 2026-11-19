using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using ToDoList.Api.Controllers;
using ToDoList.Infrastructure.Repository;
using ToDoList.Domain;

namespace ToDoList.Api.DTOs
{
    public class ToDoTaskDto
    {
     
        public  int Id { get; set; }
        public string Nazwa { get; set; }

        public bool isCompleted { get; set; }

        public Priority? Priority { get; set; }





    }
}