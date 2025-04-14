# 🛠️ FixItNow App

**FixItNow** is a cross-platform application that connects users with instant home repair and maintenance services. It consists of:

- 📱 **Mobile App** (Xamarin.Forms)
- 🌐 **Web App** (ASP.NET MVC)
- 🔌 **Web API** (ASP.NET Core Web API)

This solution is designed to streamline the booking and management of fix-it services, with full admin and user roles.

---

## 📂 Folder Structure

```bash
FixItNow/
├── FixItNow/            # Xamarin Mobile App
├── FixItNowWebApp/      # ASP.NET MVC Web App
└── FixItNowWebApi/      # ASP.NET Core Web API

🚀** Features
👥 User Features**
Register and Login (Mobile & Web)

Browse services (Plumbing, Electrical, Cleaning, etc.)

Book a service with date/time preferences

Mock payment flow (no gateway integration)

Profile creation and editing

View booking history and status

🧰 Admin Features
Login to Admin Dashboard

Add/Edit/Delete services

View bookings by all users

Download data as CSV

Manage service providers

💻 Technologies Used
Layer	Technology
Mobile App	Xamarin.Forms (.NET Standard)
Web App	ASP.NET MVC
Backend	ASP.NET Core Web API
Database	SQL Server + Entity Framework
API Format	JSON
Auth	Role-based Authentication

** How to Run the Project**
📱 Mobile App (Xamarin)
Open FixItNow.sln in Visual Studio

Set FixItNow (Xamarin project) as startup

Ensure emulator or device is connected

Update ApiService.cs base URL if necessary (e.g. 10.0.2.2 for Android Emulator)

🌐 Web App (MVC)
Set FixItNowWebApp as startup project

Run the app — it launches on a browser

Test user/admin login, bookings, and dashboards

🔌 Web API
Set FixItNowWebApi as startup project

Run API in Swagger or Postman

Base URL: http://localhost:5080/api

Ensure all 3 projects are using the same database connection string if needed.
