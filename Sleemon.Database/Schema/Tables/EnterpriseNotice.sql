CREATE TABLE [dbo].[EnterpriseNotice]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[Subject]				NVARCHAR(250)					NOT NULL,
	[AvatarPath]			NVARCHAR(MAX)					NULL,
	[Summary]				NVARCHAR(100)					NOT NULL DEFAULT(''),
	[Context]				NVARCHAR(MAX)					NOT NULL DEFAULT(''),
	[Category]				TINYINT							NOT NULL DEFAULT(1),
	[NoticeType]			TINYINT							NOT NULL, --1. 咨询。2. 活动
	[LastUpdateUser]		NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]		DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT								NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息状态： 1-Normal 2-Top 3-Slide' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EnterpriseNotice', @level2type=N'COLUMN',@level2name=N'Category'
GO