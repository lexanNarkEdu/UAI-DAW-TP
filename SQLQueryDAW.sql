SELECT * FROM Usuario
SELECT * FROM Usuario_Permiso
SELECT * FROM Permiso
SELECT * FROM Permiso_Permiso

INSERT INTO Permiso VALUES ('SinPermisos','PERMISO')

UPDATE Permiso
SET
    permiso_tipo = 'Accion'
WHERE permiso_id = 8;

DELETE FROM Permiso WHERE permiso_id = 2

INSERT INTO Permiso_Permiso VALUES (1,6)

INSERT INTO Usuario_Permiso VALUES ('YRcNPjkiPnm+2OHrdGNdsw==',7)