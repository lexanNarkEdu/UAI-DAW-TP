WITH recursivo AS (
    SELECT PP2.permisopadre_id, PP2.permisohijo_id FROM Permiso_Permiso PP2
    --WHERE PP2.permisopadre_id = 1 ac� se va variando la familia que busco
    UNION ALL 
    SELECT PP1.permisopadre_id, PP1.permisohijo_id FROM Permiso_Permiso PP1 
    INNER JOIN recursivo rec ON rec.permisohijo_id = PP1.permisopadre_id
)
SELECT PE.permiso_nombre as familia, rec.permisopadre_id, rec.permisohijo_id, P.permiso_id, P.permiso_nombre, P.permiso_tipo
FROM recursivo rec 
INNER JOIN Permiso P ON rec.permisohijo_id = P.permiso_id
INNER JOIN Permiso PE ON rec.permisopadre_id = PE.permiso_id
ORDER BY rec.permisopadre_id;

/*
SELECT IS_SRVROLEMEMBER('sysadmin');
SELECT IS_MEMBER('db_owner'); 
*/

ALTER TABLE Producto
ADD 
    producto_es_banner BIT NOT NULL DEFAULT 0,         -- Si se muestra en el banner principal
    producto_es_promocionado BIT NOT NULL DEFAULT 0;   -- Si es parte de una promoci�n destacada


SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length,
    c.is_nullable,
    c.column_id
FROM 
    sys.columns c
    JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE 
    OBJECT_NAME(c.object_id) = 'Producto'
ORDER BY 
    c.column_id;


UPDATE [dbo].[Producto]
   SET
      [producto_path_banner] = 'assets/img/Teclado-TIF 2025-01_51_32.png'
      ,[producto_es_banner] = 1
      ,[producto_usuario_modificacion] = 'om/5FIz2NEtGc2l86fLj3w=='
 WHERE [producto_id] = 5 ;


/* ------------------------------------------------------------------ 
   --------------- STORE PROCEDURE DE LA HOME PAGE ------------------             
   ------------------------------------------------------------------ */
USE DAW_DB

GO

CREATE PROCEDURE sp_ObtenerProductosParaHome
    @cantidadBanner INT,
    @cantidadDestacados INT,
    @cantidadUltimosIngresos INT
AS
BEGIN
    SET NOCOUNT ON;
     -- Banner
    SELECT TOP (@cantidadBanner)
        p.producto_id,
        p.producto_nombre,
        p.producto_precio,
        p.producto_foto,
        p.producto_descripcion,
        p.producto_stock,
        p.categoria_id,
        c.categoria_nombre,
        p.producto_activo,
        p.condicion_id,
        p.producto_path_banner
    FROM Producto p
    INNER JOIN Categoria c ON p.categoria_id = c.categoria_id
    WHERE p.producto_activo = 1 AND p.producto_es_banner = 1
    ORDER BY p.producto_fecha_creacion DESC;

    -- Destacados
    SELECT TOP (@cantidadDestacados)
        p.producto_id,
        p.producto_nombre,
        p.producto_precio,
        p.producto_foto,
        p.producto_descripcion,
        p.producto_stock,
        p.categoria_id,
        c.categoria_nombre,
        p.condicion_id,
        p.producto_activo,
        p.producto_path_banner
    FROM Producto p
    INNER JOIN Categoria c ON p.categoria_id = c.categoria_id
    WHERE p.producto_activo = 1 AND p.producto_es_promocionado = 1
    ORDER BY p.producto_fecha_creacion DESC;

    -- �ltimos Ingresos
    SELECT TOP (@cantidadUltimosIngresos)
        p.producto_id,
        p.producto_nombre,
        p.producto_precio,
        p.producto_foto,
        p.producto_descripcion,
        p.producto_stock,
        p.categoria_id,
        c.categoria_nombre,
        p.condicion_id,
        p.producto_activo,
        p.producto_path_banner
    FROM Producto p
    INNER JOIN Categoria c ON p.categoria_id = c.categoria_id
    WHERE p.producto_activo = 1 AND p.producto_es_banner != 1 AND p.producto_es_promocionado != 1
    ORDER BY p.producto_fecha_creacion DESC;
END

EXEC sp_ObtenerProductosParaHome 
    @cantidadBanner = 3, 
    @cantidadDestacados = 4, 
    @cantidadUltimosIngresos = 4;


