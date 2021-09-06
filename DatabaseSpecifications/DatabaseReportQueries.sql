-- Report Query: CompletedTaskCount
SELECT U.Username, COUNT(T.TaskId) AS CompletedTaskCount
FROM TaskManager.[User] U
    INNER JOIN TaskManager.[Task] T ON U.UserId = T.UserId
WHERE T.CompletedOn IS NOT NULL
GROUP BY U.Username
ORDER BY COUNT(T.TaskId) DESC

-- Report Query: RemovedTaskCount
SELECT U.Username, COUNT(T.TaskId) AS RemovedTaskCount
FROM TaskManager.[User] U
    INNER JOIN TaskManager.[Task] T ON U.UserId = T.UserId
WHERE T.RemovedOn IS NOT NULL
GROUP BY U.Username
ORDER BY COUNT(T.TaskId) DESC

-- Report Query: Average Time To Complete A Task
WITH TotalTimeToCompleteCTE(TaskId, UserId, Username, TotalTime, TotalTasks) AS
    (
        SELECT T.TaskId, U.UserId, U.Username, SUM(DATEDIFF(HOUR, T.CreatedOn, T.CompletedOn)), SUM(T.TaskId)
        FROM TaskManager.[Task] T
            INNER JOIN TaskManager.[User] U ON T.UserId = U.UserId
        WHERE T.CompletedOn IS NOT NULL
        GROUP BY U.UserId, U.Username, T.TaskId
    )
SELECT TTCte.Username, TTCte.TotalTime/TTCte.TotalTasks AS AverageTimeToCompleteTaskInHours
FROM TotalTimeToCompleteCTE TTCte

-- Percentage of tasks completed before the specified due date
WITH TasksCompletedBeforeDueDate(UserId, NumberOfCompletedTasks) AS
    (
        SELECT U.UserId, COUNT(T.TaskId)
        FROM TaskManager.[Task] T
            INNER JOIN TaskManager.[User] U ON T.UserId = U.UserId
        WHERE T.CompletedOn IS NOT NULL AND T.DueOn IS NOT NULL AND T.CompletedOn < T.DueOn
        GROUP BY U.UserId
    )
SELECT CTE.UserId, CTE.NumberOfCompletedTasks*100/COUNT(T.TaskId) AS PercentageOfTasksCompletedBeforeTheSpecifiedDueDate
FROM TaskManager.[Task] T
    LEFT JOIN TasksCompletedBeforeDueDate CTE ON CTE.UserId = T.UserId
WHERE (T.DueOn IS NOT NULL AND T.CompletedOn IS NOT NULL) OR (T.CompletedOn IS NULL AND T.DueOn < SYSDATETIMEOFFSET())
GROUP BY CTE.UserId, CTE.NumberOfCompletedTasks

SELECT *
FROM TaskManager.[Task]