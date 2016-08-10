CREATE TABLE [dbo].[LearningChapter]
(
	[Id]				INT					NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title]				NVARCHAR(100)		NOT NULL,
	[Description]		NVARCHAR(500)		NULL,
	[CourseId]			INT					NOT NULL CONSTRAINT [FK_LearningChapter_CourseId_LearningCourse_Id] FOREIGN KEY REFERENCES [LearningCourse]([Id]),
	[No]				INT					NOT NULL,
	[LastUpdateUser]	NVARCHAR(500)		NOT NULL,
	[LastUpdateTime]	DATETIME			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT					NOT NULL DEFAULT(1)
)
