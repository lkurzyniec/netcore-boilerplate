# netcore-boilerplate

Sample boilerplate of `.NET Core 2.2` application.

[![Build Status](https://travis-ci.com/lkurzyniec/netcore-boilerplate.svg?branch=master)](https://travis-ci.com/lkurzyniec/netcore-boilerplate)

## Source code contains

1. [Autofac](https://autofac.org/)
1. [Swagger](https://swagger.io/) + [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
1. [EF Core](https://docs.microsoft.com/ef/)
    1. [MySQL provider from Oracle](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html)
1. Tests
    1. [Integration tests](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/EmployeesTests.cs)
    1. [Unit tests](test/HappyCode.NetCoreBoilerplate.Api.UnitTests/Controllers/EmployeesControllerTests.cs)
1. Code quality
    1. [editorconfig](.editorconfig)
    1. Analizers ([Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers), [Microsoft.AspNetCore.Mvc.Api.Analyzers](https://github.com/aspnet/AspNetCore/tree/master/src/Analyzers))
    1. [Rules](HappyCode.NetCoreBoilerplate.ruleset)

## Architecture

### Api

[HappyCode.NetCoreBoilerplate.Api](src/HappyCode.NetCoreBoilerplate.Api)

* Simple Startup class - [Startup.cs](src/HappyCode.NetCoreBoilerplate.Api/Startup.cs)
  * MvcCore
  * DbContext (with MySQL)
  * Swagger
  * SwaggerUI (Swashbuckle)
  * HostedService
  * HttpClient
  * HealthCheck
* Very simple exemplary API controller - [EmployeesController.cs](src/HappyCode.NetCoreBoilerplate.Api/Controllers/EmployeesController.cs)
* Example of BackgroundService - [PingWebsiteBackgroundService.cs](src/HappyCode.NetCoreBoilerplate.Api/BackgroundServices/PingWebsiteBackgroundService.cs)

![HappyCode.NetCoreBoilerplate.Api](https://kurzyniec.pl/wp-content/uploads/2019/10/netcore-boilerplate-api.png "HappyCode.NetCoreBoilerplate.Api")

### Core

[HappyCode.NetCoreBoilerplate.Core](src/HappyCode.NetCoreBoilerplate.Core)

* Simple DbContext - [EmployeesContext.cs](src/HappyCode.NetCoreBoilerplate.Core/EmployeesContext.cs)
* Exemplary repository - [EmployeeRepository.cs](src/HappyCode.NetCoreBoilerplate.Core/Repositories/EmployeeRepository.cs)

![HappyCode.NetCoreBoilerplate.Core](https://kurzyniec.pl/wp-content/uploads/2019/10/netcore-boilerplate-core.png "HappyCode.NetCoreBoilerplate.Core")

## Tests

### Integration tests

[HappyCode.NetCoreBoilerplate.Api.IntegrationTests](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests)

* Fixture with TestServer - [TestServerClientFixture.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
* TestStartup with InMemory database - [TestStartup.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/TestStartup.cs)
* Simpe data feeder - [EmployeeContextDataFeeder.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/Infrastructure/EmployeeContextDataFeeder.cs)
* Exemplary tests - [EmployeesTests.cs](test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests/EmployeesTests.cs)

![HappyCode.NetCoreBoilerplate.Api.IntegrationTests](https://kurzyniec.pl/wp-content/uploads/2019/10/netcore-boilerplate-itests.png "HappyCode.NetCoreBoilerplate.Api.IntegrationTests")

### Unit tests

[HappyCode.NetCoreBoilerplate.Api.UnitTests](test/HappyCode.NetCoreBoilerplate.Api.UnitTests)

* Exemplary tests - [EmployeesControllerTests.cs](test/HappyCode.NetCoreBoilerplate.Api.UnitTests/Controllers/EmployeesControllerTests.cs)

[HappyCode.NetCoreBoilerplate.Core.UnitTests](test/HappyCode.NetCoreBoilerplate.Core.UnitTests)

* Some test classes to be able mock DbContext - [TestAsyncEnumerable.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Infrastructure/TestAsyncEnumerable.cs), [TestAsyncEnumerator.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Infrastructure/TestAsyncEnumerator.cs), [TestAsyncQueryProvider.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Infrastructure/TestAsyncQueryProvider.cs)
* Extension method to quickly mock of DbSet - [EnumerableExtensions.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Infrastructure/EnumerableExtensions.cs)
* Exemplary tests - [EmployeeRepositoryTests.cs](test/HappyCode.NetCoreBoilerplate.Core.UnitTests/Repositories/EmployeeRepositoryTests.cs)

![HappyCode.NetCoreBoilerplate.Core.UnitTests](https://kurzyniec.pl/wp-content/uploads/2019/10/netcore-boilerplate-utests.png "HappyCode.NetCoreBoilerplate.Core.UnitTests")

## To Do

* EF Core with MsSQL (Microsoft)
* logging by Serilog
* docker for API
* docker-compose for API and DB
* feature branch with .NET Core 3.0 (IMHO not yet ready for PROD)
* move README info to Wiki, leave here real boilerplate info

## Start application

Application can be started by `dotnet run` command executed in the `/src/HappyCode.NetCoreBoilerplate.Api` directory, by default it will be available under `http://localhost:5000`.

## Run unit tests

Run `dotnet test` command in one of the test directory, ie: `/test/HappyCode.NetCoreBoilerplate.Api.IntegrationTests`.

## Be like a star, give me a star! :star:

If:

* you like this repo/code,
* you learn something,
* you are using it in your project/application,

then please give me a star, appreciate my work. Thanks!
