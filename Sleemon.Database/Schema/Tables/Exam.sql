CREATE TABLE [dbo].[Exam]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[Title]					NVARCHAR(200)		NOT NULL,
	[Description]			NVARCHAR(500)		NULL,
	[TotalScore]			FLOAT				NOT NULL,
	[PassingScore]			FLOAT				NOT NULL,
	[Status]				TINYINT				NOT NULL DEFAULT(0), --1 已保存 2 已发布
	[GroupKey]				UNIQUEIDENTIFIER	NULL, --GUID，存在版本更新时，用于标示同属一份试卷
	[LastUpdateUser]		NVARCHAR(200)		NOT NULL,
	[LastUpdateTime]		DATETIME			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT					NOT NULL DEFAULT(1)
)
GO