USE [master]
GO
CREATE DATABASE [PE_Practice_EcommerceE]
GO
USE [PE_Practice_EcommerceE]
GO

CREATE TABLE [dbo].[Designers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](40) NOT NULL,
	[Male] [bit] NOT NULL,
	[Dob] [date] NOT NULL,
	[Nationality] [varchar](30) NOT NULL,
	[Description] [ntext] NOT NULL,
	CONSTRAINT [PK_Designers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Manufacturers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[LaunchDate] [date] NULL,
	[Description] [text] NULL,
	[Material] [varchar](30) NOT NULL,
	[ManufacturerId] [int] NULL,
	[DesignerId] [int] NULL,
	CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Reviewers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[Male] [bit] NULL,
	[Dob] [date] NULL,
	[Description] [text] NULL,
	[Nationality] [varchar](30) NULL,
	CONSTRAINT [PK_Reviewers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nchar](10) NOT NULL,
	CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Product_Tag](
	[ProductId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
	CONSTRAINT [PK_Product_Tag] PRIMARY KEY CLUSTERED ([ProductId] ASC, [TagId] ASC)
)
GO

CREATE TABLE [dbo].[Product_Reviewer](
	[ProductId] [int] NOT NULL,
	[ReviewerId] [int] NOT NULL,
	CONSTRAINT [PK_Product_Reviewer] PRIMARY KEY CLUSTERED ([ProductId] ASC, [ReviewerId] ASC)
)
GO

ALTER TABLE [dbo].[Products] WITH CHECK ADD CONSTRAINT [FK_Products_Designers] FOREIGN KEY([DesignerId]) REFERENCES [dbo].[Designers] ([Id])
GO
ALTER TABLE [dbo].[Products] WITH CHECK ADD CONSTRAINT [FK_Products_Manufacturers] FOREIGN KEY([ManufacturerId]) REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[Product_Tag] WITH CHECK ADD CONSTRAINT [FK_Product_Tag_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Product_Tag] WITH CHECK ADD CONSTRAINT [FK_Product_Tag_Tags] FOREIGN KEY([TagId]) REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[Product_Reviewer] WITH CHECK ADD CONSTRAINT [FK_Product_Reviewer_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Product_Reviewer] WITH CHECK ADD CONSTRAINT [FK_Product_Reviewer_Reviewers] FOREIGN KEY([ReviewerId]) REFERENCES [dbo].[Reviewers] ([Id])
GO

-- Seed data
SET IDENTITY_INSERT [dbo].[Designers] ON
INSERT [dbo].[Designers] ([Id],[FullName],[Male],[Dob],[Nationality],[Description]) VALUES
(1, 'Jony Ive', 1, '1967-02-27', 'England', 'Former Chief Design Officer at Apple, known for the iPhone and iMac.'),
(2, 'Yves Behar', 1, '1967-08-01', 'Switzerland', 'Founder of fuseproject, known for human-centered product design.'),
(3, 'Marc Newson', 1, '1963-10-20', 'Australia', 'Industrial designer known for futuristic furniture and consumer goods.'),
(4, 'Zaha Hadid', 0, '1950-10-31', 'Iraq', 'Architect and designer known for neo-futuristic forms.'),
(5, 'Philippe Starck', 1, '1949-01-18', 'France', 'French designer known for playful, iconic furniture pieces.')
SET IDENTITY_INSERT [dbo].[Designers] OFF
GO

SET IDENTITY_INSERT [dbo].[Manufacturers] ON
INSERT [dbo].[Manufacturers] ([Id],[Name]) VALUES
(1, 'Apple Inc.'), (2, 'Herman Miller'), (3, 'Knoll'), (4, 'Vitra'), (5, 'Kartell')
SET IDENTITY_INSERT [dbo].[Manufacturers] OFF
GO

SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([Id],[Name],[LaunchDate],[Description],[Material],[ManufacturerId],[DesignerId]) VALUES
(1, 'iPhone 12', '2020-10-23', 'Smartphone with A14 Bionic chip.', 'Aluminum', 1, 1),
(2, 'MacBook Air', '2008-01-15', 'Ultra-thin laptop.', 'Aluminum', 1, 1),
(3, 'Leaf Lamp', '2007-05-01', 'LED desk lamp with touch dimming.', 'Plastic', 2, 2),
(4, 'Lockheed Lounge', '1986-01-01', 'Riveted aluminum chaise longue.', 'Aluminum', 3, 3),
(5, 'Aqua Table', '2005-01-01', 'Fluid-form fiberglass table.', 'Fiberglass', 4, 4),
(6, 'Louis Ghost Chair', '2002-01-01', 'Transparent polycarbonate armchair.', 'Polycarbonate', 5, 5)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO

SET IDENTITY_INSERT [dbo].[Tags] ON
INSERT [dbo].[Tags] ([Id],[Title]) VALUES
(1, 'Tech'), (2, 'Furniture'), (3, 'Lighting'), (4, 'Iconic'), (5, 'Minimal')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
