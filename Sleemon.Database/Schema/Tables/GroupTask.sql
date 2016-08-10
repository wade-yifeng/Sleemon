CREATE TABLE [dbo].[GroupTask]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[Title]					NVARCHAR(200)					NOT NULL,
	[Description]			NVARCHAR(2000)					NULL DEFAULT(''),
	[RequiredGrade]			TINYINT							NOT NULL,
	[Status]				TINYINT							NOT NULL DEFAULT(0),
	[OnOff]					BIT								NOT NULL,
	[LastUpdateUser]		NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]		DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT								NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-停用 1-启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupTask', @level2type=N'COLUMN',@level2name=N'Status'
GO