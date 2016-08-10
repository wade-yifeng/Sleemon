CREATE TABLE [dbo].[ExamQuestion]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[ExamId]				INT								NOT NULL CONSTRAINT [FK_ExamQuestion_Exam] FOREIGN KEY REFERENCES [Exam]([Id]),
	[No]					SMALLINT						NOT NULL,
	[Category]				TINYINT							NOT NULL DEFAULT(0),
	[Question]				NVARCHAR(1000)					NOT NULL,
	[Image]					NVARCHAR(MAX)					NULL, --问题附带的图片路径
	[CorrectAnswer]			NVARCHAR(500)					NULL,
	[Score]					FLOAT							NOT NULL,
	[LastUpdateUser]		NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]		DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT								NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型：0-单选 1-多选 2-主观' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamQuestion', @level2type=N'COLUMN',@level2name=N'Category'
GO