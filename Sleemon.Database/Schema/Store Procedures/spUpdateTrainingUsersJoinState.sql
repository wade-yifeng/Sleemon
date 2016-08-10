CREATE PROCEDURE [dbo].[spUpdateTrainingUsersJoinState]
	@trainingId					INT,
	@userJoinStatusEntities		XML,
	@lastUpdateUser				NVARCHAR(200)
AS
BEGIN TRY
	BEGIN TRANSACTION
		CREATE TABLE #UserJoinStatusTempTable
		(
			UserUniqueId		NVARCHAR(200),
			JoinStatus			INT
		)

		INSERT INTO #UserJoinStatusTempTable
		SELECT [userJoinStatusEntity].value('(@UserUniqueId)[1]', 'NVARCHAR(200)')				AS [UserUniqueId]
			  ,[userJoinStatusEntity].value('(@JoinStatus)[1]', 'INT')							AS [JoinStatus]
		FROM @userJoinStatusEntities.nodes('/UserJoinStatusEntities/UserJoinStatusEntity') AS UserJoinStatusEntity([userJoinStatusEntity])

		UPDATE [UserTraining]
		SET [UserTraining].[JoinStatus] = #UserJoinStatusTempTable.[JoinStatus]
		FROM [dbo].[UserTraining]
		JOIN #UserJoinStatusTempTable
			ON [UserTraining].[UserUniqueId] = #UserJoinStatusTempTable.[UserUniqueId]
		WHERE [UserTraining].[IsActive] = 1
			AND [UserTraining].[TrainingId] = @trainingId;

		WITH [TaskId] AS
		(
			SELECT [Task].[Id] AS [TaskId]
			FROM [dbo].[TrainingTask]
			JOIN [dbo].[Task]
				ON [Task].[Id] = [TrainingTask].[TaskId]
			WHERE [Task].[IsActive] = 1
				AND [TrainingTask].[IsActive] = 1
				AND [TrainingTask].[TrainingId] = @trainingId
		)
		INSERT INTO [dbo].[UserTask]([UserUniqueId], [TaskId], [Status], [AssignTime], [IsActive])
		SELECT #UserJoinStatusTempTable.[UserUniqueId], [TaskId].[TaskId], 1, GETUTCDATE(), 1
		FROM [TaskId]
		CROSS JOIN  #UserJoinStatusTempTable
		WHERE #UserJoinStatusTempTable.[JoinStatus] = 2;

		WITH [GroupJoinStatus] AS
		(
			SELECT #UserJoinStatusTempTable.[JoinStatus]
			FROM #UserJoinStatusTempTable
			GROUP BY #UserJoinStatusTempTable.[JoinStatus]
		),
		[GroupUserJoinStatus] AS
		(
			SELECT [GroupJoinStatus].[JoinStatus]
				  ,COALESCE(STUFF((
					SELECT '|' + CAST(#UserJoinStatusTempTable.[UserUniqueId] AS NVARCHAR(MAX)) AS [text()] 
					FROM #UserJoinStatusTempTable
					WHERE #UserJoinStatusTempTable.[JoinStatus] = [GroupJoinStatus].[JoinStatus]
					FOR XML PATH(''), TYPE).value('.' ,'NVARCHAR(MAX)'), 1, 1, N''), N'') AS [ToUsers]
			FROM [GroupJoinStatus]
		)
		INSERT INTO [dbo].[MessageDispatch]([Subject]
										   ,[ToUsers]
										   ,[ToDepts]
										   ,[Priority]
										   ,[MessageType]
										   ,[LinkedId]
										   ,[DispatchType]
										   ,[DispatchTime]
										   ,[Status]
										   ,[LastUpdateUser]
										   ,[LastUpdateTime]
										   ,[IsActive])
		SELECT CASE WHEN [GroupUserJoinStatus].[JoinStatus] = 2 THEN N'通过报名' --TODO: Suject
					ELSE N'未通过报名' END				AS [Subject]
			  ,[GroupUserJoinStatus].[ToUsers]
			  ,NULL										AS [ToDepts]
			  ,1										AS [Priority]
			  ,3										AS [MessageType]
			  ,@trainingId								AS [LinkedId]
			  ,1										AS [DispatchType]
			  ,NULL										AS [DispatchTime]
			  ,1										AS [Status]
			  ,@lastUpdateUser							AS [LastUpdateUser]
			  ,GETUTCDATE()								AS [LastUpdateTime]
			  ,1										AS [IsActive]
		FROM [GroupUserJoinStatus]

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