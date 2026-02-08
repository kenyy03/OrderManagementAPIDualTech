# API OrderManagement Dual Tech - API RESTful

1.	✅ Tecnologías utilizadas (.NET 8, C# 12, SQL Server, EF Core, AutoMapper)
2.	✅ Instrucciones de instalación paso a paso:
•	Clonar repositorio
•	Restaurar dependencias
•	Configurar cadena de conexión
3.	✅ Script completo de base de datos:
    CREATE DATABASE OrderManagementDB;
    GO
    USE OrderManagementDB;
    GO
    CREATE TABLE Cliente (
    ClienteId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Identidad NVARCHAR(50) NOT NULL UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );
    CREATE TABLE Producto (
    ProductoId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(10,2) NOT NULL,
    Existencia INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );

    CREATE TABLE Orden (
    OrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClienteId BIGINT NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL DEFAULT 0,
    Subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    Total DECIMAL(10,2) NOT NULL DEFAULT 0,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Orden_Cliente FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
    );

    CREATE TABLE DetalleOrden (
    DetalleOrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrdenId BIGINT NOT NULL,
    ProductoId BIGINT NOT NULL,
    Cantidad INT NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_DetalleOrden_Orden FOREIGN KEY (OrdenId) REFERENCES Orden(OrdenId),
    CONSTRAINT FK_DetalleOrden_Producto FOREIGN KEY (ProductoId) REFERENCES Producto(ProductoId)
    );
    -- Índices para performance
    CREATE INDEX IX_Orden_ClienteId ON Orden(ClienteId);
    CREATE INDEX IX_DetalleOrden_OrdenId ON DetalleOrden(OrdenId);
    CREATE INDEX IX_DetalleOrden_ProductoId ON DetalleOrden(ProductoId);
    -- Datos de prueba
    INSERT INTO Cliente (Nombre, Identidad) VALUES
    ('Juan Pérez', '0801-1990-12345'),
    ('María González', '0801-1985-67890'),
    ('Carlos Rodríguez', '0801-1992-11111');
    INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia) VALUES
    ('Laptop Dell XPS 15', 'Laptop de alto rendimiento', 1299.99, 50),
    ('Mouse Logitech MX Master 3', 'Mouse ergonómico inalámbrico', 99.99, 150),
    ('Teclado Mecánico Keychron K2', 'Teclado mecánico retroiluminado', 89.99, 75),
    ('Monitor LG 27" 4K', 'Monitor 4K UHD', 449.99, 30),
    ('Webcam Logitech C920', 'Webcam Full HD 1080p', 79.99, 100);

4.	✅ Instrucciones de ejecución:
•	Cnfigurar cadena de conexión en appsettings.json
•	Ejecutar solución en Visual Studio o usar dotnet CLI
5.	✅ Documentación completa de todos los endpoints:
•	Clientes: GET (todos), GET (por ID), POST, PUT
•	Productos: GET (todos), GET (por ID), POST, PUT, DELETE
•	Órdenes: POST
•	Health Check: GET
6.	✅ Decisiones técnicas importantes:
•	Arquitectura en capas
•	Repository Pattern (con código de ejemplo)
•	Unit of Work Pattern (con código de ejemplo)
•	AutoMapper
•	Extensión de ApiResponse (con código de ejemplo)
•	Constantes centralizadas (con código de ejemplo)
•	CORS configurado
•	Swagger/OpenAPI
•	Validaciones de negocio
•	Manejo de errores
•	Índices en base de datos
7.	✅ Estructura del proyecto (árbol de directorios)
8.	✅ Notas adicionales:
•	Validaciones de Cliente
•	Cálculo de impuestos (15%)
•	Códigos de estado HTTP
•	💡Dudas con el cálculo de totales en el encabezado de la orden, viendo el ejemplo proporcionado, mis totales no coinciden, haciendo una revisión sospecho que hay un cargo gravado que no estoy aplicando, como no sé cuál el valor de ese cargo prefiero no aplicarlo, con algunas herramientas sospecho que el valor gravado es de 110 pero, en un ambiente real suponer siempre es un gran error que puede llevar a errores de operación en producción por lo que en todo caso prefiero preguntar a mi PO o aun stakeholder. Me gustaría saber mediante su feedback el cálculo correcto de los totales del encabezado sino es mucha la molestia. Gracias de antemano.