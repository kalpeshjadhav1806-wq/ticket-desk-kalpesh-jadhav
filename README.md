# **SUPPORT TICKET DESK** 

Assessment submission README 

|**Candidate**|Kalpesh Jadhav|
|---|---|
|**Submitted**|24 November 2026 — 1:00 AM IST|
|**Time spent**|3 hours 30 minutes|



## **1. About the Application** 

Support Ticket Desk is a web application where employees can raise support tickets and track their progress. Agents can pick up available tickets and resolve them, while managers can monitor tickets, view their history, and reopen recently closed tickets. Role-based access controls which features each user can access. 

## **2. Tech Stack** 

### **Backend** 

- ASP.NET Core Web API (.NET 8) and C# 

- Entity Framework Core 

- Microsoft SQL Server 

- JWT Authentication 

- Swagger / OpenAPI 

### **Frontend** 

- Angular 17 and TypeScript 

- Angular Material 

- HTML and SCSS 

### **Key packages** 

- Microsoft.EntityFrameworkCore 

- Microsoft.EntityFrameworkCore.SqlServer 

- Microsoft.EntityFrameworkCore.Tools 

- Microsoft.AspNetCore.Authentication.JwtBearer 

- Swashbuckle.AspNetCore 

## **3. How to Run** 

### **Prerequisites** 

- .NET 8 SDK and Visual Studio 2022 

- Microsoft SQL Server and SQL Server Management Studio (SSMS) 

- Node.js 20.15.1, npm 10.7.0 and Angular CLI 17.3.17 

### **Database setup** 

The SQL Server database is named **TicketDeskDb** . A backup is included in the repository root as **TicketDeskDb.bak** . 

- In SSMS, right-click Databases and select Restore Database. 

- Choose Device, browse to TicketDeskDb.bak, and complete the restore. 

- Use the database name expected by your connection string, or update the connection string to match the restored database. 

Support Ticket Desk — Assessment README 

Page 1 

EF Core migrations are in **TicketDesk.API/Migrations/** . To create the database from migrations, open the solution in Visual Studio, configure the connection string, select TicketDesk.API as the Package Manager Console default project, and run: 

```
Update-Database
```

At application startup, the initializer creates the Employee, Agent and Manager accounts if the user table is empty. 

### **Configuration** 

Update **TicketDesk.API/appsettings.Development.json** with your SQL Server connection string and configure JWT settings with your own signing key. Keep development secrets out of GitHub. 

### **Start the backend** 

Open TicketDesk.sln in Visual Studio 2022, set TicketDesk.API as the startup project, and run it. Swagger: **https://localhost:7143/swagger** . 

### **Start the frontend** 

Open a terminal in **ticket-desk-ui** and run: 

```
npm install
```

```
ng serve --open
```

Angular URL: **http://localhost:4200** . 

## **4. Seed Credentials** 

These accounts are created automatically when the application starts with an empty user table. 

|**Role**|**Username**|**Password**|
|---|---|---|
|Employee|employee|Employee@123|
|Agent|agent|Agent@123|
|Manager|manager|Manager@123|



## **5. Design Decisions** 

### **Twist 1: Double Pick-up** 

I used a conditional database update with Entity Framework Core's ExecuteUpdateAsync(). The ticket is updated only when its status is Open and it has no assigned agent. If two agents try to pick up the same ticket, only one update succeeds. The second agent receives a failure response because no matching ticket remains available. 

### **Twist 2: Audit Trail** 

A separate TicketStatusHistory table records the ticket ID, old status, new status, user who made the change, and timestamp. Managers can view history through **GET /api/Manager/tickets/{ticketId}/history** . This keeps earlier status changes available even after the current status changes. 

### **Twist 3: Re-open** 

Managers can reopen a ticket only if it is Closed and was closed within the last seven days. Reopening changes its status to Open, clears ClosedAt, and updates UpdatedAt. Tickets closed more than seven days ago, or tickets that are not Closed, are rejected. The current implementation retains the previous agent assignment when reopening. 

### **Summary for Managers** 

The manager summary uses Entity Framework Core count queries. It returns total tickets, counts by status, and overdue ticket count. A ticket is considered overdue when it was created more than 48 hours ago and is neither Resolved nor Closed. Counts are calculated in the database instead of loading all tickets into memory. 

### **Assumptions** 

- Ticket timestamps are stored in UTC. 

- Only the employee who created a ticket can close it. 

- Employees can close tickets only after they are Resolved. 

Support Ticket Desk — Assessment README 

Page 2 

- Agents can pick up tickets that are Open and unassigned. 

- Managers can reopen closed tickets within seven days. 

- Tickets older than 48 hours that are not Resolved or Closed are considered overdue. 

- JWT claims identify users and enforce role-based access. 

## **6. Status** 

|**Item**|**Status**|**Notes**|
|---|---|---|
|Login and role-based access|Done|JWT authentication and role authorization|
|Create and list tickets|Done|Creation, filtering and pagination APIs|
|Pick up a ticket|Done|Conditional update prevents double pick-up|
|Status changes with rules|Done|Status checks in service methods|
|Close a ticket|Done|Employees close resolved tickets they created|
|Manager summary|Done|Counts by status and overdue count|
|Twist 1: Double pick-up|Done|Only one agent succeeds|
|Twist 2: Audit trail|Done|Status changes stored in history table|
|Twist 3: Re-open|Done|Managers can reopen within seven days|
|Angular screens (bonus)|Partial|Login, employee dashboard and create-ticket screen|
|Stretch goals|Not done|No additional stretch goals implemented|



### **Known Issues** 

- The Angular frontend currently focuses on Employee functionality; Agent and Manager workflows are available through backend APIs. 

- The logout button in the current frontend is not fully connected to a logout workflow. 

- Reopened tickets retain their previous agent assignment. 

## **7. If I Had More Time** 

- I would complete the Angular screens for Agent and Manager roles. 

- I would improve frontend validation, error handling and loading states. 

- I would add automated unit and integration tests for the main ticket workflows. 

## **8. Submission Checklist** 

- Final code pushed to GitHub. 

- Database backup included in the repository. 

- EF Core migrations included. 

- Seed credentials listed above. 

- No development connection strings or JWT signing secrets intentionally included in the repository. 

- Fresh clone tested using only this README: not yet confirmed. 

Support Ticket Desk — Assessment README 

Page 3 

