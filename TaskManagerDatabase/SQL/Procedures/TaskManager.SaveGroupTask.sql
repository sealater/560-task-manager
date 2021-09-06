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