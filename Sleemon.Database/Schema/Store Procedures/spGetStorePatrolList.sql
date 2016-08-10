CREATE PROCEDURE [dbo].[spGetStorePatrolList]
	@pageIndex	INT,
	@pageSize	INT,
	@userName	NVARCHAR(50),
	@startFrom	DATETIME,
	@endTo		DATETIME
AS

DECLARE @userNamePattern NVARCHAR(50) = CONCAT('%', ISNULL(@userName, N''), '%');

WITH [StorePatrolWithRowNumber] AS
(
	SELECT [Task].[Id] AS [TaskId]
		  ,[Task].[Title]
		  ,[Task].[Description]
		  ,[Task].[StartFrom]
		  ,[Task].[EndTo]
		  ,[Task].[Point]
		  ,[Task].[OverduePoint]
		  ,[Task].[BelongTo]
		  ,[User].[Name]			AS [UserName]
		  ,[UserTask].[Status]
		   ,[UserTask].[Id]         AS [UserTaskId]
		  ,ROW_NUMBER() OVER(ORDER BY [Task].[LastUpdateTime] DESC, [Task].[Id])	AS [Row]
		  ,COUNT([Task].[Id]) OVER()												AS [TotalCount]
	FROM [dbo].[UserTask]
	JOIN [dbo].[Task]
		ON [Task].[Id] = [UserTask].[TaskId]
	JOIN [dbo].[User]
		ON [User].[UserUniqueId] = [UserTask].[UserUniqueId]
	WHERE [Task].[IsActive] = 1
		AND [UserTask].[IsActive] = 1
		AND [Task].[TaskCategory] = 4
		AND [User].[IsActive] = 1
		AND [User].[Name] LIKE @userNamePattern
		AND [Task].[StartFrom] > CAST(ISNULL(@startFrom, N'1900-01-01') AS DATE)
		AND ([Task].[EndTo] IS NULL OR [Task].[EndTo] < CAST(ISNULL(@endTo, N'2100-01-01') AS DATE))
)
SELECT [StorePatrolWithRowNumber].[TaskId]
	  ,[StorePatrolWithRowNumber].[Title]
	  ,[StorePatrolWithRowNumber].[Description]
	  ,[StorePatrolWithRowNumber].[StartFrom]
	  ,[StorePatrolWithRowNumber].[EndTo]
	  ,[StorePatrolWithRowNumber].[Point]
	  ,[StorePatrolWithRowNumber].[OverduePoint]
	  ,[StorePatrolWithRowNumber].[BelongTo]
	  ,[StorePatrolWithRowNumber].[UserName]
	  ,[StorePatrolWithRowNumber].[Status]
	  ,[StorePatrolWithRowNumber].[TotalCount]
	  ,[StorePatrolWithRowNumber].[UserTaskId]
FROM [StorePatrolWithRowNumber]
WHERE [StorePatrolWithRowNumber].[Row] BETWEEN (@pageIndex - 1) * @pageSize + 1 AND @pageIndex * @pageSize