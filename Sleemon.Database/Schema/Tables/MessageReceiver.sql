CREATE TABLE [dbo].[MessageReceiver]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[MessageId]			INT				NOT NULL CONSTRAINT [FK_MessageReceiver_MessageDispatch] FOREIGN KEY REFERENCES [MessageDispatch]([Id]),
	[UserId]			INT				NOT NULL CONSTRAINT [FK_MessageReceiver_User] FOREIGN KEY REFERENCES [User]([Id]),
	[ReceivedTime]		DATETIME		NULL,
	[Status]			TINYINT			NOT NULL DEFAULT(1),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-未发送 2-发送成功 3-发送失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageReceiver', @level2type=N'COLUMN',@level2name=N'Status'
GO