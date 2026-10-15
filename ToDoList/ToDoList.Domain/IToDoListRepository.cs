using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;




namespace ToDoList.Domain
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ToDoTask(int id, string n)
        {
            Id = id;
            Title = n;

        }
   
    }


  
    public interface IToDoListRepository
    {
      public List<ToDoTask> Get();






        public void Create(ToDoTask task);


        public ToDoTask Update(ToDoTask task);


        public bool Delete(int id);


        public ToDoTask GetById(int id);
    

    }
}
