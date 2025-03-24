CREATE TABLE [dbo].[Orders] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [CustomerId]  INT             NOT NULL,
    [ProductId]   INT             NOT NULL,
    [Quantity]    INT             NOT NULL,
    [TotalAmount] DECIMAL (18, 2) NOT NULL,
    [OrderDate]   DATETIME        NOT NULL
);

