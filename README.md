# 🎟️ Sistema de Venta de Tiques para Eventos

> Proyecto académico — Programación II (ITI-321)

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet\&logoColor=white)
![C# 12](https://img.shields.io/badge/C%23-12-239120?logo=csharp\&logoColor=white)
![Estado](https://img.shields.io/badge/Estado-En%20desarrollo-yellow)
![Licencia](https://img.shields.io/badge/Licencia-Acad%C3%A9mico-blue)

## 🧭 Resumen

Sistema modular para buscar eventos, gestionar usuarios (rol **usuario** y **admin**), vender tiques con control de inventario y generar recibos en PDF. La persistencia se realiza en **archivos JSON**, con arquitectura **MVC** y aplicación de patrones de diseño (**Factory Method**, **Facade**, **Observer**). Se incluyen ejercicios de **recursividad** (búsqueda/filtrado) y **paralelismo** (compras concurrentes con seguridad).

## 👥 Equipo

* **Emesis Mayerli Mairena Sevilla**
* **Jairo Steven Herrera Romero**

## 🧩 Objetivos del proyecto

* Buscar y filtrar eventos por recinto, fecha y tipo.
* Ver detalles del evento y disponibilidad de tiques.
* Comprar tiques con control de concurrencia y validaciones.
* Gestionar eventos (crear/editar/eliminar) — rol admin.
* Notificar cambios relevantes (nuevos eventos / baja disponibilidad).
* Generar **recibos PDF** y mantener **historial de compras**.

## 🏗️ Arquitectura y patrones

**Arquitectura:** Modelo–Vista–Controlador (MVC), con separación clara entre lógica de dominio, presentación e infraestructura.

**Patrones de diseño:**

* **Factory Method:** creación de entidades (p. ej., `Evento`, `Usuario`, `Compra`) y estrategias de pago simulado.
* **Facade:** fachada de **Ventas** que orquesta inventario, validaciones y persistencia.
* **Observer:** suscripción de usuarios a notificaciones (nuevos eventos / stock bajo).

```mermaid
flowchart LR
  subgraph UI[Vista]
    A[Lista de Eventos]
    B[Detalle + Compra]
  end
  subgraph Ctl[Controladores]
    C[EventosController]
    D[ComprasController]
  end
  subgraph Dom[Dominio]
    E[ServicioEventos]
    F[ServicioCompras\n(Facade)]
    G[Notificador\n(Observer)]
  end
  subgraph Infra[Infraestructura]
    H[Repositorio JSON]
    I[Generador PDF]
  end

  A --> C --> E
  B --> D --> F --> H
  E --> H
  F --> I
  F --> G
```

## 🧪 Módulos principales

* **Gestión de Usuarios:** registro/login, control de acceso, roles.
* **Gestión de Eventos:** CRUD de eventos (admin), filtros y paginación (users).
* **Compra de Tiques:** selección de cantidad, validaciones, concurrencia, recibo PDF.
* **Historial:** consultas y descarga de recibos previos.
* **Notificaciones:** eventos nuevos y stock bajo (Observer).

## ⚙️ Tecnologías

* **Backend / Lógica:** .NET 8 con C# 12.
* **Persistencia:** JSON (por ejemplo: `data/eventos.json`, `data/usuarios.json`, `data/compras.json`).
* **UI de referencia:** consola (MVP). Roadmap a cliente de escritorio (WPF) o web (ASP.NET Core MVC) si se requiere.
* **PDF:** generador de recibos (implementación desacoplada tras una interfaz `IReciboPdf`).

## 📁 Estructura propuesta del repositorio

```
.
├── src/
│   ├── Dominio/              # Entidades y servicios de negocio
│   ├── Infraestructura/      # Repositorios JSON, PDF, utilidades
│   ├── Presentacion/         # Consola (MVP) / Web / WPF
│   └── Aplicacion/           # Controladores, DTOs, validaciones
├── data/                     # Archivos JSON con datos de ejemplo
├── tests/                    # Pruebas unitarias y de concurrencia
├── docs/                     # Diagramas, notas técnicas
└── README.md
```

## 🔐 Concurrencia y transacciones

* Compras procesadas de forma **paralela** asegurando **exclusión mutua** sobre el inventario.
* Se recomienda usar `SemaphoreSlim`/`lock` y operaciones atómicas al actualizar stock y persistir.
* Estrategia **idempotente** para evitar compras duplicadas si hay reintentos.

## 🔎 Búsqueda recursiva

* Filtros compuestos (por fecha/tipo/recinto) aplicados con una función recursiva que combina predicados.
* Comparación con versión iterativa y medición simple de rendimiento.

## 🧰 Requisitos previos

* Windows 11, **.NET SDK 8.0** instalado.
* Editor: Visual Studio Code.

## 🚀 Puesta en marcha (MVP de consola)

```bash
# 1) Restaurar dependencias
 dotnet restore

# 2) Ejecutar
 dotnet run --project src/Presentacion
```

> Asegúrate de que la carpeta **`data/`** exista con los JSON de ejemplo.

### Archivos JSON de ejemplo

**`data/eventos.json`**

```json
[
  { "id": "E-001", "nombre": "Concierto Sinfónico", "recinto": "Teatro Nacional", "fecha": "2025-11-15", "tipo": "Música", "stock": 120, "precio": 18000 }
]
```

**`data/usuarios.json`**

```json
[
  { "id": "U-001", "nombre": "Emesis", "correo": "emesis@correo.com", "rol": "admin", "passwordHash": "..." },
  { "id": "U-002", "nombre": "Jairo",  "correo": "jairo@correo.com",  "rol": "usuario", "passwordHash": "..." }
]
```

**`data/compras.json`**

```json
[]
```

## 🧾 Recibos en PDF

* Al completar una compra, se genera un PDF con: datos del evento, cantidad, monto total, fecha y código de verificación.
* La interfaz `IReciboPdf` permite cambiar la librería de PDF sin tocar la lógica de negocio.

## ✅ Checklist de funcionalidades

* [ ] Registro / inicio de sesión con roles
* [ ] Búsqueda y filtrado (recursivo) de eventos
* [ ] Compra con control de stock y concurrencia
* [ ] Notificaciones (Observer)
* [ ] Historial y descarga de recibos PDF
* [ ] CRUD de eventos (admin)
* [ ] Pruebas unitarias y de carga

## 🧪 Pruebas

* **Unitarias:** entidades, servicios, validaciones.
* **Concurrencia:** simulación de múltiples compradores sobre el mismo evento.
* **Rendimiento:** comparación de búsqueda recursiva vs. iterativa.

## 🔐 Seguridad (mínimos)

* Hash de contraseñas, validaciones de entrada, sanitización de datos.
* Registros de auditoría para compras y cambios de inventario.

## 🗺️ Roadmap

* [ ] Cliente de escritorio (WPF) o web (ASP.NET Core MVC)
* [ ] Módulo de reportes
* [ ] Pruebas end-to-end
* [ ] Integración de colas para notificaciones

## 🤝 Convenciones y colaboración

* **Ramas:** `main`, `dev`, `feature/*`.
* **Commits:** estilo convencional (feat, fix, refactor, docs, test, chore).
* **PRs:** pequeños, con descripción y lista de verificación.

## 📄 Licencia

Uso **académico**. Derechos reservados al equipo del curso. Se puede ajustar a MIT/Apache-2.0 si se publica abiertamente.

---

> **Contacto / Autores**
> **Emesis Mayerli Mairena Sevilla** · **Jairo Steven Herrera Romero**
