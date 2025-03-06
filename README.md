# Bookify - Modern Apartment Booking System

## Overview

Bookify is a sophisticated booking platform built with Clean Architecture principles, designed for efficient apartment reservation management. It leverages Command Query Responsibility Segregation (CQRS) to separate read and write operations, integrates with external services like Keycloak for authentication, and utilizes advanced technologies such as Redis for caching and Jaeger for distributed tracing. The architecture emphasizes scalability, testability, and long-term maintainability.

## Key Features

### Core Architecture
- **Rich Domain Models**
  - Business logic encapsulated within domain entities (`Apartment`, `Booking`, `User`)
  - Value objects and domain services for complex business rules

- **CQRS Implementation**
  - Command handlers for write operations (e.g., `ReserveBookingCommand`)
  - Query handlers for read operations (e.g., `SearchApartmentsQuery`)
  - Clear separation of concerns for improved scalability

### Performance & Reliability
- **Background Processing**
  - Scheduled tasks via Quartz for booking reminders and maintenance
  
- **Caching Strategy**
  - Distributed caching with Redis for shared data
  - In-memory caching for frequently accessed resources
  - HTTP caching to reduce server load

- **Observability**
  - Distributed tracing with Jaeger for cross-service request flows
  - Structured logging with Serilog
  - Log aggregation via Seq for centralized analysis

### Security
- **Authentication**
  - Integration with Keycloak as identity provider
  - JWT token management

- **Authorization**
  - Role-based access control
  - Permission-based authorization
  - Resource-based security

### Quality Assurance
- **Comprehensive Testing Suite**
  - Unit tests for domain and application logic
  - Integration tests with containerized dependencies
  - End-to-end functional tests
  - Architecture tests via NetArchTest to enforce design constraints

### API Design
- **Versioning**
  - Support for multiple API versions to maintain backward compatibility
  
- **Documentation**
  - Interactive Swagger UI for testing and exploration
  
- **Health Monitoring**
  - Dedicated `/health` endpoint for infrastructure status checks

## Implementation Roadmap

- [x] Core domain model implementation
- [x] CQRS architecture setup
- [x] Authentication integration with Keycloak
- [x] Basic API endpoints
  - [x] Authentication (Login/Register)
  - [x] Apartment search
  - [x] Booking creation
- [ ] Advanced API endpoints
  - [ ] User management
  - [ ] Booking operations
  - [ ] Reviews and feedback
  - [ ] Administrative controls
- [ ] Event Sourcing for audit trails
- [x] HATEOAS implementation for API discoverability
- [ ] OpenTelemetry integration
- [ ] Message Queue integration
- [ ] API rate limiting and throttling
- [ ] Content negotiation
- [ ] Internationalization (i18n)
- [ ] Webhook support for external integrations
- [ ] Data shaping for response customization
- [ ] Search engine integration

## API Documentation

### Authentication & Authorization
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/auth/login` | POST | Authenticate users and return JWT | ✅ |
| `/auth/register` | POST | Register new users | ✅ |
| `/auth/refresh` | POST | Refresh authentication tokens | ❌ |
| `/auth/logout` | POST | Invalidate current session | ❌ |

### User Management
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/users/me` | GET | Get current user profile | ❌ |
| `/users/me` | PUT | Update user profile | ❌ |
| `/users/{id}` | GET | Get user by ID | ❌ |
| `/users` | GET | List users | ❌ |

### Apartment Listings & Search
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/apartments` | GET | List available apartments | ✅ |
| `/apartments/{id}` | GET | Get apartment details | ✅ |
| `/apartments/search` | POST | Advanced search | ❌ |
| `/apartments` | POST | Create new apartment listing | ✅ |
| `/apartments/{id}` | PATCH | Update apartment listing | ✅ |
| `/apartments/{id}` | DELETE | Delete apartment listing | ❌ |

### Booking Operations
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/bookings` | GET | List user bookings | ❌ |
| `/bookings/{id}` | GET | Get booking details | ❌ |
| `/bookings` | POST | Create booking | ✅ |
| `/bookings/{id}` | PUT | Update booking | ❌ |
| `/bookings/{id}` | DELETE | Cancel booking | ❌ |
| `/bookings/{id}/reserve` | POST | Reserve booking | ❌ |

### Reviews & Feedback
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/reviews` | GET | List reviews | ❌ |
| `/reviews/{id}` | GET | Get review details | ❌ |
| `/reviews` | POST | Create review | ❌ |
| `/reviews/{id}` | PUT | Update review | ❌ |
| `/reviews/{id}` | DELETE | Delete review | ❌ |

### Administrative Endpoints
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/admin/dashboard` | GET | System metrics | ❌ |
| `/admin/users` | GET | Manage users | ❌ |
| `/admin/users/{id}/role` | PUT | Update user roles | ❌ |
| `/admin/bookings` | GET | Manage bookings | ❌ |

### Advanced Features
| Endpoint | Method | Description | Status |
|----------|--------|-------------|--------|
| `/statistics` | GET | System analytics | ❌ |
| `/notifications` | GET | User notifications | ❌ |
| `/webhooks` | POST | Register webhooks | ❌ |
| `/settings` | GET | App settings | ❌ |

## Project Structure

```text
bookify/
├── README.md
├── Bookify.sln
├── docker-compose.yml
├── src/
│   ├── Bookify.API/               # API controllers and endpoints
│   ├── Bookify.Application/       # CQRS handlers and services
│   ├── Bookify.Domain/            # Entities and business logic
│   └── Bookify.Infrastructure/    # External integrations
├── test/
│   ├── Bookify.Api.FunctionalTests/
│   ├── Bookify.Application.IntegrationTests/
│   ├── Bookify.Application.UnitTests/
│   ├── Bookify.ArchitectureTests/
│   └── Bookify.Domain.UnitTests/
└── .github/workflows/             # CI/CD pipeline configuration

```
