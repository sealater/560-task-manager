using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerDatabase.Models
{
    public class User
    {
        /// <summary>
        /// UserId of User
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Username of User
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// PasswordHash of User
        /// </summary>
        public string PasswordHash { get; }

        /// <summary>
        /// FirstName of User
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// LastName of User
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// JoinDate of User
        /// </summary>
        public DateTimeOffset JoinDate { get; }

        internal User(int userId, string userName, string passwordHash,
            string firstName, string lastName, DateTimeOffset joinDate)
        {
            UserId = userId;
            Username = userName;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            JoinDate = joinDate;
        }
    }
}
