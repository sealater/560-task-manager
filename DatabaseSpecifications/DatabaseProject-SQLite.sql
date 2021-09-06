--CREATE SCHEMA TaskManager;

DROP TABLE IF EXISTS [Task]
DROP TABLE IF EXISTS [OwnerType]
DROP TABLE IF EXISTS [GroupMember]
DROP TABLE IF EXISTS [Group]
DROP TABLE IF EXISTS [User]

CREATE TABLE [User]
(
    UserId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    Username NVARCHAR(32) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(64) NOT NULL,
    FirstName NVARCHAR(16) NOT NULL,
    LastName NVARCHAR(16) NOT NULL,
    JoinDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
)

CREATE TABLE [Group]
(
    GroupId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    GroupName NVARCHAR(32) NOT NULL UNIQUE,
    --GroupTopic NVARCHAR(64) NOT NULL,
    OwnerId INT FOREIGN KEY REFERENCES [User](UserId)
)

CREATE TABLE [OwnerType]
(
    [TypeId] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [TypeName] NVARCHAR(16) NOT NULL
)

CREATE TABLE [GroupMember]
(
    GroupId INT NOT NULL FOREIGN KEY REFERENCES [Group](GroupId),
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](UserId),

    PRIMARY KEY
    (
        GroupId, UserId
    )
)

CREATE TABLE [Task]
(
    TaskId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    GroupId INT FOREIGN KEY REFERENCES [Group](GroupId),
    UserId INT FOREIGN KEY REFERENCES [User](UserId),
    OwnerType INT NOT NULL FOREIGN KEY REFERENCES [OwnerType](TypeId),
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
    REFERENCES [GroupMember]
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

CREATE UNIQUE NONCLUSTERED INDEX [UniqueGroupTaskTitle] ON [Task]
(
    GroupId, TaskTitle
)
WHERE OwnerType IN (2, 3); -- Group, GroupMember

CREATE UNIQUE NONCLUSTERED INDEX [UniqueUserTaskTitle] ON [Task]
(
    UserId, TaskTitle
)
WHERE OwnerType IN (1); -- Self

INSERT INTO [OwnerType] (TypeName)
VALUES ('Self'), ('Group'), ('GroupMember')

-- Add INSERT
-- Self 1
-- Group 2
-- GroupMember 3
