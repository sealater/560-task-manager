using System;

namespace TaskManagerDatabase.Models
{
    public class Task
    {
        /// <summary>
        /// property for the task id
        /// </summary>
        public int TaskId { get; }

        /// <summary>
        /// property for the foreign groupid key
        /// </summary>
        public int? GroupId { get; }

        /// <summary>
        /// property for the foreign userid key
        /// </summary>
        public int? UserId { get; }

        /// <summary>
        /// property for the foreign ownertype key
        /// </summary>
        public int OwnerType { get; }

        /// <summary>
        /// property for the title for a task
        /// </summary>
        public string TaskTitle { get; }

        /// <summary>
        /// property for the task 
        /// </summary>
        public string? TaskContent { get; }

        /// <summary>
        /// property for the creation date
        /// </summary>
        public DateTimeOffset CreatedOn { get; }

        /// <summary>
        /// property for the update date on a task
        /// </summary>
        public DateTimeOffset UpdatedOn { get; }

        /// <summary>
        /// property for the date the task was completed on 
        /// </summary>
        public DateTimeOffset? CompletedOn { get; }

        /// <summary>
        /// property for the date the task was removed from the Task tabe
        /// </summary>
        public DateTimeOffset? RemovedOn { get; }

        /// <summary>
        /// property for the date the task is due
        /// </summary>
        public DateTimeOffset? DueOn { get; }

        /// <summary>
        /// constructor for the class
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskTitle"></param>
        /// <param name="taskContent"></param>
        /// <param name="createdOn"></param>
        /// <param name="updatedOn"></param>
        /// <param name="completedOn"></param>
        /// <param name="removedOn"></param>
        /// <param name="dueOn"></param>
        internal Task(int taskId, int? groupId, int? userId, int ownerType, string taskTitle,
            string? taskContent, DateTimeOffset createdOn, DateTimeOffset updatedOn, DateTimeOffset? completedOn,
            DateTimeOffset? removedOn, DateTimeOffset? dueOn)
        {
            TaskId = taskId;
            GroupId = groupId;
            UserId = userId;
            OwnerType = ownerType;
            TaskTitle = taskTitle;
            TaskContent = taskContent;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            CompletedOn = completedOn;
            RemovedOn = removedOn;
            DueOn = dueOn;
        }


    }
}