# UAI-DAW-TP

## Usuario de prueba TIF

- Name: admin, pass: test, Rol: Administrador
- Name: op, pass: test, Rol: Operador
- Name: cliente, pass: test, Rol: Cliente

## Tabla de Permisos por Rol

| **Rol**           | GestionarBitacoraEventos | GestionarBitacoraCambios | GestionarBackup | ABMUsuarios | ABMProductos | AgregarProducto | BajarProducto | ModificarProducto |  VerProductos |
| ----------------- | ------------------------ | ------------------------ | --------------- | ----------- | ------------ | --------------- | ------------- | ----------------- | ------------ |
| **Administrador** | ✅                        | ✅                        | ✅               | ✅           | ✅            | ✅               | ✅             | ✅                 | ✅            |
| **Operador**      | ❌                        | ❌                        | ❌               | ❌           | ✅            | ✅               | ❌             | ✅                 | ✅            |
| **Cliente**       | ❌                        | ❌                        | ❌               | ❌           | ❌            | ❌               | ❌             | ❌                 | ✅            |