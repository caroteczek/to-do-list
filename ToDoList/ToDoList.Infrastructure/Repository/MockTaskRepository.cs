using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Domain;

namespace ToDoList.Infrastructure.Repository
{
    internal class MockTaskRepository: IToDoListRepository
    {
        public void Create(ToDoTask task)
        {
            return;
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
            return [
                new ToDoTask(1, "kupic mleko"),
            new ToDoTask(2, "wyplacic pieniadze")
            ];
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
