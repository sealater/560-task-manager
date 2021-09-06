CREATE OR ALTER PROCEDURE TaskManager.GetGroupsByUserId
   @UserId INT
AS

SELECT G.GroupId, G.GroupName, G.OwnerId
FROM TaskManager.[Group] G
    INNER JOIN TaskManager.[GroupMember] GM ON G.GroupId = GM.GroupId
    INNER JOIN TaskManager.[User] U ON GM.UserId = U.UserId
WHERE U.UserId = @UserId
GO