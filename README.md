# 👥 User Management System  

A simple **ASP.NET Core MVC application** for managing users and roles with **authentication** and **authorization**.  
This project demonstrates how to implement **user registration**, **login**, **role-based access control**, and **full CRUD operations** — all with a clean, responsive **Bootstrap** UI.  

---

## 🧾 Overview  

The **User Management System** allows administrators to manage users and roles with full authentication and authorization features.  
It’s a perfect project for learning **ASP.NET Core MVC**, **Entity Framework Core**, and **Bootstrap integration**.  

---

## 🚀 Features  

✅ User Registration & Login  
✅ Secure Authentication using Cookies  
✅ Role-based Authorization (Admin / User)  
✅ CRUD Operations for Users and Roles  
✅ Client-side & Server-side Validation  
✅ Modern, Responsive UI with Bootstrap  
✅ Entity Framework Core Integration  
✅ SQL Server Database  

---

## 🛠️ Tech Stack  

| Category | Technologies |
|-----------|--------------|
| **Backend** | ASP.NET Core MVC, C# |
| **Database** | Entity Framework Core, SQL Server |
| **Frontend** | HTML5, CSS3, Bootstrap 5 |
| **Authentication** | Cookie-based Authentication |
| **IDE** | Visual Studio 2022 Community |

---

## 📸 Screenshots  
<img width="1902" height="890" alt="image" src="https://github.com/user-attachments/assets/6115582f-522b-4284-b5cd-4d9d8c74b0c1" />
<img width="1920" height="887" alt="image" src="https://github.com/user-attachments/assets/90bc15d5-14c0-4251-b045-a52748b19d5d" />
<img width="1918" height="886" alt="image" src="https://github.com/user-attachments/assets/f46ab9c0-27e8-4d0e-b8d7-5f2c17b07c05" />
<img width="1920" height="889" alt="image" src="https://github.com/user-attachments/assets/153afa89-547c-4149-854b-eb22c8d95d11" />
<img width="1920" height="893" alt="image" src="https://github.com/user-attachments/assets/84d46792-c415-4911-aa82-795349375b91" />
<img width="1920" height="892" alt="image" src="https://github.com/user-attachments/assets/025d5aac-8152-4eba-8a7e-4f2b76de6574" />
<img width="1920" height="889" alt="image" src="https://github.com/user-attachments/assets/4c7f57e3-a913-4620-b286-280f21394551" />

---

## ⚙️ Installation & Setup  

Follow these steps to run the project locally 👇  

### 1️⃣ Clone the Repository  
```bash
git clone https://github.com/your-username/UserManagementSystem.git
```

### 2️⃣ Open in Visual Studio 2022
Open the .sln file in Visual Studio.

### 3️⃣ Set up the Database
1- Update your connection string in appsettings.json.

2- Run the following commands in the Package Manager Console:
```bash
add-migration InitialCreate
update-database
```

### 4️⃣ Run the Project

Press Ctrl + F5 or click Run Without Debugging.
The app will open in your browser (default: https://localhost:xxxx).

## 🧩 Project Structure

UserManagementSystem/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── UserController.cs
│   └── RoleController.cs
│
├── Models/
│   ├── User.cs
│   ├── Role.cs
│   └── ViewModels/
│
├── Views/
│   ├── Account/
│   ├── User/
│   ├── Role/
│   └── Shared/
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
└── appsettings.json

## 🧠 Learning Goals

This project was built to practice and understand:

✅ ASP.NET Core MVC architecture

✅ Authentication & Authorization

✅ Entity Framework Core and Migrations

✅ Razor Views and Model Binding

✅ Form Validation (Client & Server)

✅ Bootstrap Integration


### ⭐ If you found this project helpful, don’t forget to star the repo! 🌟
