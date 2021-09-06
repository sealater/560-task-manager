CREATE OR ALTER PROCEDURE TaskManager.GetUsersByGroupId
   @GroupId INT
AS

SELECT U.UserId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[GroupMember] GM
    INNER JOIN TaskManager.[User] U ON GM.UserId = U.UserId
WHERE GM.GroupId = @GroupId;
GO