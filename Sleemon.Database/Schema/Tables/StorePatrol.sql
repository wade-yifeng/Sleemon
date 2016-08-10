CREATE TABLE [dbo].[StorePatrol]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]			INT				NOT NULL CONSTRAINT [FK_StorePatrol_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[PatrolCategory]	INT				NOT NULL,
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-门头 2-陈列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorePatrol', @level2type=N'COLUMN',@level2name=N'PatrolCategory'
GO