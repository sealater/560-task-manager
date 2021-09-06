import json
import random
import datetime

# Helper Methods
def random_date(start, end):
    delta = end - start
    int_delta = (delta.days * 24 * 60 * 60) + delta.seconds
    random_second = random.randrange(int_delta)
    return start + datetime.timedelta(seconds=random_second)


with open("USERS.json") as USER_DATA:
    users = json.load(USER_DATA)

with open("GROUPS.json") as GROUP_DATA:
    groups = json.load(GROUP_DATA)

with open("TASKS.json") as TASK_DATA:
    tasks = json.load(TASK_DATA)

SQL = []


# Generate Users
for uid, user in enumerate(users):
    SQL.append("INSERT INTO TaskManager.[User] (Username, PasswordHash, FirstName, LastName) VALUES ('{0}', '{1}', '{2}', '{3}')".format(user["Username"], user["PasswordHash"], user["FirstName"].replace('\'', ""), user["LastName"].replace('\'', "")))
    
# Generate Groups
## This is taken care of below

# Generate Groups & GroupMembers

## for each group
### generate random number 1-100 OwnerId
### assign 1 OwnerId to each group

groupMembers = {}
for x in range(1, 100+1):
    groupMembers[x] = []

for groupId, group in enumerate(groups):
    ownerId = random.randint(1, 100)
    groupMembers[groupId+1].append(ownerId)
    # Create the group
    SQL.append("INSERT INTO TaskManager.[Group] (GroupName, OwnerId) VALUES ('{0}', '{1}')".format(group["GroupName"], ownerId))
    # Add owner to group
    SQL.append("INSERT INTO TaskManager.[GroupMember] (GroupId, UserId) VALUES ({0}, {1})".format(groupId+1, ownerId))

## for each user
### generate random number 1-10 X of groups to assign to
### generate X random numbers between 1-100
### if user not already in group then assign to group

for user in enumerate(users):
    numOfGroups = random.randint(1,5)
    for i in range(numOfGroups):
        userId = random.randint(1,100)
        
        groupId = random.randint(1,100)
        check = True
        
        for uid in groupMembers[groupId]:
            if userId == uid:
                check = False

        groupMembers[groupId].append(userId)
        if check:
            SQL.append("INSERT INTO TaskManager.[GroupMember] (GroupId, UserId) VALUES ({0}, {1})".format(groupId, userId))

# Generate User Tasks

## for each task generate random number 1-3 OwnerType
### if OwnerType = 1 then
#### generate random number 1-100 UserId to assign task to
### if OwnerType = 2
#### generate random number 1-100 GroupId to assign task to
### if OwnerType = 3
#### generate random number 1-100 GroupId & 1-100 UserId to assign task to

# Date range
d1 = datetime.datetime.strptime('5/10/2020 12:00 AM', '%m/%d/%Y %I:%M %p')
d2 = datetime.datetime.strptime('1/1/2022 12:00 AM', '%m/%d/%Y %I:%M %p')

for taskId, task in enumerate(tasks):
    ownerType = random.randint(1, 3)
    groupId = random.randint(1, 100)
    userId = random.randint(1, 100)
    dueOn = "'{}'".format(random_date(d1, d2))
    completedOrRemoved = "'{}'".format(random_date(d1, d2))
    completedOn = 'NULL'
    removedOn = 'NULL'

    if (random.randint(1,4) == 1):
        completedOn = completedOrRemoved
    elif (random.randint(1,4) == 2):
        removedOn = completedOrRemoved

    if (random.randint(1, 5) > 4):
        dueOn = 'NULL'
    
    if ownerType == 1:
        SQL.append("INSERT INTO TaskManager.[Task] (UserId, OwnerType, TaskTitle, TaskContent, CompletedOn, RemovedOn, DueOn) VALUES ({0}, 1, '{1}', '{2}', {3}, {4}, {5})".format(userId, task["TaskTitle"].replace('\'', ""), task["TaskContent"], completedOn, removedOn, dueOn))
    elif ownerType == 2:
        SQL.append("INSERT INTO TaskManager.[Task] (GroupId, OwnerType, TaskTitle, TaskContent, CompletedOn, RemovedOn, DueOn) VALUES ({0}, 2, '{1}', '{2}', {3}, {4}, {5})".format(groupId, task["TaskTitle"].replace('\'', ""), task["TaskContent"], completedOn, removedOn, dueOn))
    elif ownerType == 3:
        memberId = random.randint(0, len(groupMembers[groupId]) - 1)
        SQL.append("INSERT INTO TaskManager.[Task] (GroupId, UserId, OwnerType, TaskTitle, TaskContent, CompletedOn, RemovedOn, DueOn) VALUES ({0}, {1}, 3, '{2}', '{3}', {4}, {5}, {6})".format(groupId, groupMembers[groupId][memberId], task["TaskTitle"].replace('\'', ""), task["TaskContent"], completedOn, removedOn, dueOn))


for i in SQL:
    print(i)

input("Waiting...")
