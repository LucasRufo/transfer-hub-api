# TransferHub API

<p align="center">
  <img src="./images/transfer-hub-icon.png" />
</p>

TransferHub is an HTTP API that exposes endpoints for participant registration, individual account deposits, statements, and transfers between participants. My intention with TransferHub is to showcase a production-level project on GitHub, utilizing the tools with which I have the most experience.

Feel free to leave suggestions or to make a contribution.

## Contents

- [Tech Stack](#tech-stack)
- [Running locally](#running-locally)
- [Architecture](#architecture)
- [Endpoints](#endpoints)
- [Next steps](#next-steps)
- [License](#license)

## Tech stack

The project is using .NET 8 with Minimal APIs and PostgreSQL for the database. It has unit and integration tests using NUnit and uses some common libraries from the .NET ecosystem like Entity Framework, Fluent Validation, Fluent Assertions and Bogus.

For the integration tests, it uses the built-in `WebApplicationFactory` from .NET and uses [TestContainers](https://testcontainers.com/) to spin up a PostgreSQL Docker container.

As for database migrations, I've opted to create a separate project that acts as a Command Line Tool, facilitating the process of running migrations on a pipeline. The project is using Fluent Migrator to describe and execute the migrations.

This project offers support for running inside Kubernetes, it has a Helm chart for the API inside the `helm` folder. 

## Running locally

This project can run locally in two ways:

### 1. Kubernetes with Kind

The first way is running the project inside a local Kubernetes cluster using [Kind](https://kind.sigs.k8s.io/) and [Kindxt](https://github.com/sergioprates/kindxt). To run using this option you need Docker, Helm, Kind, Kindxt and Powershell.

Open a terminal, navigate to the `scripts` folder and run the `kind-dev-up.ps1` script. 

```bash
kind-dev-up.ps1
```

The script has the following steps to get the dev environment up and running:

- Create the Kubernetes cluster using Kindxt with two dependencies: Nginx Ingress and PostgreSQL
- Build the API Docker image
- Load the image to Kind
- Execute the migrations pointing to the PostgreSQL database inside the Kubernetes cluster.
- Install the API helm chart

### 2. Using Visual Studio or .NET CLI

This is an easier way of setting up the environment and it requires only Docker and Powershell. This option only runs a PostgreSQL docker container and the migrations, so you can run the API using Visual Studio or the .NET CLI using the `dotnet run` command.

Open a terminal, navigate to the `scripts` folder and run the `db-up.ps1` script. 

```bash
db-up.ps1
```

## Architecture

<p align="center">
  <img src="./images/dependencies-diagram.png" />
</p>

This diagram shows all the dependencies and how they interact, the Domain layer is isolated to promote maintainability and testability. As for database interactions, only the infrastructure and the migrations project can create database connections. 

## Endpoints

### Participants

#### Create Participant

Creates a new participant. 

```
  POST /api/v1/participants
```

#### Statement

Gets a statement with transactions from a participant.

```
  GET /api/v1/participants/{id}/statement?page=1&pageSize=20
```

### Transactions

#### Credit 

Creates a new deposit for a participant.

```
  POST /api/v1/transactions/credit
```

#### Transfer

Transfers money from one participant to another.

```
  POST /api/v1/transactions/transfer
```

## Next steps

- Create Load Testing scripts using [K6](https://k6.io/). My goal is to learn some strategies for load testing and how to execute them.
- Create a production environment using [Terraform](https://www.terraform.io/) on Azure. I want to learn how to use an infrastructure-as-code tool, and Terraform looks like a good option.

## License

[MIT License](https://lucasrufo.mit-license.org/) © Lucas Rufo
