CREATE PROCEDURE [dbo].[spPointStorePatrol]
	@isPass				BIT,
	@userStorePatrol	XML
AS
BEGIN TRY

	BEGIN TRANSACTION
		
		CREATE TABLE #UserStorePatrolTempTable
		(
			[Id]			INT,
			[AdminRate]		FLOAT,
			[AdminComment]	NVARCHAR(50)
		)

		CREATE TABLE #UserTaskTempTable
		(
			[TaskId]			INT,
			[UserUniqueId]		NVARCHAR(200),
			[EndTo]				DATE,
			[Point]				INT,
			[OverduePoint]		INT
		)

		INSERT INTO #UserStorePatrolTempTable
		SELECT LTRIM(RTRIM(ISNULL(x.value('(child::UserStorePatrolId/text())[1]', 'INT'), 0)))				AS [UserStorePatrolId]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::AdminRate/text())[1]', 'FLOAT'), 0)))					AS [AdminRate]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::AdminComment/text())[1]', 'NVARCHAR(50)'), N'')))		AS [AdminComment]
		FROM @userStorePatrol.nodes('ArrayOfUserStorePatrolModel/UserStorePatrolModel') AS A(x)

		UPDATE [UserStorePatrol]
		SET [UserStorePatrol].[AdminRate] = #UserStorePatrolTempTable.[AdminRate]
		   ,[UserStorePatrol].[AdminComment] = #UserStorePatrolTempTable.[AdminComment]
		   ,[UserStorePatrol].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[UserStorePatrol]
		JOIN #UserStorePatrolTempTable
			ON #UserStorePatrolTempTable.[Id] = [UserStorePatrol].[Id]
		WHERE [UserStorePatrol].[IsActive] = 1


		INSERT INTO #UserTaskTempTable
		SELECT [Task].[Id]
			  ,[UserStorePatrol].[UserUniqueId]
			  ,CAST(ISNULL([Task].[EndTo], '2100-01-01') AS DATE) AS [EndTo]
			  ,[Task].[Point]
			  ,[Task].[OverduePoint]
		FROM [dbo].[UserStorePatrol]
		JOIN #UserStorePatrolTempTable
			ON #UserStorePatrolTempTable.[Id] = [UserStorePatrol].[Id]
		JOIN [dbo].[StorePatrol]
			ON [StorePatrol].[Id] = [UserStorePatrol].[StorePatrolId]
		JOIN [dbo].[Task]
			ON [Task].[Id] = [StorePatrol].[TaskId]
		JOIN [dbo].[User]
			ON [User].[UserUniqueId] = [UserStorePatrol].[UserUniqueId]
		WHERE [Task].[IsActive] = 1
			AND [StorePatrol].[IsActive] = 1
			AND [UserStorePatrol].[IsActive] = 1
			AND [User].[IsActive] = 1

		UPDATE [UserTask]
		SET [UserTask].[CompleteTime] = GETUTCDATE()
		   ,[UserTask].[Score] = 0
		   ,[UserTask].[Point] = (CASE WHEN @isPass = 0									THEN 0
									   WHEN #UserTaskTempTable.[EndTo] >= GETUTCDATE()	THEN #UserTaskTempTable.[Point]
									   ELSE #UserTaskTempTable.[OverduePoint] END)
		   ,[UserTask].[Status] = (CASE WHEN @isPass = 0 THEN 5
									    ELSE 4 END)
		FROM [dbo].[UserTask]
		JOIN #UserTaskTempTable
			ON #UserTaskTempTable.[TaskId] = [UserTask].[TaskId]
				AND #UserTaskTempTable.[UserUniqueId] = [UserTask].[UserUniqueId]
		WHERE [UserTask].[IsActive] = 1

		--TODO: INSERT INTO [dbo].[UserMoments]
		--TODO: INSERT INTO [dbo].[UserPointRecord]

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
		IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION

		DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT
			SELECT @ErrMsg = ERROR_MESSAGE(),
					@ErrSeverity = ERROR_SEVERITY()

		RAISERROR(@ErrMsg, @ErrSeverity, 1)
END CATCH