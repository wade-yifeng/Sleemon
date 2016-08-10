CREATE TABLE [dbo].[ProsComments]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserCommentId]			INT				NOT NULL CONSTRAINT [FK_ProsComments_UserCommentId_UserComments] FOREIGN KEY REFERENCES [UserComments]([Id]),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL,
	[ProsDateTime]			DATETIME		NOT NULL DEFAULT(GETUTCDATE())
)
