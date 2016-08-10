CREATE PROCEDURE [dbo].[spGetBroadcastMessage]
	@maxCount		INT
AS
DECLARE @TargetMessage TABLE
(
	[Id]				INT,
	[MessageType]		TINYINT,
	[ToDepts]			NVARCHAR(MAX),
	[ToUsers]			NVARCHAR(MAX),
	[LinkedId]			INT,
	[Subject]			NVARCHAR(250),
	[Summary]			NVARCHAR(MAX),
	[AvatarPath]		NVARCHAR(MAX)
);

WITH [Messages] AS
(
	SELECT [MessageDispatch].[Id]
		  ,[MessageDispatch].[MessageType]
		  ,[MessageDispatch].[ToDepts]
		  ,[MessageDispatch].[ToUsers]
		  ,[MessageDispatch].[LinkedId]
		  ,[EnterpriseNotice].[Subject]
		  ,[EnterpriseNotice].[Summary]
		  ,[EnterpriseNotice].[AvatarPath]
		  ,[EnterpriseNotice].[LastUpdateTime]
		  ,[MessageDispatch].[Status]
	FROM [dbo].[MessageDispatch]
	JOIN [dbo].[EnterpriseNotice]
		ON [MessageDispatch].[LinkedId] = [EnterpriseNotice].[Id]
		AND [MessageDispatch].[MessageType] = 1
	WHERE ([MessageDispatch].[Status] = 1  OR [MessageDispatch].[Status] = 4)
		AND [EnterpriseNotice].[IsActive] = 1
		AND [MessageDispatch].[IsActive] = 1

	UNION ALL

	SELECT [MessageDispatch].[Id]
		  ,[MessageDispatch].[MessageType]
		  ,[MessageDispatch].[ToDepts]
		  ,[MessageDispatch].[ToUsers]
		  ,[MessageDispatch].[LinkedId]
		  ,[Task].[Title]					AS [Subject]
		  ,[Task].[Description]				AS [Summary]
		  ,NULL								AS [AvatarPath]
		  ,[Task].[LastUpdateTime]
		  ,[MessageDispatch].[Status]
	FROM [dbo].[MessageDispatch]
	JOIN [dbo].[Task]
		ON [MessageDispatch].[LinkedId] = [Task].[Id]
		AND [MessageDispatch].[MessageType] = 2
	WHERE ([MessageDispatch].[Status] = 1  OR [MessageDispatch].[Status] = 4)
		AND [Task].[IsActive] = 1
		AND [MessageDispatch].[IsActive] = 1

	UNION ALL

	SELECT [MessageDispatch].[Id]
		  ,[MessageDispatch].[MessageType]
		  ,[MessageDispatch].[ToDepts]
		  ,[MessageDispatch].[ToUsers]
		  ,[MessageDispatch].[LinkedId]
		  ,[Training].[Subject]
		  ,[Training].[Description]				AS [Summary]
		  ,[Training].[Avatar]					AS [AvatarPath]
		  ,[Training].[LastUpdateTime]
		  ,[MessageDispatch].[Status]
	FROM [dbo].[MessageDispatch]
	JOIN [dbo].[Training]
		ON [Training].[Id] = [MessageDispatch].[LinkedId]
			AND [MessageDispatch].[MessageType] = 3
	WHERE ([MessageDispatch].[Status] = 1  OR [MessageDispatch].[Status] = 4)
		AND [Training].[IsActive] = 1
		AND [MessageDispatch].[IsActive] = 1
),
[TargetMessages] AS
(
	SELECT [Messages].[Id]
		  ,[Messages].[MessageType]
		  ,[Messages].[ToDepts]
		  ,[Messages].[ToUsers]
		  ,[Messages].[LinkedId]
		  ,[Messages].[Subject]
		  ,[Messages].[Summary]
		  ,[Messages].[AvatarPath]
		  ,ROW_NUMBER() OVER(ORDER BY [Messages].[Status], [Messages].[LastUpdateTime], [Messages].[Id]) AS [RowNumber]
	FROM [Messages]
)
INSERT INTO @TargetMessage
SELECT [TargetMessages].[Id]
	  ,[TargetMessages].[MessageType]
	  ,[TargetMessages].[ToDepts]
	  ,[TargetMessages].[ToUsers]
	  ,[TargetMessages].[LinkedId]
	  ,[TargetMessages].[Subject]
	  ,[TargetMessages].[Summary]
	  ,[TargetMessages].[AvatarPath]
FROM [TargetMessages]
WHERE [RowNumber] BETWEEN 1 AND @maxCount;

UPDATE  [MessageDispatch]
SET [MessageDispatch].[Status] = 3
FROM [dbo].[MessageDispatch]
JOIN @TargetMessage AS [Message]
	ON [Message].[Id] = [MessageDispatch].[Id];

SELECT [Message].[Id]
	  ,[Message].[MessageType]
	  ,[Message].[ToDepts]
	  ,[Message].[ToUsers]
	  ,[Message].[LinkedId]
	  ,[Message].[Subject]
	  ,[Message].[Summary]
	  ,[Message].[AvatarPath]
FROM @TargetMessage AS [Message]
