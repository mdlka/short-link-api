# ShortLink API

A simple API for shortening URLs. This API allows users to generate a short code for any valid URL and then redirect from that code to the original URL.

## How it works

1. The user sends a URL to the `/shorten/` endpoint.
2. The API generates a unique short code and stores it in the database.
3. When someone accesses `/{code}`, they are redirected to the original URL.
4. The system uses caching to optimize lookups and reduce database load.

## Tech stack

- **.NET 8.0**
- **ASP.NET Core Web API 8.0**
- **Entity Framework Core 9.0.3**
- **PostgreSQL 17.4**
- **Redis 7.4.2**
- **Docker & Docker Compose**
- **Swashbuckle (Swagger 7.3.1)**
- **xUnit 2.5.3**
- **Testcontainers for .NET 4.3.0**

## Features

- **URL Shortening:** Create a unique short code for any valid URL.
- **Redirection:** Redirects a short code to the corresponding original URL.
- **Caching:** Uses Redis for caching to improve lookup performance.
- **Persistence:** PostgreSQL database for storing URLs.
- **API Documentation:** Swagger UI is enabled in development mode.

## Endpoints

### GET `/{code}`

- **Description:** Redirects to the original URL corresponding to the provided short code.
- **Response:**
  - **Redirect (HTTP 302):** When a matching URL is found.
  - **Not Found (HTTP 404):** When no URL matches the provided short code.

### POST `/shorten/`

- **Description:** Accepts a JSON object containing a URL and returns a shortened URL object.
- **Request Body:**
  ```json
  {
    "url": "https://example.com"
  }
  ```
- **Response:**
  ```json
  {
    "id": "0195ccaf-8995-7277-a1f3-74d1a74b4b4f",
    "originalUrl": "https://example.com",
    "shortCode": "bGWrsS"
  }
  ```
  - **Created (HTTP 201):** When the short URL is successfully created.
  - **Bad Request (HTTP 400):** When the input URL is invalid.

## Setup & Running

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) & [Docker Compose](https://docs.docker.com/compose/)

### Running with Docker Compose

1. **Development Environment:**

   - Uses `docker-compose.yml`.
   - Command:
     ```bash
     docker-compose up --build
     ```
   - Exposes the API on port `8080`.
   - Access the Swagger UI at `/swagger`.

2. **Production Environment:**

   - Uses `docker-compose.yml` and `docker-compose.prod.yml`.
   - Make sure to update environment variables as needed.
   - Command:
     ```bash
     docker-compose -f ./docker-compose.yml -f ./docker-compose.prod.yml up --build
     ```

## What could be improved

- Move sensitive data and environment-specific settings to a `.env` file or similar.
- Remove runtime migration execution from production; use a dedicated migration pipeline.
- Refactor repeated configurations and constants.
- Implement rate limiting to prevent abuse of the API.
- Implement authentication & authorization for managing short links.

## License

This project is licensed under the [MIT License](LICENSE).

