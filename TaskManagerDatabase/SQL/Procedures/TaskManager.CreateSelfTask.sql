CREATE OR ALTER PROCEDURE TaskManager.CreateSelfTask
   @TaskId INT OUTPUT,
   @UserId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

INSERT INTO TaskManager.[Task](UserId, OwnerType, TaskTitle, TaskContent, DueOn)
VALUES(@UserId, 1, @TaskTitle, @TaskContent, @DueOn);

SET @TaskId = SCOPE_IDENTITY();
GO