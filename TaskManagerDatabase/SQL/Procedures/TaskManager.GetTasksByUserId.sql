CREATE OR ALTER PROCEDURE TaskManager.GetTasksByUserId
   @UserId INT
AS

SELECT T.TaskId, T.GroupId, T.UserId, T.OwnerType, T.TaskTitle, T.TaskContent,
        T.CreatedOn, T.UpdatedOn, T.CompletedOn, T.RemovedOn, T.DueOn
FROM TaskManager.[Task] T
    INNER JOIN TaskManager.[User] U ON T.UserId = U.UserId
WHERE U.UserId = @UserId;
GO