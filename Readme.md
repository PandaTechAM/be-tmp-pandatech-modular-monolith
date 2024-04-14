﻿# Pandatech.ModularMonolith - Modular Monolith Application

## Introduction

This project serves as a template for creating .NET Web API applications. It's pre-configured with essential
dependencies and a Docker setup for ease of development and deployment. The template is designed to be flexible,
allowing easy removal of unnecessary components and addition of new features.

## Project Structure

**Root Directory:** Contains the solution file for the project.

**src Folder:** Houses all the source code and module related tests.

**tests Folder:** Contains e2e tests.

**Docker File** Located at the root, used for creating a Docker container for the application. `Dockerfile.Local`
and `docker-compose` are for local development.

## External Dependencies

This project integrates various services and configurations:

**Postgres:** Used as the primary database.

**Redis:** Implemented for caching purposes.

**RabbitMQ (RMQ):** Utilized for messaging and event-driven architecture.

**Elasticsearch:** Employed for logging advanced search capabilities.

**appsettings{environment}.json:** Configuration settings for the application.

## Project built-in features

**Integration tests setup:** The project is pre-configured with integration tests.
**Health checks:** The project has health checks for the database and other services. Also endpoint for prometheus.
**OpenTelemetry:** The project is pre-configured with OpenTelemetry for logging and tracing.
**Prometheus:** The project is pre-configured with Prometheus for monitoring.
**Hangfire** The project is pre-configured with Hangfire for background jobs.
**ResponseCrafter** The project is pre-configured with ResponseCrafter for consistent exception handling.
**Serilog** The project is pre-configured with Serilog for logging. It asynchronously logs to Elasticsearch.
**RegexBox** The project is pre-configured with RegexBox which will provide all essential regex patterns.

## Getting Started

### Pre-requisites:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)

### Basic Commands

- `dotnet restore` - Restore dependencies
- `dotnet build` - Build the project
- `dotnet run` - Run the project
- `dotnet test` - Run tests
- `docker-compose up` - Run the project in Docker

## Features

List the key features of your application.