CREATE TABLE [dbo].[ConsEnterpriseNotice]
(
	[Id]					INT PRIMARY KEY IDENTITY(1, 1)	NOT NULL,
	[UserUniqueId]			NVARCHAR(200)					NOT NULL,
	[EnterpriseNoticeId]	INT								NOT NULL CONSTRAINT [FK_ConsEnterpriseNotice_EnterpriseNoticeId_EnterpriseNotice] FOREIGN KEY REFERENCES [EnterpriseNotice]([Id]),
	[ConsDateTime]			DATETIME						NOT NULL DEFAULT(GETUTCDATE())
)
