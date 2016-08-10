CREATE PROCEDURE [dbo].[spSyncUser]
	@users	XML
AS
BEGIN TRY
	BEGIN TRANSACTION
		CREATE TABLE #UserTempTable
		(
			[UserId]			NVARCHAR(200)	NOT NULL,
			[Name]				NVARCHAR(50)	NOT NULL,
			[Password]			NVARCHAR(50)	NOT NULL,
			[Position]			NVARCHAR(100)	NOT NULL,
			[Mobile]			NVARCHAR(16)	NOT NULL,
			[Gender]			BIT				NOT NULL,
			[Email]				NVARCHAR(50)	NOT NULL,
			[WeixinId]			NVARCHAR(256)	NOT NULL,
			[Avatar]			NVARCHAR(MAX)	NOT NULL,
			[Status]			INT				NOT NULL
		)

		INSERT INTO #UserTempTable
		SELECT LTRIM(RTRIM(ISNULL(x.value('(child::UserId/text())[1]', 'NVARCHAR(200)'), N'')))				AS [UserId]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Name/text())[1]', 'NVARCHAR(50)'), N'')))				AS [Name]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Password/text())[1]', 'NVARCHAR(50)'), N'')))			AS [Password]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Position/text())[1]', 'NVARCHAR(100)'), N'')))			AS [Position]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Mobile/text())[1]', 'NVARCHAR(16)'), N'')))				AS [Mobile]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Gender/text())[1]', 'BIT'), 0)))							AS [Gender]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Email/text())[1]', 'NVARCHAR(50)'), N'')))				AS [Email]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::WeixinId/text())[1]', 'NVARCHAR(256)'), N'')))			AS [WeixinId]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Avatar/text())[1]', 'NVARCHAR(MAX)'), N'')))				AS [Avatar]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Status/text())[1]', 'INT'), 0)))							AS [Status]
		FROM @users.nodes('ArrayOfUserProfile/UserProfile') AS A(x)

		MERGE INTO [dbo].[User]
		USING #UserTempTable
			ON #UserTempTable.[UserId] = [User].[UserUniqueId] AND [User].[IsActive] = 1
		WHEN MATCHED
		THEN UPDATE SET [User].[Name] = #UserTempTable.[Name]
                       ,[User].[Position] = #UserTempTable.[Position]
                       ,[User].[Mobile] = #UserTempTable.[Mobile]
                       ,[User].[Gender] = #UserTempTable.[Gender]
                       ,[User].[Email] = #UserTempTable.[Email]
                       ,[User].[Avatar] = #UserTempTable.[Avatar]
                       ,[User].[WeixinId] = #UserTempTable.[WeixinId]
                       ,[User].[Status] = #UserTempTable.[Status]
                       ,[User].[LastUpdateTime] = GETUTCDATE()
		WHEN NOT MATCHED
		THEN INSERT([UserUniqueId]
				   ,[Name]
				   ,[WeixinId]
				   ,[Password]
				   ,[Gender]
				   ,[Position]
				   ,[Mobile]
				   ,[Email]
				   ,[Avatar]
				   ,[Status]
				   ,[Country]
				   ,[Province]
				   ,[City]
				   ,[District]
				   ,[EntryDate]
                   ,[Grade]
				   ,[Point]
                   ,[ProductAbility]
                   ,[SalesAbility]
                   ,[ExhibitAbility]
                   ,[IsAdmin]
				   ,[IsSuperAdmin]
                   ,[LastUpdateTime]
                   ,[IsActive])
		VALUES(#UserTempTable.[UserId]
			  ,#UserTempTable.[Name]
			  ,#UserTempTable.[WeixinId]
			  ,#UserTempTable.[Password]
			  ,#UserTempTable.[Gender]
			  ,#UserTempTable.[Position]
			  ,#UserTempTable.[Mobile]
			  ,#UserTempTable.[Email]
			  ,#UserTempTable.[Avatar]
			  ,#UserTempTable.[Status]
			  ,N'' --Country
			  ,N'' --Province
			  ,N'' --City
			  ,N'' --District
			  ,NULL --EntryDate
              ,0 --Grade
              ,0 --Point
              ,0 --ProductAbility
              ,0 --SalesAblility
              ,0 --ExhibitAblility
              ,0 --IsAdMin
              ,0 --IsSuperAdmin
              ,GETUTCDATE() --LastUpdateTime
              ,1) --IsActive
		WHEN NOT MATCHED BY SOURCE
		THEN UPDATE SET [User].[IsActive] = 0;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
END CATCH
