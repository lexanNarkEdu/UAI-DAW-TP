USE [DAW_DB]
GO
/****** Object:  User [admin]    Script Date: 5/28/2025 8:00:25 PM ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [admin]
GO
/****** Object:  Table [dbo].[Permiso]    Script Date: 5/28/2025 8:00:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[permiso_id] [int] IDENTITY(1,1) NOT NULL,
	[permiso_nombre] [varchar](50) NULL,
	[permiso_tipo] [varchar](50) NULL,
 CONSTRAINT [PK_Permiso] PRIMARY KEY CLUSTERED 
(
	[permiso_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permiso_Permiso]    Script Date: 5/28/2025 8:00:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso_Permiso](
	[permisopadre_id] [int] NULL,
	[permisohijo_id] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 5/28/2025 8:00:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[usuario_username] [varchar](250) NOT NULL,
	[usuario_nombre] [varchar](250) NOT NULL,
	[usuario_apellido] [varchar](250) NOT NULL,
	[usuario_dni] [varchar](250) NOT NULL,
	[usuario_domicilio] [varchar](250) NULL,
	[usuario_email] [varchar](250) NOT NULL,
	[usuario_password] [varchar](250) NOT NULL,
	[usuario_bloqueado] [bit] NOT NULL,
	[usuario_fallos_autenticacion_consecutivos] [int] NOT NULL,
	[usuario_fecha_creacion] [datetime] NOT NULL,
	[usuario_verificador_horizontal] [int] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[usuario_username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario_Permiso]    Script Date: 5/28/2025 8:00:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario_Permiso](
	[usuario_username] [varchar](250) NULL,
	[permiso_id] [int] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Usuario] ([usuario_username], [usuario_nombre], [usuario_apellido], [usuario_dni], [usuario_domicilio], [usuario_email], [usuario_password], [usuario_bloqueado], [usuario_fallos_autenticacion_consecutivos], [usuario_fecha_creacion], [usuario_verificador_horizontal]) VALUES (N'nta7Md4FXkpfZmzbQMBFwg==', N'El usuario', N'De prueba', N'87654321', N'Calle prueba 123', N'prueba@pruebamail.com', N'c893bad68927b457dbed39460e6afd62', 0, 0, CAST(N'2025-05-26T23:58:32.073' AS DateTime), 2)
INSERT [dbo].[Usuario] ([usuario_username], [usuario_nombre], [usuario_apellido], [usuario_dni], [usuario_domicilio], [usuario_email], [usuario_password], [usuario_bloqueado], [usuario_fallos_autenticacion_consecutivos], [usuario_fecha_creacion], [usuario_verificador_horizontal]) VALUES (N'YRcNPjkiPnm+2OHrdGNdsw==', N'Pepe', N'Peposo', N'12345678', N'Calle pepe 123', N'pepe@pepemail.com', N'1c1f70d4529856884d240d200887b9ae', 0, 0, CAST(N'2025-05-23T23:55:53.647' AS DateTime), 1)
GO
ALTER TABLE [dbo].[Permiso_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_Permiso_Permiso] FOREIGN KEY([permisopadre_id])
REFERENCES [dbo].[Permiso] ([permiso_id])
GO
ALTER TABLE [dbo].[Permiso_Permiso] CHECK CONSTRAINT [FK_Permiso_Permiso_Permiso]
GO
ALTER TABLE [dbo].[Permiso_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_Permiso_Permiso1] FOREIGN KEY([permisohijo_id])
REFERENCES [dbo].[Permiso] ([permiso_id])
GO
ALTER TABLE [dbo].[Permiso_Permiso] CHECK CONSTRAINT [FK_Permiso_Permiso_Permiso1]
GO
ALTER TABLE [dbo].[Usuario_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Permiso_Permiso] FOREIGN KEY([permiso_id])
REFERENCES [dbo].[Permiso] ([permiso_id])
GO
ALTER TABLE [dbo].[Usuario_Permiso] CHECK CONSTRAINT [FK_Usuario_Permiso_Permiso]
GO
ALTER TABLE [dbo].[Usuario_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Permiso_Usuario] FOREIGN KEY([usuario_username])
REFERENCES [dbo].[Usuario] ([usuario_username])
GO
ALTER TABLE [dbo].[Usuario_Permiso] CHECK CONSTRAINT [FK_Usuario_Permiso_Usuario]
GO
