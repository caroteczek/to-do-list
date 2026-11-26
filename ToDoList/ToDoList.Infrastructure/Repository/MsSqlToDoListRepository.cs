using Microsoft.Data.SqlClient;
using ToDoList.Domain;

namespace ToDoList.Infrastructure.Repository
{
    public class MsSqlToDoListRepository : IToDoListRepository
    {
        private readonly string connectionString;

        public MsSqlToDoListRepository(string cs)
        {
            connectionString = cs;
        }
        public bool Create(ToDoTask task)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var insertCommand = connection.CreateCommand();

            insertCommand.CommandText =
            "INSERT INTO tasks (nazwa, priority) VALUES (@nazwa, @priority); ";

            insertCommand.Parameters.AddWithValue("@nazwa", task.Title);
            insertCommand.Parameters.AddWithValue("@priority", task.Priority.HasValue ? (object)task.Priority.Value : DBNull.Value);

            insertCommand.ExecuteNonQuery();

            return true;
        }

        public bool Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM tasks WHERE id = @ID;";
            cmd.Parameters.AddWithValue("@ID", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public bool DeleteAllTasks()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();


            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM tasks;";
            int rowsAffected = cmd.ExecuteNonQuery();



            return rowsAffected > 0;
        }

        public List<ToDoTask> Get()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = connection.CreateCommand();
            string query =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks;
        """;



            cmd.CommandText = query;


            using SqlDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                int isCompletedOrdinal = reader.GetOrdinal("isCompleted");
                bool isCompleted = !reader.IsDBNull(isCompletedOrdinal) && reader.GetBoolean(isCompletedOrdinal);

                string priorityRaw = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                Priority? priority = Enum.TryParse<Priority>(priorityRaw, ignoreCase: true, out var parsedPriority)
                    ? parsedPriority
                    : null;
                var task = new ToDoTask(id, nazwa, isCompleted, priority.HasValue ? (Priority?)priority.Value : null);


                tasks.Add(task);
            }

            return tasks;
        }

        public ToDoTask? GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks
            WHERE id = @IDENTYFIKATOR;
        """;

            cmd.Parameters.AddWithValue("@IDENTYFIKATOR", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            ToDoTask? task = null;


            while (reader.Read())
            {
                int idOrdinal = reader.GetOrdinal("id");
                int nazwaOrdinal = reader.GetOrdinal("nazwa");
                int isCompletedOrdinal = reader.GetOrdinal("isCompleted");
                int priorityOrdinal = reader.GetOrdinal("priority");

                int Id = reader.GetInt32(idOrdinal);
                string nazwa = reader.GetString(nazwaOrdinal);
                bool isCompleted = !reader.IsDBNull(isCompletedOrdinal) && reader.GetBoolean(isCompletedOrdinal);

                string priorityText = reader.IsDBNull(priorityOrdinal) ? string.Empty : reader.GetString(priorityOrdinal);
                Priority? priority = Enum.TryParse<Priority>(priorityText, ignoreCase: true, out var parsedPriority)
                    ? parsedPriority
                    : null;

                task = new ToDoTask(Id, nazwa, isCompleted, priority);
            }


            return task;
        }

        public List<ToDoTask> Sort(string? sortBy = null)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = connection.CreateCommand();
            string query =
            """
            SELECT id, nazwa, isCompleted, priority
            FROM tasks
        """;

            //sortowanie - wybór

            if (sortBy == "alphabetical")
            {
                query += " ORDER BY nazwa ASC;"; //alfabytycznie od A do Z
            }
            else if (sortBy == "completed-first")
            {
                query += " ORDER BY isCompleted DESC;";

            }
            else if (sortBy == "uncompleted-first")
            {
                query += " ORDER BY isCompleted ASC;";

            }
            else
            {
                query += ";"; //domyślnie, bez sortowania
            }

            System.Diagnostics.Debug.WriteLine($"Wykonuję SQL: {query}");

            cmd.CommandText = query;


            using SqlDataReader reader = cmd.ExecuteReader();
            List<ToDoTask> tasks = new List<ToDoTask>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                bool isCompleted = reader.GetInt32(reader.GetOrdinal("isCompleted")) != 0;
                Priority? priority = reader.IsDBNull(3) ? (Priority?)null : (Priority)reader.GetInt32(3);


                var task = new ToDoTask(id, nazwa, isCompleted, priority.HasValue ? (Priority?)priority.Value : null);


                tasks.Add(task);
            }

            return tasks;
        }

        public ToDoTask Update(ToDoTask task)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            UPDATE tasks 
            SET nazwa = @nazwa,
        isCompleted = @isCompleted,
        priority = @priority
            WHERE id = @id;
        """;

            cmd.Parameters.AddWithValue("@nazwa", task.Title);
            cmd.Parameters.AddWithValue("@isCompleted", task.isCompleted);
            cmd.Parameters.AddWithValue("@id", task.Id);
            cmd.Parameters.AddWithValue("@priority", task.Priority.HasValue ? (object)task.Priority.Value : DBNull.Value);

            int rowsAffected = cmd.ExecuteNonQuery();
            return task;
        }
    }
}
