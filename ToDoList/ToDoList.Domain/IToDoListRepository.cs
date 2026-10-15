using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;




namespace ToDoList.Domain
{



  
    public interface IToDoListRepository
    {
      public List<ToDoTask> Get();






        public void Create(ToDoTask task);


        public ToDoTask Update(ToDoTask task);


        public bool Delete(int id);


        public ToDoTask GetById(int id);
    

    }
}
