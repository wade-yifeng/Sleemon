CREATE TABLE [dbo].[MessageDispatch]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Subject]			NVARCHAR(500)	NOT NULL,
	[ToUsers]			NVARCHAR(MAX)	NULL,
	[ToDepts]			NVARCHAR(MAX)	NULL,
	[Priority]			TINYINT			NOT NULL DEFAULT(1),
	[MessageType]		TINYINT			NOT NULL DEFAULT(0),
	[LinkedId]			INT				NULL,
	[DispatchType]		TINYINT			NOT NULL DEFAULT(1),
	[DispatchTime]		DATETIME		NULL,
	[Status]			TINYINT			NOT NULL,
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优先级： 1-低 2-中 3-高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageDispatch', @level2type=N'COLUMN',@level2name=N'Priority'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型（用于生成链接）： 1-企业资讯 2-任务 3-培训' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageDispatch', @level2type=N'COLUMN',@level2name=N'MessageType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关id（用于生成链接）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageDispatch', @level2type=N'COLUMN',@level2name=N'LinkedId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-立即发布 2-延迟发布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageDispatch', @level2type=N'COLUMN',@level2name=N'DispatchType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息状态： 1-未分发 2-发送中 3-发送成功 4-发送失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageDispatch', @level2type=N'COLUMN',@level2name=N'Status'
GO