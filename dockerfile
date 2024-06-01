FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

LABEL org.opencontainers.image.authors="Łukasz Kurzyniec" \
      org.opencontainers.image.title="HappyCode.NetCoreBoilerplate" \
      org.opencontainers.image.description="Simple API written in .NET 8."

# --------------

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /work

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

COPY ./Directory.Build.props ./
COPY ./Directory.Packages.props ./
COPY src/*/*.csproj ./
RUN for projectFile in $(ls *.csproj); \
  do \
  mkdir -p ${projectFile%.*}/ && mv $projectFile ${projectFile%.*}/; \
  done

RUN dotnet restore /work/HappyCode.NetCoreBoilerplate.Api/HappyCode.NetCoreBoilerplate.Api.csproj

COPY src .

# --------------

FROM build AS publish
WORKDIR /work/HappyCode.NetCoreBoilerplate.Api

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

RUN dotnet publish -c Release -o /app --no-restore

# --------------

FROM base AS final
COPY --from=publish /app .

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

HEALTHCHECK --interval=5m --timeout=3s --start-period=10s --retries=1 \
  CMD curl --fail http://localhost:8080/healthz/live || exit 1

ENTRYPOINT ["dotnet", "HappyCode.NetCoreBoilerplate.Api.dll"]
