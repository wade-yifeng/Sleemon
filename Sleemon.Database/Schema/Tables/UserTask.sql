CREATE TABLE [dbo].[UserTask]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL,
	[TaskId]				INT				NOT NULL CONSTRAINT [FK_UserTask_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[CompleteTime]			DATETIME		NULL,
	[Score]					FLOAT			NULL,
	[Point]					INT				NULL,
	[Status]				TINYINT			NOT NULL,
	[AssignTime]			DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态：1-未开始 2-进行中 3-已提交待审核 4-已结束 5-未合格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserTask', @level2type=N'COLUMN',@level2name=N'Status'
GO