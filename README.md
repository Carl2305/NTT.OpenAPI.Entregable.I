# Entregable I - Documentación de una API REST con OpenAPI

## API de Gestión de Reservas de Salas

Esta solución implementa una API REST completa en **ASP.NET Core (.NET 10)** para administrar las reservas de salas de reuniones de una organización, cumpliendo con todos los requerimientos de documentación de OpenAPI, especificación de seguridad mediante **JWT Bearer Token**, interfaz interactiva en **Swagger UI** y generación automática de un **cliente .NET mediante NSwag**.

---

## 1. Descripción Breve de la Solución

La **API de Gestión de Reservas de Salas** permite administrar de forma centralizada las solicitudes y estados de reservas.

### Características Principales:
- **Estructura limpia e independizada**: Separación estricta entre el modelo de dominio interno (`Reserva`) y los objetos de transferencia de datos DTO (`ReservaResponseDto`, `CrearReservaDto`, `ActualizarReservaDto`, `LoginRequestDto`, `LoginResponseDto`).
- **Validaciones de Negocio Robustas**:
  - `HoraFin` debe ser estrictamente posterior a `HoraInicio`.
  - `CantidadAsistentes` debe ser mayor a 0.
  - Validación previa de todos los datos obligatorios (`[Required]`, `[StringLength]`, `[Range]`).
  - Control de recursos no encontrados respondiendo adecuadamente con código **404 Not Found**.
- **Persistencia en Memoria**: Uso de `ConcurrentDictionary` para almacenamiento seguro entre hilos, pre-poblado con datos semilla iniciales para facilitar pruebas inmediatas.
- **Seguridad JWT/Bearer Token**: Protección de operaciones de modificación (`POST`, `PUT`, `DELETE`) requiriendo autenticación Bearer JWT.

---

## 2. Endpoints Implementados

| Método | Endpoint | Descripción | Requiere Auth |
| :--- | :--- | :--- | :---: |
| **POST** | `/api/Auth/login` | Autentica credenciales y emite un token JWT Bearer | No |
| **GET** | `/api/Reservas` | Obtiene todas las reservas (permite filtro `?fecha=YYYY-MM-DD`) | No |
| **GET** | `/api/Reservas/{id}` | Obtiene el detalle de una reserva por su `Id` | No |
| **POST** | `/api/Reservas` | Registra una nueva reserva de sala | **Sí** |
| **PUT** | `/api/Reservas/{id}` | Actualiza la información de una reserva existente | **Sí** |
| **DELETE** | `/api/Reservas/{id}` | Elimina o cancela una reserva por su `Id` | **Sí** |

### Códigos de Respuesta HTTP Documentados en OpenAPI:
- `200 OK`: Operación exitosa o consulta procesada.
- `201 Created`: Reserva creada exitosamente (retorna cabecera `Location` y objeto creado).
- `400 Bad Request`: Datos de solicitud inválidos o infracción de reglas de validación.
- `401 Unauthorized`: Petición a endpoint protegido sin token Bearer o token inválido.
- `404 Not Found`: Cuando la reserva consultada, actualizada o eliminada no existe.

---

## 3. Pasos para Ejecutar el Proyecto

### Requisitos Previos:
- **.NET 10 SDK** instalado en el sistema.

### Instrucciones de Ejecución:

1. **Clonar o abrir el repositorio** en la carpeta principal del proyecto:
   ```bash
   cd c:/Users/carlo/Downloads/actividadesNTT/OpenAPI
   ```

2. **Compilar la solución completa**:
   ```bash
   dotnet build OpenAPI.slnx
   ```

3. **Iniciar la API ASP.NET Core**:
   ```bash
   dotnet run --project src/GestorReservas.Api/GestorReservas.Api.csproj --launch-profile http
   ```

   La API iniciará en la dirección local: `http://localhost:5000`

---

## 4. Acceso a Swagger UI y Prueba con Autorización Bearer

1. Abra su navegador web e ingrese a la siguiente URL:
   ```text
   http://localhost:5000/swagger
   ```

2. **Visualizar la Documentación OpenAPI**:
   - Podrá consultar la descripción de cada endpoint, sus parámetros Path y Query, los esquemas de modelos Request/Response y los códigos HTTP posibles.

