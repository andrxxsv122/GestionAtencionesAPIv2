# 🏥 Gestión de Atenciones Médicas - API REST (.NET 8 + SQL Server)

## 📋 Descripción General

API RESTful para gestionar atenciones médicas en una clínica u hospital. Permite administrar pacientes, doctores, especialidades y atenciones médicas, con filtros avanzados, validaciones y seguridad mediante API Key.

---

## 🧰 Tecnologías Utilizadas

- [.NET 8](https://dotnet.microsoft.com/) — Framework principal para la API REST
- C# — Lenguaje de programación
- SQL Server — Base de datos relacional
- Azure Data Studio — Herramienta utilizada para gestionar la base de datos y ejecutar scripts
- Dapper — Micro ORM para acceso rápido a datos
- Swagger — Documentación interactiva de endpoints
- Autenticación por API Key (middleware)
- Middleware personalizado — Validación de autenticación vía API Key (X-Api-Key)
- Archivo .http — Para pruebas de endpoints (alternativa ligera a Postman)

---

## 🚀 Instrucciones de Setup

### 1. Clona el repositorio

```bash
git clone https://github.com/tu-usuario/GestionAtencionesAPIv2.git
cd gestion-atenciones-api
```

### 2. Restaura paquetes y ejecuta

Abre el proyecto en **Visual Studio 2022** o superior. Luego:

- Asegúrate de tener instalada la SDK de .NET 8.
- Restaura los paquetes NuGet.
- Revisa el archivo `appsettings.json` y actualiza la cadena de conexión si es necesario.

### 3. Base de datos

- Usa el archivo `Script_BD.sql` incluido en la raíz del proyecto para crear la base de datos y sus tablas.
- También se incluye un `.bak` de respaldo opcional: `FichaClinica.bak`.

### 4. Ejecutar la API

Presiona **F5** o usa:

```bash
dotnet run
```

La API estará disponible por defecto en:

```
https://localhost:7161
```

### 5. Probar con Postman o archivo `.http`

- Usa el archivo `GestionAtenciones.http` para probar todos los endpoints.
- También puedes importar la colección de Postman: `GestionAtenciones.postman_collection.json`.

---

## 🔐 API Key

Todos los endpoints requieren autenticación mediante una **API Key** enviada en los headers:

```
x-api-key: 123456
```

Esta clave está definida en `appsettings.json` bajo la sección `"ApiKey"`.

---

## 📚 Estructura del Proyecto

```
├── Controllers/
├── DTOs/
├── Middleware/
├── Models/
├── Repositories/
├── Data/
├── GestionAtenciones.http
├── GestionAtenciones.postman_collection.json
├── Script_BD.sql
├── appsettings.json
└── README.md
```

---

## 📌 Consideraciones Técnicas

- Se validan relaciones para evitar eliminaciones con atenciones registradas.
- Se valida solapamiento de horarios para un mismo doctor.
- Se calcula duración promedio de atenciones por especialidad.
- Se maneja la excepción de clave duplicada al crear especialidades (validación previa).
- API Key implementada mediante middleware personalizado.
- Arquitectura limpia: separación por capas (Controllers, Services/Repositories, Models/DTOs).

---

## 👨‍💻 Autor

Bastián Spuler  
🔗 [GitHub](https://github.com/andrxxsv122)
