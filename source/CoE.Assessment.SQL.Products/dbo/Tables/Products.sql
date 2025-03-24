CREATE TABLE [dbo].[Products] (
    [Id]    INT             NOT NULL,
    [Name]  VARCHAR (255)   NOT NULL,
    [Price] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);

