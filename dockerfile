FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

LABEL org.opencontainers.image.authors="Łukasz Kurzyniec" \
      org.opencontainers.image.title="HappyCode.NetCoreBoilerplate" \
      org.opencontainers.image.description="Simple API written in .NET 8."

# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
ENV \
  DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
  LC_ALL=en_US.UTF-8 \
  LANG=en_US.UTF-8

RUN apk add --upgrade --no-cache \
  icu-data-full \
  icu-libs \
  tzdata \
  curl

# --------------

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS restore
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

RUN cd /work/HappyCode.NetCoreBoilerplate.Api && dotnet restore -r linux-musl-x64

COPY src .

# --------------

FROM restore AS publish
WORKDIR /work/HappyCode.NetCoreBoilerplate.Api

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

RUN dotnet publish -c Release -r linux-musl-x64 \
  -o /app --no-restore

# --------------

FROM base AS final
COPY --from=publish /app .

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

ARG VERSION=2.0.0
ARG SHA=none

ENV HC_SHA=${SHA}
ENV HC_VERSION=${VERSION}

HEALTHCHECK --interval=5m --timeout=3s --start-period=10s --retries=1 \
  CMD curl --fail http://localhost:8080/healthz/live || exit 1

RUN chown "$APP_UID":"$APP_UID" /app
USER $APP_UID
ENTRYPOINT ["dotnet", "HappyCode.NetCoreBoilerplate.Api.dll"]
