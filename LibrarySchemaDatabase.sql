USE [master]
GO
/****** Object:  Database [library]    Script Date: 12.10.2017 13:06:14 ******/
CREATE DATABASE [library]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'library', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\library.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'library_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\library_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [library] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [library].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [library] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [library] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [library] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [library] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [library] SET ARITHABORT OFF 
GO
ALTER DATABASE [library] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [library] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [library] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [library] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [library] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [library] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [library] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [library] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [library] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [library] SET  DISABLE_BROKER 
GO
ALTER DATABASE [library] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [library] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [library] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [library] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [library] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [library] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [library] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [library] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [library] SET  MULTI_USER 
GO
ALTER DATABASE [library] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [library] SET DB_CHAINING OFF 
GO
ALTER DATABASE [library] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [library] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [library] SET DELAYED_DURABILITY = DISABLED 
GO
USE [library]
GO
/****** Object:  Table [dbo].[Авторы]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Авторы](
	[Код] [int] IDENTITY(1,1) NOT NULL,
	[ФИО автора] [nvarchar](50) NOT NULL,
	[Краткая биография] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Код] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Книги]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Книги](
	[Код] [int] IDENTITY(1,1) NOT NULL,
	[Автор] [int] NOT NULL CONSTRAINT [DF_Книги_Код автора]  DEFAULT ((1)),
	[Название] [nvarchar](255) NOT NULL,
	[Серия] [nvarchar](255) NULL CONSTRAINT [DF_Книги_Серия]  DEFAULT ('Нет серии'),
	[Жанр] [nvarchar](255) NULL CONSTRAINT [DF_Книги_Жанр]  DEFAULT ('Неизвестен'),
	[Год выпуска] [int] NULL,
	[Краткое описание] [nvarchar](max) NULL CONSTRAINT [DF_Книги_Краткое описание]  DEFAULT ('Нету'),
	[Ссылка на скачивание] [nvarchar](max) NOT NULL CONSTRAINT [DF__Книги__Ссылка на__75A278F5]  DEFAULT ('Ссылки нет'),
	[Средний рейтинг] [real] NULL,
 CONSTRAINT [PK__Книги__AE76132E1AD612F3] PRIMARY KEY CLUSTERED 
(
	[Код] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Отзывы]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Отзывы](
	[Создатель] [nvarchar](31) NOT NULL,
	[Код книги] [int] NOT NULL,
	[Дата создания] [date] NOT NULL,
	[Содержание] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Создатель] ASC,
	[Код книги] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Пользователи]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Пользователи](
	[Имя] [nvarchar](31) NOT NULL,
	[Дата последнего входа] [date] NOT NULL,
	[Почта] [nvarchar](50) NOT NULL,
	[Роль] [nvarchar](15) NOT NULL CONSTRAINT [DF_Пользователи_Роль]  DEFAULT (N'Пользователь'),
	[Пароль] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK__Пользова__A93D06D99A4DE988] PRIMARY KEY CLUSTERED 
(
	[Имя] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Пользователь_Книга]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Пользователь_Книга](
	[Пользователь] [nvarchar](31) NOT NULL,
	[Код книги] [int] NOT NULL,
	[Статус] [nvarchar](31) NOT NULL CONSTRAINT [DF__Пользоват__Стату__4316F928]  DEFAULT ('Заинтересовала'),
	[Рейтинг] [smallint] NULL,
 CONSTRAINT [PK__Пользова__F4FB2D80926A3569] PRIMARY KEY CLUSTERED 
(
	[Пользователь] ASC,
	[Код книги] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Сообщения]    Script Date: 12.10.2017 13:06:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Сообщения](
	[Код] [int] IDENTITY(1,1) NOT NULL,
	[Отправитель] [nvarchar](31) NULL,
	[Получатель] [nvarchar](31) NOT NULL,
	[Дата отправки] [datetime] NOT NULL,
	[Содержание] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__Сообщени__AE76132EEF6EA317] PRIMARY KEY CLUSTERED 
(
	[Код] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Книги]  WITH CHECK ADD  CONSTRAINT [FK__Книги__Код автор__00200768] FOREIGN KEY([Автор])
REFERENCES [dbo].[Авторы] ([Код])
ON UPDATE CASCADE
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[Книги] CHECK CONSTRAINT [FK__Книги__Код автор__00200768]
GO
ALTER TABLE [dbo].[Отзывы]  WITH CHECK ADD  CONSTRAINT [FK_Отзывы_Книги] FOREIGN KEY([Код книги])
REFERENCES [dbo].[Книги] ([Код])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Отзывы] CHECK CONSTRAINT [FK_Отзывы_Книги]
GO
ALTER TABLE [dbo].[Отзывы]  WITH CHECK ADD  CONSTRAINT [FK_Отзывы_Пользователи] FOREIGN KEY([Создатель])
REFERENCES [dbo].[Пользователи] ([Имя])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Отзывы] CHECK CONSTRAINT [FK_Отзывы_Пользователи]
GO
ALTER TABLE [dbo].[Пользователь_Книга]  WITH CHECK ADD  CONSTRAINT [FK__Пользоват__Код к__48CFD27E] FOREIGN KEY([Код книги])
REFERENCES [dbo].[Книги] ([Код])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Пользователь_Книга] CHECK CONSTRAINT [FK__Пользоват__Код к__48CFD27E]
GO
ALTER TABLE [dbo].[Пользователь_Книга]  WITH CHECK ADD  CONSTRAINT [FK__Пользоват__Польз__47DBAE45] FOREIGN KEY([Пользователь])
REFERENCES [dbo].[Пользователи] ([Имя])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Пользователь_Книга] CHECK CONSTRAINT [FK__Пользоват__Польз__47DBAE45]
GO
ALTER TABLE [dbo].[Сообщения]  WITH CHECK ADD  CONSTRAINT [FK__Сообщения__Отпра__4AB81AF0] FOREIGN KEY([Отправитель])
REFERENCES [dbo].[Пользователи] ([Имя])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Сообщения] CHECK CONSTRAINT [FK__Сообщения__Отпра__4AB81AF0]
GO
ALTER TABLE [dbo].[Сообщения]  WITH CHECK ADD  CONSTRAINT [FK__Сообщения__Получ__4BAC3F29] FOREIGN KEY([Получатель])
REFERENCES [dbo].[Пользователи] ([Имя])
GO
ALTER TABLE [dbo].[Сообщения] CHECK CONSTRAINT [FK__Сообщения__Получ__4BAC3F29]
GO
ALTER TABLE [dbo].[Книги]  WITH CHECK ADD  CONSTRAINT [CH_Книги_Рейт] CHECK  (([Средний рейтинг]>=(0) AND [Средний рейтинг]<=(5) OR [Средний рейтинг]=NULL))
GO
ALTER TABLE [dbo].[Книги] CHECK CONSTRAINT [CH_Книги_Рейт]
GO
ALTER TABLE [dbo].[Пользователи]  WITH CHECK ADD  CONSTRAINT [CK__Пользоват__Почта__35BCFE0A] CHECK  (([Почта] like '_%@_%._%'))
GO
ALTER TABLE [dbo].[Пользователи] CHECK CONSTRAINT [CK__Пользоват__Почта__35BCFE0A]
GO
ALTER TABLE [dbo].[Пользователь_Книга]  WITH CHECK ADD  CONSTRAINT [CK__Пользоват__Рейти__440B1D61] CHECK  (([Рейтинг]>=(0) AND [Рейтинг]<=(5) OR [Рейтинг]=NULL))
GO
ALTER TABLE [dbo].[Пользователь_Книга] CHECK CONSTRAINT [CK__Пользоват__Рейти__440B1D61]
GO
USE [master]
GO
ALTER DATABASE [library] SET  READ_WRITE 
GO
