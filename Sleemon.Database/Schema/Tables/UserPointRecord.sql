CREATE TABLE [dbo].[UserPointRecord]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]		NVARCHAR(200)	NOT NULL /*CONSTRAINT [FK_UserPointRecord_User] FOREIGN KEY REFERENCES [User]([UserId])*/,
	[Point]				INT				NOT NULL,
	[Operator]			BIT				NOT NULL,
	[Description]		NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL
)
GO