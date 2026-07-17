USE [master]
GO
CREATE DATABASE [PE_Practice_LibraryA]
GO
USE [PE_Practice_LibraryA]
GO

CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](40) NOT NULL,
	[Male] [bit] NOT NULL,
	[Dob] [date] NOT NULL,
	[Nationality] [varchar](30) NOT NULL,
	[Description] [ntext] NOT NULL,
	CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Publishers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	CONSTRAINT [PK_Publishers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[PublishDate] [date] NULL,
	[Description] [text] NULL,
	[Language] [varchar](30) NOT NULL,
	[PublisherId] [int] NULL,
	[AuthorId] [int] NULL,
	CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Translators](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[Male] [bit] NULL,
	[Dob] [date] NULL,
	[Description] [text] NULL,
	[Nationality] [varchar](30) NULL,
	CONSTRAINT [PK_Translators] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nchar](10) NOT NULL,
	CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Book_Genre](
	[BookId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
	CONSTRAINT [PK_Book_Genre] PRIMARY KEY CLUSTERED ([BookId] ASC, [GenreId] ASC)
)
GO

CREATE TABLE [dbo].[Book_Translator](
	[BookId] [int] NOT NULL,
	[TranslatorId] [int] NOT NULL,
	CONSTRAINT [PK_Book_Translator] PRIMARY KEY CLUSTERED ([BookId] ASC, [TranslatorId] ASC)
)
GO

ALTER TABLE [dbo].[Books] WITH CHECK ADD CONSTRAINT [FK_Books_Authors] FOREIGN KEY([AuthorId]) REFERENCES [dbo].[Authors] ([Id])
GO
ALTER TABLE [dbo].[Books] WITH CHECK ADD CONSTRAINT [FK_Books_Publishers] FOREIGN KEY([PublisherId]) REFERENCES [dbo].[Publishers] ([Id])
GO
ALTER TABLE [dbo].[Book_Genre] WITH CHECK ADD CONSTRAINT [FK_Book_Genre_Books] FOREIGN KEY([BookId]) REFERENCES [dbo].[Books] ([Id])
GO
ALTER TABLE [dbo].[Book_Genre] WITH CHECK ADD CONSTRAINT [FK_Book_Genre_Genres] FOREIGN KEY([GenreId]) REFERENCES [dbo].[Genres] ([Id])
GO
ALTER TABLE [dbo].[Book_Translator] WITH CHECK ADD CONSTRAINT [FK_Book_Translator_Books] FOREIGN KEY([BookId]) REFERENCES [dbo].[Books] ([Id])
GO
ALTER TABLE [dbo].[Book_Translator] WITH CHECK ADD CONSTRAINT [FK_Book_Translator_Translators] FOREIGN KEY([TranslatorId]) REFERENCES [dbo].[Translators] ([Id])
GO

-- Seed data
SET IDENTITY_INSERT [dbo].[Authors] ON
INSERT [dbo].[Authors] ([Id],[FullName],[Male],[Dob],[Nationality],[Description]) VALUES
(1, 'J.K. Rowling', 0, '1965-07-31', 'England', 'British author, best known for the Harry Potter series.'),
(2, 'George R.R. Martin', 1, '1948-09-20', 'USA', 'American novelist, author of A Song of Ice and Fire.'),
(3, 'Haruki Murakami', 1, '1949-01-12', 'Japan', 'Japanese writer known for surreal, melancholic novels.'),
(4, 'Agatha Christie', 0, '1890-09-15', 'England', 'English writer known for detective novels featuring Hercule Poirot.'),
(5, 'Stephen King', 1, '1947-09-21', 'USA', 'American author of horror and supernatural fiction.')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO

SET IDENTITY_INSERT [dbo].[Publishers] ON
INSERT [dbo].[Publishers] ([Id],[Name]) VALUES
(1, 'Bloomsbury'), (2, 'Bantam Books'), (3, 'Kodansha'), (4, 'HarperCollins'), (5, 'Scribner')
SET IDENTITY_INSERT [dbo].[Publishers] OFF
GO

SET IDENTITY_INSERT [dbo].[Books] ON
INSERT [dbo].[Books] ([Id],[Title],[PublishDate],[Description],[Language],[PublisherId],[AuthorId]) VALUES
(1, 'Harry Potter and the Philosopher''s Stone', '1997-06-26', 'A young boy discovers he is a wizard.', 'English', 1, 1),
(2, 'A Game of Thrones', '1996-08-01', 'Noble families vie for control of the Iron Throne.', 'English', 2, 2),
(3, 'Norwegian Wood', '1987-09-04', 'A nostalgic story of loss and burgeoning sexuality.', 'Japanese', 3, 3),
(4, 'Murder on the Orient Express', '1934-01-01', 'Detective Poirot investigates a murder aboard a train.', 'English', 4, 4),
(5, 'The Shining', '1977-01-28', 'A family isolated in a haunted hotel.', 'English', 5, 5),
(6, 'Kafka on the Shore', '2002-09-12', 'A teenage runaway and an aging simpleton''s intertwined journeys.', 'Japanese', 3, 3)
SET IDENTITY_INSERT [dbo].[Books] OFF
GO

SET IDENTITY_INSERT [dbo].[Genres] ON
INSERT [dbo].[Genres] ([Id],[Title]) VALUES
(1, 'Fantasy'), (2, 'Drama'), (3, 'Mystery'), (4, 'Horror'), (5, 'Romance')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO
