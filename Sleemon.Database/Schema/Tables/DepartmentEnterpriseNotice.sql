CREATE TABLE [dbo].[DepartmentEnterpriseNotice]
(
	[Id]						INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	[EnterpriseNoticeId]		INT					NOT NULL CONSTRAINT [FK_DepartmentEnterpriseNotice_EnterpriseNoticeId_EnterpriseNotice] FOREIGN KEY REFERENCES [EnterpriseNotice]([Id]),
	[DepartmentHierarchyPath]	NVARCHAR(MAX)		NOT NULL
)
GO