using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Domain;
using ToDoList.Infrastructure;


    


namespace ToDoList.Infrastructure.Repository
{
 public class MockTaskRepository: IToDoListRepository
    {

        List<ToDoTask> _MockTasksList = new List<ToDoTask>();
        public MockTaskRepository (List<ToDoTask> MockTasksList)
        {
            _MockTasksList = MockTasksList;
        }
        public void Create(ToDoTask task)
        {
            _MockTasksList.Add(task);
        }

        public bool Delete(int identyfikator)
        {
            return true;
        }
        public ToDoTask Update(int id, ToDoTask task)
        {
            return task;
        }
        public List<ToDoTask> Get()
        {
            return _MockTasksList;
        }

        public ToDoTask? GetById(int identyfikator)
        {
            return new ToDoTask(identyfikator, "kupic mleko");
        }

        

        public ToDoTask Update(ToDoTask task)
        {
            return task;
        }
    }
}
