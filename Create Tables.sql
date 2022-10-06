DROP TABLE IF EXISTS [dbo].[Rents];
DROP TABLE IF EXISTS [dbo].[Owners];
DROP TABLE IF EXISTS [dbo].[Tenants];
DROP TABLE IF EXISTS [dbo].[Properties];

CREATE TABLE [dbo].[Owners] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Tenants] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Properties] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Address] VARCHAR (150) NOT NULL,
    [Owner]   INT           NULL,
    [Tenant]  INT           NULL,
    [Rent]    INT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Owner]) REFERENCES [dbo].[Owners] ([Id]),
    FOREIGN KEY ([Tenant]) REFERENCES [dbo].[Tenants] ([Id])
);

CREATE TABLE [dbo].[Rents] (
    [Year]     INT NOT NULL,
    [Month]    INT NOT NULL,
    [Property] INT NOT NULL,
    [Tenant]   INT NOT NULL,
    [Owed]     INT NOT NULL,
    [Payed]    INT DEFAULT ((0)),
    [isPayed]     AS Cast(CASE WHEN ([Owed] - [Payed] <= 0) THEN 1 ELSE 0 END AS BIT),
    PRIMARY KEY CLUSTERED ([Year] ASC, [Month] ASC, [Property] ASC),
    FOREIGN KEY ([Property]) REFERENCES [dbo].[Properties] ([Id]),
    FOREIGN KEY ([Tenant]) REFERENCES [dbo].[Tenants] ([Id]),
    CHECK ([Month]>=(1) AND [Month]<=(12))
);

