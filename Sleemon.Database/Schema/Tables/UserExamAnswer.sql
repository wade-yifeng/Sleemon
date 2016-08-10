CREATE TABLE [dbo].[UserExamAnswer]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[TaskId]			INT				NOT NULL CONSTRAINT FK_UserExamAnswer_TaskId_Task_Id FOREIGN KEY REFERENCES [Task]([Id]),
	[UserUniqueId]		NVARCHAR(200)	NOT NULL /*CONSTRAINT [FK_UserExamAnswer_User] FOREIGN KEY REFERENCES [User]([UserId])*/,
	[ExamQuestionId]	INT				NOT NULL CONSTRAINT [FK_UserExamAnswer_ExamQuestion] FOREIGN KEY REFERENCES [ExamQuestion]([Id]),
	[MyAnswer]			NVARCHAR(500)	NOT NULL,
	--[LastUpdateUser]	INT				NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL,
	[IsActive]			BIT				NOT NULL
)
GO