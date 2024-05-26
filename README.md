# netcore-boilerplate

Boilerplate of API in ~~`.NET Core 3.1`~~ `.NET 8`

| GitHub | Codecov |
|:-------------:|:-------------:|
| [![Build & Test](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/dotnetcore.yml) [![Build docker image](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/docker.yml/badge.svg)](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/docker.yml) | [![codecov](https://codecov.io/gh/lkurzyniec/netcore-boilerplate/branch/master/graph/badge.svg)](https://codecov.io/gh/lkurzyniec/netcore-boilerplate) |

Boilerplate is a piece of code that helps you to quickly kick-off a project or start writing your source code. It is kind of a template - instead
of starting an empty project and adding the same snippets each time, you can use the boilerplate that already contains such code.

**Intention** - The intention behind this project is to mix a variety of different approaches to show different available paths.
That's why you can find here the Service approach mixed-up with Repository approach, or old-fashioned controllers mixed-up with
brand new minimal API in a separate module (modular approach). As well as, it's a kind of playground for exploring frameworks, packages, tooling.
At the end, You are in charge, so it's your decision to which path you would like to follow.

## Source code contains

1. [Central Package Management (CPM)](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
1. ~~[Autofac]~~(https://autofac.org/) (Removed in [PR19](https://github.com/lkurzyniec/netcore-boilerplate/pull/19))
1. [Swagger](https://swagger.io/) + [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
1. [FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) (Feature Flags, Feature Toggles)
1. [HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)
1. [EF Core](https://docs.microsoft.com/ef/)
    * [MySQL provider from Pomelo Foundation](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
    * [MsSQL from Microsoft](https://github.com/aspnet/EntityFrameworkCore/)
1. [Dapper](https://github.com/DapperLib/Dapper)
    * [Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/)
1. Tests
    * Integration tests with InMemory database
        * [FluentAssertions]
        * [xUnit]
        * [Verify](https://github.com/VerifyTests/Verify/)
        * [Verify.Http](https://github.com/VerifyTests/Verify.Http)
        * TestServer
    * Unit tests
        * [AutoFixture](https://github.com/AutoFixture/AutoFixture)
        * [FluentAssertions]
        * [Moq](https://github.com/moq/moq4)
        * [Moq.AutoMock](https://github.com/moq/Moq.AutoMocker)
        * [xUnit]
    * Architectural tests (conventional tests)
        * [NetArchTest](https://github.com/BenMorris/NetArchTest)
        * [xUnit]
    * ~~Load tests~~ (Removed in [PR135](https://github.com/lkurzyniec/netcore-boilerplate/pull/135))
        * ~~[NBomber]~~(https://nbomber.com/)
1. Code quality
    * [EditorConfig](https://editorconfig.org/) ([.editorconfig](.editorconfig))
    * Analyzers
        * [Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers)
        * [Microsoft.AspNetCore.Mvc.Api.Analyzers](https://github.com/aspnet/AspNetCore/tree/master/src/Analyzers)
        * [Microsoft.VisualStudio.Threading.Analyzers](https://github.com/microsoft/vs-threading)
    * CodeAnalysisRules [HappyCode.NetCoreBoilerplate.ruleset](HappyCode.NetCoreBoilerplate.ruleset)
    * Code coverage
        * [Coverlet](https://github.com/tonerdo/coverlet)
        * [Codecov](https://codecov.io/)
    * Code analysis with [CodeQL](https://codeql.github.com/)
1. [Docker](https://www.docker.com/)
    * [Dockerfile](dockerfile)
    * [Docker-compose](docker-compose.yml)
        * `mysql:8` with DB initialization
        * `mcr.microsoft.com/mssql/server:2017-latest` with DB initialization
        * `netcore-boilerplate:compose`
1. [Serilog](https://serilog.net/)
    * Sink: [Async](https://github.com/serilog/serilog-sinks-async)
1. [DbUp](http://dbup.github.io/) as a db migration tool
1. Continuous integration
    * ~~[Travis CI]~~(https://travis-ci.org/) ([travisci.yml](https://github.com/lkurzyniec/netcore-boilerplate/blob/bf65154b63f6a10d6753045c49cd378e53907816/.travis.yml))
    * [GitHub Actions](https://github.com/features/actions)
        * [dotnetcore.yml](.github/workflows/dotnetcore.yml)
        * [codeql-analysis.yml](.github/workflows/codeql-analysis.yml)
        * [docker-build.yml](.github/workflows/docker-build.yml)

## Architecture

### Api

[HappyCode.NetCoreBoilerplate.Api](src/HappyCode.NetCoreBoilerplate.Api)

* The entry point of the app - [Program.cs](src/HappyCode.NetCoreBoilerplate.Api/Program.cs)
* Simple Startup class - [Startup.cs](src/HappyCode.NetCoreBoilerplate.Api/Startup.cs)
  * MvcCore
  * DbContext (with MySQL)
  * DbContext (with MsSQL)
  * Swagger and SwaggerUI (Swashbuckle)
  * HostedService and HttpClient
  * FeatureManagement
  * HealthChecks
    * MySQL
    * MsSQL
* Filters
  * Simple `ApiKey` Authorization filter - [ApiKeyAuthorizationFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/ApiKeyAuthorizationFilter.cs)
  * Action filter to validate `ModelState` - [ValidateModelStateFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/ValidateModelStateFilter.cs)
  * Global exception filter - [HttpGlobalExceptionFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/HttpGlobalExceptionFilter.cs)
* Configurations
  * `Serilog` configuration place - [SerilogConfigurator.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Configurations/SerilogConfigurator.cs)
  * `Swagger` registration place - [SwaggerRegistration.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Registrations/SwaggerRegistration.cs)
* Simple exemplary API controllers - [EmployeesController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/EmployeesController.cs), [CarsController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/CarsController.cs), [PingsController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/PingsController.cs)
* Example of BackgroundService - [PingWebsiteBackgroundService.cs](src/HappyCode.NetCoreBoilerplate.Api/BackgroundServices/PingWebsiteBackgroundService.cs)

![HappyCode.NetCoreBoilerplate.Api](.assets/api.png "HappyCode.NetCoreBoilerplate.Api")

### Core

[HappyCode.NetCoreBoilerplate.Core](src/HappyCode.NetCoreBoilerplate.Core)

* Models
  * Dto models
  * DB models
  * AppSettings models - [Settings](src/HappyCode.NetCoreBoilerplate.Core/Settings)
* DbContexts
  * MySQL DbContext - [EmployeesContext.cs](src/HappyCode.NetCoreBoilerplate.Core/EmployeesContext.cs)
  * MsSQL DbContext - [CarsContext.cs](src/HappyCode.NetCoreBoilerplate.Core/CarsContext.cs)
* Core registrations - [CoreRegistrations.cs](src/HappyCode.NetCoreBoilerplate.Core/Registrations/CoreRegistrations.cs)
* Exemplary MySQL repository - [EmployeeRepository.cs](src/HappyCode.NetCoreBoilerplate.Core/Repositories/EmployeeRepository.cs)
* Exemplary MsSQL service - [CarService.cs](src/HappyCode.NetCoreBoilerplate.Core/Services/CarService.cs)

![HappyCode.NetCoreBoilerplate.Core](.assets/core.png "HappyCode.NetCoreBoilerplate.Core")

## DB Migrations

[HappyCode.NetCoreBoilerplate.Db](src/HappyCode.NetCoreBoilerplate.Db)

* Console application as a simple db migration tool - [Program.cs](src/HappyCode.NetCoreBoilerplate.Db/Program.cs)
* Sample migration scripts, both `.sql` and `.cs` - [S001_AddCarTypesTable.sql](src/HappyCode.NetCoreBoilerplate.Db/Scripts/Sql/S001_AddCarTypesTable.sql), [S002_ModifySomeRows.cs](src/HappyCode.NetCoreBoilerplate.Db/Scripts/Code/S002_ModifySomeRows.cs)

![HappyCode.NetCoreBoilerplate.Db](.assets/db.png "HappyCode.NetCoreBoilerplate.Db")

## Tests

### Integration tests

[HappyCode.NetCoreBoilerplate.Api.IntegrationTests](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests)

* Infrastructure
  * Fixture with TestServer - [TestServerClientFixture.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
  * TestStartup with InMemory databases - [TestStartup.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestStartup.cs)
  * Simple data feeders - [EmployeeContextDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/DataFeeders/EmployeeContextDataFeeder.cs), [CarsContextDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/DataFeeders/CarsContextDataFeeder.cs)
* Exemplary tests - [EmployeesTests.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/EmployeesTests.cs), [CarsTests.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/CarsTests.cs)

![HappyCode.NetCoreBoilerplate.Api.IntegrationTests](.assets/itests.png "HappyCode.NetCoreBoilerplate.Api.IntegrationTests")

### Unit tests

[HappyCode.NetCoreBoilerplate.Api.UnitTests](test/HappyCode.NetCoreBoilerplate.Api.UnitTests)

* Exemplary tests - [EmployeesControllerTests.cs](test/HappyCode.NetCoreBoilerplate.Api.UnitTests/Controllers/EmployeesControllerTests.cs)
* Unit tests of `ApiKeyAuthorizationFilter.cs` - [ApiKeyAuthorizationFilterTests.cs](test/HappyCode.NetCoreBoilerplate.Api.UnitTests/Infrastructure/Filters/ApiKeyAuthorizationFilterTests.cs)

[HappyCode.NetCoreBoilerplate.Core.UnitTests](test/HappyCode.NetCoreBoilerplate.Core.UnitTests)

* Extension methods to mock `DbSet` faster - [EnumerableExtensions.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Extensions/EnumerableExtensions.cs)
* Exemplary tests - [EmployeeRepositoryTests.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Repositories/EmployeeRepositoryTests.cs), [CarServiceTests.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Services/CarServiceTests.cs)

![HappyCode.NetCoreBoilerplate.Core.UnitTests](.assets/utests.png "HappyCode.NetCoreBoilerplate.Core.UnitTests")

### Architectural tests

[HappyCode.NetCoreBoilerplate.ArchitecturalTests](test/HappyCode.NetCoreBoilerplate.ArchitecturalTests)

* Exemplary tests - [ApiArchitecturalTests.cs](test/HappyCode.NetCoreBoilerplate.ArchitecturalTests/ApiArchitecturalTests.cs), [CoreArchitecturalTests.cs](test/HappyCode.NetCoreBoilerplate.ArchitecturalTests/CoreArchitecturalTests.cs)

![HappyCode.NetCoreBoilerplate.ArchitecturalTests](.assets/atests.png "HappyCode.NetCoreBoilerplate.ArchitecturalTests")

## Books module

Totally separate module, developed with a modular monolith approach.

### Module

The code organized around features (vertical slices).

[HappyCode.NetCoreBoilerplate.BooksModule](src/HappyCode.NetCoreBoilerplate.BooksModule)

* Features
  * Delete book - [Endpoint.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/DeleteBook/Endpoint.cs), [Command.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/DeleteBook/Command.cs)
  * Get book - [Endpoint.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/GetBook/Endpoint.cs), [Query.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/GetBook/Query.cs)
  * Get books - [Endpoint.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/GetBooks/Endpoint.cs), [Query.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/GetBooks/Query.cs)
  * Upsert book - [Endpoint.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/UpsertBook/Endpoint.cs), [Command.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Features/UpsertBook/Command.cs)
* Sqlite db initializer - [DbInitializer.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/Infrastructure/DbInitializer.cs)
* Module configuration place - [BooksModuleConfigurations.cs](src/HappyCode.NetCoreBoilerplate.BooksModule/BooksModuleConfigurations.cs)

![HappyCode.NetCoreBoilerplate.BooksModule](.assets/books.png "HappyCode.NetCoreBoilerplate.BooksModule")

### Integration Tests

[HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests](test/HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests)

* Infrastructure
  * Fixture with TestServer - [TestServerClientFixture.cs](test/HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
  * Very simple data feeder - [BooksDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests/Infrastructure/DataFeeders/BooksDataFeeder.cs)
* Exemplary tests - [BooksTests.cs](test/HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests/BooksTests.cs)

![HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests](.assets/books-tests.png "HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests")

## How to adapt to your project

Generally it is totally up to you! But in case you do not have any plan, You can follow below simple steps:

1. Download/clone/fork repository :arrow_heading_down:
1. Remove components and/or classes that you do not need to :fire:
1. Rename files (e.g. sln, csproj, ruleset), folders, namespaces etc :memo:
1. Appreciate the work :star:

## Build the solution

Just execute `dotnet build` in the root directory, it takes `HappyCode.NetCoreBoilerplate.sln` and build everything.

## Start the application

> When running on `Linux` (i.e. [WSL](https://learn.microsoft.com/en-us/windows/wsl/install)), make sure that all docker files
([dockerfile](dockerfile), [docker-compose](docker-compose.yml) and all [mssql files](db/mssql)) have line endings `LF`.

Just run `docker-compose up` command in the root directory. After successful build and start of all services visit http://localhost:5000/swagger/.
To check that API has connection to both MySQL and MsSQL databases visit http://localhost:5000/health/.

### Just a container

When running just a container, then features like `cars` and `employees` are disabled.

To do so, execute `docker build . -t netcore-boilerplate:local` in the root directory, and then `docker run --rm -p 5000:8080 --name netcore-boilerplate netcore-boilerplate:local`.
After all, visit http://localhost:5000/swagger/.

### Migrations

When the entire environment is up and running, you can additionally run a migration tool to add some new schema objects into MsSQL DB.
To do that, go to `src/HappyCode.NetCoreBoilerplate.Db` directory and execute `dotnet run` command.

## Run unit tests

Run `dotnet test` command in the root directory, it will look for test projects in `HappyCode.NetCoreBoilerplate.sln` and run them.

## To Do

* any idea? Please create an issue.

## Be like a star, give me a star! :star:

If:

* you like this repo/code,
* you learn something,
* you are using it in your project/application,

then please give me a `star`, appreciate my work. Thanks!

## Buy me a coffee! :coffee:

[![Buy me a coffee](https://cdn.buymeacoffee.com/buttons/lato-blue.png)](https://www.buymeacoffee.com/lkurzyniec)

[FluentAssertions]: https://fluentassertions.com/
[xUnit]: https://xunit.net/
