CREATE TABLE [dbo].[Keyword]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[Word]					NVARCHAR(20)					NOT NULL,
	[LastUpdateUser]		NVARCHAR(200)					NOT NULL,
	[LastUpdateTime]		DATETIME						NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]				BIT								NOT NULL DEFAULT(1)
)
GO