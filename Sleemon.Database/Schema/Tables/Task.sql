CREATE TABLE [dbo].[Task]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title]				NVARCHAR(200)	NOT NULL,
	[Description]		NVARCHAR(2000)	NULL,
	[TaskCategory]		TINYINT			NOT NULL,
	[StartFrom]			DATE			NOT NULL,
	[EndTo]				DATE			NULL,
	[Point]				INT				NOT NULL DEFAULT(0),
	[OverduePoint]		INT				NOT NULL DEFAULT(0),
	[ProductAbility]	INT				NOT NULL DEFAULT(0),
	[SalesAbility]		INT				NOT NULL DEFAULT(0),
	[ExhibitAbility]	INT				NOT NULL DEFAULT(0),
	[BelongTo]			INT				NOT NULL DEFAULT(0),
	[Status]			TINYINT			NOT NULL DEFAULT(0), --1 已保存 2 已发布
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型：0-每日签到 1-课程学习 2-考试 3-巡店 4-问卷调查' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-单项任务 2-培训子任务 3-任务组子任务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'BelongTo'
GO