CREATE TABLE [dbo].[Training]
(
	[Id]				INT					NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Subject]			NVARCHAR(200)		NOT NULL,
	[Description]		NVARCHAR(2000)		NULL,
	[Avatar]			NVARCHAR(100)		NULL,
	[Location]			NVARCHAR(500)		NOT NULL,
	[StartFrom]			DATETIME			NOT NULL,
	[EndTo]				DATETIME			NOT NULL,
	[MaxParticipant]	SMALLINT			NOT NULL,
	[JoinDeadline]		DATETIME			NOT NULL,
	[IsPublic]			BIT					NOT NULL,
	[IsCheckInNeeded]	BIT					NOT NULL DEFAULT(0),
	[CheckInQRCode]		NVARCHAR(1000)		NULL,
	[GroupKey]			UNIQUEIDENTIFIER	NOT NULL,
	[Status]			TINYINT					NOT NULL,
	[LastUpdateUser]	NVARCHAR(200)		NOT NULL,
	[LastUpdateTime]	DATETIME			NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT					NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-非公开 1-公开' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Training', @level2type=N'COLUMN',@level2name=N'IsPublic'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-未发布 2-已发布 3-已确认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Training', @level2type=N'COLUMN',@level2name=N'Status'
GO