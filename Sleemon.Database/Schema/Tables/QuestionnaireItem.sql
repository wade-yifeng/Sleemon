CREATE TABLE [dbo].[QuestionnaireItem]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[QuestionnaireId]	INT				NOT NULL CONSTRAINT [FK_QuestionnaireItem_Questionnaire] FOREIGN KEY REFERENCES [dbo].[Questionnaire]([Id]),
	[No]				SMALLINT		NOT NULL,
	[Question]			NVARCHAR(500)	NOT NULL,
	[Image]				NVARCHAR(500)	NULL,
	[Category]			TINYINT			NOT NULL,
	[Rate]				FLOAT			NOT NULL,
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO