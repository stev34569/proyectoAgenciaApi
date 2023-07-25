use proyectoAgencia




CREATE TABLE [dbo].[Bitacora](
	[IdBitacora] [bigint] IDENTITY(1,1) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Origen] [varchar](255) NOT NULL,
	[Mensaje] [varchar](5000) NOT NULL,
	[IdUsuario] [bigint] NOT NULL,
	[DireccionIP] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED 
(
	[IdBitacora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Rol](
	[IdRol] [tinyint] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [bigint] IDENTITY(1,1) NOT NULL,
	[CorreoElectronico] [varchar](50) NOT NULL,
	[Contrasenna] [varchar](8000) NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Estado] [bit] NOT NULL,
	[IdRol] [tinyint] NOT NULL,
	[ContrasennaTemp] [bit] NULL,
	[ContrasennaVenc] [datetime] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Rol] ON 
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol]) VALUES (1, N'Usuario')
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol]) VALUES (2, N'Administrador')
GO
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO

SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
INSERT [dbo].[Usuario] ([IdUsuario], [CorreoElectronico], [Contrasenna], [Identificacion], [Nombre], [Estado], [IdRol], [ContrasennaTemp], [ContrasennaVenc]) VALUES (8, N'sbansbach60414@ufide.ac.cr', N'pmawcx6LHLs=', N'504160414', N'Steven Bansbach Obando', 1, 1, 0, CAST(N'2023-07-06T20:40:28.730' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO

ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [AK_CorreoElectronico] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bitacora]  WITH CHECK ADD  CONSTRAINT [FK_Bitacora_Bitacora] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Bitacora] CHECK CONSTRAINT [FK_Bitacora_Bitacora]
GO

ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

CREATE PROCEDURE [dbo].[ACTUALIZAR_CLAVE]
	@IdUsuario		BIGINT,
	@Contrasenna	VARCHAR(8000)
AS
BEGIN

	UPDATE dbo.Usuario
	SET	   Contrasenna = @Contrasenna,
		   ContrasennaTemp = 0
	WHERE IdUsuario = @IdUsuario

END
GO

CREATE PROCEDURE [dbo].[ACTUALIZAR_CLAVE_TEMP]
	@IdUsuario			BIGINT,
	@ClaveTempCifrada	VARCHAR(8000)
AS
BEGIN

	UPDATE dbo.Usuario
	SET	   Contrasenna = @ClaveTempCifrada,
		   ContrasennaTemp = 1,
		   ContrasennaVenc = DATEADD(minute, 15, GETDATE())
	WHERE IdUsuario = @IdUsuario

END
GO

CREATE PROCEDURE [dbo].[CONSULTAR_USUARIOS] 

AS
BEGIN

	SELECT IdUsuario,
		   CorreoElectronico,
		   Identificacion,
		   Nombre,
		   Estado,
		   IdRol
	  FROM dbo.Usuario
	  WHERE Estado = 1

END
GO

CREATE PROCEDURE [dbo].[REGISTRAR_USUARIO]
	@CorreoElectronico	VARCHAR(50),
	@Contrasenna		VARCHAR(8000),
	@Identificacion		VARCHAR(20),
	@Nombre				VARCHAR(100)
AS
BEGIN

	IF NOT EXISTS(SELECT 1 FROM Usuario 
				  WHERE CorreoElectronico = @CorreoElectronico
					OR	Identificacion	  = @Identificacion)
	BEGIN

		DECLARE @Estado BIT,
				@Rol	TINYINT

		SELECT	@Rol	= IdRol FROM ROL WHERE NombreRol = 'Usuario' 
		SET		@Estado = 1 

		INSERT INTO dbo.Usuario(CorreoElectronico,Contrasenna,Identificacion,Nombre,Estado,IdRol)
		VALUES (@CorreoElectronico,@Contrasenna,@Identificacion,@Nombre,@Estado,@Rol)

	END

END
GO

CREATE PROCEDURE [dbo].[VALIDAR_CORREO] 
	@CorreoElectronico	VARCHAR(50)
AS
BEGIN

	SELECT IdUsuario,
		   CorreoElectronico,
		   Identificacion,
		   Nombre,
		   Estado,
		   IdRol
	  FROM dbo.Usuario
	  WHERE CorreoElectronico = @CorreoElectronico
	  AND	Estado = 1

END
GO

CREATE PROCEDURE [dbo].[VALIDAR_USUARIO] 
	@CorreoElectronico	VARCHAR(50),
	@Contrasenna		VARCHAR(8000)
AS
BEGIN

	SELECT IdUsuario,
		   CorreoElectronico,
		   Identificacion,
		   Nombre,
		   Estado,
		   IdRol,
		   ContrasennaTemp,
		   ContrasennaVenc
	  FROM dbo.Usuario
	  WHERE CorreoElectronico = @CorreoElectronico
	  AND	Contrasenna = @Contrasenna
	  AND	Estado = 1

END
GO

select * from usuario
