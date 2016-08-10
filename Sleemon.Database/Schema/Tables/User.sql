CREATE TABLE [dbo].[User]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL, --WechatId
	[Name]					NVARCHAR(50)	NOT NULL,
	[Gender]				BIT				NOT NULL DEFAULT(0),
	[WeixinId]				NVARCHAR(256)	NOT NULL,
	[Avatar]				NVARCHAR(MAX)	NULL DEFAULT(''),
	[Mobile]				NVARCHAR(16)	NULL DEFAULT(''),
	[Password]				VARCHAR(50)		NULL,
	[IsOriginalPassword]	BIT				NOT NULL DEFAULT(1),
	[Position]				NVARCHAR(100)	NULL,
	[Country]				NVARCHAR(50)	NULL,
	[Province]				NVARCHAR(50)	NULL,
	[City]					NVARCHAR(50)	NULL,
	[District]				NVARCHAR(50)	NULL,
	[EntryDate]				DATE			NULL,
	[Email]					NVARCHAR(50)	NULL,
	[Status]				INT				NOT NULL DEFAULT(0),
	[Grade]					TINYINT			NOT NULL,
	[Point]					INT				NOT NULL DEFAULT(0),
	[ProductAbility]		INT				NOT NULL DEFAULT(0),
	[SalesAbility]			INT				NOT NULL DEFAULT(0),
	[ExhibitAbility]		INT				NOT NULL DEFAULT(0),
	[IsAdmin]				BIT				NOT NULL DEFAULT(0),
	[IsSuperAdmin]			BIT				NOT NULL DEFAULT(0),
	[LastUpdateTime]		DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX IX_UserId	ON [dbo].[User]([UserUniqueId]) WHERE [IsActive] = 1
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-不是 1-是' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'IsAdmin'
GO