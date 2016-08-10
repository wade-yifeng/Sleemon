CREATE TABLE [dbo].[TrainingTask]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TrainingId]		INT				NOT NULL CONSTRAINT [FK_TrainingTask_Training] FOREIGN KEY REFERENCES [Training]([Id]),
	[No]				TINYINT			NOT NULL,
	[TaskId]			INT				NOT NULL CONSTRAINT [FK_TrainingTask_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO