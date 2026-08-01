USE [master]
GO
CREATE DATABASE [PE_Practice_CinemaH]
GO
USE [PE_Practice_CinemaH]
GO

CREATE TABLE [dbo].[Directors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](40) NOT NULL,
	[Male] [bit] NOT NULL,
	[Dob] [date] NOT NULL,
	[Nationality] [varchar](30) NOT NULL,
	[Description] [ntext] NOT NULL,
	CONSTRAINT [PK_Directors] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Producers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	CONSTRAINT [PK_Producers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Movies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[ReleaseDate] [date] NULL,
	[Description] [text] NULL,
	[Language] [varchar](30) NOT NULL,
	[ProducerId] [int] NULL,
	[DirectorId] [int] NULL,
	CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Stars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[Male] [bit] NULL,
	[Dob] [date] NULL,
	[Description] [text] NULL,
	[Nationality] [varchar](30) NULL,
	CONSTRAINT [PK_Stars] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nchar](10) NOT NULL,
	CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Movie_Genre](
	[MovieId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
	CONSTRAINT [PK_Movie_Genre] PRIMARY KEY CLUSTERED ([MovieId] ASC, [GenreId] ASC)
)
GO

CREATE TABLE [dbo].[Movie_Star](
	[MovieId] [int] NOT NULL,
	[StarId] [int] NOT NULL,
	CONSTRAINT [PK_Movie_Star] PRIMARY KEY CLUSTERED ([MovieId] ASC, [StarId] ASC)
)
GO

ALTER TABLE [dbo].[Movies] WITH CHECK ADD CONSTRAINT [FK_Movies_Directors] FOREIGN KEY([DirectorId]) REFERENCES [dbo].[Directors] ([Id])
GO
ALTER TABLE [dbo].[Movies] WITH CHECK ADD CONSTRAINT [FK_Movies_Producers] FOREIGN KEY([ProducerId]) REFERENCES [dbo].[Producers] ([Id])
GO
ALTER TABLE [dbo].[Movie_Genre] WITH CHECK ADD CONSTRAINT [FK_Movie_Genre_Movies] FOREIGN KEY([MovieId]) REFERENCES [dbo].[Movies] ([Id])
GO
ALTER TABLE [dbo].[Movie_Genre] WITH CHECK ADD CONSTRAINT [FK_Movie_Genre_Genres] FOREIGN KEY([GenreId]) REFERENCES [dbo].[Genres] ([Id])
GO
ALTER TABLE [dbo].[Movie_Star] WITH CHECK ADD CONSTRAINT [FK_Movie_Star_Movies] FOREIGN KEY([MovieId]) REFERENCES [dbo].[Movies] ([Id])
GO
ALTER TABLE [dbo].[Movie_Star] WITH CHECK ADD CONSTRAINT [FK_Movie_Star_Stars] FOREIGN KEY([StarId]) REFERENCES [dbo].[Stars] ([Id])
GO

-- Seed data
SET IDENTITY_INSERT [dbo].[Directors] ON
INSERT [dbo].[Directors] ([Id],[FullName],[Male],[Dob],[Nationality],[Description]) VALUES
(1, 'Christopher Nolan', 1, '1970-07-30', 'England', 'British-American filmmaker known for Inception and The Dark Knight trilogy.'),
(2, 'Steven Spielberg', 1, '1946-12-18', 'USA', 'American director behind Jaws, E.T. and Jurassic Park.'),
(3, 'Bong Joon-ho', 1, '1969-09-14', 'South Korea', 'South Korean director known for Parasite and Snowpiercer.'),
(4, 'Kathryn Bigelow', 0, '1951-11-27', 'USA', 'American director known for The Hurt Locker and Zero Dark Thirty.'),
(5, 'Greta Gerwig', 0, '1983-08-04', 'USA', 'American director known for Lady Bird and Barbie.')
SET IDENTITY_INSERT [dbo].[Directors] OFF
GO

SET IDENTITY_INSERT [dbo].[Producers] ON
INSERT [dbo].[Producers] ([Id],[Name]) VALUES
(1, 'Warner Bros. Pictures'), (2, 'Universal Pictures'), (3, 'CJ Entertainment'), (4, 'Legendary Pictures'), (5, 'Working Title Films')
SET IDENTITY_INSERT [dbo].[Producers] OFF
GO

SET IDENTITY_INSERT [dbo].[Movies] ON
INSERT [dbo].[Movies] ([Id],[Title],[ReleaseDate],[Description],[Language],[ProducerId],[DirectorId]) VALUES
(1, 'Inception', '2010-07-16', 'A thief who steals corporate secrets through dream-sharing technology.', 'English', 1, 1),
(2, 'The Dark Knight', '2008-07-18', 'Batman faces the Joker in Gotham City.', 'English', 1, 1),
(3, 'Jurassic Park', '1993-06-11', 'A theme park with cloned dinosaurs goes wrong.', 'English', 2, 2),
(4, 'E.T. the Extra-Terrestrial', '1982-06-11', 'A boy befriends a stranded alien.', 'English', 2, 2),
(5, 'Parasite', '2019-05-30', 'A poor family schemes to become employed by a wealthy family.', 'Korean', 3, 3),
(6, 'Zero Dark Thirty', '2012-12-19', 'The decade-long hunt for Osama bin Laden.', 'English', 4, 4),
(7, 'Barbie', '2023-07-21', 'Barbie and Ken venture from Barbieland to the real world.', 'English', 5, 5)
SET IDENTITY_INSERT [dbo].[Movies] OFF
GO

SET IDENTITY_INSERT [dbo].[Genres] ON
INSERT [dbo].[Genres] ([Id],[Title]) VALUES
(1, 'Sci-Fi'), (2, 'Action'), (3, 'Thriller'), (4, 'Drama'), (5, 'Comedy')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO

SET IDENTITY_INSERT [dbo].[Stars] ON
INSERT [dbo].[Stars] ([Id],[FullName],[Male],[Dob],[Description],[Nationality]) VALUES
(1, 'Leonardo DiCaprio', 1, '1974-11-11', 'American actor known for Inception and Titanic.', 'USA'),
(2, 'Sam Neill', 1, '1947-09-14', 'New Zealand actor known for Jurassic Park.', 'New Zealand'),
(3, 'Song Kang-ho', 1, '1967-01-17', 'South Korean actor known for Parasite.', 'South Korea'),
(4, 'Jessica Chastain', 0, '1977-03-24', 'American actress known for Zero Dark Thirty.', 'USA'),
(5, 'Margot Robbie', 0, '1990-07-02', 'Australian actress known for Barbie.', 'Australia')
SET IDENTITY_INSERT [dbo].[Stars] OFF
GO
