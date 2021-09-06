CREATE OR ALTER PROCEDURE TaskManager.CreateGroupTask
   @TaskId INT OUTPUT,
   @GroupId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

INSERT INTO TaskManager.[Task](GroupId, OwnerType, TaskTitle, TaskContent, DueOn)
VALUES(@GroupId, 2, @TaskTitle, @TaskContent, @DueOn);

SET @TaskId = SCOPE_IDENTITY();
GO