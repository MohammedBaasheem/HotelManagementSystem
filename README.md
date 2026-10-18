# 🏨 Hotel Management System API

## 📌 Project Overview
**Hotel Management System** is an enterprise-grade backend RESTful API designed to automate and streamline core hotel operations. Built with **ASP.NET Core** and adhering to **Clean Architecture** principles, the platform delivers a reliable, secure, and maintainable foundation for managing hospitality workflows including bookings, rooms, guests, invoicing, and promotional offers.

### ✨ Key Features
- **Room Management:** Comprehensive room tracking, categorization, amenities management, and real-time availability status.
- **Reservation Lifecycle:** Dynamic booking workflows, check-in/out scheduling, room assignments, and flexible stay pricing.
- **Offers & Promotions:** Advanced promotion and discount engine targeting specific rooms, seasons, and reservation windows.
- **Invoicing & Payments:** Detailed invoice breakdown (taxes, room rates, deposits, discounts) and multi-method payment records.
- **User & Access Management:** Secure Role-Based Access Control (RBAC) powered by JWT authentication and claims.
- **Auditing & Soft Deleting:** Comprehensive audit trail tracking (`CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`) with safe soft-deleting mechanisms.

---

## 👨‍💻 My Role & Key Contribution: Offers Management Module
As part of this collaborative team project, I had full ownership of designing, engineering, and integrating the **Offers & Promotions Module** end-to-end across all layers of the Clean Architecture:

### 1. Domain Modeling & Business Rules
- Designed the core `Offer` entity and the many-to-many junction entity `RoomOffer` to associate promotional campaigns with specific rooms.
- Integrated `OfferId` relations into the reservation structure (`RoomReservation`) to dynamically calculate discounts.
- Created strongly-typed domain error codes (`ErrorCode.cs`) specifically for offer-related exceptions (e.g., date conflicts, missing requirements, invalid discount values).

### 2. Application Services & Business Logic
- Developed `OfferManagmentService` handling complete CRUD workflows with audit metadata capture from JWT claims (`CurrentUserId`).
- Implemented `CodeOfferGenerator` to generate unique random promo codes (`OFFER-XXXX`) with automated database collision checks.
- Built an advanced multi-criteria dynamic filtering engine with EF Core `IQueryable`:
  - Search by code or description text (`SearchText`).
  - Active-period filtering (`IsActiveNow`) compared with UTC server time.
  - Date interval overlap (`SearchDate` between `StartDate` and `EndDate`).
  - Min/Max discount range filtering and sorting by discount value and creation date.
- Integrated **FluentValidation** (`CreateOfferValidator`) enforcing strict business rules (e.g., end date > start date, positive discount value).
- Implemented **AutoMapper** profile mappings (`OfferProfile`) with custom projections for room IDs.

### 3. Data Access & Performance Optimization
- Implemented `OfferManagmentRepository` inheriting from `GeneralRepository<Offer>` and `IOfferManagmentRepository`.
- Utilized EF Core's modern `ExecuteDeleteAsync` for high-performance bulk deletion of junction records during room updates without loading entities into memory.
- Enforced **Soft Delete** logic to safeguard historical reservation references from broken relationships.

### 4. API Endpoints & Presentation Layer
- Designed and documented RESTful endpoints in `OfferController`:
  - `GET /api/Offer/{id}` – Fetch offer details by ID.
  - `GET /api/Offer/all` – Retrieve all offers.
  - `GET /api/Offer/filter` – Dynamic multi-parameter search and filter.
  - `POST /api/Offer` – Create a new offer with automated code generation.
  - `PUT /api/Offer` – Update existing offer and associated room links.
  - `DELETE /api/Offer/{id}` – Soft delete an offer.
- Maintained a clean boundary between API contracts (`ViewModels`) and domain logic (`DTOs`) through dedicated mapping profiles.

---

## 🛠️ Tech Stack & Architecture
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Presentation)
- **Framework:** .NET 8 / ASP.NET Core Web API
- **ORM & Database:** Entity Framework Core, Microsoft SQL Server
- **Patterns & Principles:** Repository Pattern, Dependency Injection (Autofac), Separation of Concerns (SoC), Soft Delete
- **Libraries & Tools:** FluentValidation, AutoMapper, Swagger / OpenAPI

---

## 👥 Team Collaboration
This system was built collaboratively by a development team. Each member held ownership over dedicated business modules, with my responsibility centered on leading the architecture, business logic, and API delivery for the **Offers & Promotions Module**.
