USE [master]
GO
/****** Object:  Database [Atestados]    Script Date: 2022-06-10 00:41:01 ******/
CREATE DATABASE [Atestados]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Atestados', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Atestados.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Atestados_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Atestados_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Atestados] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Atestados].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Atestados] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Atestados] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Atestados] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Atestados] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Atestados] SET ARITHABORT OFF 
GO
ALTER DATABASE [Atestados] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Atestados] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Atestados] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Atestados] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Atestados] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Atestados] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Atestados] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Atestados] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Atestados] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Atestados] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Atestados] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Atestados] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Atestados] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Atestados] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Atestados] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Atestados] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Atestados] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Atestados] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Atestados] SET  MULTI_USER 
GO
ALTER DATABASE [Atestados] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Atestados] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Atestados] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Atestados] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Atestados] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Atestados] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Atestados] SET QUERY_STORE = OFF
GO
USE [Atestados]
GO
/****** Object:  Table [dbo].[Archivo]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Archivo](
	[ArchivoID] [int] IDENTITY(1,1) NOT NULL,
	[Obligatorio] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Datos] [varbinary](max) NOT NULL,
	[TipoArchivo] [nvarchar](200) NOT NULL,
	[AtestadoID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ArchivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Atestado]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Atestado](
	[AtestadoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](250) NOT NULL,
	[NumeroAutores] [int] NOT NULL,
	[Observaciones] [varchar](1000) NULL,
	[Codigo] [varchar](100) NULL,
	[HoraCreacion] [datetime] NOT NULL,
	[Enviado] [int] NULL,
	[Descargado] [int] NULL,
	[CantidadHoras] [int] NULL,
	[Lugar] [varchar](250) NULL,
	[CatalogoTipo] [varchar](100) NULL,
	[Enlace] [varchar](250) NULL,
	[PaisID] [int] NOT NULL,
	[PersonaID] [int] NOT NULL,
	[RubroID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AtestadoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AtestadoXPersona]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AtestadoXPersona](
	[AtestadoID] [int] NOT NULL,
	[PersonaID] [int] NOT NULL,
	[Porcentaje] [float] NULL,
	[Departamento] [varchar](250) NULL,
 CONSTRAINT [PK_AtestadoXPersona] PRIMARY KEY CLUSTERED 
(
	[AtestadoID] ASC,
	[PersonaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DominioIdioma]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominioIdioma](
	[DominioIdiomaID] [int] NOT NULL,
	[IdiomaID] [int] NULL,
	[Lectura] [int] NOT NULL,
	[Escrito] [int] NOT NULL,
	[Auditiva] [int] NOT NULL,
	[Oral] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DominioIdiomaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EvaluaciónXAtestado]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EvaluaciónXAtestado](
	[AtestadoID] [int] NOT NULL,
	[PersonaID] [int] NOT NULL,
	[AutorID] [int] NOT NULL,
	[PorcentajeObtenido] [float] NOT NULL,
	[Observaciones] [varchar](max) NOT NULL,
 CONSTRAINT [PK_EvaluaciónXAtestado] PRIMARY KEY CLUSTERED 
(
	[AtestadoID] ASC,
	[PersonaID] ASC,
	[AutorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fecha]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fecha](
	[FechaID] [int] NOT NULL,
	[FechaInicio] [date] NULL,
	[FechaFinal] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[FechaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Idioma]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idioma](
	[IdiomaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdiomaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InfoEditorial]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoEditorial](
	[InfoEditorialID] [int] NOT NULL,
	[Editorial] [varchar](100) NOT NULL,
	[Website] [varchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InfoEditorialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pais]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pais](
	[PaisID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[PersonaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[PrimerApellido] [varchar](50) NOT NULL,
	[SegundoApellido] [varchar](50) NULL,
	[Email] [varchar](100) NOT NULL,
	[CategoriaActual] [int] NOT NULL,
	[TipoUsuario] [int] NOT NULL,
	[Telefono] [int] NULL,
	[esActivo] [bit] NOT NULL,
	[TiempoServido] [int] NULL,
 CONSTRAINT [PK__Persona__7C34D323ECC1D28A] PRIMARY KEY CLUSTERED 
(
	[PersonaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rubro]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rubro](
	[RubroID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](1000) NULL,
	[TipoRubro] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RubroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoCategoria]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCategoria](
	[TipoCategoriaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Academicos] [varchar](50) NOT NULL,
	[Administrativos] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TipoCategoria] PRIMARY KEY CLUSTERED 
(
	[TipoCategoriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoRubro]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoRubro](
	[TipoRubroID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoRubroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoUsuario]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoUsuario](
	[TipoUsuarioID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TipoUsuario] PRIMARY KEY CLUSTERED 
(
	[TipoUsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 2022-06-10 00:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[UsuarioID] [int] NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Contrasena] [varchar](200) NOT NULL,
	[esActivo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Persona] ADD  CONSTRAINT [DF_Persona_esActivo]  DEFAULT ((1)) FOR [esActivo]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_esActivo]  DEFAULT ((1)) FOR [esActivo]
GO
ALTER TABLE [dbo].[Archivo]  WITH CHECK ADD  CONSTRAINT [FK_Archivo_AtestadoID] FOREIGN KEY([AtestadoID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Archivo] CHECK CONSTRAINT [FK_Archivo_AtestadoID]
GO
ALTER TABLE [dbo].[Atestado]  WITH CHECK ADD FOREIGN KEY([PaisID])
REFERENCES [dbo].[Pais] ([PaisID])
GO
ALTER TABLE [dbo].[Atestado]  WITH CHECK ADD  CONSTRAINT [FK__Atestado__Person__4316F928] FOREIGN KEY([PersonaID])
REFERENCES [dbo].[Persona] ([PersonaID])
GO
ALTER TABLE [dbo].[Atestado] CHECK CONSTRAINT [FK__Atestado__Person__4316F928]
GO
ALTER TABLE [dbo].[Atestado]  WITH CHECK ADD FOREIGN KEY([RubroID])
REFERENCES [dbo].[Rubro] ([RubroID])
GO
ALTER TABLE [dbo].[AtestadoXPersona]  WITH CHECK ADD  CONSTRAINT [FK_AtestadoXPersona_AtestadoID] FOREIGN KEY([AtestadoID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AtestadoXPersona] CHECK CONSTRAINT [FK_AtestadoXPersona_AtestadoID]
GO
ALTER TABLE [dbo].[AtestadoXPersona]  WITH CHECK ADD  CONSTRAINT [FK_AtestadoXPersona_PersonaID] FOREIGN KEY([PersonaID])
REFERENCES [dbo].[Persona] ([PersonaID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AtestadoXPersona] CHECK CONSTRAINT [FK_AtestadoXPersona_PersonaID]
GO
ALTER TABLE [dbo].[DominioIdioma]  WITH CHECK ADD FOREIGN KEY([IdiomaID])
REFERENCES [dbo].[Idioma] ([IdiomaID])
GO
ALTER TABLE [dbo].[DominioIdioma]  WITH CHECK ADD  CONSTRAINT [FK_DominioIdioma_AtestadoID] FOREIGN KEY([DominioIdiomaID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DominioIdioma] CHECK CONSTRAINT [FK_DominioIdioma_AtestadoID]
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado]  WITH CHECK ADD  CONSTRAINT [FK_EvaluaciónXAtestado_Atestado] FOREIGN KEY([AtestadoID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado] CHECK CONSTRAINT [FK_EvaluaciónXAtestado_Atestado]
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado]  WITH CHECK ADD  CONSTRAINT [FK_EvaluaciónXAtestado_Persona] FOREIGN KEY([PersonaID])
REFERENCES [dbo].[Persona] ([PersonaID])
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado] CHECK CONSTRAINT [FK_EvaluaciónXAtestado_Persona]
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado]  WITH CHECK ADD  CONSTRAINT [FK_EvaluaciónXAtestado_Persona1] FOREIGN KEY([AutorID])
REFERENCES [dbo].[Persona] ([PersonaID])
GO
ALTER TABLE [dbo].[EvaluaciónXAtestado] CHECK CONSTRAINT [FK_EvaluaciónXAtestado_Persona1]
GO
ALTER TABLE [dbo].[Fecha]  WITH CHECK ADD  CONSTRAINT [FK_Fecha_AtestadoID] FOREIGN KEY([FechaID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fecha] CHECK CONSTRAINT [FK_Fecha_AtestadoID]
GO
ALTER TABLE [dbo].[InfoEditorial]  WITH CHECK ADD  CONSTRAINT [FK_InfoEditorial_AtestadoID] FOREIGN KEY([InfoEditorialID])
REFERENCES [dbo].[Atestado] ([AtestadoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InfoEditorial] CHECK CONSTRAINT [FK_InfoEditorial_AtestadoID]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_TipoCategoria] FOREIGN KEY([CategoriaActual])
REFERENCES [dbo].[TipoCategoria] ([TipoCategoriaID])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_TipoCategoria]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_TipoUsuario] FOREIGN KEY([TipoUsuario])
REFERENCES [dbo].[TipoUsuario] ([TipoUsuarioID])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_TipoUsuario]
GO
ALTER TABLE [dbo].[Rubro]  WITH CHECK ADD  CONSTRAINT [Fk_TipoRubro] FOREIGN KEY([TipoRubro])
REFERENCES [dbo].[TipoRubro] ([TipoRubroID])
GO
ALTER TABLE [dbo].[Rubro] CHECK CONSTRAINT [Fk_TipoRubro]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK__Usuario__Usuario__38996AB5] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[Persona] ([PersonaID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__Usuario__38996AB5]
GO
USE [master]
GO
ALTER DATABASE [Atestados] SET  READ_WRITE 
GO
