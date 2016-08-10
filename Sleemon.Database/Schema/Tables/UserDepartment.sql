CREATE TABLE [dbo].[UserDepartment]
(
	[Id]					INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserUniqueId]			NVARCHAR(200)	NOT NULL /*CONSTRAINT FK_UserDepartment_User FOREIGN KEY REFERENCES [User]([UserId])*/,
	[DepartmentUniqueId]	INT				NOT NULL /*CONSTRAINT [FK_User_Department] FOREIGN KEY REFERENCES [Department]([Id])*/,
)
