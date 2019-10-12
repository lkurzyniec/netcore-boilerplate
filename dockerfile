FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /work
COPY src .

FROM build AS publish
WORKDIR /work/HappyCode.NetCoreBoilerplate.Api
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HappyCode.NetCoreBoilerplate.Api.dll"]
