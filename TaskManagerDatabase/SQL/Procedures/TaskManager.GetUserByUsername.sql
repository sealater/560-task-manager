CREATE OR ALTER PROCEDURE TaskManager.GetUserByUsername
   @Username NVARCHAR(32)
AS

SELECT U.UserId, U.Username, U.PasswordHash, U.FirstName, U.LastName, U.JoinDate
FROM TaskManager.[User] U
WHERE U.Username = @Username;
GO