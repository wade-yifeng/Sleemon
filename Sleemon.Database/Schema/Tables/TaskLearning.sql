CREATE TABLE [dbo].[TaskLearning]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]			INT				NOT NULL CONSTRAINT [FK_TaskLearning_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[No]				INT				NOT NULL,
	[LearningFileId]	INT				NOT NULL CONSTRAINT [FK_TaskLearning_LearningFile] FOREIGN KEY REFERENCES [LearningFile]([Id]),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO