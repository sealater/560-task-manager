CREATE OR ALTER PROCEDURE TaskManager.AddGroupMember
   @GroupId INT,
   @UserId INT
AS

INSERT INTO TaskManager.[GroupMember](GroupId, UserId)
VALUES(@GroupId, @UserId);

GO