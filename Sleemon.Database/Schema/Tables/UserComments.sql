CREATE TABLE [dbo].[UserComments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[UserUniqueId] [nvarchar](200) NOT NULL,
	[Comment] [nvarchar](250) NOT NULL,
	[Category] [tinyint] NOT NULL,
	[LinkedId] [int] NULL,
	[LegalStatus] [tinyint] NOT NULL CONSTRAINT [DF__UserComme__IsLeg__2DE6D218]  DEFAULT ((0)),
	[CommentTime] [datetime] NOT NULL CONSTRAINT [DF__UserComme__Comme__2EDAF651]  DEFAULT (getutcdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__UserComme__IsAct__2FCF1A8A]  DEFAULT ((1)),
 CONSTRAINT [PK__UserComm__3214EC07F68F220A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserComments]  WITH CHECK ADD  CONSTRAINT [FK_ParentId_UserComments] FOREIGN KEY([ParentId])
REFERENCES [dbo].[UserComments] ([Id])
GO

ALTER TABLE [dbo].[UserComments] CHECK CONSTRAINT [FK_ParentId_UserComments]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-未审核 1-合法 2-非法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserComments', @level2type=N'COLUMN',@level2name=N'LegalStatus'
GO