CREATE TABLE [dbo].[QuestionnaireChoice]
(
	[Id]						INT					NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[QuestionnaireItemId]		INT					NOT NULL CONSTRAINT FK_QuestionnaireChoice_QuestionnaireItemId_QuestionnaireItem_Id		FOREIGN KEY REFERENCES [QuestionnaireItem]([Id]),
	[No]						SMALLINT			NOT NULL,
	[Choice]					TINYINT				NOT NULL,
	[Description]				NVARCHAR(1000)		NOT NULL,
	[Image]						NVARCHAR(500)		NULL,
	[LastUpdateUser]			NVARCHAR(200)		NOT NULL,
	[LastUpdateTime]			DATETIME			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]					BIT					NOT NULL DEFAULT(1)
)
