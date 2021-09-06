CREATE OR ALTER PROCEDURE TaskManager.CreateGroupMemberTask
   @TaskId INT OUTPUT,
   @GroupId INT,
   @UserId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

INSERT INTO TaskManager.[Task](GroupId, UserId, OwnerType, TaskTitle, TaskContent, DueOn)
VALUES(@GroupId, @UserId, 3, @TaskTitle, @TaskContent, @DueOn);

SET @TaskId = SCOPE_IDENTITY();
GO