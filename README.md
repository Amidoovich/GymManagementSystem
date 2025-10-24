# 🏋️‍♂️ Gym Management System

## 📖 Overview
**Gym Management System** is a web-based application built with **ASP.NET Core MVC** to manage gym operations such as:  
- Member and Trainer management  
- Session scheduling and booking  
- Membership and training plan management  
- Dashboard analytics and reports  

The system is structured using a **Three-Layer Architecture** (Presentation, Business Logic, Data Access).

---

## 🚀 Features
- 👨‍🏫 **Trainer Management:** Full CRUD operations  
- 🧍‍♂️ **Member Management:** Add, update, delete, and view members  
- 📝 **Plan Management:** Edit, activate/deactivate (Soft Delete), and view plans  
- 🎯 **Membership Management:** Assign training plans to members  
- 📅 **Session Management:** Full CRUD operations for sessions  
- 🕒 **Session Scheduling & Booking:** Organize and book sessions with trainers and members  
- 📊 **Dashboard:** Analytics and reports for overall gym performance  

---

## 🧱 Architecture
**Three-Layer Architecture**
1. **Presentation Layer:** ASP.NET MVC Controllers + Razor Views (Bootstrap UI)  
2. **Business Logic Layer:** Services such as `TrainerService`, `SessionService` containing core logic  
3. **Data Access Layer:** Repository Pattern wrapping EF Core DbContext  

---

## 🛠️ Technology Stack
| Layer | Technology |
|-------|-------------|
| **Backend** | ASP.NET Core MVC |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Frontend** | Razor Views, Bootstrap, Custom CSS |
| **Patterns** | Repository Pattern, Unit of Work, Dependency Injection |
| **Libraries** | AutoMapper |

---

## 📂 Core Entities
### 👤 Member
- Represents a registered gym member  
- Includes personal details, health record, photo, and plan subscription  
- Relationships:  
  - One HealthRecord per Member  
  - One Plan per Member  
  - Many Sessions attended  

### 💪 Trainer
- Represents a trainer who conducts gym sessions  
- Includes specialties and hire date  
- One trainer can lead many sessions  

### 📘 Plan
- Represents a gym membership plan (duration, price, description, activation status)  
- Can be assigned to multiple members  

### 🧘‍♀️ Category
- Represents session categories (e.g., Boxing, CrossFit, Yoga)  

### 🕒 Session
- Represents a scheduled session with a trainer and category  
- Includes capacity, start/end dates, and participants  

---

## ⚙️ Business Rules
- Email and phone must be unique and valid for members/trainers  
- Members with active bookings cannot be deleted  
- Trainers with future sessions cannot be deleted  
- Members must have an active membership to book sessions  
- Sessions must have available capacity  
- Members cannot book the same session twice  
- Membership end date is automatically calculated (StartDate + Plan.DurationDays)  

---

## 🔐 Identity Module
The system uses **ASP.NET Identity** for authentication and authorization:  
- Login / Logout  
- Access Denied handling  
- User Roles and Permissions  

---

## 🧭 Controllers Overview
| Controller | Description |
|-------------|--------------|
| `HomeController` | Landing page and dashboard overview |
| `MemberController` | CRUD operations for members and health records |
| `TrainerController` | CRUD operations for trainers |
| `SessionController` | CRUD for sessions and bookings |
| `PlanController` | CRUD for plans + activate/deactivate |
| `AccountController` | User login, logout, and role management |

---

## 🧰 Tools Used
- Visual Studio / VS Code  
- SQL Server Management Studio  
- Entity Framework Core Migrations  
- AutoMapper  

---

## 🏁 How to Run the Project
1. Open the project using **Visual Studio 2022**  
2. Update the database connection string in `appsettings.json`  
3. Run the EF migration command:
   ```bash
   dotnet ef database update
   ```
4. Run the project using IIS Express or Kestrel  
5. Navigate to:
   ```
   https://localhost:xxxx
   ```

---

## 👨‍💻 Author
**Developed by:** [Ahmed Samy]  
**Role:** .NET Core Software Engineer  
**Contact:** [AhmedSamy1Ami@gmail.com]
