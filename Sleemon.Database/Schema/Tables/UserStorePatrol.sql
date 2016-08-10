CREATE TABLE [dbo].[UserStorePatrol]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL,
	[StorePatrolId]			INT				NOT NULL CONSTRAINT [FK_UserStorePatrolFile_StorePatrol] FOREIGN KEY REFERENCES [StorePatrol]([Id]),
	[Description]			NVARCHAR(500)	NULL,
	[FilePath]				NVARCHAR(500)	NOT NULL,
	[AdminRate]				FLOAT			NULL,
	[AdminComment]			NVARCHAR(500)	NULL,
	[LastUpdateTime]		DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO