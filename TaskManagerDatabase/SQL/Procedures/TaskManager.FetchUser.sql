CREATE OR ALTER PROCEDURE TaskManager.FetchUser
   @UserId INT
AS

SELECT U.PersonId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[User] U
WHERE U.UserId = @UserId;
GO