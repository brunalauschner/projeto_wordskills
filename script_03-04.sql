USE [master]
GO
/****** Object:  Database [projeto_ws]    Script Date: 03/04/2020 21:38:19 ******/
CREATE DATABASE [projeto_ws]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'projeto_ws', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\projeto_ws.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'projeto_ws_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\projeto_ws_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [projeto_ws] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [projeto_ws].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [projeto_ws] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [projeto_ws] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [projeto_ws] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [projeto_ws] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [projeto_ws] SET ARITHABORT OFF 
GO
ALTER DATABASE [projeto_ws] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [projeto_ws] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [projeto_ws] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [projeto_ws] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [projeto_ws] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [projeto_ws] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [projeto_ws] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [projeto_ws] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [projeto_ws] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [projeto_ws] SET  DISABLE_BROKER 
GO
ALTER DATABASE [projeto_ws] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [projeto_ws] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [projeto_ws] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [projeto_ws] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [projeto_ws] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [projeto_ws] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [projeto_ws] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [projeto_ws] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [projeto_ws] SET  MULTI_USER 
GO
ALTER DATABASE [projeto_ws] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [projeto_ws] SET DB_CHAINING OFF 
GO
ALTER DATABASE [projeto_ws] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [projeto_ws] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [projeto_ws] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [projeto_ws] SET QUERY_STORE = OFF
GO
USE [projeto_ws]
GO
/****** Object:  Table [dbo].[cidade]    Script Date: 03/04/2020 21:38:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cidade](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NOT NULL,
	[id_estadoss] [int] NOT NULL,
 CONSTRAINT [PK_cidade] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[estados]    Script Date: 03/04/2020 21:38:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[estados](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NOT NULL,
	[id_paiss] [int] NOT NULL,
 CONSTRAINT [PK_estados] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[paises]    Script Date: 03/04/2020 21:38:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[paises](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_paises] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuario]    Script Date: 03/04/2020 21:38:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NOT NULL,
	[dataDeNascimento] [varchar](50) NOT NULL,
	[foto] [image] NOT NULL,
	[telefone] [varchar](50) NOT NULL,
	[celular] [varchar](50) NULL,
	[email] [varchar](50) NOT NULL,
	[senha] [varchar](50) NOT NULL,
	[cep] [varchar](50) NOT NULL,
	[id_pais] [int] NOT NULL,
	[id_estado] [int] NOT NULL,
	[id_cidade] [int] NOT NULL,
	[bairro] [varchar](50) NOT NULL,
	[endereco] [varchar](50) NOT NULL,
	[administrador] [bit] NOT NULL,
 CONSTRAINT [PK_usuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[cidade]  WITH CHECK ADD  CONSTRAINT [FK_cidade_estados] FOREIGN KEY([id_estadoss])
REFERENCES [dbo].[estados] ([id])
GO
ALTER TABLE [dbo].[cidade] CHECK CONSTRAINT [FK_cidade_estados]
GO
ALTER TABLE [dbo].[estados]  WITH CHECK ADD  CONSTRAINT [FK_estados_paises] FOREIGN KEY([id_paiss])
REFERENCES [dbo].[paises] ([id])
GO
ALTER TABLE [dbo].[estados] CHECK CONSTRAINT [FK_estados_paises]
GO
ALTER TABLE [dbo].[usuario]  WITH CHECK ADD  CONSTRAINT [FK_usuario_cidade] FOREIGN KEY([id_cidade])
REFERENCES [dbo].[cidade] ([id])
GO
ALTER TABLE [dbo].[usuario] CHECK CONSTRAINT [FK_usuario_cidade]
GO
ALTER TABLE [dbo].[usuario]  WITH CHECK ADD  CONSTRAINT [FK_usuario_estados] FOREIGN KEY([id_estado])
REFERENCES [dbo].[estados] ([id])
GO
ALTER TABLE [dbo].[usuario] CHECK CONSTRAINT [FK_usuario_estados]
GO
ALTER TABLE [dbo].[usuario]  WITH CHECK ADD  CONSTRAINT [FK_usuario_paises] FOREIGN KEY([id_pais])
REFERENCES [dbo].[paises] ([id])
GO
ALTER TABLE [dbo].[usuario] CHECK CONSTRAINT [FK_usuario_paises]
GO
USE [master]
GO
ALTER DATABASE [projeto_ws] SET  READ_WRITE 
GO
