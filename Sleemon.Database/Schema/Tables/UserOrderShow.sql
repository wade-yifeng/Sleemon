CREATE TABLE [dbo].[UserOrderShow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserUniqueId] [nvarchar](200) NOT NULL,
	[LegalStatus] [tinyint] NOT NULL CONSTRAINT [DF_UserOrderShow_IsLegal]  DEFAULT ((0)),
	[ShowTime] [datetime] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL CONSTRAINT [DF__UserOrder__LastU__3493CFA7]  DEFAULT (getutcdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__UserOrder__IsAct__3587F3E0]  DEFAULT ((1)),
 CONSTRAINT [PK__UserOrde__3214EC07E8EE2576] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-未审核 1-合法 2-非法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserOrderShow', @level2type=N'COLUMN',@level2name=N'LegalStatus'
GO