3. **Probar Endpoints Protegidos (Opción Authorize)**:
   - Despliegue el endpoint `POST /api/Auth/login` en Swagger UI.
   - Haga clic en **Try it out** y luego en **Execute** usando las credenciales predeterminadas:
     ```json
     {
       "username": "admin",
       "password": "admin123"
     }
     ```
   - En la respuesta (200 OK), copie la cadena del campo `"token"`.
   - Haga clic en el botón **Authorize** (ubicado en la parte superior derecha de la página).
   - Ingrese el token copiado y presione **Authorize**.
   - A partir de este momento, podrá probar los endpoints protegidos (`POST /api/Reservas`, `PUT`, `DELETE`) directamente desde Swagger UI utilizando la función **Try it out**.

---

## 5. Especificación OpenAPI Generada

El archivo de especificación OpenAPI en formato JSON se encuentra generado y sincronizado en la raíz del proyecto:
- **Ruta del archivo OpenAPI**: [`openapi.json`](file:///c:/Users/carlo/Downloads/actividadesNTT/OpenAPI/openapi.json)

También puede descargarse directamente desde la API en ejecución accediendo a:
- `http://localhost:5000/swagger/v1/swagger.json`

---

## 6. Generación del Cliente .NET

El cliente C# fue generado automáticamente a partir de la especificación [`openapi.json`](file:///c:/Users/carlo/Downloads/actividadesNTT/OpenAPI/openapi.json) utilizando la herramienta **NSwag** (`nswag.consolecore` v14.7.1).

### Comando utilizado para la generación:
```bash
nswag openapi2csclient /input:openapi.json /output:src/GestorReservas.Client/ReservasApiClient.cs /className:ReservasApiClient /namespace:GestorReservas.Client
```

### Demostración del Cliente .NET:
El proyecto [`GestorReservas.Client`](file:///c:/Users/carlo/Downloads/actividadesNTT/OpenAPI/src/GestorReservas.Client) contiene una aplicación de consola que consume la API REST a través de la clase generada `ReservasApiClient`, ejecutando las siguientes acciones de forma automática:
1. Autenticación en `POST /api/Auth/login` para recibir el token JWT.
2. Consulta de todas las reservas mediante `GET /api/Reservas`.
3. Registro de una nueva reserva mediante `POST /api/Reservas` enviando el Bearer Token.
4. Consulta de la reserva recién creada mediante `GET /api/Reservas/{id}`.
5. Actualización de la reserva mediante `PUT /api/Reservas/{id}`.
6. Eliminación de la reserva mediante `DELETE /api/Reservas/{id}`.

### Para ejecutar la demostración del Cliente .NET:
Asegúrese de que la API esté corriendo en `http://localhost:5000` y ejecute en otra terminal:
```bash
dotnet run --project src/GestorReservas.Client/GestorReservas.Client.csproj
```

---

## 7. Estructura de Entregables

```text
OpenAPI/
├── openapi.json                          # Especificación OpenAPI v3.0 exportada
├── OpenAPI.slnx                          # Archivo de solución .NET
├── README.md                             # Documentación y guía de ejecución
└── src/
    ├── GestorReservas.Api/               # Código fuente de la Web API en ASP.NET Core
    │   ├── Controllers/
    │   │   ├── AuthController.cs         # Endpoint de login JWT
    │   │   └── ReservasController.cs     # Endpoints CRUD de reservas documentados
    │   ├── Models/
    │   │   ├── Domain/                   # Modelos internos (Reserva, EstadoReserva)
    │   │   └── DTOs/                     # Objetos de Transferencia de Datos (DTOs)
    │   ├── Services/                     # Lógica de negocio e in-memory repository
    │   ├── Program.cs                    # Configuración de SwaggerGen, XML Docs y JWT
    │   └── GestorReservas.Api.csproj
    └── GestorReservas.Client/            # Cliente .NET generado a partir de OpenAPI
        ├── ReservasApiClient.cs          # Cliente C# generado por NSwag
        ├── Program.cs                    # Demostración del cliente consumiendo la API
        └── GestorReservas.Client.csproj
```
