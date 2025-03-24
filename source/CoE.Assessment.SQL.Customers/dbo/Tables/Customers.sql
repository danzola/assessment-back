CREATE TABLE [dbo].[Customers] (
    [Id]        INT           NOT NULL,
    [FirstName] VARCHAR (255) NOT NULL,
    [LastName]  VARCHAR (255) NOT NULL,
    [Email]     VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

