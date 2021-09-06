-- Add GroupMember
CREATE OR ALTER PROCEDURE TaskManager.AddGroupMember
   @GroupId INT,
   @UserId INT
AS

INSERT INTO TaskManager.[GroupMember](GroupId, UserId)
VALUES(@GroupId, @UserId);

GO

-- Complete Task
CREATE OR ALTER PROCEDURE TaskManager.CompleteTask
   @TaskId INT
AS

UPDATE TaskManager.Task
SET CompletedOn = SYSDATETIMEOFFSET(),
    UpdatedOn = SYSDATETIMEOFFSET()
WHERE TaskId = @TaskId
GO

-- Create Group
CREATE OR ALTER PROCEDURE TaskManager.CreateGroup
   @GroupId INT OUTPUT,
   @GroupName NVARCHAR(32),
   @OwnerId INT
AS

INSERT INTO TaskManager.[Group](GroupName, OwnerId)
VALUES(@GroupName, @OwnerId);

SET @GroupId = SCOPE_IDENTITY();
GO

-- Create GroupMember Task
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

-- Create Group Task
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

-- Create Self Task
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

-- Get Group By GroupName
CREATE OR ALTER PROCEDURE TaskManager.GetGroupByGroupName
   @GroupName NVARCHAR(32)
AS

SELECT G.GroupId, G.GroupName, G.OwnerId
FROM TaskManager.[Group] G
WHERE G.GroupName = @GroupName;
GO

-- Fetch Group
CREATE OR ALTER PROCEDURE TaskManager.FetchGroup
   @GroupId INT
AS

SELECT G.GroupId, G.GroupName, G.OwnerId
FROM TaskManager.[Group] G
WHERE G.GroupId = @GroupId;
GO

-- Fetch Task
CREATE OR ALTER PROCEDURE TaskManager.FetchTask
   @TaskId INT
AS

SELECT *
FROM TaskManager.[Task] T
WHERE T.TaskId = @TaskId;
GO

-- Fetch User
CREATE OR ALTER PROCEDURE TaskManager.FetchUser
   @UserId INT
AS

SELECT U.UserId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[User] U
WHERE U.UserId = @UserId;
GO

-- Get Groups By UserId
CREATE OR ALTER PROCEDURE TaskManager.GetGroupsByUserId
   @UserId INT
AS

SELECT G.GroupId, G.GroupName, G.OwnerId
FROM TaskManager.[Group] G
    INNER JOIN TaskManager.[GroupMember] GM ON G.GroupId = GM.GroupId
    INNER JOIN TaskManager.[User] U ON GM.UserId = U.UserId
WHERE U.UserId = @UserId
GO

-- Get Tasks By GroupId
CREATE OR ALTER PROCEDURE TaskManager.GetTasksByGroupId
   @GroupId INT
AS

SELECT T.TaskId, T.GroupId, T.UserId, T.OwnerType, T.TaskTitle, T.TaskContent,
        T.CreatedOn, T.UpdatedOn, T.CompletedOn, T.RemovedOn, T.DueOn
FROM TaskManager.[Task] T
    INNER JOIN TaskManager.[Group] G ON T.GroupId = G.GroupId
WHERE G.GroupId = @GroupId AND T.OwnerType = 2; -- Group Task
GO

-- Get Tasks By UserId
CREATE OR ALTER PROCEDURE TaskManager.GetTasksByUserId
   @UserId INT
AS

SELECT T.TaskId, T.GroupId, T.UserId, T.OwnerType, T.TaskTitle, T.TaskContent,
        T.CreatedOn, T.UpdatedOn, T.CompletedOn, T.RemovedOn, T.DueOn
FROM TaskManager.[Task] T
    INNER JOIN TaskManager.[User] U ON T.UserId = U.UserId
WHERE U.UserId = @UserId;
GO

-- Get User By Username
CREATE OR ALTER PROCEDURE TaskManager.GetUserByUsername
   @Username NVARCHAR(32)
AS

SELECT U.UserId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[User] U
WHERE U.Username = @Username;
GO

-- Get Users By GroupId

CREATE OR ALTER PROCEDURE TaskManager.GetUsersByGroupId
   @GroupId INT
AS

SELECT U.UserId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[GroupMember] GM
    INNER JOIN TaskManager.[User] U ON GM.UserId = U.UserId
WHERE GM.GroupId = @GroupId;
GO

-- Remove Task
CREATE OR ALTER PROCEDURE TaskManager.RemoveTask
   @TaskId INT
AS

UPDATE TaskManager.Task
SET RemovedOn = SYSDATETIMEOFFSET(),
    UpdatedOn = SYSDATETIMEOFFSET()
WHERE TaskId = @TaskId
GO

-- Save GroupMember Task
CREATE OR ALTER PROCEDURE TaskManager.SaveGroupMemberTask
   @TaskId INT,
   @GroupId INT,
   @UserId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

MERGE TaskManager.Task T
USING
      (
         VALUES(@TaskId, @GroupId, @UserId, @TaskTitle, @TaskContent, @DueOn)
      ) S(TaskId, GroupId, UserId, TaskTitle, TaskContent, DueOn)
   ON S.TaskId = T.TaskId
WHEN MATCHED AND NOT EXISTS
      (
         SELECT S.TaskTitle, S.TaskContent, S.DueOn
         INTERSECT
         SELECT  T.TaskTitle, T.TaskContent, T.DueOn
      ) THEN
   UPDATE
   SET
      TaskTitle = S.TaskTitle,
      TaskContent = S.TaskContent,
      DueOn = S.DueOn,
      UpdatedOn = SYSDATETIMEOFFSET();
GO

-- Save Group Task
CREATE OR ALTER PROCEDURE TaskManager.SaveGroupTask
   @TaskId INT,
   @GroupId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

MERGE TaskManager.Task T
USING
      (
         VALUES(@TaskId, @GroupId, @TaskTitle, @TaskContent, @DueOn)
      ) S(TaskId, GroupId, TaskTitle, TaskContent, DueOn)
   ON S.TaskId = T.TaskId
WHEN MATCHED AND NOT EXISTS
      (
         SELECT S.TaskTitle, S.TaskContent, S.DueOn
         INTERSECT
         SELECT  T.TaskTitle, T.TaskContent, T.DueOn
      ) THEN
   UPDATE
   SET
      TaskTitle = S.TaskTitle,
      TaskContent = S.TaskContent,
      DueOn = S.DueOn,
      UpdatedOn = SYSDATETIMEOFFSET();
GO

-- Save Self Task
CREATE OR ALTER PROCEDURE TaskManager.SaveSelfTask
   @TaskId INT,
   @UserId INT,
   @TaskTitle NVARCHAR(32),
   @TaskContent NVARCHAR(1024),
   @DueOn DATETIMEOFFSET
AS

MERGE TaskManager.Task T
USING
      (
         VALUES(@TaskId, @UserId, @TaskTitle, @TaskContent, @DueOn)
      ) S(TaskId, UserId, TaskTitle, TaskContent, DueOn)
   ON S.TaskId = T.TaskId
WHEN MATCHED AND NOT EXISTS
      (
         SELECT S.TaskTitle, S.TaskContent, S.DueOn
         INTERSECT
         SELECT  T.TaskTitle, T.TaskContent, T.DueOn
      ) THEN
   UPDATE
   SET
      TaskTitle = S.TaskTitle,
      TaskContent = S.TaskContent,
      DueOn = S.DueOn,
      UpdatedOn = SYSDATETIMEOFFSET();
GO