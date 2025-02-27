# Bookify Booking System

## Overview

`Bookify` is a modern booking system built with Clean Architecture principles, designed for managing apartment reservations. It leverages CQRS for separation of concerns, integrates with external services like Keycloak for authentication, and uses technologies such as Redis for caching and Jaeger for distributed tracing. The project emphasizes scalability, testability, and maintainability.
## Project
- **Rich Domain Models**  
  - Business logic is encapsulated within the domain layer for better cohesion and maintainability.
  - Entities like `Apartment`, `Booking`, and `User` encapsulate business logic.
- **CQRS**  
  - Separation of commands and queries improves scalability and clarity.
  - Commands (e.g., `ReserveBookingCommand`) and queries (e.g., `SearchApartmentsQuery`) are separated.
- **Background Jobs**  
  Tasks such as reminders or cleanup are scheduled using Quartz.
- **Distributed Tracing**  
  Jaeger monitors request flows across services for improved observability.
- **Caching**  
  - **Distributed Caching:** Uses Redis for high-speed data retrieval.
  - **In-memory Caching:** Enhances performance with local caching.
- **HTTP Caching**  
  Reduces server load by caching responses.
- **Authentication**  
  Managed via Keycloak, an external identity provider.
- **Authorization**  
  Supports role-based, permission-based, and resource-based access controls.
- **Testing**  
  - **Unit Tests:** Validates domain and application logic.
  - **Integration Tests:** Utilizes Docker containers.
  - **Functional Tests:** Provides end-to-end validation.
  - **Architecture Tests:** Uses NetArchTest to enforce design rules.
- **Logging**  
  Structured logging with Serilog and log aggregation with Seq ensures clear and actionable logs.
- **Health Checks**  
  Accessible at `/health` to monitor the status of the database, Redis, and Keycloak.
- **API Versioning**  
  Maintains backward compatibility as the API evolves.
- **API Documentation**  
  Interactive documentation and testing capabilities are provided via Swagger.
## Planned Features
The following features are in progress to further enhance system capabilities:
- **API Development**
  
- **Event Sourcing**  
  Implements auditing and tracks historical changes (e.g., reservation updates).
- **HATEOAS**  
  Adds hypermedia links in API responses to guide clients on available actions.
- **OpenTelemetry**  
  Offers advanced monitoring and tracing across distributed systems.
- **Message Queue**  
  Enables asynchronous processing for tasks like notifications or payments.
- **Throttling and Rate Limiting**  
  Prevents API abuse and ensures fair usage.
- **Content Negotiation**  
  Supports multiple response formats (e.g., JSON, XML).
- **I18n (Internationalization)**  
  Provides multi-language support for global accessibility.
- **Webhooks**  
  Sends real-time notifications to external systems for events such as new reservations.
- **Data Shaping**  
  Allows clients to specify which fields to include in API responses.
- **Search Engine Integration**  
  Incorporates advanced search capabilities using Lucene .NET or ElasticSearch.
# API Developments

## Authentication & Authorization

### POST /auth/login
Authenticate users by accepting credentials (e.g., email and password) and returning a JWT or session token. (done)

### POST /auth/register
Register a new user with required details and send a confirmation email if needed. (done)

### POST /auth/refresh
Allow clients to refresh their tokens for continued access without re-login.

### POST /auth/logout
Invalidate the current token or session, logging the user out securely.

---

## User Management

### GET /users/me
Retrieve the profile of the currently authenticated user.

### PUT /users/me
Update profile details such as name, email, or preferences.

### GET /users/{id}
For admin or profile-viewing purposes, retrieve a user’s details by ID.

### GET /users
List all users (typically restricted to admin roles or for public profiles with limited info).

---

## Apartment Listings & Search

### GET /apartments
Return a list of available apartments. Supports filtering by location, price, amenities, etc. (done)

### GET /apartments/{id}
Retrieve detailed information for a specific apartment.

### POST /apartments/search
Offer a flexible search endpoint where clients can send complex query parameters (e.g., date ranges, ratings, proximity).

---

## Booking Operations

### GET /bookings
List all bookings for the authenticated user, with options for filtering by status (upcoming, past, etc.).

### GET /bookings/{id}
Retrieve details of a particular booking.

### POST /bookings
Create a new booking. Include details like apartment ID, date range, and any special requirements. (done)

### PUT /bookings/{id}
Update an existing booking. This endpoint might allow changing dates or adding special requests (depending on business rules).

### DELETE /bookings/{id}
Cancel a booking. Consider soft deletion or providing a cancellation reason.

### POST /bookings/{id}/reserve
A specialized endpoint for reserving a booking before final confirmation (can include temporary hold logic and payment integration).

---

## Reviews & Feedback

### GET /reviews
Retrieve reviews, optionally filtered by apartment or user.

### GET /reviews/{id}
Get details for a specific review.

### POST /reviews
Submit a new review. Include fields like rating, comments, and associated apartment ID.

### PUT /reviews/{id}
Edit an existing review if allowed (e.g., within a certain time frame).

### DELETE /reviews/{id}
Remove a review. This might be allowed for the author or an admin moderator.

---

## Administrative Endpoints

### GET /admin/dashboard
Provides a snapshot of system metrics such as recent bookings, user sign-ups, and revenue.

### GET /admin/users
List all users with administrative filters and search capabilities.

### PUT /admin/users/{id}/role
Update a user's role or permissions, managing access control centrally.

### GET /admin/bookings
Retrieve all bookings in the system for oversight and management.

---

## Additional & Advanced Features

### GET /statistics
Provide aggregated data like booking trends, revenue analytics, and user activity over time.

### GET /notifications
List notifications for a user, such as booking confirmations, reminders, or promotional offers.

### POST /webhooks
Allow external systems to subscribe to events (e.g., booking confirmation, cancellations) by registering webhook endpoints.

### GET /settings
Retrieve application settings or user-specific preferences, which could be useful for a customizable user experience.

## Directory Structure
```text
loingtan-clean_architecture_booking_application/
├── README.md
├── Bookify.sln
├── LICENSE.txt
├── docker-compose.dcproj
├── docker-compose.override.yml
├── docker-compose.yml
├── launchSettings.json
├── .dockerignore
├── src/
│   ├── Bookify.API/               # Presentation layer: API endpoints and controllers
│   ├── Bookify.Application/       # Application layer: CQRS commands, queries, and services
│   ├── Bookify.Domain/            # Domain layer: Entities, value objects, and business logic
│   └── Bookify.Infrastructure/    # Infrastructure layer: External service implementations
├── test/                          # Test projects for all layers
│   ├── Bookify.Api.FunctionalTests/
│   ├── Bookify.Application.IntegrationTests/
│   ├── Bookify.Application.UnitTests/
│   ├── Bookify.ArchitectureTests/
│   └── Bookify.Domain.UnitTests/
└── .github/
└── workflows/
└── dotnet.yml             # CI/CD pipeline configuration
```
