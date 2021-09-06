--CREATE SCHEMA TaskManager;

DROP TABLE IF EXISTS TaskManager.[Task]
DROP TABLE IF EXISTS TaskManager.[OwnerType]
DROP TABLE IF EXISTS TaskManager.[GroupMember]
DROP TABLE IF EXISTS TaskManager.[Group]
DROP TABLE IF EXISTS TaskManager.[User]

CREATE TABLE TaskManager.[User]
(
    UserId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    Username NVARCHAR(32) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(64) NOT NULL,
    FirstName NVARCHAR(16) NOT NULL,
    LastName NVARCHAR(16) NOT NULL,
    JoinDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
)

CREATE TABLE TaskManager.[Group]
(
    GroupId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    GroupName NVARCHAR(32) NOT NULL UNIQUE,
    --GroupTopic NVARCHAR(64) NOT NULL,
    OwnerId INT FOREIGN KEY REFERENCES TaskManager.[User](UserId)
)

CREATE TABLE TaskManager.[OwnerType]
(
    [TypeId] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [TypeName] NVARCHAR(16) NOT NULL
)

CREATE TABLE TaskManager.[GroupMember]
(
    GroupId INT NOT NULL FOREIGN KEY REFERENCES TaskManager.[Group](GroupId),
    UserId INT NOT NULL FOREIGN KEY REFERENCES TaskManager.[User](UserId),

    PRIMARY KEY
    (
        GroupId, UserId
    )
)

CREATE TABLE TaskManager.[Task]
(
    TaskId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    GroupId INT FOREIGN KEY REFERENCES TaskManager.[Group](GroupId),
    UserId INT FOREIGN KEY REFERENCES TaskManager.[User](UserId),
    OwnerType INT NOT NULL FOREIGN KEY REFERENCES TaskManager.[OwnerType](TypeId),
    TaskTitle NVARCHAR(32) NOT NULL,
    TaskContent NVARCHAR(1024),
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CompletedOn DATETIMEOFFSET,
    RemovedOn DATETIMEOFFSET,
    DueOn DATETIMEOFFSET,

    FOREIGN KEY
    ( 
        GroupId, UserId
    )
    REFERENCES TaskManager.[GroupMember]
    (
        GroupId, UserId
    ),

    CHECK
    (
        OwnerType = 1 AND UserId IS NOT NULL AND GroupId IS NULL
        OR OwnerType = 2 AND UserId IS NULL AND GroupId IS NOT NULL
        OR OwnerType = 3 AND UserId IS NOT NULL AND GroupId IS NOT NULL
    )
)

CREATE UNIQUE NONCLUSTERED INDEX [UniqueGroupTaskTitle] ON TaskManager.[Task]
(
    GroupId, TaskTitle
)
WHERE OwnerType IN (2, 3); -- Group, GroupMember

CREATE UNIQUE NONCLUSTERED INDEX [UniqueUserTaskTitle] ON TaskManager.[Task]
(
    UserId, TaskTitle
)
WHERE OwnerType IN (1); -- Self

INSERT INTO TaskManager.[OwnerType] (TypeName)
VALUES ('Self'), ('Group'), ('GroupMember')

-- Add INSERT
-- Self 1
-- Group 2
-- GroupMember 3
