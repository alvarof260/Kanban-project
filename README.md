---

# 🏗️ Kanban Managment App

Este proyecto es una aplicación de tablero Kanban que permite a los usuarios gestionar tareas en diferentes tableros. Los administradores pueden gestionar usuarios y los dueños de tableros pueden administrar todas las tareas dentro de sus tableros.

## 🚀 Características

- **Autenticación de usuarios**: Inicio de sesión y gestión de sesiones.
- **Gestión de tableros**: Creación, edición, eliminación de tableros.
- **Gestión de tareas**: Creación, edición, eliminación y asignación de tareas.
- **Permisos de usuario**:

  🔹 El dueño del tablero puede crear y modificar todas las tareas.
  🔹 Los usuarios asignados solo pueden modificar el estado de sus propias tareas.

- **Gestión de usuarios (Admin)**: Creación, edición y eliminación de usuarios (solo accesible por administradores).
- **Persistencia de datos** con una API REST en ASP.NET y conexión a una base de datos.
- **Interfaz dinámica** con React y TypeScript.

---

## 🛠 Tecnologías utilizadas

### Frontend

- ⚛ React + TypeScript
- 🎨 Tailwind CSS
- 📦 Context API para manejo de estado de sesión
- 🌐 React Router para navegación

### Backend

- 🏗️ ASP.NET Web API
- 🗄️ Base de datos SQL (por ejemplo, SQL Server o PostgreSQL)

---

📂 Estructura del proyecto

```bash
/backend
│── /Controllers        # Controladores de la API
│── /Enums              # Enumeraciones utilizadas en el backend
│── /Interfaces         # Interfaces de los modelos y servicios
│── /Models             # Modelos de la base de datos
│── /Properties         # Configuraciones de propiedades
│── /Repositories       # Acceso a datos y lógica de negocio
│── /ViewModels         # Modelos de datos para las respuestas de la API
│── Program.cs          # Configuración principal del backend
│── appsettings.json    # Configuración de la API
│── backend.http        # Archivo para pruebas de endpoints

/frontend
│── /public             # Archivos estáticos
│── /src
│   ├── /assets        # Recursos estáticos como imágenes o íconos
│   ├── /components    # Componentes reutilizables de la UI
│   ├── /contexts      # Context API (manejo de estados globales)
│   ├── /guards        # Protección de rutas para roles y autenticación
│   ├── /hooks         # Hooks personalizados
│   ├── /icons         # Íconos utilizados en la UI
│   ├── /layouts       # Estructuras generales de las páginas
│   ├── /models        # Modelos de datos usados en el frontend
│   ├── /pages         # Páginas principales de la aplicación
│   ├── App.tsx        # Componente raíz del frontend
│   ├── constants.ts   # Constantes del proyecto
│   ├── index.css      # Estilos globales
│   ├── main.tsx       # Punto de entrada del frontend

```

---

## 📌 Instalación y configuración

### Requisitos previos

1. Tener instalado Node.js y bun.
2. Tener configurado el backend con ASP.NET Web API.
3. Tener configurada una base de datos SQL.

### pasos para ejecutar el backend

1. Navegar a la carpeta del backend

```sh
cd backend
```

2. Restaurar paquetes:

```sh
dotnet restore
```

3. Configuar la base de datos en `appsettings.json`
4. Ejecutar la API:

```sh
dotnet run
```

---

### Pasos para ejecutar el frontend

1. Navegar a la carpeta del frontend:

```sh
cd frontend
```

2. Instalar dependencias:

```sh
npm install
```

3. Ejecutar el proyecto:

```sh
bun dev
```

---
