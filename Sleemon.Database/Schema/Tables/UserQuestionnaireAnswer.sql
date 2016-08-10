CREATE TABLE [dbo].[UserQuestionnaireAnswer]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]				INT				NOT NULL CONSTRAINT FK_UserQuestionnaireAnswer_TaskId_Task_Id FOREIGN KEY REFERENCES [Task]([Id]),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL,
	[QuestionnaireItemId]	INT				NOT NULL CONSTRAINT [FK_UserQuestionnaireAnswer_QuestionnaireItemId_QuestionnaireItem] FOREIGN KEY REFERENCES [QuestionnaireItem]([Id]),
	[MyAnswer]				NVARCHAR(500)	NOT NULL,
	[LastUpdateTime]		DATE			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO