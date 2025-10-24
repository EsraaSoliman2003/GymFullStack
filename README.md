# GymFullStack

## Overview

GymFullStack is a comprehensive web application designed for gym management, built using ASP.NET (Core). It provides role-based dashboards for Admins, Trainers, and Trainees, enabling efficient user management, course tracking, and gym operations. The application supports user authentication, including login and registration, and is styled with Bootstrap for a modern, responsive user interface.

## Features

- **Role-Based Access**: Distinct dashboards for Admins (user management, reports), Trainers (course management), and Trainees (course enrollment and tracking).
- **User Authentication**: Secure login and registration system with role assignment (Admin, Trainer, Trainee).
- **Responsive Design**: Built with Bootstrap and custom CSS for a seamless experience across devices.
- **Scalable Architecture**: Organized MVC structure for easy maintenance and extensibility.
- **Modular Dashboards**: Dedicated views for each user role with placeholder pages for future features (e.g., reports, course details).

## Project Structure

```
MyMvcProject/
│
├── Controllers/
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── HomeController.cs
│   ├── TraineeController.cs
│   └── TrainerController.cs
│
├── Models/
│   ├── AppDbContext.cs                      # Common user model (Id, Name, Email, Role, etc.)
│   ├── ErrorViewModel.cs            # ViewModel for login form
│   ├── ForgotPasswordViewModel.cs         # ViewModel for registration form
│   └── User.cs                      # Common user model (Id, Name, Email, Role, etc.)
│
├── Views/
│   ├── Admin/
│   │   └── Index.cshtml
│   ├── Auth/
│   │   ├── ChooseAuth.cshtml
│   │   ├── ForgotPassword.cshtml
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   ├── Home/
│   │   ├── About.cshtml
│   │   ├── Contact.cshtml
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _Layout.cshtml.css
│   ├── Trainee/
│   │   └── Index.cshtml
│   └── Trainer/
│       └── Index.cshtml
│
├── wwwroot/ (ASP.NET Core)
│   ├── css/
│   │   └── site.css                 # Main styles
│   ├── js/
│   │   └── site.js                  # Custom JavaScript
│   └── lib/                         # External libraries (Bootstrap, jQuery)
├── README.md
├── appsettings.json                 # Configuration for ASP.NET Core
└── Program.cs                       # Entry point for ASP.NET Core
```

## Prerequisites

- **.NET Core** (for ASP.NET Core)
- **Visual Studio** or **Visual Studio Code** with C# extensions
- **SQL Server** (optional, for database integration)

## Installation

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/EsraaSoliman2003/GymFullStack.git
   cd GymFullStack
   ```

2. **Restore Dependencies**:

   - For ASP.NET Core:
     ```bash
     dotnet restore
     ```

3. **Configure the Application**:

   - Update `appsettings.json` (Core) with your database connection string and other settings.
   - Ensure Bootstrap and jQuery are included in `wwwroot/lib` or `Content/lib`.

4. **Run the Application**:

   - For ASP.NET Core:
     ```bash
     dotnet run
     ```

5. **Access the Application**:
   - Open your browser and navigate to `http://localhost:<port>`.

## Usage

- **Admins**: Log in with admin credentials to access the Admin dashboard. Manage users and view reports.
- **Trainers**: Log in to manage courses and view trainee progress.
- **Trainees**: Log in to view enrolled courses and track progress.
- **Public Users**: Access the Home, About, and Contact pages without authentication. Register or log in to access role-specific dashboards.

## Future Enhancements

- Implement database integration for user and course data.
- Add advanced reporting features for Admins.
- Enhance course management with scheduling and progress tracking.
- Integrate email notifications for user actions (e.g., registration, course enrollment).

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m "Add your feature"`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a pull request.

## License

This project is licensed under the MIT License. See the [
