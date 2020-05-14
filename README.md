# netcore-boilerplate

Boilerplate of API in `.NET Core 3.1`

| Travis CI     | GitHub        | Codecov       |
|:-------------:|:-------------:|:-------------:|
| [![Travis CI Build Status](https://travis-ci.com/lkurzyniec/netcore-boilerplate.svg?branch=master)](https://travis-ci.com/lkurzyniec/netcore-boilerplate) | [![GitHub Build Status](https://github.com/lkurzyniec/netcore-boilerplate/workflows/Build%20%26%20Test/badge.svg)](https://github.com/lkurzyniec/netcore-boilerplate/actions) | [![codecov](https://codecov.io/gh/lkurzyniec/netcore-boilerplate/branch/master/graph/badge.svg)](https://codecov.io/gh/lkurzyniec/netcore-boilerplate) |

Boilerplate is a piece of code that helps you to quickly kick-off a project or start writing your source code. It is kind of a template - instead
of starting an empty project and adding the same snippets each time, you can use the boilerplate that already contains such code.

## Source code contains

1. [Autofac](https://autofac.org/)
1. [Swagger](https://swagger.io/) + [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
1. [EF Core](https://docs.microsoft.com/ef/)
    * [MySQL provider from Pomelo Foundation](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
    * [MsSQL from Microsoft](https://github.com/aspnet/EntityFrameworkCore/)
1. Tests
    * Integration tests with InMemory database
        * [FluentAssertions]
        * [xUnit]
    * Unit tests
        * [AutoFixture](https://github.com/AutoFixture/AutoFixture)
        * [FluentAssertions]
        * [Moq](https://github.com/moq/moq4)
        * [Moq.AutoMock](https://github.com/moq/Moq.AutoMocker)
        * [xUnit]
    * Load tests
        * [FluentAssertions]
        * [NBomber](https://nbomber.com/)
        * [xUnit]
1. Code quality
    * [EditorConfig](https://editorconfig.org/) ([.editorconfig](.editorconfig))
    * Analizers ([Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers), [Microsoft.AspNetCore.Mvc.Api.Analyzers](https://github.com/aspnet/AspNetCore/tree/master/src/Analyzers))
    * [Rules](HappyCode.NetCoreBoilerplate.ruleset)
    * Code coverage
        * [Coverlet](https://github.com/tonerdo/coverlet)
        * [Codecov](https://codecov.io/)
1. Docker
    * [Dockerfile](dockerfile)
    * [Docker-compose](docker-compose.yml)
        * `mysql:8` with DB initialization
        * `mcr.microsoft.com/mssql/server:2017-latest` with DB initialization
        * `netcore-boilerplate:local`
1. [Serilog](https://serilog.net/)
    * Sink: [Async](https://github.com/serilog/serilog-sinks-async)
    * Enrich: [CorrelationId](https://github.com/ekmsystems/serilog-enrichers-correlation-id)
1. [DbUp](http://dbup.github.io/) as a db migration tool
1. Continuous integration
    * [Travis CI](https://travis-ci.org/) ([.travis.yml](.travis.yml))
    * [GitHub Actions](https://github.com/features/actions) ([dotnetcore.yml](.github/workflows/dotnetcore.yml))

## Architecture

### Api

[HappyCode.NetCoreBoilerplate.Api](src/HappyCode.NetCoreBoilerplate.Api)

* Simple Startup class - [Startup.cs](src/HappyCode.NetCoreBoilerplate.Api/Startup.cs)
  * MvcCore
  * DbContext (with MySQL)
  * DbContext (with MsSQL)
  * Swagger and SwaggerUI (Swashbuckle)
  * HostedService
  * HttpClient
  * HealthCheck
* Filters
  * Simple `ApiKey` Authorization filter - [ApiKeyAuthorizationFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/ApiKeyAuthorizationFilter.cs)
  * Action filter to validate `ModelState` - [ValidateModelStateFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/ValidateModelStateFilter.cs)
  * Global exception filter - [HttpGlobalExceptionFilter.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Filters/HttpGlobalExceptionFilter.cs)
* Configurations
  * Dependency registration place - [ContainerConfigurator.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Configurations/ContainerConfigurator.cs)
  * `Serilog` configuration place - [SerilogConfigurator.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Configurations/SerilogConfigurator.cs)
  * `Swagger` configuration place - [SwaggerConfigurator.cs](src/HappyCode.NetCoreBoilerplate.Api/Infrastructure/Configurations/SwaggerConfigurator.cs)
* Simple exemplary API controllers - [EmployeesController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/EmployeesController.cs), [CarsController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/CarsController.cs)
* Example of BackgroundService - [PingWebsiteBackgroundService.cs](src/HappyCode.NetCoreBoilerplate.Api/BackgroundServices/PingWebsiteBackgroundService.cs)

![HappyCode.NetCoreBoilerplate.Api](https://kurzyniec.pl/wp-content/uploads/2020/04/netcore-boilerplate-api.png "HappyCode.NetCoreBoilerplate.Api")

### Core

[HappyCode.NetCoreBoilerplate.Core](src/HappyCode.NetCoreBoilerplate.Core)

* Simple MySQL DbContext - [EmployeesContext.cs](src/HappyCode.NetCoreBoilerplate.Core/EmployeesContext.cs)
* Simple MsSQL DbContext - [CarsContext.cs](src/HappyCode.NetCoreBoilerplate.Core/CarsContext.cs)
* Exemplary MySQL repository - [EmployeeRepository.cs](src/HappyCode.NetCoreBoilerplate.Core/Repositories/EmployeeRepository.cs)
* Exemplary MsSQL service - [CarService.cs](src/HappyCode.NetCoreBoilerplate.Core/Services/CarService.cs)

![HappyCode.NetCoreBoilerplate.Core](https://kurzyniec.pl/wp-content/uploads/2019/12/netcore-boilerplate-core.png "HappyCode.NetCoreBoilerplate.Core")

## DB Migrations

[HappyCode.NetCoreBoilerplate.Db](src/HappyCode.NetCoreBoilerplate.Db)

* Console application as a simple db migration tool - [Program.cs](src/HappyCode.NetCoreBoilerplate.Db/Program.cs)
* Sample migration scripts, both `.sql` and `.cs` - [S001_AddCarTypesTable.sql](src/HappyCode.NetCoreBoilerplate.Db/Scripts/Sql/S001_AddCarTypesTable.sql), [S002_ModifySomeRows.cs](src/HappyCode.NetCoreBoilerplate.Db/Scripts/Code/S002_ModifySomeRows.cs)

![HappyCode.NetCoreBoilerplate.Db](https://kurzyniec.pl/wp-content/uploads/2019/12/netcore-boilerplate-db.png "HappyCode.NetCoreBoilerplate.Db")

## Tests

### Integration tests

[HappyCode.NetCoreBoilerplate.Api.IntegrationTests](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests)

* Fixture with TestServer - [TestServerClientFixture.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
* TestStartup with InMemory databases - [TestStartup.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestStartup.cs)
* Simple data feeders - [EmployeeContextDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/EmployeeContextDataFeeder.cs), [CarsContextDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/CarsContextDataFeeder.cs)
* Exemplary tests - [EmployeesTests.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/EmployeesTests.cs), [CarsTests.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/CarsTests.cs)

![HappyCode.NetCoreBoilerplate.Api.IntegrationTests](https://kurzyniec.pl/wp-content/uploads/2019/12/netcore-boilerplate-itests.png "HappyCode.NetCoreBoilerplate.Api.IntegrationTests")

### Unit tests

[HappyCode.NetCoreBoilerplate.Api.UnitTests](test/HappyCode.NetCoreBoilerplate.Api.UnitTests)

* Exemplary tests - [EmployeesControllerTests.cs](test/HappyCode.NetCoreBoilerplate.Api.UnitTests/Controllers/EmployeesControllerTests.cs)

[HappyCode.NetCoreBoilerplate.Core.UnitTests](test/HappyCode.NetCoreBoilerplate.Core.UnitTests)

* Extension methods to mock `DbSet` faster - [EnumerableExtensions.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Infrastructure/EnumerableExtensions.cs)
* Exemplary tests - [EmployeeRepositoryTests.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Repositories/EmployeeRepositoryTests.cs), [CarServiceTests.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Services/CarServiceTests.cs)

![HappyCode.NetCoreBoilerplate.Core.UnitTests](https://kurzyniec.pl/wp-content/uploads/2019/12/netcore-boilerplate-utests.png "HappyCode.NetCoreBoilerplate.Core.UnitTests")

### Load tests

> **Keep in mind that entire environment has to be up and running.**

[HappyCode.NetCoreBoilerplate.Api.LoadTests](test/HappyCode.NetCoreBoilerplate.Api.LoadTests)

* Base class for controller - [LoadTestsBase.cs](test/HappyCode.NetCoreBoilerplate.Api.LoadTests/Controllers/LoadTestsBase.cs)
* Exemplary tests - [EmployeesControllerTests.cs](test/HappyCode.NetCoreBoilerplate.Api.LoadTests/Controllers/EmployeesControllerTests.cs), [CarsControllerTests.cs](test/HappyCode.NetCoreBoilerplate.Api.LoadTests/Controllers/CarsControllerTests.cs)

![HappyCode.NetCoreBoilerplate.Api.LoadTests](https://kurzyniec.pl/wp-content/uploads/2020/05/netcore-boilerplate-ltests.png "HappyCode.NetCoreBoilerplate.Api.LoadTests")

## How to adapt to your project

Generally it is totally up to you! But in case you do not have any plan, You can follow below simple steps:

1. Download/clone/fork repository
1. Remove components and/or classes that you do not need to
1. Rename files (e.g. sln, csproj, ruleset), folders, namespaces etc.
1. Give us a star!

## Build the solution

Just execute `dotnet build` in the root directory, it takes `HappyCode.NetCoreBoilerplate.sln` and build everything.

## Start the application

### Standalone

At first, you need to have up and running [MySQL](https://www.mysql.com/downloads/) and [MsSQL](https://www.microsoft.com/sql-server/sql-server-downloads) database servers on localhost with initialized
database by [mysql script](db/mysql/mysql-employees.sql) and [mssql script](db/mssql/mssql-cars.sql).

Then the application (API) can be started by `dotnet run` command executed in the `src/HappyCode.NetCoreBoilerplate.Api` directory.
By default it will be available under http://localhost:5000/, but keep in mind that documentation is available under
http://localhost:5000/swagger/.

### Docker (recommended)

Just run `docker-compose up` command in the root directory and after successful start of services visit http://localhost:5000/swagger/.

### Migrations

When the entire environment is up and running, you can additionally run a migration tool to add some new schema objects into MsSQL DB. To do that, go to `src/HappyCode.NetCoreBoilerplate.Db` directory and execute `dotnet run` command.

## Run unit tests

Run `dotnet test` command in the root directory, it will look for test projects in `HappyCode.NetCoreBoilerplate.sln` and run them.

## Migrate from ASP .NET Core 2.2 to 3.1

Need to migrate from `.NET Core 2.2` to `.NET Core 3.1`? There's an [Microsoft article](https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio) about this, but you can also, just in case, take a look at my [migration commit](https://github.com/lkurzyniec/netcore-boilerplate/commit/45764d631bce10b0d4d8db47f786ad696fa65d67) where you can find the complete list of required changes.

## To Do

* any idea? Please create an issue.

## Be like a star, give me a star! :star:

If:

* you like this repo/code,
* you learn something,
* you are using it in your project/application,

then please give us a star, appreciate our work. Thanks!

## Contribution

You are very welcome to submit either issues or pull requests to this repository!

For pull request please follow this rules:

* Commit messages should be clear and as much as possible descriptive.
* Rebase if required.
* Make sure that your code compile and run locally.
* Changes do not break any tests and code quality rules.

[FluentAssertions]: https://fluentassertions.com/
[xUnit]: https://xunit.net/
