CREATE PROCEDURE [dbo].[spSaveTrainingDetail]
	@training	XML
AS
BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @traingId INT,
	    @subject NVARCHAR(200),
		@startFrom DATETIME,
		@avatar NVARCHAR(100),
		@status INT,
		@description NVARCHAR(2000),
		@location NVARCHAR(500),
		@endTo DATETIME,
		@maxParticipant INT,
		@joinDeadline DATETIME,
		@isPublic BIT,
		@isCheckInNeeded BIT,
		@checkInQRCode NVARCHAR(1000),
		@state INT,
		@lastUpdateUser NVARCHAR(200),
		@task XML,
		@groupKey UNIQUEIDENTIFIER,
		@lastUpdateDate DATETIME = GETUTCDATE();

		SELECT @traingId = x.value('(child::TrainingId/text())[1]', 'INT')
			  ,@subject = x.value('(child::Subject/text())[1]', 'NVARCHAR(200)')
			  ,@startFrom = (CASE WHEN x.value('(child::StartFrom/text())[1]', 'DATETIME2') < CAST( '1900-01-01' AS DATETIME) THEN GETUTCDATE()
							 ELSE x.value('(child::StartFrom/text())[1]', 'DATETIME') END)
			  ,@avatar = x.value('(child::Avatar/text())[1]', 'NVARCHAR(100)')
			  ,@status = x.value('(child::Status/text())[1]', 'INT')
			  ,@description = x.value('(child::Description/text())[1]', 'NVARCHAR(2000)')
			  ,@location = x.value('(child::Location/text())[1]', 'NVARCHAR(500)')
			  ,@endTo = (CASE WHEN x.value('(child::EndTo/text())[1]', 'DATETIME2') < CAST( '1900-01-01' AS DATETIME) THEN NULL
						 ELSE x.value('(child::EndTo/text())[1]', 'DATETIME') END)
			  ,@maxParticipant = x.value('(child::MaxParticipant/text())[1]', 'INT')
			  ,@joinDeadline = (CASE WHEN x.value('(child::JoinDeadline/text())[1]', 'DATETIME2') < CAST( '1900-01-01' AS DATETIME) THEN GETUTCDATE()
								ELSE x.value('(child::JoinDeadline/text())[1]', 'DATETIME') END )
			  ,@isPublic = x.value('(child::IsPublic/text())[1]', 'BIT')
			  ,@isCheckInNeeded = x.value('(child::IsCheckInNeeded/text())[1]', 'BIT')
			  ,@checkInQRCode = x.value('(child::CheckInQRCode/text())[1]', 'NVARCHAR(1000)')
			  ,@state = x.value('(child::Status/text())[1]', 'INT')
			  ,@lastUpdateUser = x.value('(child::LastUpdateUser/text())[1]', 'NVARCHAR(200)')
			  ,@task = x.query('child::Tasks')
		FROM @training.nodes('/TrainingDetailModel') AS A(x);

		IF EXISTS(SELECT * FROM [dbo].[Training] WHERE [Training].[IsActive] = 1 AND [Training].[Id] = @traingId)
		BEGIN
			SET @groupKey = (SELECT TOP(1) [Training].[GroupKey] FROM [dbo].[Training] WHERE [Training].[IsActive] = 1 AND [Training].[Id] = @traingId)

			UPDATE [Training]
			SET [Training].[IsActive] = 0
				,[Training].[LastUpdateTime] = @lastUpdateDate
				,[Training].[LastUpdateUser] = @lastUpdateUser
			FROM [dbo].[Training]
			WHERE [Training].[IsActive] = 1 
				AND [Training].[Id] = @traingId
		END
		ELSE
		BEGIN
			SET @groupKey = NEWID()
		END

		INSERT INTO [dbo].[Training]([Subject]
									,[Description]
									,[Avatar]
									,[Location]
									,[StartFrom]
									,[EndTo]
									,[MaxParticipant]
									,[JoinDeadline]
									,[IsPublic]
									,[IsCheckInNeeded]
									,[CheckInQRCode]
									,[GroupKey]
									,[Status]
									,[LastUpdateUser]
									,[LastUpdateTime]
									,[IsActive])
		SELECT @subject
				,@description
				,@avatar
				,@location
				,@startFrom
				,@endTo
				,@maxParticipant
				,@joinDeadline
				,@isPublic
				,@isCheckInNeeded
				,@checkInQRCode
				,@groupKey
				,@state
				,@lastUpdateUser
				,@lastUpdateDate
				,1

		IF @state = 2
		BEGIN
			DECLARE @taskTempTable TABLE
			(
				[Title]				NVARCHAR(200),
				[Description]		NVARCHAR(2000),
				[TaskCategory]		TINYINT,
				[StartFrom]			DATE,
				[EndTo]				DATE,
				[Point]				INT,
				[OverduePoint]		INT,
				[ProductAbility]	INT,
				[SalesAbility]		INT,
				[ExhibitAbility]	INT,
				[BelongTo]			INT,
				[Status]			TINYINT,
				[Exams]				XML,
				[Questionnaires]	XML,
				[LearningFiles]		XML
			)

			DECLARE @deletedTaskId TABLE
			(
				Id		INT
			)

			DECLARE @insertedTaskId TABLE
			(
				Id		INT
			)

			UPDATE [Task]
			SET [Task].[IsActive] = 0, [Task].[LastUpdateTime] = @lastUpdateDate, [Task].[LastUpdateUser] = @lastUpdateUser
			OUTPUT DELETED.[Id]
			INTO @deletedTaskId
			FROM [dbo].[Task]
			JOIN [dbo].[TrainingTask]
				ON [TrainingTask].[TaskId] = [Task].[Id]
			WHERE [Task].[IsActive] = 1
				AND [TrainingTask].[IsActive] = 1
				AND [TrainingTask].[TrainingId] = @traingId

			UPDATE [TaskExam]
			SET [TaskExam].[IsActive] = 0, [TaskExam].[LastUpdateTime] = @lastUpdateDate, [TaskExam].[LastUpdateUser] = @lastUpdateUser
			FROM [dbo].[TaskExam]
			JOIN @deletedTaskId AS [DeletedTask]
				ON [DeletedTask].[Id] = [TaskExam].[TaskId]
			AND [TaskExam].[IsActive] = 1

			UPDATE [TaskLearning]
			SET [TaskLearning].[IsActive] = 0, [TaskLearning].[LastUpdateTime] = @lastUpdateDate, [TaskLearning].[LastUpdateUser] = @lastUpdateUser
			FROM [dbo].[TaskLearning]
			JOIN @deletedTaskId AS [DeletedTask]
				ON [DeletedTask].[Id] = [TaskLearning].[TaskId]
			AND [TaskLearning].[IsActive] = 1

			UPDATE [TaskQuestionnaire]
			SET [TaskQuestionnaire].[IsActive] = 0, [TaskQuestionnaire].[LastUpdateTime] = @lastUpdateDate, [TaskQuestionnaire].[LastUpdateUser] = @lastUpdateUser
			FROM [dbo].[TaskQuestionnaire]
			JOIN @deletedTaskId AS [DeletedTask]
				ON [DeletedTask].[Id] = [TaskQuestionnaire].[TaskId]
			AND [TaskQuestionnaire].[IsActive] = 1

			--TODO: 签到Task

			INSERT INTO @taskTempTable
			SELECT x.value('(child::Title/text())[1]', 'NVARCHAR(200)')				AS [Title]
				  ,x.value('(child::Description/text())[1]', 'NVARCHAR(2000)')		AS [Description]
				  ,x.value('(child::TaskCategory/text())[1]', 'TINYINT')			AS [TaskCategory]
				  ,x.value('(child::StartFrom/text())[1]', 'DATE')					AS [StartFrom]
				  ,x.value('(child::EndTo/text())[1]', 'DATE')						AS [EndTo]
				  ,x.value('(child::Point/text())[1]', 'INT')						AS [Point]
				  ,x.value('(child::OverduePoint/text())[1]', 'INT')				AS [OverduePoint]
				  ,x.value('(child::ProductAbility/text())[1]', 'INT')				AS [ProductAbility]
				  ,x.value('(child::SalesAbility/text())[1]', 'INT')				AS [SalesAbility]
				  ,x.value('(child::ExhibitAbility/text())[1]', 'INT')				AS [ExhibitAbility]
				  ,x.value('(child::BelongTo/text())[1]', 'INT')					AS [BelongTo]
				  ,x.value('(child::TaskStatus/text())[1]', 'TINYINT')				AS [Status]
				  ,x.query('child::Exams')											AS [Exams]
				  ,x.query('child::Questionnaires')									AS [Questionnaires]
				  ,x.query('child::LearningFiles')									AS [LearningFiles]
			FROM @task.nodes('/Tasks/TaskDetailsModel') AS A(x);

			DECLARE @taskId					INT,
					@title					NVARCHAR(200),
					@taskDescription		NVARCHAR(2000),
					@taskCategory			TINYINT,
					@taskStartFrom			DATE,
					@taskEndTo				DATE,
					@point					INT,
					@overduePoint			INT,
					@productAbility			INT,
					@salesAbility			INT,
					@exhibitAbility			INT,
					@belongTo				INT,
					@taskStatus				TINYINT,
					@exams					XML,
					@questionnaires			XML,
					@learningFiles			XML,
					@index					INT = 0,
					@examNo					INT = 0,
					@questionnairesNo		INT	= 0,
					@learningFilesNo		INT = 0

			DECLARE task_cursor CURSOR FOR
			SELECT [TaskTempTable].[Title]
				  ,[TaskTempTable].[Description]
				  ,[TaskTempTable].[TaskCategory]
				  ,[TaskTempTable].[StartFrom]
				  ,[TaskTempTable].[EndTo]
				  ,[TaskTempTable].[Point]
				  ,[TaskTempTable].[OverduePoint]
				  ,[TaskTempTable].[ProductAbility]
				  ,[TaskTempTable].[SalesAbility]
				  ,[TaskTempTable].[ExhibitAbility]
				  ,[TaskTempTable].[BelongTo]
				  ,[TaskTempTable].[Status]
				  ,[TaskTempTable].[Exams]
				  ,[TaskTempTable].[Questionnaires]
				  ,[TaskTempTable].[LearningFiles]
			FROM @taskTempTable AS [TaskTempTable]

			OPEN task_cursor

			FETCH NEXT FROM task_cursor
			INTO @title
				,@taskDescription
				,@taskCategory
				,@taskStartFrom
				,@taskEndTo
				,@point
				,@overduePoint
				,@productAbility
				,@salesAbility
				,@exhibitAbility
				,@belongTo
				,@taskStatus
				,@exams
				,@questionnaires
				,@learningFiles

			WHILE @@FETCH_STATUS = 0
			BEGIN
		
				DELETE @insertedTaskId

				INSERT INTO [dbo].[Task]([Title]
										,[Description]
										,[TaskCategory]
										,[StartFrom]
										,[EndTo]
										,[Point]
										,[OverduePoint]
										,[ProductAbility]
										,[SalesAbility]
										,[ExhibitAbility]
										,[BelongTo]
										,[Status]
										,[LastUpdateUser]
										,[LastUpdateTime]
										,[IsActive])
				OUTPUT INSERTED.[Id] INTO @insertedTaskId
				SELECT @title
					  ,@taskDescription
					  ,@taskCategory
					  ,@taskStartFrom
					  ,@taskEndTo
					  ,@point
					  ,@overduePoint
					  ,@productAbility
					  ,@salesAbility
					  ,@exhibitAbility
					  ,@belongTo
					  ,@taskStatus
					  ,@lastUpdateUser
					  ,@lastUpdateDate
					  ,1

				IF @exams IS NOT NULL
				BEGIN
					INSERT INTO [dbo].[TaskExam]([TaskId]
												,[No]
												,[ExamId]
												,[LearningFileId]
												,[LastUpdateUser]
												,[LastUpdateTime]
												,[IsActive])
					SELECT [InsertedTask].[Id]						AS [TaskId]
						  ,@examNo									AS [No]
						  ,x.value('(child::Id/text())[1]', 'INT')	AS [ExamId]
						  ,NULL										AS [LearningFileId]
						  ,@lastUpdateUser							AS [LastUpdateUser]
						  ,@lastUpdateDate							AS [LastUpdateTime]
						  ,1										AS [IsActive]
					FROM @exams.nodes('/Exams/ExamDetailModel') AS A(x)
					CROSS JOIN @insertedTaskId AS [InsertedTask]

					SET @examNo = @examNo + 1
				END

				IF @questionnaires IS NOT NULL
				BEGIN
					INSERT INTO [dbo].[TaskQuestionnaire]([TaskId]
														 ,[No]
														 ,[QuestionnaireId]
														 ,[LastUpdateUser]
														 ,[LastUpdateTime]
														 ,[IsActive])
					SELECT [InsertedTask].[Id]						AS [TaskId]
						  ,@questionnairesNo						AS [No]
						  ,x.value('(child::Id/text())[1]', 'INT')	AS [QuestionnaireId]
						  ,@lastUpdateUser							AS [LastUpdateUser]
						  ,@lastUpdateDate							AS [LastUpdateTime]
						  ,1										AS [IsActive]
					FROM @questionnaires.nodes('/Questionnaires/QuestionnaireDetailModel') AS A(x)
					CROSS JOIN @insertedTaskId AS [InsertedTask]

					SET @questionnairesNo = @questionnairesNo + 1
				END

				IF @learningFiles IS NOT NULL
				BEGIN
					INSERT INTO [dbo].[TaskLearning]([TaskId]
													,[No]
													,[LearningFileId]
													,[LastUpdateUser]
													,[LastUpdateTime]
													,[IsActive])
					SELECT [InsertedTask].[Id]						AS [TaskId]
						  ,@learningFilesNo							AS [No]
						  ,x.value('(child::Id/text())[1]', 'INT')	AS [LearningFileId]
						  ,@lastUpdateUser							AS [LastUpdateUser]
						  ,@lastUpdateDate							AS [LastUpdateTime]
						  ,1										AS [IsActive]
					FROM @learningFiles.nodes('/LearningFiles/LearningFileDetailModel') AS A(x)
					CROSS JOIN @insertedTaskId AS [InsertedTask]

					SET @learningFilesNo = @learningFilesNo + 1
				END

				INSERT INTO [dbo].[TrainingTask]([TrainingId]
												,[No]
												,[TaskId]
												,[LastUpdateUser]
												,[LastUpdateTime]
												,[IsActive])
				SELECT @traingId				AS [TrainingId]
					  ,@index					AS [No]
					  ,[InsertedTask].[Id]		AS [TaskId]
					  ,@lastUpdateUser			AS [LastUpdateUser]
					  ,@lastUpdateDate			AS [LastUpdateTime]
					  ,1						AS [IsActive]
				FROM @insertedTaskId AS [InsertedTask]
		
				FETCH NEXT FROM task_cursor
				INTO @title
					,@taskDescription
					,@taskCategory
					,@taskStartFrom
					,@taskEndTo
					,@point
					,@overduePoint
					,@productAbility
					,@salesAbility
					,@exhibitAbility
					,@belongTo
					,@taskStatus
					,@exams
					,@questionnaires
					,@learningFiles


				SET @index = @index +1
			END
			CLOSE task_cursor
			DEALLOCATE task_cursor
		END
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
