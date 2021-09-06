CREATE OR ALTER PROCEDURE TaskManager.FetchTask
   @TaskId INT
AS

SELECT *
FROM TaskManager.[Task] T
WHERE T.TaskId = @TaskId;
GO