using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.Text;
using TaskManagerDatabase.Models;

namespace TaskManagerDatabase
{
    public class SqlTaskRepository
    {
        /// <summary>
        /// Connection string used to connect to database
        /// </summary>
        private readonly string connectionString;

        public SqlTaskRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        ///  Creates a group task given the Group's Id
        /// </summary>
        /// <param name="groupId">Group's Id</param>
        /// <param name="taskTitle">Task Title</param>
        /// <param name="taskContent">Task Content</param>
        /// <param name="dueOn">Due Date of Task or Null</param>
        /// <returns></returns>
        public Task CreateGroupTask(int groupId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (string.IsNullOrWhiteSpace(taskTitle))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskTitle));

            if (string.IsNullOrWhiteSpace(taskContent))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskContent));

            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.CreateGroupTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("GroupId", groupId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        var p = command.Parameters.Add("TaskId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var taskId = (int)command.Parameters["TaskId"].Value;

                        return new Task(taskId, groupId, null, 2, taskTitle, taskContent,
                            DateTimeOffset.Now, DateTimeOffset.Now, null, null, dueOn);
                    }
                }
            }
        }

        /// <summary>
        ///  Creates a self task given the User's Id
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <param name="taskTitle">Task Title</param>
        /// <param name="taskContent">Task Content</param>
        /// <param name="dueOn">Due Date of Task or Null</param>
        /// <returns></returns>
        public Task CreateSelfTask(int userId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (string.IsNullOrWhiteSpace(taskTitle))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskTitle));

            if (string.IsNullOrWhiteSpace(taskContent))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskContent));

            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.CreateSelfTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("UserId", userId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        var p = command.Parameters.Add("TaskId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var taskId = (int)command.Parameters["TaskId"].Value;

                        return new Task(taskId, null, userId, 1, taskTitle, taskContent,
                            DateTimeOffset.Now, DateTimeOffset.Now, null, null, dueOn);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a task for a member within a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <param name="taskTitle"></param>
        /// <param name="taskContent"></param>
        /// <param name="dueOn"></param>
        /// <returns></returns>
        public Task CreateGroupMemberTask(int groupId, int userId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (string.IsNullOrWhiteSpace(taskTitle))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskTitle));

            if (string.IsNullOrWhiteSpace(taskContent))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(taskContent));

            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.CreateGroupMemberTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("GroupId", groupId);
                        command.Parameters.AddWithValue("UserId", userId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        var p = command.Parameters.Add("TaskId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var taskId = (int)command.Parameters["TaskId"].Value;

                        return new Task(taskId, groupId, userId, 3, taskTitle, taskContent,
                            DateTimeOffset.Now, DateTimeOffset.Now, null, null, dueOn);
                    }
                }
            }
        }

        public void SaveGroupTask(int taskId, int groupId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (taskTitle == null)
                throw new ArgumentNullException(nameof(taskTitle));

            if (taskContent == null)
                throw new ArgumentNullException(nameof(taskContent));

            // Save address to the database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.SaveGroupTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TaskId", taskId);
                        command.Parameters.AddWithValue("GroupId", groupId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        public void SaveSelfTask(int taskId, int userId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (taskTitle == null)
                throw new ArgumentNullException(nameof(taskTitle));

            if (taskContent == null)
                throw new ArgumentNullException(nameof(taskContent));

            // Save address to the database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.SaveSelfTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TaskId", taskId);
                        command.Parameters.AddWithValue("UserId", userId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Saves a group members task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <param name="taskTitle"></param>
        /// <param name="taskContent"></param>
        /// <param name="dueOn"></param>
        public void SaveGroupMemberTask(int taskId, int groupId, int userId, string taskTitle, string taskContent, DateTimeOffset? dueOn)
        {
            // Verify parameters.
            if (taskTitle == null)
                throw new ArgumentNullException(nameof(taskTitle));

            if (taskContent == null)
                throw new ArgumentNullException(nameof(taskContent));

            // Save address to the database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.SaveGroupMemberTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TaskId", taskId);
                        command.Parameters.AddWithValue("GroupId", groupId);
                        command.Parameters.AddWithValue("UserId", userId);
                        command.Parameters.AddWithValue("TaskTitle", taskTitle);
                        command.Parameters.AddWithValue("TaskContent", taskContent);
                        command.Parameters.AddWithValue("DueOn", (object)dueOn ?? DBNull.Value);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Completes a task by setting it's CompletedOn column
        /// </summary>
        /// <param name="taskId"></param>
        public void CompleteTask(int taskId)
        {
            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.CompleteTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TaskId", taskId);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Completes a task by setting it's RemovedOn column
        /// </summary>
        /// <param name="taskId"></param>
        public void RemoveTask(int taskId)
        {
            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.RemoveTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TaskId", taskId);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve Task from database
        /// </summary>
        /// <param name="taskId">TaskId of the Task</param>
        /// <returns></returns>
        public Task FetchTask(int taskId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.FetchTask", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("TaskId", taskId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var task = TranslateTask(reader);

                        if (task == null)
                            throw new NullReferenceException(); // Group null

                        return task;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve multiple Tasks by UserId from database
        /// </summary>
        /// <param name="userId">UserId of the User</param>
        /// <returns></returns>
        public IReadOnlyList<Task> GetTasksByUserId(int userId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetTasksByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserId", userId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var tasks = TranslateTasks(reader);

                        return tasks;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve multiple Tasks by GroupId from database
        /// </summary>
        /// <param name="groupId">GroupId of the Group</param>
        /// <returns></returns>
        public IReadOnlyList<Task> GetTasksByGroupId(int groupId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetTasksByGroupId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("GroupId", groupId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var tasks = TranslateTasks(reader);

                        return tasks;
                    }
                }
            }
        }

        /// <summary>
        /// Translate SQL query response into Task class
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Task TranslateTask(SqlDataReader reader)
        {
            var taskIdOrdinal = reader.GetOrdinal("TaskId");
            var groupIdOrdinal = reader.GetOrdinal("GroupId");
            var userIdOrdinal = reader.GetOrdinal("UserId");
            var ownerTypeOrdinal = reader.GetOrdinal("OwnerType");
            var taskTitleOrdinal = reader.GetOrdinal("TaskTitle");
            var taskContentOrdinal = reader.GetOrdinal("TaskContent");
            var createdOnOrdinal = reader.GetOrdinal("CreatedOn");
            var updatedOnOrdinal = reader.GetOrdinal("UpdatedOn");
            var completedOnOrdinal = reader.GetOrdinal("CompletedOn");
            var removedOnOrdinal = reader.GetOrdinal("RemovedOn");
            var dueOnOrdinal = reader.GetOrdinal("DueOn");

            if (!reader.Read())
                return null;

            return new Task(
               reader.GetInt32(taskIdOrdinal),
               reader.IsDBNull(groupIdOrdinal) ? null : (int?)reader.GetInt32(groupIdOrdinal),
               reader.IsDBNull(userIdOrdinal) ? null : (int?)reader.GetInt32(userIdOrdinal),
               reader.GetInt32(ownerTypeOrdinal),
               reader.GetString(taskTitleOrdinal),
               reader.IsDBNull(taskContentOrdinal) ? null : reader.GetString(taskContentOrdinal),
               reader.GetDateTimeOffset(createdOnOrdinal),
               reader.GetDateTimeOffset(updatedOnOrdinal),
               reader.IsDBNull(completedOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(completedOnOrdinal),
               reader.IsDBNull(removedOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(removedOnOrdinal),
               reader.IsDBNull(dueOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(dueOnOrdinal)
               );
        }

        /// <summary>
        /// Translate SQL query response w/multiple rows into Task class list
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IReadOnlyList<Task> TranslateTasks(SqlDataReader reader)
        {
            var tasks = new List<Task>();

            var taskIdOrdinal = reader.GetOrdinal("TaskId");
            var groupIdOrdinal = reader.GetOrdinal("GroupId");
            var userIdOrdinal = reader.GetOrdinal("UserId");
            var ownerTypeOrdinal = reader.GetOrdinal("OwnerType");
            var taskTitleOrdinal = reader.GetOrdinal("TaskTitle");
            var taskContentOrdinal = reader.GetOrdinal("TaskContent");
            var createdOnOrdinal = reader.GetOrdinal("CreatedOn");
            var updatedOnOrdinal = reader.GetOrdinal("UpdatedOn");
            var completedOnOrdinal = reader.GetOrdinal("CompletedOn");
            var removedOnOrdinal = reader.GetOrdinal("RemovedOn");
            var dueOnOrdinal = reader.GetOrdinal("DueOn");

            while (reader.Read())
            {
                tasks.Add(new Task(
                   reader.GetInt32(taskIdOrdinal),
                   reader.IsDBNull(groupIdOrdinal) ? null : (int?)reader.GetInt32(groupIdOrdinal),
                   reader.IsDBNull(userIdOrdinal) ? null : (int?)reader.GetInt32(userIdOrdinal),
                   reader.GetInt32(ownerTypeOrdinal),
                   reader.GetString(taskTitleOrdinal),
                   reader.IsDBNull(taskContentOrdinal) ? null : reader.GetString(taskContentOrdinal),
                   reader.GetDateTimeOffset(createdOnOrdinal),
                   reader.GetDateTimeOffset(updatedOnOrdinal),
                   reader.IsDBNull(completedOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(completedOnOrdinal),
                   reader.IsDBNull(removedOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(removedOnOrdinal),
                   reader.IsDBNull(dueOnOrdinal) ? null : (DateTimeOffset?)reader.GetDateTimeOffset(dueOnOrdinal)
               ));
            }

            return tasks;
        }
    }
}
