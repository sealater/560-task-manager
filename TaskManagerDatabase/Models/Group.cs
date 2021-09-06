using System;

namespace TaskManagerDatabase.Models
{
    public class Group
    {
        /// <summary>
        /// GroupId of Group
        /// </summary>
        public int GroupId { get; }

        /// <summary>
        /// GroupName of Group
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// OwnerId of Group
        /// </summary>
        public int OwnerId { get; }

        /// <summary>
        /// A constructor for the properties
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <param name="ownerId"></param>
        internal Group(int groupId, string groupName, int ownerId)
        {
            GroupId = groupId;
            GroupName = groupName;
            OwnerId = ownerId;
        }

    }
}