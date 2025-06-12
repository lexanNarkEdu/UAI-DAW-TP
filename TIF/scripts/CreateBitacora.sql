-- ========================================
-- SCRIPT SIMPLIFICADO - SOLO TABLA EVENTO
-- ========================================

-- 1. Eliminar tabla Bitacora existente (con sus constraints)
IF OBJECT_ID('dbo.Bitacora', 'U') IS NOT NULL
BEGIN
    -- Eliminar Foreign Keys
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Bitacora_Evento')
        ALTER TABLE [dbo].[Bitacora] DROP CONSTRAINT [FK_Bitacora_Evento]
    
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Bitacora_Usuario')
        ALTER TABLE [dbo].[Bitacora] DROP CONSTRAINT [FK_Bitacora_Usuario]
    
    -- Eliminar tabla
    DROP TABLE [dbo].[Bitacora]
    PRINT 'Tabla Bitacora eliminada'
END
GO

-- 2. Eliminar tabla Evento existente (con sus constraints) 
IF OBJECT_ID('dbo.Evento', 'U') IS NOT NULL
BEGIN
    -- Eliminar constraint único si existe
    IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'UK_Eventos_Nombre')
        ALTER TABLE [dbo].[Evento] DROP CONSTRAINT [UK_Eventos_Nombre]
    
    -- Eliminar tabla
    DROP TABLE [dbo].[Evento]
    PRINT 'Tabla Evento eliminada'
END
GO

-- 3. Eliminar tabla intermedia si existe (por si se ejecutó el script anterior)
IF OBJECT_ID('dbo.Bitacora_Evento', 'U') IS NOT NULL
BEGIN
    -- Eliminar Foreign Keys primero
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Bitacora_Evento_Bitacora')
        ALTER TABLE [dbo].[Bitacora_Evento] DROP CONSTRAINT [FK_Bitacora_Evento_Bitacora]
    
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Bitacora_Evento_Evento')
        ALTER TABLE [dbo].[Bitacora_Evento] DROP CONSTRAINT [FK_Bitacora_Evento_Evento]
    
    -- Eliminar tabla
    DROP TABLE [dbo].[Bitacora_Evento]
    PRINT 'Tabla Bitacora_Evento eliminada'
END
GO

-- ========================================
-- CREAR TABLA EVENTO SIMPLIFICADA
-- ========================================

-- 4. Crear tabla Evento como LOG/BITÁCORA único
CREATE TABLE [dbo].[Evento](
    [evento_id] [int] IDENTITY(1,1) NOT NULL,
    [evento_usuario_username] [varchar](250) NOT NULL,
    [evento_nombre] [varchar](100) NOT NULL,
    [evento_descripcion] [varchar](500) NOT NULL,
    [evento_criticidad_id] [int] NOT NULL,
    [evento_fecha_hora] [datetime] NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_Evento] PRIMARY KEY CLUSTERED ([evento_id] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
              ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- ========================================
-- AGREGAR CONSTRAINTS Y FOREIGN KEYS
-- ========================================

-- 5. Agregar Foreign Key hacia Usuario
ALTER TABLE [dbo].[Evento] WITH CHECK ADD CONSTRAINT [FK_Evento_Usuario] 
    FOREIGN KEY([evento_usuario_username]) REFERENCES [dbo].[Usuario] ([usuario_username])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Usuario]
GO

-- 6. Agregar índice para consultas por usuario y fecha
CREATE NONCLUSTERED INDEX [IX_Evento_Usuario_Fecha] ON [dbo].[Evento]
(
    [evento_usuario_username] ASC,
    [evento_fecha_hora] DESC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, 
      DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
      OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- 7. Agregar índice para consultas por criticidad
CREATE NONCLUSTERED INDEX [IX_Evento_Criticidad_Fecha] ON [dbo].[Evento]
(
    [evento_criticidad_id] ASC,
    [evento_fecha_hora] DESC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, 
      DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
      OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- ========================================
-- DATOS DE PRUEBA (opcional)
-- ========================================

-- 8. Insertar algunos eventos de ejemplo
INSERT INTO [dbo].[Evento] 
    ([evento_usuario_username], [evento_nombre], [evento_descripcion], [evento_criticidad_id])
VALUES 
    ('YRcNPjkiPnm+2OHrdGNdsw==', 'Login Exitoso', 'Usuario ingresó al sistema correctamente', 1),
    ('YRcNPjkiPnm+2OHrdGNdsw==', 'Consulta Datos', 'Usuario consultó información de clientes', 2),
    ('nta7Md4FXkpfZmzbQMBFwg==', 'Login Fallido', 'Intento de login con credenciales incorrectas', 3),
    ('YRcNPjkiPnm+2OHrdGNdsw==', 'Logout', 'Usuario cerró sesión normalmente', 1)
GO

-- ========================================
-- VERIFICACIÓN FINAL
-- ========================================
PRINT ''
PRINT '========================================='
PRINT 'SIMPLIFICACIÓN COMPLETADA EXITOSAMENTE'
PRINT '========================================='
PRINT 'Estructura final:'
PRINT '- Una sola tabla: Evento'
PRINT '- Funciona como bitácora/log completo'
PRINT '- Relación directa: Usuario (1) -> Evento (N)'
PRINT ''
PRINT 'Campos principales:'
PRINT '- evento_id: Identificador único'
PRINT '- evento_usuario_username: FK hacia Usuario'
PRINT '- evento_nombre: Tipo de evento'
PRINT '- evento_descripcion: Detalle del evento'
PRINT '- evento_criticidad_id: Nivel de importancia'
PRINT '- evento_fecha_hora: Timestamp automático'
PRINT ''
PRINT 'Índices creados para optimizar consultas por:'
PRINT '- Usuario + Fecha'
PRINT '- Criticidad + Fecha'
PRINT '========================================='

-- Mostrar registros insertados
SELECT 
    evento_id,
    evento_usuario_username,
    evento_nombre,
    evento_descripcion,
    evento_criticidad_id,
    evento_fecha_hora
FROM [dbo].[Evento]
ORDER BY evento_fecha_hora DESC