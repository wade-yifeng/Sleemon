CREATE TABLE [dbo].[ProsEnterpriseNotice]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[UserUniqueId]			NVARCHAR(200)					NOT NULL,
	[EnterpriseNoticeId]	INT								NOT NULL CONSTRAINT [FK_ProsEnterpriseNotice_EnterpriseNoticeId_EnterpriseNotice] FOREIGN KEY REFERENCES [EnterpriseNotice]([Id]),
	[ProsDateTime]			DATETIME						NOT NULL DEFAULT(GETUTCDATE())
)
