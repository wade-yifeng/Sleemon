CREATE TABLE [dbo].[LearningFile]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Subject]			NVARCHAR(100)	NOT NULL,
	[Description]		NVARCHAR(MAX)	NULL,
	[Content]			NVARCHAR(MAX)	NOT NULL,
	[FileType]			TINYINT			NOT NULL,
	[ChapterId]			INT				NOT NULL CONSTRAINT [FK_LearningFile_ChapterId_LearningChapter_Id] FOREIGN KEY REFERENCES [LearningChapter]([Id]),
	[No]				INT				NOT NULL,
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO