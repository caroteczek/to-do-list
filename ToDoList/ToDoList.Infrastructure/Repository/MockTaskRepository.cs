using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Domain;

namespace ToDoList.Infrastructure.Repository
{
    internal class MockTaskRepository: IToDoListRepository
    {
        public void Post(ToDoTask task)
        {
            return;
        }

        public bool Delete(int identyfikator)
        {
            return true;
        }
        public ToDoTask Put(int id, ToDoTask task)
        {
            return task;
        }
        public List<ToDoTask> Get()
        {
            return [
                new ToDoTask(1, "kupic mleko"),
            new ToDoTask(2, "wyplacic pieniadze")
            ];
        }

        public ToDoTask? GetById(int identyfikator)
        {
            return new ToDoTask(identyfikator, "kupic mleko");
        }

        

        public ToDoTask Put(ToDoTask task)
        {
            return task;
        }
    }
}
