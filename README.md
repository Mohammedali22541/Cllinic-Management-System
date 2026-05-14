<div align="center">

# 🏥 Clinic Management System

### Modern ASP.NET Core MVC Application For Managing Clinics, Doctors & Appointments

<br/>

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=.net)]()
[![C#](https://img.shields.io/badge/C%23-Backend-239120?style=for-the-badge&logo=c-sharp)]()
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver)]()
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-ORM-7A3E9D?style=for-the-badge)]()
[![Bootstrap](https://img.shields.io/badge/Bootstrap-Responsive%20UI-7952B3?style=for-the-badge&logo=bootstrap)]()
[![Status](https://img.shields.io/badge/Status-Completed-success?style=for-the-badge)]()

<br/>

### 🔗 Live Demo

http://clinic-management-iti.runasp.net/Account/Login

</div>

---

# 📌 Overview

Clinic Management System is a modern web application built using **ASP.NET Core MVC** to simplify clinic operations including:

- Doctor management
- Patient management
- Appointment booking
- Doctor scheduling
- Appointment workflow tracking

The platform supports both **administrators** and **patients** with role-based authorization and business-driven validation rules.

---

# ✨ Features

## 👨‍💼 Admin Features

- Manage doctors
- Configure doctor availability
- Manage patients
- Manage appointments
- Update appointment status
- Dashboard statistics
- Soft delete support
- SweetAlert confirmation dialogs

---

## 👤 Patient Features

- Register & login
- Manage profile
- Book appointments
- View appointment history
- Cancel pending appointments

---

## ⚙️ System Features

- ASP.NET Core Identity Authentication
- Role-based Authorization
- Dark / Light Mode
- Responsive Bootstrap UI
- Toast Notifications
- Double Booking Prevention
- Doctor Availability Validation
- Repository Pattern
- Unit Of Work Pattern
- Entity Framework Core Migrations

---

# 🔄 Appointment Workflow

```text
Pending
   ├── Completed
   └── Cancelled
```

### Business Rules

- Appointments cannot be booked in the past
- Booking must match doctor availability
- Slot duration validation is enforced
- Double booking is prevented
- Completed appointments are final
- Cancelled appointments are final

---

# 🧠 Architecture

```text
Presentation Layer
ASP.NET Core MVC + Razor Views
        ↓
Business Layer
Services + DTOs + Business Rules
        ↓
Data Layer
Repositories + Unit Of Work + EF Core
        ↓
SQL Server Database
```

---

# 🏗️ Project Structure

```text
Clinic Management System
│
├── Clinic App
│   ├── Controllers
│   ├── Views
│   ├── wwwroot
│   └── Program.cs
│
├── ClinicApp.Business
│   ├── Services
│   ├── DTOs
│   └── Interfaces
│
├── ClinicApp.Data
│   ├── Context
│   ├── Entities
│   ├── Repositories
│   ├── Configurations
│   └── Migrations
```

---

# 🗄️ Main Entities

| Entity | Description |
|---|---|
| ApplicationUser | ASP.NET Identity user |
| Doctor | Doctor profile and specialization |
| Patient | Patient profile |
| Appointment | Appointment booking |
| DoctorAvailability | Doctor working schedule |

---

# 🧩 Technologies Used

| Technology | Purpose |
|---|---|
| ASP.NET Core MVC | Web Framework |
| Entity Framework Core | ORM |
| SQL Server | Database |
| ASP.NET Core Identity | Authentication & Authorization |
| Bootstrap 5 | Responsive UI |
| JavaScript | Client-side interactions |
| LINQ | Data querying |
| Repository Pattern | Data abstraction |
| Unit Of Work | Transaction management |
| SweetAlert2 | Confirmation dialogs |

---

# 🛠️ Local Setup

## Clone Repository

```bash
git clone https://github.com/Mohammedali22541/Clinic-Management-System.git

cd Clinic-Management-System
```

---

## Configure Database

Edit:

```text
Clinic App/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ClinicManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Restore Packages

```bash
dotnet restore
```

---

## Apply Migrations

```bash
dotnet ef database update --project ClinicApp.Data --startup-project "Clinic App"
```

---

## Run Application

```bash
dotnet run --project "Clinic App"
```

---

# 🌍 Deployment

Hosted on:

### MonsterASP.NET

Production URL:

http://clinic-management-iti.runasp.net/Account/Login

---

# 🚀 Future Improvements

- Calendar integration
- Email notifications
- SMS reminders
- Medical records
- Online payments
- Prescription management
- Analytics dashboard
- Receptionist role

---

# 👨‍💻 Author

## Mohammed Ali

### GitHub

https://github.com/Mohammedali22541

---

<div align="center">

### ⭐ Portfolio Project Built With ASP.NET Core MVC ⭐

</div>
