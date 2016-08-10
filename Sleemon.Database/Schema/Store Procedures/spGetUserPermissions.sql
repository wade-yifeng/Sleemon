CREATE PROCEDURE [dbo].[spGetUserPermissions]
	@userUniqueId	NVARCHAR(200)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [Permission].[Id]
	  ,[Permission].[ParentId]
	  ,[Permission].[Name]
	  ,[Permission].[Url]
	  ,[Permission].[IconClass]
	  ,[Permission].[IsMenu]
	  ,[Permission].[Description]
	  ,[Permission].[Sort]
	  ,[Permission].[IsActive]
	  ,[Permission].[LastUpdateTime]
	  ,[Permission].[LastUpdateUser]
FROM [dbo].[UserRole]
JOIN [dbo].[Role]
	ON [Role].[Id] = [UserRole].[RoleId]
		AND [Role].[IsActive] = 1
JOIN [dbo].[RolePermission]
	ON [RolePermission].[RoleId] = [UserRole].[RoleId]
		AND [RolePermission].[IsActive] = 1
JOIN [dbo].[Permission]
	ON [Permission].[Id] = [RolePermission].[PermissionId]
		AND [Permission].[IsActive] = 1
WHERE [UserRole].[IsActive] = 1
	AND [UserRole].[UserUniqueId] = @userUniqueId
GO