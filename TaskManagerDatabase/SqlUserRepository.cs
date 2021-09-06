using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.Text;
using TaskManagerDatabase.Models;

namespace TaskManagerDatabase
{
    public class SqlUserRepository
    {
        /// <summary>
        /// Connection string used to connect to database
        /// </summary>
        private readonly string connectionString;

        public SqlUserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Retrieve User from database
        /// </summary>
        /// <param name="userId">UserId of the User</param>
        /// <returns></returns>
        public User FetchUser(int userId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.FetchUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserId", userId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var user = TranslateUser(reader);

                        if (user == null)
                            throw new NullReferenceException(); // User null

                        return user;
                    }
                }
            }
        }

        /// <summary>
        /// Get User by Username from database || Returns null if no User found
        /// </summary>
        /// <param name="userName">UserName of the User</param>
        /// <returns></returns>
        public User GetUserByUsername(string userName)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetUserByUsername", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var user = TranslateUser(reader);

                        return user;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve multiple Users by GroupId from database
        /// </summary>
        /// <param name="groupId">GroupId of the Group</param>
        /// <returns></returns>
        public IReadOnlyList<User> GetUsersByGroupId(int groupId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetUsersByGroupId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("GroupId", groupId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var users = TranslateUsers(reader);

                        return users;
                    }
                }
            }
        }

        /// <summary>
        /// Translate SQL query response into User class
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private User TranslateUser(SqlDataReader reader)
        {
            var userIdOrdinal = reader.GetOrdinal("UserId");
            var userNameOrdinal = reader.GetOrdinal("Username");
            var passwordHashOrdinal = reader.GetOrdinal("PasswordHash");
            var firstNameOrdinal = reader.GetOrdinal("FirstName");
            var lastNameOrdinal = reader.GetOrdinal("LastName");
            var joinDateOrdinal = reader.GetOrdinal("JoinDate");

            if (!reader.Read())
                return null;

            return new User(
               reader.GetInt32(userIdOrdinal),
               reader.GetString(userNameOrdinal),
               reader.GetString(passwordHashOrdinal),
               reader.GetString(firstNameOrdinal),
               reader.GetString(lastNameOrdinal),
               reader.GetDateTimeOffset(joinDateOrdinal));
        }

        /// <summary>
        /// Translate SQL query response into User's class
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IReadOnlyList<User> TranslateUsers(SqlDataReader reader)
        {
            var users = new List<User>();

            var userIdOrdinal = reader.GetOrdinal("UserId");
            var userNameOrdinal = reader.GetOrdinal("Username");
            var passwordHashOrdinal = reader.GetOrdinal("PasswordHash");
            var firstNameOrdinal = reader.GetOrdinal("FirstName");
            var lastNameOrdinal = reader.GetOrdinal("LastName");
            var joinDateOrdinal = reader.GetOrdinal("JoinDate");

            while (reader.Read())
            {
                users.Add(new User(
                    reader.GetInt32(userIdOrdinal),
                    reader.GetString(userNameOrdinal),
                    reader.GetString(passwordHashOrdinal),
                    reader.GetString(firstNameOrdinal),
                    reader.GetString(lastNameOrdinal),
                    reader.GetDateTimeOffset(joinDateOrdinal)));
            }

            return users;
        }
    }
}
