# 🚀 QuickStarter API - Simulación de Crowdfunding

Una API RESTful desarrollada en **ASP.NET Core** que simula la funcionalidad de plataformas como Kickstarter, utilizando **Mercado Pago** como pasarela de pagos. La solución está enfocada en escalabilidad, buenas prácticas y validaciones robustas.

---

## 🛠️ Tecnologías y Herramientas

- **ASP.NET Core** – Backend moderno y robusto
- **SQL Server** – Almacenamiento relacional
- **Entity Framework Core** – ORM para persistencia de datos
- **Identity Framework** – Gestión de usuarios y roles
- **FluentValidation** – Validaciones limpias y desacopladas
- **LINQ** – Consultas eficientes y expresivas
- **JWT** – Autenticación segura mediante tokens
- **Mercado Pago SDK** – Integración de pagos en línea

---

## 📦 Funcionalidades

- ✅ Registro e inicio de sesión con tokens JWT
- ✅ Crear y administrar campañas de financiación
- ✅ Realizar aportes económicos a campañas usando Mercado Pago
- ✅ Validaciones sólidas con FluentValidation
- ✅ Consultas filtradas y ordenadas con LINQ
- ✅ API RESTful bien estructurada y documentada

---

## 📄 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- Cuenta de [Mercado Pago Developer](https://www.mercadopago.com.mx/developers/panel)
- Visual Studio 2022+ o VS Code

---

## ⚙️ Variables de entorno requeridas

Crea un archivo `.env` o configura estas variables en tu entorno de desarrollo:

```env
CONNECTION_STRING=Server=localhost;Database=QuickStarterDb;Trusted_Connection=True;
MERCADO_PAGO_TOKEN=tu_token_mercado_pago
TOKEN_SECRET=clave_super_secreta_para_jwt
