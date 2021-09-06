CREATE OR ALTER PROCEDURE TaskManager.CompleteTask
   @TaskId INT
AS

UPDATE TaskManager.Task
SET CompletedOn = SYSDATETIMEOFFSET(),
    UpdatedOn = SYSDATETIMEOFFSET()
GO