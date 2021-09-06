using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using TaskManagerDatabase.Models;

namespace TaskManagerDatabase
{
    public class SqlGroupRepository
    {
        /// <summary>
        /// Connection string used to connect to database
        /// </summary>
        private readonly string connectionString;

        public SqlGroupRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new group
        /// </summary>
        /// <param name="groupName">Group Name</param>
        /// <param name="ownerId">Owner's User Id</param>
        /// <returns></returns>
        public Group CreateGroup(string groupName, int ownerId)
        {
            // Verify parameters.
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(groupName));

            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.CreateGroup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("GroupName", groupName);
                        command.Parameters.AddWithValue("OwnerId", ownerId);

                        var p = command.Parameters.Add("GroupId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var groupId = (int)command.Parameters["GroupId"].Value;

                        return new Group(groupId, groupName, ownerId);
                    }
                }
            }
        }

        /// <summary>
        /// Add's a member to a groupd given the groups Group Id and the users User Id
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="userId">User Id</param>
        public void AddGroupMember(int groupId, int userId)
        {
            // Save to database.
            using (var transaction = new TransactionScope())
            {
                using (var connection = Program.ExtConnection)
                {
                    using (var command = new SqlCommand("TaskManager.AddGroupMember", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("GroupId", groupId);
                        command.Parameters.AddWithValue("UserId", userId);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve Group from database
        /// </summary>
        /// <param name="groupId">GroupId of the Group</param>
        /// <returns></returns>
        public Group FetchGroup(int groupId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.FetchGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("GroupId", groupId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var group = TranslateGroup(reader);

                        if (group == null)
                            throw new NullReferenceException(); // Group null

                        return group;
                    }
                }
            }
        }

        /// <summary>
        /// Get Group from database by GroupName
        /// </summary>
        /// <param name="groupId">GroupName of the Group</param>
        /// <returns></returns>
        public Group GetGroupByGroupName(string groupName)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetGroupByGroupname", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("GroupName", groupName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var group = TranslateGroup(reader);

                        return group;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve multiple Groups by UserId from database
        /// </summary>
        /// <param name="userId">UserId of the User</param>
        /// <returns></returns>
        public IReadOnlyList<Group> GetGroupsByUserId(int userId)
        {
            using (var connection = Program.ExtConnection)
            {
                using (var command = new SqlCommand("TaskManager.GetGroupsByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserId", userId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var groups = TranslateGroups(reader);

                        return groups;
                    }
                }
            }
        }

        /// <summary>
        /// Translate SQL query response into Group class
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Group TranslateGroup(SqlDataReader reader)
        {
            var groupIdOrdinal = reader.GetOrdinal("GroupId");
            var groupNameOrdinal = reader.GetOrdinal("GroupName");
            var ownerIdOrdinal = reader.GetOrdinal("OwnerId");

            if (!reader.Read())
                return null;

            return new Group(
               reader.GetInt32(groupIdOrdinal),
               reader.GetString(groupNameOrdinal),
               reader.GetInt32(ownerIdOrdinal)
               );
        }

        /// <summary>
        /// Translate SQL query response w/multiple rows into Group class list
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IReadOnlyList<Group> TranslateGroups(SqlDataReader reader)
        {
            var groups = new List<Group>();

            var groupIdOrdinal = reader.GetOrdinal("GroupId");
            var groupNameOrdinal = reader.GetOrdinal("GroupName");
            var ownerIdOrdinal = reader.GetOrdinal("OwnerId");

            while (reader.Read())
            {
                groups.Add(new Group(
                    reader.GetInt32(groupIdOrdinal),
                    reader.GetString(groupNameOrdinal),
                    reader.GetInt32(ownerIdOrdinal)));
            }

            return groups;
        }
    }
}
