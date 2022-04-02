FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /work

COPY src/*/*.csproj ./
RUN for projectFile in $(ls *.csproj); \
do \
  mkdir -p ${projectFile%.*}/ && mv $projectFile ${projectFile%.*}/; \
done

RUN dotnet restore /work/HappyCode.NetCoreBoilerplate.Api/HappyCode.NetCoreBoilerplate.Api.csproj

COPY src .

FROM build AS publish
WORKDIR /work/HappyCode.NetCoreBoilerplate.Api
RUN dotnet publish -c Release -o /app --no-restore

LABEL maintainer="Lukasz Kurzyniec (lkurzyniec@gmail.com)"

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app .

HEALTHCHECK --interval=5m --timeout=3s --start-period=10s --retries=1 \
  CMD curl --fail http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "HappyCode.NetCoreBoilerplate.Api.dll"]
