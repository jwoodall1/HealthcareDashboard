# Healthcare Provider Dashboard 🏥

A full-stack MVC application built to manage healthcare providers, patient demographics, and clinical encounters. I developed this project to get hands-on experience with enterprise-level design patterns, relational database architecture, and containerized development. 

## 🚀 What I Built

* **Relational Data Mapping:** Engineered a SQL database to track complex relationships between Patients, Providers, and Encounters using Entity Framework Core's eager loading (`.Include()`).
* **Automated Audit Trails:** Built a custom `IAuditable` interface that intercepts database saves to automatically stamp `CreatedAt` and `UpdatedAt` times on critical clinical records. 
* **Full CRUD Functionality:** Created secure, server-side rendered forms for managing network data, protected by built-in anti-forgery tokens.
* **Modern UI:** Styled a clean, responsive interface using Bootstrap, customized with a corporate-style color palette.

## 💻 Tech Stack

* **Backend:** C#, ASP.NET Core MVC (.NET 8)
* **Database:** Microsoft SQL Server (Containerized via Docker)
* **ORM:** Entity Framework Core (Code-First)
* **Frontend:** Razor Pages, HTML5, CSS3, Bootstrap 5

## 🚦 How to Run It Locally

If you want to spin this up on your own machine, you will need the .NET 8 SDK and Docker Desktop installed.

**1. Boot up the SQL Database**
I used Docker Compose to keep the local environment clean. Run this to spin up the SQL Server container in the background:
```bash
docker-compose up -d db