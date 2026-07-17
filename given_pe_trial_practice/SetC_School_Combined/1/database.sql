USE [master]
GO
CREATE DATABASE [PE_Practice_SchoolC]
GO
USE [PE_Practice_SchoolC]
GO

CREATE TABLE [dbo].[Teachers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](40) NOT NULL,
	[Male] [bit] NOT NULL,
	[Dob] [date] NOT NULL,
	[Nationality] [varchar](30) NOT NULL,
	[Description] [ntext] NOT NULL,
	CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Courses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[StartDate] [date] NULL,
	[Description] [text] NULL,
	[Language] [varchar](30) NOT NULL,
	[DepartmentId] [int] NULL,
	[TeacherId] [int] NULL,
	CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Assistants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[Male] [bit] NULL,
	[Dob] [date] NULL,
	[Description] [text] NULL,
	[Nationality] [varchar](30) NULL,
	CONSTRAINT [PK_Assistants] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nchar](10) NOT NULL,
	CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Course_Tag](
	[CourseId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
	CONSTRAINT [PK_Course_Tag] PRIMARY KEY CLUSTERED ([CourseId] ASC, [TagId] ASC)
)
GO

CREATE TABLE [dbo].[Course_Assistant](
	[CourseId] [int] NOT NULL,
	[AssistantId] [int] NOT NULL,
	CONSTRAINT [PK_Course_Assistant] PRIMARY KEY CLUSTERED ([CourseId] ASC, [AssistantId] ASC)
)
GO

ALTER TABLE [dbo].[Courses] WITH CHECK ADD CONSTRAINT [FK_Courses_Teachers] FOREIGN KEY([TeacherId]) REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Courses] WITH CHECK ADD CONSTRAINT [FK_Courses_Departments] FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Course_Tag] WITH CHECK ADD CONSTRAINT [FK_Course_Tag_Courses] FOREIGN KEY([CourseId]) REFERENCES [dbo].[Courses] ([Id])
GO
ALTER TABLE [dbo].[Course_Tag] WITH CHECK ADD CONSTRAINT [FK_Course_Tag_Tags] FOREIGN KEY([TagId]) REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[Course_Assistant] WITH CHECK ADD CONSTRAINT [FK_Course_Assistant_Courses] FOREIGN KEY([CourseId]) REFERENCES [dbo].[Courses] ([Id])
GO
ALTER TABLE [dbo].[Course_Assistant] WITH CHECK ADD CONSTRAINT [FK_Course_Assistant_Assistants] FOREIGN KEY([AssistantId]) REFERENCES [dbo].[Assistants] ([Id])
GO

-- Seed data
SET IDENTITY_INSERT [dbo].[Teachers] ON
INSERT [dbo].[Teachers] ([Id],[FullName],[Male],[Dob],[Nationality],[Description]) VALUES
(1, 'Richard Feynman', 1, '1918-05-11', 'USA', 'Theoretical physicist known for work in quantum electrodynamics.'),
(2, 'Marie Curie', 0, '1867-11-07', 'Poland', 'Physicist and chemist, pioneer of radioactivity research.'),
(3, 'Alan Turing', 1, '1912-06-23', 'England', 'Mathematician and computer scientist, father of theoretical computer science.'),
(4, 'Ada Lovelace', 0, '1815-12-10', 'England', 'Mathematician, wrote the first algorithm for a computing machine.'),
(5, 'Carl Sagan', 1, '1934-11-09', 'USA', 'Astronomer and science communicator.')
SET IDENTITY_INSERT [dbo].[Teachers] OFF
GO

SET IDENTITY_INSERT [dbo].[Departments] ON
INSERT [dbo].[Departments] ([Id],[Name]) VALUES
(1, 'Physics'), (2, 'Chemistry'), (3, 'Computer Science'), (4, 'Mathematics'), (5, 'Astronomy')
SET IDENTITY_INSERT [dbo].[Departments] OFF
GO

SET IDENTITY_INSERT [dbo].[Courses] ON
INSERT [dbo].[Courses] ([Id],[Title],[StartDate],[Description],[Language],[DepartmentId],[TeacherId]) VALUES
(1, 'Quantum Mechanics 101', '2024-09-01', 'Introduction to quantum theory.', 'English', 1, 1),
(2, 'Radioactivity Fundamentals', '2024-09-05', 'Basics of radioactive decay and applications.', 'English', 2, 2),
(3, 'Introduction to Computing', '2024-09-10', 'History and theory of computation.', 'English', 3, 3),
(4, 'Algorithms and Machines', '2025-01-15', 'Turing machines and algorithmic thinking.', 'English', 3, 3),
(5, 'Analytical Engine Programming', '2024-09-12', 'Programming concepts using historical machines.', 'English', 4, 4),
(6, 'Cosmos and Beyond', '2024-09-20', 'A tour of the universe.', 'English', 5, 5)
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO

SET IDENTITY_INSERT [dbo].[Tags] ON
INSERT [dbo].[Tags] ([Id],[Title]) VALUES
(1, 'Science'), (2, 'Theory'), (3, 'Lab'), (4, 'History'), (5, 'Advanced')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
