WITH recursivo AS (
    SELECT PP2.permisopadre_id, PP2.permisohijo_id FROM Permiso_Permiso PP2
    --WHERE PP2.permisopadre_id = 1 acá se va variando la familia que busco
    UNION ALL 
    SELECT PP1.permisopadre_id, PP1.permisohijo_id FROM Permiso_Permiso PP1 
    INNER JOIN recursivo rec ON rec.permisohijo_id = PP1.permisopadre_id
)
SELECT PE.permiso_nombre as familia, rec.permisopadre_id, rec.permisohijo_id, P.permiso_id, P.permiso_nombre, P.permiso_tipo
FROM recursivo rec 
INNER JOIN Permiso P ON rec.permisohijo_id = P.permiso_id
INNER JOIN Permiso PE ON rec.permisopadre_id = PE.permiso_id
ORDER BY rec.permisopadre_id;


