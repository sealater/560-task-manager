CREATE OR ALTER PROCEDURE TaskManager.FetchGroup
   @GroupId INT
AS

SELECT G.GroupId, G.GroupName, G.OwnerId
FROM TaskManager.[Group] G
WHERE G.GroupId = @GroupId;
GO