CREATE TABLE [dbo].[Permission] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ParentId]       INT            DEFAULT ((0)) NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Url]            NVARCHAR (50)  NULL,
    [IconClass]      NVARCHAR (50)  NULL,
    [IsMenu]         BIT            DEFAULT ((1)) NOT NULL,
    [Description]    NVARCHAR (500) NOT NULL,
	[Sort]           int            Not NULL DEFAULT(1),
    [IsActive]       BIT            DEFAULT ((1)) NOT NULL,
    [LastUpdateUser] NVARCHAR (200) NOT NULL,
    [LastUpdateTime] DATETIME       DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

