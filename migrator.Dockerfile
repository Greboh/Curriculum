FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Curriculum.Migrator/Curriculum.Migrator.csproj Curriculum.Migrator/
COPY Curriculum.Infrastructure/Curriculum.Infrastructure.csproj Curriculum.Infrastructure/
COPY Curriculum.Core/Curriculum.Core.csproj Curriculum.Core/

RUN dotnet restore Curriculum.Migrator/Curriculum.Migrator.csproj

COPY Curriculum.Migrator/ Curriculum.Migrator/
COPY Curriculum.Infrastructure/ Curriculum.Infrastructure/
COPY Curriculum.Core/ Curriculum.Core/

RUN dotnet publish Curriculum.Migrator/Curriculum.Migrator.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

## Runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Curriculum.Migrator.dll"]