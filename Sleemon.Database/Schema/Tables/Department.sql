CREATE TABLE [dbo].[Department]
(
	[Id]					INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	[UniqueId]				INT					NOT NULL,
	[HierarchyId]			AS CONVERT(HIERARCHYID, [ParentHierarchyId].ToString() + CONVERT(NVARCHAR(20), [UniqueId]) + '/') PERSISTED,
	[ParentHierarchyId]		HIERARCHYID			NULL,
	[Level]					AS [ParentHierarchyId].GetLevel(),
	[Name]					NVARCHAR(500)		NOT NULL DEFAULT(''),
	[ParentId]				INT					NOT NULL	/*CONSTRAINT [FK_Department_ParentDepartMent] FOREIGN KEY REFERENCES [Department]([Id])*/,
	[Order]					INT					NOT NULL,
	[LastUpdateTime]		DATETIME			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT					NOT NULL DEFAULT(1)
)
GO