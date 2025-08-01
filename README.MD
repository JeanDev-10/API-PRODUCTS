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

## 🧪 Tests Unitarios

Se incluye una solución aparte llamada `API-Productos.Unit-Testing` con pruebas unitarias para asegurar la calidad y correcto funcionamiento de las capas principales del proyecto.

```bash
ProductApi.UnitTests/
├── Services/
│ └── ProductServiceTests.cs # Pruebas de lógica de negocio
├── Controllers/
│ └── ProductControllerTests.cs # Pruebas de comportamiento del controlador
├── Validators/
│ └── ProductCreateDtoValidatorTests.cs
```

---

### 🔧 Herramientas utilizadas

| Paquete                      | Propósito                              |
|-----------------------------|-----------------------------------------|
| `xUnit`                     | Framework de testing principal          |
| `Moq`                       | Simulación (mock) de dependencias       |
| `FluentAssertions`          | Assertions más expresivas               |
| `FluentValidation` | Validación de reglas en DTOs         |

---

### ✅ Cobertura actual

| Capa                  | Cobertura         | Detalles                                                   |
|-----------------------|-------------------|------------------------------------------------------------|
| `ProductService`      | ✅ 100%            | Pruebas de creación, actualización, eliminación y obtención por ID |
| `ProductController`   | ✅ 100%            | Pruebas de cada endpoint con validaciones y errores        |
| `FluentValidation`    | ✅ 100%            | Reglas para `ProductCreateDTO` probadas                    |
| `PagedResponse<T>`    | ✅ Usado y probado en `GetAll`              |

---

### 🧪 Pruebas destacadas

#### ✔️ ProductServiceTests
- Simulación del repositorio (`IProductRepository`).
- Pruebas unitarias de los métodos: `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `GetByIdAsync`.

#### ✔️ ProductControllerTests
- Mock de `IProductService` y `IValidator<ProductCreateDTO>`.
- Verifica:
  - Respuestas HTTP (`200`, `400`, `404`).
  - Errores de validación.
  - Respuesta estructurada con `ApiResponseData<T>` y `PagedResponse<T>`.

#### ✔️ Validaciones
- Se prueba cada regla definida con FluentValidation.
- Casos válidos e inválidos, usando `TestValidate`.

---
### 🚀 Ejecutar los tests

Ubícate en la carpeta raíz del proyecto y ejecuta:
```bash
dotnet test ProductApi.UnitTests
```
---

## 📈 Próximos pasos
- Agregar tests de integración con base de datos SQLite.
- Integrar cobertura de código con coverlet y reportgenerator.
- Automatizar pruebas en CI (GitHub Actions).


## 🔥 Ejecutar la Aplicación

Instalar dependencias:

```bash
dotnet restore
```

Ejecutar:

```bash
dotnet run
```
