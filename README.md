# Mythos Balance 🏛️

Mythos Balance is a mythology-inspired activity tracking and life-balance platform that transforms productivity into a journey through the world of the Greek Gods.

Originally developed as a Web Programming course project, it evolved into a complete ASP.NET Core MVC application featuring activity tracking, user management, analytics, and a custom mythology-driven design system.

## 🚀 Features

*   **Mythological Domains:** Categorize your daily activities into 6 different mythological life domains.
*   **Activity Tracking:** Track your tasks by separating them into "Past Activities" and "Upcoming Activities (Planned)".
*   **Dynamic Statistics:** Visualize how much focus you give to each life domain with dynamic charts on your profile page.
*   **Admin Panel:** A dedicated administrator interface to manage users, total activities, and mythological guides.
*   **Custom Design:** Moving away from classic dashboards, it features a custom dark-mode interface with glassmorphism effects, inspired by ancient Greek aesthetics.

## 🏛️ The Mythological Guides

In Mythos Balance, your life is divided into 5 distinct domains, each guided by a mythological figure that embodies its core values:

*   **Hygieia (Health):** The goddess of health, cleanliness, and hygiene. She guides your physical and mental well-being, focusing on the philosophy of preventative care and healthy habits rather than just curing illness.
*   **Athena (Education):** The goddess of wisdom, strategy, and science. She represents your intellectual journey, from reading books to acquiring new academic or professional skills. The owl, her symbol, represents seeing clearly through the darkness of ignorance.
*   **Apollo (Creativity):** The god of music, poetry, and beauty. He governs your hobbies and creative expressions, inspiring every melody you play, picture you paint, or art piece you create.
*   **Hermes (Travel):** The fleet-footed god of travelers and messengers. He is the patron of your journeys, explorations, and the breaking of boundaries as you discover new cities or cultures.
*   **Charites / The Graces (Social):** The three goddesses of charm, joy, and social harmony (Aglaia, Euphrosyne, and Thalia). They represent the essence of human connection, friendship, empathy, and community celebrations.

## 🛠️ Technologies Used

*   **Backend:** ASP.NET Core 8.0
*   **Database:** SQLite (chosen for portability without requiring complex setup)
*   **ORM:** Entity Framework Core (Code-First approach)
*   **Authentication:** ASP.NET Core Identity (User registration, login, and role management)
*   **Frontend:** HTML5, CSS3 (Custom Vanilla CSS), JavaScript
*   **Libraries:** Bootstrap 5 (for grid system and basic layout), Chart.js (for data visualization and charts)

## 🏗️ Project Architecture & Structure

The project is built on a layered **MVC (Model-View-Controller)** architecture and utilizes various design patterns to keep the code clean and maintainable:

*   **Models:** Represents the database tables as C# classes (`Activity`, `ApplicationUser`, `MythologyGuide`, `LifeDomain`).
*   **ViewModels:** DTO (Data Transfer Object) classes that contain only the necessary data to be passed to the View. This prevents exposing database entities directly to the presentation layer.
*   **Controllers:** Handles incoming HTTP requests from the user, calls necessary services/repositories, and returns the appropriate View (`ActivityController`, `ProfileController`, `AdminController`).
*   **Repositories:** The data access layer where database operations (CRUD) are performed. The **Repository Design Pattern** is used to isolate database queries from Controllers (`ActivityRepository`, `GuideRepository`).
*   **Services:** The layer where business logic is implemented. For example, notification checks and data processing are handled here (`NotificationService`).
*   **Views:** Razor (`.cshtml`) files that make up the user interface.

## ⚙️ Setup & Installation

To run the project on your local machine, follow these steps:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/yourusername/MythosBalance.git
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
