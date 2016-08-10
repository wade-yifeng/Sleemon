CREATE TABLE [dbo].[ExamChoice]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[ExamQuestionId]		INT								NOT NULL CONSTRAINT [FK_ExamChoice_ExamQuestion] FOREIGN KEY REFERENCES [ExamQuestion]([Id]),
	[Choice]				TINYINT							NOT NULL,
	[Description]			NVARCHAR(MAX)					NOT NULL,
	[Image]					NVARCHAR(MAX)					NULL, --选项附带的图片路径
	[IsAnswer]				BIT								NOT NULL,
	[LastUpdateUser]		NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]		DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT								NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-A 2-B 3-C 4-D 5-E...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamChoice', @level2type=N'COLUMN',@level2name=N'Choice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-否 1-是' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamChoice', @level2type=N'COLUMN',@level2name=N'IsAnswer'
GO