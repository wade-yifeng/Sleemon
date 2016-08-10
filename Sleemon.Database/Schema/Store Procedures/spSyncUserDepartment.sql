CREATE PROCEDURE [dbo].[spSyncUserDepartment]
	@userDepartment	XML
AS
BEGIN TRY
	BEGIN TRANSACTION
		CREATE TABLE #UserDepartmentTempTable
		(
			[UserUniqueId]			NVARCHAR(200)	NOT NULL,
			[DepartmentUniqueId]	INT	NOT NULL
		)

		INSERT INTO #UserDepartmentTempTable
		SELECT LTRIM(RTRIM(ISNULL(x.value('(child::UserId/text())[1]', 'NVARCHAR(200)'), N'')))			AS [UserUniqueId]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::DepartmentId/text())[1]', 'INT'), 0)))				AS [DepartmentUniqueId]
		FROM @userDepartment.nodes('ArrayOfUserDepartmentSyncModel/UserDepartmentSyncModel') AS A(x)

		DELETE FROM [dbo].[UserDepartment];
		INSERT INTO [dbo].[UserDepartment]([UserUniqueId], [DepartmentUniqueId])
		SELECT #UserDepartmentTempTable.[UserUniqueId], #UserDepartmentTempTable.[DepartmentUniqueId]
		FROM #UserDepartmentTempTable
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
END CATCH
