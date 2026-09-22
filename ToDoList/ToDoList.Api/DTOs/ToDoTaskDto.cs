using System.Runtime.CompilerServices;

namespace ToDoList.Api.DTOs
{
    public class ToDoTaskDto
    {
        static int _lastId = 0;
        public  int Id { get; set; }
        public string Nazwa { get; set; }


        public ToDoTaskDto(int id, string nazwa)
       {
            Id = id;
      
         Nazwa = nazwa;


       }

     
    }
}