CREATE OR ALTER PROCEDURE TaskManager.GetTasksByGroupId
   @GroupId INT
AS

SELECT T.TaskId, T.GroupId, T.UserId, T.OwnerType, T.TaskTitle, T.TaskContent,
        T.CreatedOn, T.UpdatedOn, T.CompletedOn, T.RemovedOn, T.DueOn
FROM TaskManager.[Task] T
    INNER JOIN TaskManager.[Group] G ON T.GroupId = G.GroupId
WHERE G.GroupId = @GroupId AND T.OwnerType = 2; -- Group Task
GO