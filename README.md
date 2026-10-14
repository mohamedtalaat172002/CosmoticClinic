# Cosmetic Clinic

An ASP.NET Core 8 cosmetic-clinic solution with a public MVC website, an admin dashboard, and a REST API. The solution follows a Clean Architecture-style separation between domain rules, application use cases, and infrastructure concerns.

## Projects

| Project | Responsibility |
| --- | --- |
| `Clinic.Domain` | Core entities, enums, and domain abstractions. |
| `Clinic.Application` | Use cases implemented with MediatR commands/queries, DTOs, validation, and mapping. |
| `Clinic.Infrastructure` | EF Core persistence, repositories, Identity, Cloudinary image storage, and other implementations. |
| `Clinic.Api` | REST API and Swagger UI for external clients. |
| `Clinic.web` | MVC website: public pages, localization, authentication, and the admin dashboard. |

## Features

- **Public clinic website** with a polished, responsive experience for treatments, articles, clinic details, contact, and consultation requests.
- **Treatment catalogue** with search, pagination, SEO-friendly slugs, treatment detail pages, and related articles.
- **Article / clinic journal** with published-draft status support, search, pagination, article details, and slug-based URLs.
- **Consultation requests** collected from the public website and managed from the administrative area.
- **Clinic settings** for clinic name, doctor information, specialization, contact data, address, and social-media links.
- **Administrative dashboard** for managing clinic settings, treatments, articles, and consultation requests.
- **Role-based authorization** for administration features using ASP.NET Core Identity.
- **Authentication workflow** with login, refresh tokens, logout, reset-password support, and JWT-based API authentication.
- **Arabic and English localization** for the public MVC experience, including culture switching through a request-culture cookie.
- **Multi-image support** for treatments, articles, and clinic hero media.
  - Images have `DisplayOrder` and `IsPrimary` metadata.
  - The primary image is used on listing/home cards.
  - All images are displayed on detail pages in Bootstrap carousels with navigation controls.
  - Uploading a new image collection replaces the existing gallery during an update.
- **Cloudinary image hosting** through the Infrastructure image-storage abstraction. Images are uploaded to Cloudinary folders for treatments, articles, and clinic settings; the returned URL and Cloudinary public ID are persisted for later display and deletion.
- **Image lifecycle management**: old Cloudinary assets are deleted when a gallery is replaced or its owning treatment/article is deleted.
- **REST API** organized with versioned routes under `Api/V1`, plus Swagger/OpenAPI documentation in Development.
- **Validation pipeline** using FluentValidation and MediatR behaviors for application commands.
- **Repository and Unit of Work patterns** for persistence access and transactional saves.
- **Entity Framework Core + SQL Server** for data access and schema management.
- **Structured logging** with Serilog and optional Seq integration.

## Prerequisites

- .NET 8 SDK
- SQL Server (or SQL Server Express/LocalDB)
- A Cloudinary account/configuration when image upload is used

## Configuration

Configure the following in `Clinic.Api/appsettings.json` and `Clinic.web/appsettings.json` (or preferably in User Secrets/environment variables):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<your SQL Server connection string>"
  },
  "jwtSittings": {
    "key": "<strong secret key>",
    "Issuer": "ClinicIssuer",
    "Audience": "ClinicAudience",
    "DurationInHours": 24
  }
}
```

Add the Cloudinary settings expected by the infrastructure image-storage implementation before uploading images. Never commit production connection strings, JWT secrets, or Cloudinary credentials.

### Cloudinary

The project uses Cloudinary for image hosting rather than storing uploaded assets in `wwwroot`. Configure the Cloudinary values used by the Infrastructure implementation in secrets or environment variables. At minimum, this typically includes:

```json
{
  "Cloudinary": {
    "CloudName": "<cloud name>",
    "ApiKey": "<API key>",
    "ApiSecret": "<API secret>"
  }
}
```

The application stores each asset's delivery URL and Cloudinary public ID. This makes public rendering fast while still allowing the application to remove old remote files safely.

## Database

Create/apply the database migrations from the project that owns the EF Core `DbContext`:

```powershell
dotnet ef database update --project Clinic.Infrastructure --startup-project Clinic.Api
```

If your migrations are created from another startup project, use that project instead.

## Run the MVC Website

`Clinic.web` references the Application and Infrastructure layers directly, so it can be run on its own:

```powershell
dotnet run --project Clinic.web
```

Default HTTP URL: `http://localhost:5027`

## Run the API

```powershell
dotnet run --project Clinic.Api
```

Default HTTP URL: `http://localhost:5298`  
Swagger in Development: `http://localhost:5298/swagger`

## Build

```powershell
dotnet restore
dotnet build Clinic.sln
```

## Main Routes

| Area | Route |
| --- | --- |
| Home | `/` |
| Treatments | `/Treatments` |
| Treatment details | `/treatments/{slug}` |
| Articles | `/Articles` |
| Article details | `/articles/{slug}` |
| Consultation | `/Consultations/Create` |
| Admin dashboard | `/Dashboard` |

## API Areas

The API exposes versioned endpoints under `Api/V1` for treatments, articles, consultations, and clinic settings. Authentication endpoints are exposed under `api/Authentication`.

## Development Notes

- Keep domain/business behavior in `Clinic.Domain` and `Clinic.Application` rather than MVC or API controllers.
- `Clinic.web` is responsible for presentation concerns only.
- API and MVC use the same Application and Infrastructure layers.
- Image galleries are represented by image collections; the primary image is selected using `IsPrimary`.
- Image upload inputs belong to MVC/API request models; streams are passed to Application commands, while the Cloudinary-specific work remains in Infrastructure.
- Treat Cloudinary credentials, SQL connection strings, and JWT keys as secrets and configure them outside source control for real deployments.
- Keep controllers thin: they should coordinate requests and responses, while validation and business decisions belong in the Application layer.
