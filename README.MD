# 📦 API_Productos

API RESTful para la gestión de productos utilizando .NET 8, Entity Framework Core, FluentValidation, y paginación con filtros dinámicos. Incluye manejo estructurado de respuestas con clases ApiResponse, validaciones robustas, y limitación de solicitudes (rate limiting).

## 📁 Estructura del Proyecto

```bash
API_Productos/
├── Controllers/              # Endpoints HTTP
├── DTOs/                    # Data Transfer Objects
├── ├──Product/         # Respuestas estándar
│       ├── ProductCreateDto.cs
│       ├── ProductUpdateDto.cs
│       ├── ProductResponseDto.cs
│   └── ApiResponse/         # Respuestas estándar
│       ├── ApiResponse.cs
│       ├── ApiResponseData.cs
│       └── PagedResponse.cs
├── Models/                  # Entidad Product.cs
├── Repositories/           # Interfaces y repositorio
├── Services/               # Lógica de negocio
├── Validators/             # Validaciones con FluentValidation
├── Interaces/             # interfaces
├── Context/                   # DbContext
├── appsettings.json        # Configuración
├── Program.cs              # Configuración general y middleware
```

## 🚀 Endpoints

Todos los endpoints están bajo la ruta base:

```bash
/api/products
```

### ✅ GET /api/products

**Descripción**: Obtener lista paginada de productos con filtros opcionales.

**Query params:**

| Nombre      | Tipo    | Descripción                          |
| ----------- | ------- | ------------------------------------ |
| name        | string  | Filtrar por nombre                   |
| description | string  | Filtrar por descripción              |
| price       | decimal | Filtrar por precio exacto            |
| stock       | int     | Filtrar por cantidad en stock exacta |
| page        | int     | Página actual (por defecto: 1)       |

## ✅ Validaciones con FluentValidation

Ubicadas en /Validators/, cada DTO se valida antes de llegar al servicio.

Reglas para ProductCreateDto y ProductUpdateDto:
|Campo |Validación|
|-------|----------|
|name |No vacío, máx. 100 caracteres|
|description |No vacío, máx. 500 caracteres|
|price |>= 0|
|stock |>= 0|

Los errores se devuelven automáticamente como respuestas 400 BadRequest con detalles de validación.


## 📚 Librerías Usadas
Paquete	Uso Principal
| FluentValidation	| Validación de DTOs|
|-------------------|---------------------|
| EntityFrameworkCore	| ORM para base de datos|
| EntityFrameworkCore.SqlServer	| Proveedor de SQL Server|
| Swashbuckle.AspNetCore	| Swagger UI y documentación automática|
| xUnit, Moq, FluentAssertions (tests)	| Testing unitario e integración|

Configurar la base de datos:

Usar migraciones de EF Core:

```bash
dotnet ef database update
```

Configurar conexión a BD:
Editar appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=db;User Id=sa;Password=TuContraseña;TrustServerCertificate=True;"
}
```

## 🔥 Ejecutar la Aplicación

Instalar dependencias:

```bash
dotnet restore
```

Ejecutar:

```bash
dotnet run
```
