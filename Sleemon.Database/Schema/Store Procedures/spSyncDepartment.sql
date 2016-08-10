CREATE PROCEDURE [dbo].[spSyncDepartment]
	@department		XML
AS
BEGIN TRY
	BEGIN TRANSACTION
		CREATE TABLE #departmentTempTable
		(
	        [DepartmentUniqueId]	INT					NOT NULL,
	        [Name]					NVARCHAR(500)		NOT NULL,
	        [ParentDepartmentId]	INT					NOT NULL,
	        [Order]					INT					NOT NULL,
			[ParentHierarchyPath]	NVARCHAR(MAX)		NULL
		)

		INSERT INTO #departmentTempTable
		SELECT LTRIM(RTRIM(ISNULL(x.value('(child::Id/text())[1]', 'INT'), 0)))				                AS [DepartmentUniqueId]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::Name/text())[1]', 'NVARCHAR(500)'), N'')))				AS [Name]
			  ,LTRIM(RTRIM(ISNULL(x.value('(child::ParentId/text())[1]', 'INT'), 0)))                       AS [ParentDepartmentId]
              ,LTRIM(RTRIM(ISNULL(x.value('(child::Order/text())[1]', 'INT'), 0)))                          AS [Order]
			  ,LTRIM(RTRIM(x.value('(child::ParentHierarchyPath/text())[1]', 'NVARCHAR(MAX)')))				AS [ParentHierarchyPath]
		FROM @department.nodes('ArrayOfDepartmentSyncModel/DepartmentSyncModel') AS A(x)

		MERGE INTO [dbo].[Department]
		USING #departmentTempTable
			ON #departmentTempTable.[DepartmentUniqueId] = [Department].[UniqueId] AND [Department].[IsActive] = 1
		WHEN MATCHED
		THEN UPDATE SET [Department].[Name] = #departmentTempTable.[Name]
					   ,[Department].[ParentId] = #departmentTempTable.[ParentDepartmentId]
					   ,[Department].[Order] = #departmentTempTable.[Order]
					   ,[Department].[ParentHierarchyId] = CONVERT(HIERARCHYID, #departmentTempTable.[ParentHierarchyPath])
		WHEN NOT MATCHED
		THEN INSERT([UniqueId]
				   ,[Name]
				   ,[ParentId]
				   ,[ParentHierarchyId]
                   ,[Order]
                   ,[LastUpdateTime]
                   ,[IsActive])
		VALUES(#departmentTempTable.[DepartmentUniqueId]
			  ,#departmentTempTable.[Name]
			  ,#departmentTempTable.[ParentDepartmentId]
			  ,CONVERT(HIERARCHYID, #departmentTempTable.[ParentHierarchyPath])
			  ,#departmentTempTable.[Order]
			  ,GETUTCDATE()
			  ,1)
		WHEN NOT MATCHED BY SOURCE
		THEN UPDATE SET [Department].[IsActive] = 0;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
END CATCH
