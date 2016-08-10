CREATE TABLE [dbo].[TaskQuestionnaire]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]			INT				NOT NULL CONSTRAINT [FK_TaskQuestionnaire_Task] FOREIGN KEY REFERENCES [Task]([Id]),
	[No]				INT				NOT NULL,
	[QuestionnaireId]	INT				NOT NULL CONSTRAINT [FK_TaskQuestionnaire_Questionnaire] FOREIGN KEY REFERENCES [Questionnaire]([Id]),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO