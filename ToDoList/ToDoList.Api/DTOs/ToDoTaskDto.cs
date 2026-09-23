using System.Runtime.CompilerServices;

namespace ToDoList.Api.DTOs
{
    public class ToDoTaskDto
    {
     
        public  int Id { get; set; }
        public string Nazwa { get; set; }


        public ToDoTaskDto(int id, string nazwa)
       {
            Id = id;
      
         Nazwa = nazwa;


       }

     
    }
}