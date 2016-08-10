CREATE TABLE [dbo].[UserTraining]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL /*CONSTRAINT [FK_UserTraining_User] FOREIGN KEY REFERENCES [User]([UserId])*/,
	[TrainingId]			INT				NOT NULL CONSTRAINT [FK_UserTraining_Training] FOREIGN KEY REFERENCES [Training]([Id]),
	[JoinTime]				DATETIME		NULL,
	[JoinStatus]			INT				NULL,
	[IsCheckInDone]			BIT				NULL,
	[IsActive]				BIT				NOT NULL DEFAULT(1)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-未审核 2-已通过 3-未通过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserTraining', @level2type=N'COLUMN',@level2name=N'JoinStatus'
GO