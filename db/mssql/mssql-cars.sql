USE [master]
GO

IF DB_ID('cars') IS NOT NULL
  set noexec on               -- prevent creation when already exists

/****** Object:  Database [cars]    Script Date: 18.10.2019 18:33:09 ******/
CREATE DATABASE [cars];
GO

USE [cars]
GO

/****** Object:  Table [dbo].[Cars]    Script Date: 18.10.2019 18:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Plate] [varchar](50) NOT NULL,
	[Model] [varchar](50) NULL,
	[OwnerId] [int] NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Owners]    Script Date: 18.10.2019 18:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Owners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[FullName]  AS (([FirstName]+' ')+[LastName]),
 CONSTRAINT [PK_Owners] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Cars] ON
GO
INSERT [dbo].[Cars] ([Id], [Plate], [Model], [OwnerId]) VALUES (1, N'JHV 770', N'Mercedes-Benz GLE Coupe', 1)
GO
INSERT [dbo].[Cars] ([Id], [Plate], [Model], [OwnerId]) VALUES (2, N'TAD-3173', N'Datsun GO+', 1)
GO
INSERT [dbo].[Cars] ([Id], [Plate], [Model], [OwnerId]) VALUES (3, N'43-L348', N'Maruti Suzuki Swift', 2)
GO
INSERT [dbo].[Cars] ([Id], [Plate], [Model], [OwnerId]) VALUES (4, N'XPB-2935', N'Land Rover Discovery Sport', 3)
GO
INSERT [dbo].[Cars] ([Id], [Plate], [Model], [OwnerId]) VALUES (5, N'805-UXC', N'Nissan GT-R', NULL)
GO
SET IDENTITY_INSERT [dbo].[Cars] OFF
GO

SET IDENTITY_INSERT [dbo].[Owners] ON
GO
INSERT [dbo].[Owners] ([Id], [FirstName], [LastName]) VALUES (1, N'Peter', N'Diaz')
GO
INSERT [dbo].[Owners] ([Id], [FirstName], [LastName]) VALUES (2, N'Leon', N'Leonard')
GO
INSERT [dbo].[Owners] ([Id], [FirstName], [LastName]) VALUES (3, N'Shirley', N'Baker')
GO
INSERT [dbo].[Owners] ([Id], [FirstName], [LastName]) VALUES (4, N'Nancy', N'Davis')
GO
SET IDENTITY_INSERT [dbo].[Owners] OFF
GO

ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Cars_Owners] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Owners] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FK_Cars_Owners]
GO

/****** Users ******/

CREATE LOGIN [migrator] WITH PASSWORD = 'simplePWD123!'
GO
CREATE USER [migrator] FOR LOGIN [migrator] WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_owner', N'migrator'
GO

CREATE LOGIN [user] WITH PASSWORD = 'simplePWD123!'
GO
CREATE USER [user] FOR LOGIN [user] WITH DEFAULT_SCHEMA=[dbo]
GO
GRANT CONTROL ON DATABASE::cars TO [user];
GO
