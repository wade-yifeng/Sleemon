CREATE TABLE [dbo].[UserMoments]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL /*CONSTRAINT[FK_UserTask_User] FOREIGN KEY REFERENCES  [User]([UserId])*/,
	[Moment]				NVARCHAR(200)	NOT NULL,
	[Category]				TINYINT			NOT NULL,
	[RefId]					INT				NULL,
	[PostTime]				DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO