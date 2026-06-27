# ProcrastiNation

ProcrastiNation is a specialized web application designed to transform the process of personal time tracking from a routine procedure into a tool for social interaction and gamified reflection. Unlike traditional time-trackers focused on strict productivity, this platform embraces the emotional and social aspects of procrastination, allowing users to log their wasted hours, share them with a community, and earn ironic titles and achievements.

# Key Features
### User Subsystem
- Time Tracking;
- Social Feed & Interaction;
- Gamification Engine;
- Personal Analytics;
- Real-time Notifications;
- Profile Management.

### Administrator Subsystem
- Content Moderation;
- User Management;
- Activity Dictionary Control;
- Global Analytics.

# Some screenshots
<img height="500" alt="Landing page" src="https://github.com/user-attachments/assets/cfc358ce-7df0-4f0b-8e56-5b274ba90d18" />
<img height="500" alt="Home" src="https://github.com/user-attachments/assets/968fed35-7375-480a-a2c6-6236c146ded0" />
<img height="500" alt="Global" src="https://github.com/user-attachments/assets/21cb91b1-39a8-4cc2-abfd-c8e3ca5b6779" />
<img height="500" alt="Achievements" src="https://github.com/user-attachments/assets/d7e120cb-047f-4063-badf-70400660f656" />
<img height="500" alt="image" src="https://github.com/user-attachments/assets/a81e001e-dc33-45a2-aac5-2a3d4b3614a0" />
<img height="500" alt="Profile" src="https://github.com/user-attachments/assets/9bc0dad5-4cee-4c01-bbf1-8bff5f7cd7d7" />

  
# Technology Stack
- Backend: C#, ASP.NET Core MVC (.NET 10.0);
- Database: PostgreSQL;
- ORM: Entity Framework Core;
- Frontend: HTML5, CSS3, JavaScript;
- Architecture & Patterns: Model-View-Controller, Dependency Injection, Action Filters.

# Database Architecture
The system utilizes a normalized relational database design comprising 12 primary tables:
- ```users```: Authentication data, roles, ban status, and aggregated personal statistics.
- `logs`: Core table storing individual time-tracking records.
- `activities` & `categories`: Dictionaries for classifying log entries.
- `comments` & `likes`: Tables handling social interactions.
- `titles` & `achievements`: Dictionaries for the gamification engine.
- `usertitles` & `userachievements`: Junction tables tracking unlocked rewards.
- `notifications`: Storage for user alerts.
- `globalstats`: Aggregated platform-wide metrics to reduce query load on the main feed.

# Prerequisites
Before you begin, ensure you have met the following requirements:
- .NET 10.0 SDK or later installed.
- PostgreSQL installed and running locally or remotely.
- An IDE such as Visual Studio, JetBrains Rider, or Visual Studio Code.

# Installation and Setup
1. Clone the repository:
```Bash
git clone https://github.com/yourusername/ProcrastiNation.git
cd ProcrastiNation
```
2. Configure the Database Connection:
Open `appsettings.json` and update the DefaultConnection string with your PostgreSQL credentials:
```JSON
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=ProcrastinationDb;Username=postgres;Password=yourpassword"
}
```
3. Apply Entity Framework Migrations:
Open your terminal or Package Manager Console and run:
```Bash
dotnet ef database update
```
This command will create the necessary tables in your PostgreSQL database.

4. Run the Application:
```Bash
dotnet run
```
The application will be available at `https://localhost:5001` or `http://localhost:5000`.
