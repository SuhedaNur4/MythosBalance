# Mythos Balance 🏛️

Mythos Balance is a mythology-inspired activity tracking and life-balance platform that transforms productivity into a journey through the world of the Greek Gods.

Originally developed as a Web Programming course project, it evolved into a complete ASP.NET Core MVC application featuring activity tracking, user management, analytics, and a custom mythology-driven design system.

## 🚀 Features

| Feature | Description |
|----------|-------------|
| 🏛️ Mythological Domains | Categorize your daily activities into 5 different mythological life domains. |
| ✅ Activity Tracking | Track your tasks by separating them into Past Activities and Upcoming Activities. |
| 📊 Dynamic Statistics | Visualize your focus across life domains with interactive charts. |
| 👨‍💼 Admin Panel | Manage users, activities, and mythological guides through a dedicated admin interface. |
| 🎨 Custom Design | A mythology-inspired interface with a custom design system and immersive user experience. |


## 🏛️ The Mythological Guides

| Guide | Domain | Description |
|---------|---------|-------------|
| 🩺 **Hygieia** | Health | Goddess of health and well-being, guiding healthy habits and preventive care. |
| 🦉 **Athena** | Education | Goddess of wisdom and strategy, representing learning, knowledge, and personal development. |
| 🎼 **Apollo** | Creativity | God of music, poetry, and arts, inspiring creativity and self-expression. |
| 👟 **Hermes** | Travel | Patron of travelers and explorers, encouraging discovery and new experiences. |
| 🌸 **Charites (The Graces)** | Social | Goddesses of joy, friendship, and social harmony, representing meaningful human connections. |


## 🛠️ Technologies Used

| Category | Technology | Description |
| :--- | :--- | :--- |
| **Backend** | ASP.NET Core 8.0 | Core MVC framework for handling application logic. |
| **Database** | SQLite | Lightweight database chosen for portability and zero-configuration. |
| **ORM** | Entity Framework Core | Used with a Code-First approach for database modeling. |
| **Security** | ASP.NET Core Identity | Built-in authentication, authorization, and role management. |
| **Frontend** | HTML5, CSS3, JS | Custom luxury design using Vanilla CSS and JavaScript. |
| **Libraries** | Bootstrap 5 & Chart.js | Bootstrap for grid layouts; Chart.js for dynamic data visualization. |


## 🏗️ Project Architecture & Structure

The project is built on a layered **MVC (Model-View-Controller)** architecture and utilizes the **Repository Design Pattern** to keep the codebase clean, testable, and maintainable.

### Folder Structure

```text
MythosBalance/
 ├── Controllers/             # Application routing and HTTP request handlers
 │   ├── ActivityController.cs # Manages user activities (CRUD)
 │   ├── AdminController.cs    # Admin panel routing and user management
 │   ├── GuideController.cs    # Mythological guide display logic
 │   ├── HomeController.cs     # Main dashboard and landing page
 │   └── ProfileController.cs  # User profile, statistics, and domain details
 ├── Data/                    # Database context and seed data
 │   ├── ApplicationDbContext.cs # EF Core database context
 │   └── SeedData.cs           # Initial database population (Gods, Domains, Roles)
 ├── Migrations/              # Entity Framework Core database migrations
 ├── Models/                  # Core entities mapping to database tables
 │   ├── Activity.cs
 │   ├── ApplicationUser.cs    # Custom Identity user
 │   ├── LifeDomain.cs         # E.g., Health, Education, Travel
 │   └── MythologyGuide.cs     # E.g., Athena, Apollo, Hygieia
 ├── Repositories/            # Data access layer (Repository Pattern)
 │   ├── ActivityRepository.cs # Database queries for activities
 │   └── GuideRepository.cs    # Database queries for mythological guides
 ├── Services/                # Business logic layer
 │   ├── ActivityService.cs    # Logic for upcoming/past activities
 │   └── NotificationService.cs # Logic for user inactivity reminders
 ├── ViewModels/              # Data Transfer Objects (DTOs) for UI representation
 │   ├── DashboardViewModel.cs
 │   └── ProfileViewModels.cs
 ├── Views/                   # Razor pages (.cshtml)
 │   ├── Activity/            # Activity creation and editing forms
 │   ├── Admin/               # Admin dashboard views
 │   ├── Profile/             # Profile & dynamic chart views
 │   └── Shared/              # Main layout (_Layout.cshtml) and common UI components
 └── wwwroot/                 # Static web assets
     ├── css/                 # Custom luxury design system (site.css)
     ├── images/              # Logos and asset folders
     │   └── gods/            # High-quality mythological guide portraits
     └── js/                  # Custom client-side scripts (Chart.js initializers)
```

### Architectural Layers

*   **Models:** Represents the database tables as C# classes.
*   **ViewModels:** DTO (Data Transfer Object) classes that prevent exposing database entities directly to the presentation layer.
*   **Controllers:** Directs user flow by calling necessary repositories and services, then returning the appropriate View.
*   **Repositories:** Isolates database query logic from the controllers.
*   **Services:** Handles pure business rules, independent of HTTP contexts or database queries.
*   **Views:** Responsible purely for rendering the UI using data provided by the ViewModels.

## ⚙️ Setup & Installation

To run the project on your local machine, follow these steps:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/SuhedaNur4/MythosBalance.git
    cd MythosBalance
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Create the database (Apply Migrations):**
    ```bash
    dotnet ef database update
    ```

4.  **Run the project:**
    ```bash
    dotnet run
    ```

5.  Open your browser and navigate to `http://localhost:5000` to access the application.

## 📝 License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
