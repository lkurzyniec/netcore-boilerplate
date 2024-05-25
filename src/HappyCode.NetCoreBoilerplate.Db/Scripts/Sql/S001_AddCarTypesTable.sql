CREATE TABLE [dbo].[CarTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](20) NOT NULL,
 CONSTRAINT [PK_CarTypes] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)) ON [PRIMARY]
GO

INSERT INTO [CarTypes] ([Type])
VALUES ('Hatchback'), ('Kombi'), ('Sedan')
GO

GRANT ALL ON CarTypes TO [user]
GO

ALTER TABLE [dbo].[Cars]
    ADD [CarTypeId] INT NULL CONSTRAINT [FK_Cars_CarTypes] FOREIGN KEY([CarTypeId]) REFERENCES [dbo].[CarTypes] ([Id])
    ON UPDATE CASCADE
GO
