CREATE TABLE [dbo].[UserDailySignIn]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]		NVARCHAR(200)	NOT NULL /*CONSTRAINT [FK_UserDailySignIn_User] FOREIGN KEY REFERENCES [User]([UserId])*/,--TODO: UserId - wechatid
	[SignInDate]		DATE			NOT NULL,
	--[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO