CREATE TABLE [dbo].[OrderShowFile]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[OrderShowId]		INT				NOT NULL CONSTRAINT [FK_OrderShowFile_UserOrderShow] FOREIGN KEY REFERENCES [UserOrderShow]([Id]),
	[Description]		NVARCHAR(500)	NULL,
	[FilePath]			NVARCHAR(500)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL,
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO