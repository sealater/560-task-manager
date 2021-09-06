CREATE OR ALTER PROCEDURE TaskManager.CreateGroup
   @GroupId INT OUTPUT,
   @GroupName NVARCHAR(32),
   @OwnerId INT
AS

INSERT INTO TaskManager.[Group](GroupName, OwnerId)
VALUES(@GroupName, @OwnerId);

SET @GroupId = SCOPE_IDENTITY();
GO