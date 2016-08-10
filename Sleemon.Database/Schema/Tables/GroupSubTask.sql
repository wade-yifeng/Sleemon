CREATE TABLE [dbo].[GroupSubTask]
(
	[Id]						INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[GroupTaskId]				INT								NOT NULL CONSTRAINT [FK_GroupSubTask_GroupTask] FOREIGN KEY REFERENCES [GroupTask]([Id]),
	[No]						TINYINT							NOT NULL,
	[TaskId]					INT								NOT NULL CONSTRAINT [FK_GroupSubTask_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[OffsetDays]				INT								NOT NULL,
	[LastUpdateUser]			NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]			DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]					BIT								NOT NULL DEFAULT(1)
)
GO