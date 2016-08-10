CREATE TABLE [dbo].[TaskExam]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]			INT				NOT NULL CONSTRAINT [FK_TaskExam_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[No]				INT				NOT NULL,
	[ExamId]			INT				NOT NULL CONSTRAINT [FK_TaskExam_Exam] FOREIGN KEY REFERENCES [Exam]([Id]),
	[LearningFileId]	INT				NULL CONSTRAINT [FK_TaskExam_LearningFile] FOREIGN KEY REFERENCES [LearningFile]([Id]),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO